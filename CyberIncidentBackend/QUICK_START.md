# 🚀 Quick Start Guide - Cyber Incident Backend

## Prerequisites Check ✅

Before starting, ensure you have:
- [ ] Java 17 or higher (`java -version`)
- [ ] Maven (`mvn -version`)
- [ ] PostgreSQL running (`pg_isready` or check services)
- [ ] PostgreSQL password set to: `2`

## 3-Step Quick Start

### Step 1: Setup Database (30 seconds)
```bash
setup-database.bat
```

This will:
- Create `cyber_incident_db` database
- Create all tables (users, incidents, incident_types)
- Insert sample data (5 users + 10 incidents)

### Step 2: Start Application (1 minute)
```bash
start.bat
```

This will:
- Build the Maven project
- Start Spring Boot application
- Launch API on http://localhost:8080

### Step 3: Test It Works (10 seconds)
Open a new terminal and run:
```bash
curl http://localhost:8080/api/users
```

You should see 5 users in JSON format. ✅

## Alternative: Manual Setup

### Manual Database Setup
```bash
# Open PostgreSQL command line
psql -U postgres

# Create database
CREATE DATABASE cyber_incident_db;

# Exit and run init script
\q
psql -U postgres -d cyber_incident_db -f database-init.sql
```

### Manual Application Start
```bash
# Build
mvn clean install

# Run
mvn spring-boot:run
```

## Quick Tests

After starting the application, try these:

**1. Get all incidents:**
```bash
curl http://localhost:8080/api/incidents
```

**2. Get analytics dashboard:**
```bash
curl http://localhost:8080/api/analytics/dashboard
```

**3. Create new incident:**
```bash
curl -X POST http://localhost:8080/api/incidents ^
  -H "Content-Type: application/json" ^
  -d "{\"title\":\"New Incident\",\"description\":\"Test\",\"incidentType\":\"PHISHING\",\"severityLevel\":\"MEDIUM\",\"incidentDate\":\"2024-12-11T10:00:00\",\"reporterId\":1}"
```

**4. Filter incidents by type:**
```bash
curl "http://localhost:8080/api/incidents?type=PHISHING"
```

## What You Get

After setup, you'll have:
- ✅ Running API server on port 8080
- ✅ 5 sample users in database
- ✅ 10 sample incidents in database
- ✅ 25+ working REST API endpoints
- ✅ Complete analytics system
- ✅ Ready for frontend integration

## API Endpoints Summary

### Users
- `GET /api/users` - List all users
- `POST /api/users` - Create user
- `GET /api/users/{id}` - Get user

### Incidents
- `GET /api/incidents` - List all (with filters)
- `POST /api/incidents` - Create incident
- `PATCH /api/incidents/{id}/status` - Update status
- `DELETE /api/incidents/{id}` - Delete

### Analytics
- `GET /api/analytics/dashboard` - Complete dashboard
- `GET /api/analytics/incident-types` - Count by type
- `GET /api/analytics/severity-stats` - Count by severity
- `GET /api/analytics/timeline?days=30` - Timeline

## Common Issues & Solutions

### "Cannot connect to database"
➡️ Check PostgreSQL is running:
```bash
# Windows
net start postgresql-x64-15
```

### "Port 8080 already in use"
➡️ Change port in `src/main/resources/application.properties`:
```properties
server.port=8081
```

### "Database does not exist"
➡️ Run setup script again:
```bash
setup-database.bat
```

### "Build failed"
➡️ Clean and rebuild:
```bash
mvn clean install -U
```

## Stopping the Application

Press `Ctrl+C` in the terminal where the application is running.

## Next Steps

1. ✅ Backend is ready - all 25+ endpoints working
2. 📋 Frontend team can start integration
3. 📋 Test endpoints using Postman (see `API_TESTING.md`)
4. 📋 Review `README.md` for detailed documentation

## Need More Help?

- **Full Documentation**: See `README.md`
- **API Testing**: See `API_TESTING.md`
- **Project Summary**: See `PROJECT_SUMMARY.md`

---

**Ready in 3 steps. Let's go! 🚀**

