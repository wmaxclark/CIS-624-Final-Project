print '' print '*** using farm_db ***'
GO

USE farm_db
GO

print '' print '*** creating product table ***'
GO
CREATE TABLE [dbo].[Product] (
	[ProductID]					[int]	IDENTITY(100000, 1) NOT NULL,
	[OperationID]					[int]			NOT NULL,
	[ProductName]					[nvarchar](64)		NOT NULL,
	[ProductDescription]			[nvarchar](1024) 	NOT NULL,
	[InputCost]					[decimal](10,2)	NOT NULL,
	[Unit]						[nvarchar](64)		NOT NULL,
	[UnitPrice]					[decimal](10,2)	NOT NULL,
	[GerminationDate]				[DateTime]		NOT NULL,
	[PlantDate]					[DateTime]		NOT NULL,
	[TransplantDate]				[DateTime]		NOT NULL,
	[HarvestDate]					[DateTime]		NOT NULL,
	CONSTRAINT [pk_product_productID] PRIMARY KEY([ProductID] ASC),
	CONSTRAINT [fk_product_operation] FOREIGN KEY([OperationID])
		REFERENCES [dbo].[FarmOperation](OperationID)
)
GO

print '' print '***  creating product test records ***'
GO
INSERT INTO [dbo].[Product]
	([OperationID],[ProductName],[ProductDescription],[InputCost],[Unit], [UnitPrice],[GerminationDate],[PlantDate],[TransplantDate],[HarvestDate])
	VALUES
		(100000, "Pawpaw", "(Asimina triloba) the American papaw, a small deciduous tree native to the eastern United States and Canada", 1.0, "Single", 4.0, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-09-30 00:00:00"),
		(100000, "Mangosteen", "(Garcinia mangostana) is an exotic, tropical fruit with a slightly sweet and sour flavor.", 10.1, "Bundle", 20, "2021-05-10 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100000, "Cranberries", "Cranberries are a group of evergreen dwarf shrubs or trailing vines in the subgenus Oxycoccus of the genus Vaccinium. ", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100000, "Apple", "An apple is an edible fruit produced by an apple tree (Malus domestica)", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100000, "Pumpkin", "A pumpkin is a cultivar of winter squash that is round with smooth, slightly ribbed skin, and is most often deep yellow to orange in coloration.", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100001, "Pawpaw", "(Asimina triloba) the American papaw, a small deciduous tree native to the eastern United States and Canada", 1.0, "Single", 4.0, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-09-30 00:00:00"),
		(100001, "Mangosteen", "(Garcinia mangostana) is an exotic, tropical fruit with a slightly sweet and sour flavor.", 10.1, "Bundle", 20, "2021-05-10 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100001, "Cranberries", "Cranberries are a group of evergreen dwarf shrubs or trailing vines in the subgenus Oxycoccus of the genus Vaccinium. ", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100001, "Apple", "An apple is an edible fruit produced by an apple tree (Malus domestica)", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100001, "Pumpkin", "A pumpkin is a cultivar of winter squash that is round with smooth, slightly ribbed skin, and is most often deep yellow to orange in coloration.", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100002, "Pawpaw", "(Asimina triloba) the American papaw, a small deciduous tree native to the eastern United States and Canada", 1.0, "Single", 4.0, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-09-30 00:00:00"),
		(100002, "Mangosteen", "(Garcinia mangostana) is an exotic, tropical fruit with a slightly sweet and sour flavor.", 10.1, "Bundle", 20, "2021-05-10 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100002, "Cranberries", "Cranberries are a group of evergreen dwarf shrubs or trailing vines in the subgenus Oxycoccus of the genus Vaccinium. ", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100002, "Apple", "An apple is an edible fruit produced by an apple tree (Malus domestica)", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00"),
		(100002, "Pumpkin", "A pumpkin is a cultivar of winter squash that is round with smooth, slightly ribbed skin, and is most often deep yellow to orange in coloration.", 10.1, "Bundle", 20, "2021-05-09 00:00:00","2021-05-16 00:00:00","2021-05-23 00:00:00","2021-05-30 00:00:00")
GO

print '' print '*** stored procedures for product ***'
GO

print '' print '*** creating sp_create_product ***'
GO
CREATE PROCEDURE [dbo].[sp_create_product]
	(
		@OperationID					[int],
		@ProductName					[nvarchar](64),
		@ProductDescription				[nvarchar](1024),
		@InputCost					[decimal](10,2),
		@Unit						[nvarchar](64),
		@UnitPrice					[decimal](10,2),
		@GerminationDate				[DateTime],
		@PlantDate					[DateTime],
		@TransplantDate				[DateTime],
		@HarvestDate					[DateTime]
	)
AS
	BEGIN
		INSERT INTO [dbo].[Product]
				(OperationID, ProductName, ProductDescription, InputCost,
					Unit, UnitPrice, GerminationDate,
					PlantDate,
					TransplantDate,
					HarvestDate)
			VALUES
				(@OperationID, @ProductName,
					@ProductDescription, @InputCost,
					@Unit, @UnitPrice, @GerminationDate,
					@PlantDate,
					@TransplantDate,
					@HarvestDate)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** creating sp_select_product_by_operation ***'
GO
CREATE PROCEDURE [dbo].[sp_select_product_by_operation]
	(
		@OperationID					[int]
	)
AS
	BEGIN
		SELECT ProductID, ProductName, ProductDescription, InputCost,
			Unit, UnitPrice, GerminationDate,
			PlantDate,
			TransplantDate,
			HarvestDate
		FROM Product
		WHERE OperationID = @OperationID
		ORDER BY ProductName ASC
	END
GO

print '' print '*** creating sp_select_product_by_productid ***'
GO
CREATE PROCEDURE [dbo].[sp_select_product_by_productid]
	(
		@ProductID					[int]
	)
AS
	BEGIN
		SELECT OperationID, ProductName, ProductDescription, InputCost,
			Unit, UnitPrice, GerminationDate,
			PlantDate,
			TransplantDate,
			HarvestDate
		FROM Product
		WHERE ProductID = @ProductID
		ORDER BY ProductName ASC
	END
GO

print '' print '*** creating sp_update_product ***'
GO
CREATE PROCEDURE [dbo].[sp_update_product]
	(
		@ProductID					[int],
		@OperationID					[int],
		@ProductName					[nvarchar](64),
		@ProductDescription				[nvarchar](1024),
		@InputCost					[decimal](10,2),
		@Unit						[nvarchar](64),
		@UnitPrice					[decimal](10,2),
		@GerminationDate				[DateTime],
		@PlantDate					[DateTime],
		@TransplantDate				[DateTime],
		@HarvestDate					[DateTime]
	)
AS
	BEGIN
		UPDATE Product
		SET OperationID = @OperationID,
			ProductName = @ProductName,
			ProductDescription = @ProductDescription,
			InputCost = @InputCost,
			Unit = @Unit,
			UnitPrice = @UnitPrice,
			GerminationDate = @GerminationDate,
			PlantDate = @PlantDate,
			TransplantDate =
			@TransplantDate,
			HarvestDate = @HarvestDate
		WHERE ProductID = @ProductID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_delete_product ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_product]
	(
		@ProductID					[int]
	)
AS
	BEGIN
		DELETE FROM Product
			WHERE ProductID = @ProductID
		RETURN @@ROWCOUNT
	END
GO
