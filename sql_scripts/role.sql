print '' print '*** using farm_db ***'
GO

USE farm_db
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