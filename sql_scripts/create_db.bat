ECHO off

rem batch file to run a script to create a database
rem 09/14/2020

sqlcmd -S YURY-BOT\LOCALHOST -E -i farm_db.sql
rem sqlcmd -S localhost\sqlexpress -E -i camp_db_pm.sql
rem sqlcmd -S localhost\mssqlserver -E -i camp_db_pm.sql

ECHO .
ECHO if no error message appear DB was created
PAUSE
