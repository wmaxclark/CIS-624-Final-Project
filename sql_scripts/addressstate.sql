print '' print '*** using farm_db ***'
GO

USE farm_db
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