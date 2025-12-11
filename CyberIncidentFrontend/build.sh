#!/bin/bash
# Build Script for Cyber Incident WPF Frontend
# Bash script for Linux/Mac (if using .NET on these platforms)

echo "================================================"
echo "  Cyber Incident WPF - Build Script"
echo "================================================"
echo ""

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Check if .NET SDK is installed
echo -e "${YELLOW}Checking .NET SDK...${NC}"
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}ERROR: .NET SDK not found!${NC}"
    echo -e "${RED}Please install .NET 6 SDK or higher from:${NC}"
    echo -e "${RED}https://dotnet.microsoft.com/download${NC}"
    exit 1
fi
DOTNET_VERSION=$(dotnet --version)
echo -e "${GREEN}✓ .NET SDK $DOTNET_VERSION found${NC}"
echo ""

# Check if backend is running
echo -e "${YELLOW}Checking backend API...${NC}"
if curl -s -f "http://localhost:8080/api/incidents" > /dev/null 2>&1; then
    echo -e "${GREEN}✓ Backend API is running on http://localhost:8080${NC}"
else
    echo -e "${YELLOW}⚠ WARNING: Cannot connect to backend API at http://localhost:8080${NC}"
    echo -e "${YELLOW}  Make sure the Java Spring Boot backend is running!${NC}"
    read -p "Continue anyway? (y/n) " -n 1 -r
    echo ""
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi
echo ""

# Clean previous build
echo -e "${YELLOW}Cleaning previous build...${NC}"
dotnet clean --nologo --verbosity quiet
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Clean completed${NC}"
else
    echo -e "${YELLOW}⚠ Clean failed (continuing anyway)${NC}"
fi
echo ""

# Restore NuGet packages
echo -e "${YELLOW}Restoring NuGet packages...${NC}"
dotnet restore --nologo
if [ $? -ne 0 ]; then
    echo -e "${RED}ERROR: Package restore failed!${NC}"
    exit 1
fi
echo -e "${GREEN}✓ Packages restored${NC}"
echo ""

# Build the project
echo -e "${YELLOW}Building project...${NC}"
dotnet build --nologo --configuration Release
if [ $? -ne 0 ]; then
    echo -e "${RED}ERROR: Build failed!${NC}"
    exit 1
fi
echo -e "${GREEN}✓ Build completed successfully${NC}"
echo ""

# Ask if user wants to run the application
echo "================================================"
echo -e "${GREEN}Build completed successfully!${NC}"
echo "================================================"
echo ""
read -p "Do you want to run the application now? (y/n) " -n 1 -r
echo ""

if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo ""
    echo -e "${YELLOW}Starting application...${NC}"
    echo ""
    dotnet run --no-build --configuration Release
else
    echo ""
    echo -e "${CYAN}To run the application later, use:${NC}"
    echo "  dotnet run"
    echo ""
fi

