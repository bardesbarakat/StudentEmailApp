using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using StudentEmailApp.Services;

namespace StudentEmailApp.Forms
{
    /// <summary>
    /// Reports & Analytics: filter by data source, creating user, college, year and month,
    /// with a live summary card (total + top producer) and a CSV (Excel-compatible) export.
    /// </summary>
    public partial class frmReports : Form
    {
        // Palette (matches frmMain)
        private static readonly Color Gold       = ColorTranslator.FromHtml("#D4AF37");
        private static readonly Color Navy       = ColorTranslator.FromHtml("#0F172A");
        private static readonly Color Slate      = ColorTranslator.FromHtml("#64748B");
        private static readonly Color Border     = ColorTranslator.FromHtml("#E2E8F0");
        private static readonly Color ContentBg  = ColorTranslator.FromHtml("#F8FAFC");

        // ---- Column mapping: adjust here if your DB column names differ ----
        private const string ColUser   = "user_name";
        private const string ColCollege = "college";
        private const string ColDate   = "create_date";
        private const string ColDept   = "dept";
        private const string ColStage  = "stage";
        private const string ColType   = "type";   // study type / degree
        // ---------------------------------------------------------------------

        private sealed record Item(string Text, object? Value)
        {
            public override string ToString() => Text;
        }

        private readonly string _username;
        private readonly string _userRole;
        private bool _loadingFilters;
        private FlowLayoutPanel? _filterFlow;
        private Panel? _filterCard;
        private TableLayoutPanel? _filterRoot;

        private ComboBox cmbReportType = null!, cmbSource = null!, cmbUser = null!, cmbCollege = null!, cmbYear = null!, cmbMonth = null!;
        private Button btnGenerate = null!, btnExport = null!;
        private Label lblTotal = null!, lblTopUser = null!;
        private DataGridView dgvReport = null!;

        public frmReports() : this("Guest", "Admin") { }

        public frmReports(string username, string userRole)
        {
            InitializeComponent();
            _username = username;
            _userRole = userRole;
            BuildUi();
            Load += (s, e) => LoadFilterLists(); // fills combos only; the grid stays empty until "عرض التقرير"
        }

        #region UI

        private void BuildUi()
        {
            SuspendLayout();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Normal;
            Dock = DockStyle.Fill;
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = false;
            BackColor = ContentBg;
            Font = new Font("Segoe UI", 10F);
            Text = "التقارير والإحصائيات";

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(12),
                BackColor = ContentBg
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));          // filters
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));     // summary card
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));     // grid

            // ---------------- 1. Filter panel (FlowLayoutPanel, flows right-to-left) ----------------
            // TopFilterPanel: height is recomputed from the flow's WRAPPED size (see FitFilterHeight),
            // so a second row of controls always gets room instead of hiding under the summary card.
            var filterCard = new Panel
            {
                Dock = DockStyle.Fill, AutoSize = false,
                BackColor = Color.White, Padding = new Padding(12, 10, 12, 10), Margin = new Padding(0, 0, 0, 10)
            };
            filterCard.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Border), 0, 0, filterCard.Width - 1, filterCard.Height - 1);

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, AutoSize = false,
                FlowDirection = FlowDirection.LeftToRight, // with RightToLeft.Yes this flows from the right edge
                RightToLeft = RightToLeft.Yes, WrapContents = true, BackColor = Color.White
            };
            _filterFlow = flow;

            cmbReportType = MakeFilter(flow, "نوع التقرير", 190);
            cmbSource  = MakeFilter(flow, "نوع البيانات", 130);
            cmbUser    = MakeFilter(flow, "المستخدم", 130);
            cmbCollege = MakeFilter(flow, "الكلية", 130);
            cmbYear    = MakeFilter(flow, "السنة", 100);
            cmbMonth   = MakeFilter(flow, "الشهر", 120);

            btnGenerate = MakeButton("🔍 عرض التقرير", Gold, Navy, 130);
            btnGenerate.Click += (s, e) => GenerateReport();
            btnExport = MakeButton("📊 تصدير لـ Excel", Color.White, Navy, 130);
            btnExport.FlatAppearance.BorderSize = 1;
            btnExport.FlatAppearance.BorderColor = Border;
            btnExport.Click += (s, e) => ExportToExcel();

            var buttons = new FlowLayoutPanel
            {
                AutoSize = true, FlowDirection = FlowDirection.LeftToRight, RightToLeft = RightToLeft.Yes,
                Margin = new Padding(8, 22, 0, 0), BackColor = Color.White
            };
            buttons.Controls.Add(btnGenerate);
            buttons.Controls.Add(btnExport);
            flow.Controls.Add(buttons);

            filterCard.Controls.Add(flow);
            _filterRoot = root;
            _filterCard = filterCard;
            filterCard.SizeChanged += (s, e) => FitFilterHeight(); // sidebar toggle / window resize
            Shown += (s, e) => FitFilterHeight();

            // Report types (keys are matched in ResolveReport's switch)
            cmbReportType.Items.Add(new Item("إحصائيات الكليات", "college"));
            cmbReportType.Items.Add(new Item("إحصائيات الأقسام والشعب", "dept"));
            cmbReportType.Items.Add(new Item("إحصائيات الفرق الدراسية", "stage"));
            cmbReportType.Items.Add(new Item("إحصائيات الدرجات العلمية", "degree"));
            cmbReportType.Items.Add(new Item("إحصائيات المستخدمين", "user"));
            cmbReportType.Items.Add(new Item("معدل الإنجاز اليومي", "daily"));
            cmbReportType.SelectedIndex = 0;

            // Data source: whitelist of tables (never concatenate user text into SQL)
            cmbSource.Items.Add(new Item("بكالوريوس", "mails"));
            cmbSource.Items.Add(new Item("دراسات عليا", "PostGradMails"));
            bool isAdmin = _userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            cmbSource.SelectedIndex = _userRole.Equals("Postgrad", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
            cmbSource.Enabled = isAdmin; // staff only see their own data source
            cmbSource.SelectedIndexChanged += (s, e) => { if (!_loadingFilters) LoadFilterLists(); };

            // ---------------- 2. Summary card ----------------
            var summary = new Panel
            {
                Dock = DockStyle.Fill, BackColor = Navy, Padding = new Padding(24, 10, 24, 10), Margin = new Padding(0, 0, 0, 10)
            };
            lblTotal = new Label
            {
                Dock = DockStyle.Top, Height = 44, ForeColor = Gold, BackColor = Navy,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleRight,
                Text = "إجمالي السجلات: 0"
            };
            lblTopUser = new Label
            {
                Dock = DockStyle.Fill, ForeColor = Color.FromArgb(203, 213, 225), BackColor = Navy,
                Font = new Font("Segoe UI", 10.5F), TextAlign = ContentAlignment.TopRight,
                Text = "اضغط \"عرض التقرير\" لعرض النتائج"
            };
            summary.Controls.Add(lblTopUser);
            summary.Controls.Add(lblTotal);

            // ---------------- 3. Grid ----------------
            var gridCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) };
            dgvReport = new DataGridView
            {
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                RowHeadersVisible = false, BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false, RightToLeft = RightToLeft.Yes,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 40, RowTemplate = { Height = 32 }
            };
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Navy;
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvReport.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvReport.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            dgvReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 249, 225);
            dgvReport.DefaultCellStyle.SelectionForeColor = Navy;
            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = ContentBg;
            gridCard.Controls.Add(dgvReport);

            root.Controls.Add(filterCard, 0, 0);
            root.Controls.Add(summary, 0, 1);
            root.Controls.Add(gridCard, 0, 2);
            Controls.Add(root);

            ResumeLayout(false);
        }

        /// <summary>
        /// Sizes row 0 of the root layout to the flow's wrapped height for the current width,
        /// so wrapped buttons are never clipped and the summary card is pushed down instead.
        /// </summary>
        private void FitFilterHeight()
        {
            if (_filterFlow == null || _filterCard == null || _filterRoot == null) return;
            int available = _filterCard.ClientSize.Width - _filterCard.Padding.Horizontal;
            if (available <= 0) return;

            int flowHeight = _filterFlow.GetPreferredSize(new Size(available, 0)).Height;
            float target = flowHeight + _filterCard.Padding.Vertical + _filterCard.Margin.Vertical + 2;

            var row = _filterRoot.RowStyles[0];
            if (row.SizeType != SizeType.Absolute || Math.Abs(row.Height - target) > 0.5f)
            {
                row.SizeType = SizeType.Absolute;
                row.Height = target;
            }
        }

        /// <summary>Adds a "label over flat combo" block to the flow panel.</summary>
        private static ComboBox MakeFilter(FlowLayoutPanel host, string caption, int width)
        {
            var block = new TableLayoutPanel
            {
                AutoSize = true, ColumnCount = 1, RowCount = 2, Margin = new Padding(6, 0, 6, 8),
                BackColor = Color.White
            };
            var lbl = new Label
            {
                Text = caption, AutoSize = true, ForeColor = Slate,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold), Margin = new Padding(2, 0, 2, 4)
            };
            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat, Width = width,
                Font = new Font("Segoe UI", 10.5F), BackColor = ContentBg, ForeColor = Navy, Margin = new Padding(2)
            };
            block.Controls.Add(lbl, 0, 0);
            block.Controls.Add(cmb, 0, 1);
            host.Controls.Add(block);
            return cmb;
        }

        private static Button MakeButton(string text, Color back, Color fore, int width)
        {
            var b = new Button
            {
                Text = text, Width = width, Height = 36, FlatStyle = FlatStyle.Flat, BackColor = back,
                ForeColor = fore, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand, Margin = new Padding(4, 0, 4, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        #endregion

        #region Filter lists

        private string SelectedTable() => (cmbSource.SelectedItem as Item)?.Value as string ?? "mails";

        private static readonly string[] ArabicMonths =
        {
            "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
            "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
        };

        /// <summary>Fills User / College / Year / Month combos for the selected data source.</summary>
        private void LoadFilterLists()
        {
            _loadingFilters = true;
            try
            {
                string table = SelectedTable(); // whitelisted value, safe to embed as [identifier]
                var users = new List<Item> { new("الكل", null) };
                var colleges = new List<Item> { new("الكل", null) };
                var years = new List<Item> { new("الكل", null) };

                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    ReadInto(conn, $"SELECT DISTINCT [{ColUser}] FROM dbo.[{table}] WHERE [{ColUser}] IS NOT NULL ORDER BY 1",
                        v => users.Add(new Item(v.ToString()!, v.ToString())));
                    ReadInto(conn, $"SELECT DISTINCT [{ColCollege}] FROM dbo.[{table}] WHERE [{ColCollege}] IS NOT NULL ORDER BY 1",
                        v => colleges.Add(new Item(v.ToString()!, v.ToString())));
                    ReadInto(conn, $"SELECT DISTINCT YEAR([{ColDate}]) FROM dbo.[{table}] WHERE [{ColDate}] IS NOT NULL ORDER BY 1 DESC",
                        v => years.Add(new Item(v.ToString()!, Convert.ToInt32(v))));
                }

                var months = new List<Item> { new("الكل", null) };
                for (int m = 1; m <= 12; m++) months.Add(new Item($"{m:00} - {ArabicMonths[m - 1]}", m));

                Bind(cmbUser, users);
                Bind(cmbCollege, colleges);
                Bind(cmbYear, years);
                Bind(cmbMonth, months);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"تعذر تحميل قوائم التصفية:\n{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _loadingFilters = false; }
        }

        private static void ReadInto(SqlConnection conn, string sql, Action<object> add)
        {
            using var cmd = new SqlCommand(sql, conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                if (!r.IsDBNull(0)) add(r.GetValue(0));
        }

        private static void Bind(ComboBox cmb, List<Item> items)
        {
            cmb.Items.Clear();
            cmb.Items.AddRange(items.Cast<object>().ToArray());
            cmb.SelectedIndex = 0;
        }

        #endregion

        #region Dynamic SQL

        private static SqlParameter OptionalParam(string name, SqlDbType type, int size, object? value)
        {
            var p = size > 0 ? new SqlParameter(name, type, size) : new SqlParameter(name, type);
            p.Value = value ?? DBNull.Value; // 'All' => NULL => condition is skipped in the WHERE clause
            return p;
        }

        /// <summary>
        /// Maps the selected report type to its grouping expression (used for both SELECT and GROUP BY).
        /// Blank/NULL values are folded into "غير محدد" so every record is counted.
        /// </summary>
        private (string GroupExpr, string OrderBy, string Title) ResolveReport(string reportKey)
        {
            static string Clean(string col) => $"ISNULL(NULLIF(LTRIM(RTRIM(CAST([{col}] AS NVARCHAR(200)))), N''), N'غير محدد')";

            switch (reportKey)
            {
                case "college": return (Clean(ColCollege), "[العدد] DESC", "إحصائيات الكليات");
                case "dept":    return (Clean(ColDept),    "[العدد] DESC", "إحصائيات الأقسام والشعب");
                case "stage":   return (Clean(ColStage),   "[العدد] DESC", "إحصائيات الفرق الدراسية");
                case "degree":  return (Clean(ColType),    "[العدد] DESC", "إحصائيات الدرجات العلمية");
                case "user":    return (Clean(ColUser),    "[العدد] DESC", "إحصائيات المستخدمين");
                case "daily":   return ($"CAST([{ColDate}] AS DATE)", "[البيان] DESC", "معدل الإنجاز اليومي");
                default: throw new InvalidOperationException("Unknown report type.");
            }
        }

        /// <summary>Builds and runs the aggregated query (البيان / العدد) for the selected report type and data source.</summary>
        private void GenerateReport()
        {
            string table = SelectedTable(); // whitelist: "mails" | "PostGradMails"
            if (table != "mails" && table != "PostGradMails") return;

            string reportKey = (cmbReportType.SelectedItem as Item)?.Value as string ?? "college";
            var (groupExpr, orderBy, title) = ResolveReport(reportKey);

            // Same WHERE for every report type: the other filters stay intact. 'All' => NULL => skipped.
            string sql = $@"
                SELECT {groupExpr} AS [البيان], COUNT(*) AS [العدد]
                FROM dbo.[{table}]
                WHERE (@username IS NULL OR [{ColUser}] = @username)
                  AND (@college  IS NULL OR [{ColCollege}] = @college)
                  AND (@year     IS NULL OR YEAR([{ColDate}])  = @year)
                  AND (@month    IS NULL OR MONTH([{ColDate}]) = @month)
                GROUP BY {groupExpr}
                ORDER BY {orderBy}
                OPTION (RECOMPILE)";

            try
            {
                Cursor = Cursors.WaitCursor;
                using var conn = DbHelper.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(OptionalParam("@username", SqlDbType.NVarChar, 100, (cmbUser.SelectedItem as Item)?.Value));
                cmd.Parameters.Add(OptionalParam("@college",  SqlDbType.NVarChar, 200, (cmbCollege.SelectedItem as Item)?.Value));
                cmd.Parameters.Add(OptionalParam("@year",     SqlDbType.Int, 0, (cmbYear.SelectedItem as Item)?.Value));
                cmd.Parameters.Add(OptionalParam("@month",    SqlDbType.Int, 0, (cmbMonth.SelectedItem as Item)?.Value));

                var dt = new DataTable();
                using (var adapter = new SqlDataAdapter(cmd)) adapter.Fill(dt);

                // Dates arrive as DateTime; show them as plain yyyy-MM-dd text so the grid and CSV stay clean
                if (reportKey == "daily")
                {
                    var txt = new DataTable();
                    txt.Columns.Add("البيان", typeof(string));
                    txt.Columns.Add("العدد", typeof(int));
                    foreach (DataRow r in dt.Rows)
                        txt.Rows.Add(Convert.ToDateTime(r["البيان"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), Convert.ToInt32(r["العدد"]));
                    dt = txt;
                }

                dgvReport.DataSource = dt;
                ApplyHeaders();
                UpdateSummary(dt, title);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء إنشاء التقرير:\n{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor = Cursors.Default; }
        }

        private void ApplyHeaders()
        {
            foreach (DataGridViewColumn c in dgvReport.Columns)
            {
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 120;
                c.DefaultCellStyle.Alignment = c.Name == "العدد"
                    ? DataGridViewContentAlignment.MiddleCenter
                    : DataGridViewContentAlignment.MiddleRight;
                if (c.Name == "العدد") c.DefaultCellStyle.Format = "N0";
            }
        }

        private void UpdateSummary(DataTable dt, string title)
        {
            // Grand total = sum of the العدد column (each record falls in exactly one group)
            long grandTotal = dt.AsEnumerable().Sum(r => Convert.ToInt64(r["العدد"]));
            lblTotal.Text = $"إجمالي السجلات: {grandTotal:N0}";

            if (dt.Rows.Count == 0) { lblTopUser.Text = "لا توجد سجلات مطابقة لعوامل التصفية"; return; }

            var top = dt.AsEnumerable()
                .Select(r => (Name: Convert.ToString(r["البيان"]) ?? "—", Count: Convert.ToInt64(r["العدد"])))
                .OrderByDescending(x => x.Count)
                .First();

            lblTopUser.Text = $"{title} — عدد البنود: {dt.Rows.Count:N0}   ◄   الأعلى: {top.Name} ({top.Count:N0})";
        }

        #endregion

        #region Export

        /// <summary>Exports the current result as UTF-8 (BOM) CSV, which Excel opens directly with Arabic text intact.</summary>
        private void ExportToExcel()
        {
            if (dgvReport.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات لتصديرها. قم بعرض التقرير أولاً.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Filter = "Excel-compatible CSV (*.csv)|*.csv",
                FileName = $"Report_{SelectedTable()}_{DateTime.Now:yyyyMMdd_HHmm}.csv"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                var sb = new StringBuilder();
                var cols = dgvReport.Columns.Cast<DataGridViewColumn>().ToList();
                sb.AppendLine(string.Join(",", cols.Select(c => Csv(c.HeaderText))));
                foreach (DataRow row in dt.Rows)
                    sb.AppendLine(string.Join(",", cols.Select(c => Csv(Convert.ToString(row[c.Name], CultureInfo.InvariantCulture)))));

                File.WriteAllText(dlg.FileName, sb.ToString(), new UTF8Encoding(true));
                MessageBox.Show("تم تصدير التقرير بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل التصدير:\n{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Csv(string? v)
        {
            v ??= "";
            return v.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0 ? $"\"{v.Replace("\"", "\"\"")}\"" : v;
        }

        #endregion
    }
}

