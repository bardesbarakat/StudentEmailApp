using System;
using System.Data;
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

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Clear any previous error message
            lblError.Text = "";

            // 1. Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblError.Text = "Please enter both username and password.";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Verifying...";

            try
            {
                // 2. Connect to SQL Server and validate credentials against "Users" table
                bool isAuthenticated = false;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Parameterized SQL query to prevent SQL Injection
                    const string query = "SELECT UserID, Username, Password FROM dbo.Users WHERE Username = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbPassword = reader["Password"]?.ToString() ?? "";

                                // Compare entered password with database password
                                if (dbPassword == password)
                                {
                                    isAuthenticated = true;
                                    UserSession.CurrentUserId = Convert.ToInt32(reader["UserID"]);
                                }
                            }
                        }
                    }

                    // 3. If authenticated: save session, log to SystemLogs, and open Form 2
                    if (isAuthenticated)
                    {
                        // Save username globally in static variable
                        UserSession.CurrentUsername = username;

                        // Insert audit trail into "SystemLogs"
                        const string logQuery = @"
                            INSERT INTO dbo.SystemLogs (Username, ActionTime, ActionDescription) 
                            VALUES (@Username, GETDATE(), @ActionDescription)";

                        using (SqlCommand logCmd = new SqlCommand(logQuery, conn))
                        {
                            logCmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;
                            logCmd.Parameters.Add("@ActionDescription", SqlDbType.NVarChar, 1000).Value = $"{username} logged in";
                            logCmd.ExecuteNonQuery();
                        }

                        // Hide Form 1 and display Form 2 (Target Selection)
                        this.Hide();
                        using (frmSelectTarget selectForm = new frmSelectTarget())
                        {
                            selectForm.ShowDialog();
                        }

                        // Reset password and restore login form when user logs out
                        txtPassword.Clear();
                        lblError.Text = "";
                        this.Show();
                        txtUsername.Focus();
                    }
                    else
                    {
                        // Invalid credentials: display error message
                        lblError.Text = "Invalid username or password. Please try again.";
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                lblError.Text = $"Database Connection Error:\n{sqlEx.Message}";
            }
            catch (Exception ex)
            {
                lblError.Text = $"An error occurred:\n{ex.Message}";
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Sign In";
            }
        }
    }
}
