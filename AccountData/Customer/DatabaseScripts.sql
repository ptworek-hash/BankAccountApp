-- Customer Management System - Database Schema
-- This script can be used to create the database schema in SQL Server
-- For SQLite, the schema is auto-created by CustomerDataAccessDapper.InitializeDatabase()

-- =================================================================
-- SQL SERVER VERSION (if you want to use SQL Server instead)
-- =================================================================

-- Create Customer table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customer')
BEGIN
    CREATE TABLE Customer (
        Id BIGINT PRIMARY KEY IDENTITY(1,1),
        FirstName NVARCHAR(100),
        LastName NVARCHAR(100) NOT NULL,
        CompanyName NVARCHAR(200) NOT NULL,
        CONSTRAINT CHK_Customer_RequiredFields CHECK (
            LEN(LTRIM(RTRIM(LastName))) > 0 AND 
            LEN(LTRIM(RTRIM(CompanyName))) > 0
        )
    );
END
GO

-- Create Address table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Address')
BEGIN
    CREATE TABLE Address (
        Id BIGINT PRIMARY KEY IDENTITY(1,1),
        CustomerId BIGINT NOT NULL,
        Street NVARCHAR(200),
        City NVARCHAR(100),
        State NVARCHAR(50),
        Zip NVARCHAR(20),
        CONSTRAINT FK_Address_Customer FOREIGN KEY (CustomerId) 
            REFERENCES Customer(Id) ON DELETE CASCADE
    );

    -- Create index for performance
    CREATE INDEX IX_Address_CustomerId ON Address(CustomerId);
END
GO

-- =================================================================
-- SQLite VERSION (auto-created by code, shown here for reference)
-- =================================================================

/*
CREATE TABLE IF NOT EXISTS Customer (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT,
    LastName TEXT NOT NULL,
    CompanyName TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Address (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerId INTEGER NOT NULL,
    Street TEXT,
    City TEXT,
    State TEXT,
    Zip TEXT,
    FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
);

CREATE INDEX IF NOT EXISTS IX_Address_CustomerId 
ON Address(CustomerId);
*/

-- =================================================================
-- SAMPLE DATA (optional - for testing)
-- =================================================================

-- Insert sample customers
INSERT INTO Customer (FirstName, LastName, CompanyName) 
VALUES 
    ('John', 'Doe', 'Acme Corporation'),
    ('Jane', 'Smith', 'TechStart Inc'),
    ('Bob', 'Johnson', 'Global Solutions LLC');

-- Get the IDs (in SQL Server, you'd use SCOPE_IDENTITY() in the same batch)
-- For this sample, assuming IDs 1, 2, 3

-- Insert sample addresses
INSERT INTO Address (CustomerId, Street, City, State, Zip)
VALUES 
    (1, '123 Main Street', 'New York', 'NY', '10001'),
    (2, '456 Tech Boulevard', 'San Francisco', 'CA', '94102'),
    (3, '789 Business Park Drive', 'Austin', 'TX', '78701');
