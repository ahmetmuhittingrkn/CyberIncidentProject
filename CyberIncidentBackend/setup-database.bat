@echo off
echo ========================================
echo Database Setup for Cyber Incident Platform
echo ========================================
echo.

set PGPASSWORD=2
set PGUSER=postgres
set PGHOST=localhost
set PGPORT=5432

echo Creating database...
psql -U %PGUSER% -h %PGHOST% -p %PGPORT% -c "CREATE DATABASE cyber_incident_db;" 2>nul
if %errorlevel% equ 0 (
    echo Database created successfully!
) else (
    echo Database might already exist or creation failed.
)
echo.

echo Running initialization script...
psql -U %PGUSER% -h %PGHOST% -p %PGPORT% -d cyber_incident_db -f database-init.sql

if %errorlevel% equ 0 (
    echo.
    echo ========================================
    echo Database setup completed successfully!
    echo ========================================
) else (
    echo.
    echo ========================================
    echo Database setup failed!
    echo Please check PostgreSQL is running and credentials are correct.
    echo ========================================
)

echo.
pause

