-- Cyber Incident Database Schema

-- Users Table
CREATE TABLE IF NOT EXISTS users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    full_name VARCHAR(200) NOT NULL,
    role VARCHAR(50) NOT NULL DEFAULT 'USER',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE
);

-- Incidents Table
CREATE TABLE IF NOT EXISTS incidents (
    incident_id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    incident_type VARCHAR(100) NOT NULL,
    severity_level VARCHAR(50) NOT NULL,
    incident_date TIMESTAMP NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'OPEN',
    reporter_id INTEGER NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    resolved_at TIMESTAMP,
    FOREIGN KEY (reporter_id) REFERENCES users(user_id) ON DELETE CASCADE
);

-- Incident Types Reference
CREATE TABLE IF NOT EXISTS incident_types (
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
('OTHER', 'Other security incidents')
ON CONFLICT (type_name) DO NOTHING;

-- Create indexes for better query performance
CREATE INDEX IF NOT EXISTS idx_incidents_type ON incidents(incident_type);
CREATE INDEX IF NOT EXISTS idx_incidents_severity ON incidents(severity_level);
CREATE INDEX IF NOT EXISTS idx_incidents_status ON incidents(status);
CREATE INDEX IF NOT EXISTS idx_incidents_date ON incidents(incident_date);
CREATE INDEX IF NOT EXISTS idx_incidents_reporter ON incidents(reporter_id);

-- Insert sample users
INSERT INTO users (username, email, full_name, role) VALUES
('admin', 'admin@cyberincident.com', 'System Administrator', 'ADMIN'),
('john.doe', 'john.doe@company.com', 'John Doe', 'USER'),
('jane.smith', 'jane.smith@company.com', 'Jane Smith', 'USER')
ON CONFLICT (username) DO NOTHING;

