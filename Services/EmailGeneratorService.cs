using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace StudentEmailApp.Services
{
    /// <summary>
    /// Service responsible for generating unique student emails with in-memory sequence tracking.
    /// Handles prefix generation, database sequence querying, and duplicate prevention within batches.
    /// </summary>
    public class EmailGeneratorService
    {
        // In-memory tracker holding the current highest sequence number for each 2-letter prefix.
        // Key: prefix (e.g., "am"), Value: current highest sequence (e.g., 1116)
        private readonly Dictionary<string, int> _prefixCounters = new(StringComparer.OrdinalIgnoreCase);

        // Optional email domain (e.g., "@university.edu" or leave empty "" for plain usernames like "am1111")
        public string EmailDomain { get; set; } = "";

        /// <summary>
        /// Clears the in-memory sequence cache if a new batch generation is initiated.
        /// </summary>
        public void ResetCache()
        {
            _prefixCounters.Clear();
        }

        /// <summary>
        /// Generates an email address for a given first name and last name against the specified database table.
        /// Synchronizes with SQL Server on first encounter of each prefix, then auto-increments in memory.
        /// </summary>
        /// <param name="firstName">Student first name (e.g., "Ahmed")</param>
        /// <param name="lastName">Student last name (e.g., "Mohamed")</param>
        /// <param name="targetTable">Target SQL table name ("mails" or "PostGradMails")</param>
        /// <param name="conn">Active SqlConnection</param>
        /// <returns>Generated email string (e.g., "am1116" or "am1116@university.edu")</returns>
        public string GenerateEmail(string firstName, string lastName, string targetTable, SqlConnection conn)
        {
            // 1. Validate inputs and extract 2-letter lowercase prefix (e.g. "Ahmed", "Mohamed" => "am")
            string prefix = ExtractPrefix(firstName, lastName);

            // 2. Check if this prefix has already been loaded into our memory cache during this batch
            if (!_prefixCounters.TryGetValue(prefix, out int currentSequence))
            {
                // First time seeing this prefix in this batch: query the database for the max existing sequence
                int highestInDb = GetHighestSequenceFromDb(targetTable, prefix, conn);

                // If no record exists in DB (highestInDb == 0), the starting sequence base is 1110 
                // so that the first student becomes 1110 + 1 = 1111.
                // If a record exists (e.g. 1115), base is 1115.
                currentSequence = (highestInDb < 1111) ? 1110 : highestInDb;

                _prefixCounters[prefix] = currentSequence;
            }

            // 3. Increment the sequence in memory for this student
            currentSequence++;
            _prefixCounters[prefix] = currentSequence;

            // 4. Format the final email string
            string email = $"{prefix}{currentSequence}{EmailDomain}";
            return email;
        }

        /// <summary>
        /// Extracts the first letter of first name and first letter of last name, converted to lowercase.
        /// Example: "Ahmed", "Mohamed" -> "am"
        /// </summary>
        public static string ExtractPrefix(string firstName, string lastName)
        {
            string cleanFirst = (firstName ?? "").Trim();
            string cleanLast = (lastName ?? "").Trim();

            char firstInitial = 'x';
            char lastInitial = 'x';

            foreach (char c in cleanFirst)
            {
                if (char.IsLetter(c))
                {
                    firstInitial = char.ToLowerInvariant(c);
                    break;
                }
            }

            foreach (char c in cleanLast)
            {
                if (char.IsLetter(c))
                {
                    lastInitial = char.ToLowerInvariant(c);
                    break;
                }
            }

            return $"{firstInitial}{lastInitial}";
        }

        /// <summary>
        /// Queries the target database table to find the highest numeric sequence for a prefix.
        /// Example: If DB contains "am1111", "am1112", "am1115", returns 1115.
        /// If no matching records exist, returns 0.
        /// </summary>
        public static int GetHighestSequenceFromDb(string targetTable, string prefix, SqlConnection conn)
        {
            // Validate table name against SQL injection
            string validatedTable = (targetTable.Equals("PostGradMails", StringComparison.OrdinalIgnoreCase))
                ? "PostGradMails"
                : "mails";

            // Query existing records matching the prefix
            string query = $"SELECT mail FROM dbo.[{validatedTable}] WHERE mail LIKE @prefix + '%'";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add("@prefix", SqlDbType.NVarChar, 50).Value = prefix;

            int maxNumberFound = 0;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string mail = reader["mail"]?.ToString() ?? "";
                
                // Extract only the part after the prefix
                // e.g. from "am1115" or "am1115@domain.edu", strip "am"
                if (mail.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    string remainder = mail.Substring(prefix.Length);

                    // Match leading digits right after the prefix
                    var match = Regex.Match(remainder, @"^(\d+)");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int parsedNum))
                    {
                        if (parsedNum > maxNumberFound)
                        {
                            maxNumberFound = parsedNum;
                        }
                    }
                }
            }

            return maxNumberFound;
        }
    }
}
