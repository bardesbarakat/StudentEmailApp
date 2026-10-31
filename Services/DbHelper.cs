using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace StudentEmailApp.Services
{
    /// <summary>
    /// Centralized ADO.NET helper for SQL Server connectivity and SystemLogs tracking.
    /// </summary>
    public static class DbHelper
    {
        private static string _connectionString;

        static DbHelper()
        {
            try
            {
                var configConn = System.Configuration.ConfigurationManager.ConnectionStrings["StudentMailsDb"]?.ConnectionString;
                _connectionString = !string.IsNullOrWhiteSpace(configConn) 
                    ? configConn 
                    : "Server=.\\SQLEXPRESS;Database=StudentMails;Trusted_Connection=True;TrustServerCertificate=True;";
            }
            catch
            {
                _connectionString = "Server=.\\SQLEXPRESS;Database=StudentMails;Trusted_Connection=True;TrustServerCertificate=True;";
            }
        }

        // Allows runtime inspection or override
        public static string ConnectionString 
        { 
            get => _connectionString; 
            set => _connectionString = value; 
        }

        /// <summary>
        /// Creates and returns a new SqlConnection object.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Inserts an audit trail entry into the "SystemLogs" table.
        /// Requirements: tracks logins, file uploads, generated emails, and saved records.
        /// </summary>
        public static bool LogAction(string username, string actionDescription)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();

                const string query = @"
                    INSERT INTO dbo.SystemLogs (Username, ActionTime, ActionDescription) 
                    VALUES (@Username, GETDATE(), @ActionDescription)";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = 
                    string.IsNullOrWhiteSpace(username) ? "System" : username;
                cmd.Parameters.Add("@ActionDescription", SqlDbType.NVarChar, 1000).Value = 
                    actionDescription;

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                // In production, write to event log / debug console so UI is not disrupted
                System.Diagnostics.Debug.WriteLine($"[DbHelper.LogAction Error]: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Authenticates the user against the Users table.
        /// </summary>
        public static bool ValidateUser(string username, string password, out int userId)
        {
            userId = 0;
            try
            {
                using var conn = GetConnection();
                conn.Open();

                const string query = @"
                    SELECT UserID, Password 
                    FROM dbo.Users 
                    WHERE Username = @Username";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username.Trim();

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string storedPassword = reader["Password"]?.ToString() ?? "";
                    if (storedPassword == password) // In production, use BCrypt or Argon2 hashing
                    {
                        userId = Convert.ToInt32(reader["UserID"]);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DbHelper.ValidateUser Error]: {ex.Message}");
                throw; // Rethrow to let the login form display friendly SQL error if connection failed
            }
        }
    }
}
