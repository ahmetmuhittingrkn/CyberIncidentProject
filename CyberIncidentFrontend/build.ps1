# Build Script for Cyber Incident WPF Frontend
# PowerShell script to build and run the application

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Cyber Incident WPF - Build Script" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Check if .NET 6 SDK is installed
Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: .NET SDK not found!" -ForegroundColor Red
    Write-Host "Please install .NET 6 SDK or higher from:" -ForegroundColor Red
    Write-Host "https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}
Write-Host "✓ .NET SDK $dotnetVersion found" -ForegroundColor Green
Write-Host ""

# Check if backend is running
Write-Host "Checking backend API..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:8080/api/incidents" -Method GET -TimeoutSec 2 -UseBasicParsing -ErrorAction SilentlyContinue
    Write-Host "✓ Backend API is running on http://localhost:8080" -ForegroundColor Green
} catch {
    Write-Host "⚠ WARNING: Cannot connect to backend API at http://localhost:8080" -ForegroundColor Yellow
    Write-Host "  Make sure the Java Spring Boot backend is running!" -ForegroundColor Yellow
    $continue = Read-Host "Continue anyway? (y/n)"
    if ($continue -ne "y") {
        exit 1
    }
}
Write-Host ""

# Clean previous build
Write-Host "Cleaning previous build..." -ForegroundColor Yellow
dotnet clean --nologo --verbosity quiet
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Clean completed" -ForegroundColor Green
} else {
    Write-Host "⚠ Clean failed (continuing anyway)" -ForegroundColor Yellow
}
Write-Host ""

# Restore NuGet packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore --nologo
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Package restore failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Packages restored" -ForegroundColor Green
Write-Host ""

# Build the project
Write-Host "Building project..." -ForegroundColor Yellow
dotnet build --nologo --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Build completed successfully" -ForegroundColor Green
Write-Host ""

# Ask if user wants to run the application
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
$run = Read-Host "Do you want to run the application now? (y/n)"

if ($run -eq "y") {
    Write-Host ""
    Write-Host "Starting application..." -ForegroundColor Yellow
    Write-Host ""
    dotnet run --no-build --configuration Release
} else {
    Write-Host ""
    Write-Host "To run the application later, use:" -ForegroundColor Cyan
    Write-Host "  dotnet run" -ForegroundColor White
    Write-Host ""
}

