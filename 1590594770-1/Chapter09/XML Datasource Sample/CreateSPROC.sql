USE [AdventureWorks]
GO
/****** Object:  StoredProcedure [dbo].[GetProducts]    Script Date: 06/12/2005 15:48:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetProducts]
	-- Add the parameters for the function here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT     Production.Product.Name, Production.ProductCategory.Name AS CategoryName, Production.ProductSubcategory.Name AS SubCategoryName, 
                      Production.Product.ProductNumber, Production.Product.Color, Production.Product.StandardCost, Production.Product.ListPrice, Production.Product.Size, 
                      Production.Product.Weight, Production.Product.Class, Production.Product.Style, Production.Product.ProductID
FROM         Production.Product INNER JOIN
                      Production.ProductSubcategory ON Production.Product.ProductSubcategoryID = Production.ProductSubcategory.ProductSubcategoryID INNER JOIN
                      Production.ProductCategory ON Production.ProductSubcategory.ProductCategoryID = Production.ProductCategory.ProductCategoryID ORDER BY CategoryName, SubCategoryName, Production.Product.ListPrice
END


GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF