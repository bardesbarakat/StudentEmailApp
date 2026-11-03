using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

        // Tracks whether there are newly generated (unsaved) emails in dgvGenerated.
        // After a successful save, dgvGenerated is kept populated for Export/Report,
        // while btnSaveToDb is disabled to prevent duplicate database inserts.
        private bool _hasUnsavedGeneratedEmails = false;

        // Print Report pagination tracking
        private int _printRowIndex = 0;
        private int _printPageNumber = 1;
        private List<DataGridViewRow> _printRows = new();

        public frmGenerator(string targetTable, string targetTitle)
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _targetTable = string.IsNullOrWhiteSpace(targetTable) ? "mails" : targetTable;
            _targetTitle = string.IsNullOrWhiteSpace(targetTitle) ? "Students" : targetTitle;
        }

        private void frmGenerator_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigureFoolProofUiAndColumns();

                lblTargetTitle.Text = $"نظام إنشاء وحفظ الإيميلات الجامعية — {_targetTitle}";
                lblTargetTableInfo.Text = $"الجدول المستهدف: dbo.[{_targetTable}]";

                // Initialize Footer StatusStrip
                lblStatusUser.Text = $"👤 المستخدم: {UserSession.CurrentUsername}";
                lblStatusTable.Text = $"🎯 الجدول: dbo.[{_targetTable}]";
                lblStatusAction.Text = "في انتظار رفع الملف...";

                UpdateOriginalSummary();
                UpdateGeneratedSummary();
                UpdateUiState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region UI/UX Configuration & Fool-Proof State Management

        #region Wizard Navigation (View 1: Steps 1+2  ⇄  View 2: Step 3)

        private Panel pnlView1 = null!;   // الخطوة 1 (رفع الملف) + الخطوة 2 (إنشاء الإيميلات)
        private Panel pnlView2 = null!;   // الخطوة 3 (الحفظ)
        private Button btnBackToStep1 = null!;
        private bool _wizardArranged;

        /// <summary>
        /// Splits the screen into two full-size views so each DataGridView gets the whole height.
        /// Existing controls are only re-parented; their names, events and data stay untouched.
        /// </summary>
        private void ArrangeWizardViews()
        {
            if (_wizardArranged) return;
            _wizardArranged = true;

            SuspendLayout();

            pnlView1 = new Panel { Name = "pnlView1", Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8, 4, 8, 4) };
            pnlView2 = new Panel { Name = "pnlView2", Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8, 4, 8, 4) };

            // ---- View 2 back bar (top of the view) ----
            var pnlBackBar = new Panel
            {
                Dock = DockStyle.Top, Height = 48, BackColor = Color.White, RightToLeft = RightToLeft.Yes,
                Padding = new Padding(4, 6, 4, 6)
            };
            btnBackToStep1 = new Button
            {
                Text = "→ رجوع للخطوة السابقة",
                Dock = DockStyle.Right, // physically on the right edge (RTL)
                Width = 220,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(71, 85, 105),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBackToStep1.FlatAppearance.BorderSize = 0;
            btnBackToStep1.Click += (s, e) => ShowWizardView(1); // data is kept: only visibility changes
            pnlBackBar.Controls.Add(btnBackToStep1);

            // ---- Re-parent: Fill control is added FIRST (index 0 = docked last), Top controls after ----
            splitMain.Panel1.Controls.Remove(grpStep2);
            splitMain.Panel2.Controls.Remove(grpStep3);
            Controls.Remove(grpStep1);
            Controls.Remove(splitMain);

            grpStep2.Dock = DockStyle.Fill;   // dgvOriginal (Fill) now uses the full height
            grpStep3.Dock = DockStyle.Fill;   // dgvGenerated (Fill) now uses the full height

            pnlView1.Controls.Add(grpStep2);
            pnlView1.Controls.Add(grpStep1);
            pnlView2.Controls.Add(grpStep3);
            pnlView2.Controls.Add(pnlBackBar);

            Controls.Add(pnlView2);
            Controls.Add(pnlView1);
            pnlView2.BringToFront();   // Fill panels must be at index 0 so they respect
            pnlView1.BringToFront();   // the top bar and the status strip

            pnlView1.Visible = true;
            pnlView2.Visible = false;

            ResumeLayout(true);
        }

        private void ShowWizardView(int step)
        {
            bool first = step == 1;
            // Show the new view before hiding the old one to avoid flicker
            (first ? pnlView1 : pnlView2).Visible = true;
            (first ? pnlView2 : pnlView1).Visible = false;
            (first ? pnlView1 : pnlView2).BringToFront();

            lblStatusAction.Text = first
                ? "الخطوة 1-2: ارفع الملف ثم أنشئ الإيميلات"
                : "الخطوة 3: راجع الإيميلات ثم احفظها في قاعدة البيانات";
        }

        #endregion

        private bool _step1Arranged;

        /// <summary>
        /// "الخطوة 1: رفع الملف" laid out Right-to-Left. Starting at the RIGHT edge:
        /// ملف الإكسيل: → [file path] → اختر → رفع الملف → مسح القائمة → Domain (opt): → [domain] → LEFT edge.
        /// A one-row RTL TableLayoutPanel places column 0 at the right, the path column stretches
        /// (Percent 100) and every other column keeps its width, so buttons stay aligned when maximized.
        /// frmGenerator serves both the Undergraduates and Postgraduates forms, so this applies to both.
        /// </summary>
        private void ArrangeStep1Rtl()
        {
            if (_step1Arranged) return;
            _step1Arranged = true;

            btnBrowse.Text = "📁 اختر";

            grpStep1.RightToLeft = RightToLeft.Yes;
            pnlExcelControls.RightToLeft = RightToLeft.Yes;

            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 1,
                RightToLeft = RightToLeft.Yes,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));          // 0: ملف الإكسيل:
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));     // 1: file path (stretches)
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));          // 2: اختر
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));          // 3: رفع الملف
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));          // 4: مسح القائمة
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));          // 5: Domain (opt):
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));    // 6: domain textbox

            // Labels: vertically centered, right-aligned text
            foreach (var lbl in new[] { lblExcelPath, lblDomain })
            {
                lbl.Anchor = AnchorStyles.None;
                lbl.AutoSize = true;
                lbl.Margin = new Padding(6, 0, 6, 0);
            }

            // Path textbox: stretch horizontally with the window (Latin path => keep LTR text flow)
            txtExcelPath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtExcelPath.RightToLeft = RightToLeft.No;
            txtExcelPath.Margin = new Padding(4, 0, 4, 0);

            // Buttons: fixed width, vertically centered => perfectly aligned row
            foreach (var btn in new[] { btnBrowse, btnLoadSheet, btnClearOriginal })
            {
                btn.Anchor = AnchorStyles.None;
                btn.AutoSize = false;
                btn.Height = 30;
                btn.Margin = new Padding(4, 0, 4, 0);
            }
            btnBrowse.Width = 100;
            btnLoadSheet.Width = 130;
            btnClearOriginal.Width = 140;

            txtDomain.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDomain.RightToLeft = RightToLeft.No;
            txtDomain.Margin = new Padding(4, 0, 4, 0);

            pnlExcelControls.Controls.Clear();
            tlp.Controls.Add(lblExcelPath, 0, 0);
            tlp.Controls.Add(txtExcelPath, 1, 0);
            tlp.Controls.Add(btnBrowse, 2, 0);
            tlp.Controls.Add(btnLoadSheet, 3, 0);
            tlp.Controls.Add(btnClearOriginal, 4, 0);
            tlp.Controls.Add(lblDomain, 5, 0);
            tlp.Controls.Add(txtDomain, 6, 0);
            pnlExcelControls.Controls.Add(tlp);
        }

        /// <summary>
        /// Configures GroupBoxes ("الخطوة 1: رفع الملف", "الخطوة 2: إنشاء الإيميلات", "الخطوة 3: الحفظ"),
        /// places the "Generate Emails" button horizontally across the middle of the form directly below
        /// dgvOriginal and above dgvGenerated, and ensures all required dbo.mails columns exist.
        /// </summary>
        private void ConfigureFoolProofUiAndColumns()
        {
            if (grpStep1 != null) grpStep1.Text = "الخطوة 1: رفع الملف";
            if (grpStep2 != null) grpStep2.Text = "الخطوة 2: إنشاء الإيميلات";
            if (grpStep3 != null) grpStep3.Text = "الخطوة 3: الحفظ";

            lblExcelPath.Text = "ملف الإكسيل:";
            btnBrowse.Text = "📁 اختر ملف...";
            btnLoadSheet.Text = "📥 رفع الملف";
            btnClearOriginal.Text = "🗑 مسح القائمة";
            lblOrigTitle.Text = "📥 بيانات الطلاب المرفوعة من الإكسيل";
            chkSelectAllOrig.Text = "تحديد الكل";
            btnCreateEmails.Text = "⚡ إنشاء الإيميلات للطلاب المحددين ←";

            lblGenTitle.Text = "⚡ الإيميلات الجاهزة للحفظ";
            chkSelectAllGen.Text = "تحديد الكل";
            btnClearGenerated.Text = "🗑 مسح القائمة";
            btnSaveToDb.Text = "💾 حفظ في قاعدة البيانات";
            btnExportCsv.Text = "📊 تصدير CSV";
            btnShowReport.Text = "🖨 طباعة تقرير";
            btnBack.Text = "← رجوع";

            if (string.IsNullOrWhiteSpace(txtDomain.Text))
            {
                txtDomain.Text = "@fayoum.edu.eg";
            }

            ArrangeStep1Rtl();
            ArrangeWizardViews();

            // Ensure "Generate Emails" button is placed horizontally across the middle of the form
            // directly below dgvOriginal and above dgvGenerated
            if (pnlMiddleGenerateBar != null && btnCreateEmails.Parent != pnlMiddleGenerateBar)
            {
                pnlMiddleGenerateBar.Controls.Add(btnCreateEmails);
                btnCreateEmails.Dock = DockStyle.Fill;
            }

            // Configure dgvOriginal columns
            colOrigId.HeaderText = "الرقم القومي (id)";
            colOrigFullName.HeaderText = "الاسم باللغة الانجليزية";
            colOrigStatus.HeaderText = "حالة التسجيل";

            EnsureColumn(dgvOriginal, "colOrigArName", "الاسم باللغة العربية (ar_name)", 160, "colOrigFullName");
            EnsureColumn(dgvOriginal, "colOrigCollege", "الكلية (college)", 120, "colOrigStatus");
            EnsureColumn(dgvOriginal, "colOrigDept", "الشعبة (dept)", 110, "colOrigStatus");
            EnsureColumn(dgvOriginal, "colOrigCode", "الكود (code)", 90, "colOrigStatus");
            EnsureColumn(dgvOriginal, "colOrigStage", "الفرقة (stage)", 90, "colOrigStatus");
            EnsureColumn(dgvOriginal, "colOrigType", "طبيعة الدراسة (type)", 110, "colOrigStatus");
            EnsureColumn(dgvOriginal, "colOrigStudentStat", "الحالة (stat)", 95, "colOrigStatus");

            // Configure dgvGenerated columns
            colGenId.HeaderText = "الرقم القومي (id)";
            colGenFullName.HeaderText = "الاسم باللغة الانجليزية";
            colGenFirstName.HeaderText = "First Name (first_name)";
            colGenLastName.HeaderText = "Last Name (last_name)";
            colGenEmail.HeaderText = "الإيميل الجامعي (mail)";
            colGenStatus.HeaderText = "حالة الحفظ";

            EnsureColumn(dgvGenerated, "colGenArName", "الاسم باللغة العربية (ar_name)", 150, "colGenFullName");
            EnsureColumn(dgvGenerated, "colGenCollege", "الكلية (college)", 110, "colGenEmail");
            EnsureColumn(dgvGenerated, "colGenDept", "الشعبة (dept)", 100, "colGenEmail");
            EnsureColumn(dgvGenerated, "colGenCode", "الكود (code)", 85, "colGenEmail");
            EnsureColumn(dgvGenerated, "colGenStage", "الفرقة (stage)", 85, "colGenEmail");
            EnsureColumn(dgvGenerated, "colGenType", "طبيعة الدراسة (type)", 100, "colGenEmail");
            EnsureColumn(dgvGenerated, "colGenStudentStat", "الحالة (stat)", 85, "colGenEmail");
        }

        private static void EnsureColumn(DataGridView dgv, string name, string headerText, int width, string insertBeforeColumn)
        {
            if (dgv.Columns.Contains(name))
            {
                dgv.Columns[name]!.HeaderText = headerText;
                return;
            }

            int insertIdx = dgv.Columns.Contains(insertBeforeColumn)
                ? dgv.Columns[insertBeforeColumn]!.Index
                : dgv.Columns.Count;

            dgv.Columns.Insert(insertIdx, new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                ReadOnly = true,
                Width = width
            });
        }

        /// <summary>
        /// Centralizes fool-proof state management:
        /// - "Generate" button disabled until an Excel file is loaded.
        /// - "Save" button disabled until emails are generated, and disabled after saving
        ///   while keeping Export/Report buttons active.
        /// </summary>
        private void UpdateUiState()
        {
            bool hasLoadedStudents = dgvOriginal.Rows.Count > 0;
            bool hasSelectedOriginal = false;
            foreach (DataGridViewRow row in dgvOriginal.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colOrigSelect"].Value ?? false))
                {
                    hasSelectedOriginal = true;
                    break;
                }
            }

            bool hasGeneratedEmails = dgvGenerated.Rows.Count > 0;
            bool hasSelectedGenerated = GetSelectedGeneratedRows().Count > 0;

            btnCreateEmails.Enabled = hasLoadedStudents && hasSelectedOriginal;
            btnClearOriginal.Enabled = hasLoadedStudents;

            // Save button is only enabled when there are unsaved generated emails selected
            btnSaveToDb.Enabled = hasGeneratedEmails && hasSelectedGenerated && _hasUnsavedGeneratedEmails;

            // Export & Report remain enabled as long as dgvGenerated has selected rows (even after saving)
            btnExportCsv.Enabled = hasGeneratedEmails && hasSelectedGenerated;
            btnShowReport.Enabled = hasGeneratedEmails && hasSelectedGenerated;
            btnClearGenerated.Enabled = hasGeneratedEmails;
        }

        #endregion

        #region 1. الخطوة 1: رفع الملف (Excel Loading with UseHeaderRow = true & Exact Header Mapping)

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using var ofd = new OpenFileDialog
                {
                    Filter = "Excel & CSV Files (*.xlsx;*.xls;*.csv)|*.xlsx;*.xls;*.csv|Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    Title = "اختر ملف بيانات الطلاب (Excel / CSV)"
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtExcelPath.Text = ofd.FileName;
                    lblStatusAction.Text = $"تم اختيار الملف: {Path.GetFileName(ofd.FileName)} — اضغط 'رفع الملف' للبدء.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadSheet_Click(object sender, EventArgs e)
        {
            string filePath = txtExcelPath.Text.Trim();

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("يرجى اختيار ملف إكسيل صحيح أولاً.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConfigureFoolProofUiAndColumns();

                btnLoadSheet.Enabled = false;
                btnLoadSheet.Text = "جاري الرفع...";
                lblStatusAction.Text = "جاري قراءة ملف الإكسيل...";
                dgvOriginal.Rows.Clear();

                int importedCount = 0;

                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Requirement 1: Configure ExcelDataReader to treat the first row as column headers
                    // so the header row ("الاسم باللغة العربية", etc.) is NEVER loaded as student data.
                    var dataSetConfig = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };

                    var result = reader.AsDataSet(dataSetConfig);
                    if (result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("ملف الإكسيل المختار فارغ ولا يحتوي على بيانات طلاب.", "ملف فارغ",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lblStatusAction.Text = "في انتظار رفع الملف...";
                        return;
                    }

                    DataTable dt = result.Tables[0];

                    // Resolve exact Arabic column headers to their column indices in dt
                    var colMap = ResolveExactExcelColumns(dt.Columns);

                    dgvOriginal.SuspendLayout();

                    // Start loading actual student data from dt.Rows (which starts at the 2nd row of the Excel file)
                    foreach (DataRow row in dt.Rows)
                    {
                        string arName     = GetCellString(row, colMap.ArNameCol, isIdentifier: false);
                        string enFullName = GetCellString(row, colMap.EnNameCol, isIdentifier: false);
                        string nationalId = GetCellString(row, colMap.NationalIdCol, isIdentifier: true);
                        string college    = GetCellString(row, colMap.CollegeCol, isIdentifier: false);
                        string dept       = GetCellString(row, colMap.DeptCol, isIdentifier: false);
                        string code       = GetCellString(row, colMap.CodeCol, isIdentifier: true);
                        string stage      = GetCellString(row, colMap.StageCol, isIdentifier: false);
                        string studyType  = GetCellString(row, colMap.TypeCol, isIdentifier: false);
                        string stat       = GetCellString(row, colMap.StatCol, isIdentifier: false);

                        // Skip completely empty rows or accidental repeated header rows
                        if (string.IsNullOrWhiteSpace(enFullName) &&
                            string.IsNullOrWhiteSpace(arName) &&
                            string.IsNullOrWhiteSpace(nationalId))
                        {
                            continue;
                        }

                        if (IsHeaderTextRow(arName, enFullName, nationalId))
                        {
                            continue;
                        }

                        importedCount++;
                        int rowIdx = dgvOriginal.Rows.Add();
                        DataGridViewRow dgvRow = dgvOriginal.Rows[rowIdx];

                        dgvRow.Cells["colOrigSelect"].Value      = true;
                        dgvRow.Cells["colOrigIndex"].Value       = importedCount;
                        dgvRow.Cells["colOrigId"].Value          = nationalId; // "الرقم القومي" -> id
                        dgvRow.Cells["colOrigArName"].Value      = arName;     // "الاسم باللغة العربية" -> ar_name
                        dgvRow.Cells["colOrigFullName"].Value    = enFullName; // "الاسم باللغة الانجليزية" -> first_name + last_name
                        dgvRow.Cells["colOrigCollege"].Value     = college;    // "الكلية" -> college
                        dgvRow.Cells["colOrigDept"].Value        = dept;       // "الشعبة" -> dept
                        dgvRow.Cells["colOrigCode"].Value        = code;       // "الكود" -> code
                        dgvRow.Cells["colOrigStage"].Value       = stage;      // "الفرقة" -> stage
                        dgvRow.Cells["colOrigType"].Value        = studyType;  // "طبيعة الدراسة" -> type
                        dgvRow.Cells["colOrigStudentStat"].Value = stat;       // "الحالة" -> stat
                        dgvRow.Cells["colOrigStatus"].Value      = "جاهز";
                    }

                    dgvOriginal.ResumeLayout();
                }

                chkSelectAllOrig.Checked = importedCount > 0;
                UpdateOriginalSummary();
                UpdateUiState();

                lblStatusAction.Text = "جاهز لإنشاء الإيميلات...";

                MessageBox.Show(
                    $"تم رفع بيانات الإكسيل بنجاح. عدد الطلاب: {importedCount}",
                    "تم رفع الملف",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatusAction.Text = "في انتظار رفع الملف...";
            }
            finally
            {
                btnLoadSheet.Enabled = true;
                btnLoadSheet.Text = "📥 رفع الملف";
                UpdateUiState();
            }
        }

        private static bool IsHeaderTextRow(string arName, string enFullName, string nationalId)
        {
            string normAr = NormalizeArabicHeader(arName);
            string normEn = NormalizeArabicHeader(enFullName);
            string normId = NormalizeArabicHeader(nationalId);

            return normAr.Contains("الاسم باللغه العربيه", StringComparison.OrdinalIgnoreCase) ||
                   normEn.Contains("الاسم باللغه الانجليزيه", StringComparison.OrdinalIgnoreCase) ||
                   normId.Contains("الرقم القومي", StringComparison.OrdinalIgnoreCase) ||
                   normEn.Equals("Full English Name", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Resolves the exact Arabic Excel column headers to their DataTable column indices:
        /// - "الاسم باللغة العربية" -> ArNameCol (ar_name)
        /// - "الاسم باللغة الانجليزية" -> EnNameCol (first_name + last_name)
        /// - "الرقم القومي" -> NationalIdCol (id)
        /// - "الكلية" -> CollegeCol (college)
        /// - "الشعبة" -> DeptCol (dept)
        /// - "الكود" -> CodeCol (code)
        /// - "الفرقة" -> StageCol (stage)
        /// - "طبيعة الدراسة" -> TypeCol (type)
        /// - "الحالة" -> StatCol (stat)
        /// </summary>
        private static (
            int ArNameCol,
            int EnNameCol,
            int NationalIdCol,
            int CollegeCol,
            int DeptCol,
            int CodeCol,
            int StageCol,
            int TypeCol,
            int StatCol
        ) ResolveExactExcelColumns(DataColumnCollection columns)
        {
            int arNameCol     = -1;
            int enNameCol     = -1;
            int nationalIdCol = -1;
            int collegeCol    = -1;
            int deptCol       = -1;
            int codeCol       = -1;
            int stageCol      = -1;
            int typeCol       = -1;
            int statCol       = -1;

            for (int c = 0; c < columns.Count; c++)
            {
                string rawHeader = columns[c].ColumnName?.Trim() ?? "";
                string norm = NormalizeArabicHeader(rawHeader);

                if (string.IsNullOrEmpty(norm))
                    continue;

                // 1. "الاسم باللغة العربية" -> ar_name
                if (norm.Contains("الاسم باللغه العربيه", StringComparison.OrdinalIgnoreCase) ||
                    norm.Contains("الاسم بالعربيه", StringComparison.OrdinalIgnoreCase) ||
                    norm.Contains("عربي", StringComparison.OrdinalIgnoreCase) ||
                    norm.Equals("ar_name", StringComparison.OrdinalIgnoreCase))
                {
                    arNameCol = c;
                }
                // 2. "الاسم باللغة الانجليزية" -> first_name & last_name
                else if (norm.Contains("الاسم باللغه الانجليزيه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("الاسم بالانجليزيه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("انجليزي", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("english", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("full name", StringComparison.OrdinalIgnoreCase) ||
                         norm.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    enNameCol = c;
                }
                // 3. "الرقم القومي" -> id
                else if (norm.Contains("الرقم القومي", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("قومي", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("national id", StringComparison.OrdinalIgnoreCase) ||
                         norm.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    nationalIdCol = c;
                }
                // 4. "الكلية" -> college
                else if (norm.Contains("الكليه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Equals("كليه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("college", StringComparison.OrdinalIgnoreCase))
                {
                    collegeCol = c;
                }
                // 5. "الشعبة" -> dept
                else if (norm.Contains("الشعبه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("شعبه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("القسم", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("dept", StringComparison.OrdinalIgnoreCase))
                {
                    deptCol = c;
                }
                // 6. "الكود" -> code
                else if (norm.Contains("الكود", StringComparison.OrdinalIgnoreCase) ||
                         norm.Equals("كود", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("كود الطالب", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("code", StringComparison.OrdinalIgnoreCase))
                {
                    codeCol = c;
                }
                // 7. "الفرقة" -> stage
                else if (norm.Contains("الفرقه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("فرقه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("stage", StringComparison.OrdinalIgnoreCase))
                {
                    stageCol = c;
                }
                // 8. "طبيعة الدراسة" -> type
                else if (norm.Contains("طبيعه الدراسه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("طبيعه", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("type", StringComparison.OrdinalIgnoreCase))
                {
                    typeCol = c;
                }
                // 9. "الحالة" -> stat
                else if (norm.Contains("الحاله", StringComparison.OrdinalIgnoreCase) ||
                         norm.Equals("حاله", StringComparison.OrdinalIgnoreCase) ||
                         norm.Contains("stat", StringComparison.OrdinalIgnoreCase))
                {
                    statCol = c;
                }
            }

            // Positional fallback for simple 2-column files
            if (nationalIdCol == -1 && columns.Count >= 1 && arNameCol != 0 && enNameCol != 0)
                nationalIdCol = 0;
            if (enNameCol == -1 && columns.Count >= 2 && nationalIdCol != 1 && arNameCol != 1)
                enNameCol = 1;

            return (arNameCol, enNameCol, nationalIdCol, collegeCol, deptCol, codeCol, stageCol, typeCol, statCol);
        }

        /// <summary>
        /// Normalizes Arabic characters (أ/إ/آ -> ا, ة -> ه, ى -> ي) for reliable header matching.
        /// </summary>
        private static string NormalizeArabicHeader(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return input.Trim()
                .Replace('أ', 'ا')
                .Replace('إ', 'ا')
                .Replace('آ', 'ا')
                .Replace('ة', 'ه')
                .Replace('ى', 'ي');
        }

        /// <summary>
        /// Requirement 2: Reads "الاسم باللغة الانجليزية", splits it so the very first word becomes
        /// first_name and all remaining words become last_name, and extracts the 2-letter email prefix
        /// directly from this first_name and last_name combination.
        /// </summary>
        private static (string prefix, string firstName, string lastName) SplitEnglishNameAndBuildPrefix(string fullEnglishName)
        {
            if (string.IsNullOrWhiteSpace(fullEnglishName))
                return ("xx", "", "");

            string[] words = fullEnglishName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
                return ("xx", "", "");

            string firstName = words[0];
            string lastName = words.Length > 1
                ? string.Join(" ", words.Skip(1))
                : "";

            char firstInitial = 'x';
            foreach (char c in firstName)
            {
                if (char.IsLetter(c))
                {
                    firstInitial = char.ToLowerInvariant(c);
                    break;
                }
            }

            char secondInitial = 'x';
            if (!string.IsNullOrEmpty(lastName))
            {
                foreach (char c in lastName)
                {
                    if (char.IsLetter(c))
                    {
                        secondInitial = char.ToLowerInvariant(c);
                        break;
                    }
                }
            }
            else if (firstName.Length > 1)
            {
                for (int i = 1; i < firstName.Length; i++)
                {
                    if (char.IsLetter(firstName[i]))
                    {
                        secondInitial = char.ToLowerInvariant(firstName[i]);
                        break;
                    }
                }
            }

            string prefix = $"{firstInitial}{secondInitial}";
            return (prefix, firstName, lastName);
        }

        /// <summary>
        /// Safely extracts a string value from a DataRow cell, preventing scientific notation on 14-digit National IDs.
        /// </summary>
        private static string GetCellString(DataRow row, int colIndex, bool isIdentifier)
        {
            if (colIndex < 0 || colIndex >= row.ItemArray.Length)
                return string.Empty;

            object? val = row[colIndex];
            if (val == null || val == DBNull.Value)
                return string.Empty;

            if (isIdentifier)
            {
                if (val is double d)
                    return d.ToString("0", CultureInfo.InvariantCulture).Trim();
                if (val is decimal dec)
                    return dec.ToString("0", CultureInfo.InvariantCulture).Trim();
            }

            return val.ToString()?.Trim() ?? string.Empty;
        }

        private void btnClearOriginal_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOriginal.Rows.Count == 0) return;

                var confirm = MessageBox.Show(
                    "هل أنت متأكد من مسح جميع بيانات الطلاب المرفوعة؟",
                    "تأكيد المسح",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    dgvOriginal.Rows.Clear();
                    txtExcelPath.Clear();
                    UpdateOriginalSummary();
                    UpdateUiState();
                    lblStatusAction.Text = "في انتظار رفع الملف...";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 2. الخطوة 2: إنشاء الإيميلات (High-Performance National ID Validation & Email Generation)

        private void btnCreateEmails_Click(object sender, EventArgs e)
        {
            if (dgvOriginal.Rows.Count == 0)
            {
                MessageBox.Show("يرجى رفع ملف الإكسيل أولاً.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ConfigureFoolProofUiAndColumns();

            const string domainSuffix = "@fayoum.edu.eg";
            int generatedCount = 0;
            int duplicateCount = 0;

            btnCreateEmails.Enabled = false;
            btnCreateEmails.Text = "جاري الفحص والإنشاء...";
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = dgvOriginal.Rows.Count;
            lblStatusAction.Text = "جاري فحص الأرقام القومية وإنشاء الإيميلات...";

            try
            {
                string validatedTable = (_targetTable.Equals("PostGradMails", StringComparison.OrdinalIgnoreCase))
                    ? "PostGradMails"
                    : "mails";

                // =========================================================================
                // 1. HIGH-PERFORMANCE PRE-FETCH:
                //    (A) Single SQL query to fetch all existing National IDs (id) -> HashSet<string>
                //    (B) Single SQL query to fetch highest email sequences per 2-letter prefix
                // =========================================================================
                var existingNationalIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var maxEmailDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();

                    // (A) Pre-fetch all existing National IDs from the 'id' column in dbo.mails
                    string nationalIdQuery = $@"
                        SELECT LTRIM(RTRIM(id)) AS NationalId 
                        FROM dbo.[{validatedTable}] 
                        WHERE id IS NOT NULL AND LTRIM(RTRIM(id)) <> ''";

                    using (var cmdIds = new SqlCommand(nationalIdQuery, conn))
                    using (var readerIds = cmdIds.ExecuteReader())
                    {
                        while (readerIds.Read())
                        {
                            string existingId = readerIds["NationalId"]?.ToString()?.Trim() ?? "";
                            if (!string.IsNullOrEmpty(existingId))
                            {
                                existingNationalIds.Add(existingId);
                            }
                        }
                    }

                    // (B) Pre-fetch highest existing email sequence for each 2-letter prefix
                    string prefixQuery = $@"
                        SELECT 
                            LOWER(SUBSTRING(mail, 1, 2)) AS Prefix, 
                            MAX(mail) AS MaxEmail 
                        FROM dbo.[{validatedTable}] 
                        WHERE LEN(mail) >= 2 
                        GROUP BY LOWER(SUBSTRING(mail, 1, 2))";

                    using (var cmdPrefixes = new SqlCommand(prefixQuery, conn))
                    using (var readerPrefixes = cmdPrefixes.ExecuteReader())
                    {
                        while (readerPrefixes.Read())
                        {
                            string prefix = readerPrefixes["Prefix"]?.ToString()?.Trim().ToLowerInvariant() ?? "";
                            string maxEmail = readerPrefixes["MaxEmail"]?.ToString()?.Trim() ?? "";

                            if (!string.IsNullOrEmpty(prefix))
                            {
                                maxEmailDict[prefix] = maxEmail;
                            }
                        }
                    }
                }

                // =========================================================================
                // 2. IN-MEMORY VALIDATION & EMAIL GENERATION LOOP (O(1) HashSet Lookup)
                // =========================================================================
                dgvOriginal.SuspendLayout();
                dgvGenerated.SuspendLayout();

                foreach (DataGridViewRow rawRow in dgvOriginal.Rows)
                {
                    progressBar.Value++;

                    bool isChecked = Convert.ToBoolean(rawRow.Cells["colOrigSelect"].Value ?? false);
                    if (!isChecked)
                        continue;

                    string nationalId = rawRow.Cells["colOrigId"].Value?.ToString()?.Trim() ?? "";
                    string arName     = rawRow.Cells["colOrigArName"].Value?.ToString()?.Trim() ?? "";
                    string enFullName = rawRow.Cells["colOrigFullName"].Value?.ToString()?.Trim() ?? "";
                    string college    = rawRow.Cells["colOrigCollege"].Value?.ToString()?.Trim() ?? "";
                    string dept       = rawRow.Cells["colOrigDept"].Value?.ToString()?.Trim() ?? "";
                    string code       = rawRow.Cells["colOrigCode"].Value?.ToString()?.Trim() ?? "";
                    string stage      = rawRow.Cells["colOrigStage"].Value?.ToString()?.Trim() ?? "";
                    string studyType  = rawRow.Cells["colOrigType"].Value?.ToString()?.Trim() ?? "";
                    string stat       = rawRow.Cells["colOrigStudentStat"].Value?.ToString()?.Trim() ?? "";

                    if (string.IsNullOrWhiteSpace(enFullName))
                        continue;

                    // ---------------------------------------------------------------------
                    // SKIP DUPLICATES: Check if "الرقم القومي" exists in HashSet (O(1) lookup)
                    // ---------------------------------------------------------------------
                    if (!string.IsNullOrWhiteSpace(nationalId) && existingNationalIds.Contains(nationalId))
                    {
                        // Skip email generation, do NOT add to dgvGenerated.
                        // Visually flag the row in dgvOriginal with LightCoral & status "مسجل مسبقاً"
                        duplicateCount++;
                        rawRow.DefaultCellStyle.BackColor = Color.LightCoral;
                        rawRow.DefaultCellStyle.ForeColor = Color.White;
                        rawRow.Cells["colOrigStatus"].Value = "مسجل مسبقاً";
                        continue;
                    }

                    // Add new National ID to HashSet immediately to also prevent duplicates within the same sheet
                    if (!string.IsNullOrWhiteSpace(nationalId))
                    {
                        existingNationalIds.Add(nationalId);
                    }

                    // Requirement 2: Split "الاسم باللغة الانجليزية" into first_name (1st word) and last_name (remaining words)
                    // and generate the email address using this first_name and last_name combination
                    var (prefix, firstName, lastName) = SplitEnglishNameAndBuildPrefix(enFullName);

                    string generatedEmail;

                    // Calculate next sequence using the in-memory Dictionary
                    if (!maxEmailDict.TryGetValue(prefix, out string? lastEmail) || string.IsNullOrEmpty(lastEmail))
                    {
                        generatedEmail = $"{prefix}1111{domainSuffix}";
                    }
                    else
                    {
                        int nextSeq = 1111;
                        if (lastEmail.Length > 2)
                        {
                            var match = Regex.Match(lastEmail.Substring(2), @"^(\d+)");
                            if (match.Success && int.TryParse(match.Groups[1].Value, out int currentNumber))
                            {
                                nextSeq = Math.Max(1111, currentNumber + 1);
                            }
                        }

                        generatedEmail = $"{prefix}{nextSeq}{domainSuffix}";
                    }

                    // Update in-memory dictionary for subsequent students with the same initials
                    maxEmailDict[prefix] = generatedEmail;
                    generatedCount++;

                    // Add new student to dgvGenerated (final save list)
                    int genIndex = dgvGenerated.Rows.Add();
                    DataGridViewRow genRow = dgvGenerated.Rows[genIndex];

                    genRow.Cells["colGenSelect"].Value      = true;
                    genRow.Cells["colGenIndex"].Value       = dgvGenerated.Rows.Count;
                    genRow.Cells["colGenId"].Value          = nationalId;     // "الرقم القومي" -> id
                    genRow.Cells["colGenArName"].Value      = arName;         // "الاسم باللغة العربية" -> ar_name
                    genRow.Cells["colGenFullName"].Value    = enFullName;
                    genRow.Cells["colGenFirstName"].Value   = firstName;      // Extracted First Word -> first_name
                    genRow.Cells["colGenLastName"].Value    = lastName;       // Extracted Remaining Words -> last_name
                    genRow.Cells["colGenCollege"].Value     = college;        // "الكلية" -> college
                    genRow.Cells["colGenDept"].Value        = dept;           // "الشعبة" -> dept
                    genRow.Cells["colGenCode"].Value        = code;           // "الكود" -> code
                    genRow.Cells["colGenStage"].Value       = stage;          // "الفرقة" -> stage
                    genRow.Cells["colGenType"].Value        = studyType;      // "طبيعة الدراسة" -> type
                    genRow.Cells["colGenStudentStat"].Value = stat;           // "الحالة" -> stat
                    genRow.Cells["colGenEmail"].Value       = generatedEmail; // Generated Email -> mail
                    genRow.Cells["colGenStatus"].Value      = "جاهز للحفظ";

                    genRow.DefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);

                    rawRow.Cells["colOrigStatus"].Value = "تم الإنشاء";
                    rawRow.DefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
                    rawRow.DefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
                }

                dgvGenerated.ResumeLayout();
                dgvOriginal.ResumeLayout();

                if (generatedCount > 0)
                {
                    _hasUnsavedGeneratedEmails = true;
                }

                chkSelectAllGen.Checked = dgvGenerated.Rows.Count > 0;
                UpdateOriginalSummary();
                UpdateGeneratedSummary();
                UpdateUiState();

                lblStatusAction.Text = "جاهز للحفظ...";

                string message = $"تم إنشاء الإيميلات بنجاح. الجاهز للحفظ: {generatedCount}";
                if (duplicateCount > 0)
                {
                    message += $"\nتم تخطي {duplicateCount} طالب لأن أرقامهم القومية مسجلة مسبقاً.";
                }

                MessageBox.Show(
                    message,
                    "اكتمل إنشاء الإيميلات",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Forward action: move to View 2 (الخطوة 3) once there are generated emails to review/save
                if (dgvGenerated.Rows.Count > 0)
                {
                    ShowWizardView(2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
                btnCreateEmails.Text = "⚡ إنشاء الإيميلات للطلاب المحددين ←";
                UpdateUiState();
            }
        }

        private void chkSelectAllOrig_CheckedChanged(object sender, EventArgs e)
        {
            bool check = chkSelectAllOrig.Checked;
            foreach (DataGridViewRow row in dgvOriginal.Rows)
            {
                row.Cells["colOrigSelect"].Value = check;
            }
            UpdateOriginalSummary();
            UpdateUiState();
        }

        private void dgvOriginal_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colOrigSelect.Index)
            {
                UpdateOriginalSummary();
                UpdateUiState();
            }
        }

        private void dgvOriginal_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOriginal.IsCurrentCellDirty && dgvOriginal.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvOriginal.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void UpdateOriginalSummary()
        {
            int total = dgvOriginal.Rows.Count;
            int selected = 0;

            foreach (DataGridViewRow row in dgvOriginal.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colOrigSelect"].Value ?? false))
                    selected++;
            }

            lblOriginalCount.Text = $"عدد الطلاب: {total} (المحدد: {selected})";
        }

        #endregion

        #region 3. الخطوة 3: الحفظ (SqlBulkCopy Save with Exact dbo.mails Column Mappings)

        private void btnSaveToDb_Click(object sender, EventArgs e)
        {
            ConfigureFoolProofUiAndColumns();

            var selectedRows = GetSelectedGeneratedRows();

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد إيميل واحد على الأقل للحفظ.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSaveToDb.Enabled = false;
            btnSaveToDb.Text = "جاري الحفظ...";
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = 100;
            lblStatusAction.Text = "جاري حفظ البيانات في قاعدة البيانات...";

            try
            {
                string validatedTable = (_targetTable.Equals("PostGradMails", StringComparison.OrdinalIgnoreCase))
                    ? "PostGradMails"
                    : "mails";

                // =========================================================================
                // 1. CONSTRUCT IN-MEMORY DATATABLE WITH EXACT dbo.mails COLUMNS:
                //    first_name, last_name, mail, create_date, user_name, id,
                //    college, dept, code, type, stat, ar_name, stage
                // =========================================================================
                DataTable dtBulk = new DataTable();
                dtBulk.Columns.Add("first_name", typeof(string));  // Extracted First Word
                dtBulk.Columns.Add("last_name", typeof(string));   // Extracted Remaining Words
                dtBulk.Columns.Add("mail", typeof(string));        // Generated Email
                dtBulk.Columns.Add("create_date", typeof(string)); // System generated date
                dtBulk.Columns.Add("user_name", typeof(string));   // Logged-in user
                dtBulk.Columns.Add("id", typeof(string));          // "الرقم القومي" (National ID)
                dtBulk.Columns.Add("college", typeof(string));     // "الكلية"
                dtBulk.Columns.Add("dept", typeof(string));        // "الشعبة"
                dtBulk.Columns.Add("code", typeof(string));        // "الكود"
                dtBulk.Columns.Add("type", typeof(string));        // "طبيعة الدراسة"
                dtBulk.Columns.Add("stat", typeof(string));        // "الحالة"
                dtBulk.Columns.Add("ar_name", typeof(string));     // "الاسم باللغة العربية"
                dtBulk.Columns.Add("stage", typeof(string));       // "الفرقة"

                string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var savedRows = new List<DataGridViewRow>();

                foreach (var genRow in selectedRows)
                {
                    string email = genRow.Cells["colGenEmail"].Value?.ToString()?.Trim() ?? "";
                    string rowStatus = genRow.Cells["colGenStatus"].Value?.ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(email) || rowStatus == "تم الحفظ")
                        continue;

                    dtBulk.Rows.Add(
                        genRow.Cells["colGenFirstName"].Value?.ToString()?.Trim() ?? "",   // first_name
                        genRow.Cells["colGenLastName"].Value?.ToString()?.Trim() ?? "",    // last_name
                        email,                                                             // mail
                        currentDate,                                                       // create_date
                        UserSession.CurrentUsername,                                       // user_name
                        genRow.Cells["colGenId"].Value?.ToString()?.Trim() ?? "",          // id ("الرقم القومي")
                        genRow.Cells["colGenCollege"].Value?.ToString()?.Trim() ?? "",     // college ("الكلية")
                        genRow.Cells["colGenDept"].Value?.ToString()?.Trim() ?? "",        // dept ("الشعبة")
                        genRow.Cells["colGenCode"].Value?.ToString()?.Trim() ?? "",        // code ("الكود")
                        genRow.Cells["colGenType"].Value?.ToString()?.Trim() ?? "",        // type ("طبيعة الدراسة")
                        genRow.Cells["colGenStudentStat"].Value?.ToString()?.Trim() ?? "", // stat ("الحالة")
                        genRow.Cells["colGenArName"].Value?.ToString()?.Trim() ?? "",      // ar_name ("الاسم باللغة العربية")
                        genRow.Cells["colGenStage"].Value?.ToString()?.Trim() ?? ""        // stage ("الفرقة")
                    );

                    savedRows.Add(genRow);
                }

                if (dtBulk.Rows.Count == 0)
                {
                    MessageBox.Show("لا توجد بيانات جديدة للحفظ.", "تنبيه",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // =========================================================================
                // 2. EXECUTE HIGH-PERFORMANCE SqlBulkCopy WITH EXACT COLUMN MAPPINGS
                // =========================================================================
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();

                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.FireTriggers, trans))
                            {
                                bulkCopy.DestinationTableName = $"dbo.[{validatedTable}]";
                                bulkCopy.BatchSize = 5000;
                                bulkCopy.BulkCopyTimeout = 120;

                                // Exact SqlBulkCopy.ColumnMappings matching dbo.mails schema
                                bulkCopy.ColumnMappings.Add("first_name", "first_name");
                                bulkCopy.ColumnMappings.Add("last_name", "last_name");
                                bulkCopy.ColumnMappings.Add("mail", "mail");
                                bulkCopy.ColumnMappings.Add("create_date", "create_date");
                                bulkCopy.ColumnMappings.Add("user_name", "user_name");
                                bulkCopy.ColumnMappings.Add("id", "id");
                                bulkCopy.ColumnMappings.Add("college", "college");
                                bulkCopy.ColumnMappings.Add("dept", "dept");
                                bulkCopy.ColumnMappings.Add("code", "code");
                                bulkCopy.ColumnMappings.Add("type", "type");
                                bulkCopy.ColumnMappings.Add("stat", "stat");
                                bulkCopy.ColumnMappings.Add("ar_name", "ar_name");
                                bulkCopy.ColumnMappings.Add("stage", "stage");

                                bulkCopy.WriteToServer(dtBulk);
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }

                    // =========================================================================
                    // 3. AUDIT LOGGING INTO dbo.SystemLogs (Distinct Colleges via LINQ)
                    // =========================================================================
                    string username = string.IsNullOrWhiteSpace(UserSession.CurrentUsername)
                        ? "System"
                        : UserSession.CurrentUsername;
                    int count = dtBulk.Rows.Count;

                    // 1. Extract all unique/distinct college names from the DataTable using LINQ
                    var distinctColleges = dtBulk.AsEnumerable()
                        .Select(row => row.Field<string>("college")?.Trim())
                        .Where(college => !string.IsNullOrWhiteSpace(college))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    // 2. Join distinct college names into a single Arabic comma-separated string (e.g., "الالسن، التجارة، الحقوق")
                    string distinctCollegesString = distinctColleges.Count > 0
                        ? string.Join("، ", distinctColleges)
                        : "غير محدد";

                    // 3. Dynamically identify the modified target table (dbo.mails or dbo.PostGradMails)
                    string targetTableName = $"dbo.{validatedTable}";

                    // 4. Construct the detailed Arabic audit log message combining Action (INSERT), Dynamic Table, Count, and Distinct Colleges
                    string logDetails = $"قام المستخدم {username} بإجراء عملية إدخال (INSERT) لعدد {count} سجل في جدول {targetTableName}، يخص طلاب الكليات التالية: {distinctCollegesString}.";

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
                }

                // =========================================================================
                // 4. KEEP dgvGenerated POPULATED FOR EXPORT/REPORT & DISABLE SAVE BUTTON
                // =========================================================================
                dgvGenerated.SuspendLayout();
                foreach (var savedRow in savedRows)
                {
                    savedRow.Cells["colGenStatus"].Value = "تم الحفظ";
                    savedRow.DefaultCellStyle.BackColor = Color.FromArgb(220, 252, 231);
                }
                dgvGenerated.ResumeLayout();

                _hasUnsavedGeneratedEmails = false;
                UpdateGeneratedSummary();
                UpdateUiState(); // Disables btnSaveToDb while keeping btnExportCsv & btnShowReport enabled

                lblStatusAction.Text = "تم الحفظ بنجاح.";

                MessageBox.Show(
                    "تم حفظ الإيميلات في قاعدة البيانات بنجاح!",
                    "نجاح الحفظ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
                btnSaveToDb.Text = "💾 حفظ في قاعدة البيانات";
                UpdateUiState();
            }
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGenerated.Rows.Count == 0)
                {
                    MessageBox.Show("لا توجد إيميلات لتصديرها إلى ملف CSV.", "تنبيه",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var sfd = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    DefaultExt = "csv",
                    FileName = $"GoogleWorkspace_Users_{_targetTable}_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                    Title = "حفظ ملف الـ CSV"
                };

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                // A. Exact 6-column Google Workspace header row (do not change a single character)
                const string headerLine = "First Name,Last Name,Email Address [Required],Password [Required],Org Unit Path [Required],Recovery Phone [MUST BE IN THE E.164 FORMAT]";

                int exportedCount = 0;

                using (var writer = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    // Write the exact header row as the very first line
                    writer.WriteLine(headerLine);

                    // B. Iterate through dgvGenerated grid rows
                    foreach (DataGridViewRow row in dgvGenerated.Rows)
                    {
                        if (row.IsNewRow)
                            continue;

                        string firstName = EscapeCsv(row.Cells["colGenFirstName"].Value?.ToString()?.Trim() ?? "");
                        string lastName  = EscapeCsv(row.Cells["colGenLastName"].Value?.ToString()?.Trim() ?? "");
                        string email     = EscapeCsv(row.Cells["colGenEmail"].Value?.ToString()?.Trim() ?? "");

                        if (string.IsNullOrWhiteSpace(email))
                            continue;

                        // Auto-generate an 8-character alphanumeric password for each row
                        string password = EscapeCsv(GenerateRandomPassword());

                        // Column 1: first_name
                        // Column 2: last_name
                        // Column 3: mail (already includes @fayoum.edu.eg from generation step)
                        // Column 4: Auto-generated password
                        // Column 5: /Student (Static string for Org Unit Path)
                        // Column 6: Recovery Phone (left completely empty)
                        string csvLine = $"{firstName},{lastName},{email},{password},/Student,";
                        writer.WriteLine(csvLine);
                        exportedCount++;
                    }
                }

                lblStatusAction.Text = $"تم التصدير بنجاح ({exportedCount} طالب).";

                MessageBox.Show(
                    "تم التصدير بنجاح! الملف جاهز للرفع على جوجل.",
                    "نجاح التصدير",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Generates an 8-character secure alphanumeric password combining uppercase, lowercase, and digits.
        /// </summary>
        private static string GenerateRandomPassword()
        {
            const int length = 8;
            const string upperChars = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijkmnopqrstuvwxyz";
            const string digitChars = "23456789";
            const string allChars   = upperChars + lowerChars + digitChars;

            char[] passwordChars = new char[length];

            // Guarantee at least one uppercase letter, one lowercase letter, and one digit
            passwordChars[0] = upperChars[RandomNumberGenerator.GetInt32(upperChars.Length)];
            passwordChars[1] = lowerChars[RandomNumberGenerator.GetInt32(lowerChars.Length)];
            passwordChars[2] = digitChars[RandomNumberGenerator.GetInt32(digitChars.Length)];

            for (int i = 3; i < length; i++)
            {
                passwordChars[i] = allChars[RandomNumberGenerator.GetInt32(allChars.Length)];
            }

            // Shuffle character positions using Fisher-Yates
            for (int i = passwordChars.Length - 1; i > 0; i--)
            {
                int swapIndex = RandomNumberGenerator.GetInt32(i + 1);
                (passwordChars[i], passwordChars[swapIndex]) = (passwordChars[swapIndex], passwordChars[i]);
            }

            return new string(passwordChars);
        }

        private void btnShowReport_Click(object sender, EventArgs e)
        {
            _printRows = GetSelectedGeneratedRows();

            if (_printRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد صف واحد على الأقل لطباعة التقرير.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _printRowIndex = 0;
            _printPageNumber = 1;

            try
            {
                using var printDoc = new PrintDocument();
                printDoc.DocumentName = $"Student Emails Report - {_targetTitle}";
                printDoc.DefaultPageSettings.Landscape = true;
                printDoc.PrintPage += PrintDocument_PrintPage;

                using var previewDlg = new PrintPreviewDialog
                {
                    Document = printDoc,
                    Width = 960,
                    Height = 650,
                    StartPosition = FormStartPosition.CenterParent,
                    Text = $"معاينة التقرير — {_targetTitle} ({_printRows.Count} طالب)"
                };

                lblStatusAction.Text = "جاري عرض معاينة الطباعة...";
                previewDlg.ShowDialog();
                lblStatusAction.Text = "تم إغلاق معاينة التقرير.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (e.Graphics == null) return;
            Graphics g = e.Graphics;
            int margin = 40;
            int y = margin;
            int pageWidth = e.PageBounds.Width - (margin * 2);

            using var headerFont = new Font("Segoe UI", 16, FontStyle.Bold);
            using var subFont = new Font("Segoe UI", 10, FontStyle.Regular);
            using var boldFont = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using var regularFont = new Font("Segoe UI", 9, FontStyle.Regular);

            g.DrawString($"Student Email Generation Report — {_targetTitle}", headerFont, Brushes.Black, margin, y);
            y += 32;

            string meta = $"Generated by: {UserSession.CurrentUsername}   |   Database Table: dbo.[{_targetTable}]   |   Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}   |   Total Rows: {_printRows.Count}";
            g.DrawString(meta, subFont, Brushes.Gray, margin, y);
            y += 26;

            g.DrawLine(Pens.LightGray, margin, y, margin + pageWidth, y);
            y += 10;

            int colNumW = 40;
            int colIdW = 130;
            int colNameW = 240;
            int colFirstW = 100;
            int colLastW = 120;
            int colMailW = 240;

            g.FillRectangle(new SolidBrush(Color.FromArgb(241, 245, 249)), margin, y, pageWidth, 26);
            g.DrawRectangle(Pens.LightGray, margin, y, pageWidth, 26);

            int x = margin + 5;
            g.DrawString("#", boldFont, Brushes.Black, x, y + 4); x += colNumW;
            g.DrawString("National ID", boldFont, Brushes.Black, x, y + 4); x += colIdW;
            g.DrawString("English Name", boldFont, Brushes.Black, x, y + 4); x += colNameW;
            g.DrawString("First Name", boldFont, Brushes.Black, x, y + 4); x += colFirstW;
            g.DrawString("Last Name", boldFont, Brushes.Black, x, y + 4); x += colLastW;
            g.DrawString("Generated Email", boldFont, Brushes.Black, x, y + 4); x += colMailW;
            g.DrawString("College", boldFont, Brushes.Black, x, y + 4);
            y += 28;

            int rowHeight = 24;

            while (_printRowIndex < _printRows.Count)
            {
                if (y + rowHeight > e.PageBounds.Height - margin - 30)
                {
                    e.HasMorePages = true;
                    g.DrawString($"Page {_printPageNumber}", regularFont, Brushes.Gray, margin + pageWidth - 60, e.PageBounds.Height - margin);
                    _printPageNumber++;
                    return;
                }

                var row = _printRows[_printRowIndex];
                x = margin + 5;

                if (_printRowIndex % 2 == 1)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(248, 250, 252)), margin, y, pageWidth, rowHeight);
                }

                string num = (_printRowIndex + 1).ToString();
                string id = row.Cells["colGenId"].Value?.ToString() ?? "";
                string fullName = row.Cells["colGenFullName"].Value?.ToString() ?? "";
                string firstName = row.Cells["colGenFirstName"].Value?.ToString() ?? "";
                string lastName = row.Cells["colGenLastName"].Value?.ToString() ?? "";
                string email = row.Cells["colGenEmail"].Value?.ToString() ?? "";
                string college = row.Cells["colGenCollege"].Value?.ToString() ?? "";

                g.DrawString(num, regularFont, Brushes.Black, x, y + 4); x += colNumW;
                g.DrawString(id, regularFont, Brushes.Black, x, y + 4); x += colIdW;
                g.DrawString(Truncate(fullName, 28), regularFont, Brushes.Black, x, y + 4); x += colNameW;
                g.DrawString(Truncate(firstName, 14), regularFont, Brushes.Black, x, y + 4); x += colFirstW;
                g.DrawString(Truncate(lastName, 18), regularFont, Brushes.Black, x, y + 4); x += colLastW;
                g.DrawString(email, boldFont, new SolidBrush(Color.FromArgb(15, 118, 110)), x, y + 4); x += colMailW;
                g.DrawString(Truncate(college, 18), regularFont, Brushes.Black, x, y + 4);

                g.DrawLine(Pens.Gainsboro, margin, y + rowHeight, margin + pageWidth, y + rowHeight);

                y += rowHeight;
                _printRowIndex++;
            }

            g.DrawString($"Page {_printPageNumber} (End of Report)", regularFont, Brushes.Gray, margin + pageWidth - 140, e.PageBounds.Height - margin);
            e.HasMorePages = false;
        }

        private void btnClearGenerated_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGenerated.Rows.Count == 0) return;

                var confirm = MessageBox.Show(
                    "هل أنت متأكد من مسح قائمة الإيميلات التي تم إنشاؤها؟",
                    "تأكيد المسح",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    dgvGenerated.Rows.Clear();
                    _hasUnsavedGeneratedEmails = false;
                    UpdateGeneratedSummary();
                    UpdateUiState();
                    lblStatusAction.Text = "تم مسح قائمة الإيميلات.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تنفيذ المهمة: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkSelectAllGen_CheckedChanged(object sender, EventArgs e)
        {
            bool check = chkSelectAllGen.Checked;
            foreach (DataGridViewRow row in dgvGenerated.Rows)
            {
                row.Cells["colGenSelect"].Value = check;
            }
            UpdateGeneratedSummary();
            UpdateUiState();
        }

        private void dgvGenerated_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colGenSelect.Index)
            {
                UpdateGeneratedSummary();
                UpdateUiState();
            }
        }

        private void dgvGenerated_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvGenerated.IsCurrentCellDirty && dgvGenerated.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvGenerated.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void UpdateGeneratedSummary()
        {
            int total = dgvGenerated.Rows.Count;
            int selected = 0;

            foreach (DataGridViewRow row in dgvGenerated.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colGenSelect"].Value ?? false))
                    selected++;
            }

            lblGeneratedCount.Text = $"الإيميلات الجاهزة: {total} (المحدد: {selected})";
        }

        private List<DataGridViewRow> GetSelectedGeneratedRows()
        {
            var list = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvGenerated.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colGenSelect"].Value ?? false))
                {
                    list.Add(row);
                }
            }
            return list;
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }

        private static string Truncate(string val, int maxLen)
        {
            if (string.IsNullOrEmpty(val)) return "";
            return val.Length <= maxLen ? val : val.Substring(0, maxLen - 2) + "..";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
