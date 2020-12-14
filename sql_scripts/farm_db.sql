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

print '' print '*** creating addressstate table ***'
GO
CREATE TABLE [dbo].[AddressState] (
	[AddressState]	[char](2)	NOT NULL
	CONSTRAINT [pk_addressState_addressState] PRIMARY KEY([AddressState] ASC)
)
GO

print '' print '***  creating addressstate records ***'
GO
INSERT INTO [dbo].[AddressState]
	([AddressState])
	VALUES
		("AR")
		,("AZ")
		,("CA")
		,("CO")
		,("CT")
		,("DC")
		,("DE")
		,("FL")
		,("GA")
		,("HI")
		,("IA")
		,("ID")
		,("IL")
		,("IN")
		,("KS")
		,("KY")
		,("LA")
		,("MA")
		,("MD")
		,("ME")
		,("MI")
		,("MN")
		,("MO")
		,("MS")
		,("MT")
		,("NC")
		,("NE")
		,("NH")
		,("NJ")
		,("NM")
		,("NV")
		,("NY")
		,("ND")
		,("OH")
		,("OK")
		,("OR")
		,("PA")
		,("RI")
		,("SC")
		,("SD")
		,("TN")
		,("TX")
		,("UT")
		,("VT")
		,("VA")
		,("WA")
		,("WI")
		,("WV")
		,("WY")
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
		, ('Customer')
GO



print '' print '*** creating farmoperation table ***'
GO

CREATE TABLE [dbo].[FarmOperation] (
	[OperationName]	[nvarchar](64)	NOT NULL,
	[OperationID]		[int]		IDENTITY(100000, 1) NOT NULL,
	[UserID_Operator]	[int]		NOT NULL,
	[AddressState]		[char](2)		NOT NULL,
	[MaxShares]		[int]		NULL,
	[Active]			[bit]		NOT NULL DEFAULT 1
	CONSTRAINT [pk_farmOperation_operationID] PRIMARY KEY([OperationID] ASC),
	CONSTRAINT [fk_farmOperation_AddressState] FOREIGN KEY([AddressState])
		REFERENCES [dbo].[AddressState]([AddressState])
)
GO

print '' print '***  creating farmoperation test records ***'
GO
INSERT INTO [dbo].[FarmOperation]
	([OperationName],[UserID_Operator],[AddressState],[Active])
	VALUES
		('Tebows Farm', 100002, "IA", 1)
GO

print '' print '***  creating userrole table ***'
GO
CREATE TABLE [dbo].[UserRole] (
	[OperationID]	[int]		NULL,
	[UserID]		[int]		NOT NULL,
	[RoleName]	[nvarchar](64)	NOT NULL,
	CONSTRAINT [fk_userRole_userID] FOREIGN KEY([UserID])
		REFERENCES [dbo].[UserAccount](UserID)
)
GO

print '' print '***  creating userrole test records ***'
GO
INSERT INTO [dbo].[UserRole]
	([OperationID],[UserID],[RoleName])
	VALUES
		(100000, 100002, 'Farmer')
GO

print '' print '***  creating task table ***'
GO
CREATE TABLE [dbo].[Task] (
	[UserID_Sender]		[int]		NOT NULL,
	[UserID_Assignee]		[int]		NOT NULL,
	[AssignDate]			[DateTime]	NOT NULL,
	[DueDate]				[DateTime]	NULL,
	[TaskName]			[nvarchar](64) NOT NULL,
	[TaskDescription]		[nvarchar](1024) NULL,
	[Finished]			[bit]		NOT NULL DEFAULT 0
	CONSTRAINT [pk_task_taskID] PRIMARY KEY([UserID_Sender],
		[UserID_Assignee], [AssignDate] ASC)
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
	[SharePortion]					[decimal](10,2)NOT NULL DEFAULT 1,
	[Frequency]					[int]		NOT NULL DEFAULT 1,
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
	[UserID_Customer]	[int]		NOT NULL,
	[AddressState]		[char](2)		NOT NULL,
	CONSTRAINT [fk_restaraunt_AddressState] FOREIGN KEY([AddressState])
		REFERENCES [dbo].[AddressState]([AddressState])
)
GO

print '' print '*** creating directsale table ***'
GO
CREATE TABLE [dbo].[DirectSale] (
	[UserID_Customer]		[int]		NOT NULL,
	[AddressState]			[char](2)		NOT NULL,
	CONSTRAINT [fk_directSale_userID_customer] FOREIGN KEY([UserID_Customer])
		REFERENCES [dbo].[UserAccount](UserID),
	CONSTRAINT [fk_directSale_AddressState] FOREIGN KEY([AddressState])
			REFERENCES [dbo].[AddressState]([AddressState])
)
GO

print '' print '*** creating marketstall table ***'
GO
CREATE TABLE [dbo].[MarketStall] (
	[OperationID]				[int]		NOT NULL,
	[Frequency]				[int]		NOT NULL,
	[AddressState]			[char](2)		NOT NULL,
	CONSTRAINT [fk_marketStall_AddressState] FOREIGN KEY([AddressState])
			REFERENCES [dbo].[AddressState]([AddressState]),
	CONSTRAINT [fk_marketStall_operationID] FOREIGN KEY([OperationID])
		REFERENCES [dbo].[FarmOperation](OperationID)
)
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


print '' print '*** stored procedures for useraccount ***'
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
		SELECT SCOPE_IDENTITY()
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

print '' print '*** stored procedures for role ***'
GO

print '' print '*** creating sp_create_role ***'
GO
CREATE PROCEDURE [dbo].[sp_create_role]
	(
		@RoleName				[nvarchar](64)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Role]
				([RoleName])
			VALUES
				(@RoleName)
	END
GO

print '' print '*** creating sp_select_all_role ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_role]
AS
	BEGIN
		SELECT RoleName
		FROM Role
		ORDER BY RoleName ASC
	END
GO

print '' print '*** creating sp_delete_role ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_role]
	(
		@RoleName				[nvarchar](64)
	)
AS
	BEGIN
		DELETE FROM Role
		WHERE RoleName = @RoleName
		RETURN @@ROWCOUNT
	END
GO



print '' print '*** stored procedures for userrole ***'
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
		INSERT INTO [dbo].[UserRole]
				([UserID], [RoleName])
			VALUES
				(@UserID, @RoleName)
	END
GO

print '' print '*** creating sp_create_user_role_with_operation ***'
GO
CREATE PROCEDURE [dbo].[sp_create_user_role_with_operation]
	(
		@UserID				[int],
		@OperationID			[int],
		@RoleName				[nvarchar](64)
	)
AS
	BEGIN
		INSERT INTO [dbo].[UserRole]
				([UserID], [OperationID], [RoleName])
			VALUES
				(@UserID, @OperationID, @RoleName)
	END
GO

print '' print '*** creating sp_select_user_role_by_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_user_role_by_id]
	(
		@UserID				[int]
	)
AS
	BEGIN
		SELECT RoleName
		FROM UserRole
		WHERE @UserID = UserID
	END
GO

print '' print '*** creating sp_select_user_role_by_operation ***'
GO
CREATE PROCEDURE [dbo].[sp_select_user_role_by_operation]
	(
		@OperationID				[int]
	)
AS
	BEGIN
		SELECT UserAccount.UserID, FirstName, LastName, Email, RoleName
		FROM UserRole
			JOIN UserAccount ON UserRole.UserID = UserAccount.UserID
		WHERE UserRole.OperationID = @OperationID
	END
GO

print '' print '*** creating sp_update_user_role_by_email ***'
GO
CREATE PROCEDURE [dbo].[sp_update_user_role_by_email]
	(
		@UserID				[int],
		@RoleName				[nvarchar](64)
	)
AS
	BEGIN
		IF EXISTS (SELECT RoleName
					FROM UserRole
					WHERE RoleName = @RoleName)
		UPDATE UserRole
			SET RoleName = @RoleName
			WHERE UserID = @UserID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** stored procedures for addressstate ***'
GO

print '' print '*** creating sp_select_all_address ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_address]
AS
BEGIN
	SELECT AddressState
	FROM addressstate
	ORDER BY AddressState ASC
END
GO

print '' print '*** stored procedures for farmoperation ***'
GO

print '' print '*** creating sp_create_farmoperation ***'
GO
CREATE PROCEDURE [dbo].[sp_create_farmoperation]
	(
		@UserID_Operator		[int],
		@AddressState			[char](2),
		@OperationName			[nvarchar](64)
	)
AS
BEGIN
	INSERT INTO [dbo].[FarmOperation]
			(UserID_Operator,AddressState,OperationName)
		VALUES
			(@UserID_Operator, @AddressState,@OperationName)
	SELECT SCOPE_IDENTITY()
END
GO

print '' print '*** creating sp_select_all_farmoperation ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_farmoperation]
AS
BEGIN
	SELECT OperationID, OperationName, UserID_Operator, AddressState, MaxShares, Active
	FROM FarmOperation
	ORDER BY OperationName ASC
END
GO

print '' print '*** creating sp_select_farmoperation_by_operator ***'
GO
CREATE PROCEDURE [dbo].[sp_select_farmoperation_by_operator]
	(
		@UserID_Operator		[int]
	)
AS
BEGIN
	SELECT OperationID, OperationName, AddressState, MaxShares, Active
	FROM FarmOperation
	WHERE UserID_Operator = @UserID_Operator
END
GO

print '' print '*** creating sp_select_farmoperation_by_state ***'
GO
CREATE PROCEDURE [dbo].[sp_select_farmoperation_by_addressstate]
	(
		@AddressState			[char](2)
	)
AS
BEGIN
	SELECT OperationID, OperationName, MaxShares, Active
	FROM FarmOperation
	WHERE AddressState = @AddressState
END
GO

print '' print '*** creating sp_update_farmoperation ***'
GO
CREATE PROCEDURE [dbo].[sp_update_farmoperation]
	(
		@OperationID				[int],
		@UserID_Operator			[int],
		@MaxShares				[int],
		@AddressState				[char](2),
		@Active					[bit],
		@OperationName				[nvarchar](64)
	)
AS
	BEGIN
		UPDATE FarmOperation
			SET UserID_Operator = @UserID_Operator,
				MaxShares = @MaxShares,
				AddressState = @AddressState,
				Active = @Active,
				OperationName = @OperationName
			WHERE OperationID = @OperationID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_remove_farmoperation ***'
GO
CREATE PROCEDURE [dbo].[sp_remove_farmoperation]
	(
		@OperationID				[int],
		@UserID_Operator			[int],
		@OperationName				[nvarchar](64)
	)
AS
	BEGIN
		DELETE FROM FarmOperation
			WHERE OperationID = @OperationID
			AND UserID_Operator = @UserID_Operator
			AND OperationName = @OperationName
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** stored procedures for task ***'
GO

print '' print '*** creating sp_create_task ***'
GO
CREATE PROCEDURE [dbo].[sp_create_task]
	(
		@UserID_Sender			[int],
		@UserID_Assignee		[int],
		@AssignDate			[DateTime],
		@DueDate				[DateTime],
		@TaskName				[nvarchar](64),
		@TaskDescription		[nvarchar](1024),
		@Finished				[bit]
	)
AS
	BEGIN
		INSERT INTO [dbo].[Task]
				(UserID_Sender, UserID_Assignee, AssignDate, DueDate,
					TaskName, TaskDescription)
			VALUES
				(@UserID_Sender, @UserID_Assignee, @AssignDate, @DueDate,
					@TaskName, @TaskDescription)
	END
GO

print '' print '*** creating sp_select_task_by_sender ***'
GO
CREATE PROCEDURE [dbo].[sp_select_task_by_sender]
	(
		@UserID_Sender			[int]
	)
AS
	BEGIN
		SELECT UserID_Assignee,AssignDate,DueDate,
		TaskName,TaskDescription,Finished
		FROM Task
		WHERE UserID_Sender = @UserID_Sender
	END
GO

print '' print '*** creating sp_select_task_by_assignee ***'
GO
CREATE PROCEDURE [dbo].[sp_select_task_by_assignee]
	(
		@UserID_Assignee		[int],
		@Finished				[bit]
	)
AS
	BEGIN
		SELECT UserID_Sender,AssignDate,DueDate,TaskName,TaskDescription
		FROM Task
		WHERE UserID_Assignee = @UserID_Assignee
			AND Finished = @Finished
	END
GO

print '' print '*** creating sp_finish_task ***'
GO
CREATE PROCEDURE [dbo].[sp_finish_task]
	(
		@UserID_Sender			[int],
		@UserID_Assignee		[int],
		@AssignDate			[DateTime],
		@TaskName				[nvarchar](64),
		@TaskDescription		[nvarchar](1024)
	)
AS
	BEGIN
		UPDATE Task
		SET Finished = 1
		WHERE UserID_Sender = @UserID_Sender
			AND UserID_Assignee = @UserID_Assignee
			AND AssignDate = @AssignDate
			AND TaskName = @TaskName
			AND TaskDescription = @TaskDescription
		RETURN @@ROWCOUNT
	END
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

print '' print '*** stored procedures for weeklyshare ***'
GO

print '' print '*** creating sp_create_weeklyshare ***'
GO
CREATE PROCEDURE [dbo].[sp_create_weeklyshare]
	(
		@UserID_Customer				[int],
		@OperationID					[int],
		@SharePortion					[decimal](10,2),
		@Frequency					[int]
	)
AS
	BEGIN
		INSERT INTO [dbo].[WeeklyShare]
				(UserID_Customer, OperationID, SharePortion, Frequency)
			VALUES
				(@UserID_Customer, @OperationID, @SharePortion, @Frequency)
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_select_weeklyshare_by_customer ***'
GO
CREATE PROCEDURE [dbo].[sp_select_weeklyshare_by_customer]
	(
		@UserID_Customer		[int]
	)
AS
	BEGIN
		SELECT OperationID,SharePortion,Frequency
		FROM WeeklyShare
		WHERE UserID_Customer = @UserID_Customer
	END
GO

print '' print '*** creating sp_select_weeklyshare_by_operation ***'
GO
CREATE PROCEDURE [dbo].[sp_select_weeklyshare_by_operation]
	(
		@OperationID			[int]
	)
AS
	BEGIN
		SELECT UserID_Customer,SharePortion,Frequency
		FROM WeeklyShare
		WHERE OperationID = @OperationID
	END
GO

print '' print '*** creating sp_update_weeklyshare ***'
GO
CREATE PROCEDURE [dbo].[sp_update_weeklyshare]
	(
		@UserID_Customer				[int],
		@OperationID					[int],
		@SharePortion					[decimal](10,2),
		@Frequency					[int]
	)
AS
	BEGIN
		UPDATE WeeklyShare
		SET SharePortion = @SharePortion,
			Frequency = @Frequency
		WHERE UserID_Customer = @UserID_Customer
			AND OperationID = @OperationID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_delete_weeklyshare ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_weeklyshare]
	(
		@UserID_Customer				[int],
		@OperationID					[int],
		@SharePortion					[decimal](10,2),
		@Frequency					[int]
	)
AS
	BEGIN
		DELETE FROM WeeklyShare
			WHERE UserID_Customer = @UserID_Customer
				AND OperationID = @OperationID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** stored procedures for restaraunt ***'
GO

print '' print '*** creating sp_create_restaraunt ***'
GO
CREATE PROCEDURE [dbo].[sp_create_restaraunt]
	(
		@UserID_Customer				[int],
		@AddressState					[char](2)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Restaraunt]
				(UserID_Customer, AddressState)
			VALUES
				(@UserID_Customer, @AddressState)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** creating sp_select_restaraunt_by_customer ***'
GO
CREATE PROCEDURE [dbo].[sp_select_restaraunt_by_customer]
	(
		@UserID_Customer				[int]
	)
AS
	BEGIN
		SELECT AddressState
		FROM Restaraunt
		WHERE UserID_Customer = @UserID_Customer
	END
GO

print '' print '*** creating sp_select_restaraunt_by_state ***'
GO
CREATE PROCEDURE [dbo].[sp_select_restaraunt_by_state]
	(
		@AddressState					[char](2)
	)
AS
	BEGIN
		SELECT UserID_Customer
		FROM Restaraunt
		WHERE AddressState = @AddressState
	END
GO

print '' print '*** creating sp_delete_restaraunt ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_restaraunt]
	(
		@UserID_Customer			[int],
		@AddressState				[char](2)
	)
AS
	BEGIN
		DELETE FROM Restaraunt
		WHERE UserID_Customer = @UserID_Customer
			AND AddressState = @AddressState
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** stored procedures for directsale ***'
GO

print '' print '*** creating sp_create_directsale ***'
GO
CREATE PROCEDURE [dbo].[sp_create_directsale]
	(
		@UserID_Customer				[int],
		@AddressState				[char](2)
	)
AS
	BEGIN
		INSERT INTO [dbo].[DirectSale]
				(UserID_Customer, AddressState)
			VALUES
				(@UserID_Customer, @AddressState)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** creating sp_select_directsale_by_customer ***'
GO
CREATE PROCEDURE [dbo].[sp_select_directsale_by_customer]
	(
		@UserID_Customer				[int]
	)
AS
	BEGIN
		SELECT AddressState
		FROM Restaraunt
		WHERE UserID_Customer = @UserID_Customer
	END
GO

print '' print '*** creating sp_select_directsale_by_state ***'
GO
CREATE PROCEDURE [dbo].[sp_select_directsale_by_state]
	(
		@AddressState				[char](2)
	)
AS
	BEGIN
		SELECT UserID_Customer
		FROM Restaraunt
		WHERE AddressState = @AddressState
	END
GO

print '' print '*** creating sp_delete_directsale ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_directsale]
	(
		@UserID_Customer			[int],
		@AddressState				[char](2)
	)
AS
	BEGIN
		DELETE FROM DirectSale
		WHERE UserID_Customer = @UserID_Customer
			AND AddressState = @AddressState
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** stored procedures for marketstall ***'
GO

print '' print '*** creating sp_create_marketstall ***'
GO
CREATE PROCEDURE [dbo].[sp_create_marketstall]
	(
		@OperationID					[int],
		@AddressState					[char](2),
		@Frequency					[int]
	)
AS
	BEGIN
		INSERT INTO [dbo].[MarketStall]
				(OperationID, AddressState, Frequency)
			VALUES
				(@OperationID, @AddressState, @Frequency)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** creating sp_select_marketstall_by_operation ***'
GO
CREATE PROCEDURE [dbo].[sp_select_marketstall_by_operation]
	(
		@OperationID				[int]
	)
AS
	BEGIN
		SELECT AddressState, Frequency
		FROM MarketStall
		WHERE OperationID = @OperationID
	END
GO

print '' print '*** creating sp_delete_marketstall ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_marketstall]
	(
		@OperationID					[int],
		@AddressState					[char](2),
		@Frequency					[int]
	)
AS
	BEGIN
		DELETE FROM MarketStall
		WHERE OperationID = @OperationID
			AND AddressState = @AddressState
			AND Frequency = @Frequency
		RETURN @@ROWCOUNT
	END
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
