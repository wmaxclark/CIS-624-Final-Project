print '' print '*** using farm_db ***'
GO

USE farm_db
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