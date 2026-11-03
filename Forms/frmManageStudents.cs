using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using StudentEmailApp.Models;
using StudentEmailApp.Services;

namespace StudentEmailApp.Forms
{
    public partial class frmManageStudents : Form
    {
        private readonly string _username;
        private string _validatedTable;
        private readonly bool _canSwitchTable;

        // Master Layout & UI Controls
        private TableLayoutPanel tlpMainLayout = null!;
        private Panel pnlTopHeader = null!;
        private Label lblTitle = null!;
        private ComboBox cmbTableSelector = null!;
        private Button btnBack = null!;

        private GroupBox grpSearch = null!;
        private Label lblSearchPrompt = null!;
        private TextBox txtSearch = null!;
        private Button btnSearch = null!;
        private Label lblResultsCount = null!;

        private GroupBox grpGrid = null!;
        private DataGridView dgvStudents = null!;

        private GroupBox grpEditPanel = null!;
        private TextBox txtNationalId = null!; // ReadOnly = true, Enabled = false (LOCKED)
        private TextBox txtEmail = null!;      // ReadOnly = true, Enabled = false (LOCKED)
        private TextBox txtArName = null!;
        private TextBox txtFirstName = null!;
        private TextBox txtLastName = null!;
        private TextBox txtCollege = null!;
        private TextBox txtDept = null!;
        private TextBox txtCode = null!;
        private TextBox txtStage = null!;
        private TextBox txtType = null!;
        private TextBox txtStat = null!;
        private Button btnSaveChanges = null!;

        public frmManageStudents(string username, string tableName)
        {
            InitializeComponent();

            _username = string.IsNullOrWhiteSpace(username)
                ? UserSession.CurrentUsername
                : username.Trim();

            _validatedTable = (!string.IsNullOrWhiteSpace(tableName) &&
                               tableName.Contains("PostGradMails", StringComparison.OrdinalIgnoreCase))
                ? "PostGradMails"
                : "mails";

            _canSwitchTable = UserSession.CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            BuildManageStudentsUi();
        }

        private void BuildManageStudentsUi()
        {
            SuspendLayout();

            this.Text = "إدارة وتعديل بيانات الطلاب - Search & Edit Students";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Normal;
            this.Dock = DockStyle.Fill;   // fill the SPA content area
            this.AutoScroll = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = Size.Empty;
            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = false;
            this.DoubleBuffered = true;

            // =========================================================================
            // MASTER 4-ROW RESPONSIVE TABLELAYOUTPANEL (Prevents any cut-off or overlap)
            // =========================================================================
            tlpMainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(12),
                BackColor = Color.FromArgb(244, 246, 249)
            };
            tlpMainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));  // Row 0: Header Bar
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));  // Row 1: Search Section
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Row 2: Responsive DataGridView
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 320F)); // Row 3: Spaced Edit Panel

            // =========================================================================
            // 1. TOP HEADER BAR
            // =========================================================================
            pnlTopHeader = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#1E293B"), // Dark Slate/Navy matching SaaS Sidebar
                Padding = new Padding(16, 8, 16, 8),
                Margin = new Padding(0, 0, 0, 8)
            };

            lblTitle = new Label
            {
                Text = $"🔍 إدارة وتعديل بيانات الطلاب — الجدول الحالي: dbo.{_validatedTable}",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Dock = DockStyle.Right
            };

            btnBack = new Button
            {
                Text = "← إغلاق / رجوع",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#334155"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 34),
                Dock = DockStyle.Left,
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) => this.Close();

            cmbTableSelector = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Width = 250,
                Location = new Point(160, 10),
                Visible = _canSwitchTable
            };
            cmbTableSelector.Items.Add("dbo.mails (Undergraduates)");
            cmbTableSelector.Items.Add("dbo.PostGradMails (Postgraduates)");
            cmbTableSelector.SelectedIndex = _validatedTable.Equals("PostGradMails", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
            cmbTableSelector.SelectedIndexChanged += (s, e) =>
            {
                _validatedTable = cmbTableSelector.SelectedIndex == 1 ? "PostGradMails" : "mails";
                lblTitle.Text = $"🔍 إدارة وتعديل بيانات الطلاب — الجدول الحالي: dbo.{_validatedTable}";
                ClearEditFields();
                ExecuteSearch();
            };

            pnlTopHeader.Controls.Add(cmbTableSelector);
            pnlTopHeader.Controls.Add(lblTitle);
            pnlTopHeader.Controls.Add(btnBack);

            // =========================================================================
            // 2. TOP SECTION: SEARCH BAR
            // =========================================================================
            grpSearch = new GroupBox
            {
                Text = "البحث عن طالب (بالرقم القومي أو الاسم بالعربية أو الإيميل الجامعي)",
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 101, 192),
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(14, 8, 14, 8),
                Margin = new Padding(0, 0, 0, 8)
            };

            var pnlSearchRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1
            };
            pnlSearchRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            pnlSearchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlSearchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            pnlSearchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));

            lblSearchPrompt = new Label
            {
                Text = "كلمة البحث:",
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Anchor = AnchorStyles.Right,
                Margin = new Padding(8, 0, 8, 0)
            };

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11.5F, FontStyle.Regular),
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                PlaceholderText = "ابحث بالرقم القومي (id)، أو الاسم باللغة العربية (ar_name)، أو الإيميل الجامعي (mail)...",
                Margin = new Padding(8, 6, 8, 6)
            };
            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    btnSearch.PerformClick();
                }
            };

            btnSearch = new Button
            {
                Text = "🔍 بحث",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(21, 101, 192),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand,
                Margin = new Padding(8, 4, 8, 4)
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += btnSearch_Click;

            lblResultsCount = new Label
            {
                Text = "عدد النتائج: 0",
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 101, 192),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlSearchRow.Controls.Add(lblSearchPrompt, 0, 0);
            pnlSearchRow.Controls.Add(txtSearch, 1, 0);
            pnlSearchRow.Controls.Add(btnSearch, 2, 0);
            pnlSearchRow.Controls.Add(lblResultsCount, 3, 0);
            grpSearch.Controls.Add(pnlSearchRow);

            // =========================================================================
            // 3. MIDDLE SECTION: RESPONSIVE RTL DATAGRIDVIEW
            // =========================================================================
            grpGrid = new GroupBox
            {
                Text = "نتائج البحث (اضغط على أي صف لتحميل بيانات الطالب في لوحة التعديل بالأسفل)",
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 101, 192),
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 8)
            };

            dgvStudents = new DataGridView
            {
                Dock = DockStyle.Fill,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                RightToLeft = RightToLeft.Yes,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ScrollBars = ScrollBars.Both,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 40,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                RowTemplate = { Height = 34 }
            };
            dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#1E293B");
            dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dgvStudents.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStudents.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            dgvStudents.DefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
            dgvStudents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvStudents.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dgvStudents.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            dgvStudents.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvStudents.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvStudents.CellClick += dgvStudents_CellClick;

            grpGrid.Controls.Add(dgvStudents);

            // =========================================================================
            // 4. BOTTOM SECTION: SPACED & ORGANIZED EDIT PANEL (TableLayoutPanel)
            // =========================================================================
            grpEditPanel = new GroupBox
            {
                Text = "تعديل بيانات الطالب المحدد (الرقم القومي والإيميل الجامعي محميان ضد التعديل)",
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 101, 192),
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 12, 16, 12),
                Margin = new Padding(0)
            };

            var tlpEdit = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 4,
                Padding = new Padding(6)
            };
            for (int c = 0; c < 4; c++)
            {
                tlpEdit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }
            tlpEdit.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F)); // Row 0: 4 fields
            tlpEdit.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F)); // Row 1: 4 fields
            tlpEdit.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F)); // Row 2: 3 fields
            tlpEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));   // Row 3: Save Button

            // Row 0: Locked National ID, Locked Email, Arabic Name, Student Code
            txtNationalId = CreateSpacedFieldCell(tlpEdit, 0, 0, "🔒 الرقم القومي (id) [ثابت]:", isLocked: true);
            txtEmail      = CreateSpacedFieldCell(tlpEdit, 1, 0, "🔒 الإيميل الجامعي (mail) [ثابت]:", isLocked: true);
            txtArName     = CreateSpacedFieldCell(tlpEdit, 2, 0, "الاسم باللغة العربية (ar_name):", isLocked: false);
            txtCode       = CreateSpacedFieldCell(tlpEdit, 3, 0, "الكود (code):", isLocked: false);

            // Row 1: First Name, Last Name, College, Department
            txtFirstName  = CreateSpacedFieldCell(tlpEdit, 0, 1, "الاسم الأول بالإنجليزية (first_name):", isLocked: false);
            txtLastName   = CreateSpacedFieldCell(tlpEdit, 1, 1, "باقي الاسم بالإنجليزية (last_name):", isLocked: false);
            txtCollege    = CreateSpacedFieldCell(tlpEdit, 2, 1, "الكلية (college):", isLocked: false);
            txtDept       = CreateSpacedFieldCell(tlpEdit, 3, 1, "الشعبة / القسم (dept):", isLocked: false);

            // Row 2: Stage, Study Type, Status (with 2-column span on Status for clean balance)
            txtStage      = CreateSpacedFieldCell(tlpEdit, 0, 2, "الفرقة الدراسية (stage):", isLocked: false);
            txtType       = CreateSpacedFieldCell(tlpEdit, 1, 2, "طبيعة الدراسة (type):", isLocked: false);
            txtStat       = CreateSpacedFieldCell(tlpEdit, 2, 2, "الحالة (stat):", isLocked: false, colSpan: 2);

            // Row 3: Save Changes Button
            btnSaveChanges = new Button
            {
                Text = "💾 حفظ التعديلات (Save Changes)",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3B82F6"),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand,
                Enabled = false,
                Margin = new Padding(10, 6, 10, 2)
            };
            btnSaveChanges.FlatAppearance.BorderSize = 0;
            btnSaveChanges.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2563EB");
            btnSaveChanges.Click += btnSaveChanges_Click;

            tlpEdit.Controls.Add(btnSaveChanges, 0, 3);
            tlpEdit.SetColumnSpan(btnSaveChanges, 4);

            grpEditPanel.Controls.Add(tlpEdit);

            // Add all 4 sections to Master TableLayoutPanel
            tlpMainLayout.Controls.Add(pnlTopHeader, 0, 0);
            tlpMainLayout.Controls.Add(grpSearch, 0, 1);
            tlpMainLayout.Controls.Add(grpGrid, 0, 2);
            tlpMainLayout.Controls.Add(grpEditPanel, 0, 3);

            this.Controls.Add(tlpMainLayout);

            // Grid stays empty on load; data is fetched only when the user clicks Search (بحث).

            ResumeLayout(false);
        }

        /// <summary>
        /// Creates a well-spaced 2-row TableLayoutPanel cell (Label on top + full-width anchored TextBox below)
        /// with 10px horizontal margins so fields never squish or overlap.
        /// </summary>
        private static TextBox CreateSpacedFieldCell(
            TableLayoutPanel parentGrid,
            int col,
            int row,
            string labelText,
            bool isLocked,
            int colSpan = 1)
        {
            var cellTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(10, 4, 10, 4),
                Padding = new Padding(0)
            };
            cellTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cellTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            cellTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var lbl = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = isLocked ? Color.FromArgb(185, 28, 28) : Color.FromArgb(30, 41, 59),
                TextAlign = ContentAlignment.BottomRight,
                Margin = new Padding(0, 0, 0, 3)
            };

            var txt = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                Font = new Font("Segoe UI", 11F, isLocked ? FontStyle.Bold : FontStyle.Regular),
                ReadOnly = isLocked,
                Enabled = !isLocked,
                BackColor = isLocked ? Color.FromArgb(226, 232, 240) : Color.White,
                ForeColor = isLocked ? Color.FromArgb(71, 85, 105) : Color.FromArgb(15, 23, 42),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 2, 0, 2)
            };

            cellTable.Controls.Add(lbl, 0, 0);
            cellTable.Controls.Add(txt, 0, 1);

            parentGrid.Controls.Add(cellTable, col, row);
            if (colSpan > 1)
            {
                parentGrid.SetColumnSpan(cellTable, colSpan);
            }

            return txt;
        }

        #region 2. Search Logic (Parameterized LIKE Query on id, ar_name, mail)

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            ExecuteSearch();
        }

        private void ExecuteSearch()
        {
            string term = txtSearch.Text.Trim();

            try
            {
                using var conn = DbHelper.GetConnection();
                conn.Open();

                string sql = $@"
                    SELECT 
                        id,
                        ar_name,
                        first_name,
                        last_name,
                        mail,
                        college,
                        dept,
                        code,
                        stage,
                        type,
                        stat,
                        create_date,
                        user_name
                    FROM dbo.[{_validatedTable}]
                    WHERE id LIKE @search 
                       OR ar_name LIKE @search 
                       OR mail LIKE @search
                    ORDER BY pk DESC";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@search", SqlDbType.NVarChar, 200).Value = $"%{term}%";

                using var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                dgvStudents.DataSource = dt;
                FormatGridColumnHeaders();

                lblResultsCount.Text = $"عدد النتائج: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء البحث في قاعدة البيانات:\n{ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatGridColumnHeaders()
        {
            if (dgvStudents.Columns.Count == 0) return;

            ConfigureColumn("id", "الرقم القومي", 125F);
            ConfigureColumn("ar_name", "الاسم باللغة العربية", 160F);
            ConfigureColumn("first_name", "First Name", 95F);
            ConfigureColumn("last_name", "Last Name", 130F);
            ConfigureColumn("mail", "الإيميل الجامعي", 155F);
            ConfigureColumn("college", "الكلية", 110F);
            ConfigureColumn("dept", "الشعبة", 100F);
            ConfigureColumn("code", "الكود", 80F);
            ConfigureColumn("stage", "الفرقة", 85F);
            ConfigureColumn("type", "طبيعة الدراسة", 95F);
            ConfigureColumn("stat", "الحالة", 80F);
            ConfigureColumn("create_date", "تاريخ الإنشاء", 115F);
            ConfigureColumn("user_name", "المستخدم", 85F);
        }

        private void ConfigureColumn(string colName, string headerText, float fillWeight)
        {
            if (dgvStudents.Columns.Contains(colName))
            {
                var col = dgvStudents.Columns[colName]!;
                col.HeaderText = headerText;
                col.MinimumWidth = 65;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        #endregion

        #region 3. CellClick Auto-Fill & Save Changes (UPDATE + dbo.SystemLogs Audit)

        private void dgvStudents_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvStudents.Rows.Count)
                return;

            DataGridViewRow row = dgvStudents.Rows[e.RowIndex];

            txtNationalId.Text = GetRowCellValue(row, "id");
            txtEmail.Text      = GetRowCellValue(row, "mail");
            txtArName.Text     = GetRowCellValue(row, "ar_name");
            txtFirstName.Text  = GetRowCellValue(row, "first_name");
            txtLastName.Text   = GetRowCellValue(row, "last_name");
            txtCollege.Text    = GetRowCellValue(row, "college");
            txtDept.Text       = GetRowCellValue(row, "dept");
            txtCode.Text       = GetRowCellValue(row, "code");
            txtStage.Text      = GetRowCellValue(row, "stage");
            txtType.Text       = GetRowCellValue(row, "type");
            txtStat.Text       = GetRowCellValue(row, "stat");

            btnSaveChanges.Enabled = !string.IsNullOrWhiteSpace(txtNationalId.Text);
        }

        private static string GetRowCellValue(DataGridViewRow row, string columnName)
        {
            if (!row.DataGridView!.Columns.Contains(columnName))
                return string.Empty;

            return row.Cells[columnName].Value?.ToString()?.Trim() ?? string.Empty;
        }

        private void btnSaveChanges_Click(object? sender, EventArgs e)
        {
            string nationalId = txtNationalId.Text.Trim();

            if (string.IsNullOrWhiteSpace(nationalId))
            {
                MessageBox.Show("يرجى اختيار طالب من الجدول أولاً لتعديل بياناته.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string tableName = $"dbo.{_validatedTable}";
                string username = string.IsNullOrWhiteSpace(_username)
                    ? UserSession.CurrentUsername
                    : _username;

                using var conn = DbHelper.GetConnection();
                conn.Open();

                // 1. Execute UPDATE query modifying ONLY the allowed fields (id and mail are NEVER modified)
                string updateSql = $@"
                    UPDATE dbo.[{_validatedTable}]
                    SET ar_name    = @ar_name,
                        first_name = @first_name,
                        last_name  = @last_name,
                        college    = @college,
                        dept       = @dept,
                        code       = @code,
                        stage      = @stage,
                        type       = @type,
                        stat       = @stat
                    WHERE id = @id;";

                using (var updateCmd = new SqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.Add("@ar_name", SqlDbType.NVarChar, 150).Value    = txtArName.Text.Trim();
                    updateCmd.Parameters.Add("@first_name", SqlDbType.NVarChar, 100).Value = txtFirstName.Text.Trim();
                    updateCmd.Parameters.Add("@last_name", SqlDbType.NVarChar, 100).Value  = txtLastName.Text.Trim();
                    updateCmd.Parameters.Add("@college", SqlDbType.NVarChar, 100).Value    = txtCollege.Text.Trim();
                    updateCmd.Parameters.Add("@dept", SqlDbType.NVarChar, 100).Value       = txtDept.Text.Trim();
                    updateCmd.Parameters.Add("@code", SqlDbType.NVarChar, 50).Value        = txtCode.Text.Trim();
                    updateCmd.Parameters.Add("@stage", SqlDbType.NVarChar, 50).Value       = txtStage.Text.Trim();
                    updateCmd.Parameters.Add("@type", SqlDbType.NVarChar, 50).Value        = txtType.Text.Trim();
                    updateCmd.Parameters.Add("@stat", SqlDbType.NVarChar, 50).Value        = txtStat.Text.Trim();
                    updateCmd.Parameters.Add("@id", SqlDbType.NVarChar, 50).Value          = nationalId;

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("لم يتم العثور على سجل الطالب لتحديثه.", "تنبيه",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // 2. Detailed Audit Logging into dbo.SystemLogs immediately after UPDATE succeeds
                string logDetails = $"قام المستخدم {username} بإجراء عملية تعديل (UPDATE) لبيانات الطالب صاحب الرقم القومي {nationalId} في جدول {tableName}.";

                const string logSql = @"
                    INSERT INTO dbo.SystemLogs (Username, ActionTime, ActionDescription)
                    VALUES (@Username, @ActionTime, @ActionDescription);";

                using (var logCmd = new SqlCommand(logSql, conn))
                {
                    logCmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;
                    logCmd.Parameters.Add("@ActionTime", SqlDbType.DateTime).Value = DateTime.Now;
                    logCmd.Parameters.Add("@ActionDescription", SqlDbType.NVarChar, 1000).Value = logDetails;

                    logCmd.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "تم تعديل البيانات وتسجيل العملية بنجاح!",
                    "نجاح التعديل",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Refresh DataGridView to reflect updated student record
                ExecuteSearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearEditFields()
        {
            txtNationalId.Clear();
            txtEmail.Clear();
            txtArName.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtCollege.Clear();
            txtDept.Clear();
            txtCode.Clear();
            txtStage.Clear();
            txtType.Clear();
            txtStat.Clear();
            btnSaveChanges.Enabled = false;
        }

        #endregion
    }
}
