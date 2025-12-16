-- Database Initialization Script for Cyber Incident Platform
-- Run this script to set up the database from scratch

-- Create database (run this as postgres superuser)
-- CREATE DATABASE cyber_incident_db;

-- Connect to the database
-- \c cyber_incident_db

-- Drop existing tables if they exist (use with caution!)
DROP TABLE IF EXISTS incidents CASCADE;
DROP TABLE IF EXISTS users CASCADE;
DROP TABLE IF EXISTS incident_types CASCADE;

-- Users Table
CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    full_name VARCHAR(200) NOT NULL,
    role VARCHAR(50) NOT NULL DEFAULT 'USER',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE
);

-- Incidents Table
CREATE TABLE incidents (
    incident_id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    incident_type VARCHAR(100) NOT NULL,
    severity_level VARCHAR(50) NOT NULL,
    incident_date TIMESTAMP NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'OPEN',
    reporter_id INTEGER NOT NULL,
    iocs TEXT,                               -- Bulunan IOC'ler (IP, domain, hash vb.)
    opened_by_analyst VARCHAR(200),          -- Incident'ı açan analist
    closed_by_analyst VARCHAR(200),          -- Incident'ı kapatan analist
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    resolved_at TIMESTAMP,
    FOREIGN KEY (reporter_id) REFERENCES users(user_id) ON DELETE CASCADE
);

-- Incident Types Reference
CREATE TABLE incident_types (
    type_id SERIAL PRIMARY KEY,
    type_name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT
);

-- Insert default incident types
INSERT INTO incident_types (type_name, description) VALUES
('PHISHING', 'Phishing email or message attempts'),
('UNAUTHORIZED_ACCESS', 'Unauthorized system or data access'),
('MALWARE', 'Malware detection or infection'),
('DATA_BREACH', 'Data breach or leak'),
('DOS_ATTACK', 'Denial of Service attack'),
('SOCIAL_ENGINEERING', 'Social engineering attempt'),
('RANSOMWARE', 'Ransomware attack'),
('INSIDER_THREAT', 'Insider threat or suspicious employee activity'),
('OTHER', 'Other security incidents');

-- Create indexes for better query performance
CREATE INDEX idx_incidents_type ON incidents(incident_type);
CREATE INDEX idx_incidents_severity ON incidents(severity_level);
CREATE INDEX idx_incidents_status ON incidents(status);
CREATE INDEX idx_incidents_date ON incidents(incident_date);
CREATE INDEX idx_incidents_reporter ON incidents(reporter_id);

-- Insert sample users
INSERT INTO users (username, email, full_name, role) VALUES
('admin', 'admin@cyberincident.com', 'System Administrator', 'ADMIN'),
('john.doe', 'john.doe@company.com', 'John Doe', 'USER'),
('jane.smith', 'jane.smith@company.com', 'Jane Smith', 'USER'),
('alice.wilson', 'alice.wilson@company.com', 'Alice Wilson', 'USER'),
('bob.johnson', 'bob.johnson@company.com', 'Bob Johnson', 'USER');

-- Insert sample incidents for testing
INSERT INTO incidents (title, description, incident_type, severity_level, incident_date, reporter_id, status) VALUES
('Suspicious Phishing Email', 'Received email claiming to be from IT asking for password reset', 'PHISHING', 'MEDIUM', '2024-12-10 09:30:00', 2, 'RESOLVED'),
('Unauthorized Login Attempt', 'Multiple failed login attempts from unknown IP address', 'UNAUTHORIZED_ACCESS', 'HIGH', '2024-12-10 14:15:00', 3, 'IN_PROGRESS'),
('Malware Detection on Workstation', 'Antivirus detected trojan on employee computer', 'MALWARE', 'CRITICAL', '2024-12-09 16:45:00', 4, 'RESOLVED'),
('Potential Data Breach', 'Customer data may have been accessed by unauthorized party', 'DATA_BREACH', 'CRITICAL', '2024-12-08 11:20:00', 2, 'IN_PROGRESS'),
('DoS Attack on Web Server', 'Web server experiencing unusual traffic patterns', 'DOS_ATTACK', 'HIGH', '2024-12-11 08:00:00', 5, 'OPEN'),
('Social Engineering Call', 'Employee received suspicious call requesting access credentials', 'SOCIAL_ENGINEERING', 'MEDIUM', '2024-12-11 10:30:00', 3, 'OPEN'),
('Ransomware Alert', 'Files on shared drive encrypted with ransom note', 'RANSOMWARE', 'CRITICAL', '2024-12-07 13:45:00', 4, 'RESOLVED'),
('Insider Threat Suspicion', 'Employee accessing files outside their authorization', 'INSIDER_THREAT', 'HIGH', '2024-12-11 11:00:00', 2, 'OPEN'),
('Unknown Security Event', 'Unusual network activity detected', 'OTHER', 'LOW', '2024-12-11 12:00:00', 5, 'OPEN'),
('Phishing Website Report', 'Discovered fake website mimicking company login page', 'PHISHING', 'HIGH', '2024-12-09 15:30:00', 3, 'RESOLVED');

-- Verify data
SELECT 'Users Count: ' || COUNT(*) FROM users;
SELECT 'Incidents Count: ' || COUNT(*) FROM incidents;
SELECT 'Incident Types Count: ' || COUNT(*) FROM incident_types;

-- Display sample data
SELECT * FROM users;
SELECT incident_id, title, incident_type, severity_level, status FROM incidents ORDER BY created_at DESC;

-- Success message
SELECT '✓ Database initialized successfully!' AS status;

