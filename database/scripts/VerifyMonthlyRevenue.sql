-- Clean up previous test data if any
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL
    DELETE FROM dbo.Orders;
GO

-- Insert sample orders spanning multiple months
INSERT INTO dbo.Orders (CustomerId, OrderDate, Amount) VALUES
    (1, '2024-01-05', 150.00),
    (2, '2024-01-15', 200.00),
    (3, '2024-02-10', 350.00),
    (1, '2024-01-25', 100.00),
    (2, '2024-02-20', 400.00);
GO

-- Execute the procedure for January 2024 (expected revenue = 150 + 200 + 100 = 450)
DECLARE @Year INT = 2024, @Month INT = 1;
EXEC dbo.sp_GetMonthlyRevenue @Year = @Year, @Month = @Month;
GO

-- Execute the procedure for February 2024 (expected revenue = 350 + 400 = 750)
SET @Month = 2;
EXEC dbo.sp_GetMonthlyRevenue @Year = @Year, @Month = @Month;
GO