using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace StudentEmailApp.Services
{
    /// <summary>
    /// Service responsible for generating unique student emails with in-memory sequence tracking.
    /// Handles prefix generation from single full-name column, DB sequence querying, and duplicate prevention.
    /// </summary>
    public class EmailGeneratorService
    {
        // In-memory tracker holding current highest sequence number for each 2-letter prefix.
        // Key: prefix (e.g., "gr"), Value: current highest sequence (e.g., 1116)
        private readonly Dictionary<string, int> _prefixCounters = new(StringComparer.OrdinalIgnoreCase);

        // Optional email domain (e.g., "@university.edu" or leave empty "" for plain usernames like "gr1111")
        public string EmailDomain { get; set; } = "";

        /// <summary>
        /// Clears the in-memory sequence cache before starting a new batch.
        /// </summary>
        public void ResetCache()
        {
            _prefixCounters.Clear();
        }

        /// <summary>
        /// Parses a multi-word English full name (e.g. "Ganna Reda Ibrahim Atiya"), extracts the 1st letter
        /// of the 1st name and 1st letter of the 2nd name in lowercase ("gr"), and auto-increments sequentially.
        /// </summary>
        public string GenerateEmailFromFullName(string fullName, string targetTable, SqlConnection conn, out string firstName, out string lastName)
        {
            var parsed = ParseFullName(fullName);
            firstName = parsed.firstName;
            lastName = parsed.lastName;
            string prefix = parsed.prefix;

            // First time seeing this prefix in this batch: query target database table
            if (!_prefixCounters.TryGetValue(prefix, out int currentSequence))
            {
                int highestInDb = GetHighestSequenceFromDb(targetTable, prefix, conn);

                // If no record exists, base is 1110 (so first student becomes 1110 + 1 = 1111)
                // If records exist (e.g. 1115), base is 1115 (so first student becomes 1116)
                currentSequence = (highestInDb < 1111) ? 1110 : highestInDb;
                _prefixCounters[prefix] = currentSequence;
            }

            // Auto-increment in memory for this student
            currentSequence++;
            _prefixCounters[prefix] = currentSequence;

            return $"{prefix}{currentSequence}{EmailDomain}";
        }

        /// <summary>
        /// Splits a full English name by spaces and extracts initials for the 1st and 2nd words.
        /// Example: "Ganna Reda Ibrahim Atiya" -> prefix: "gr", firstName: "Ganna", lastName: "Reda"
        /// </summary>
        public static (string prefix, string firstName, string lastName) ParseFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return ("xx", "", "");

            string[] parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return ("xx", "", "");

            string firstName = parts[0];
            string lastName = parts.Length > 1 ? parts[1] : "";

            char firstInitial = 'x';
            char secondInitial = 'x';

            foreach (char c in firstName)
            {
                if (char.IsLetter(c))
                {
                    firstInitial = char.ToLowerInvariant(c);
                    break;
                }
            }

            if (parts.Length > 1)
            {
                foreach (char c in parts[1])
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
        /// Queries the target database table to find the highest numeric sequence for a prefix.
        /// Example: If DB contains "gr1111", "gr1112", "gr1115", returns 1115.
        /// If no matching records exist, returns 0.
        /// </summary>
        public static int GetHighestSequenceFromDb(string targetTable, string prefix, SqlConnection conn)
        {
            string validatedTable = (targetTable.Equals("PostGradMails", StringComparison.OrdinalIgnoreCase))
                ? "PostGradMails"
                : "mails";

            string query = $"SELECT mail FROM dbo.[{validatedTable}] WHERE mail LIKE @prefix + '%'";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add("@prefix", SqlDbType.NVarChar, 50).Value = prefix;

            int maxNumberFound = 0;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string mail = reader["mail"]?.ToString() ?? "";
                if (mail.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    string remainder = mail.Substring(prefix.Length);
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
