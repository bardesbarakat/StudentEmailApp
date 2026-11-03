using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using StudentEmailApp.Models;
using StudentEmailApp.Services;

namespace StudentEmailApp.Forms
{
    public partial class frmLogin : Form
    {
        // Connection string pointing to the SQL Server database "StudentMails"
        private readonly string connectionString = DbHelper.ConnectionString;

        // Modern Minimal Color Palette (Royal Blue #4169E1 & Clean White)
        private static readonly Color PrimaryColor       = Color.FromArgb(65, 105, 225);  // Royal Blue (#4169E1)
        private static readonly Color PrimaryHoverColor  = Color.FromArgb(45, 82, 196);   // Darker Royal Blue on Hover
        private static readonly Color TitleBarBackColor  = Color.FromArgb(248, 250, 252); // Subtle Slate Header
        private static readonly Color UnderlineIdleColor = Color.FromArgb(203, 213, 225); // Light Slate Gray
        private static readonly Color TextDarkColor      = Color.FromArgb(15, 23, 42);    // Deep Slate Text
        private static readonly Color TextMutedColor     = Color.FromArgb(100, 116, 139); // Muted Subtitle Text
        private static readonly Color DangerRedColor     = Color.FromArgb(220, 38, 38);   // Close Button Hover & Error Text

        // UI Controls built programmatically
        private Panel pnlTitleBar = null!;
        private Label lblWindowTitle = null!;
        private Button btnClose = null!;
        private Panel pnlAccentStripe = null!;
        private Panel pnlContent = null!;
        private PictureBox picBrandLogo = null!;
        private Label lblHeaderTitle = null!;
        private Label lblHeaderSub = null!;

        private Label lblUsername = null!;
        private PictureBox picUserIcon = null!;
        private TextBox txtUsername = null!;
        private Panel pnlUserUnderline = null!;

        private Label lblPassword = null!;
        private PictureBox picPassIcon = null!;
        private TextBox txtPassword = null!;
        private Button btnTogglePassword = null!;
        private Panel pnlPassUnderline = null!;

        private Button btnLogin = null!;
        private Label lblError = null!;

        // Dragging state for the borderless title bar
        private bool _isDragging = false;
        private Point _dragStartPoint;

        public frmLogin()
        {
            InitializeComponent();
            BuildModernLoginUi();
        }

        /// <summary>
        /// Programmatically builds and styles the modern borderless Arabic Login UI.
        /// </summary>
        private void BuildModernLoginUi()
        {
            SuspendLayout();

            // =========================================================================
            // 1. MODERN BORDERLESS FORM CONFIGURATION
            // =========================================================================
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(440, 540);
            this.Padding = new Padding(2); // Leaves a 2px inset on all 4 sides so docked panels don't cover the custom border
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = false;
            this.DoubleBuffered = true;
            this.Text = "تسجيل الدخول - نظام الإيميلات الجامعية";

            // =========================================================================
            // 2. CUSTOM TOP TITLE BAR WITH DRAG LOGIC & HOVER RED "X" CLOSE BUTTON
            // =========================================================================
            pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = TitleBarBackColor,
                Cursor = Cursors.SizeAll
            };
            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            pnlTitleBar.MouseMove += TitleBar_MouseMove;
            pnlTitleBar.MouseUp   += TitleBar_MouseUp;

            lblWindowTitle = new Label
            {
                Text = "نظام إدارة الإيميلات الجامعية",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = TextMutedColor,
                AutoSize = true,
                Location = new Point(215, 10),
                Cursor = Cursors.SizeAll
            };
            lblWindowTitle.MouseDown += TitleBar_MouseDown;
            lblWindowTitle.MouseMove += TitleBar_MouseMove;
            lblWindowTitle.MouseUp   += TitleBar_MouseUp;

            btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = TextMutedColor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(46, 42),
                Location = new Point(this.ClientSize.Width - 46, 0),
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = DangerRedColor;
            btnClose.MouseEnter += (s, e) =>
            {
                btnClose.BackColor = DangerRedColor;
                btnClose.ForeColor = Color.White;
            };
            btnClose.MouseLeave += (s, e) =>
            {
                btnClose.BackColor = Color.Transparent;
                btnClose.ForeColor = TextMutedColor;
            };
            btnClose.Click += (s, e) => Application.Exit();

            pnlTitleBar.Controls.Add(lblWindowTitle);
            pnlTitleBar.Controls.Add(btnClose);

            // Top Royal Blue accent bar underneath the title bar
            pnlAccentStripe = new Panel
            {
                Dock = DockStyle.Top,
                Height = 3,
                BackColor = PrimaryColor
            };

            // =========================================================================
            // 3. MAIN CONTENT AREA & BRAND HEADER
            // =========================================================================
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(40, 20, 40, 20)
            };

            picBrandLogo = new PictureBox
            {
                Size = new Size(64, 64),
                Location = new Point((this.ClientSize.Width - 64) / 2, 24),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = CreateShieldUserBadgeIcon(64, PrimaryColor)
            };

            lblHeaderTitle = new Label
            {
                Text = "تسجيل الدخول",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = TextDarkColor,
                AutoSize = false,
                Size = new Size(360, 40),
                Location = new Point(40, 96),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblHeaderSub = new Label
            {
                Text = "يرجى إدخال اسم المستخدم وكلمة المرور للمتابعة",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = TextMutedColor,
                AutoSize = false,
                Size = new Size(360, 25),
                Location = new Point(40, 136),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // =========================================================================
            // 4. INTERACTIVE USERNAME FIELD (ENGLISH LABEL/PLACEHOLDER + LEFT ICON + UNDERLINE)
            // =========================================================================
            lblUsername = new Label
            {
                Text = "Username",
                RightToLeft = RightToLeft.No,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = TextDarkColor,
                AutoSize = false,
                Size = new Size(350, 22),
                Location = new Point(45, 182),
                TextAlign = ContentAlignment.MiddleLeft
            };

            picUserIcon = new PictureBox
            {
                Size = new Size(28, 28),
                Location = new Point(45, 210),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = CreateFieldIcon("user", TextMutedColor, isFocused: false)
            };

            txtUsername = new TextBox
            {
                BorderStyle = BorderStyle.None,
                RightToLeft = RightToLeft.No,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Regular),
                ForeColor = TextDarkColor,
                BackColor = Color.White,
                Location = new Point(82, 214),
                Size = new Size(313, 25),
                TextAlign = HorizontalAlignment.Left,
                PlaceholderText = "Enter your username",
                TabIndex = 0
            };

            pnlUserUnderline = new Panel
            {
                Location = new Point(45, 244),
                Size = new Size(350, 2),
                BackColor = UnderlineIdleColor
            };

            txtUsername.Enter += (s, e) =>
            {
                pnlUserUnderline.BackColor = PrimaryColor;
                pnlUserUnderline.Height = 3;
                lblUsername.ForeColor = PrimaryColor;
                picUserIcon.Image = CreateFieldIcon("user", PrimaryColor, isFocused: true);
            };
            txtUsername.Leave += (s, e) =>
            {
                pnlUserUnderline.BackColor = UnderlineIdleColor;
                pnlUserUnderline.Height = 2;
                lblUsername.ForeColor = TextDarkColor;
                picUserIcon.Image = CreateFieldIcon("user", TextMutedColor, isFocused: false);
            };

            // =========================================================================
            // 5. INTERACTIVE PASSWORD FIELD (ENGLISH LABEL/PLACEHOLDER + LEFT ICON + TOGGLE + UNDERLINE)
            // =========================================================================
            lblPassword = new Label
            {
                Text = "Password",
                RightToLeft = RightToLeft.No,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = TextDarkColor,
                AutoSize = false,
                Size = new Size(350, 22),
                Location = new Point(45, 268),
                TextAlign = ContentAlignment.MiddleLeft
            };

            picPassIcon = new PictureBox
            {
                Size = new Size(28, 28),
                Location = new Point(45, 296),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = CreateFieldIcon("lock", TextMutedColor, isFocused: false)
            };

            txtPassword = new TextBox
            {
                BorderStyle = BorderStyle.None,
                RightToLeft = RightToLeft.No,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Regular),
                ForeColor = TextDarkColor,
                BackColor = Color.White,
                Location = new Point(82, 300),
                Size = new Size(276, 25),
                TextAlign = HorizontalAlignment.Left,
                PlaceholderText = "Enter your password",
                UseSystemPasswordChar = true,
                TabIndex = 1
            };

            btnTogglePassword = new Button
            {
                Text = "👁",
                RightToLeft = RightToLeft.No,
                Font = new Font("Segoe UI Symbol", 10.5F, FontStyle.Regular),
                ForeColor = TextMutedColor,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(32, 26),
                Location = new Point(363, 298),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnTogglePassword.FlatAppearance.BorderSize = 0;
            btnTogglePassword.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
            btnTogglePassword.Click += BtnTogglePassword_Click;

            pnlPassUnderline = new Panel
            {
                Location = new Point(45, 330),
                Size = new Size(350, 2),
                BackColor = UnderlineIdleColor
            };

            txtPassword.Enter += (s, e) =>
            {
                pnlPassUnderline.BackColor = PrimaryColor;
                pnlPassUnderline.Height = 3;
                lblPassword.ForeColor = PrimaryColor;
                picPassIcon.Image = CreateFieldIcon("lock", PrimaryColor, isFocused: true);
            };
            txtPassword.Leave += (s, e) =>
            {
                pnlPassUnderline.BackColor = UnderlineIdleColor;
                pnlPassUnderline.Height = 2;
                lblPassword.ForeColor = TextDarkColor;
                picPassIcon.Image = CreateFieldIcon("lock", TextMutedColor, isFocused: false);
            };

            // =========================================================================
            // 6. INTERACTIVE FLAT LOGIN BUTTON ("تسجيل الدخول") WITH HOVER EFFECTS
            // =========================================================================
            btnLogin = new Button
            {
                Text = "تسجيل الدخول",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(350, 46),
                Location = new Point(45, 366),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                TabIndex = 2
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatAppearance.MouseOverBackColor = PrimaryHoverColor;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = PrimaryHoverColor;
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = PrimaryColor;
            btnLogin.Click += btnLogin_Click;

            // =========================================================================
            // 7. ARABIC FEEDBACK / ERROR MESSAGE LABEL
            // =========================================================================
            lblError = new Label
            {
                Text = "",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = DangerRedColor,
                AutoSize = false,
                Size = new Size(350, 56),
                Location = new Point(45, 422),
                TextAlign = ContentAlignment.TopCenter
            };

            // Add controls to Content Panel
            pnlContent.Controls.Add(picBrandLogo);
            pnlContent.Controls.Add(lblHeaderTitle);
            pnlContent.Controls.Add(lblHeaderSub);
            pnlContent.Controls.Add(lblUsername);
            pnlContent.Controls.Add(picUserIcon);
            pnlContent.Controls.Add(txtUsername);
            pnlContent.Controls.Add(pnlUserUnderline);
            pnlContent.Controls.Add(lblPassword);
            pnlContent.Controls.Add(picPassIcon);
            pnlContent.Controls.Add(txtPassword);
            pnlContent.Controls.Add(btnTogglePassword);
            pnlContent.Controls.Add(pnlPassUnderline);
            pnlContent.Controls.Add(btnLogin);
            pnlContent.Controls.Add(lblError);

            // Add Panels to Form
            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlAccentStripe);
            this.Controls.Add(pnlTitleBar);

            this.AcceptButton = btnLogin;

            ResumeLayout(false);
        }

        #region Custom Form Border (OnPaint Override), Title Bar Dragging & Password Toggle

        /// <summary>
        /// Draws a clean 2-pixel border around the exact edges of the borderless form.
        /// Combined with this.Padding = new Padding(2), docked child panels never overlap the border.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderWidth = 2;
            ControlPaint.DrawBorder(
                e.Graphics,
                this.ClientRectangle,
                PrimaryColor, borderWidth, ButtonBorderStyle.Solid, // Left
                PrimaryColor, borderWidth, ButtonBorderStyle.Solid, // Top
                PrimaryColor, borderWidth, ButtonBorderStyle.Solid, // Right
                PrimaryColor, borderWidth, ButtonBorderStyle.Solid  // Bottom
            );
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        private void TitleBar_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragStartPoint = new Point(e.X, e.Y);
            }
        }

        private void TitleBar_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(
                    currentScreenPos.X - _dragStartPoint.X,
                    currentScreenPos.Y - _dragStartPoint.Y
                );
            }
        }

        private void TitleBar_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
            }
        }

        private void BtnTogglePassword_Click(object? sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.ForeColor = txtPassword.UseSystemPasswordChar
                ? TextMutedColor
                : PrimaryColor;
            txtPassword.Focus();
        }

        #endregion

        #region Vector Icon Helpers (No External Assets Needed)

        private static Bitmap CreateShieldUserBadgeIcon(int size, Color color)
        {
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var bgBrush = new SolidBrush(Color.FromArgb(238, 242, 255));
            g.FillEllipse(bgBrush, 2, 2, size - 4, size - 4);

            using var pen = new Pen(color, 2.5f);
            // Head
            g.DrawEllipse(pen, size / 2f - 9, size / 2f - 14, 18, 18);
            // Shoulders arc
            g.DrawArc(pen, size / 2f - 16, size / 2f + 6, 32, 22, 180, 180);

            return bmp;
        }

        private static Bitmap CreateFieldIcon(string iconType, Color color, bool isFocused)
        {
            var bmp = new Bitmap(28, 28);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Soft circular badge behind the icon
            Color bgTint = isFocused
                ? Color.FromArgb(235, 242, 255)
                : Color.FromArgb(241, 245, 249);
            using (var bgBrush = new SolidBrush(bgTint))
            {
                g.FillEllipse(bgBrush, 1, 1, 26, 26);
            }

            using var pen = new Pen(color, 1.8f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };

            if (iconType == "user")
            {
                // User head & shoulders profile icon
                g.DrawEllipse(pen, 10, 6, 8, 8);
                g.DrawArc(pen, 6, 15, 16, 10, 180, 180);
            }
            else if (iconType == "lock")
            {
                // Padlock shackle, body & keyhole dot
                g.DrawArc(pen, 9, 6, 10, 9, 180, 180);
                g.DrawRectangle(pen, 7, 12, 14, 10);
                using var dotBrush = new SolidBrush(color);
                g.FillEllipse(dotBrush, 12.5f, 15.5f, 3f, 3f);
            }

            return bmp;
        }

        #endregion

        #region Authentication & Audit Logging

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            lblError.Text = "";

            // 1. Validate inputs in Arabic
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblError.Text = "يرجى إدخال اسم المستخدم وكلمة المرور.";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "جاري التحقق...";

            try
            {
                bool isAuthenticated = false;
                string userRole = "Admin";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Using SELECT * so it safely reads UserRole (or Role) without failing if the column name varies
                    const string query = "SELECT * FROM dbo.Users WHERE Username = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbPassword = reader["Password"]?.ToString() ?? "";

                                if (dbPassword == password)
                                {
                                    isAuthenticated = true;
                                    UserSession.CurrentUserId = Convert.ToInt32(reader["UserID"]);
                                    userRole = ReadUserRoleFromReader(reader, username);
                                }
                            }
                        }
                    }

                    if (isAuthenticated)
                    {
                        UserSession.CurrentUsername = username;
                        UserSession.CurrentUserRole = userRole;

                        // Pass Username and UserRole to the Main SPA Sidebar Navigation Form
                        this.Hide();
                        using (frmMain mainForm = new frmMain(username, userRole))
                        {
                            mainForm.ShowDialog();
                        }

                        txtPassword.Clear();
                        txtPassword.UseSystemPasswordChar = true;
                        lblError.Text = "";
                        this.Show();
                        txtUsername.Focus();
                    }
                    else
                    {
                        lblError.Text = "اسم المستخدم أو كلمة المرور غير صحيحة. يرجى المحاولة مرة أخرى.";
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                lblError.Text = $"خطأ في الاتصال بقاعدة البيانات:\n{sqlEx.Message}";
            }
            catch (Exception ex)
            {
                lblError.Text = $"حدث خطأ غير متوقع:\n{ex.Message}";
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "تسجيل الدخول";
            }
        }

        /// <summary>
        /// Safely extracts the user's role ("Admin", "Undergrad", or "Postgrad") from the SqlDataReader.
        /// </summary>
        private static string ReadUserRoleFromReader(SqlDataReader reader, string username)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string colName = reader.GetName(i);
                if (colName.Equals("UserRole", StringComparison.OrdinalIgnoreCase) ||
                    colName.Equals("Role", StringComparison.OrdinalIgnoreCase) ||
                    colName.Equals("user_role", StringComparison.OrdinalIgnoreCase))
                {
                    string? rawRole = reader.IsDBNull(i) ? null : reader.GetValue(i)?.ToString()?.Trim();
                    if (!string.IsNullOrWhiteSpace(rawRole))
                    {
                        return rawRole;
                    }
                }
            }

            return "Admin";
        }

        #endregion
    }
}
