/*================================================================
  Verification Script: VerifyMonthlyRevenue
  Description: Inserts test data, runs sp_GetMonthlyRevenue,
               and displays the result.
================================================================*/

-- Clean up previous test data
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL
BEGIN
    DELETE FROM dbo.Orders;
END

-- Insert sample orders
INSERT INTO dbo.Orders (OrderDate, CustomerId, Amount)
VALUES
    ('2023-01-15', 1, 150.00),
    ('2023-01-20', 2, 250.00),
    ('2023-02-05', 1, 300.00),
    ('2023-01-31', 3, 100.00);

-- Execute stored procedure for January 2023
EXEC dbo.sp_GetMonthlyRevenue @Year = 2023, @Month = 1;
GO