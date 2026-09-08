CREATE TABLE dbo.Orders (
    OrderId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    CONSTRAINT CK_Orders_Amount_NonNegative CHECK (Amount >= 0)
);
GO

-- Non‑clustered index to speed up queries filtering by OrderDate (and optionally CustomerId)
CREATE NONCLUSTERED INDEX IX_Orders_OrderDate_CustomerId
    ON dbo.Orders (OrderDate ASC, CustomerId ASC);
GO