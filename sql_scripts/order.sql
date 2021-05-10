print '' print '*** using farm_db ***'
GO

USE farm_db
GO

print '' print '*** creating product order table ***'
GO
CREATE TABLE [dbo].[ProductOrder] (
	[OrderID]				[int]		IDENTITY(100000, 1) NOT NULL,
	[UserID_Customer]		[int]		NOT NULL,
	[OperationID]			[int]		NOT NULL,
	[OrderDate]			[datetime]	NOT NULL
	CONSTRAINT [pk_productOrder_orderID] PRIMARY KEY([OrderID] ASC),
	CONSTRAINT [fk_productOrder_userID_customer] FOREIGN KEY([UserID_Customer])
		REFERENCES [dbo].[UserAccount]([UserID])
		ON UPDATE CASCADE,
	CONSTRAINT [fk_productOrder_operationID] FOREIGN KEY([OperationID])
		REFERENCES [dbo].[FarmOperation]([OperationID])
		ON UPDATE CASCADE
)
GO

print '' print '*** creating orderline table ***'
GO
CREATE TABLE [dbo].[OrderLine] (
	[OrderID]				[int]		NOT NULL,
	[ProductID]			[int]		NOT NULL,
	[OrderLineID]			[int]		IDENTITY(1, 1) NOT NULL,
	[PriceCharged]			[decimal](10,2) NOT NULL
	CONSTRAINT [pk_orderLine_orderLineID] PRIMARY KEY([OrderLineID] ASC),
	CONSTRAINT [fk_orderLine_orderID] FOREIGN KEY([OrderID])
		REFERENCES [dbo].[ProductOrder]([OrderID]),
	CONSTRAINT [fk_orderLine_productID] FOREIGN KEY([ProductID])
		REFERENCES [dbo].[Product]([ProductID]),
)
GO

print '' print '*** stored procedures for productorder ***'
GO

print '' print '*** creating sp_create_productorder ***'
GO
CREATE PROCEDURE [dbo].[sp_create_productorder]
	(
		@UserID_Customer				[int],
		@OperationID					[int],
		@OrderDate					[DateTime]
	)
AS
	BEGIN
		INSERT INTO [dbo].[ProductOrder]
				(UserID_Customer, OperationID, OrderDate)
			VALUES
				(@UserID_Customer, @OperationID, @OrderDate)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** creating sp_select_productorder_by_operation ***'
GO
CREATE PROCEDURE [dbo].[sp_select_productorder_by_operation]
	(
		@OperationID				[int]
	)
AS
	BEGIN
		SELECT OrderID, UserID_Customer, OrderDate
		FROM ProductOrder
		WHERE OperationID = @OperationID
	END
GO

print '' print '*** creating sp_select_productorder_by_customer ***'
GO
CREATE PROCEDURE [dbo].[sp_select_productorder_by_customer]
	(
		@UserID_Customer				[int]
	)
AS
	BEGIN
		SELECT OrderID, OperationID, OrderDate
		FROM ProductOrder
		WHERE UserID_Customer = @UserID_Customer
		ORDER BY OrderDate DESC
	END
GO

print '' print '*** creating sp_delete_productorder ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_productorder]
	(
		@OrderID						[int]
	)
AS
	BEGIN
		DELETE FROM ProductOrder
		WHERE OrderID = @OrderID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** stored procedures for orderline ***'
GO

print '' print '*** creating sp_create_orderline ***'
GO
CREATE PROCEDURE [dbo].[sp_create_orderline]
	(
		@OrderID						[int],
		@ProductID					[int],
		@PriceCharged					[decimal](10,2)
	)
AS
	BEGIN
		INSERT INTO [dbo].[OrderLine]
				(OrderID, ProductID, PriceCharged)
			VALUES
				(@OrderID, @ProductID, @PriceCharged)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** creating sp_select_orderline_by_order ***'
GO
CREATE PROCEDURE [dbo].[sp_select_orderline_by_order]
	(
		@OrderID						[int]
	)
AS
	BEGIN
		SELECT ProductID, OrderLineID, PriceCharged
		FROM OrderLine
		WHERE OrderID = @OrderID
	END
GO

print '' print '*** creating sp_update_orderline ***'
GO
CREATE PROCEDURE [dbo].[sp_update_orderline]
	(
		@OrderID					[int],
		@ProductID					[int],
		@PriceCharged					[decimal](10,2)
	)
AS
	BEGIN
		UPDATE OrderLine
			SET ProductID = @ProductID,
			PriceCharged = @PriceCharged
		WHERE OrderID = @OrderID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_delete_orderline ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_orderline]
	(
		@OrderLineID						[int]
	)
AS
	BEGIN
		DELETE FROM OrderLine
		WHERE OrderLineID = @OrderLineID
		RETURN @@ROWCOUNT
	END
GO
