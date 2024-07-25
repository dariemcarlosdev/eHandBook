@echo off
REM Navigate to the project directory
cd /d "%~dp0"

REM Set environment variables
set ASPNETCORE_ENVIRONMENT=Development
set CUSTOM_ENV_VARIABLE=YourValue

REM Run the project with dotnet watch
dotnet watch run --launch-profile SelfHostLaunch