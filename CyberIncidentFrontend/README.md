# Cyber Incident Reporting & Analysis Platform - WPF Frontend

## Overview
This is a C# WPF (.NET 6) frontend application for managing cybersecurity incidents. It connects to a Java Spring Boot backend API and provides a modern, user-friendly interface for incident reporting, management, and analytics.

## Features

### 1. Incident Management
- **View All Incidents**: Browse incidents in a sortable, filterable DataGrid
- **Create New Incidents**: Easy-to-use form for reporting new security incidents
- **Update Status**: Change incident status (OPEN, IN_PROGRESS, RESOLVED, CLOSED)
- **View Details**: Detailed view of individual incidents
- **Delete Incidents**: Remove incidents from the system

### 2. Advanced Filtering
- Filter by incident type (Phishing, Malware, Data Breach, etc.)
- Filter by severity level (LOW, MEDIUM, HIGH, CRITICAL)
- Date range filtering
- Text search across titles and descriptions

### 3. Analytics Dashboard
- Real-time statistics and metrics
- Incidents by type breakdown
- Incidents by severity level
- Status summary with visual indicators
- Critical incident count
- Total, open, and resolved incident counts

### 4. Modern UI/UX
- **Premium Modern Design**: Beautiful gradient backgrounds, card-based layouts, and contemporary color palette
- **Smooth Animations**: Elegant hover effects, transitions, and loading animations
- **Material Design Principles**: Rounded corners, subtle shadows, and depth-based hierarchy
- **Professional Color Scheme**: 
  - Primary: Indigo (#6366F1)
  - Success: Emerald (#10B981)
  - Warning: Amber (#F59E0B)
  - Danger: Red (#EF4444)
- **Enhanced Typography**: Modern Segoe UI font with proper weight hierarchy
- **Interactive Elements**: Animated buttons with shadow effects and smooth state transitions
- **Visual Status Indicators**: Color-coded badges for severity levels and incident status
- **Intuitive Navigation**: Dark-themed sidebar with icon-enhanced menu items
- **Loading States**: Beautiful animated loading overlays
- **Responsive Layout**: Flexible grid system that adapts to different screen sizes
- **Professional Cards**: Shadow-enhanced cards with proper spacing and padding

## Project Structure

```
CyberIncidentWPF/
├── Models/                 # Data models
│   ├── Incident.cs
│   ├── User.cs
│   ├── IncidentType.cs
│   ├── ApiResponse.cs
│   └── AnalyticsData.cs
├── ViewModels/            # MVVM ViewModels
│   ├── MainViewModel.cs
│   ├── IncidentListViewModel.cs
│   ├── CreateIncidentViewModel.cs
│   └── AnalyticsViewModel.cs
├── Views/                 # XAML Views
│   ├── IncidentListView.xaml
│   ├── CreateIncidentView.xaml
│   ├── AnalyticsView.xaml
│   └── IncidentDetailWindow.xaml
├── Services/             # API Services
│   └── ApiService.cs
├── Helpers/              # Utility classes
│   ├── ObservableObject.cs
│   ├── RelayCommand.cs
│   └── Converters.cs
├── MainWindow.xaml       # Main application window
├── App.xaml              # Application resources
└── CyberIncidentWPF.csproj
```

## Prerequisites

- .NET 6 SDK or higher
- Visual Studio 2022 or Rider
- Backend API running on http://localhost:8080

## Installation

1. **Clone or extract the project**

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

## Configuration

### Backend API URL
The API base URL is configured in `Services/ApiService.cs`:
```csharp
private const string BaseUrl = "http://localhost:8080/api";
```

Change this to match your backend server location if needed.

## Usage

### Starting the Application
1. Ensure the backend API is running on port 8080
2. Launch the WPF application
3. The application will open with the Incident List view

### Creating an Incident
1. Click "➕ Create Incident" in the sidebar
2. Fill in the required fields:
   - Title
   - Description
   - Incident Type
   - Severity Level
   - Incident Date
   - Reporter ID
3. Click "Create Incident"

### Managing Incidents
1. Click "📊 Incident List" in the sidebar
2. Use filters to narrow down incidents
3. Select an incident to view details or update status
4. Use action buttons to:
   - View detailed information
   - Update status
   - Delete incidents

### Viewing Analytics
1. Click "📈 Analytics Dashboard" in the sidebar
2. View real-time statistics:
   - Total incidents
   - Open incidents
   - Critical incidents
   - Resolved incidents
3. Analyze incidents by type, severity, and status
4. Click "Refresh Data" to update statistics

## API Integration

### Endpoints Used

- `GET /api/incidents` - Get all incidents (with filters)
- `GET /api/incidents/{id}` - Get incident by ID
- `POST /api/incidents` - Create new incident
- `PUT /api/incidents/{id}` - Update incident
- `PATCH /api/incidents/{id}/status` - Update status
- `DELETE /api/incidents/{id}` - Delete incident
- `GET /api/analytics/incident-types` - Get type statistics
- `GET /api/analytics/severity-stats` - Get severity statistics
- `GET /api/analytics/status-summary` - Get status summary
- `GET /api/analytics/critical-count` - Get critical count

## Incident Types

- **PHISHING** - Phishing email or message attempts
- **UNAUTHORIZED_ACCESS** - Unauthorized system or data access
- **MALWARE** - Malware detection or infection
- **DATA_BREACH** - Data breach or leak
- **DOS_ATTACK** - Denial of Service attack
- **SOCIAL_ENGINEERING** - Social engineering attempt
- **RANSOMWARE** - Ransomware attack
- **INSIDER_THREAT** - Insider threat or suspicious activity
- **OTHER** - Other security incidents

## Severity Levels

- **LOW** - Minor incidents with minimal impact
- **MEDIUM** - Moderate incidents requiring attention
- **HIGH** - Serious incidents needing urgent response
- **CRITICAL** - Critical incidents requiring immediate action

## Status Values

- **OPEN** - Newly reported, not yet addressed
- **IN_PROGRESS** - Currently being investigated
- **RESOLVED** - Issue has been resolved
- **CLOSED** - Incident fully closed

## Troubleshooting

### Cannot Connect to Backend
- Verify backend is running: `http://localhost:8080/api`
- Check firewall settings
- Ensure CORS is configured in backend

### No Data Displayed
- Check backend database has data
- Verify API endpoints are returning data
- Check browser console for errors

### Build Errors
- Ensure .NET 6 SDK is installed
- Run `dotnet restore` to restore packages
- Clean and rebuild solution

## Development Team

- **Frontend Developer**: Muhammed Enes Gürkan (21118080030)
- **Backend Developer**: Ahmet Muhittin Gürkan (21118080059)
- **Integration Lead**: Salih Kırlıoğlu (21118080019)

## Technologies Used

- **Framework**: .NET 6 WPF
- **Architecture**: MVVM (Model-View-ViewModel)
- **HTTP Client**: HttpClient
- **JSON**: System.Text.Json
- **UI**: XAML with custom styles

## License

This project is developed for educational purposes.

## UI/UX Design Features

### Design Philosophy
The application follows modern design principles inspired by Material Design and contemporary web applications, featuring:

1. **Color System**
   - **Primary Colors**: Indigo and Purple gradients for main actions
   - **Semantic Colors**: Green (success), Yellow (warning), Red (danger), Blue (info)
   - **Neutral Palette**: Gray scale for text and backgrounds
   - **Gradient Backgrounds**: Smooth color transitions for visual depth

2. **Visual Hierarchy**
   - Large, bold headings (32px) with proper weight distribution
   - Descriptive subtitles in muted gray
   - Card-based content organization
   - Consistent spacing system (8px grid)

3. **Interactive Elements**
   - **Buttons**: Rounded corners (8-10px), shadow effects on hover
   - **Cards**: White background with subtle shadows, 12-16px border radius
   - **Inputs**: Clean borders with focus states, smooth transitions
   - **Data Tables**: Alternating row colors, hover states, rounded badges

4. **Animations**
   - Fade-in transitions for view changes
   - Hover shadow elevation
   - Rotating/scaling loading indicators
   - Smooth state changes

5. **Typography**
   - Font Family: Segoe UI (Windows native)
   - Heading Sizes: 32px (Main), 18px (Section), 13-14px (Body)
   - Font Weights: Bold (700), SemiBold (600), Medium (500), Regular (400)
   - Line Heights: Optimized for readability

6. **Layout Structure**
   - **Sidebar**: Dark gradient background (#1F2937 → #111827) with 280px width
   - **Main Content**: Light gray background (#F9FAFB) with 30px padding
   - **Cards**: White with shadow, proper internal padding (20-35px)
   - **Grid System**: Flexible columns with consistent gutters

### Component Styling

#### Navigation Sidebar
- Gradient dark background
- Circular icon badge with brand color and glow effect
- Menu items with hover states
- System status indicator with animated dot
- Clean footer with version info

#### Dashboard Cards
- Four-column stat cards with gradient backgrounds
- Large numbers (42px) for emphasis
- Icons for visual recognition
- Shadow effects for depth

#### Data Tables
- Clean headers with subtle background
- Hover effects on rows
- Colored badges for status and severity
- No visible grid lines for modern look
- Proper cell padding (12px)

#### Forms
- Large input fields with rounded corners
- Two-column layout for efficient space usage
- Info cards with gradient backgrounds
- Large, prominent submit buttons
- Clear visual feedback for all states

#### Loading States
- White modal overlay with shadow
- Centered content with animations
- Clear messaging and visual indicators

## Version

**Version 2.0** - December 2024 (Modern UI Update)
- Complete UI/UX redesign with modern design principles
- Enhanced visual hierarchy and color system
- Smooth animations and transitions
- Professional card-based layouts
- Improved user experience across all views

