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
		('test@test.com', 'Test', 'Test')
		, ('randy@company.com', 'Randy', 'Savage')
		, ('tim@company.com', 'Tim', 'Tebow')
		, ('burna@company.com', 'Burna', 'Boy')
		, ('ed@company.com', 'Ed', 'Sheeran')
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

print '' print '*** creating sp_select_user_by_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_id]
	(
		@UserID				[nvarchar](100)
	)
AS
	BEGIN
		SELECT FirstName, LastName, Email, Active
		FROM UserAccount
		WHERE UserID = @UserID
	END
GO
