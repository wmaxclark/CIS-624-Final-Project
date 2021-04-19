ECHO off

rem batch file to run a script to create a database
rem 09/14/2020

sqlcmd -S LOCALHOST -E -i farm_db.sql
sqlcmd -S LOCALHOST -E -i useraccount.sql
sqlcmd -S LOCALHOST -E -i addressstate.sql
sqlcmd -S LOCALHOST -E -i role.sql
sqlcmd -S LOCALHOST -E -i farmoperation.sql
sqlcmd -S LOCALHOST -E -i userrole.sql
sqlcmd -S LOCALHOST -E -i product.sql
sqlcmd -S LOCALHOST -E -i order.sql
sqlcmd -S LOCALHOST -E -i directsale.sql
sqlcmd -S LOCALHOST -E -i weeklyshare.sql

ECHO .
ECHO if no error message appear DB was created
PAUSE
