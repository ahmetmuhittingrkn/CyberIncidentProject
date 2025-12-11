# Architecture Overview - Cyber Incident WPF Frontend

## 🏗️ Application Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                     WPF Application Layer                        │
│                   (CyberIncidentWPF.exe)                        │
└─────────────────────────────────────────────────────────────────┘
                              │
                              │
        ┌─────────────────────┼─────────────────────┐
        │                     │                     │
        ▼                     ▼                     ▼
┌──────────────┐    ┌──────────────┐    ┌──────────────┐
│   Views      │    │  ViewModels  │    │   Models     │
│   (XAML)     │◄───┤   (Logic)    │◄───┤   (Data)     │
└──────────────┘    └──────────────┘    └──────────────┘
                              │
                              │ Commands & Data Binding
                              │
                              ▼
                    ┌──────────────┐
                    │   Services   │
                    │  (ApiService)│
                    └──────────────┘
                              │
                              │ HTTP Requests
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │     Spring Boot Backend API             │
        │     http://localhost:8080/api           │
        └─────────────────────────────────────────┘
                              │
                              │ JDBC
                              │
                              ▼
                    ┌──────────────┐
                    │  PostgreSQL  │
                    │   Database   │
                    └──────────────┘
```

---

## 📦 MVVM Pattern Implementation

```
┌─────────────────────────────────────────────────────────────────┐
│                        View (XAML)                               │
│  ┌────────────────┐  ┌────────────────┐  ┌────────────────┐   │
│  │ IncidentList   │  │ CreateIncident │  │   Analytics    │   │
│  │     View       │  │      View      │  │     View       │   │
│  └────────────────┘  └────────────────┘  └────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
         │                       │                      │
         │ Data Binding          │ Data Binding         │ Data Binding
         │ Commands              │ Commands             │ Commands
         │                       │                      │
         ▼                       ▼                      ▼
┌─────────────────────────────────────────────────────────────────┐
│                      ViewModel Layer                             │
│  ┌────────────────┐  ┌────────────────┐  ┌────────────────┐   │
│  │ IncidentList   │  │ CreateIncident │  │   Analytics    │   │
│  │   ViewModel    │  │   ViewModel    │  │   ViewModel    │   │
│  │                │  │                │  │                │   │
│  │ • Properties   │  │ • Properties   │  │ • Properties   │   │
│  │ • Commands     │  │ • Commands     │  │ • Commands     │   │
│  │ • Logic        │  │ • Validation   │  │ • Calculation  │   │
│  └────────────────┘  └────────────────┘  └────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
         │                       │                      │
         │                       │                      │
         └───────────────────────┼──────────────────────┘
                                 │
                                 │ Uses
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │      ApiService         │
                    │                         │
                    │ • GetIncidentsAsync()   │
                    │ • CreateIncidentAsync() │
                    │ • UpdateStatusAsync()   │
                    │ • GetAnalyticsAsync()   │
                    │ • etc.                  │
                    └─────────────────────────┘
                                 │
                                 │ Maps to/from
                                 │
                                 ▼
┌─────────────────────────────────────────────────────────────────┐
│                        Model Layer                               │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐  ┌───────────┐│
│  │  Incident  │  │    User    │  │ Analytics  │  │ ApiResponse││
│  │            │  │            │  │   Data     │  │            ││
│  │ • IncidentId│ │ • UserId   │  │ • Stats    │  │ • Generic  ││
│  │ • Title    │  │ • Username │  │ • Counts   │  │   Wrapper  ││
│  │ • Severity │  │ • Email    │  │ • Charts   │  │            ││
│  └────────────┘  └────────────┘  └────────────┘  └───────────┘│
└─────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Data Flow Diagram

### Incident Creation Flow:

```
    User Action                  ViewModel                 Service                Backend
        │                            │                        │                      │
        │ 1. Fill Form              │                        │                      │
        ├───────────────────────────►│                        │                      │
        │                            │ 2. Validate Input      │                      │
        │                            │                        │                      │
        │ 3. Click "Create"         │                        │                      │
        ├───────────────────────────►│                        │                      │
        │                            │ 4. CreateCommand       │                      │
        │                            ├────────────────────────►│                      │
        │                            │                        │ 5. POST /incidents   │
        │                            │                        ├──────────────────────►│
        │                            │                        │                      │ 6. Save to DB
        │                            │                        │ 7. Return Created    │
        │                            │                        │◄──────────────────────┤
        │                            │ 8. Return Result       │                      │
        │                            │◄────────────────────────┤                      │
        │ 9. Show Success Message    │                        │                      │
        │◄───────────────────────────┤                        │                      │
        │                            │ 10. Clear Form         │                      │
        │                            │                        │                      │
```

### Incident List with Filters:

```
    User Action              ViewModel               Service              Backend
        │                        │                      │                    │
        │ 1. Select Filters     │                      │                    │
        ├───────────────────────►│                      │                    │
        │                        │ 2. Apply Filters    │                    │
        │                        │                      │                    │
        │ 3. Click Apply        │                      │                    │
        ├───────────────────────►│                      │                    │
        │                        │ 4. Build Query      │                    │
        │                        ├──────────────────────►│                    │
        │                        │                      │ 5. GET /incidents  │
        │                        │                      │    ?type=x&sev=y   │
        │                        │                      ├────────────────────►│
        │                        │                      │                    │ 6. Query DB
        │                        │                      │ 7. Return Filtered │
        │                        │                      │◄────────────────────┤
        │                        │ 8. Update Collection │                    │
        │                        │◄──────────────────────┤                    │
        │ 9. UI Refreshes       │                      │                    │
        │◄───────────────────────┤                      │                    │
```

---

## 🗂️ Folder Structure & Responsibilities

```
CyberIncidentWPF/
│
├── 📁 Models/                    ← Data Transfer Objects (DTOs)
│   ├── Incident.cs              → Incident entity model
│   ├── User.cs                  → User entity model
│   ├── IncidentType.cs          → Type reference model
│   ├── ApiResponse.cs           → Generic API response wrapper
│   └── AnalyticsData.cs         → Analytics models
│
├── 📁 ViewModels/               ← Business Logic & State
│   ├── MainViewModel.cs         → Navigation controller
│   ├── IncidentListViewModel.cs → List management & filtering
│   ├── CreateIncidentViewModel.cs → Form validation & creation
│   └── AnalyticsViewModel.cs    → Statistics & calculations
│
├── 📁 Views/                    ← User Interface (XAML)
│   ├── IncidentListView.xaml    → DataGrid with filters
│   ├── CreateIncidentView.xaml  → Creation form
│   ├── AnalyticsView.xaml       → Dashboard with cards
│   └── IncidentDetailWindow.xaml → Detail popup
│
├── 📁 Services/                 ← External Communication
│   └── ApiService.cs            → HTTP client for REST API
│
├── 📁 Helpers/                  ← Utilities & Infrastructure
│   ├── ObservableObject.cs      → INotifyPropertyChanged base
│   ├── RelayCommand.cs          → ICommand implementation
│   └── Converters.cs            → Value converters for binding
│
├── MainWindow.xaml              ← Application shell
├── App.xaml                     ← Global resources & config
└── CyberIncidentWPF.csproj      ← Project configuration
```

---

## 🔌 API Integration Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                      ApiService.cs                               │
│                                                                  │
│  ┌────────────────────────────────────────────────────────┐    │
│  │              HttpClient                                 │    │
│  │  • BaseAddress: http://localhost:8080/api              │    │
│  │  • Timeout: 30 seconds                                 │    │
│  │  • Headers: Content-Type: application/json             │    │
│  └────────────────────────────────────────────────────────┘    │
│                                                                  │
│  Methods:                                                        │
│  ┌──────────────────────────┐  ┌──────────────────────────┐   │
│  │   Incident Operations    │  │   Analytics Operations   │   │
│  ├──────────────────────────┤  ├──────────────────────────┤   │
│  │ • GetIncidentsAsync()    │  │ • GetTypeStatsAsync()    │   │
│  │ • GetIncidentByIdAsync() │  │ • GetSeverityStatsAsync()│   │
│  │ • CreateIncidentAsync()  │  │ • GetCriticalCountAsync()│   │
│  │ • UpdateIncidentAsync()  │  │ • GetStatusSummaryAsync()│   │
│  │ • UpdateStatusAsync()    │  │ • GetTimelineDataAsync() │   │
│  │ • DeleteIncidentAsync()  │  └──────────────────────────┘   │
│  └──────────────────────────┘                                   │
│                                                                  │
│  ┌──────────────────────────┐                                   │
│  │    User Operations       │                                   │
│  ├──────────────────────────┤                                   │
│  │ • GetUsersAsync()        │                                   │
│  │ • GetUserByIdAsync()     │                                   │
│  │ • CreateUserAsync()      │                                   │
│  └──────────────────────────┘                                   │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🎨 UI Component Hierarchy

```
MainWindow
│
├── Sidebar Navigation
│   ├── Logo/Title
│   ├── Menu Items
│   │   ├── 📊 Incident List
│   │   ├── ➕ Create Incident
│   │   └── 📈 Analytics Dashboard
│   ├── System Status
│   └── Version Info
│
└── Content Area (Dynamic)
    │
    ├── IncidentListView
    │   ├── Header
    │   ├── Filter Panel
    │   │   ├── Search Box
    │   │   ├── Type Filter
    │   │   ├── Severity Filter
    │   │   ├── Date Range
    │   │   └── Action Buttons
    │   ├── DataGrid
    │   │   └── 7 Columns
    │   └── Action Bar
    │       ├── View Details
    │       ├── Update Status
    │       └── Delete
    │
    ├── CreateIncidentView
    │   ├── Header
    │   └── Form
    │       ├── Title Input
    │       ├── Description Input
    │       ├── Type Dropdown
    │       ├── Severity Dropdown
    │       ├── Date Picker
    │       ├── Reporter ID Input
    │       ├── Type Descriptions
    │       └── Action Buttons
    │           ├── Create
    │           └── Clear
    │
    └── AnalyticsView
        ├── Header + Refresh
        ├── Summary Cards (4)
        │   ├── Total Incidents
        │   ├── Open Incidents
        │   ├── Critical Incidents
        │   └── Resolved Incidents
        └── Charts Grid
            ├── Incidents by Type (Table)
            ├── Incidents by Severity (Table)
            └── Status Summary (Cards)
```

---

## 🔒 Error Handling Strategy

```
┌─────────────────────────────────────────────────────────────┐
│                    Error Handling Flow                       │
└─────────────────────────────────────────────────────────────┘

API Call
   │
   ├──► Try Block
   │      │
   │      ├──► HttpClient Request
   │      │      │
   │      │      ├──► Success ──► Deserialize ──► Return Data
   │      │      │
   │      │      └──► HTTP Error ──► Catch Block
   │      │
   │      └──► Network Error ──► Catch Block
   │
   └──► Catch Block
          │
          ├──► Log Error Message
          │
          ├──► Create User-Friendly Message
          │
          └──► Throw New Exception
                 │
                 └──► ViewModel Catches
                        │
                        ├──► Show MessageBox
                        │
                        └──► Update UI State
```

**Error Types Handled:**
- ❌ Network Connection Errors
- ❌ HTTP Status Errors (4xx, 5xx)
- ❌ JSON Deserialization Errors
- ❌ Timeout Errors
- ❌ Null Reference Errors

---

## 📊 State Management

```
ViewModel State:
┌────────────────────────────────────────────────────┐
│  Property                    Notifies View         │
├────────────────────────────────────────────────────┤
│  • IsLoading                → Loading Overlay      │
│  • SelectedIncident         → Button Enable State  │
│  • Incidents Collection     → DataGrid Update      │
│  • Filter Properties        → Filter UI            │
│  • Validation Errors        → Error Messages       │
└────────────────────────────────────────────────────┘

Command State:
┌────────────────────────────────────────────────────┐
│  Command              Can Execute Condition        │
├────────────────────────────────────────────────────┤
│  • LoadIncidents      → Always                     │
│  • CreateIncident     → Valid Form                 │
│  • UpdateStatus       → Incident Selected          │
│  • DeleteIncident     → Incident Selected          │
│  • ViewDetails        → Incident Selected          │
└────────────────────────────────────────────────────┘
```

---

## 🔄 Asynchronous Operations

```
All API calls use async/await pattern:

public async Task<List<Incident>> GetIncidentsAsync()
{
    try
    {
        // Set loading state
        IsLoading = true;
        
        // Await API call
        var response = await _httpClient.GetAsync("/incidents");
        
        // Await deserialization
        var json = await response.Content.ReadAsStringAsync();
        var incidents = JsonSerializer.Deserialize<List<Incident>>(json);
        
        return incidents;
    }
    catch (Exception ex)
    {
        // Handle error
        throw new Exception($"Error: {ex.Message}", ex);
    }
    finally
    {
        // Always clear loading state
        IsLoading = false;
    }
}
```

**Benefits:**
- ✅ Non-blocking UI
- ✅ Responsive user experience
- ✅ Proper exception handling
- ✅ Loading indicators

---

## 🎯 Design Patterns Used

1. **MVVM Pattern**
   - Separation of concerns
   - Testable ViewModels
   - Data binding

2. **Command Pattern**
   - RelayCommand for user actions
   - CanExecute for validation

3. **Observer Pattern**
   - INotifyPropertyChanged
   - ObservableCollection

4. **Repository Pattern** (Backend)
   - ApiService as data access layer

5. **Async/Await Pattern**
   - All I/O operations
   - Non-blocking calls

---

## 🚀 Deployment Architecture

```
Development Environment:
┌─────────────────────────────────────────────────┐
│  Developer Machine                               │
│  ├── Visual Studio / Rider                      │
│  ├── .NET 6 SDK                                 │
│  └── WPF Application                            │
└─────────────────────────────────────────────────┘
         │
         │ HTTP
         │
         ▼
┌─────────────────────────────────────────────────┐
│  Backend Server (localhost:8080)                │
│  ├── Java 17+                                   │
│  ├── Spring Boot                                │
│  └── Tomcat                                     │
└─────────────────────────────────────────────────┘
         │
         │ JDBC
         │
         ▼
┌─────────────────────────────────────────────────┐
│  Database Server (localhost:5432)               │
│  ├── PostgreSQL 15+                             │
│  └── cyber_incident_db                          │
└─────────────────────────────────────────────────┘
```

---

## 📈 Performance Considerations

1. **Data Loading**
   - Async loading prevents UI freeze
   - Pagination ready (backend support needed)
   - Lazy loading for large datasets

2. **Memory Management**
   - ObservableCollections updated, not replaced
   - Proper disposal of HttpClient
   - No memory leaks in ViewModels

3. **UI Rendering**
   - DataGrid virtualization
   - Efficient data binding
   - Minimal code-behind

---

## 🔐 Security Considerations

1. **API Communication**
   - HTTPS ready (change BaseUrl)
   - Token authentication ready
   - Error messages don't expose sensitive data

2. **Input Validation**
   - Client-side validation
   - Server-side validation (backend)
   - SQL injection prevention (backend)

3. **Data Protection**
   - No passwords stored in frontend
   - Sensitive data not logged
   - Secure communication channel

---

This architecture provides a solid foundation for a maintainable, scalable, and performant WPF application! 🎉

