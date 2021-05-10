print '' print '*** using farm_db ***'
GO

USE farm_db
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
		(100000, 100000, 'Farmer'),
		(100001, 100001, 'Farmer'),
		(100002, 100002, 'Farmer'),
		(100003, 100003, 'Farmer')
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
