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

            // Register encoding provider for ExcelDataReader compatibility
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

        #region 1. Excel Upload (Populates dgvOriginal)
        private void btnUploadExcel_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Excel & CSV Files (*.xlsx;*.xls;*.csv)|*.xlsx;*.xls;*.csv|All Files (*.*)|*.*",
                Title = "Select Excel Student List"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                btnUploadExcel.Enabled = false;
                btnUploadExcel.Text = "Reading...";
                dgvOriginal.Rows.Clear();
                dgvGenerated.Rows.Clear();

                int importedCount = 0;

                using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    if (result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("The Excel file has no records.", "Empty File", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DataTable dt = result.Tables[0];
                    bool isFirstRow = true;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.ItemArray.Length < 2) continue;

                        string col0 = row[0]?.ToString()?.Trim() ?? ""; // ID / Code / Col 0
                        string col1 = row[1]?.ToString()?.Trim() ?? ""; // Full English Name / Col 1

                        // Check if first row is a header
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            if (col1.Contains("name", StringComparison.OrdinalIgnoreCase) ||
                                col0.Contains("id", StringComparison.OrdinalIgnoreCase) ||
                                col1.Contains("english", StringComparison.OrdinalIgnoreCase))
                            {
                                continue; // Skip header
                            }
                        }

                        // Skip rows where the full name column is blank
                        if (string.IsNullOrWhiteSpace(col1))
                            continue;

                        importedCount++;
                        dgvOriginal.Rows.Add(
                            true,          // colOrigSelect (checked by default)
                            importedCount, // colOrigIndex
                            col0,          // colOrigId (Column 0)
                            col1,          // colOrigFullName (Column 1)
                            "Ready"        // colOrigStatus
                        );
                    }
                }

                chkSelectAll.Checked = true;
                UpdateSelectionSummary();
                btnCreateEmails.Enabled = importedCount > 0;
                btnSaveToDb.Enabled = false;

                // Insert log into SystemLogs
                DbHelper.LogAction(
                    UserSession.CurrentUsername,
                    $"{UserSession.CurrentUsername} uploaded an Excel file with {importedCount} records for {_targetTable}"
                );

                MessageBox.Show(
                    $"Imported {importedCount} students into 'Original Data' (Left Panel).\nSelect the desired rows and click 'Create Emails' to generate addresses into the Right Panel.",
                    "Upload Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read Excel file:\n{ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUploadExcel.Enabled = true;
                btnUploadExcel.Text = "📁 Upload Excel";
            }
        }
        #endregion

        #region 2. Email Generation Logic (Reads dgvOriginal -> Writes dgvGenerated)
        private void btnCreateEmails_Click(object sender, EventArgs e)
        {
            if (dgvOriginal.Rows.Count == 0)
            {
                MessageBox.Show("Please upload an Excel file first.", "No Data", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _emailService.EmailDomain = txtDomain.Text.Trim();
            _emailService.ResetCache(); // Reset in-memory sequence cache for fresh batch
            dgvGenerated.Rows.Clear();  // Clear previous generated data

            int generatedCount = 0;
            btnCreateEmails.Enabled = false;
            btnCreateEmails.Text = "Generating...";
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = dgvOriginal.Rows.Count;

            try
            {
                using var conn = DbHelper.GetConnection();
                conn.Open();

                // Loop ONLY through checked rows in dgvOriginal
                foreach (DataGridViewRow rawRow in dgvOriginal.Rows)
                {
                    progressBar.Value++;

                    bool isChecked = Convert.ToBoolean(rawRow.Cells["colOrigSelect"].Value ?? false);
                    if (!isChecked)
                        continue;

                    string id = rawRow.Cells["colOrigId"].Value?.ToString() ?? "";
                    string fullName = rawRow.Cells["colOrigFullName"].Value?.ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(fullName))
                        continue;

                    // Parse full name:
                    // 1. Split string by spaces (e.g., "Ganna Reda Ibrahim Atiya")
                    // 2. Extract 1st letter of 1st name + 1st letter of 2nd name in lowercase ("gr")
                    // 3. Query DB for max sequence for "gr%". Starts at gr1111 or increments in memory
                    string email = _emailService.GenerateEmailFromFullName(
                        fullName,
                        _targetTable,
                        conn,
                        out string firstName,
                        out string lastName
                    );

                    generatedCount++;

                    // Add generated result to dgvGenerated (Right Panel)
                    int genIndex = dgvGenerated.Rows.Add(
                        generatedCount,
                        id,
                        fullName,
                        firstName,
                        lastName,
                        email,
                        "Generated"
                    );

                    // Style generated row with light green background
                    dgvGenerated.Rows[genIndex].DefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);

                    // Update status in original grid
                    rawRow.Cells["colOrigStatus"].Value = "Processed";
                    rawRow.DefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
                }

                // Log generation to SystemLogs
                DbHelper.LogAction(
                    UserSession.CurrentUsername,
                    $"{UserSession.CurrentUsername} generated {generatedCount} emails for {_targetTable}"
                );

                btnSaveToDb.Enabled = generatedCount > 0;
                lblGenCount.Text = $"({generatedCount} generated)";
                lblSelectionStats.Text = $"Generated {generatedCount} sequential emails in Right Panel. Ready to save.";

                MessageBox.Show(
                    $"Successfully generated {generatedCount} sequential emails into 'dgvGenerated'!\nAll duplicate initials in this batch were incremented sequentially in memory without collisions.",
                    "Emails Generated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating emails:\n{ex.Message}", "Generation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
                btnCreateEmails.Enabled = true;
                btnCreateEmails.Text = "⚡ Create Emails →";
            }
        }
        #endregion

        #region 3. Save to Database (Reads ONLY from dgvGenerated)
        private void btnSaveToDb_Click(object sender, EventArgs e)
        {
            if (dgvGenerated.Rows.Count == 0)
            {
                MessageBox.Show("There are no generated records to save.", "Notice", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to save the {dgvGenerated.Rows.Count} generated records to table '{_targetTable}'?",
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
            progressBar.Maximum = dgvGenerated.Rows.Count;

            int savedCount = 0;

            try
            {
                using var conn = DbHelper.GetConnection();
                conn.Open();

                string validatedTable = (_targetTable.Equals("PostGradMails", StringComparison.OrdinalIgnoreCase))
                    ? "PostGradMails"
                    : "mails";

                using var trans = conn.BeginTransaction();
                try
                {
                    // Save to target SQL Server table
                    string insertSql = $@"
                        INSERT INTO dbo.[{validatedTable}] 
                            (first_name, last_name, mail, id, create_date, user_name) 
                        VALUES 
                            (@first_name, @last_name, @mail, @id, CONVERT(NVARCHAR(50), GETDATE(), 120), @user_name)";

                    using var cmd = new SqlCommand(insertSql, conn, trans);
                    var pFirstName = cmd.Parameters.Add("@first_name", SqlDbType.NVarChar, 100);
                    var pLastName = cmd.Parameters.Add("@last_name", SqlDbType.NVarChar, 100);
                    var pMail = cmd.Parameters.Add("@mail", SqlDbType.NVarChar, 150);
                    var pId = cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@user_name", SqlDbType.NVarChar, 100).Value = UserSession.CurrentUsername;

                    // Read ONLY from dgvGenerated
                    foreach (DataGridViewRow genRow in dgvGenerated.Rows)
                    {
                        progressBar.Value++;

                        string status = genRow.Cells["colGenStatus"].Value?.ToString() ?? "";
                        string email = genRow.Cells["colGenEmail"].Value?.ToString() ?? "";

                        // Save only records that haven't been saved yet
                        if (!string.IsNullOrWhiteSpace(email) && status != "Saved in DB")
                        {
                            pFirstName.Value = genRow.Cells["colGenFirstName"].Value?.ToString() ?? "";
                            pLastName.Value = genRow.Cells["colGenLastName"].Value?.ToString() ?? "";
                            pMail.Value = email;
                            pId.Value = genRow.Cells["colGenId"].Value?.ToString() ?? "";

                            cmd.ExecuteNonQuery();

                            genRow.Cells["colGenStatus"].Value = "Saved in DB";
                            genRow.DefaultCellStyle.BackColor = Color.FromArgb(236, 253, 245); // Darker green
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

                // Insert log into SystemLogs
                DbHelper.LogAction(
                    UserSession.CurrentUsername,
                    $"{UserSession.CurrentUsername} saved {savedCount} records to {_targetTable}"
                );

                btnSaveToDb.Enabled = false;
                lblSelectionStats.Text = $"Saved {savedCount} records to {_targetTable} successfully.";

                MessageBox.Show(
                    $"Successfully saved {savedCount} records to table '{_targetTable}' and logged to SystemLogs.",
                    "Save Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving records to database:\n{ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSaveToDb.Enabled = true;
            }
            finally
            {
                progressBar.Visible = false;
                btnSaveToDb.Text = "💾 Save to Database";
            }
        }
        #endregion

        #region Helper Events
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool check = chkSelectAll.Checked;
            foreach (DataGridViewRow row in dgvOriginal.Rows)
            {
                row.Cells["colOrigSelect"].Value = check;
            }
            UpdateSelectionSummary();
        }

        private void dgvOriginal_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colOrigSelect.Index)
            {
                UpdateSelectionSummary();
            }
        }

        private void dgvOriginal_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOriginal.IsCurrentCellDirty && dgvOriginal.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvOriginal.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void UpdateSelectionSummary()
        {
            int total = dgvOriginal.Rows.Count;
            int selected = 0;

            foreach (DataGridViewRow row in dgvOriginal.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colOrigSelect"].Value ?? false))
                    selected++;
            }

            lblRawCount.Text = $"({total} uploaded | {selected} sel)";
            lblSelectionStats.Text = $"Original Rows: {total} | Selected for generation: {selected}";
            btnCreateEmails.Enabled = selected > 0;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
