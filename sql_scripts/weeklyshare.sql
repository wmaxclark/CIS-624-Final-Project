print '' print '*** using farm_db ***'
GO

USE farm_db
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