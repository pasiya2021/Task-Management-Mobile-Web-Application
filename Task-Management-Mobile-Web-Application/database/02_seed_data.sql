-- Insert sample users and tasks for testing
USE TaskManagerDB;
GO

INSERT INTO Users (Name, Email, CreatedDate)
VALUES ('Alice Johnson', 'alice.johnson@example.com', GETDATE()),
       ('Bob Smith', 'bob.smith@example.com', GETDATE());

INSERT INTO Tasks (Title, Description, Status, UserId, CreatedDate, DueDate)
VALUES ('Design UI Mockups', 'Create mockups for the new dashboard', 'Pending', 1, GETDATE(), DATEADD(day, 7, GETDATE())),
       ('API Integration', 'Integrate backend APIs with frontend', 'In Progress', 2, GETDATE(), DATEADD(day, 3, GETDATE()));
