namespace StudentEmailApp.Forms
{
    partial class frmGenerator
    {
        private System.ComponentModel.IContainer components = null;

        // Top Bar
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblTargetTitle;
        private System.Windows.Forms.Label lblTargetTableInfo;
        private System.Windows.Forms.Button btnBack;

        // Step GroupBoxes (Fool-Proof Beginner UI Flow)
        private System.Windows.Forms.GroupBox grpStep1;
        private System.Windows.Forms.GroupBox grpStep2;
        private System.Windows.Forms.GroupBox grpStep3;
        private System.Windows.Forms.Panel pnlMiddleGenerateBar;

        // Excel Controls Section
        private System.Windows.Forms.Panel pnlExcelControls;
        private System.Windows.Forms.Label lblExcelPath;
        private System.Windows.Forms.TextBox txtExcelPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnLoadSheet;
        private System.Windows.Forms.Button btnClearOriginal;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.TextBox txtDomain;

        // Split Container (Top: Original, Bottom: Generated)
        private System.Windows.Forms.SplitContainer splitMain;

        // Panel 1 (Top Grid: dgvOriginal)
        private System.Windows.Forms.Panel pnlOrigHeader;
        private System.Windows.Forms.Label lblOrigTitle;
        private System.Windows.Forms.Label lblOriginalCount;
        private System.Windows.Forms.CheckBox chkSelectAllOrig;
        private System.Windows.Forms.Button btnCreateEmails;
        private System.Windows.Forms.DataGridView dgvOriginal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colOrigSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigCollege;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigDept;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigStatus;

        // Panel 2 (Bottom Grid: dgvGenerated)
        private System.Windows.Forms.Panel pnlGenHeader;
        private System.Windows.Forms.Label lblGenTitle;
        private System.Windows.Forms.Label lblGeneratedCount;
        private System.Windows.Forms.CheckBox chkSelectAllGen;
        private System.Windows.Forms.Button btnClearGenerated;
        private System.Windows.Forms.DataGridView dgvGenerated;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colGenSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenCollege;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenDept;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenStatus;

        // Bottom Action Bar
        private System.Windows.Forms.Panel pnlBottomActions;
        private System.Windows.Forms.Button btnShowReport;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Button btnSaveToDb;
        private System.Windows.Forms.ProgressBar progressBar;

        // Footer / StatusStrip
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusUser;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusTable;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusAction;

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
            System.Windows.Forms.DataGridViewCellStyle dgvHeader1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvRow1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvHeader2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvRow2 = new System.Windows.Forms.DataGridViewCellStyle();

            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblTargetTableInfo = new System.Windows.Forms.Label();
            this.lblTargetTitle = new System.Windows.Forms.Label();
            this.grpStep1 = new System.Windows.Forms.GroupBox();
            this.grpStep2 = new System.Windows.Forms.GroupBox();
            this.grpStep3 = new System.Windows.Forms.GroupBox();
            this.pnlMiddleGenerateBar = new System.Windows.Forms.Panel();
            this.pnlExcelControls = new System.Windows.Forms.Panel();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.lblDomain = new System.Windows.Forms.Label();
            this.btnClearOriginal = new System.Windows.Forms.Button();
            this.btnLoadSheet = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtExcelPath = new System.Windows.Forms.TextBox();
            this.lblExcelPath = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvOriginal = new System.Windows.Forms.DataGridView();
            this.colOrigSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colOrigIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigCollege = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlOrigHeader = new System.Windows.Forms.Panel();
            this.btnCreateEmails = new System.Windows.Forms.Button();
            this.chkSelectAllOrig = new System.Windows.Forms.CheckBox();
            this.lblOriginalCount = new System.Windows.Forms.Label();
            this.lblOrigTitle = new System.Windows.Forms.Label();
            this.dgvGenerated = new System.Windows.Forms.DataGridView();
            this.colGenSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colGenIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenCollege = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlGenHeader = new System.Windows.Forms.Panel();
            this.btnClearGenerated = new System.Windows.Forms.Button();
            this.chkSelectAllGen = new System.Windows.Forms.CheckBox();
            this.lblGeneratedCount = new System.Windows.Forms.Label();
            this.lblGenTitle = new System.Windows.Forms.Label();
            this.pnlBottomActions = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnSaveToDb = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.btnShowReport = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusSeparator1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusTable = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusSeparator2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusAction = new System.Windows.Forms.ToolStripStatusLabel();

            this.pnlTopBar.SuspendLayout();
            this.pnlExcelControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOriginal)).BeginInit();
            this.pnlOrigHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGenerated)).BeginInit();
            this.pnlGenHeader.SuspendLayout();
            this.pnlBottomActions.SuspendLayout();
            this.statusStrip.SuspendLayout();
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
            this.pnlTopBar.Size = new System.Drawing.Size(1150, 56);
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
            this.btnBack.Location = new System.Drawing.Point(1045, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(90, 32);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "← Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // 
            // lblTargetTableInfo
            // 
            this.lblTargetTableInfo.AutoSize = true;
            this.lblTargetTableInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTargetTableInfo.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTargetTableInfo.Location = new System.Drawing.Point(20, 32);
            this.lblTargetTableInfo.Name = "lblTargetTableInfo";
            this.lblTargetTableInfo.Size = new System.Drawing.Size(161, 15);
            this.lblTargetTableInfo.TabIndex = 1;
            this.lblTargetTableInfo.Text = "Target Database Table: dbo.[mails]";

            // 
            // lblTargetTitle
            // 
            this.lblTargetTitle.AutoSize = true;
            this.lblTargetTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTargetTitle.ForeColor = System.Drawing.Color.White;
            this.lblTargetTitle.Location = new System.Drawing.Point(18, 8);
            this.lblTargetTitle.Name = "lblTargetTitle";
            this.lblTargetTitle.Size = new System.Drawing.Size(268, 25);
            this.lblTargetTitle.TabIndex = 0;
            this.lblTargetTitle.Text = "Student Email Generator Studio";

            // 
            // grpStep1 (الخطوة 1: رفع الملف)
            // 
            this.grpStep1.BackColor = System.Drawing.Color.White;
            this.grpStep1.Controls.Add(this.pnlExcelControls);
            this.grpStep1.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpStep1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpStep1.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.grpStep1.Location = new System.Drawing.Point(0, 56);
            this.grpStep1.Name = "grpStep1";
            this.grpStep1.Padding = new System.Windows.Forms.Padding(8, 4, 8, 6);
            this.grpStep1.Size = new System.Drawing.Size(1150, 82);
            this.grpStep1.TabIndex = 1;
            this.grpStep1.TabStop = false;
            this.grpStep1.Text = "الخطوة 1: رفع الملف";

            // 
            // pnlExcelControls
            // 
            this.pnlExcelControls.BackColor = System.Drawing.Color.White;
            this.pnlExcelControls.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlExcelControls.Controls.Add(this.txtDomain);
            this.pnlExcelControls.Controls.Add(this.lblDomain);
            this.pnlExcelControls.Controls.Add(this.btnClearOriginal);
            this.pnlExcelControls.Controls.Add(this.btnLoadSheet);
            this.pnlExcelControls.Controls.Add(this.btnBrowse);
            this.pnlExcelControls.Controls.Add(this.txtExcelPath);
            this.pnlExcelControls.Controls.Add(this.lblExcelPath);
            this.pnlExcelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExcelControls.Location = new System.Drawing.Point(8, 23);
            this.pnlExcelControls.Name = "pnlExcelControls";
            this.pnlExcelControls.Size = new System.Drawing.Size(1134, 53);
            this.pnlExcelControls.TabIndex = 1;

            // 
            // lblExcelPath
            // 
            this.lblExcelPath.AutoSize = true;
            this.lblExcelPath.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblExcelPath.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblExcelPath.Location = new System.Drawing.Point(14, 19);
            this.lblExcelPath.Name = "lblExcelPath";
            this.lblExcelPath.Size = new System.Drawing.Size(68, 17);
            this.lblExcelPath.TabIndex = 0;
            this.lblExcelPath.Text = "Excel File:";

            // 
            // txtExcelPath
            // 
            this.txtExcelPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExcelPath.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.txtExcelPath.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtExcelPath.Location = new System.Drawing.Point(86, 16);
            this.txtExcelPath.Name = "txtExcelPath";
            this.txtExcelPath.PlaceholderText = "Click 'Browse' to select an Excel (.xlsx, .xls) or .csv file...";
            this.txtExcelPath.ReadOnly = true;
            this.txtExcelPath.Size = new System.Drawing.Size(370, 24);
            this.txtExcelPath.TabIndex = 1;

            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnBrowse.ForeColor = System.Drawing.Color.White;
            this.btnBrowse.Location = new System.Drawing.Point(464, 13);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(95, 30);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "📁 Browse...";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            // 
            // btnLoadSheet
            // 
            this.btnLoadSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSheet.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnLoadSheet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadSheet.FlatAppearance.BorderSize = 0;
            this.btnLoadSheet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadSheet.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLoadSheet.ForeColor = System.Drawing.Color.White;
            this.btnLoadSheet.Location = new System.Drawing.Point(567, 13);
            this.btnLoadSheet.Name = "btnLoadSheet";
            this.btnLoadSheet.Size = new System.Drawing.Size(120, 30);
            this.btnLoadSheet.TabIndex = 3;
            this.btnLoadSheet.Text = "📥 Load Sheet";
            this.btnLoadSheet.UseVisualStyleBackColor = false;
            this.btnLoadSheet.Click += new System.EventHandler(this.btnLoadSheet_Click);

            // 
            // btnClearOriginal
            // 
            this.btnClearOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearOriginal.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnClearOriginal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearOriginal.FlatAppearance.BorderSize = 0;
            this.btnClearOriginal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearOriginal.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClearOriginal.ForeColor = System.Drawing.Color.White;
            this.btnClearOriginal.Location = new System.Drawing.Point(695, 13);
            this.btnClearOriginal.Name = "btnClearOriginal";
            this.btnClearOriginal.Size = new System.Drawing.Size(160, 30);
            this.btnClearOriginal.TabIndex = 4;
            this.btnClearOriginal.Text = "🗑 Clear Loaded Students";
            this.btnClearOriginal.UseVisualStyleBackColor = false;
            this.btnClearOriginal.Click += new System.EventHandler(this.btnClearOriginal_Click);

            // 
            // lblDomain
            // 
            this.lblDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDomain.AutoSize = true;
            this.lblDomain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDomain.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblDomain.Location = new System.Drawing.Point(865, 20);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(91, 15);
            this.lblDomain.TabIndex = 5;
            this.lblDomain.Text = "Domain (opt):";

            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDomain.Location = new System.Drawing.Point(960, 16);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.PlaceholderText = "@univ.edu (optional)";
            this.txtDomain.Size = new System.Drawing.Size(175, 24);
            this.txtDomain.TabIndex = 6;

            // 
            // splitMain (Horizontal Split: Top = Step 2, Bottom = Step 3)
            // 
            this.splitMain.BackColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 138);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;

            // 
            // splitMain.Panel1 (Top: grpStep2 -> dgvOriginal)
            // 
            this.splitMain.Panel1.BackColor = System.Drawing.Color.White;
            this.splitMain.Panel1.Controls.Add(this.grpStep2);
            this.splitMain.Panel1.Padding = new System.Windows.Forms.Padding(6, 4, 6, 2);

            // 
            // grpStep2 (الخطوة 2: إنشاء الإيميلات)
            // 
            this.grpStep2.BackColor = System.Drawing.Color.White;
            this.grpStep2.Controls.Add(this.dgvOriginal);
            this.grpStep2.Controls.Add(this.pnlMiddleGenerateBar);
            this.grpStep2.Controls.Add(this.pnlOrigHeader);
            this.grpStep2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpStep2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpStep2.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.grpStep2.Location = new System.Drawing.Point(6, 4);
            this.grpStep2.Name = "grpStep2";
            this.grpStep2.Padding = new System.Windows.Forms.Padding(6, 4, 6, 6);
            this.grpStep2.Size = new System.Drawing.Size(1138, 250);
            this.grpStep2.TabIndex = 0;
            this.grpStep2.TabStop = false;
            this.grpStep2.Text = "الخطوة 2: إنشاء الإيميلات";

            // 
            // splitMain.Panel2 (Bottom: grpStep3 -> dgvGenerated + Save)
            // 
            this.splitMain.Panel2.BackColor = System.Drawing.Color.White;
            this.splitMain.Panel2.Controls.Add(this.grpStep3);
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(6, 2, 6, 4);
            this.splitMain.Size = new System.Drawing.Size(1150, 572);
            this.splitMain.SplitterDistance = 285;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 2;

            // 
            // grpStep3 (الخطوة 3: الحفظ)
            // 
            this.grpStep3.BackColor = System.Drawing.Color.White;
            this.grpStep3.Controls.Add(this.dgvGenerated);
            this.grpStep3.Controls.Add(this.pnlGenHeader);
            this.grpStep3.Controls.Add(this.pnlBottomActions);
            this.grpStep3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpStep3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpStep3.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.grpStep3.Location = new System.Drawing.Point(6, 2);
            this.grpStep3.Name = "grpStep3";
            this.grpStep3.Padding = new System.Windows.Forms.Padding(6, 4, 6, 6);
            this.grpStep3.Size = new System.Drawing.Size(1138, 300);
            this.grpStep3.TabIndex = 0;
            this.grpStep3.TabStop = false;
            this.grpStep3.Text = "الخطوة 3: الحفظ";

            // 
            // pnlOrigHeader
            // 
            this.pnlOrigHeader.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlOrigHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOrigHeader.Controls.Add(this.chkSelectAllOrig);
            this.pnlOrigHeader.Controls.Add(this.lblOriginalCount);
            this.pnlOrigHeader.Controls.Add(this.lblOrigTitle);
            this.pnlOrigHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOrigHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlOrigHeader.Name = "pnlOrigHeader";
            this.pnlOrigHeader.Size = new System.Drawing.Size(1150, 38);
            this.pnlOrigHeader.TabIndex = 0;

            // 
            // lblOrigTitle
            // 
            this.lblOrigTitle.AutoSize = true;
            this.lblOrigTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOrigTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblOrigTitle.Location = new System.Drawing.Point(12, 9);
            this.lblOrigTitle.Name = "lblOrigTitle";
            this.lblOrigTitle.Size = new System.Drawing.Size(183, 19);
            this.lblOrigTitle.TabIndex = 0;
            this.lblOrigTitle.Text = "📥 بيانات الطلاب المرفوعة من الإكسيل";

            // 
            // lblOriginalCount
            // 
            this.lblOriginalCount.AutoSize = true;
            this.lblOriginalCount.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblOriginalCount.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblOriginalCount.Location = new System.Drawing.Point(260, 10);
            this.lblOriginalCount.Name = "lblOriginalCount";
            this.lblOriginalCount.Size = new System.Drawing.Size(193, 17);
            this.lblOriginalCount.TabIndex = 1;
            this.lblOriginalCount.Text = "عدد الطلاب: 0 (المحدد: 0)";

            // 
            // chkSelectAllOrig
            // 
            this.chkSelectAllOrig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSelectAllOrig.AutoSize = true;
            this.chkSelectAllOrig.Checked = true;
            this.chkSelectAllOrig.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAllOrig.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.chkSelectAllOrig.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.chkSelectAllOrig.Location = new System.Drawing.Point(1015, 9);
            this.chkSelectAllOrig.Name = "chkSelectAllOrig";
            this.chkSelectAllOrig.Size = new System.Drawing.Size(95, 19);
            this.chkSelectAllOrig.TabIndex = 2;
            this.chkSelectAllOrig.Text = "تحديد الكل";
            this.chkSelectAllOrig.UseVisualStyleBackColor = true;
            this.chkSelectAllOrig.CheckedChanged += new System.EventHandler(this.chkSelectAllOrig_CheckedChanged);

            // 
            // pnlMiddleGenerateBar (Directly below dgvOriginal and above dgvGenerated)
            // 
            this.pnlMiddleGenerateBar.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.pnlMiddleGenerateBar.Controls.Add(this.btnCreateEmails);
            this.pnlMiddleGenerateBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlMiddleGenerateBar.Location = new System.Drawing.Point(6, 200);
            this.pnlMiddleGenerateBar.Name = "pnlMiddleGenerateBar";
            this.pnlMiddleGenerateBar.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlMiddleGenerateBar.Size = new System.Drawing.Size(1126, 44);
            this.pnlMiddleGenerateBar.TabIndex = 3;

            // 
            // btnCreateEmails (Horizontally across the middle of the form)
            // 
            this.btnCreateEmails.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnCreateEmails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateEmails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateEmails.Enabled = false;
            this.btnCreateEmails.FlatAppearance.BorderSize = 0;
            this.btnCreateEmails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateEmails.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnCreateEmails.ForeColor = System.Drawing.Color.White;
            this.btnCreateEmails.Location = new System.Drawing.Point(4, 5);
            this.btnCreateEmails.Name = "btnCreateEmails";
            this.btnCreateEmails.Size = new System.Drawing.Size(1118, 34);
            this.btnCreateEmails.TabIndex = 3;
            this.btnCreateEmails.Text = "⚡ إنشاء الإيميلات للطلاب المحددين ↓";
            this.btnCreateEmails.UseVisualStyleBackColor = false;
            this.btnCreateEmails.Click += new System.EventHandler(this.btnCreateEmails_Click);

            // 
            // dgvOriginal
            // 
            this.dgvOriginal.AllowUserToAddRows = false;
            this.dgvOriginal.AllowUserToDeleteRows = false;
            this.dgvOriginal.BackgroundColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvHeader1.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            dgvHeader1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            dgvHeader1.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvOriginal.ColumnHeadersDefaultCellStyle = dgvHeader1;
            this.dgvOriginal.ColumnHeadersHeight = 34;
            this.dgvOriginal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOriginal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrigSelect,
            this.colOrigIndex,
            this.colOrigId,
            this.colOrigFullName,
            this.colOrigCollege,
            this.colOrigDept,
            this.colOrigCode,
            this.colOrigStatus});
            dgvRow1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRow1.BackColor = System.Drawing.Color.White;
            dgvRow1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgvRow1.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dgvRow1.SelectionBackColor = System.Drawing.Color.FromArgb(224, 231, 255);
            dgvRow1.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvOriginal.DefaultCellStyle = dgvRow1;
            this.dgvOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOriginal.EnableHeadersVisualStyles = false;
            this.dgvOriginal.Location = new System.Drawing.Point(0, 42);
            this.dgvOriginal.Name = "dgvOriginal";
            this.dgvOriginal.RowHeadersVisible = false;
            this.dgvOriginal.RowTemplate.Height = 32;
            this.dgvOriginal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOriginal.Size = new System.Drawing.Size(1150, 218);
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
            this.colOrigId.HeaderText = "National ID / الرقم القومي";
            this.colOrigId.Name = "colOrigId";
            this.colOrigId.ReadOnly = true;
            this.colOrigId.Width = 155;

            // 
            // colOrigFullName
            // 
            this.colOrigFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colOrigFullName.HeaderText = "Full English Name";
            this.colOrigFullName.Name = "colOrigFullName";
            this.colOrigFullName.ReadOnly = true;

            // 
            // colOrigCollege
            // 
            this.colOrigCollege.HeaderText = "College / الكلية";
            this.colOrigCollege.Name = "colOrigCollege";
            this.colOrigCollege.ReadOnly = true;
            this.colOrigCollege.Width = 130;

            // 
            // colOrigDept
            // 
            this.colOrigDept.HeaderText = "Dept / القسم";
            this.colOrigDept.Name = "colOrigDept";
            this.colOrigDept.ReadOnly = true;
            this.colOrigDept.Width = 120;

            // 
            // colOrigCode
            // 
            this.colOrigCode.HeaderText = "Code / الكود";
            this.colOrigCode.Name = "colOrigCode";
            this.colOrigCode.ReadOnly = true;
            this.colOrigCode.Width = 100;

            // 
            // colOrigStatus
            // 
            this.colOrigStatus.HeaderText = "Status";
            this.colOrigStatus.Name = "colOrigStatus";
            this.colOrigStatus.ReadOnly = true;
            this.colOrigStatus.Width = 210;

            // 
            // pnlGenHeader
            // 
            this.pnlGenHeader.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlGenHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGenHeader.Controls.Add(this.btnClearGenerated);
            this.pnlGenHeader.Controls.Add(this.chkSelectAllGen);
            this.pnlGenHeader.Controls.Add(this.lblGeneratedCount);
            this.pnlGenHeader.Controls.Add(this.lblGenTitle);
            this.pnlGenHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGenHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlGenHeader.Name = "pnlGenHeader";
            this.pnlGenHeader.Size = new System.Drawing.Size(1150, 42);
            this.pnlGenHeader.TabIndex = 0;

            // 
            // lblGenTitle
            // 
            this.lblGenTitle.AutoSize = true;
            this.lblGenTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGenTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblGenTitle.Location = new System.Drawing.Point(12, 10);
            this.lblGenTitle.Name = "lblGenTitle";
            this.lblGenTitle.Size = new System.Drawing.Size(186, 19);
            this.lblGenTitle.TabIndex = 0;
            this.lblGenTitle.Text = "⚡ Generated Student Mails";

            // 
            // lblGeneratedCount
            // 
            this.lblGeneratedCount.AutoSize = true;
            this.lblGeneratedCount.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGeneratedCount.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.lblGeneratedCount.Location = new System.Drawing.Point(210, 11);
            this.lblGeneratedCount.Name = "lblGeneratedCount";
            this.lblGeneratedCount.Size = new System.Drawing.Size(206, 17);
            this.lblGeneratedCount.TabIndex = 1;
            this.lblGeneratedCount.Text = "Generated Emails: 0 (0 selected)";

            // 
            // chkSelectAllGen
            // 
            this.chkSelectAllGen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSelectAllGen.AutoSize = true;
            this.chkSelectAllGen.Checked = true;
            this.chkSelectAllGen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAllGen.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.chkSelectAllGen.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.chkSelectAllGen.Location = new System.Drawing.Point(875, 12);
            this.chkSelectAllGen.Name = "chkSelectAllGen";
            this.chkSelectAllGen.Size = new System.Drawing.Size(75, 19);
            this.chkSelectAllGen.TabIndex = 2;
            this.chkSelectAllGen.Text = "Select All";
            this.chkSelectAllGen.UseVisualStyleBackColor = true;
            this.chkSelectAllGen.CheckedChanged += new System.EventHandler(this.chkSelectAllGen_CheckedChanged);

            // 
            // btnClearGenerated
            // 
            this.btnClearGenerated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearGenerated.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnClearGenerated.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearGenerated.FlatAppearance.BorderSize = 0;
            this.btnClearGenerated.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearGenerated.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearGenerated.ForeColor = System.Drawing.Color.White;
            this.btnClearGenerated.Location = new System.Drawing.Point(970, 6);
            this.btnClearGenerated.Name = "btnClearGenerated";
            this.btnClearGenerated.Size = new System.Drawing.Size(165, 30);
            this.btnClearGenerated.TabIndex = 3;
            this.btnClearGenerated.Text = "🗑 Clear Mails List";
            this.btnClearGenerated.UseVisualStyleBackColor = false;
            this.btnClearGenerated.Click += new System.EventHandler(this.btnClearGenerated_Click);

            // 
            // dgvGenerated
            // 
            this.dgvGenerated.AllowUserToAddRows = false;
            this.dgvGenerated.AllowUserToDeleteRows = false;
            this.dgvGenerated.BackgroundColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvGenerated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvHeader2.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            dgvHeader2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            dgvHeader2.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvGenerated.ColumnHeadersDefaultCellStyle = dgvHeader2;
            this.dgvGenerated.ColumnHeadersHeight = 34;
            this.dgvGenerated.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGenerated.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGenSelect,
            this.colGenIndex,
            this.colGenId,
            this.colGenFullName,
            this.colGenFirstName,
            this.colGenLastName,
            this.colGenCollege,
            this.colGenDept,
            this.colGenCode,
            this.colGenEmail,
            this.colGenStatus});
            dgvRow2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRow2.BackColor = System.Drawing.Color.White;
            dgvRow2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgvRow2.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dgvRow2.SelectionBackColor = System.Drawing.Color.FromArgb(224, 231, 255);
            dgvRow2.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvGenerated.DefaultCellStyle = dgvRow2;
            this.dgvGenerated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGenerated.EnableHeadersVisualStyles = false;
            this.dgvGenerated.Location = new System.Drawing.Point(0, 42);
            this.dgvGenerated.Name = "dgvGenerated";
            this.dgvGenerated.RowHeadersVisible = false;
            this.dgvGenerated.RowTemplate.Height = 32;
            this.dgvGenerated.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGenerated.Size = new System.Drawing.Size(1150, 232);
            this.dgvGenerated.TabIndex = 1;
            this.dgvGenerated.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGenerated_CellValueChanged);
            this.dgvGenerated.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvGenerated_CurrentCellDirtyStateChanged);

            // 
            // colGenSelect
            // 
            this.colGenSelect.HeaderText = "Select";
            this.colGenSelect.Name = "colGenSelect";
            this.colGenSelect.Width = 55;

            // 
            // colGenIndex
            // 
            this.colGenIndex.HeaderText = "#";
            this.colGenIndex.Name = "colGenIndex";
            this.colGenIndex.ReadOnly = true;
            this.colGenIndex.Width = 45;

            // 
            // colGenId
            // 
            this.colGenId.HeaderText = "National ID (id)";
            this.colGenId.Name = "colGenId";
            this.colGenId.ReadOnly = true;
            this.colGenId.Width = 135;

            // 
            // colGenFullName
            // 
            this.colGenFullName.HeaderText = "Full English Name";
            this.colGenFullName.Name = "colGenFullName";
            this.colGenFullName.ReadOnly = true;
            this.colGenFullName.Width = 180;

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
            // colGenCollege
            // 
            this.colGenCollege.HeaderText = "College";
            this.colGenCollege.Name = "colGenCollege";
            this.colGenCollege.ReadOnly = true;
            this.colGenCollege.Width = 110;

            // 
            // colGenDept
            // 
            this.colGenDept.HeaderText = "Dept";
            this.colGenDept.Name = "colGenDept";
            this.colGenDept.ReadOnly = true;
            this.colGenDept.Width = 100;

            // 
            // colGenCode
            // 
            this.colGenCode.HeaderText = "Code";
            this.colGenCode.Name = "colGenCode";
            this.colGenCode.ReadOnly = true;
            this.colGenCode.Width = 90;

            // 
            // colGenEmail
            // 
            this.colGenEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGenEmail.HeaderText = "Generated Email Address";
            this.colGenEmail.Name = "colGenEmail";
            this.colGenEmail.ReadOnly = true;

            // 
            // colGenStatus
            // 
            this.colGenStatus.HeaderText = "Status";
            this.colGenStatus.Name = "colGenStatus";
            this.colGenStatus.ReadOnly = true;
            this.colGenStatus.Width = 110;

            // 
            // pnlBottomActions
            // 
            this.pnlBottomActions.BackColor = System.Drawing.Color.White;
            this.pnlBottomActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottomActions.Controls.Add(this.progressBar);
            this.pnlBottomActions.Controls.Add(this.btnSaveToDb);
            this.pnlBottomActions.Controls.Add(this.btnExportCsv);
            this.pnlBottomActions.Controls.Add(this.btnShowReport);
            this.pnlBottomActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomActions.Location = new System.Drawing.Point(0, 654);
            this.pnlBottomActions.Name = "pnlBottomActions";
            this.pnlBottomActions.Size = new System.Drawing.Size(1150, 56);
            this.pnlBottomActions.TabIndex = 3;

            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(16, 38);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(460, 10);
            this.progressBar.TabIndex = 3;
            this.progressBar.Visible = false;

            // 
            // btnShowReport
            // 
            this.btnShowReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowReport.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnShowReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowReport.FlatAppearance.BorderSize = 0;
            this.btnShowReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowReport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnShowReport.ForeColor = System.Drawing.Color.White;
            this.btnShowReport.Location = new System.Drawing.Point(495, 11);
            this.btnShowReport.Name = "btnShowReport";
            this.btnShowReport.Size = new System.Drawing.Size(170, 34);
            this.btnShowReport.TabIndex = 0;
            this.btnShowReport.Text = "🖨 Show Report";
            this.btnShowReport.UseVisualStyleBackColor = false;
            this.btnShowReport.Click += new System.EventHandler(this.btnShowReport_Click);

            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportCsv.BackColor = System.Drawing.Color.FromArgb(14, 116, 144);
            this.btnExportCsv.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportCsv.FlatAppearance.BorderSize = 0;
            this.btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCsv.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportCsv.ForeColor = System.Drawing.Color.White;
            this.btnExportCsv.Location = new System.Drawing.Point(680, 11);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(190, 34);
            this.btnExportCsv.TabIndex = 1;
            this.btnExportCsv.Text = "📊 Show CSV (Export)";
            this.btnExportCsv.UseVisualStyleBackColor = false;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);

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
            this.btnSaveToDb.Location = new System.Drawing.Point(885, 9);
            this.btnSaveToDb.Name = "btnSaveToDb";
            this.btnSaveToDb.Size = new System.Drawing.Size(250, 38);
            this.btnSaveToDb.TabIndex = 2;
            this.btnSaveToDb.Text = "💾 حفظ الإيميلات (Save)";
            this.btnSaveToDb.UseVisualStyleBackColor = false;
            this.btnSaveToDb.Click += new System.EventHandler(this.btnSaveToDb_Click);

            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusUser,
            this.lblStatusSeparator1,
            this.lblStatusTable,
            this.lblStatusSeparator2,
            this.lblStatusAction});
            this.statusStrip.Location = new System.Drawing.Point(0, 710);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1150, 24);
            this.statusStrip.TabIndex = 4;

            // 
            // lblStatusUser
            // 
            this.lblStatusUser.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusUser.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblStatusUser.Name = "lblStatusUser";
            this.lblStatusUser.Size = new System.Drawing.Size(147, 19);
            this.lblStatusUser.Text = "👤 Logged in User: [User]";

            // 
            // lblStatusSeparator1
            // 
            this.lblStatusSeparator1.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblStatusSeparator1.Name = "lblStatusSeparator1";
            this.lblStatusSeparator1.Size = new System.Drawing.Size(10, 19);
            this.lblStatusSeparator1.Text = "|";

            // 
            // lblStatusTable
            // 
            this.lblStatusTable.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblStatusTable.Name = "lblStatusTable";
            this.lblStatusTable.Size = new System.Drawing.Size(155, 19);
            this.lblStatusTable.Text = "🎯 Target: dbo.[mails]";

            // 
            // lblStatusSeparator2
            // 
            this.lblStatusSeparator2.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblStatusSeparator2.Name = "lblStatusSeparator2";
            this.lblStatusSeparator2.Size = new System.Drawing.Size(10, 19);
            this.lblStatusSeparator2.Text = "|";

            // 
            // lblStatusAction
            // 
            this.lblStatusAction.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblStatusAction.Name = "lblStatusAction";
            this.lblStatusAction.Size = new System.Drawing.Size(130, 19);
            this.lblStatusAction.Text = "في انتظار رفع الملف...";

            // 
            // frmGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(1150, 734);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.grpStep1);
            this.Controls.Add(this.pnlTopBar);
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "frmGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Email Generator Studio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmGenerator_Load);
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlExcelControls.ResumeLayout(false);
            this.pnlExcelControls.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOriginal)).EndInit();
            this.pnlOrigHeader.ResumeLayout(false);
            this.pnlOrigHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGenerated)).EndInit();
            this.pnlGenHeader.ResumeLayout(false);
            this.pnlGenHeader.PerformLayout();
            this.pnlBottomActions.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
