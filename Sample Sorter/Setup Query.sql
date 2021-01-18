CREATE DATABASE SampleDatabase;

GO

USE SampleDatabase;

GO


CREATE PROCEDURE [dbo].[SampleDatabase_RecreateTable]
AS
	IF OBJECT_ID('[dbo].[SampleData]', 'U') IS NOT NULL 
		DROP TABLE [dbo].[SampleData]

	CREATE TABLE [dbo].[SampleData] (
		SampleId INT IDENTITY(1, 1) NOT NULL,
		SampleText VARCHAR(10) NOT NULL,
		PRIMARY KEY (SampleId)
	);

GO


CREATE PROCEDURE [dbo].[SampleData_GetList]
AS	SELECT 
	   [SampleId]
      ,[SampleText]
	FROM 
	   [dbo].[SampleData];

GO

CREATE PROCEDURE [dbo].[SampleData_GetListSorted]
AS
	SELECT 
	   [SampleId]
      ,[SampleText]
	FROM 
	   [dbo].[SampleData]
	ORDER BY [SampleText] ASC;

GO
