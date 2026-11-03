using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using StudentEmailApp.Models;
using StudentEmailApp.Services;

namespace StudentEmailApp.Forms
{
    public partial class frmMain : Form
    {
        private readonly string _username;
        private readonly string _userRole;

        // =========================================================================
        // MODERN SAAS COLOR PALETTE (Slate/Navy #1E293B + Vibrant Blue #3B82F6)
        // =========================================================================
        private static readonly Color SidebarBgColor      = ColorTranslator.FromHtml("#0F172A"); // Dark Slate/Navy
        private static readonly Color SidebarBrandBg      = ColorTranslator.FromHtml("#0B1220"); // Deepest Slate Brand Header
        private static readonly Color SidebarIdleFgColor  = ColorTranslator.FromHtml("#94A3B8"); // Light Gray Text & Icons
        private static readonly Color SidebarHoverBgColor = ColorTranslator.FromHtml("#1E293B"); // Hover Background
        private static readonly Color SidebarHoverFgColor = ColorTranslator.FromHtml("#FFFFFF"); // White Text & Icons on Hover
        private static readonly Color SidebarActiveBg     = ColorTranslator.FromHtml("#162033"); // Subtle Active Row Tint
        private static readonly Color ActiveBlueAccent    = ColorTranslator.FromHtml("#D4AF37"); // Vibrant Blue Active Indicator & Text/Icon
        private static readonly Color TopBarBgColor       = ColorTranslator.FromHtml("#FFFFFF"); // Clean White Top Bar
        private static readonly Color TopBarBorderColor   = ColorTranslator.FromHtml("#E2E8F0"); // 1px Subtle Bottom Border
        private static readonly Color ContentBgColor      = ColorTranslator.FromHtml("#F8FAFC"); // Off-White Content Background
        private static readonly Color CloseHoverRed       = ColorTranslator.FromHtml("#FF4C4C"); // Close Button Hover Red

        // Sidebar Width Constants
        private const int SidebarExpandedWidth  = 250;
        private const int SidebarCollapsedWidth = 65;
        private bool _isSidebarCollapsed = false;

        // Layout Panels & Controls
        private Panel pnlTopBar = null!;
        private TableLayoutPanel tlpShell = null!;
        private Button btnToggleSidebar = null!;
        private Label lblCurrentPageTitle = null!;
        private Label lblUserBadge = null!;
        private Button btnMinimize = null!;
        private Button btnMaximize = null!;
        private Button btnClose = null!;

        private Panel pnlSidebar = null!;
        private Panel pnlSidebarBrand = null!;
        private PictureBox picBrandIcon = null!;
        private Label lblBrandTitle = null!;
        private Label lblBrandSub = null!;
        private Panel pnlRightAccent = null!;

        private Button btnDashboard = null!;
        private Button btnUndergrad = null!;
        private Button btnPostgrad = null!;
        private Button btnManageData = null!;
        private Button btnReports = null!;
        private Button btnBackup = null!;
        private Button btnSignOut = null!;
        private readonly List<Button> _sidebarButtons = new();

        private Panel pnlContent = null!;
        private ToolTip _sidebarToolTip = null!;

        // Active state & Child Form tracking
        private Button? _currentActiveButton;
        private Form? _activeChildForm;

        // Dragging state for borderless Top Bar
        private bool _isDragging = false;
        private Point _dragStartPoint;

        public frmMain()
            : this(UserSession.CurrentUsername, UserSession.CurrentUserRole)
        {
        }

        public frmMain(string username, string userRole)
        {
            InitializeComponent();

            _username = string.IsNullOrWhiteSpace(username) ? "Guest" : username.Trim();
            _userRole = string.IsNullOrWhiteSpace(userRole) ? "Admin" : userRole.Trim();

            UserSession.CurrentUsername = _username;
            UserSession.CurrentUserRole = _userRole;

            BuildSpaShellUi();

            // Strict layout enforcement (end of constructor).
            // The shell is a TableLayoutPanel (see BuildSpaShellUi), so cells can never overlap,
            // regardless of control z-order. The z-order lines below only guard the Fill panel.
            pnlSidebar.Dock = DockStyle.Fill;
            pnlTopBar.Dock = DockStyle.Fill;
            pnlContent.Dock = DockStyle.Fill;
            PerformLayout();
        }

        /// <summary>
        /// Programmatically builds the RTL SaaS Sidebar navigation layout with Z-Order fix and collapsible sidebar.
        /// </summary>
        private void BuildSpaShellUi()
        {
            SuspendLayout();

            _sidebarToolTip = new ToolTip();

            // =========================================================================
            // 1. FORM PROPERTIES (RTL + Borderless Maximized)
            // =========================================================================
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            // Maximize is applied in frmMain_Load (after the UI is built and the handle/DPI are final).
            this.MinimumSize = new Size(1100, 700);
            this.BackColor = ContentBgColor;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = false;
            this.DoubleBuffered = true;
            this.Text = "نظام إدارة الإيميلات الجامعية — SPA";

            // =========================================================================
            // 2. TOP BAR PANEL (Docked Top, Height 45px, Clean White #FFFFFF + 1px Border #E2E8F0)
            // =========================================================================
            pnlTopBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = TopBarBgColor
            };
            pnlTopBar.Paint += (s, e) =>
            {
                using var bottomBorderPen = new Pen(TopBarBorderColor, 1);
                e.Graphics.DrawLine(bottomBorderPen, 0, pnlTopBar.Height - 1, pnlTopBar.Width, pnlTopBar.Height - 1);
            };
            pnlTopBar.MouseDown += TopBar_MouseDown;
            pnlTopBar.MouseMove += TopBar_MouseMove;
            pnlTopBar.MouseUp   += TopBar_MouseUp;

            // Hamburger Sidebar Toggle Button ('☰') in the Top-Right Corner
            btnToggleSidebar = new Button
            {
                Text = "☰",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#1E293B"),
                BackColor = TopBarBgColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(52, 44),
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnToggleSidebar.FlatAppearance.BorderSize = 0;
            btnToggleSidebar.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#F1F5F9");
            btnToggleSidebar.Click += btnToggleSidebar_Click;
            _sidebarToolTip.SetToolTip(btnToggleSidebar, "طي / توسيع القائمة الجانبية");

            // Current Page Title next to the Hamburger Toggle Button
            lblCurrentPageTitle = new Label
            {
                Text = "الرئيسية (Dashboard)",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F172A"),
                AutoSize = false,
                Width = 380,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 0, 12, 0),
                TextAlign = ContentAlignment.MiddleRight
            };
            lblCurrentPageTitle.MouseDown += TopBar_MouseDown;
            lblCurrentPageTitle.MouseMove += TopBar_MouseMove;
            lblCurrentPageTitle.MouseUp   += TopBar_MouseUp;

            // User Role Badge in Top Bar
            lblUserBadge = new Label
            {
                Text = $"👤 {_username}  |  {GetArabicRoleName(_userRole)}",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                AutoSize = false,
                Width = 260,
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = TopBarBgColor
            };
            lblUserBadge.MouseDown += TopBar_MouseDown;
            lblUserBadge.MouseMove += TopBar_MouseMove;
            lblUserBadge.MouseUp   += TopBar_MouseUp;

            // Window Control Buttons (Close, Maximize, Minimize) docked on the Left for RTL layout
            btnClose = CreateWindowControlButton("✕", isCloseButton: true);
            btnClose.Click += (s, e) => Application.Exit();

            btnMaximize = CreateWindowControlButton("▢", isCloseButton: false);
            btnMaximize.Click += (s, e) =>
            {
                this.WindowState = this.WindowState == FormWindowState.Maximized
                    ? FormWindowState.Normal
                    : FormWindowState.Maximized;
            };

            btnMinimize = CreateWindowControlButton("—", isCloseButton: false);
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            pnlTopBar.Controls.Add(lblUserBadge);
            pnlTopBar.Controls.Add(lblCurrentPageTitle);
            pnlTopBar.Controls.Add(btnToggleSidebar);
            pnlTopBar.Controls.Add(btnMinimize);
            pnlTopBar.Controls.Add(btnMaximize);
            pnlTopBar.Controls.Add(btnClose);

            // =========================================================================
            // 3. RIGHT-ALIGNED SIDEBAR PANEL (Docked Right, Width 250px, Dark Slate #1E293B)
            // =========================================================================
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Right,
                Width = SidebarExpandedWidth,
                BackColor = SidebarBgColor
            };

            // Brand Header inside Right Sidebar
            pnlSidebarBrand = new Panel
            {
                Dock = DockStyle.Top,
                Height = 84,
                BackColor = SidebarBrandBg
            };

            picBrandIcon = new PictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(SidebarExpandedWidth - 56, 22),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = CreateSidebarVectorIcon("brand", ActiveBlueAccent)
            };

            lblBrandTitle = new Label
            {
                Text = "نظام الإيميلات الجامعية",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(176, 24),
                Location = new Point(10, 18),
                TextAlign = ContentAlignment.MiddleRight
            };

            lblBrandSub = new Label
            {
                Text = "جامعة الفيوم — Fayoum Univ",
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = ActiveBlueAccent,
                AutoSize = false,
                Size = new Size(176, 20),
                Location = new Point(10, 44),
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlSidebarBrand.Controls.Add(picBrandIcon);
            pnlSidebarBrand.Controls.Add(lblBrandTitle);
            pnlSidebarBrand.Controls.Add(lblBrandSub);

            // Vibrant 5px Right-Edge Indicator Panel (#3B82F6 on the RIGHT edge of active button)
            pnlRightAccent = new Panel
            {
                Size = new Size(5, 56),
                BackColor = ActiveBlueAccent,
                Visible = false
            };
            pnlSidebar.Controls.Add(pnlRightAccent);

            // Create the 5 Sidebar Navigation Buttons
            btnDashboard  = CreateSidebarButton("الرئيسية", "dashboard");
            btnUndergrad  = CreateSidebarButton("إيميلات البكالوريوس", "undergrad");
            btnPostgrad   = CreateSidebarButton("إيميلات الدراسات العليا", "postgrad");
            btnManageData = CreateSidebarButton("إدارة البيانات", "manage");
            btnReports    = CreateSidebarButton("التقارير والإحصائيات", "reports");
            btnBackup     = CreateSidebarButton("نسخة احتياطية", "backup");
            btnBackup.Dock = DockStyle.Bottom; // sits directly above "تسجيل الخروج"
            btnSignOut    = CreateSidebarButton("تسجيل الخروج", "signout");
            btnSignOut.Dock = DockStyle.Bottom;

            _sidebarButtons.AddRange(new[] { btnDashboard, btnUndergrad, btnPostgrad, btnManageData, btnReports, btnBackup, btnSignOut });

            // Wire Sidebar Button Click Events
            btnDashboard.Click += (s, e) =>
            {
                ActivateButton(btnDashboard);
                lblCurrentPageTitle.Text = "الرئيسية (Dashboard)";
                ShowEmbeddedDashboardHome();
            };

            btnUndergrad.Click += (s, e) =>
            {
                ActivateButton(btnUndergrad);
                lblCurrentPageTitle.Text = "إيميلات البكالوريوس (dbo.mails)";
                var genForm = new frmGenerator("mails", "طلاب البكالوريوس - Undergraduates");
                genForm.FormClosed += (_, _) => ShowEmbeddedDashboardHomeIfClosed(genForm);
                OpenChildForm(genForm);
            };

            btnPostgrad.Click += (s, e) =>
            {
                ActivateButton(btnPostgrad);
                lblCurrentPageTitle.Text = "إيميلات الدراسات العليا (dbo.PostGradMails)";
                var genForm = new frmGenerator("PostGradMails", "الدراسات العليا - Postgraduates");
                genForm.FormClosed += (_, _) => ShowEmbeddedDashboardHomeIfClosed(genForm);
                OpenChildForm(genForm);
            };

            btnManageData.Click += (s, e) =>
            {
                ActivateButton(btnManageData);
                lblCurrentPageTitle.Text = "إدارة وتعديل بيانات الطلاب (Search & Edit)";
                string defaultTable = _userRole.Equals("Postgrad", StringComparison.OrdinalIgnoreCase)
                    ? "PostGradMails"
                    : "mails";
                var manageForm = new frmManageStudents(_username, defaultTable);
                manageForm.FormClosed += (_, _) => ShowEmbeddedDashboardHomeIfClosed(manageForm);
                OpenChildForm(manageForm);
            };

            btnReports.Click += (s, e) =>
            {
                ActivateButton(btnReports);
                lblCurrentPageTitle.Text = "التقارير والإحصائيات (Reports & Analytics)";
                var reportsForm = new frmReports(_username, _userRole);
                reportsForm.FormClosed += (_, _) => ShowEmbeddedDashboardHomeIfClosed(reportsForm);
                OpenChildForm(reportsForm);
            };

            btnBackup.Click += btnBackup_Click; // action button: no page change, so it is not "activated"

            btnSignOut.Click += (s, e) =>
            {
                var confirm = MessageBox.Show(
                    "هل أنت متأكد من تسجيل الخروج؟",
                    "تأكيد تسجيل الخروج",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    UserSession.Clear();
                    this.Close();
                }
            };

            // Add Sidebar Controls (Reverse order for DockStyle.Top stacking)
            pnlSidebar.Controls.Add(btnBackup);
            pnlSidebar.Controls.Add(btnSignOut);
            pnlSidebar.Controls.Add(btnReports);
            pnlSidebar.Controls.Add(btnManageData);
            pnlSidebar.Controls.Add(btnPostgrad);
            pnlSidebar.Controls.Add(btnUndergrad);
            pnlSidebar.Controls.Add(btnDashboard);
            pnlSidebar.Controls.Add(pnlSidebarBrand);

            // =========================================================================
            // 4. CONTENT PANEL (Docked Fill, Off-White #F8FAFC) & Z-ORDER FIX
            // =========================================================================
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ContentBgColor,
                Padding = new Padding(0)
            };

            // Add Panels to Form and explicitly enforce Z-Order so pnlContent (Dock = Fill)
            // NEVER slides under pnlSidebar (Dock = Right) or pnlTopBar (Dock = Top)
            // Shell grid: [ content | sidebar ] x [ top bar / content ]. Sidebar spans both rows.
            // Table cells never overlap, so no control can float over another.
            tlpShell = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                RightToLeft = RightToLeft.No, // keep column 0 = left, column 1 = right
                BackColor = ContentBgColor,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            tlpShell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpShell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, SidebarExpandedWidth));
            tlpShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tlpShell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F)); // Row 2: Footer

            foreach (var p in new[] { pnlTopBar, pnlSidebar, pnlContent })
            {
                p.Dock = DockStyle.Fill;
                p.Margin = Padding.Empty;
                p.RightToLeft = RightToLeft.Yes;
            }

            tlpShell.Controls.Add(pnlTopBar, 0, 0);
            tlpShell.Controls.Add(pnlContent, 0, 1);
            tlpShell.Controls.Add(pnlSidebar, 1, 0);
            tlpShell.SetRowSpan(pnlSidebar, 2);
            // Footer (Row 2, spans both columns, full width)
            var lblFooter = new Label
            {
                Text = "مشروع البوابة الالكترونية 2026",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                TextAlign = ContentAlignment.MiddleCenter,
                RightToLeft = RightToLeft.Yes,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#64748B"),
                BackColor = ColorTranslator.FromHtml("#F1F5F9")
            };
            lblFooter.Paint += (s, e) =>
            {
                using var topBorder = new Pen(TopBarBorderColor, 1);
                e.Graphics.DrawLine(topBorder, 0, 0, lblFooter.Width, 0);
            };
            tlpShell.Controls.Add(lblFooter, 0, 2);
            tlpShell.SetColumnSpan(lblFooter, 2);

            this.Controls.Add(tlpShell);

            this.Load += frmMain_Load;

            ResumeLayout(false);
        }

        #region Collapsible Sidebar Toggle ('☰' Expanded 250px <-> Collapsed 65px)

        private void btnToggleSidebar_Click(object? sender, EventArgs e)
        {
            _isSidebarCollapsed = !_isSidebarCollapsed;
            pnlSidebar.SuspendLayout();

            if (_isSidebarCollapsed)
            {
                // COLLAPSED STATE (65px): Hide text, center icons
                pnlSidebar.Width = SidebarCollapsedWidth;

                lblBrandTitle.Visible = false;
                lblBrandSub.Visible = false;
                picBrandIcon.Location = new Point((SidebarCollapsedWidth - picBrandIcon.Width) / 2, 22);

                foreach (var btn in _sidebarButtons)
                {
                    btn.Text = string.Empty;
                    btn.TextImageRelation = TextImageRelation.Overlay;
                    btn.ImageAlign = ContentAlignment.MiddleCenter;
                    btn.Padding = Padding.Empty;
                }
            }
            else
            {
                // EXPANDED STATE (250px): Restore text and right-aligned layout
                pnlSidebar.Width = SidebarExpandedWidth;

                picBrandIcon.Location = new Point(SidebarExpandedWidth - 56, 22);
                lblBrandTitle.Visible = true;
                lblBrandSub.Visible = true;

                foreach (var btn in _sidebarButtons)
                {
                    string fullTitle = GetSidebarButtonTitle(btn.Tag?.ToString() ?? "");
                    btn.Text = fullTitle + "   ";
                    btn.TextImageRelation = TextImageRelation.TextBeforeImage;
                    btn.TextAlign = ContentAlignment.MiddleRight;
                    btn.ImageAlign = ContentAlignment.MiddleRight;
                    btn.Padding = new Padding(12, 0, 18, 0);
                }
            }

            // Keep the 5px active indicator pinned to the right edge of pnlSidebar
            if (_currentActiveButton != null && pnlRightAccent.Visible)
            {
                pnlRightAccent.Location = new Point(pnlSidebar.Width - pnlRightAccent.Width, _currentActiveButton.Top);
                pnlRightAccent.BringToFront();
            }

            pnlSidebar.ResumeLayout(true);

            // Resize the grid column explicitly. pnlSidebar is Dock=Fill inside its cell, so its own
            // Width is ignored by the layout: the TableLayoutPanel column style is what actually resizes.
            tlpShell.ColumnStyles[1].SizeType = SizeType.Absolute;
            tlpShell.ColumnStyles[1].Width = _isSidebarCollapsed ? SidebarCollapsedWidth : SidebarExpandedWidth;

            // Re-ensure ContentPanel respects the new Sidebar width
            pnlContent.BringToFront();
        }

        private static string GetSidebarButtonTitle(string iconKey) => iconKey switch
        {
            "dashboard" => "الرئيسية",
            "undergrad" => "إيميلات البكالوريوس",
            "postgrad"  => "إيميلات الدراسات العليا",
            "manage"    => "إدارة البيانات",
            "reports"   => "التقارير والإحصائيات",
            "backup"    => "نسخة احتياطية",
            "signout"   => "تسجيل الخروج",
            _           => ""
        };

        #endregion

        #region Sidebar Button Factory, SaaS Hover (#334155 + White) & Active State (#3B82F6)

        private Button CreateSidebarButton(string text, string iconKey)
        {
            var btn = new Button
            {
                Text = text + "   ",
                Tag = iconKey,
                Dock = DockStyle.Top,
                Height = 56,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = SidebarIdleFgColor,
                BackColor = SidebarBgColor,
                RightToLeft = RightToLeft.No,
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleRight,
                TextImageRelation = TextImageRelation.TextBeforeImage,
                Padding = new Padding(12, 0, 18, 0),
                Cursor = Cursors.Hand,
                Image = CreateSidebarVectorIcon(iconKey, SidebarIdleFgColor)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = SidebarHoverBgColor;

            _sidebarToolTip.SetToolTip(btn, text);

            // SaaS Hover Effects: Background -> #334155, Text & Icon -> White (#FFFFFF)
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = SidebarHoverBgColor;
                btn.ForeColor = SidebarHoverFgColor;
                btn.Image = CreateSidebarVectorIcon(iconKey, SidebarHoverFgColor);
            };

            btn.MouseLeave += (s, e) =>
            {
                if (_currentActiveButton == btn)
                {
                    btn.BackColor = SidebarActiveBg;
                    btn.ForeColor = ActiveBlueAccent;
                    btn.Image = CreateSidebarVectorIcon(iconKey, ActiveBlueAccent);
                }
                else
                {
                    btn.BackColor = SidebarBgColor;
                    btn.ForeColor = SidebarIdleFgColor;
                    btn.Image = CreateSidebarVectorIcon(iconKey, SidebarIdleFgColor);
                }
            };

            return btn;
        }

        private void ActivateButton(Button senderBtn)
        {
            // 1. Reset previously active button to Idle (#94A3B8 on #1E293B)
            DisableActiveButton();

            // 2. Highlight newly clicked button with Vibrant Blue (#3B82F6)
            string iconKey = senderBtn.Tag?.ToString() ?? "dashboard";
            _currentActiveButton = senderBtn;
            senderBtn.BackColor = SidebarActiveBg;
            senderBtn.ForeColor = ActiveBlueAccent;
            senderBtn.Image = CreateSidebarVectorIcon(iconKey, ActiveBlueAccent);

            // 3. Position the 5px Vibrant Blue (#3B82F6) indicator on the RIGHT edge of the active button
            pnlRightAccent.Height = senderBtn.Height;
            pnlRightAccent.Location = new Point(pnlSidebar.Width - pnlRightAccent.Width, senderBtn.Top);
            pnlRightAccent.Visible = true;
            pnlRightAccent.BringToFront();
        }

        private void DisableActiveButton()
        {
            if (_currentActiveButton != null)
            {
                string prevIconKey = _currentActiveButton.Tag?.ToString() ?? "dashboard";
                _currentActiveButton.BackColor = SidebarBgColor;
                _currentActiveButton.ForeColor = SidebarIdleFgColor;
                _currentActiveButton.Image = CreateSidebarVectorIcon(prevIconKey, SidebarIdleFgColor);
            }
        }

        #endregion

        #region Child Form Embedding Logic (OpenChildForm) & Embedded Dashboard View

        /// <summary>
        /// Seamlessly embeds any child Form inside the SPA ContentPanel (pnlContent).
        /// </summary>
        public void OpenChildForm(Form childForm)
        {
            if (_activeChildForm != null)
            {
                var prev = _activeChildForm;
                _activeChildForm = null;
                prev.Close();
                prev.Dispose();
            }

            pnlContent.Controls.Clear();

            // Ensure Z-Order so pnlContent respects the right sidebar boundary
            pnlContent.BringToFront();

            _activeChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.WindowState = FormWindowState.Normal;
            childForm.MinimumSize = Size.Empty; // a fixed MinimumSize makes the embedded form overflow (and get clipped by) the content area
            childForm.Dock = DockStyle.Fill;

            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void ShowEmbeddedDashboardHomeIfClosed(Form closedForm)
        {
            if (_activeChildForm == closedForm)
            {
                _activeChildForm = null;
                ActivateButton(btnDashboard);
                lblCurrentPageTitle.Text = "الرئيسية (Dashboard)";
                ShowEmbeddedDashboardHome();
            }
        }

        /// <summary>
        /// Displays the embedded Target Selection Dashboard inside pnlContent when "الرئيسية" is selected.
        /// </summary>
        private void ShowEmbeddedDashboardHome()
        {
            if (_activeChildForm != null)
            {
                var prev = _activeChildForm;
                _activeChildForm = null;
                prev.Close();
                prev.Dispose();
            }

            pnlContent.Controls.Clear();
            pnlContent.BringToFront();

            var pnlHomeWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ContentBgColor
            };

            var pnlCard = new Panel
            {
                Size = new Size(740, 440),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblWelcomeTitle = new Label
            {
                Text = $"مرحباً بك، {_username}",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F172A"),
                AutoSize = false,
                Size = new Size(660, 44),
                Location = new Point(40, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblRoleSub = new Label
            {
                Text = $"الصلاحية الحالية: {GetArabicRoleName(_userRole)} — اختر القسم المطلوب من القائمة الجانبية أو من البطاقات التالية:",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#64748B"),
                AutoSize = false,
                Size = new Size(660, 28),
                Location = new Point(40, 74),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnQuickUndergrad = CreateDashboardQuickButton(
                "🎓  إنشاء إيميلات البكالوريوس (Undergraduates — dbo.mails)",
                new Point(50, 122),
                ActiveBlueAccent,
                isEnabled: btnUndergrad.Visible && btnUndergrad.Enabled
            );
            btnQuickUndergrad.Click += (s, e) => btnUndergrad.PerformClick();

            var btnQuickPostgrad = CreateDashboardQuickButton(
                "🔬  إنشاء إيميلات الدراسات العليا (Postgraduates — dbo.PostGradMails)",
                new Point(50, 224),
                ColorTranslator.FromHtml("#0D9488"),
                isEnabled: btnPostgrad.Visible && btnPostgrad.Enabled
            );
            btnQuickPostgrad.Click += (s, e) => btnPostgrad.PerformClick();

            var btnQuickManage = CreateDashboardQuickButton(
                "🔍  إدارة وتعديل بيانات الطلاب (Search & Edit Student Records)",
                new Point(50, 326),
                ColorTranslator.FromHtml("#4F46E5"),
                isEnabled: true
            );
            btnQuickManage.Click += (s, e) => btnManageData.PerformClick();

            pnlCard.Controls.Add(lblWelcomeTitle);
            pnlCard.Controls.Add(lblRoleSub);
            pnlCard.Controls.Add(btnQuickUndergrad);
            pnlCard.Controls.Add(btnQuickPostgrad);
            pnlCard.Controls.Add(btnQuickManage);

            void CenterHomeCard()
            {
                int x = Math.Max(20, (pnlHomeWrapper.ClientSize.Width - pnlCard.Width) / 2);
                int y = Math.Max(20, (pnlHomeWrapper.ClientSize.Height - pnlCard.Height) / 2);
                pnlCard.Location = new Point(x, y);
            }

            pnlHomeWrapper.Resize += (s, e) => CenterHomeCard();
            pnlHomeWrapper.Controls.Add(pnlCard);
            pnlContent.Controls.Add(pnlHomeWrapper);
            CenterHomeCard();
        }

        private static Button CreateDashboardQuickButton(string text, Point location, Color accentColor, bool isEnabled)
        {
            var btn = new Button
            {
                Text = isEnabled ? text : $"🔒 {text} — (غير مصرح لهذه الصلاحية)",
                Location = location,
                Size = new Size(640, 84),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                BackColor = isEnabled ? Color.White : ColorTranslator.FromHtml("#E2E8F0"),
                ForeColor = isEnabled ? accentColor : ColorTranslator.FromHtml("#94A3B8"),
                Enabled = isEnabled,
                Cursor = isEnabled ? Cursors.Hand : Cursors.No,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = TopBarBorderColor;
            if (isEnabled)
            {
                btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#F1F5F9");
            }
            return btn;
        }

        #endregion

        #region Database Backup (نسخة احتياطية)

        // Placeholder used only if the connection string has no "Database=" value.
        // Replace with your actual database name if needed.
        private const string FallbackDatabaseName = "YourDatabaseName";

        private async void btnBackup_Click(object? sender, EventArgs e)
        {
            // Database name comes from the existing connection string (Initial Catalog / Database)
            string dbName = new SqlConnectionStringBuilder(DbHelper.ConnectionString).InitialCatalog;
            if (string.IsNullOrWhiteSpace(dbName)) dbName = FallbackDatabaseName;

            using var dlg = new SaveFileDialog
            {
                Title = "اختر مكان حفظ النسخة الاحتياطية",
                Filter = "SQL Server Backup (*.bak)|*.bak",
                DefaultExt = "bak",
                AddExtension = true,
                FileName = $"FayoumMailDB_{DateTime.Now:yyyyMMdd}.bak",
                OverwritePrompt = true
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            string backupPath = dlg.FileName;
            // Identifiers can't be parameterized; escape ']' so the name stays a single bracketed identifier
            string safeDb = dbName.Replace("]", "]]");
            string sql = $"BACKUP DATABASE [{safeDb}] TO DISK = @BackupPath";

            try
            {
                Cursor = Cursors.WaitCursor;
                btnBackup.Enabled = false;

                using var conn = DbHelper.GetConnection();
                await conn.OpenAsync();

                using var cmd = new SqlCommand(sql, conn) { CommandTimeout = 0 }; // large DBs can take minutes
                cmd.Parameters.Add("@BackupPath", SqlDbType.NVarChar, 260).Value = backupPath;
                await cmd.ExecuteNonQueryAsync();

                MessageBox.Show(
                    $"تم إنشاء النسخة الاحتياطية بنجاح\n\n{backupPath}",
                    "نسخة احتياطية",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            catch (SqlException ex)
            {
                // 3201 = cannot open backup device, 3013 = backup terminated; OS error 5 = access denied
                bool accessDenied = ex.Number == 3201
                    || ex.Message.Contains("Access is denied", StringComparison.OrdinalIgnoreCase)
                    || ex.Message.Contains("Operating system error 5", StringComparison.OrdinalIgnoreCase);

                string msg = accessDenied
                    ? "تعذر حفظ النسخة الاحتياطية: خدمة SQL Server لا تملك صلاحية الكتابة في هذا المجلد.\n\n" +
                      "الحل: اختر مجلداً عاماً أو في جذر القرص مثل D:\\ أو C:\\Backups ثم أعد المحاولة.\n\n" +
                      $"تفاصيل تقنية:\n{ex.Message}"
                    : $"حدث خطأ أثناء إنشاء النسخة الاحتياطية:\n{ex.Message}";

                MessageBox.Show(msg, "فشل النسخ الاحتياطي", MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ غير متوقع:\n{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnBackup.Enabled = true;
            }
        }

        #endregion

        #region Role-Based Access Control (RBAC) & Form Load

        private void frmMain_Load(object? sender, EventArgs e)
        {
            // Borderless forms maximize over the taskbar unless bounds are limited to the working area
            this.MaximizedBounds = Screen.FromControl(this).WorkingArea;
            this.WindowState = FormWindowState.Maximized;
            // Enforce Z-order so ContentPanel (Dock = Fill) never overlaps the Right Sidebar
            pnlContent.BringToFront();

            ApplySidebarRbac();

            // Default view on startup: Activate "الرئيسية" (Dashboard)
            ActivateButton(btnDashboard);
            ShowEmbeddedDashboardHome();
        }

        private void ApplySidebarRbac()
        {
            btnDashboard.Visible = true;
            btnDashboard.Enabled = true;

            btnSignOut.Visible = true;
            btnSignOut.Enabled = true;

            btnManageData.Visible = true;
            btnManageData.Enabled = true;

            // Reports expose per-user productivity, so they are Admin-only
            bool isAdminRole = _userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            btnReports.Visible = isAdminRole;
            btnReports.Enabled = isAdminRole;
            btnBackup.Visible = isAdminRole;   // backups are Admin-only
            btnBackup.Enabled = isAdminRole;

            if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                btnUndergrad.Visible = true;
                btnUndergrad.Enabled = true;

                btnPostgrad.Visible = true;
                btnPostgrad.Enabled = true;
            }
            else if (_userRole.Equals("Undergrad", StringComparison.OrdinalIgnoreCase))
            {
                btnUndergrad.Visible = true;
                btnUndergrad.Enabled = true;

                btnPostgrad.Visible = false;
                btnPostgrad.Enabled = false;
            }
            else if (_userRole.Equals("Postgrad", StringComparison.OrdinalIgnoreCase))
            {
                btnUndergrad.Visible = false;
                btnUndergrad.Enabled = false;

                btnPostgrad.Visible = true;
                btnPostgrad.Enabled = true;
            }
            else
            {
                btnUndergrad.Visible = false;
                btnPostgrad.Visible = false;
            }
        }

        private static string GetArabicRoleName(string role)
        {
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                return "مدير النظام - Admin";
            if (role.Equals("Undergrad", StringComparison.OrdinalIgnoreCase))
                return "مسؤول البكالوريوس - Undergrad";
            if (role.Equals("Postgrad", StringComparison.OrdinalIgnoreCase))
                return "مسؤول الدراسات العليا - Postgrad";
            return role;
        }

        #endregion

        #region Custom Top Bar Buttons & Dragging Logic

        private static Button CreateWindowControlButton(string symbol, bool isCloseButton)
        {
            var btn = new Button
            {
                Text = symbol,
                Dock = DockStyle.Left,
                Width = 46,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                BackColor = TopBarBgColor,
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btn.FlatAppearance.BorderSize = 0;

            if (isCloseButton)
            {
                btn.FlatAppearance.MouseOverBackColor = CloseHoverRed;
                btn.MouseEnter += (s, e) =>
                {
                    btn.BackColor = CloseHoverRed;
                    btn.ForeColor = Color.White;
                };
                btn.MouseLeave += (s, e) =>
                {
                    btn.BackColor = TopBarBgColor;
                    btn.ForeColor = ColorTranslator.FromHtml("#475569");
                };
            }
            else
            {
                btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#E2E8F0");
            }

            return btn;
        }

        private void TopBar_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragStartPoint = new Point(e.X, e.Y);
            }
        }

        private void TopBar_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(
                    currentScreenPos.X - _dragStartPoint.X,
                    currentScreenPos.Y - _dragStartPoint.Y
                );
            }
        }

        private void TopBar_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
            }
        }

        #endregion

        #region High-DPI Vector Icon Renderer (#94A3B8 Idle / #FFFFFF Hover / #3B82F6 Active)

        private static Bitmap CreateSidebarVectorIcon(string iconKey, Color color)
        {
            var bmp = new Bitmap(26, 26);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var pen = new Pen(color, 2.0f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };

            switch (iconKey)
            {
                case "dashboard":
                    g.DrawRectangle(pen, 4, 4, 7, 7);
                    g.DrawRectangle(pen, 15, 4, 7, 7);
                    g.DrawRectangle(pen, 4, 15, 7, 7);
                    g.DrawRectangle(pen, 15, 15, 7, 7);
                    break;

                case "undergrad":
                    PointF[] capTop =
                    {
                        new(13, 5),
                        new(23, 10),
                        new(13, 15),
                        new(3, 10)
                    };
                    g.DrawPolygon(pen, capTop);
                    g.DrawLines(pen, new PointF[] { new(7, 12.5f), new(7, 19), new(13, 21.5f), new(19, 19), new(19, 12.5f) });
                    g.DrawLine(pen, 22, 10.5f, 22, 18);
                    break;

                case "postgrad":
                    g.DrawRectangle(pen, 4, 5, 18, 13);
                    g.DrawLine(pen, 8, 9, 18, 9);
                    g.DrawLine(pen, 8, 13, 14, 13);
                    g.DrawEllipse(pen, 14, 14, 6, 6);
                    break;

                case "manage":
                    g.DrawEllipse(pen, 5, 4, 16, 6);
                    g.DrawArc(pen, 5, 9, 16, 6, 0, 180);
                    g.DrawArc(pen, 5, 14, 16, 6, 0, 180);
                    g.DrawLine(pen, 5, 7, 5, 17);
                    g.DrawLine(pen, 21, 7, 21, 17);
                    break;

                case "reports":
                    g.DrawLine(pen, 4, 22, 22, 22);
                    g.DrawRectangle(pen, 5, 13, 4, 9);
                    g.DrawRectangle(pen, 11, 7, 4, 15);
                    g.DrawRectangle(pen, 17, 10, 4, 12);
                    break;

                case "backup": // database cylinder + down arrow
                    g.DrawEllipse(pen, 4, 3, 14, 5);
                    g.DrawLine(pen, 4, 5.5f, 4, 15);
                    g.DrawLine(pen, 18, 5.5f, 18, 11);
                    g.DrawArc(pen, 4, 10, 14, 5, 0, 180);
                    g.DrawLine(pen, 19, 14, 19, 23);
                    g.DrawLines(pen, new PointF[] { new(15, 19.5f), new(19, 23.5f), new(23, 19.5f) });
                    break;

                case "signout":
                    g.DrawLines(pen, new PointF[] { new(14, 4), new(5, 4), new(5, 22), new(14, 22) });
                    g.DrawLine(pen, 10, 13, 22, 13);
                    g.DrawLines(pen, new PointF[] { new(18, 9), new(22, 13), new(18, 17) });
                    break;

                default:
                    g.DrawRectangle(pen, 3, 6, 20, 14);
                    g.DrawLines(pen, new PointF[] { new(3, 6), new(13, 14), new(23, 6) });
                    break;
            }

            return bmp;
        }

        #endregion
    }
}
