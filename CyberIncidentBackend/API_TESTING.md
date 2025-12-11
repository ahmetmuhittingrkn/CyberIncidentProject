# API Testing Guide

## Base URL
```
http://localhost:8080/api
```

## User Endpoints

### 1. Create User
```bash
POST /api/users
Content-Type: application/json

{
  "username": "testuser",
  "email": "test@example.com",
  "fullName": "Test User",
  "role": "USER"
}
```

**cURL:**
```bash
curl -X POST http://localhost:8080/api/users \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","fullName":"Test User","role":"USER"}'
```

### 2. Get All Users
```bash
GET /api/users
```

**cURL:**
```bash
curl http://localhost:8080/api/users
```

### 3. Get User by ID
```bash
GET /api/users/1
```

**cURL:**
```bash
curl http://localhost:8080/api/users/1
```

### 4. Update User
```bash
PUT /api/users/1
Content-Type: application/json

{
  "username": "updateduser",
  "email": "updated@example.com",
  "fullName": "Updated User",
  "role": "ADMIN",
  "isActive": true
}
```

### 5. Delete User
```bash
DELETE /api/users/1
```

## Incident Endpoints

### 1. Create Incident
```bash
POST /api/incidents
Content-Type: application/json

{
  "title": "Suspicious Email Received",
  "description": "Received a phishing email claiming to be from IT department",
  "incidentType": "PHISHING",
  "severityLevel": "MEDIUM",
  "incidentDate": "2024-12-11T10:30:00",
  "reporterId": 1
}
```

**cURL:**
```bash
curl -X POST http://localhost:8080/api/incidents \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Suspicious Email Received",
    "description": "Received a phishing email claiming to be from IT department",
    "incidentType": "PHISHING",
    "severityLevel": "MEDIUM",
    "incidentDate": "2024-12-11T10:30:00",
    "reporterId": 1
  }'
```

### 2. Get All Incidents
```bash
GET /api/incidents
```

**cURL:**
```bash
curl http://localhost:8080/api/incidents
```

### 3. Get Incidents with Filters
```bash
GET /api/incidents?type=PHISHING&severity=MEDIUM&status=OPEN
```

**cURL:**
```bash
curl "http://localhost:8080/api/incidents?type=PHISHING&severity=MEDIUM&status=OPEN"
```

### 4. Get Incident by ID
```bash
GET /api/incidents/1
```

**cURL:**
```bash
curl http://localhost:8080/api/incidents/1
```

### 5. Update Incident
```bash
PUT /api/incidents/1
Content-Type: application/json

{
  "title": "Updated Title",
  "description": "Updated description",
  "incidentType": "PHISHING",
  "severityLevel": "HIGH",
  "incidentDate": "2024-12-11T10:30:00",
  "reporterId": 1,
  "status": "IN_PROGRESS"
}
```

### 6. Update Incident Status
```bash
PATCH /api/incidents/1/status
Content-Type: application/json

{
  "status": "RESOLVED"
}
```

**cURL:**
```bash
curl -X PATCH http://localhost:8080/api/incidents/1/status \
  -H "Content-Type: application/json" \
  -d '{"status":"RESOLVED"}'
```

### 7. Delete Incident
```bash
DELETE /api/incidents/1
```

**cURL:**
```bash
curl -X DELETE http://localhost:8080/api/incidents/1
```

### 8. Get Incidents by Reporter
```bash
GET /api/incidents/reporter/1
```

**cURL:**
```bash
curl http://localhost:8080/api/incidents/reporter/1
```

### 9. Get Total Incident Count
```bash
GET /api/incidents/count
```

**cURL:**
```bash
curl http://localhost:8080/api/incidents/count
```

### 10. Get Incident Count by Status
```bash
GET /api/incidents/count/OPEN
```

**cURL:**
```bash
curl http://localhost:8080/api/incidents/count/OPEN
```

## Analytics Endpoints

### 1. Get Incident Count by Type
```bash
GET /api/analytics/incident-types
```

**cURL:**
```bash
curl http://localhost:8080/api/analytics/incident-types
```

**Expected Response:**
```json
{
  "PHISHING": 5,
  "MALWARE": 3,
  "DATA_BREACH": 2,
  "UNAUTHORIZED_ACCESS": 4
}
```

### 2. Get Severity Statistics
```bash
GET /api/analytics/severity-stats
```

**cURL:**
```bash
curl http://localhost:8080/api/analytics/severity-stats
```

**Expected Response:**
```json
{
  "CRITICAL": 3,
  "HIGH": 4,
  "MEDIUM": 6,
  "LOW": 2
}
```

### 3. Get Status Statistics
```bash
GET /api/analytics/status-stats
```

**cURL:**
```bash
curl http://localhost:8080/api/analytics/status-stats
```

### 4. Get Critical Incident Count
```bash
GET /api/analytics/critical-count
```

**cURL:**
```bash
curl http://localhost:8080/api/analytics/critical-count
```

**Expected Response:**
```json
{
  "criticalCount": 3
}
```

### 5. Get Open Incident Count
```bash
GET /api/analytics/open-count
```

**cURL:**
```bash
curl http://localhost:8080/api/analytics/open-count
```

### 6. Get Resolved Incident Count
```bash
GET /api/analytics/resolved-count
```

**cURL:**
```bash
curl http://localhost:8080/api/analytics/resolved-count
```

### 7. Get Incident Timeline
```bash
GET /api/analytics/timeline?days=30
```

**cURL:**
```bash
curl "http://localhost:8080/api/analytics/timeline?days=30"
```

**Expected Response:**
```json
[
  {
    "date": "2024-12-11",
    "count": 5
  },
  {
    "date": "2024-12-10",
    "count": 3
  }
]
```

### 8. Get Status Summary
```bash
GET /api/analytics/status-summary
```

**cURL:**
```bash
curl http://localhost:8080/api/analytics/status-summary
```

**Expected Response:**
```json
{
  "total": 15,
  "open": 5,
  "inProgress": 3,
  "resolved": 4,
  "closed": 3
}
```

### 9. Get Top Reporters
```bash
GET /api/analytics/top-reporters?limit=10
```

**cURL:**
```bash
curl "http://localhost:8080/api/analytics/top-reporters?limit=10"
```

**Expected Response:**
```json
[
  {
    "user_id": 2,
    "full_name": "John Doe",
    "incident_count": 8
  },
  {
    "user_id": 3,
    "full_name": "Jane Smith",
    "incident_count": 5
  }
]
```

### 10. Get Complete Dashboard
```bash
GET /api/analytics/dashboard
```

**cURL:**
```bash
curl http://localhost:8080/api/analytics/dashboard
```

**Expected Response:**
```json
{
  "statusSummary": {
    "total": 15,
    "open": 5,
    "inProgress": 3,
    "resolved": 4,
    "closed": 3
  },
  "severityStats": {
    "CRITICAL": 3,
    "HIGH": 4,
    "MEDIUM": 6,
    "LOW": 2
  },
  "typeStats": {
    "PHISHING": 5,
    "MALWARE": 3,
    "DATA_BREACH": 2
  },
  "criticalCount": 3,
  "openCount": 5,
  "resolvedCount": 7
}
```

## Testing Workflow

### 1. Start the Application
```bash
mvn spring-boot:run
```

### 2. Verify Server is Running
```bash
curl http://localhost:8080/api/users
```

### 3. Create a Test User
```bash
curl -X POST http://localhost:8080/api/users \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@test.com","fullName":"Test User","role":"USER"}'
```

### 4. Create a Test Incident
```bash
curl -X POST http://localhost:8080/api/incidents \
  -H "Content-Type: application/json" \
  -d '{
    "title":"Test Incident",
    "description":"This is a test incident",
    "incidentType":"PHISHING",
    "severityLevel":"MEDIUM",
    "incidentDate":"2024-12-11T10:00:00",
    "reporterId":1
  }'
```

### 5. Get All Incidents
```bash
curl http://localhost:8080/api/incidents
```

### 6. Test Analytics
```bash
curl http://localhost:8080/api/analytics/dashboard
```

## Postman Collection

Import this JSON into Postman for easy testing:

### Environment Variables
- `base_url`: http://localhost:8080/api
- `user_id`: 1
- `incident_id`: 1

### Collection Structure
1. **Users**
   - Create User
   - Get All Users
   - Get User by ID
   - Update User
   - Delete User

2. **Incidents**
   - Create Incident
   - Get All Incidents
   - Get Incident by ID
   - Update Incident
   - Update Status
   - Delete Incident

3. **Analytics**
   - Dashboard
   - Type Stats
   - Severity Stats
   - Timeline

## Common Error Responses

### 400 Bad Request
```json
{
  "error": "Title is required"
}
```

### 404 Not Found
```json
{
  "error": "Incident not found with ID: 999"
}
```

### 500 Internal Server Error
```json
{
  "error": "Failed to create incident: Database connection failed"
}
```

## Valid Values Reference

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

