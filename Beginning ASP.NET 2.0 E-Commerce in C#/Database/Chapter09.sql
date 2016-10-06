CREATE TABLE ShoppingCart(
	CartID char(36) NOT NULL,
	ProductID INT NOT NULL,
	Quantity INT NOT NULL,
	DateAdded SMALLDATETIME NOT NULL,
 CONSTRAINT PK_ShoppingCart PRIMARY KEY CLUSTERED (CartID ASC, ProductID ASC)
)

GO

ALTER TABLE ShoppingCart WITH CHECK ADD CONSTRAINT FK_ShoppingCart_Product FOREIGN KEY(ProductID)
REFERENCES Product (ProductID)

GO

CREATE PROCEDURE ShoppingCartAddItem
(@CartID char(36),
 @ProductID INT)
AS
IF EXISTS 
        (SELECT CartID 
         FROM ShoppingCart 
         WHERE ProductID = @ProductID AND CartID = @CartID)
    UPDATE ShoppingCart
    SET Quantity = Quantity + 1
    WHERE ProductID = @ProductID AND CartID = @CartID
ELSE
    IF EXISTS (SELECT Name FROM Product WHERE ProductID=@ProductID)
        INSERT INTO ShoppingCart (CartID, ProductID, Quantity, DateAdded)
        VALUES (@CartID, @ProductID, 1, GETDATE())

GO

CREATE PROCEDURE ShoppingCartGetItems
(@CartID char(36))
AS
SELECT Product.ProductID, Product.Name, Product.Price, ShoppingCart.Quantity, 
       Product.Price * ShoppingCart.Quantity AS Subtotal
FROM ShoppingCart INNER JOIN Product
ON ShoppingCart.ProductID = Product.ProductID
WHERE ShoppingCart.CartID = @CartID

GO

CREATE PROCEDURE ShoppingCartGetTotalAmount
(@CartID char(36))
AS
SELECT ISNULL(SUM(Product.Price * ShoppingCart.Quantity), 0)
FROM ShoppingCart INNER JOIN Product
ON ShoppingCart.ProductID = Product.ProductID
WHERE ShoppingCart.CartID = @CartID

GO

CREATE PROCEDURE ShoppingCartRemoveItem
(@CartID char(36),
 @ProductID INT)
AS
DELETE FROM ShoppingCart
WHERE CartID = @CartID and ProductID = @ProductID

GO

CREATE Procedure ShoppingCartUpdateItem
(@CartID char(36),
 @ProductID INT,
 @Quantity INT)
As
IF @Quantity <= 0 
  EXEC ShoppingCartRemoveItem @CartID, @ProductID
ELSE
  UPDATE ShoppingCart
  SET Quantity = @Quantity, DateAdded = GETDATE()
  WHERE ProductID = @ProductID AND CartID = @CartID

GO

ALTER PROCEDURE DeleteProduct
(@ProductID INT)
AS
DELETE FROM ShoppingCart WHERE ProductID=@ProductID
DELETE FROM ProductCategory WHERE ProductID=@ProductID
DELETE FROM Product where ProductID=@ProductID

GO

CREATE PROCEDURE ShoppingCartDeleteOldCarts
(@Days smallINT)
AS
DELETE FROM ShoppingCart
WHERE CartID IN
  (SELECT CartID
   FROM ShoppingCart
   GROUP BY CartID
   HAVING MIN(DATEDIFF(dd,DateAdded,GETDATE())) >= @Days)

GO

CREATE PROCEDURE ShoppingCartCountOldCarts
(@Days smallINT)
AS
SELECT COUNT(CartID) 
FROM ShoppingCart
WHERE CartID IN
  (SELECT CartID
   FROM ShoppingCart
   GROUP BY CartID
   HAVING MIN(DATEDIFF(dd,DateAdded,GETDATE())) >= @Days)

GO
