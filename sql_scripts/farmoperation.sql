print '' print '*** using farm_db ***'
GO

USE farm_db
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