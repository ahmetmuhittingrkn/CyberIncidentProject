# Cyber Incident Reporting & Analysis Platform - Backend

## Overview
Java Spring Boot backend for the Cyber Incident Management System with PostgreSQL database and manual SQL queries.

## Technology Stack
- **Java**: 17
- **Spring Boot**: 3.2.0
- **Database**: PostgreSQL 15+
- **Build Tool**: Maven
- **Database Access**: JdbcTemplate (Manual SQL)

## Project Structure
```
src/main/java/com/cyberincident/
├── CyberIncidentApplication.java      # Main application class
├── config/
│   ├── DatabaseConfig.java            # Database configuration
│   └── CorsConfig.java                # CORS configuration
├── controller/
│   ├── IncidentController.java        # Incident REST endpoints
│   ├── UserController.java            # User REST endpoints
│   └── AnalyticsController.java       # Analytics REST endpoints
├── model/
│   ├── Incident.java                  # Incident model
│   ├── User.java                      # User model
│   └── IncidentType.java              # Incident type model
├── service/
│   ├── IncidentService.java           # Incident business logic
│   ├── UserService.java               # User business logic
│   └── AnalyticsService.java          # Analytics business logic
└── repository/
    ├── IncidentRepository.java        # Incident data access (JdbcTemplate)
    ├── UserRepository.java            # User data access (JdbcTemplate)
    └── AnalyticsRepository.java       # Analytics data access (JdbcTemplate)
```

## Prerequisites
1. **Java 17 or higher** installed
2. **Maven** installed
3. **PostgreSQL 15+** installed and running
4. Database created: `cyber_incident_db`

## Database Setup

### Step 1: Create Database
```sql
CREATE DATABASE cyber_incident_db;
```

### Step 2: Run Schema Script
Connect to the database and run the SQL script from `src/main/resources/schema.sql`

Or manually:
```bash
psql -U postgres -d cyber_incident_db -f src/main/resources/schema.sql
```

### Step 3: Verify Tables
```sql
\dt  -- List all tables
-- Should show: users, incidents, incident_types
```

## Configuration

Edit `src/main/resources/application.properties`:
```properties
spring.datasource.url=jdbc:postgresql://localhost:5432/cyber_incident_db
spring.datasource.username=postgres
spring.datasource.password=2
```

## Build & Run

### Build Project
```bash
mvn clean install
```

### Run Application
```bash
mvn spring-boot:run
```

Or run the JAR:
```bash
java -jar target/cyber-incident-backend-1.0.0.jar
```

The API will start on **http://localhost:8080**

## API Endpoints

### User Endpoints
- `POST /api/users` - Create new user
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/username/{username}` - Get user by username
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### Incident Endpoints
- `POST /api/incidents` - Create new incident
- `GET /api/incidents` - Get all incidents (with filters)
  - Query params: `type`, `severity`, `startDate`, `endDate`, `status`
- `GET /api/incidents/{id}` - Get incident by ID
- `GET /api/incidents/reporter/{reporterId}` - Get incidents by reporter
- `PUT /api/incidents/{id}` - Update incident
- `PATCH /api/incidents/{id}/status` - Update incident status
- `DELETE /api/incidents/{id}` - Delete incident
- `GET /api/incidents/count` - Get total incident count
- `GET /api/incidents/count/{status}` - Get incident count by status

### Analytics Endpoints
- `GET /api/analytics/incident-types` - Get incident count by type
- `GET /api/analytics/severity-stats` - Get incident count by severity
- `GET /api/analytics/status-stats` - Get incident count by status
- `GET /api/analytics/critical-count` - Get critical incident count
- `GET /api/analytics/open-count` - Get open incident count
- `GET /api/analytics/resolved-count` - Get resolved incident count
- `GET /api/analytics/timeline?days=30` - Get incidents over time
- `GET /api/analytics/status-summary` - Get status summary
- `GET /api/analytics/top-reporters?limit=10` - Get top reporters
- `GET /api/analytics/dashboard` - Get complete dashboard data

## Testing with Postman/curl

### Create User
```bash
curl -X POST http://localhost:8080/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "fullName": "Test User",
    "role": "USER"
  }'
```

### Create Incident
```bash
curl -X POST http://localhost:8080/api/incidents \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Suspicious Email",
    "description": "Received phishing email",
    "incidentType": "PHISHING",
    "severityLevel": "MEDIUM",
    "incidentDate": "2024-12-11T10:30:00",
    "reporterId": 1
  }'
```

### Get All Incidents
```bash
curl http://localhost:8080/api/incidents
```

### Get Analytics
```bash
curl http://localhost:8080/api/analytics/dashboard
```

## Valid Values

### Incident Types
- PHISHING
- UNAUTHORIZED_ACCESS
- MALWARE
- DATA_BREACH
- DOS_ATTACK
- SOCIAL_ENGINEERING
- RANSOMWARE
- INSIDER_THREAT
- OTHER

### Severity Levels
- LOW
- MEDIUM
- HIGH
- CRITICAL

### Status Values
- OPEN
- IN_PROGRESS
- RESOLVED
- CLOSED

### User Roles
- USER
- ADMIN

## Troubleshooting

### Database Connection Error
1. Ensure PostgreSQL is running
2. Verify database exists: `cyber_incident_db`
3. Check username/password in `application.properties`
4. Verify PostgreSQL is listening on port 5432

### Build Errors
```bash
mvn clean install -U
```

### Port Already in Use
Change port in `application.properties`:
```properties
server.port=8081
```

## Development Team
- **Backend Developer**: Ahmet Muhittin Gürkan (21118080059)
- **Frontend Developer**: Muhammed Enes Gürkan (21118080030)
- **Integration Lead**: Salih Kırlıoğlu (21118080019)

## License
Educational Project - December 2024

