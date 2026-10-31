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
        private System.Windows.Forms.Label lblDomainLabel;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel pnlLeftHeader;
        private System.Windows.Forms.Label lblRawHeader;
        private System.Windows.Forms.Label lblRawCount;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.DataGridView dgvOriginal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colOrigSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigStatus;
        private System.Windows.Forms.Panel pnlRightHeader;
        private System.Windows.Forms.Label lblGenHeader;
        private System.Windows.Forms.Label lblGenCount;
        private System.Windows.Forms.DataGridView dgvGenerated;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenStatus;
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
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvRowStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvRowStyle2 = new System.Windows.Forms.DataGridViewCellStyle();

            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblTargetTableInfo = new System.Windows.Forms.Label();
            this.lblTargetTitle = new System.Windows.Forms.Label();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.lblDomainLabel = new System.Windows.Forms.Label();
            this.btnUploadExcel = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvOriginal = new System.Windows.Forms.DataGridView();
            this.colOrigSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colOrigIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLeftHeader = new System.Windows.Forms.Panel();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.lblRawCount = new System.Windows.Forms.Label();
            this.lblRawHeader = new System.Windows.Forms.Label();
            this.dgvGenerated = new System.Windows.Forms.DataGridView();
            this.colGenIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlRightHeader = new System.Windows.Forms.Panel();
            this.lblGenCount = new System.Windows.Forms.Label();
            this.lblGenHeader = new System.Windows.Forms.Label();
            this.pnlBottomBar = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblSelectionStats = new System.Windows.Forms.Label();
            this.btnSaveToDb = new System.Windows.Forms.Button();
            this.btnCreateEmails = new System.Windows.Forms.Button();

            this.pnlTopBar.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOriginal)).BeginInit();
            this.pnlLeftHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGenerated)).BeginInit();
            this.pnlRightHeader.SuspendLayout();
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
            this.pnlTopBar.Size = new System.Drawing.Size(1200, 65);
            this.pnlTopBar.TabIndex = 0;

            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(1080, 16);
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
            this.lblTargetTableInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
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
            this.lblTargetTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTargetTitle.ForeColor = System.Drawing.Color.White;
            this.lblTargetTitle.Location = new System.Drawing.Point(20, 12);
            this.lblTargetTitle.Name = "lblTargetTitle";
            this.lblTargetTitle.Size = new System.Drawing.Size(306, 25);
            this.lblTargetTitle.TabIndex = 0;
            this.lblTargetTitle.Text = "Email Generator — Split Screen";

            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.White;
            this.pnlToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlToolbar.Controls.Add(this.txtDomain);
            this.pnlToolbar.Controls.Add(this.lblDomainLabel);
            this.pnlToolbar.Controls.Add(this.btnUploadExcel);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 65);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(1200, 55);
            this.pnlToolbar.TabIndex = 1;

            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDomain.Location = new System.Drawing.Point(1020, 14);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.PlaceholderText = "@univ.edu (optional)";
            this.txtDomain.Size = new System.Drawing.Size(160, 24);
            this.txtDomain.TabIndex = 4;

            // 
            // lblDomainLabel
            // 
            this.lblDomainLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDomainLabel.AutoSize = true;
            this.lblDomainLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDomainLabel.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblDomainLabel.Location = new System.Drawing.Point(895, 18);
            this.lblDomainLabel.Name = "lblDomainLabel";
            this.lblDomainLabel.Size = new System.Drawing.Size(119, 15);
            this.lblDomainLabel.TabIndex = 3;
            this.lblDomainLabel.Text = "Email Domain (opt):";

            // 
            // btnUploadExcel
            // 
            this.btnUploadExcel.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnUploadExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUploadExcel.FlatAppearance.BorderSize = 0;
            this.btnUploadExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadExcel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUploadExcel.ForeColor = System.Drawing.Color.White;
            this.btnUploadExcel.Location = new System.Drawing.Point(18, 9);
            this.btnUploadExcel.Name = "btnUploadExcel";
            this.btnUploadExcel.Size = new System.Drawing.Size(160, 36);
            this.btnUploadExcel.TabIndex = 0;
            this.btnUploadExcel.Text = "📁 Upload Excel";
            this.btnUploadExcel.UseVisualStyleBackColor = false;
            this.btnUploadExcel.Click += new System.EventHandler(this.btnUploadExcel_Click);

            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 120);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1 (Left: Original Data)
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer.Panel1.Controls.Add(this.dgvOriginal);
            this.splitContainer.Panel1.Controls.Add(this.pnlLeftHeader);
            // 
            // splitContainer.Panel2 (Right: Generated Data)
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer.Panel2.Controls.Add(this.dgvGenerated);
            this.splitContainer.Panel2.Controls.Add(this.pnlRightHeader);
            this.splitContainer.Size = new System.Drawing.Size(1200, 480);
            this.splitContainer.SplitterDistance = 580;
            this.splitContainer.SplitterWidth = 6;
            this.splitContainer.TabIndex = 2;

            // 
            // pnlLeftHeader
            // 
            this.pnlLeftHeader.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlLeftHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLeftHeader.Controls.Add(this.chkSelectAll);
            this.pnlLeftHeader.Controls.Add(this.lblRawCount);
            this.pnlLeftHeader.Controls.Add(this.lblRawHeader);
            this.pnlLeftHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLeftHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftHeader.Name = "pnlLeftHeader";
            this.pnlLeftHeader.Size = new System.Drawing.Size(580, 42);
            this.pnlLeftHeader.TabIndex = 0;

            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Checked = true;
            this.chkSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.chkSelectAll.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.chkSelectAll.Location = new System.Drawing.Point(485, 11);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(75, 19);
            this.chkSelectAll.TabIndex = 2;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);

            // 
            // lblRawCount
            // 
            this.lblRawCount.AutoSize = true;
            this.lblRawCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.lblRawCount.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblRawCount.Location = new System.Drawing.Point(235, 12);
            this.lblRawCount.Name = "lblRawCount";
            this.lblRawCount.Size = new System.Drawing.Size(125, 15);
            this.lblRawCount.TabIndex = 1;
            this.lblRawCount.Text = "(0 uploaded | 0 sel)";

            // 
            // lblRawHeader
            // 
            this.lblRawHeader.AutoSize = true;
            this.lblRawHeader.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblRawHeader.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblRawHeader.Location = new System.Drawing.Point(12, 10);
            this.lblRawHeader.Name = "lblRawHeader";
            this.lblRawHeader.Size = new System.Drawing.Size(217, 19);
            this.lblRawHeader.TabIndex = 0;
            this.lblRawHeader.Text = "📥 Original Data (dgvOriginal)";

            // 
            // dgvOriginal
            // 
            this.dgvOriginal.AllowUserToAddRows = false;
            this.dgvOriginal.AllowUserToDeleteRows = false;
            this.dgvOriginal.BackgroundColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvHeaderStyle1.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            dgvHeaderStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            dgvHeaderStyle1.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvHeaderStyle1.Padding = new System.Windows.Forms.Padding(4);
            dgvHeaderStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgvHeaderStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvOriginal.ColumnHeadersDefaultCellStyle = dgvHeaderStyle1;
            this.dgvOriginal.ColumnHeadersHeight = 38;
            this.dgvOriginal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOriginal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrigSelect,
            this.colOrigIndex,
            this.colOrigId,
            this.colOrigFullName,
            this.colOrigStatus});
            dgvRowStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle1.BackColor = System.Drawing.Color.White;
            dgvRowStyle1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgvRowStyle1.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dgvRowStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(224, 231, 255);
            dgvRowStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvOriginal.DefaultCellStyle = dgvRowStyle1;
            this.dgvOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOriginal.EnableHeadersVisualStyles = false;
            this.dgvOriginal.Location = new System.Drawing.Point(0, 42);
            this.dgvOriginal.Name = "dgvOriginal";
            this.dgvOriginal.RowHeadersVisible = false;
            this.dgvOriginal.RowTemplate.Height = 34;
            this.dgvOriginal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOriginal.Size = new System.Drawing.Size(580, 438);
            this.dgvOriginal.TabIndex = 1;
            this.dgvOriginal.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOriginal_CellValueChanged);
            this.dgvOriginal.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvOriginal_CurrentCellDirtyStateChanged);

            // 
            // colOrigSelect
            // 
            this.colOrigSelect.HeaderText = "Select";
            this.colOrigSelect.Name = "colOrigSelect";
            this.colOrigSelect.Width = 55;

            // 
            // colOrigIndex
            // 
            this.colOrigIndex.HeaderText = "#";
            this.colOrigIndex.Name = "colOrigIndex";
            this.colOrigIndex.ReadOnly = true;
            this.colOrigIndex.Width = 45;

            // 
            // colOrigId
            // 
            this.colOrigId.HeaderText = "ID (Col 0)";
            this.colOrigId.Name = "colOrigId";
            this.colOrigId.ReadOnly = true;
            this.colOrigId.Width = 90;

            // 
            // colOrigFullName
            // 
            this.colOrigFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colOrigFullName.HeaderText = "Full English Name (Col 1)";
            this.colOrigFullName.Name = "colOrigFullName";
            this.colOrigFullName.ReadOnly = true;

            // 
            // colOrigStatus
            // 
            this.colOrigStatus.HeaderText = "Status";
            this.colOrigStatus.Name = "colOrigStatus";
            this.colOrigStatus.ReadOnly = true;
            this.colOrigStatus.Width = 85;

            // 
            // pnlRightHeader
            // 
            this.pnlRightHeader.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlRightHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRightHeader.Controls.Add(this.lblGenCount);
            this.pnlRightHeader.Controls.Add(this.lblGenHeader);
            this.pnlRightHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRightHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlRightHeader.Name = "pnlRightHeader";
            this.pnlRightHeader.Size = new System.Drawing.Size(614, 42);
            this.pnlRightHeader.TabIndex = 0;

            // 
            // lblGenCount
            // 
            this.lblGenCount.AutoSize = true;
            this.lblGenCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.lblGenCount.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.lblGenCount.Location = new System.Drawing.Point(260, 12);
            this.lblGenCount.Name = "lblGenCount";
            this.lblGenCount.Size = new System.Drawing.Size(76, 15);
            this.lblGenCount.TabIndex = 1;
            this.lblGenCount.Text = "(0 generated)";

            // 
            // lblGenHeader
            // 
            this.lblGenHeader.AutoSize = true;
            this.lblGenHeader.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblGenHeader.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblGenHeader.Location = new System.Drawing.Point(12, 10);
            this.lblGenHeader.Name = "lblGenHeader";
            this.lblGenHeader.Size = new System.Drawing.Size(242, 19);
            this.lblGenHeader.TabIndex = 0;
            this.lblGenHeader.Text = "⚡ Processed Emails (dgvGenerated)";

            // 
            // dgvGenerated
            // 
            this.dgvGenerated.AllowUserToAddRows = false;
            this.dgvGenerated.AllowUserToDeleteRows = false;
            this.dgvGenerated.BackgroundColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvGenerated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvHeaderStyle2.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            dgvHeaderStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            dgvHeaderStyle2.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvHeaderStyle2.Padding = new System.Windows.Forms.Padding(4);
            dgvHeaderStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgvHeaderStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvGenerated.ColumnHeadersDefaultCellStyle = dgvHeaderStyle2;
            this.dgvGenerated.ColumnHeadersHeight = 38;
            this.dgvGenerated.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGenerated.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGenIndex,
            this.colGenId,
            this.colGenFullName,
            this.colGenFirstName,
            this.colGenLastName,
            this.colGenEmail,
            this.colGenStatus});
            dgvRowStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle2.BackColor = System.Drawing.Color.White;
            dgvRowStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgvRowStyle2.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dgvRowStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(224, 231, 255);
            dgvRowStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvGenerated.DefaultCellStyle = dgvRowStyle2;
            this.dgvGenerated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGenerated.EnableHeadersVisualStyles = false;
            this.dgvGenerated.Location = new System.Drawing.Point(0, 42);
            this.dgvGenerated.Name = "dgvGenerated";
            this.dgvGenerated.RowHeadersVisible = false;
            this.dgvGenerated.RowTemplate.Height = 34;
            this.dgvGenerated.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGenerated.Size = new System.Drawing.Size(614, 438);
            this.dgvGenerated.TabIndex = 1;

            // 
            // colGenIndex
            // 
            this.colGenIndex.HeaderText = "#";
            this.colGenIndex.Name = "colGenIndex";
            this.colGenIndex.ReadOnly = true;
            this.colGenIndex.Width = 40;

            // 
            // colGenId
            // 
            this.colGenId.HeaderText = "ID";
            this.colGenId.Name = "colGenId";
            this.colGenId.ReadOnly = true;
            this.colGenId.Width = 75;

            // 
            // colGenFullName
            // 
            this.colGenFullName.HeaderText = "Full English Name";
            this.colGenFullName.Name = "colGenFullName";
            this.colGenFullName.ReadOnly = true;
            this.colGenFullName.Width = 160;

            // 
            // colGenFirstName
            // 
            this.colGenFirstName.HeaderText = "1st Name";
            this.colGenFirstName.Name = "colGenFirstName";
            this.colGenFirstName.ReadOnly = true;
            this.colGenFirstName.Width = 85;

            // 
            // colGenLastName
            // 
            this.colGenLastName.HeaderText = "2nd Name";
            this.colGenLastName.Name = "colGenLastName";
            this.colGenLastName.ReadOnly = true;
            this.colGenLastName.Width = 85;

            // 
            // colGenEmail
            // 
            this.colGenEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGenEmail.HeaderText = "Generated Email";
            this.colGenEmail.Name = "colGenEmail";
            this.colGenEmail.ReadOnly = true;

            // 
            // colGenStatus
            // 
            this.colGenStatus.HeaderText = "Status";
            this.colGenStatus.Name = "colGenStatus";
            this.colGenStatus.ReadOnly = true;
            this.colGenStatus.Width = 85;

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
            this.pnlBottomBar.Location = new System.Drawing.Point(0, 600);
            this.pnlBottomBar.Name = "pnlBottomBar";
            this.pnlBottomBar.Size = new System.Drawing.Size(1200, 65);
            this.pnlBottomBar.TabIndex = 3;

            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(20, 42);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(650, 10);
            this.progressBar.TabIndex = 3;
            this.progressBar.Visible = false;

            // 
            // lblSelectionStats
            // 
            this.lblSelectionStats.AutoSize = true;
            this.lblSelectionStats.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSelectionStats.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSelectionStats.Location = new System.Drawing.Point(20, 18);
            this.lblSelectionStats.Name = "lblSelectionStats";
            this.lblSelectionStats.Size = new System.Drawing.Size(298, 15);
            this.lblSelectionStats.TabIndex = 2;
            this.lblSelectionStats.Text = "Ready. Please upload an Excel file to populate Original data.";

            // 
            // btnSaveToDb
            // 
            this.btnSaveToDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveToDb.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnSaveToDb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveToDb.Enabled = false;
            this.btnSaveToDb.FlatAppearance.BorderSize = 0;
            this.btnSaveToDb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveToDb.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveToDb.ForeColor = System.Drawing.Color.White;
            this.btnSaveToDb.Location = new System.Drawing.Point(970, 12);
            this.btnSaveToDb.Name = "btnSaveToDb";
            this.btnSaveToDb.Size = new System.Drawing.Size(210, 40);
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
            this.btnCreateEmails.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreateEmails.ForeColor = System.Drawing.Color.White;
            this.btnCreateEmails.Location = new System.Drawing.Point(740, 12);
            this.btnCreateEmails.Name = "btnCreateEmails";
            this.btnCreateEmails.Size = new System.Drawing.Size(215, 40);
            this.btnCreateEmails.TabIndex = 0;
            this.btnCreateEmails.Text = "⚡ Create Emails →";
            this.btnCreateEmails.UseVisualStyleBackColor = false;
            this.btnCreateEmails.Click += new System.EventHandler(this.btnCreateEmails_Click);

            // 
            // frmGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(1200, 665);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlBottomBar);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.pnlTopBar);
            this.MinimumSize = new System.Drawing.Size(1000, 550);
            this.Name = "frmGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Email Generator - Split Screen";
            this.Load += new System.EventHandler(this.frmGenerator_Load);
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOriginal)).EndInit();
            this.pnlLeftHeader.ResumeLayout(false);
            this.pnlLeftHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGenerated)).EndInit();
            this.pnlRightHeader.ResumeLayout(false);
            this.pnlRightHeader.PerformLayout();
            this.pnlBottomBar.ResumeLayout(false);
            this.pnlBottomBar.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
