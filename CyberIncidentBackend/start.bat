@echo off
echo ========================================
echo Cyber Incident Backend - Starting...
echo ========================================
echo.

echo Checking Java version...
java -version
echo.

echo Building project...
call mvn clean install -DskipTests
echo.

if %errorlevel% neq 0 (
    echo Build failed! Please check for errors.
    pause
    exit /b %errorlevel%
)

echo Build successful!
echo.
echo Starting Spring Boot application...
echo API will be available at: http://localhost:8080/api
echo.
call mvn spring-boot:run

pause

