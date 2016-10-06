CREATE FUNCTION dbo.WordCount
(@Word VARCHAR(15), 
@Phrase VARCHAR(1000))
RETURNS SMALLINT
AS
BEGIN

/* If @Word or @Phrase is NULL the function returns 0 */
IF @Word IS NULL OR @Phrase IS NULL RETURN 0

/* @BiggerWord is a string one character longer than @Word */
DECLARE @BiggerWord VARCHAR(21)
SELECT @BiggerWord = @Word + 'x'

/* Replace @Word with @BiggerWord in @Phrase */
DECLARE @BiggerPhrase VARCHAR(2000)
SELECT @BiggerPhrase = REPLACE (@Phrase, @Word, @BiggerWord)

/* The length difference between @BiggerPhrase and @phrase
   is the number we''re looking for */
RETURN LEN(@BiggerPhrase) - LEN(@Phrase)
END

GO

CREATE PROCEDURE SearchCatalog 
(@DescriptionLength INT,
 @PageNumber TINYINT,
 @ProductsPerPage TINYINT,
 @HowManyResults SMALLINT OUTPUT,
 @AllWords BIT,
 @Word1 VARCHAR(15) = NULL,
 @Word2 VARCHAR(15) = NULL,
 @Word3 VARCHAR(15) = NULL,
 @Word4 VARCHAR(15) = NULL,
 @Word5 VARCHAR(15) = NULL)
AS

/* Create the table variable that will contain the search results */
DECLARE @Products TABLE
(RowNumber SMALLINT IDENTITY (1,1) NOT NULL,
 ProductID INT,
 Name VARCHAR(50),
 Description VARCHAR(1000),
 Price MONEY,
 Image1FileName VARCHAR(50),
 Image2FileName VARCHAR(50),
 Rank INT)

/* Populate @Products for an any-words search */
IF @AllWords = 0 
   INSERT INTO @Products           
   SELECT ProductID, Name, 
          SUBSTRING(Description, 1, @DescriptionLength) + '...' AS Description, 
          Price, Image1FileName, Image2FileName,
          3 * dbo.WordCount(@Word1, Name) + dbo.WordCount(@Word1, Description) +
          3 * dbo.WordCount(@Word2, Name) + dbo.WordCount(@Word2, Description) +
          3 * dbo.WordCount(@Word3, Name) + dbo.WordCount(@Word3, Description) +
          3 * dbo.WordCount(@Word4, Name) + dbo.WordCount(@Word4, Description) +
          3 * dbo.WordCount(@Word5, Name) + dbo.WordCount(@Word5, Description) 
          AS Rank
   FROM Product
   ORDER BY Rank DESC
   
/* Populate @Products for an all-words search */
IF @AllWords = 1 
   INSERT INTO @Products           
   SELECT ProductID, Name, 
          SUBSTRING(Description, 1, @DescriptionLength) + '...' AS Description, 
          Price, Image1FileName, Image2FileName,
          (3 * dbo.WordCount(@Word1, Name) + dbo.WordCount(@Word1, Description)) *
          CASE 
            WHEN @Word2 IS NULL THEN 1 
            ELSE 3 * dbo.WordCount(@Word2, Name) + dbo.WordCount(@Word2, Description)
          END *
          CASE 
            WHEN @Word3 IS NULL THEN 1 
            ELSE 3 * dbo.WordCount(@Word3, Name) + dbo.WordCount(@Word3, Description)
          END *
          CASE 
            WHEN @Word4 IS NULL THEN 1 
            ELSE 3 * dbo.WordCount(@Word4, Name) + dbo.WordCount(@Word4, Description)
          END *
          CASE 
            WHEN @Word5 IS NULL THEN 1 
            ELSE 3 * dbo.WordCount(@Word5, Name) + dbo.WordCount(@Word5, Description)
          END
          AS Rank
   FROM Product
   ORDER BY Rank DESC

/* Save the number of searched products in an output variable */
SELECT @HowManyResults = COUNT(*) 
FROM @Products 
WHERE Rank > 0

/* Send back the requested products */
SELECT ProductID, Name, Description, Price, Image1FileName, Image2FileName, Rank
FROM @Products
WHERE Rank > 0
  AND RowNumber BETWEEN (@PageNumber-1) * @ProductsPerPage + 1 
                    AND @PageNumber * @ProductsPerPage
ORDER BY Rank DESC

GO

