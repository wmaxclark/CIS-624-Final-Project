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