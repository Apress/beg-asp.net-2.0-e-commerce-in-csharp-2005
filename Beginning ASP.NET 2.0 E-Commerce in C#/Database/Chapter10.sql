CREATE TABLE Orders(
	OrderID INT IDENTITY(1,1) NOT NULL,
	DateCreated SMALLDATETIME NOT NULL CONSTRAINT DF_Orders_DateCreated  DEFAULT (getdate()),
	DateShipped SMALLDATETIME NULL,
	Verified BIT NOT NULL CONSTRAINT DF_Orders_Verified DEFAULT ((0)),
	Completed BIT NOT NULL CONSTRAINT DF_Orders_Completed DEFAULT ((0)),
	Canceled BIT NOT NULL CONSTRAINT DF_Orders_Canceled DEFAULT ((0)),
	Comments VARCHAR(1000) NULL,
	CustomerName VARCHAR(50) NULL,
	CustomerEmail VARCHAR(50) NULL,
	ShippingAddress VARCHAR(500) NULL,
 CONSTRAINT PK_Orders PRIMARY KEY CLUSTERED(OrderID ASC)
)

GO

CREATE TABLE OrderDetail(
	OrderID INT NOT NULL,
	ProductID INT NOT NULL,
	ProductName VARCHAR(50) NOT NULL,
	Quantity INT NOT NULL,
	UnitCost MONEY NOT NULL,
	Subtotal  AS (Quantity*UnitCost),
 CONSTRAINT PK_OrderDetail PRIMARY KEY CLUSTERED(OrderID ASC, ProductID ASC)
)
  
GO

ALTER TABLE OrderDetail WITH CHECK ADD CONSTRAINT FK_OrderDetail_Orders FOREIGN KEY(OrderID)
REFERENCES Orders (OrderID)

GO

CREATE PROCEDURE CreateOrder 
(@CartID char(36))
AS
/* Insert a new record INTo Orders */
DECLARE @OrderID INT
INSERT INTO Orders DEFAULT VALUES
/* Save the new Order ID */
SET @OrderID = @@IDENTITY
/* Add the order details to OrderDetail */
INSERT INTO OrderDetail 
     (OrderID, ProductID, ProductName, Quantity, UnitCost)
SELECT 
     @OrderID, Product.ProductID, Product.Name, 
     ShoppingCart.Quantity, Product.Price
FROM Product JOIN ShoppingCart
ON Product.ProductID = ShoppingCart.ProductID
WHERE ShoppingCart.CartID = @CartID
/* Clear the shopping cart */
DELETE FROM ShoppingCart
WHERE CartID = @CartID
/* Return the Order ID */
SELECT @OrderID

GO

CREATE PROCEDURE OrderGetDetails
(@OrderID INT)
AS
SELECT Orders.OrderID, 
       ProductID, 
       ProductName, 
       Quantity, 
       UnitCost, 
       Subtotal
FROM OrderDetail JOIN Orders
ON Orders.OrderID = OrderDetail.OrderID
WHERE Orders.OrderID = @OrderID

GO

CREATE PROCEDURE OrdersGetByRecent 
(@Count smallINT)
AS
-- Set the number of rows to be returned
SET ROWCOUNT @Count
-- Get list of orders
SELECT OrderID, DateCreated, DateShipped, 
       Verified, Completed, Canceled, CustomerName
FROM Orders
ORDER BY DateCreated DESC
-- Reset rowcount value
SET ROWCOUNT 0

GO

CREATE PROCEDURE OrdersGetByDate 
(@StartDate SMALLDATETIME,
 @EndDate SMALLDATETIME)
AS
SELECT OrderID, DateCreated, DateShipped, 
       Verified, Completed, Canceled, CustomerName
FROM Orders
WHERE DateCreated BETWEEN @StartDate AND @EndDate
ORDER BY DateCreated DESC

GO

CREATE PROCEDURE OrdersGetUnverifiedUncanceled
AS
SELECT OrderID, DateCreated, DateShipped, 
       Verified, Completed, Canceled, CustomerName
FROM Orders
WHERE Verified=0 AND Canceled=0
ORDER BY DateCreated DESC

GO

CREATE PROCEDURE OrdersGetVerifiedUncompleted
AS
SELECT OrderID, DateCreated, DateShipped, 
       Verified, Completed, Canceled, CustomerName
FROM Orders
WHERE Verified=1 AND Completed=0
ORDER BY DateCreated DESC

GO

CREATE PROCEDURE OrderGetInfo
(@OrderID INT)
AS
SELECT OrderID, 
      (SELECT ISNULL(SUM(Subtotal), 0) FROM OrderDetail WHERE OrderID = @OrderID)        
       AS TotalAmount, 
       DateCreated, 
       DateShipped, 
       Verified, 
       Completed, 
       Canceled, 
       Comments, 
       CustomerName, 
       ShippingAddress, 
       CustomerEmail
FROM Orders
WHERE OrderID = @OrderID

GO

CREATE PROCEDURE OrderUpdate
(@OrderID INT,
 @DateCreated SMALLDATETIME,
 @DateShipped SMALLDATETIME = NULL,
 @Verified BIT,
 @Completed BIT,
 @Canceled BIT,
 @Comments VARCHAR(200),
 @CustomerName VARCHAR(50),
 @ShippingAddress VARCHAR(200),
 @CustomerEmail VARCHAR(50))
AS
UPDATE Orders
SET DateCreated=@DateCreated,
    DateShipped=@DateShipped,
    Verified=@Verified,
    Completed=@Completed,
    Canceled=@Canceled,
    Comments=@Comments,
    CustomerName=@CustomerName,
    ShippingAddress=@ShippingAddress,
    CustomerEmail=@CustomerEmail
WHERE OrderID = @OrderID

GO

CREATE PROCEDURE OrderMarkVerified
(@OrderID INT)
AS
UPDATE Orders
SET Verified = 1
WHERE OrderID = @OrderID

GO

CREATE PROCEDURE OrderMarkCompleted
(@OrderID INT)
AS
UPDATE Orders
SET Completed = 1,
    DateShipped = GETDATE()
WHERE OrderID = @OrderID

GO

CREATE PROCEDURE OrderMarkCanceled
(@OrderID INT)
AS
UPDATE Orders
SET Canceled = 1
WHERE OrderID = @OrderID

GO
