package com.cyberincident.service;

import com.cyberincident.model.Incident;
import com.cyberincident.repository.IncidentRepository;
import com.cyberincident.repository.UserRepository;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;

@Service
public class IncidentService {

    private final IncidentRepository incidentRepository;
    private final UserRepository userRepository;

    private static final List<String> VALID_INCIDENT_TYPES = Arrays.asList(
        "PHISHING", "UNAUTHORIZED_ACCESS", "MALWARE", "DATA_BREACH",
        "DOS_ATTACK", "SOCIAL_ENGINEERING", "RANSOMWARE", "INSIDER_THREAT", "OTHER"
    );

    private static final List<String> VALID_SEVERITY_LEVELS = Arrays.asList(
        "LOW", "MEDIUM", "HIGH", "CRITICAL"
    );

    private static final List<String> VALID_STATUSES = Arrays.asList(
        "OPEN", "IN_PROGRESS", "RESOLVED", "CLOSED"
    );

    public IncidentService(IncidentRepository incidentRepository, UserRepository userRepository) {
        this.incidentRepository = incidentRepository;
        this.userRepository = userRepository;
    }

    public Incident createIncident(Incident incident) {
        // Validate required fields
        validateIncident(incident);

        // Validate reporter exists
        if (userRepository.findById(incident.getReporterId()).isEmpty()) {
            throw new IllegalArgumentException("Reporter not found with ID: " + incident.getReporterId());
        }

        // Set default status if not provided
        if (incident.getStatus() == null || incident.getStatus().trim().isEmpty()) {
            incident.setStatus("OPEN");
        }

        return incidentRepository.create(incident);
    }

    public List<Incident> getAllIncidents(String type, String severity, 
                                         LocalDateTime startDate, LocalDateTime endDate, String status) {
        // Validate filters
        if (type != null && !type.isEmpty() && !VALID_INCIDENT_TYPES.contains(type)) {
            throw new IllegalArgumentException("Invalid incident type: " + type);
        }
        if (severity != null && !severity.isEmpty() && !VALID_SEVERITY_LEVELS.contains(severity)) {
            throw new IllegalArgumentException("Invalid severity level: " + severity);
        }
        if (status != null && !status.isEmpty() && !VALID_STATUSES.contains(status)) {
            throw new IllegalArgumentException("Invalid status: " + status);
        }

        return incidentRepository.findAll(type, severity, startDate, endDate, status);
    }

    public Optional<Incident> getIncidentById(Integer incidentId) {
        return incidentRepository.findById(incidentId);
    }

    public List<Incident> getIncidentsByReporter(Integer reporterId) {
        return incidentRepository.findByReporterId(reporterId);
    }

    public Incident updateIncident(Integer incidentId, Incident incident) {
        // Check if incident exists
        Optional<Incident> existingIncident = incidentRepository.findById(incidentId);
        if (existingIncident.isEmpty()) {
            throw new IllegalArgumentException("Incident not found with ID: " + incidentId);
        }

        // Validate incident data
        validateIncident(incident);

        // Validate reporter exists
        if (userRepository.findById(incident.getReporterId()).isEmpty()) {
            throw new IllegalArgumentException("Reporter not found with ID: " + incident.getReporterId());
        }

        return incidentRepository.update(incidentId, incident);
    }

    public void updateIncidentStatus(Integer incidentId, String status) {
        // Check if incident exists
        Optional<Incident> existingIncident = incidentRepository.findById(incidentId);
        if (existingIncident.isEmpty()) {
            throw new IllegalArgumentException("Incident not found with ID: " + incidentId);
        }

        // Validate status
        if (!VALID_STATUSES.contains(status)) {
            throw new IllegalArgumentException("Invalid status: " + status);
        }

        incidentRepository.updateStatus(incidentId, status);
    }

    public void deleteIncident(Integer incidentId) {
        // Check if incident exists
        Optional<Incident> existingIncident = incidentRepository.findById(incidentId);
        if (existingIncident.isEmpty()) {
            throw new IllegalArgumentException("Incident not found with ID: " + incidentId);
        }

        incidentRepository.delete(incidentId);
    }

    public Integer getTotalIncidentCount() {
        return incidentRepository.countAll();
    }

    public Integer getIncidentCountByStatus(String status) {
        if (!VALID_STATUSES.contains(status)) {
            throw new IllegalArgumentException("Invalid status: " + status);
        }
        return incidentRepository.countByStatus(status);
    }

    private void validateIncident(Incident incident) {
        if (incident.getTitle() == null || incident.getTitle().trim().isEmpty()) {
            throw new IllegalArgumentException("Title is required");
        }
        if (incident.getDescription() == null || incident.getDescription().trim().isEmpty()) {
            throw new IllegalArgumentException("Description is required");
        }
        if (incident.getIncidentType() == null || incident.getIncidentType().trim().isEmpty()) {
            throw new IllegalArgumentException("Incident type is required");
        }
        if (!VALID_INCIDENT_TYPES.contains(incident.getIncidentType())) {
            throw new IllegalArgumentException("Invalid incident type: " + incident.getIncidentType());
        }
        if (incident.getSeverityLevel() == null || incident.getSeverityLevel().trim().isEmpty()) {
            throw new IllegalArgumentException("Severity level is required");
        }
        if (!VALID_SEVERITY_LEVELS.contains(incident.getSeverityLevel())) {
            throw new IllegalArgumentException("Invalid severity level: " + incident.getSeverityLevel());
        }
        if (incident.getIncidentDate() == null) {
            throw new IllegalArgumentException("Incident date is required");
        }
        if (incident.getReporterId() == null) {
            throw new IllegalArgumentException("Reporter ID is required");
        }
        if (incident.getStatus() != null && !incident.getStatus().isEmpty() 
            && !VALID_STATUSES.contains(incident.getStatus())) {
            throw new IllegalArgumentException("Invalid status: " + incident.getStatus());
        }
    }
}

