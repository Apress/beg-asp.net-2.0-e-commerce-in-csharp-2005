CREATE TABLE Department(
	DepartmentID INT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(50) NOT NULL,
	Description VARCHAR(1000) NULL,
 CONSTRAINT PK_Department PRIMARY KEY CLUSTERED (DepartmentID ASC)
)

GO

CREATE PROCEDURE GetDepartments AS
SELECT DepartmentID, Name, Description
FROM Department

GO

INSERT INTO Department(Name, Description)		
VALUES ('Anniversary Balloons', 'These sweet balloons are the perfect gift for someone you love.')
GO

INSERT INTO Department(Name, Description )		
VALUES ('Balloons for Children', 'The colorful and funny balloons will make any child smile!')
GO

