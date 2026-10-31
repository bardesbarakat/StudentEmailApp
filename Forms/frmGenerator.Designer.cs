namespace StudentEmailApp.Forms
{
    partial class frmGenerator
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblTargetTitle;
        private System.Windows.Forms.Label lblTargetTableInfo;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnUploadExcel;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Label lblStudentCount;
        private System.Windows.Forms.Label lblDomainLabel;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGeneratedEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Panel pnlBottomBar;
        private System.Windows.Forms.Button btnCreateEmails;
        private System.Windows.Forms.Button btnSaveToDb;
        private System.Windows.Forms.Label lblSelectionStats;
        private System.Windows.Forms.ProgressBar progressBar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblTargetTableInfo = new System.Windows.Forms.Label();
            this.lblTargetTitle = new System.Windows.Forms.Label();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.lblDomainLabel = new System.Windows.Forms.Label();
            this.lblStudentCount = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnUploadExcel = new System.Windows.Forms.Button();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGeneratedEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBottomBar = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblSelectionStats = new System.Windows.Forms.Label();
            this.btnSaveToDb = new System.Windows.Forms.Button();
            this.btnCreateEmails = new System.Windows.Forms.Button();
            this.pnlTopBar.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.pnlBottomBar.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlTopBar.Controls.Add(this.btnBack);
            this.pnlTopBar.Controls.Add(this.lblTargetTableInfo);
            this.pnlTopBar.Controls.Add(this.lblTargetTitle);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(950, 65);
            this.pnlTopBar.TabIndex = 0;

            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(830, 16);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 32);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "← Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // 
            // lblTargetTableInfo
            // 
            this.lblTargetTableInfo.AutoSize = true;
            this.lblTargetTableInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTargetTableInfo.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTargetTableInfo.Location = new System.Drawing.Point(22, 38);
            this.lblTargetTableInfo.Name = "lblTargetTableInfo";
            this.lblTargetTableInfo.Size = new System.Drawing.Size(176, 17);
            this.lblTargetTableInfo.TabIndex = 1;
            this.lblTargetTableInfo.Text = "Target Database Table: mails";

            // 
            // lblTargetTitle
            // 
            this.lblTargetTitle.AutoSize = true;
            this.lblTargetTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTargetTitle.ForeColor = System.Drawing.Color.White;
            this.lblTargetTitle.Location = new System.Drawing.Point(20, 12);
            this.lblTargetTitle.Name = "lblTargetTitle";
            this.lblTargetTitle.Size = new System.Drawing.Size(287, 25);
            this.lblTargetTitle.TabIndex = 0;
            this.lblTargetTitle.Text = "Email Generator - Undergrads";

            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.White;
            this.pnlToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlToolbar.Controls.Add(this.txtDomain);
            this.pnlToolbar.Controls.Add(this.lblDomainLabel);
            this.pnlToolbar.Controls.Add(this.lblStudentCount);
            this.pnlToolbar.Controls.Add(this.chkSelectAll);
            this.pnlToolbar.Controls.Add(this.btnUploadExcel);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 65);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(950, 60);
            this.pnlToolbar.TabIndex = 1;

            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDomain.Location = new System.Drawing.Point(780, 17);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.PlaceholderText = "@univ.edu (optional)";
            this.txtDomain.Size = new System.Drawing.Size(150, 24);
            this.txtDomain.TabIndex = 4;

            // 
            // lblDomainLabel
            // 
            this.lblDomainLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDomainLabel.AutoSize = true;
            this.lblDomainLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDomainLabel.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblDomainLabel.Location = new System.Drawing.Point(685, 21);
            this.lblDomainLabel.Name = "lblDomainLabel";
            this.lblDomainLabel.Size = new System.Drawing.Size(91, 15);
            this.lblDomainLabel.TabIndex = 3;
            this.lblDomainLabel.Text = "Domain (opt):";

            // 
            // lblStudentCount
            // 
            this.lblStudentCount.AutoSize = true;
            this.lblStudentCount.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStudentCount.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblStudentCount.Location = new System.Drawing.Point(310, 19);
            this.lblStudentCount.Name = "lblStudentCount";
            this.lblStudentCount.Size = new System.Drawing.Size(188, 19);
            this.lblStudentCount.TabIndex = 2;
            this.lblStudentCount.Text = "Uploaded: 0 students (0 sel)";

            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Checked = true;
            this.chkSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkSelectAll.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.chkSelectAll.Location = new System.Drawing.Point(195, 18);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(82, 21);
            this.chkSelectAll.TabIndex = 1;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);

            // 
            // btnUploadExcel
            // 
            this.btnUploadExcel.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnUploadExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUploadExcel.FlatAppearance.BorderSize = 0;
            this.btnUploadExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadExcel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnUploadExcel.ForeColor = System.Drawing.Color.White;
            this.btnUploadExcel.Location = new System.Drawing.Point(20, 11);
            this.btnUploadExcel.Name = "btnUploadExcel";
            this.btnUploadExcel.Size = new System.Drawing.Size(150, 36);
            this.btnUploadExcel.TabIndex = 0;
            this.btnUploadExcel.Text = "📁 Upload Excel";
            this.btnUploadExcel.UseVisualStyleBackColor = false;
            this.btnUploadExcel.Click += new System.EventHandler(this.btnUploadExcel_Click);

            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.BackgroundColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvStudents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colIndex,
            this.colFirstName,
            this.colLastName,
            this.colGeneratedEmail,
            this.colStatus});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(224, 231, 255);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStudents.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStudents.EnableHeadersVisualStyles = false;
            this.dgvStudents.Location = new System.Drawing.Point(0, 125);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.RowTemplate.Height = 32;
            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.Size = new System.Drawing.Size(950, 410);
            this.dgvStudents.TabIndex = 2;
            this.dgvStudents.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStudents_CellValueChanged);
            this.dgvStudents.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvStudents_CurrentCellDirtyStateChanged);

            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "Select";
            this.colSelect.Name = "colSelect";
            this.colSelect.Width = 60;

            // 
            // colIndex
            // 
            this.colIndex.HeaderText = "#";
            this.colIndex.Name = "colIndex";
            this.colIndex.ReadOnly = true;
            this.colIndex.Width = 50;

            // 
            // colFirstName
            // 
            this.colFirstName.HeaderText = "First Name (Col 1)";
            this.colFirstName.Name = "colFirstName";
            this.colFirstName.ReadOnly = true;
            this.colFirstName.Width = 180;

            // 
            // colLastName
            // 
            this.colLastName.HeaderText = "Last Name (Col 2)";
            this.colLastName.Name = "colLastName";
            this.colLastName.ReadOnly = true;
            this.colLastName.Width = 180;

            // 
            // colGeneratedEmail
            // 
            this.colGeneratedEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGeneratedEmail.HeaderText = "Generated Email Address";
            this.colGeneratedEmail.Name = "colGeneratedEmail";
            this.colGeneratedEmail.ReadOnly = true;

            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 140;

            // 
            // pnlBottomBar
            // 
            this.pnlBottomBar.BackColor = System.Drawing.Color.White;
            this.pnlBottomBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottomBar.Controls.Add(this.progressBar);
            this.pnlBottomBar.Controls.Add(this.lblSelectionStats);
            this.pnlBottomBar.Controls.Add(this.btnSaveToDb);
            this.pnlBottomBar.Controls.Add(this.btnCreateEmails);
            this.pnlBottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomBar.Location = new System.Drawing.Point(0, 535);
            this.pnlBottomBar.Name = "pnlBottomBar";
            this.pnlBottomBar.Size = new System.Drawing.Size(950, 65);
            this.pnlBottomBar.TabIndex = 3;

            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(20, 42);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(460, 10);
            this.progressBar.TabIndex = 3;
            this.progressBar.Visible = false;

            // 
            // lblSelectionStats
            // 
            this.lblSelectionStats.AutoSize = true;
            this.lblSelectionStats.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSelectionStats.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSelectionStats.Location = new System.Drawing.Point(20, 18);
            this.lblSelectionStats.Name = "lblSelectionStats";
            this.lblSelectionStats.Size = new System.Drawing.Size(263, 15);
            this.lblSelectionStats.TabIndex = 2;
            this.lblSelectionStats.Text = "Ready. Please upload an Excel file to begin.";

            // 
            // btnSaveToDb
            // 
            this.btnSaveToDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveToDb.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnSaveToDb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveToDb.Enabled = false;
            this.btnSaveToDb.FlatAppearance.BorderSize = 0;
            this.btnSaveToDb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveToDb.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSaveToDb.ForeColor = System.Drawing.Color.White;
            this.btnSaveToDb.Location = new System.Drawing.Point(740, 12);
            this.btnSaveToDb.Name = "btnSaveToDb";
            this.btnSaveToDb.Size = new System.Drawing.Size(190, 40);
            this.btnSaveToDb.TabIndex = 1;
            this.btnSaveToDb.Text = "💾 Save to Database";
            this.btnSaveToDb.UseVisualStyleBackColor = false;
            this.btnSaveToDb.Click += new System.EventHandler(this.btnSaveToDb_Click);

            // 
            // btnCreateEmails
            // 
            this.btnCreateEmails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateEmails.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnCreateEmails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateEmails.Enabled = false;
            this.btnCreateEmails.FlatAppearance.BorderSize = 0;
            this.btnCreateEmails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateEmails.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCreateEmails.ForeColor = System.Drawing.Color.White;
            this.btnCreateEmails.Location = new System.Drawing.Point(530, 12);
            this.btnCreateEmails.Name = "btnCreateEmails";
            this.btnCreateEmails.Size = new System.Drawing.Size(195, 40);
            this.btnCreateEmails.TabIndex = 0;
            this.btnCreateEmails.Text = "⚡ Create Emails";
            this.btnCreateEmails.UseVisualStyleBackColor = false;
            this.btnCreateEmails.Click += new System.EventHandler(this.btnCreateEmails_Click);

            // 
            // frmGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(950, 600);
            this.Controls.Add(this.dgvStudents);
            this.Controls.Add(this.pnlBottomBar);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.pnlTopBar);
            this.MinimumSize = new System.Drawing.Size(850, 500);
            this.Name = "frmGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Email Generator";
            this.Load += new System.EventHandler(this.frmGenerator_Load);
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.pnlBottomBar.ResumeLayout(false);
            this.pnlBottomBar.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
