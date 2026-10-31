-- ==========================================================
-- Database: StudentMails
-- Complete Database Setup Script
-- Includes: Users, SystemLogs, mails, and PostGradMails
-- ==========================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'StudentMails')
BEGIN
    CREATE DATABASE [StudentMails];
END
GO

USE [StudentMails];
GO

-- ==========================================================
-- 1. Users Table (Authentication)
-- ==========================================================
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
    DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL
);
GO

-- Seed test users
INSERT INTO dbo.Users (Username, Password)
VALUES 
    (N'admin', N'123'),
    (N'staff', N'staff123'),
    (N'registrar', N'pass123');
GO

-- ==========================================================
-- 2. SystemLogs Table
-- Tracks logins, file uploads, generated emails, and saved records
-- ==========================================================
IF OBJECT_ID(N'dbo.SystemLogs', N'U') IS NOT NULL
    DROP TABLE dbo.SystemLogs;
GO

CREATE TABLE dbo.SystemLogs (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    ActionTime DATETIME NOT NULL DEFAULT (GETDATE()),
    ActionDescription NVARCHAR(1000) NOT NULL
);
GO

CREATE NONCLUSTERED INDEX IX_SystemLogs_Username_Time 
ON dbo.SystemLogs (Username, ActionTime DESC);
GO

-- ==========================================================
-- 3. mails Table (Undergraduates)
-- Contains all columns as specified:
-- pk, first_name, last_name, mail, create_date, user_name,
-- id, college, dept, code, type, stat, ar_name, stage, col1, col2, col3
-- ==========================================================
IF OBJECT_ID(N'dbo.mails', N'U') IS NOT NULL
    DROP TABLE dbo.mails;
GO

CREATE TABLE dbo.mails (
    pk INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(100) NULL,
    last_name NVARCHAR(100) NULL,
    mail NVARCHAR(150) NOT NULL,
    create_date NVARCHAR(50) NULL,
    user_name NVARCHAR(100) NULL,
    id NVARCHAR(50) NULL,
    college NVARCHAR(100) NULL,
    dept NVARCHAR(100) NULL,
    code NVARCHAR(50) NULL,
    type NVARCHAR(50) NULL,
    stat NVARCHAR(50) NULL,
    ar_name NVARCHAR(150) NULL,
    stage NVARCHAR(50) NULL,
    col1 NVARCHAR(150) NULL,
    col2 NVARCHAR(150) NULL,
    col3 NVARCHAR(150) NULL
);
GO

CREATE NONCLUSTERED INDEX IX_mails_mail 
ON dbo.mails (mail);
GO

-- Sample existing records to test auto-incrementing logic (am1111 - am1115)
INSERT INTO dbo.mails (first_name, last_name, mail, create_date, user_name)
VALUES
    (N'Ahmed', N'Mohamed', N'am1111', CONVERT(NVARCHAR(50), GETDATE(), 120), N'admin'),
    (N'Ali', N'Mahmoud', N'am1112', CONVERT(NVARCHAR(50), GETDATE(), 120), N'admin'),
    (N'Amr', N'Mostafa', N'am1115', CONVERT(NVARCHAR(50), GETDATE(), 120), N'admin');
GO

-- ==========================================================
-- 4. PostGradMails Table (Postgraduates)
-- Exactly the same schema as "mails"
-- ==========================================================
IF OBJECT_ID(N'dbo.PostGradMails', N'U') IS NOT NULL
    DROP TABLE dbo.PostGradMails;
GO

CREATE TABLE dbo.PostGradMails (
    pk INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(100) NULL,
    last_name NVARCHAR(100) NULL,
    mail NVARCHAR(150) NOT NULL,
    create_date NVARCHAR(50) NULL,
    user_name NVARCHAR(100) NULL,
    id NVARCHAR(50) NULL,
    college NVARCHAR(100) NULL,
    dept NVARCHAR(100) NULL,
    code NVARCHAR(50) NULL,
    type NVARCHAR(50) NULL,
    stat NVARCHAR(50) NULL,
    ar_name NVARCHAR(150) NULL,
    stage NVARCHAR(50) NULL,
    col1 NVARCHAR(150) NULL,
    col2 NVARCHAR(150) NULL,
    col3 NVARCHAR(150) NULL
);
GO

CREATE NONCLUSTERED INDEX IX_PostGradMails_mail 
ON dbo.PostGradMails (mail);
GO

-- Sample existing record for Postgrads
INSERT INTO dbo.PostGradMails (first_name, last_name, mail, create_date, user_name)
VALUES
    (N'Sarah', N'Khaled', N'sk1111', CONVERT(NVARCHAR(50), GETDATE(), 120), N'admin');
GO

PRINT 'Database StudentMails and all tables created successfully!';
