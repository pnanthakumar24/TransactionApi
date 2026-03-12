-- Stored procedures for TransactionApi (database-first/SP approach)

USE Sample_Demo;
GO

IF OBJECT_ID('dbo.sp_CreateTransaction', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CreateTransaction;
GO

CREATE PROCEDURE dbo.sp_CreateTransaction
    @UserId INT,
    @CardMasked NVARCHAR(100),
    @CardHolder NVARCHAR(200),
    @Amount DECIMAL(18,2),
    @Currency NVARCHAR(10),
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Transactions (UserId, CardMasked, CardHolder, Amount, Currency, Status, CreatedAt)
    VALUES (@UserId, @CardMasked, @CardHolder, @Amount, @Currency, @Status, SYSUTCDATETIME());

    SELECT * FROM Transactions WHERE Id = SCOPE_IDENTITY();
END;
GO

IF OBJECT_ID('dbo.sp_GetTransactionsByUser', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetTransactionsByUser;
GO

CREATE PROCEDURE dbo.sp_GetTransactionsByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Transactions WHERE UserId = @UserId ORDER BY CreatedAt DESC;
END;
GO
