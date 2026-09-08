/*================================================================
  Stored Procedure: dbo.sp_GetMonthlyRevenue
  Description: Returns total revenue for a given year and month.
================================================================*/
CREATE PROCEDURE dbo.sp_GetMonthlyRevenue
    @Year  INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @TotalRevenue DECIMAL(18,2);

        SELECT @TotalRevenue = ISNULL(SUM(Amount), 0)
        FROM dbo.Orders
        WHERE YEAR(OrderDate) = @Year
          AND MONTH(OrderDate) = @Month;

        SELECT @TotalRevenue AS TotalRevenue;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END
GO