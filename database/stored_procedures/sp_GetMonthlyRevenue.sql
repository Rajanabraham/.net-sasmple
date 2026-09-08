SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetMonthlyRevenue
    @Year  INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Revenue DECIMAL(18,2);
        
        SELECT @Revenue = ISNULL(SUM(Amount), 0)
        FROM dbo.Orders
        WHERE YEAR(OrderDate) = @Year
          AND MONTH(OrderDate) = @Month;

        SELECT @Revenue AS MonthlyRevenue;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR ('sp_GetMonthlyRevenue failed: %s', 16, 1, @ErrorMessage);
    END CATCH
END
GO