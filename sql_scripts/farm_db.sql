IF EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'farm_db')
BEGIN
	DROP DATABASE farm_db
	print '' print '*** dropping database farm_db ***'
END
GO

print '' print '*** creating farm_db ***'
GO
CREATE DATABASE farm_db
GO

print '' print '*** using farm_db ***'
GO

USE farm_db
GO

print '' print '*** creating useraccount table ***'
GO
CREATE TABLE [dbo].[UserAccount] (
	[UserID]		[int]		IDENTITY(100000, 1) NOT NULL,
	[Email]			[nvarchar](100)	NOT NULL,
	[FirstName]		[nvarchar](64)	NOT NULL,
	[LastName]		[nvarchar](64)	NOT NULL,
	[PasswordHash]	[nvarchar](100) NOT NULL DEFAULT
	'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E',
	[Active]		[bit]			NOT NULL DEFAULT 1,
	CONSTRAINT [pk_userAccount_userID] PRIMARY KEY([UserID] ASC),
	CONSTRAINT [ak_userAccount_email] UNIQUE([Email] ASC)
)
GO

print '' print '*** creating useraccount test records ***'
GO
INSERT INTO [dbo].[UserAccount]
		([Email], [FirstName], [LastName])
	VALUES
		('mark@company.com', 'Mark', 'Marcus')
		, ('randy@company.com', 'Randy', 'Savage')
		, ('tim@company.com', 'Tim', 'Tebow')
		, ('burna@company.com', 'Burna', 'Boy')
		, ('ed@company.com', 'Ed', 'Sheeran')
GO

print '' print '*** creating address table ***'
GO
CREATE TABLE [dbo].[Address] (
	[ZipCode]		[int]		NOT NULL,
	[AddressState]	[char](2)	NOT NULL,
	CONSTRAINT [pk_zipCode] PRIMARY KEY([ZipCode] ASC)
)
GO

print '' print '*** creating role table ***'
GO
CREATE TABLE [dbo].[Role] (
	[RoleName]		[nvarchar](64)	NOT NULL,
	CONSTRAINT [ak_role_roleName] UNIQUE([RoleName] ASC)
)
GO

print '' print '***  creating role test records ***'
GO
INSERT INTO [dbo].[Role]
	([RoleName])
	VALUES
		('Farmer')
		, ('Helper')
		, ('Customer')
GO

print '' print '***  creating userrole table ***'
GO
CREATE TABLE [dbo].[UserRole] (
	[UserID]	[int]			NOT NULL,
	[RoleName]	[nvarchar](64)	NOT NULL,
	CONSTRAINT [fk_userRole_userID] FOREIGN KEY([UserID])
		REFERENCES [dbo].[UserAccount](UserID)
)
GO

print '' print '*** creating farmoperation table ***'
GO

CREATE TABLE [dbo].[FarmOperation] (
	[OperationID]		[int]		IDENTITY(100000, 1) NOT NULL,
	[UserID_Operator]	[int]		NOT NULL,
	[ZIP]				[int]		NOT NULL,
	[Active]			[bit]		NOT NULL DEFAULT 1
	CONSTRAINT [pk_farmOperation_operationID] PRIMARY KEY([OperationID] ASC),
	CONSTRAINT [fk_farmOperation_zip] FOREIGN KEY([ZIP])
		REFERENCES [dbo].[Address]([ZipCode])
)
GO

print '' print '***  creating task table ***'
GO
CREATE TABLE [dbo].[Task] (
	[UserID_Sender]			[int]		NOT NULL,
	[UserID_Assignee]		[int]		NOT NULL,
	[AssignDate]			[DateTime]	NOT NULL,
	[DueDate]				[DateTime]	NOT NULL,
	[TaskName]				[nvarchar](64) NOT NULL,
	[TaskDescription]		[nvarchar](1024) NULL,
	[Finished]				[bit]		NOT NULL DEFAULT 0
	CONSTRAINT [pk_task_taskID] PRIMARY KEY([UserID_Sender], [UserID_Assignee], [AssignDate] ASC)
	CONSTRAINT [fk_task_userID_sender] FOREIGN KEY([UserID_Sender])
		REFERENCES [dbo].[UserAccount](UserID),
	CONSTRAINT [fk_task_userID_assignee] FOREIGN KEY([UserID_Assignee])
		REFERENCES [dbo].[UserAccount](UserID)
)
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

print '' print '*** creating weeklyshare table ***'
GO
CREATE TABLE [dbo].[WeeklyShare] (
	[UserID_Customer]				[int]		NOT NULL,
	[OperationID]					[int]		NOT NULL,
	[SharePortion]					[decimal](10,2)	NOT NULL DEFAULT 1,
	[Frequency]					[int]		NOT NULL DEFAULT 1,
	CONSTRAINT [pk_weeklyShare_shareID] PRIMARY KEY([OperationID], [UserID_Customer] ASC),
	CONSTRAINT [fk_weeklyShare_userID_customer] FOREIGN KEY([UserID_Customer])
		REFERENCES [dbo].[UserAccount](UserID)
		ON UPDATE CASCADE,
	CONSTRAINT [fk_weeklyShare_operationID] FOREIGN KEY([OperationID])
		REFERENCES [dbo].[FarmOperation](OperationID)
		ON UPDATE CASCADE
)
GO

print '' print '*** creating restaraunt table ***'
GO
CREATE TABLE [dbo].[Restaraunt] (
	[UserID_Customer]		[int]		NOT NULL,
	[ZIP]					[int]		NOT NULL,
	CONSTRAINT [fk_restaraunt_userID_customer] FOREIGN KEY([UserID_Customer])
		REFERENCES [dbo].[UserAccount](UserID)
)
GO

print '' print '*** creating directsale table ***'
GO
CREATE TABLE [dbo].[DirectSale] (
	[UserID_Customer]		[int]		NOT NULL,
	[ZIP]					[int]		NOT NULL,
	CONSTRAINT [fk_directSale_userID_customer] FOREIGN KEY([UserID_Customer])
		REFERENCES [dbo].[UserAccount](UserID)
		ON UPDATE CASCADE
)
GO

print '' print '*** creating marketstall table ***'
GO
CREATE TABLE [dbo].[MarketStall] (
	[OperationID]			[int]		NOT NULL,
	[ZIP]					[int]		NOT NULL,
	[Frequency]				[int]		NOT NULL
	CONSTRAINT [fk_marketStall_operationID] FOREIGN KEY([OperationID])
		REFERENCES [dbo].[FarmOperation](OperationID)
		ON UPDATE CASCADE
)
GO

print '' print '*** creating order table ***'
GO
CREATE TABLE [dbo].[ProductOrder] (
	[OrderID]				[int]		IDENTITY(100000, 1) NOT NULL,
	[UserID_Customer]		[int]		NOT NULL,
	[OperationID]			[int]		NOT NULL,
	[OrderDate]				[datetime]	NOT NULL
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


print '' print '*** STORED PROCEDURES FOR USERS ***'
GO

print '' print '*** creating sp_create_user_account ***'
GO
CREATE PROCEDURE [dbo].[sp_create_user_account]
	(
	@Email				[nvarchar](100),
	@FirstName			[nvarchar](64),
	@LastName				[nvarchar](64),
	@PasswordHash			[nvarchar](100)
	)
AS
	BEGIN
		INSERT INTO [dbo].[UserAccount]
				([Email], [FirstName], [LastName], [PasswordHash])
			VALUES
				(@Email, @FirstName, @LastName, @PasswordHash)
		RETURN @@ROWCOUNT
	END
GO


print '' print '*** creating sp_authenticate_user ***'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
	(
	@Email					[nvarchar](100),
	@PasswordHash				[nvarchar](100)
	)
AS
	BEGIN
		SELECT COUNT(Email)
		FROM UserAccount
		WHERE Email = @Email
		  AND PasswordHash = @PasswordHash
		  AND Active = 1
	END
GO

print '' print '*** creating sp_update_passwordhash ***'
GO
CREATE PROCEDURE [dbo].[sp_update_passwordhash]
	(
		@Email				[nvarchar](100),
		@OldPasswordHash		[nvarchar](100),
		@NewPasswordHash		[nvarchar](100)
	)
AS
	BEGIN
		UPDATE UserAccount
			SET PasswordHash = @NewPasswordHash
			WHERE Email = @Email
			  AND PasswordHash = @OldPasswordHash
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_select_user_by_email ***'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_email]
	(
		@Email				[nvarchar](100)
	)
AS
	BEGIN
		SELECT UserID, Email, FirstName, LastName, Active
		FROM UserAccount
		WHERE Email = @Email
	END
GO

print '' print '*** creating sp_create_user_role ***'
GO
CREATE PROCEDURE [dbo].[sp_create_user_role]
	(
		@UserID				[int],
		@RoleName				[nvarchar](64)
	)
AS
	BEGIN
		IF EXISTS (SELECT RoleName
					FROM UserRole
					WHERE RoleName = @RoleName)
		INSERT INTO [dbo].[UserRole]
				([UserID], [RoleName])
			VALUES
				(@UserID, @RoleName)
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_select_user_role_by_email ***'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_email]
	(
		@Email				[nvarchar](100)
	)
AS
	BEGIN
		SELECT RoleName
		FROM UserRole
			JOIN UserAccount ON UserRole.UserID = UserAccount.UserID
		WHERE @Email = UserAccount.Email
	END
GO

print '' print '*** creating sp_update_user_role_by_email ***'
GO
CREATE PROCEDURE [dbo].[sp_update_user_role]
	(
		@UserID				[int],
		@Email 				[nvarchar](100),
		@RoleName				[nvarchar](64)
	)
AS
	BEGIN
		IF EXISTS (SELECT RoleName
					FROM UserRole
					WHERE RoleName = @RoleName)
		UPDATE UserRole
			SET RoleName = @RoleName
			WHERE UserID = @UserID AND Email = @Email
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_create_address ***'
GO
CREATE PROCEDURE [dbo].[sp_create_address]
	(
		@ZipCode				[int],
		@AddressState			[char](2)
	)
AS
BEGIN
	INSERT INTO [dbo].[Addres]
			([ZipCode], [AddressState])
		VALUES
			(@ZipCode, @AddressState)
	RETURN @@ROWCOUNT
END
GO

print '' print '*** creating sp_select_address_by_zipcode ***'
GO
CREATE PROCEDURE [dbo].[sp_create_address]
	(
		@ZipCode				[int]
	)
AS
BEGIN
	SELECT ZipCode, AddressState
	FROM Address
	WHERE ZipCode = @ZipCode
END
GO
