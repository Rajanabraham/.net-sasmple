CREATE PROCEDURE dbo.sp_GetMonthlyRevenue
    @Year  INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StartDate DATE = DATEFROMPARTS(@Year, @Month, 1);
    DECLARE @EndDate   DATE = DATEADD(MONTH, 1, @StartDate);
    DECLARE @TotalRevenue DECIMAL(18,2) = 0;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @TotalRevenue = SUM(Amount)
        FROM dbo.Orders WITH (NOLOCK)
        WHERE OrderDate >= @StartDate AND OrderDate < @EndDate;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (N'Error in sp_GetMonthlyRevenue: %s', 16, 1, @ErrorMessage);
        RETURN;
    END CATCH;

    SELECT @TotalRevenue AS MonthlyRevenue;
END;
GO