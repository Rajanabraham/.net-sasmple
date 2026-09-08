/*================================================================
  Table: dbo.Orders
  Description: Stores order information for revenue calculations.
================================================================*/
CREATE TABLE dbo.Orders
(
    OrderId     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderDate   DATETIME NOT NULL,
    CustomerId  INT NOT NULL,
    Amount      DECIMAL(18,2) NOT NULL,
    -- Additional columns can be added as needed
);
GO

/* Non‑clustered index to support queries filtering by OrderDate */
CREATE NONCLUSTERED INDEX IX_Orders_OrderDate
    ON dbo.Orders (OrderDate);
GO