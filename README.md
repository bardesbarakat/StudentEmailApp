Student Email Management System
A modern, robust C# Windows Forms desktop application designed for universities to automate the generation and management of student email accounts. The system intelligently parses Excel files, generates unique email addresses based on student initials and database sequence tracking, and segregates data between Undergraduate and Postgraduate students.

✨ Features
Modern User Interface: Built with a sleek, contemporary UI (using MaterialSkin/Guna UI) featuring flat controls, customized data grids, and responsive design.

Secure Authentication: Dedicated login portal with credential validation to restrict unauthorized access.

Comprehensive Audit Logging: Tracks all user activities within the system, including login times, file uploads, and database modifications, ensuring complete accountability.

Intelligent Email Generation:

Extracts the first letter of the first and last names (e.g., Ahmed Mohamed -> am).

Automatically queries the database for the highest existing sequence for those initials.

Assigns the next available number (starting at 1111).

Handles in-memory auto-incrementing for batch processing to prevent duplicates before saving.

Excel Integration: Seamlessly upload and preview student records from Excel files using checkbox-enabled DataGridViews for selective processing.

Dual-Target System: Dynamically switches database operations between Undergraduate (mails table) and Postgraduate (PostGradMails table) cohorts.

🛠 Technologies Used
Language: C#

Framework: .NET (Windows Forms)

Database: Microsoft SQL Server

Data Access: ADO.NET (SqlConnection, SqlCommand)

Libraries: ExcelDataReader / EPPlus (for Excel parsing), UI Packages (MaterialSkin.2 / Guna.UI2)

📋 Prerequisites
Before running the application, ensure you have the following installed:

Visual Studio 2022 (or later) with the ".NET desktop development" workload.

Microsoft SQL Server (LocalDB, Express, or Enterprise).
