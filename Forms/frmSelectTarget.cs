using System;
using System.Drawing;
using System.Windows.Forms;
using StudentEmailApp.Models;

namespace StudentEmailApp.Forms
{
    public partial class frmSelectTarget : Form
    {
        private readonly string _username;
        private readonly string _userRole;

        // Parameterless constructor for Designer / fallback support
        public frmSelectTarget()
            : this(UserSession.CurrentUsername, UserSession.CurrentUserRole)
        {
        }

        // Constructor receiving Username and UserRole from frmLogin
        public frmSelectTarget(string username, string userRole)
        {
            InitializeComponent();

            _username = string.IsNullOrWhiteSpace(username) ? "Guest" : username.Trim();
            _userRole = string.IsNullOrWhiteSpace(userRole) ? "Admin" : userRole.Trim();

            UserSession.CurrentUsername = _username;
            UserSession.CurrentUserRole = _userRole;

            // Open in Full Screen (Maximized) mode
            this.WindowState = FormWindowState.Maximized;
        }

        private void frmSelectTarget_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {_username}  |  Role: {_userRole}";

            ApplyRoleBasedAccessControl();
            CenterDashboardCard();
        }

        private void frmSelectTarget_Resize(object? sender, EventArgs e)
        {
            CenterDashboardCard();
        }

        /// <summary>
        /// Dynamically centers the main dashboard container (pnlCenterCard) both horizontally
        /// and vertically within the workspace area below pnlHeader, regardless of screen resolution.
        /// </summary>
        private void CenterDashboardCard()
        {
            if (pnlCenterCard == null || pnlHeader == null)
                return;

            int availableWidth = this.ClientSize.Width;
            int availableHeight = this.ClientSize.Height - pnlHeader.Height;

            int centerX = Math.Max(20, (availableWidth - pnlCenterCard.Width) / 2);
            int centerY = pnlHeader.Height + Math.Max(20, (availableHeight - pnlCenterCard.Height) / 2);

            pnlCenterCard.Location = new Point(centerX, centerY);
        }

        /// <summary>
        /// Evaluates _userRole ("Admin", "Undergrad", "Postgrad") and enables/disables
        /// the Undergraduate and Postgraduate buttons with clear visual styling.
        /// </summary>
        private void ApplyRoleBasedAccessControl()
        {
            if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin: Both buttons are visible and enabled
                SetButtonAccessState(
                    btnUndergrad,
                    isAllowed: true,
                    activeForeColor: Color.FromArgb(30, 64, 175),
                    activeHoverColor: Color.FromArgb(238, 242, 255),
                    baseText: "🎓  Create Emails for Undergraduates\n(Database Table: mails)"
                );

                SetButtonAccessState(
                    btnPostgrad,
                    isAllowed: true,
                    activeForeColor: Color.FromArgb(15, 118, 110),
                    activeHoverColor: Color.FromArgb(240, 253, 250),
                    baseText: "🔬  Create Emails for Postgraduates\n(Database Table: PostGradMails)"
                );
            }
            else if (_userRole.Equals("Undergrad", StringComparison.OrdinalIgnoreCase))
            {
                // Undergrad Role: Enable Undergraduate button, disable Postgraduate button
                SetButtonAccessState(
                    btnUndergrad,
                    isAllowed: true,
                    activeForeColor: Color.FromArgb(30, 64, 175),
                    activeHoverColor: Color.FromArgb(238, 242, 255),
                    baseText: "🎓  Create Emails for Undergraduates\n(Database Table: mails)"
                );

                SetButtonAccessState(
                    btnPostgrad,
                    isAllowed: false,
                    activeForeColor: Color.FromArgb(15, 118, 110),
                    activeHoverColor: Color.FromArgb(240, 253, 250),
                    baseText: "🔒  Create Emails for Postgraduates — [Restricted Access / غير مصرح]\n(Database Table: PostGradMails)"
                );
            }
            else if (_userRole.Equals("Postgrad", StringComparison.OrdinalIgnoreCase))
            {
                // Postgrad Role: Disable Undergraduate button, enable Postgraduate button
                SetButtonAccessState(
                    btnUndergrad,
                    isAllowed: false,
                    activeForeColor: Color.FromArgb(30, 64, 175),
                    activeHoverColor: Color.FromArgb(238, 242, 255),
                    baseText: "🔒  Create Emails for Undergraduates — [Restricted Access / غير مصرح]\n(Database Table: mails)"
                );

                SetButtonAccessState(
                    btnPostgrad,
                    isAllowed: true,
                    activeForeColor: Color.FromArgb(15, 118, 110),
                    activeHoverColor: Color.FromArgb(240, 253, 250),
                    baseText: "🔬  Create Emails for Postgraduates\n(Database Table: PostGradMails)"
                );
            }
            else
            {
                // Least-privilege fallback if an unrecognized role is encountered
                SetButtonAccessState(
                    btnUndergrad,
                    isAllowed: false,
                    activeForeColor: Color.FromArgb(30, 64, 175),
                    activeHoverColor: Color.FromArgb(238, 242, 255),
                    baseText: "🔒  Create Emails for Undergraduates — [Restricted Access / غير مصرح]\n(Database Table: mails)"
                );

                SetButtonAccessState(
                    btnPostgrad,
                    isAllowed: false,
                    activeForeColor: Color.FromArgb(15, 118, 110),
                    activeHoverColor: Color.FromArgb(240, 253, 250),
                    baseText: "🔒  Create Emails for Postgraduates — [Restricted Access / غير مصرح]\n(Database Table: PostGradMails)"
                );
            }
        }

        /// <summary>
        /// Configures the enabled state and visual appearance (active vs. grayed-out restricted) for a dashboard button.
        /// </summary>
        private static void SetButtonAccessState(
            Button button,
            bool isAllowed,
            Color activeForeColor,
            Color activeHoverColor,
            string baseText)
        {
            button.Visible = true;
            button.Enabled = isAllowed;
            button.Text = baseText;

            if (isAllowed)
            {
                button.BackColor = Color.White;
                button.ForeColor = activeForeColor;
                button.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                button.FlatAppearance.MouseOverBackColor = activeHoverColor;
                button.Cursor = Cursors.Hand;
            }
            else
            {
                // Visual restricted style: grayed-out background, muted slate border & text
                button.BackColor = Color.FromArgb(226, 232, 240);
                button.ForeColor = Color.FromArgb(148, 163, 184);
                button.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                button.Cursor = Cursors.No;
            }
        }

        private void btnUndergrad_Click(object sender, EventArgs e)
        {
            // Open Form 3 in Maximized mode targeting the Undergraduates table ("mails")
            this.Hide();
            using var generatorForm = new frmGenerator("mails", "Undergraduate Students")
            {
                WindowState = FormWindowState.Maximized
            };
            generatorForm.ShowDialog();
            this.Show();
        }

        private void btnPostgrad_Click(object sender, EventArgs e)
        {
            // Open Form 3 in Maximized mode targeting the Postgraduates table ("PostGradMails")
            this.Hide();
            using var generatorForm = new frmGenerator("PostGradMails", "Postgraduate Students")
            {
                WindowState = FormWindowState.Maximized
            };
            generatorForm.ShowDialog();
            this.Show();
        }

        private void btnManageStudents_Click(object sender, EventArgs e)
        {
            // Determine target table based on user role ("PostGradMails" for Postgrad role, "mails" otherwise)
            string defaultTableName = _userRole.Equals("Postgrad", StringComparison.OrdinalIgnoreCase)
                ? "PostGradMails"
                : "mails";

            this.Hide();
            using var manageForm = new frmManageStudents(_username, defaultTableName)
            {
                WindowState = FormWindowState.Maximized
            };
            manageForm.ShowDialog();
            this.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Sign out and return to Login
            UserSession.Clear();
            this.Close();
        }
    }
}
