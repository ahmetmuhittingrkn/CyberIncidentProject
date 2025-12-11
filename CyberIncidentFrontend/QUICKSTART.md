# Quick Start Guide - Cyber Incident WPF Frontend

## 🚀 Get Started in 3 Steps

### Step 1: Prerequisites
Make sure you have:
- ✅ .NET 6 SDK or higher installed
- ✅ Backend API running on `http://localhost:8080`
- ✅ PostgreSQL database configured with sample data

### Step 2: Build and Run

**Option A: Using Command Line**
```bash
# Navigate to project directory
cd CyberIncidentFrontend

# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

**Option B: Using Visual Studio**
1. Open `CyberIncidentWPF.csproj` in Visual Studio 2022
2. Press `F5` or click "Start Debugging"

**Option C: Using Rider**
1. Open `CyberIncidentWPF.csproj` in JetBrains Rider
2. Click the "Run" button or press `Shift+F10`

### Step 3: Test the Application

1. **View Incidents**
   - Application opens with Incident List
   - You should see incidents from the database

2. **Create a Test Incident**
   - Click "➕ Create Incident" in sidebar
   - Fill in the form:
     - Title: "Test Phishing Email"
     - Description: "Suspicious email received from unknown sender"
     - Type: PHISHING
     - Severity: MEDIUM
     - Date: Today's date
     - Reporter ID: 1
   - Click "Create Incident"

3. **View Analytics**
   - Click "📈 Analytics Dashboard"
   - See statistics and charts

## 🔧 Configuration

### Change Backend URL
Edit `Services/ApiService.cs` line 14:
```csharp
private const string BaseUrl = "http://localhost:8080/api";
```

### Default Reporter ID
Edit `ViewModels/CreateIncidentViewModel.cs` line 18:
```csharp
private int _reporterId = 1; // Change to your user ID
```

## 🧪 Testing Features

### Test Filters
1. Go to Incident List
2. Select Type: "PHISHING"
3. Select Severity: "HIGH"
4. Click "Apply Filters"

### Test Status Update
1. Select an incident in the list
2. Click "Update Status ▼"
3. Choose "IN_PROGRESS"
4. Incident status updates immediately

### Test Details View
1. Select an incident
2. Click "View Details"
3. See full incident information

## ❗ Troubleshooting

### "Cannot connect to backend"
**Solution**: 
```bash
# Check if backend is running
curl http://localhost:8080/api/incidents
```

### "No incidents displayed"
**Solution**: Create sample data in database
```sql
-- Insert a test user
INSERT INTO users (username, email, full_name, role) 
VALUES ('testuser', 'test@example.com', 'Test User', 'USER');

-- Insert a test incident
INSERT INTO incidents (title, description, incident_type, severity_level, 
                       incident_date, reporter_id, status)
VALUES ('Test Incident', 'This is a test', 'PHISHING', 'MEDIUM', 
        NOW(), 1, 'OPEN');
```

### Build errors
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

## 📊 Sample Data

For testing, create users first:
```sql
INSERT INTO users (username, email, full_name, role) VALUES
('admin', 'admin@company.com', 'System Administrator', 'ADMIN'),
('jdoe', 'jdoe@company.com', 'John Doe', 'USER'),
('ssmith', 'ssmith@company.com', 'Sarah Smith', 'USER');
```

Then create incidents:
```sql
INSERT INTO incidents (title, description, incident_type, severity_level, 
                       incident_date, reporter_id, status) VALUES
('Phishing Email Detected', 'Suspicious email claiming to be from IT', 
 'PHISHING', 'MEDIUM', NOW(), 1, 'OPEN'),
('Unauthorized Access Attempt', 'Multiple failed login attempts detected', 
 'UNAUTHORIZED_ACCESS', 'HIGH', NOW(), 2, 'IN_PROGRESS'),
('Malware Found', 'Trojan detected on workstation', 
 'MALWARE', 'CRITICAL', NOW(), 3, 'OPEN');
```

## 🎯 Key Features to Test

1. ✅ Create new incident
2. ✅ View all incidents
3. ✅ Filter by type and severity
4. ✅ Search incidents
5. ✅ Update incident status
6. ✅ View incident details
7. ✅ Delete incident
8. ✅ View analytics dashboard
9. ✅ Refresh data

## 📱 UI Navigation

**Sidebar Menu:**
- 📊 Incident List - Main incident management
- ➕ Create Incident - Report new incidents
- 📈 Analytics Dashboard - View statistics

**Colors:**
- 🔴 Red = Critical severity
- 🟠 Orange = High severity
- 🟡 Yellow = Medium severity
- 🟢 Green = Low severity / Resolved

## 💡 Tips

1. **Refresh Data**: Click refresh button to reload from backend
2. **Quick Status Update**: Right-click incident → Update Status
3. **Keyboard Shortcuts**: Use Tab to navigate forms
4. **Clear Filters**: Use "Clear Filters" button to reset

## 🆘 Support

If you encounter issues:
1. Check backend logs
2. Verify database connection
3. Check API endpoint responses
4. Review README.md for detailed documentation

## 🎓 Learning Resources

- [WPF Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- [MVVM Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
- [REST API Best Practices](https://restfulapi.net/)

---

**Happy Testing! 🎉**

