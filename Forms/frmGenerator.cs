using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using StudentEmailApp.Models;
using StudentEmailApp.Services;

namespace StudentEmailApp.Forms
{
    public partial class frmGenerator : Form
    {
        private readonly string _targetTable;
        private readonly string _targetTitle;
        private readonly EmailGeneratorService _emailService = new();

        public frmGenerator(string targetTable, string targetTitle)
        {
            InitializeComponent();

            // Register encoding provider required for ExcelDataReader to read all Excel formats
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _targetTable = string.IsNullOrWhiteSpace(targetTable) ? "mails" : targetTable;
            _targetTitle = string.IsNullOrWhiteSpace(targetTitle) ? "Students" : targetTitle;
        }

        private void frmGenerator_Load(object sender, EventArgs e)
        {
            lblTargetTitle.Text = $"Email Generator — {_targetTitle}";
            lblTargetTableInfo.Text = $"Target Database Table: dbo.[{_targetTable}]";
            UpdateSelectionSummary();
        }

        #region 1. Excel Upload Logic
        private void btnUploadExcel_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*",
                Title = "Select Excel Student List"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                btnUploadExcel.Enabled = false;
                btnUploadExcel.Text = "Reading...";
                dgvStudents.Rows.Clear();

                int importedCount = 0;

                using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    if (result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("The selected Excel file is empty.", "Empty File", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DataTable dt = result.Tables[0];
                    bool isFirstRow = true;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.ItemArray.Length < 2)
                            continue;

                        string col1 = row[0]?.ToString()?.Trim() ?? "";
                        string col2 = row[1]?.ToString()?.Trim() ?? "";

                        // Check if the first row is a header (e.g. "First Name", "Last Name")
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            if (col1.Contains("first", StringComparison.OrdinalIgnoreCase) ||
                                col1.Contains("name", StringComparison.OrdinalIgnoreCase) ||
                                col2.Contains("last", StringComparison.OrdinalIgnoreCase))
                            {
                                // Skip header row
                                continue;
                            }
                        }

                        // Skip rows where both columns are blank
                        if (string.IsNullOrWhiteSpace(col1) && string.IsNullOrWhiteSpace(col2))
                            continue;

                        importedCount++;
                        int rowIndex = dgvStudents.Rows.Add(
                            true,                   // colSelect (checked by default)
                            importedCount,          // colIndex
                            col1,                   // colFirstName
                            col2,                   // colLastName
                            "",                     // colGeneratedEmail
                            "Ready"                 // colStatus
                        );
                    }
                }

                // Update summary count and UI state
                chkSelectAll.Checked = true;
                UpdateSelectionSummary();
                btnCreateEmails.Enabled = importedCount > 0;
                btnSaveToDb.Enabled = false;

                // Log the action to SystemLogs
                DbHelper.LogAction(
                    UserSession.CurrentUsername,
                    $"{UserSession.CurrentUsername} uploaded an Excel file with {importedCount} records for {_targetTable}"
                );

                MessageBox.Show(
                    $"Successfully loaded {importedCount} student records from Excel.\nClick 'Create Emails' to generate sequential emails.",
                    "Upload Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to read Excel file:\n{ex.Message}",
                    "Excel Read Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnUploadExcel.Enabled = true;
                btnUploadExcel.Text = "📁 Upload Excel";
            }
        }
        #endregion

        #region 2. Email Generation Logic (Crucial Auto-Increment)
        private void btnCreateEmails_Click(object sender, EventArgs e)
        {
            if (dgvStudents.Rows.Count == 0)
            {
                MessageBox.Show("Please upload student data from Excel first.", "No Data", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Set optional email domain (e.g., "@univ.edu" or blank)
            _emailService.EmailDomain = txtDomain.Text.Trim();
            // Reset in-memory cache for a fresh generation batch
            _emailService.ResetCache();

            int checkedRows = 0;
            int generatedCount = 0;

            btnCreateEmails.Enabled = false;
            btnCreateEmails.Text = "Generating...";
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = dgvStudents.Rows.Count;

            try
            {
                using var conn = DbHelper.GetConnection();
                conn.Open();

                // Loop ONLY through the checked rows in the DataGridView
                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    progressBar.Value++;

                    bool isChecked = Convert.ToBoolean(row.Cells["colSelect"].Value ?? false);
                    if (!isChecked)
                        continue;

                    checkedRows++;

                    string firstName = row.Cells["colFirstName"].Value?.ToString() ?? "";
                    string lastName = row.Cells["colLastName"].Value?.ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
                        continue;

                    // Generate the email using our memory-tracked service
                    // Handles:
                    // - 1st letter of First Name + 1st letter of Last Name (lowercase, e.g. 'a' + 'm' = "am")
                    // - Queries target table on first encounter for highest sequence (e.g., 1115)
                    // - Starts at 1111 if no record is found in DB
                    // - In-memory sequential increments (am1116, am1117) for duplicates in the same batch
                    string email = _emailService.GenerateEmail(firstName, lastName, _targetTable, conn);

                    row.Cells["colGeneratedEmail"].Value = email;
                    row.Cells["colStatus"].Value = "Generated";
                    row.DefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244); // light green accent

                    generatedCount++;
                }

                // Log email generation action to SystemLogs
                DbHelper.LogAction(
                    UserSession.CurrentUsername,
                    $"{UserSession.CurrentUsername} generated {generatedCount} emails for {_targetTable}"
                );

                btnSaveToDb.Enabled = generatedCount > 0;
                lblSelectionStats.Text = $"Generated {generatedCount} sequential emails. Ready to save.";

                MessageBox.Show(
                    $"Successfully generated {generatedCount} sequential email addresses.\nAll duplicate initials in this batch were incremented sequentially in memory.",
                    "Emails Generated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error generating emails:\n{ex.Message}",
                    "Generation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                progressBar.Visible = false;
                btnCreateEmails.Enabled = true;
                btnCreateEmails.Text = "⚡ Create Emails";
            }
        }
        #endregion

        #region 3. Save to Database Logic
        private void btnSaveToDb_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                $"Are you sure you want to save the generated emails to the '{_targetTable}' database table?",
                "Confirm Save",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            btnSaveToDb.Enabled = false;
            btnSaveToDb.Text = "Saving...";
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = dgvStudents.Rows.Count;

            int savedCount = 0;

            try
            {
                using var conn = DbHelper.GetConnection();
                conn.Open();

                // Validate target table name to protect query integrity
                string validatedTable = (_targetTable.Equals("PostGradMails", StringComparison.OrdinalIgnoreCase))
                    ? "PostGradMails"
                    : "mails";

                // Use transaction to ensure all-or-nothing data integrity
                using var trans = conn.BeginTransaction();
                try
                {
                    string insertSql = $@"
                        INSERT INTO dbo.[{validatedTable}] (first_name, last_name, mail, create_date, user_name) 
                        VALUES (@first_name, @last_name, @mail, CONVERT(NVARCHAR(50), GETDATE(), 120), @user_name)";

                    using var cmd = new SqlCommand(insertSql, conn, trans);
                    var pFirstName = cmd.Parameters.Add("@first_name", SqlDbType.NVarChar, 100);
                    var pLastName = cmd.Parameters.Add("@last_name", SqlDbType.NVarChar, 100);
                    var pMail = cmd.Parameters.Add("@mail", SqlDbType.NVarChar, 150);
                    cmd.Parameters.Add("@user_name", SqlDbType.NVarChar, 100).Value = UserSession.CurrentUsername;

                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        progressBar.Value++;

                        bool isChecked = Convert.ToBoolean(row.Cells["colSelect"].Value ?? false);
                        string status = row.Cells["colStatus"].Value?.ToString() ?? "";
                        string email = row.Cells["colGeneratedEmail"].Value?.ToString() ?? "";

                        // Save only checked rows with generated emails that are not already saved
                        if (isChecked && !string.IsNullOrWhiteSpace(email) && status != "Saved in DB")
                        {
                            pFirstName.Value = row.Cells["colFirstName"].Value?.ToString() ?? "";
                            pLastName.Value = row.Cells["colLastName"].Value?.ToString() ?? "";
                            pMail.Value = email;

                            cmd.ExecuteNonQuery();

                            row.Cells["colStatus"].Value = "Saved in DB";
                            row.Cells["colSelect"].Value = false; // uncheck after save
                            row.DefaultCellStyle.BackColor = Color.FromArgb(236, 253, 245);
                            savedCount++;
                        }
                    }

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }

                // Log the save action into SystemLogs
                DbHelper.LogAction(
                    UserSession.CurrentUsername,
                    $"{UserSession.CurrentUsername} saved {savedCount} records to {_targetTable}"
                );

                lblSelectionStats.Text = $"Saved {savedCount} records to {_targetTable} successfully.";
                btnSaveToDb.Enabled = false;

                MessageBox.Show(
                    $"Successfully saved {savedCount} records to table '{_targetTable}' and logged the transaction.",
                    "Save Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error saving records to database:\n{ex.Message}",
                    "Database Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                btnSaveToDb.Enabled = true;
            }
            finally
            {
                progressBar.Visible = false;
                btnSaveToDb.Text = "💾 Save to Database";
                UpdateSelectionSummary();
            }
        }
        #endregion

        #region Helper Methods & UI Events
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool check = chkSelectAll.Checked;
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                // Don't re-check rows that were already saved
                string status = row.Cells["colStatus"].Value?.ToString() ?? "";
                if (status != "Saved in DB")
                {
                    row.Cells["colSelect"].Value = check;
                }
            }
            UpdateSelectionSummary();
        }

        private void dgvStudents_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colSelect.Index)
            {
                UpdateSelectionSummary();
            }
        }

        private void dgvStudents_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Commit checkbox state immediately when clicked without waiting for lost focus
            if (dgvStudents.IsCurrentCellDirty && dgvStudents.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvStudents.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void UpdateSelectionSummary()
        {
            int total = dgvStudents.Rows.Count;
            int selected = 0;
            int generated = 0;

            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSelect"].Value ?? false))
                    selected++;

                string email = row.Cells["colGeneratedEmail"].Value?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(email))
                    generated++;
            }

            lblStudentCount.Text = $"Uploaded: {total} students ({selected} selected)";
            lblSelectionStats.Text = $"Total Rows: {total} | Checked: {selected} | Generated Emails: {generated}";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
