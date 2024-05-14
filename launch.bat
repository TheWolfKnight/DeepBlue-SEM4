@echo off

call dotnet build

if %ERRORLEVEL% NEQ 0 GOTO FAILEDCOMPILE

call dotnet run --project src\DeepBlue.Aspire.AppHost

:FAILEDCOMPILE
echo "Failed to build project"
echo %ERRORLEVEL%
exit /B %ERRORLEVEL%
