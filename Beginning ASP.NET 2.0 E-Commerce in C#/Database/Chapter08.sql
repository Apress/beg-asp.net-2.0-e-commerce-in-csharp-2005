CREATE PROCEDURE AddDepartment
(@DepartmentName VARCHAR(50),
@DepartmentDescription VARCHAR(1000))
AS
INSERT INTO Department (Name, Description)
VALUES (@DepartmentName, @DepartmentDescription)

GO

CREATE PROCEDURE UpdateDepartment
(@DepartmentID INT,
@DepartmentName VARCHAR(50),
@DepartmentDescription VARCHAR(1000))
AS
UPDATE Department
SET Name = @DepartmentName, Description = @DepartmentDescription
WHERE DepartmentID = @DepartmentID

GO

CREATE PROCEDURE DeleteDepartment
(@DepartmentID INT)
AS
DELETE FROM Department
WHERE DepartmentID = @DepartmentID

GO

CREATE PROCEDURE CreateCategory
(@DepartmentID INT,
@CategoryName VARCHAR(50),
@CategoryDescription VARCHAR(50))
AS
INSERT INTO Category (DepartmentID, Name, Description)
VALUES (@DepartmentID, @CategoryName, @CategoryDescription)

GO

CREATE PROCEDURE UpdateCategory
(@CategoryID INT,
@CategoryName VARCHAR(50),
@CategoryDescription VARCHAR(1000))
AS
UPDATE Category
SET Name = @CategoryName, Description = @CategoryDescription
WHERE CategoryID = @CategoryID

GO

CREATE PROCEDURE DeleteCategory
(@CategoryID INT)
AS
DELETE FROM Category
WHERE CategoryID = @CategoryID

GO

CREATE PROCEDURE GetAllProductsInCategory
(@CategoryID INT)
AS
SELECT Product.ProductID, Name, Description, Price, Image1FileName, 
       Image2FileName, OnDepartmentPromotion, OnCatalogPromotion
FROM Product INNER JOIN ProductCategory
  ON Product.ProductID = ProductCategory.ProductID
WHERE ProductCategory.CategoryID = @CategoryID

GO

CREATE PROCEDURE CreateProduct
(@CategoryID INT,
 @ProductName VARCHAR(50),
 @ProductDescription VARCHAR(1000),
 @ProductPrice MONEY,
 @Image1FileName VARCHAR(50),
 @Image2FileName VARCHAR(50),
 @OnDepartmentPromotion BIT,
 @OnCatalogPromotion BIT)
AS
-- Declare a variable to hold the generated product ID
DECLARE @ProductID INT
-- Create the new product entry
INSERT INTO Product 
    (Name, 
     Description, 
     Price, 
     Image1FileName, 
     Image2FileName,
     OnDepartmentPromotion, 
     OnCatalogPromotion )
VALUES 
    (@ProductName, 
     @ProductDescription, 
     @ProductPrice, 
     @Image1FileName, 
     @Image2FileName,
     @OnDepartmentPromotion, 
     @OnCatalogPromotion)
-- Save the generated product ID to a variable
SELECT @ProductID = @@Identity
-- Associate the product with a category
INSERT INTO ProductCategory (ProductID, CategoryID)
VALUES (@ProductID, @CategoryID)

GO

CREATE PROCEDURE UpdateProduct
(@ProductID INT,
 @ProductName VARCHAR(50),
 @ProductDescription VARCHAR(5000),
 @ProductPrice MONEY,
 @Image1FileName VARCHAR(50),
 @Image2FileName VARCHAR(50),
 @OnDepartmentPromotion BIT,
 @OnCatalogPromotion BIT)
AS
UPDATE Product
SET Name = @ProductName,
    Description = @ProductDescription,
    Price = @ProductPrice,
    Image1FileName = @Image1FileName,
    Image2FileName = @Image2FileName,
    OnDepartmentPromotion = @OnDepartmentPromotion,
    OnCatalogPromotion = @OnCatalogPromotion
WHERE ProductID = @ProductID

GO

CREATE PROCEDURE MoveProductToCategory
(@ProductID INT, @OldCategoryID INT, @NewCategoryID INT)
AS
UPDATE ProductCategory
SET CategoryID = @NewCategoryID
WHERE CategoryID = @OldCategoryID
  AND ProductID = @ProductID

GO

CREATE PROCEDURE AssignProductToCategory
(@ProductID INT, @CategoryID INT)
AS
INSERT INTO ProductCategory (ProductID, CategoryID)
VALUES (@ProductID, @CategoryID)

GO

CREATE PROCEDURE RemoveProductFromCategory
(@ProductID INT, @CategoryID INT)
AS
DELETE FROM ProductCategory
WHERE CategoryID = @CategoryID AND ProductID = @ProductID

GO

CREATE PROCEDURE DeleteProduct
(@ProductID INT)
AS
DELETE FROM ProductCategory WHERE ProductID=@ProductID
DELETE FROM Product where ProductID=@ProductID

GO

CREATE PROCEDURE GetCategoriesWithProduct
(@ProductID INT)
AS
SELECT Category.CategoryID, Name
FROM Category INNER JOIN ProductCategory
ON Category.CategoryID = ProductCategory.CategoryID
WHERE ProductCategory.ProductID = @ProductID

GO

CREATE PROCEDURE GetCategoriesWithoutProduct
(@ProductID INT)
AS
SELECT CategoryID, Name
FROM Category
WHERE CategoryID NOT IN
   (SELECT Category.CategoryID
    FROM Category INNER JOIN ProductCategory
    ON Category.CategoryID = ProductCategory.CategoryID
    WHERE ProductCategory.ProductID = @ProductID)

GO
