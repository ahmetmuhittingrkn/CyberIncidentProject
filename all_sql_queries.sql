-- =============================================
-- CYBER INCIDENT PROJECT - TÜM SQL SORGULARI
-- =============================================


-- =============================================
-- TABLO SİLME KOMUTLARI
-- database-init.sql dosyasında kullanılıyor
-- Veritabanını sıfırdan oluşturmak için tablolar siliniyor
-- =============================================

DROP TABLE IF EXISTS incidents CASCADE;  -- incidents tablosunu sil
DROP TABLE IF EXISTS users CASCADE;      -- users tablosunu sil  
DROP TABLE IF EXISTS incident_types CASCADE;  -- incident_types tablosunu sil


-- =============================================
-- USERS TABLOSU OLUŞTURMA
-- Sistemdeki kullanıcıları tutan tablo
-- schema.sql ve database-init.sql dosyalarında var
-- =============================================

CREATE TABLE IF NOT EXISTS users (
    user_id SERIAL PRIMARY KEY,           -- kullanıcı id (otomatik artan)
    username VARCHAR(100) NOT NULL UNIQUE, -- kullanıcı adı (benzersiz)
    email VARCHAR(150) NOT NULL UNIQUE,    -- email adresi (benzersiz)
    full_name VARCHAR(200) NOT NULL,       -- tam isim
    role VARCHAR(50) NOT NULL DEFAULT 'USER',  -- rol (USER veya ADMIN)
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- oluşturulma tarihi
    is_active BOOLEAN DEFAULT TRUE         -- aktif mi değil mi
);


-- =============================================
-- INCIDENTS TABLOSU OLUŞTURMA  
-- Siber güvenlik olaylarını tutan ana tablo
-- database-init.sql dosyasında var
-- =============================================

CREATE TABLE incidents (
    incident_id SERIAL PRIMARY KEY,          -- olay id (otomatik artan)
    title VARCHAR(255) NOT NULL,             -- olay başlığı
    description TEXT NOT NULL,               -- olay açıklaması
    incident_type VARCHAR(100) NOT NULL,     -- olay tipi (PHISHING, MALWARE vs.)
    severity_level VARCHAR(50) NOT NULL,     -- önem seviyesi (LOW, MEDIUM, HIGH, CRITICAL)
    incident_date TIMESTAMP NOT NULL,        -- olayın gerçekleştiği tarih
    status VARCHAR(50) NOT NULL DEFAULT 'OPEN',  -- durum (OPEN, IN_PROGRESS, RESOLVED, CLOSED)
    reporter_id INTEGER NOT NULL,            -- bildiren kullanıcı id (users tablosuyla ilişkili)
    iocs TEXT,                               -- bulunan IOC'ler (IP, domain, hash vb.)
    opened_by_analyst VARCHAR(200),          -- olayı açan analist ismi
    closed_by_analyst VARCHAR(200),          -- olayı kapatan analist ismi
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- kayıt oluşturulma tarihi
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- son güncelleme tarihi
    resolved_at TIMESTAMP,                   -- çözülme tarihi
    FOREIGN KEY (reporter_id) REFERENCES users(user_id) ON DELETE CASCADE  -- foreign key
);


-- =============================================
-- INCIDENT_TYPES TABLOSU OLUŞTURMA
-- Olay tiplerini tutan referans tablo
-- schema.sql ve database-init.sql dosyalarında var
-- =============================================

CREATE TABLE IF NOT EXISTS incident_types (
    type_id SERIAL PRIMARY KEY,              -- tip id (otomatik artan)
    type_name VARCHAR(100) NOT NULL UNIQUE,  -- tip adı (benzersiz)
    description TEXT                         -- açıklama
);


-- =============================================
-- İNDEX OLUŞTURMA KOMUTLARI
-- Sorgu performansını artırmak için indexler
-- schema.sql ve database-init.sql dosyalarında var
-- =============================================

CREATE INDEX idx_incidents_type ON incidents(incident_type);      -- olay tipine göre index
CREATE INDEX idx_incidents_severity ON incidents(severity_level); -- önem seviyesine göre index
CREATE INDEX idx_incidents_status ON incidents(status);           -- duruma göre index
CREATE INDEX idx_incidents_date ON incidents(incident_date);      -- tarihe göre index
CREATE INDEX idx_incidents_reporter ON incidents(reporter_id);    -- bildiren kişiye göre index


-- =============================================
-- INCIDENT_TYPES TABLOSUNA VERİ EKLEME
-- Varsayılan olay tiplerini ekliyor
-- schema.sql ve database-init.sql dosyalarında var
-- =============================================

INSERT INTO incident_types (type_name, description) VALUES
('PHISHING', 'Phishing email or message attempts'),           -- oltalama saldırısı
('UNAUTHORIZED_ACCESS', 'Unauthorized system or data access'), -- yetkisiz erişim
('MALWARE', 'Malware detection or infection'),                 -- zararlı yazılım
('DATA_BREACH', 'Data breach or leak'),                        -- veri sızıntısı
('DOS_ATTACK', 'Denial of Service attack'),                    -- dos saldırısı
('SOCIAL_ENGINEERING', 'Social engineering attempt'),          -- sosyal mühendislik
('RANSOMWARE', 'Ransomware attack'),                           -- fidye yazılımı
('INSIDER_THREAT', 'Insider threat or suspicious employee activity'),  -- içeriden tehdit
('OTHER', 'Other security incidents')                          -- diğer olaylar
ON CONFLICT (type_name) DO NOTHING;  -- eğer zaten varsa ekleme yapma


-- =============================================
-- USERS TABLOSUNA ÖRNEK VERİ EKLEME
-- Test için örnek kullanıcılar ekleniyor
-- database-init.sql dosyasında var
-- =============================================

INSERT INTO users (username, email, full_name, role) VALUES
('admin', 'admin@cyberincident.com', 'System Administrator', 'ADMIN'),
('john.doe', 'john.doe@company.com', 'John Doe', 'USER'),
('jane.smith', 'jane.smith@company.com', 'Jane Smith', 'USER'),
('alice.wilson', 'alice.wilson@company.com', 'Alice Wilson', 'USER'),
('bob.johnson', 'bob.johnson@company.com', 'Bob Johnson', 'USER')
ON CONFLICT (username) DO NOTHING;


-- =============================================
-- INCIDENTS TABLOSUNA ÖRNEK VERİ EKLEME
-- Test için örnek olaylar ekleniyor
-- database-init.sql dosyasında var
-- =============================================

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


-- =============================================
-- USER İŞLEMLERİ (UserRepository.java)
-- Backend'de kullanıcı işlemleri için kullanılan sorgular
-- =============================================

-- Yeni kullanıcı ekleme - create() metodunda kullanılıyor
INSERT INTO users (username, email, full_name, role, is_active) 
VALUES (?, ?, ?, ?, ?) RETURNING user_id;

-- Tüm kullanıcıları getirme - findAll() metodunda kullanılıyor
SELECT * FROM users ORDER BY created_at DESC;

-- ID'ye göre kullanıcı getirme - findById() metodunda kullanılıyor
SELECT * FROM users WHERE user_id = ?;

-- Kullanıcı adına göre arama - findByUsername() metodunda kullanılıyor
SELECT * FROM users WHERE username = ?;

-- Email'e göre arama - findByEmail() metodunda kullanılıyor
SELECT * FROM users WHERE email = ?;

-- Kullanıcı güncelleme - update() metodunda kullanılıyor
UPDATE users SET username = ?, email = ?, full_name = ?, role = ?, is_active = ? 
WHERE user_id = ?;

-- Kullanıcı silme - delete() metodunda kullanılıyor
DELETE FROM users WHERE user_id = ?;

-- Kullanıcı adı var mı kontrol - existsByUsername() metodunda kullanılıyor
SELECT COUNT(*) FROM users WHERE username = ?;

-- Email var mı kontrol - existsByEmail() metodunda kullanılıyor
SELECT COUNT(*) FROM users WHERE email = ?;


-- =============================================
-- INCIDENT İŞLEMLERİ (IncidentRepository.java)
-- Backend'de olay işlemleri için kullanılan sorgular
-- =============================================

-- Yeni olay ekleme - create() metodunda kullanılıyor
INSERT INTO incidents (title, description, incident_type, severity_level, 
                       incident_date, reporter_id, status, iocs, opened_by_analyst) 
VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?) RETURNING incident_id;

-- Tüm olayları getirme (kullanıcı ismiyle birlikte) - findAll() metodunda kullanılıyor
-- Dinamik olarak filtreler ekleniyor (tip, seviye, tarih, durum)
SELECT i.*, u.full_name as reporter_name 
FROM incidents i 
JOIN users u ON i.reporter_id = u.user_id 
WHERE 1=1
ORDER BY i.created_at DESC;

-- Tipe göre filtreleme (dinamik ekleniyor)
-- AND i.incident_type = ?

-- Önem seviyesine göre filtreleme (dinamik ekleniyor)
-- AND i.severity_level = ?

-- Başlangıç tarihine göre filtreleme (dinamik ekleniyor)
-- AND i.incident_date >= ?

-- Bitiş tarihine göre filtreleme (dinamik ekleniyor)
-- AND i.incident_date <= ?

-- Duruma göre filtreleme (dinamik ekleniyor)
-- AND i.status = ?

-- ID'ye göre olay getirme - findById() metodunda kullanılıyor
SELECT i.*, u.full_name as reporter_name 
FROM incidents i 
JOIN users u ON i.reporter_id = u.user_id 
WHERE i.incident_id = ?;

-- Olay güncelleme - update() metodunda kullanılıyor
UPDATE incidents SET title = ?, description = ?, incident_type = ?, 
                     severity_level = ?, incident_date = ?, status = ?, iocs = ?, 
                     reporter_id = ?, updated_at = CURRENT_TIMESTAMP 
WHERE incident_id = ?;

-- Durum güncelleme (normal) - updateStatus() metodunda kullanılıyor
UPDATE incidents SET status = ?, updated_at = CURRENT_TIMESTAMP 
WHERE incident_id = ?;

-- Durum güncelleme (RESOLVED veya CLOSED için) - updateStatus() metodunda kullanılıyor
UPDATE incidents SET status = ?, updated_at = CURRENT_TIMESTAMP, 
                     resolved_at = CURRENT_TIMESTAMP, closed_by_analyst = ? 
WHERE incident_id = ?;

-- Olay silme - delete() metodunda kullanılıyor
DELETE FROM incidents WHERE incident_id = ?;

-- Bildiren kişiye göre olayları getirme - findByReporterId() metodunda kullanılıyor
SELECT i.*, u.full_name as reporter_name 
FROM incidents i 
JOIN users u ON i.reporter_id = u.user_id 
WHERE i.reporter_id = ? ORDER BY i.created_at DESC;

-- Duruma göre olay sayısı - countByStatus() metodunda kullanılıyor
SELECT COUNT(*) FROM incidents WHERE status = ?;

-- Toplam olay sayısı - countAll() metodunda kullanılıyor
SELECT COUNT(*) FROM incidents;


-- =============================================
-- ANALİTİK İŞLEMLERİ (AnalyticsRepository.java)
-- Dashboard ve istatistik sorguları
-- =============================================

-- Olay tiplerine göre sayı - getIncidentCountByType() metodunda kullanılıyor
-- Kaç tane PHISHING, MALWARE vs. var onu döndürüyor
SELECT incident_type, COUNT(*) as count FROM incidents 
GROUP BY incident_type ORDER BY count DESC;

-- Önem seviyelerine göre sayı - getIncidentCountBySeverity() metodunda kullanılıyor
-- CRITICAL, HIGH, MEDIUM, LOW kaçar tane var
SELECT severity_level, COUNT(*) as count FROM incidents 
GROUP BY severity_level ORDER BY 
CASE severity_level 
    WHEN 'CRITICAL' THEN 1 
    WHEN 'HIGH' THEN 2 
    WHEN 'MEDIUM' THEN 3 
    WHEN 'LOW' THEN 4 
END;

-- Durumlara göre sayı - getIncidentCountByStatus() metodunda kullanılıyor
-- OPEN, IN_PROGRESS, RESOLVED, CLOSED kaçar tane var
SELECT status, COUNT(*) as count FROM incidents 
GROUP BY status ORDER BY count DESC;

-- Kritik olay sayısı - getCriticalIncidentCount() metodunda kullanılıyor
-- CRITICAL seviyeli ve henüz kapatılmamış olayların sayısı
SELECT COUNT(*) as critical_count FROM incidents 
WHERE severity_level = 'CRITICAL' AND status != 'CLOSED';

-- Açık olay sayısı - getOpenIncidentCount() metodunda kullanılıyor
-- Durumu OPEN olan olayların sayısı
SELECT COUNT(*) FROM incidents WHERE status = 'OPEN';

-- Çözülmüş olay sayısı - getResolvedIncidentCount() metodunda kullanılıyor
-- RESOLVED veya CLOSED olan olayların sayısı
SELECT COUNT(*) FROM incidents WHERE status IN ('RESOLVED', 'CLOSED');

-- Olay zaman çizelgesi - getIncidentTimeline() metodunda kullanılıyor
-- Son X gündeki olayların günlük dağılımı (grafik için)
SELECT DATE(incident_date) as date, COUNT(*) as count 
FROM incidents 
WHERE incident_date >= CURRENT_DATE - INTERVAL '30 days'
GROUP BY DATE(incident_date) 
ORDER BY date DESC;

-- Durum özeti - getStatusSummary() metodunda kullanılıyor
-- Tüm durumların toplam sayısını tek sorguda getiriyor
SELECT 
    COUNT(*) as total, 
    SUM(CASE WHEN status = 'OPEN' THEN 1 ELSE 0 END) as open, 
    SUM(CASE WHEN status = 'IN_PROGRESS' THEN 1 ELSE 0 END) as in_progress, 
    SUM(CASE WHEN status = 'RESOLVED' THEN 1 ELSE 0 END) as resolved, 
    SUM(CASE WHEN status = 'CLOSED' THEN 1 ELSE 0 END) as closed 
FROM incidents;

-- En çok bildirim yapan kullanıcılar - getTopReporters() metodunda kullanılıyor
-- Hangi kullanıcı kaç olay bildirmiş sıralaması
SELECT u.user_id, u.full_name, COUNT(i.incident_id) as incident_count 
FROM users u 
LEFT JOIN incidents i ON u.user_id = i.reporter_id 
GROUP BY u.user_id, u.full_name 
ORDER BY incident_count DESC 
LIMIT ?;


-- =============================================
-- VERİTABANI DOĞRULAMA SORGULARI
-- database-init.sql dosyasında var
-- Veritabanı başladıktan sonra verileri kontrol etmek için
-- =============================================

SELECT 'Users Count: ' || COUNT(*) FROM users;
SELECT 'Incidents Count: ' || COUNT(*) FROM incidents;
SELECT 'Incident Types Count: ' || COUNT(*) FROM incident_types;

SELECT * FROM users;
SELECT incident_id, title, incident_type, severity_level, status FROM incidents ORDER BY created_at DESC;

SELECT '✓ Database initialized successfully!' AS status;
