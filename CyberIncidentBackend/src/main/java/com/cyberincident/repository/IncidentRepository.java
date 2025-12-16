package com.cyberincident.repository;

import com.cyberincident.model.Incident;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.RowMapper;
import org.springframework.jdbc.support.GeneratedKeyHolder;
import org.springframework.jdbc.support.KeyHolder;
import org.springframework.stereotype.Repository;

import java.sql.PreparedStatement;
import java.sql.Statement;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Repository
public class IncidentRepository {

    private final JdbcTemplate jdbcTemplate;

    public IncidentRepository(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    private final RowMapper<Incident> incidentRowMapper = (rs, rowNum) -> {
        Incident incident = new Incident();
        incident.setIncidentId(rs.getInt("incident_id"));
        incident.setTitle(rs.getString("title"));
        incident.setDescription(rs.getString("description"));
        incident.setIncidentType(rs.getString("incident_type"));
        incident.setSeverityLevel(rs.getString("severity_level"));

        Timestamp incidentDate = rs.getTimestamp("incident_date");
        if (incidentDate != null) {
            incident.setIncidentDate(incidentDate.toLocalDateTime());
        }

        incident.setStatus(rs.getString("status"));
        incident.setReporterId(rs.getInt("reporter_id"));

        // IOC ve Analist alanları
        incident.setIocs(rs.getString("iocs"));
        incident.setOpenedByAnalyst(rs.getString("opened_by_analyst"));
        incident.setClosedByAnalyst(rs.getString("closed_by_analyst"));

        // Reporter name may be included in JOIN queries
        try {
            incident.setReporterName(rs.getString("reporter_name"));
        } catch (Exception e) {
            // Column not present in this query
        }

        Timestamp createdAt = rs.getTimestamp("created_at");
        if (createdAt != null) {
            incident.setCreatedAt(createdAt.toLocalDateTime());
        }

        Timestamp updatedAt = rs.getTimestamp("updated_at");
        if (updatedAt != null) {
            incident.setUpdatedAt(updatedAt.toLocalDateTime());
        }

        Timestamp resolvedAt = rs.getTimestamp("resolved_at");
        if (resolvedAt != null) {
            incident.setResolvedAt(resolvedAt.toLocalDateTime());
        }

        return incident;
    };

    public Incident create(Incident incident) {
        String sql = "INSERT INTO incidents (title, description, incident_type, severity_level, " +
                "incident_date, reporter_id, status, iocs, opened_by_analyst) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?) " +
                "RETURNING incident_id";

        KeyHolder keyHolder = new GeneratedKeyHolder();

        jdbcTemplate.update(connection -> {
            PreparedStatement ps = connection.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS);
            ps.setString(1, incident.getTitle());
            ps.setString(2, incident.getDescription());
            ps.setString(3, incident.getIncidentType());
            ps.setString(4, incident.getSeverityLevel());
            ps.setTimestamp(5, Timestamp.valueOf(incident.getIncidentDate()));
            ps.setInt(6, incident.getReporterId());
            ps.setString(7, incident.getStatus() != null ? incident.getStatus() : "OPEN");
            ps.setString(8, incident.getIocs());
            ps.setString(9, incident.getOpenedByAnalyst());
            return ps;
        }, keyHolder);

        Integer incidentId = (Integer) keyHolder.getKeys().get("incident_id");
        incident.setIncidentId(incidentId);

        return findById(incidentId).orElse(incident);
    }

    public List<Incident> findAll(String type, String severity, LocalDateTime startDate, LocalDateTime endDate,
            String status) {
        StringBuilder sql = new StringBuilder(
                "SELECT i.*, u.full_name as reporter_name FROM incidents i " +
                        "JOIN users u ON i.reporter_id = u.user_id WHERE 1=1");

        List<Object> params = new ArrayList<>();

        if (type != null && !type.isEmpty()) {
            sql.append(" AND i.incident_type = ?");
            params.add(type);
        }

        if (severity != null && !severity.isEmpty()) {
            sql.append(" AND i.severity_level = ?");
            params.add(severity);
        }

        if (startDate != null) {
            sql.append(" AND i.incident_date >= ?");
            params.add(Timestamp.valueOf(startDate));
        }

        if (endDate != null) {
            sql.append(" AND i.incident_date <= ?");
            params.add(Timestamp.valueOf(endDate));
        }

        if (status != null && !status.isEmpty()) {
            sql.append(" AND i.status = ?");
            params.add(status);
        }

        sql.append(" ORDER BY i.created_at DESC");

        return jdbcTemplate.query(sql.toString(), incidentRowMapper, params.toArray());
    }

    public Optional<Incident> findById(Integer incidentId) {
        String sql = "SELECT i.*, u.full_name as reporter_name FROM incidents i " +
                "JOIN users u ON i.reporter_id = u.user_id WHERE i.incident_id = ?";
        List<Incident> incidents = jdbcTemplate.query(sql, incidentRowMapper, incidentId);
        return incidents.isEmpty() ? Optional.empty() : Optional.of(incidents.get(0));
    }

    public Incident update(Integer incidentId, Incident incident) {
        String sql = "UPDATE incidents SET title = ?, description = ?, incident_type = ?, " +
                "severity_level = ?, incident_date = ?, status = ?, iocs = ?, updated_at = CURRENT_TIMESTAMP " +
                "WHERE incident_id = ?";

        jdbcTemplate.update(sql,
                incident.getTitle(),
                incident.getDescription(),
                incident.getIncidentType(),
                incident.getSeverityLevel(),
                Timestamp.valueOf(incident.getIncidentDate()),
                incident.getStatus(),
                incident.getIocs(),
                incidentId);

        return findById(incidentId).orElse(incident);
    }

    public void updateStatus(Integer incidentId, String status, String analystName) {
        StringBuilder sql = new StringBuilder("UPDATE incidents SET status = ?, updated_at = CURRENT_TIMESTAMP");
        List<Object> params = new ArrayList<>();
        params.add(status);

        if (status.equals("RESOLVED") || status.equals("CLOSED")) {
            sql.append(", resolved_at = CURRENT_TIMESTAMP, closed_by_analyst = ?");
            params.add(analystName);
        }

        sql.append(" WHERE incident_id = ?");
        params.add(incidentId);

        jdbcTemplate.update(sql.toString(), params.toArray());
    }

    public void delete(Integer incidentId) {
        String sql = "DELETE FROM incidents WHERE incident_id = ?";
        jdbcTemplate.update(sql, incidentId);
    }

    public List<Incident> findByReporterId(Integer reporterId) {
        String sql = "SELECT i.*, u.full_name as reporter_name FROM incidents i " +
                "JOIN users u ON i.reporter_id = u.user_id " +
                "WHERE i.reporter_id = ? ORDER BY i.created_at DESC";
        return jdbcTemplate.query(sql, incidentRowMapper, reporterId);
    }

    public Integer countByStatus(String status) {
        String sql = "SELECT COUNT(*) FROM incidents WHERE status = ?";
        return jdbcTemplate.queryForObject(sql, Integer.class, status);
    }

    public Integer countAll() {
        String sql = "SELECT COUNT(*) FROM incidents";
        return jdbcTemplate.queryForObject(sql, Integer.class);
    }
}
