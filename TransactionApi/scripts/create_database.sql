-- Adjust server/credentials as needed.
CREATE DATABASE Sample_Demo;
GO

USE Sample_Demo;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(200) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Transactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    CardMasked NVARCHAR(100) NOT NULL,
    CardHolder NVARCHAR(200) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Currency NVARCHAR(10) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID('dbo.Payments', 'U') IS NOT NULL
    DROP TABLE dbo.Payments;
GO

CREATE TABLE dbo.Payments
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RequestId UNIQUEIDENTIFIER NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Currency NVARCHAR(10) NOT NULL,
    Reference NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE UNIQUE INDEX UX_Payments_Reference ON dbo.Payments(Reference);
GO

IF OBJECT_ID('dbo.sp_CreatePayment', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CreatePayment;
GO

CREATE PROCEDURE dbo.sp_CreatePayment
    @RequestId UNIQUEIDENTIFIER,
    @Amount DECIMAL(18,2),
    @Currency NVARCHAR(10),
    @Reference NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Payments (RequestId, Amount, Currency, Reference, CreatedAt)
    VALUES (@RequestId, @Amount, @Currency, @Reference, SYSUTCDATETIME());

    SELECT * FROM Payments WHERE Id = SCOPE_IDENTITY();
END;
GO

IF OBJECT_ID('dbo.sp_GetPayments', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetPayments;
GO

CREATE PROCEDURE dbo.sp_GetPayments
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Reference, Amount, Currency, CreatedAt
    FROM Payments
    ORDER BY CreatedAt DESC;
END;
GO

IF OBJECT_ID('dbo.sp_UpdatePayment', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_UpdatePayment;
GO

CREATE PROCEDURE dbo.sp_UpdatePayment
    @Id INT,
    @Amount DECIMAL(18,2),
    @Currency NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Payments
    SET Amount = @Amount,
        Currency = @Currency
    WHERE Id = @Id;

    SELECT * FROM Payments WHERE Id = @Id;
END;
GO

IF OBJECT_ID('dbo.sp_DeletePayment', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_DeletePayment;
GO

CREATE PROCEDURE dbo.sp_DeletePayment
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Payments WHERE Id = @Id;

    SELECT @@ROWCOUNT AS Deleted;
END;
GO