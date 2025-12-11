package com.cyberincident.repository;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Repository
public class AnalyticsRepository {

    private final JdbcTemplate jdbcTemplate;

    public AnalyticsRepository(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    public Map<String, Integer> getIncidentCountByType() {
        String sql = "SELECT incident_type, COUNT(*) as count FROM incidents " +
                     "GROUP BY incident_type ORDER BY count DESC";
        
        Map<String, Integer> result = new HashMap<>();
        
        jdbcTemplate.query(sql, rs -> {
            result.put(rs.getString("incident_type"), rs.getInt("count"));
        });
        
        return result;
    }

    public Map<String, Integer> getIncidentCountBySeverity() {
        String sql = "SELECT severity_level, COUNT(*) as count FROM incidents " +
                     "GROUP BY severity_level ORDER BY " +
                     "CASE severity_level " +
                     "WHEN 'CRITICAL' THEN 1 " +
                     "WHEN 'HIGH' THEN 2 " +
                     "WHEN 'MEDIUM' THEN 3 " +
                     "WHEN 'LOW' THEN 4 " +
                     "END";
        
        Map<String, Integer> result = new HashMap<>();
        
        jdbcTemplate.query(sql, rs -> {
            result.put(rs.getString("severity_level"), rs.getInt("count"));
        });
        
        return result;
    }

    public Map<String, Integer> getIncidentCountByStatus() {
        String sql = "SELECT status, COUNT(*) as count FROM incidents " +
                     "GROUP BY status ORDER BY count DESC";
        
        Map<String, Integer> result = new HashMap<>();
        
        jdbcTemplate.query(sql, rs -> {
            result.put(rs.getString("status"), rs.getInt("count"));
        });
        
        return result;
    }

    public Integer getCriticalIncidentCount() {
        String sql = "SELECT COUNT(*) as critical_count FROM incidents " +
                     "WHERE severity_level = 'CRITICAL' AND status != 'CLOSED'";
        return jdbcTemplate.queryForObject(sql, Integer.class);
    }

    public Integer getOpenIncidentCount() {
        String sql = "SELECT COUNT(*) FROM incidents WHERE status = 'OPEN'";
        return jdbcTemplate.queryForObject(sql, Integer.class);
    }

    public Integer getResolvedIncidentCount() {
        String sql = "SELECT COUNT(*) FROM incidents WHERE status IN ('RESOLVED', 'CLOSED')";
        return jdbcTemplate.queryForObject(sql, Integer.class);
    }

    public List<Map<String, Object>> getIncidentTimeline(Integer days) {
        String sql = "SELECT DATE(incident_date) as date, COUNT(*) as count " +
                     "FROM incidents " +
                     "WHERE incident_date >= CURRENT_DATE - INTERVAL '" + days + " days' " +
                     "GROUP BY DATE(incident_date) " +
                     "ORDER BY date DESC";
        
        return jdbcTemplate.queryForList(sql);
    }

    public Map<String, Object> getStatusSummary() {
        Map<String, Object> summary = new HashMap<>();
        
        String sql = "SELECT " +
                     "COUNT(*) as total, " +
                     "SUM(CASE WHEN status = 'OPEN' THEN 1 ELSE 0 END) as open, " +
                     "SUM(CASE WHEN status = 'IN_PROGRESS' THEN 1 ELSE 0 END) as in_progress, " +
                     "SUM(CASE WHEN status = 'RESOLVED' THEN 1 ELSE 0 END) as resolved, " +
                     "SUM(CASE WHEN status = 'CLOSED' THEN 1 ELSE 0 END) as closed " +
                     "FROM incidents";
        
        jdbcTemplate.query(sql, rs -> {
            summary.put("total", rs.getInt("total"));
            summary.put("open", rs.getInt("open"));
            summary.put("inProgress", rs.getInt("in_progress"));
            summary.put("resolved", rs.getInt("resolved"));
            summary.put("closed", rs.getInt("closed"));
        });
        
        return summary;
    }

    public List<Map<String, Object>> getTopReporters(Integer limit) {
        String sql = "SELECT u.user_id, u.full_name, COUNT(i.incident_id) as incident_count " +
                     "FROM users u " +
                     "LEFT JOIN incidents i ON u.user_id = i.reporter_id " +
                     "GROUP BY u.user_id, u.full_name " +
                     "ORDER BY incident_count DESC " +
                     "LIMIT ?";
        
        return jdbcTemplate.queryForList(sql, limit);
    }
}

