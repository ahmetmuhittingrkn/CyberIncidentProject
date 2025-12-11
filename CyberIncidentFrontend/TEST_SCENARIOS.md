# Test Scenarios - Cyber Incident WPF Frontend

## 🧪 Complete Testing Guide

This document provides step-by-step test scenarios to verify all features of the application.

---

## Prerequisites

Before testing:
- ✅ Backend API running on http://localhost:8080
- ✅ PostgreSQL database populated with test data
- ✅ At least 1 user in the database
- ✅ At least 3-5 test incidents created

---

## Test Scenario 1: Application Startup

### Steps:
1. Run `dotnet run` or execute `.\build.ps1`
2. Application window opens

### Expected Results:
- ✅ Main window displays with title "Cyber Incident Reporting & Analysis Platform"
- ✅ Sidebar visible with 3 menu items
- ✅ System status shows "Backend Connected" with green indicator
- ✅ Incident List view loads automatically
- ✅ Incidents display in DataGrid (if data exists)

### Pass Criteria:
- No errors on startup
- UI renders correctly
- Data loads from API

---

## Test Scenario 2: View Incident List

### Steps:
1. Navigate to "📊 Incident List" (if not already there)
2. Observe the DataGrid

### Expected Results:
- ✅ All incidents displayed in table
- ✅ Columns: ID, Title, Type, Severity, Status, Date, Reporter
- ✅ Severity colors displayed:
  - CRITICAL = Red & Bold
  - HIGH = Orange
  - MEDIUM = Yellow
  - LOW = Green
- ✅ Status colors displayed:
  - OPEN = Blue
  - IN_PROGRESS = Orange
  - RESOLVED = Green
  - CLOSED = Gray

### Pass Criteria:
- All data visible and properly formatted
- Colors match severity/status levels
- No missing or null values

---

## Test Scenario 3: Filter Incidents by Type

### Steps:
1. In Incident List view
2. Select "PHISHING" from Type dropdown
3. Click "Apply Filters"

### Expected Results:
- ✅ DataGrid updates to show only PHISHING incidents
- ✅ Other incident types are filtered out
- ✅ Count of displayed incidents matches filter

### Pass Criteria:
- Filter applies correctly
- Only matching incidents shown
- No errors or crashes

---

## Test Scenario 4: Filter by Severity

### Steps:
1. Clear previous filters
2. Select "CRITICAL" from Severity dropdown
3. Click "Apply Filters"

### Expected Results:
- ✅ Only CRITICAL severity incidents displayed
- ✅ All displayed incidents show red color
- ✅ Filter applies immediately

### Pass Criteria:
- Correct filtering
- UI updates properly

---

## Test Scenario 5: Date Range Filter

### Steps:
1. Clear previous filters
2. Select Start Date: 7 days ago
3. Select End Date: Today
4. Click "Apply Filters"

### Expected Results:
- ✅ Only incidents within date range displayed
- ✅ Incidents outside date range hidden
- ✅ Date format displays correctly (yyyy-MM-dd HH:mm)

### Pass Criteria:
- Date filtering works accurately
- No date parsing errors

---

## Test Scenario 6: Text Search

### Steps:
1. Clear all filters
2. Type "phishing" in Search box
3. Click "Apply Filters"

### Expected Results:
- ✅ Incidents with "phishing" in title or description shown
- ✅ Search is case-insensitive
- ✅ Partial matches work

### Pass Criteria:
- Search functionality works
- Results are relevant

---

## Test Scenario 7: Clear Filters

### Steps:
1. Apply multiple filters (type, severity, date)
2. Click "Clear Filters" button

### Expected Results:
- ✅ All filter dropdowns reset to null
- ✅ Search box clears
- ✅ Date pickers clear
- ✅ All incidents display again

### Pass Criteria:
- Filters clear completely
- Full data set reloads

---

## Test Scenario 8: View Incident Details

### Steps:
1. Select an incident in the DataGrid
2. Click "View Details" button

### Expected Results:
- ✅ Detail window opens as modal dialog
- ✅ All fields displayed:
  - Incident ID
  - Title
  - Description
  - Type
  - Severity (color-coded)
  - Status (color-coded)
  - Incident Date
  - Reporter Name
  - Created At
  - Updated At
  - Resolved At (or "Not resolved yet")
- ✅ Window title: "Incident Details"
- ✅ Close button works

### Pass Criteria:
- All data displays correctly
- Modal window functions properly
- Colors applied to severity/status

---

## Test Scenario 9: Create New Incident

### Steps:
1. Click "➕ Create Incident" in sidebar
2. Fill in form:
   - Title: "Test Security Incident"
   - Description: "This is a test incident for validation"
   - Type: "MALWARE"
   - Severity: "HIGH"
   - Date: Today
   - Reporter ID: 1
3. Click "Create Incident"

### Expected Results:
- ✅ Success message appears with new Incident ID
- ✅ Form clears after creation
- ✅ Can verify new incident in Incident List

### Pass Criteria:
- Incident created in database
- API call succeeds
- User receives confirmation

---

## Test Scenario 10: Form Validation

### Steps:
1. In Create Incident view
2. Leave Title empty
3. Try to click "Create Incident"

### Expected Results:
- ✅ Create button is disabled (or shows validation error)
- ✅ Cannot submit with empty required fields
- ✅ Form validates before submission

### Pass Criteria:
- Validation prevents invalid submissions
- User feedback provided

---

## Test Scenario 11: Clear Form

### Steps:
1. Fill in Create Incident form partially
2. Click "Clear Form" button

### Expected Results:
- ✅ All fields reset to defaults
- ✅ Title and Description clear
- ✅ Type returns to "PHISHING"
- ✅ Severity returns to "MEDIUM"
- ✅ Date resets to today

### Pass Criteria:
- Form clears completely
- Ready for new input

---

## Test Scenario 12: Update Incident Status

### Steps:
1. Go to Incident List
2. Select an incident with status "OPEN"
3. Click "Update Status ▼"
4. Select "IN_PROGRESS"

### Expected Results:
- ✅ Context menu appears with 4 options
- ✅ Click IN_PROGRESS
- ✅ Success message displays
- ✅ DataGrid refreshes
- ✅ Selected incident now shows "IN_PROGRESS" status
- ✅ Status color changes to Orange

### Pass Criteria:
- Status updates in database
- UI reflects change immediately
- Color updates correctly

---

## Test Scenario 13: Delete Incident

### Steps:
1. Select an incident in list
2. Click "Delete Incident" button
3. Confirm deletion in popup

### Expected Results:
- ✅ Confirmation dialog appears: "Are you sure?"
- ✅ Click "Yes"
- ✅ Incident deleted from database
- ✅ Success message: "Incident deleted successfully!"
- ✅ DataGrid refreshes
- ✅ Incident removed from list

### Pass Criteria:
- Deletion confirmed before executing
- Incident removed permanently
- UI updates correctly

---

## Test Scenario 14: Cancel Deletion

### Steps:
1. Select an incident
2. Click "Delete Incident"
3. Click "No" in confirmation dialog

### Expected Results:
- ✅ Dialog closes
- ✅ Incident NOT deleted
- ✅ Incident remains in list

### Pass Criteria:
- Cancel option works
- No unintended deletions

---

## Test Scenario 15: View Analytics Dashboard

### Steps:
1. Click "📈 Analytics Dashboard" in sidebar
2. Wait for data to load

### Expected Results:
- ✅ 4 summary cards display:
  - Total Incidents (blue)
  - Open Incidents (yellow)
  - Critical Incidents (red)
  - Resolved Incidents (green)
- ✅ Each card shows a number
- ✅ Numbers are accurate and match database

### Pass Criteria:
- All cards display
- Numbers calculated correctly
- No loading errors

---

## Test Scenario 16: Analytics - Incidents by Type

### Steps:
1. In Analytics Dashboard
2. View "Incidents by Type" table

### Expected Results:
- ✅ Table shows all incident types with counts
- ✅ Types listed: PHISHING, MALWARE, etc.
- ✅ Count column shows number of each type
- ✅ Counts are accurate

### Pass Criteria:
- Data displays in table format
- Counts match actual data

---

## Test Scenario 17: Analytics - Incidents by Severity

### Steps:
1. View "Incidents by Severity" table

### Expected Results:
- ✅ Table shows: LOW, MEDIUM, HIGH, CRITICAL
- ✅ Each severity has a count
- ✅ Colors applied:
  - CRITICAL = Red
  - HIGH = Orange
  - MEDIUM = Yellow
  - LOW = Green

### Pass Criteria:
- All severities listed
- Colors match severity levels
- Counts accurate

---

## Test Scenario 18: Analytics - Status Summary

### Steps:
1. View "Incidents by Status" section
2. Observe status cards

### Expected Results:
- ✅ Cards for each status: OPEN, IN_PROGRESS, RESOLVED, CLOSED
- ✅ Each card shows count
- ✅ Colors applied to numbers:
  - OPEN = Blue
  - IN_PROGRESS = Orange
  - RESOLVED = Green
  - CLOSED = Gray

### Pass Criteria:
- All statuses represented
- Visual layout is clean
- Data is accurate

---

## Test Scenario 19: Refresh Analytics Data

### Steps:
1. In Analytics Dashboard
2. Click "Refresh Data" button
3. Wait for reload

### Expected Results:
- ✅ Loading indicator appears briefly
- ✅ All statistics refresh
- ✅ Updated data displays
- ✅ No errors during refresh

### Pass Criteria:
- Refresh completes successfully
- Data updates to latest values

---

## Test Scenario 20: Navigation Between Views

### Steps:
1. Click "📊 Incident List"
2. Click "➕ Create Incident"
3. Click "📈 Analytics Dashboard"
4. Return to "📊 Incident List"

### Expected Results:
- ✅ Each click changes main content area
- ✅ Views load without delay
- ✅ No flash or flicker
- ✅ Previous data persists where appropriate
- ✅ Sidebar remains visible

### Pass Criteria:
- Smooth navigation
- No memory leaks
- Views render correctly

---

## Test Scenario 21: Error Handling - Backend Offline

### Steps:
1. Stop backend API server
2. In WPF app, click "Refresh" on Incident List

### Expected Results:
- ✅ Error message displays
- ✅ Message is user-friendly: "Error loading incidents: ..."
- ✅ Application doesn't crash
- ✅ User can continue using app

### Pass Criteria:
- Graceful error handling
- Clear error messages
- No application crash

---

## Test Scenario 22: Loading Indicators

### Steps:
1. Click "Refresh" button
2. Observe during API call

### Expected Results:
- ✅ Loading overlay appears
- ✅ Semi-transparent background
- ✅ "Loading..." text displayed
- ✅ UI blocks interactions during load
- ✅ Overlay disappears when complete

### Pass Criteria:
- Loading state is clear
- User cannot double-click
- UI feedback provided

---

## Test Scenario 23: Large Dataset Performance

### Steps:
1. Populate database with 100+ incidents
2. Load Incident List
3. Apply filters
4. Navigate between views

### Expected Results:
- ✅ List loads in reasonable time (< 3 seconds)
- ✅ Filtering is responsive
- ✅ No UI freezing
- ✅ Smooth scrolling

### Pass Criteria:
- Performance is acceptable
- No lag or freezing

---

## Test Scenario 24: Multiple Status Updates

### Steps:
1. Select incident (status: OPEN)
2. Update to IN_PROGRESS
3. Update same incident to RESOLVED
4. Update to CLOSED

### Expected Results:
- ✅ Each update succeeds
- ✅ UI updates after each change
- ✅ Final status is CLOSED
- ✅ Database reflects final state

### Pass Criteria:
- Multiple updates work
- Status persists correctly

---

## Test Scenario 25: Window Resize and Layout

### Steps:
1. Resize main window smaller
2. Resize window larger
3. Maximize window

### Expected Results:
- ✅ Layout adjusts responsively
- ✅ No content cut off
- ✅ Sidebar remains functional
- ✅ DataGrid adjusts width
- ✅ No visual glitches

### Pass Criteria:
- Responsive layout
- All content accessible

---

## 📊 Test Summary Template

After completing all tests, fill out:

```
Total Tests: 25
Passed: __
Failed: __
Blocked: __

Critical Issues Found: __
Minor Issues Found: __

Overall Status: [PASS / FAIL / PARTIAL]

Notes:
_________________________________
_________________________________
```

---

## 🐛 Bug Report Template

If you find issues, use this format:

```
Test Scenario: [Number and name]
Steps to Reproduce:
1. 
2. 
3. 

Expected Result:


Actual Result:


Severity: [Critical / High / Medium / Low]

Screenshots: [If applicable]
```

---

## ✅ Acceptance Criteria

Application is ready for deployment when:
- ✅ All 25 test scenarios pass
- ✅ No critical bugs
- ✅ Performance is acceptable
- ✅ Error handling works
- ✅ UI is responsive
- ✅ Data persists correctly

---

**Happy Testing! 🧪**

