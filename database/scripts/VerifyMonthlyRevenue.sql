-- Clean up any previous test data
DELETE FROM dbo.Orders;
GO

-- Insert sample orders spanning multiple months
INSERT INTO dbo.Orders (CustomerId, OrderDate, Amount) VALUES
    (1, '2024-01-05', 150.00),
    (2, '2024-01-15', 250.00),
    (3, '2024-02-10', 300.00),
    (4, '2024-02-20', 450.00),
    (5, '2024-03-01', 500.00);
GO

-- Verify revenue for January 2024
DECLARE @Year INT = 2024, @Month INT = 1;
EXEC dbo.sp_GetMonthlyRevenue @Year = @Year, @Month = @Month;
GO

-- Verify revenue for February 2024
SET @Month = 2;
EXEC dbo.sp_GetMonthlyRevenue @Year = @Year, @Month = @Month;
GO

-- Verify revenue for a month with no orders (April 2024)
SET @Month = 4;
EXEC dbo.sp_GetMonthlyRevenue @Year = @Year, @Month = @Month;
GO