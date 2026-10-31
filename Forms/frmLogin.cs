using System;
using System.Drawing;
using System.Windows.Forms;
using StudentEmailApp.Models;
using StudentEmailApp.Services;

namespace StudentEmailApp.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Clear previous error
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblError.Text = "Please enter both username and password.";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Verifying...";

            try
            {
                // Validate credentials against SQL Server "Users" table
                bool isValid = DbHelper.ValidateUser(username, password, out int userId);

                if (isValid)
                {
                    // 1. Save username in static session variable
                    UserSession.CurrentUsername = username;
                    UserSession.CurrentUserId = userId;

                    // 2. Insert record into SystemLogs table
                    DbHelper.LogAction(username, $"{username} logged in");

                    // 3. Open Form 2 (frmSelectTarget)
                    this.Hide();
                    using var selectForm = new frmSelectTarget();
                    selectForm.ShowDialog();

                    // If user returns/logs out, clear fields and show login again
                    txtPassword.Clear();
                    lblError.Text = "";
                    this.Show();
                    txtUsername.Focus();
                }
                else
                {
                    lblError.Text = "Invalid username or password. Please try again.";
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Database Connection Error:\n{ex.Message}";
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Sign In";
            }
        }
    }
}
