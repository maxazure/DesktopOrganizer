@echo off
echo Starting Desktop Organizer...
echo.

cd /d "%~dp0DesktopOrganizer.UI\bin\Debug\net8.0-windows"

if not exist "DesktopOrganizer.UI.exe" (
    echo Error: DesktopOrganizer.UI.exe not found!
    echo Please build the project first.
    pause
    exit /b 1
)

echo Running application...
start "" "DesktopOrganizer.UI.exe"

echo Application started. Check for any error dialogs.
echo If no window appears, there may be a startup error.
echo.
pause