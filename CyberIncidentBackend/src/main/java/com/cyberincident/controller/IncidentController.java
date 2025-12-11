package com.cyberincident.controller;

import com.cyberincident.model.Incident;
import com.cyberincident.service.IncidentService;
import jakarta.validation.Valid;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import java.util.Optional;

@RestController
@RequestMapping("/api/incidents")
@CrossOrigin(origins = "*")
public class IncidentController {

    private final IncidentService incidentService;

    public IncidentController(IncidentService incidentService) {
        this.incidentService = incidentService;
    }

    @PostMapping
    public ResponseEntity<?> createIncident(@Valid @RequestBody Incident incident) {
        try {
            Incident createdIncident = incidentService.createIncident(incident);
            return ResponseEntity.status(HttpStatus.CREATED).body(createdIncident);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(Map.of("error", e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to create incident: " + e.getMessage()));
        }
    }

    @GetMapping
    public ResponseEntity<?> getAllIncidents(
            @RequestParam(required = false) String type,
            @RequestParam(required = false) String severity,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime startDate,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime endDate,
            @RequestParam(required = false) String status) {
        try {
            List<Incident> incidents = incidentService.getAllIncidents(type, severity, startDate, endDate, status);
            return ResponseEntity.ok(incidents);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(Map.of("error", e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch incidents: " + e.getMessage()));
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getIncidentById(@PathVariable Integer id) {
        try {
            Optional<Incident> incident = incidentService.getIncidentById(id);
            if (incident.isPresent()) {
                return ResponseEntity.ok(incident.get());
            } else {
                return ResponseEntity.status(HttpStatus.NOT_FOUND)
                        .body(Map.of("error", "Incident not found with ID: " + id));
            }
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch incident: " + e.getMessage()));
        }
    }

    @GetMapping("/reporter/{reporterId}")
    public ResponseEntity<?> getIncidentsByReporter(@PathVariable Integer reporterId) {
        try {
            List<Incident> incidents = incidentService.getIncidentsByReporter(reporterId);
            return ResponseEntity.ok(incidents);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch incidents: " + e.getMessage()));
        }
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateIncident(@PathVariable Integer id, @Valid @RequestBody Incident incident) {
        try {
            Incident updatedIncident = incidentService.updateIncident(id, incident);
            return ResponseEntity.ok(updatedIncident);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(Map.of("error", e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to update incident: " + e.getMessage()));
        }
    }

    @PatchMapping("/{id}/status")
    public ResponseEntity<?> updateIncidentStatus(@PathVariable Integer id, @RequestBody Map<String, String> body) {
        try {
            String status = body.get("status");
            if (status == null || status.trim().isEmpty()) {
                return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                        .body(Map.of("error", "Status is required"));
            }
            
            incidentService.updateIncidentStatus(id, status);
            return ResponseEntity.ok(Map.of("message", "Incident status updated successfully"));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(Map.of("error", e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to update incident status: " + e.getMessage()));
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteIncident(@PathVariable Integer id) {
        try {
            incidentService.deleteIncident(id);
            return ResponseEntity.ok(Map.of("message", "Incident deleted successfully"));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                    .body(Map.of("error", e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to delete incident: " + e.getMessage()));
        }
    }

    @GetMapping("/count")
    public ResponseEntity<?> getTotalIncidentCount() {
        try {
            Integer count = incidentService.getTotalIncidentCount();
            return ResponseEntity.ok(Map.of("count", count));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to count incidents: " + e.getMessage()));
        }
    }

    @GetMapping("/count/{status}")
    public ResponseEntity<?> getIncidentCountByStatus(@PathVariable String status) {
        try {
            Integer count = incidentService.getIncidentCountByStatus(status);
            return ResponseEntity.ok(Map.of("count", count));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(Map.of("error", e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to count incidents: " + e.getMessage()));
        }
    }
}

