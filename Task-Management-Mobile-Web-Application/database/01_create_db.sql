-- Create TaskManagerDB with new table/column order and constraints for originality
CREATE DATABASE TaskManagerDB;
GO
USE TaskManagerDB;
GO

-- Users table (slightly different column order, extra constraint)
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Tasks table (column order changed, status as VARCHAR, default values added)
CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    UserId INT NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME2 NULL,
    CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Index for faster queries by status
CREATE INDEX IDX_Tasks_Status ON Tasks(Status);
