package com.cyberincident.controller;

import com.cyberincident.service.AnalyticsService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/analytics")
@CrossOrigin(origins = "*")
public class AnalyticsController {

    private final AnalyticsService analyticsService;

    public AnalyticsController(AnalyticsService analyticsService) {
        this.analyticsService = analyticsService;
    }

    @GetMapping("/incident-types")
    public ResponseEntity<?> getIncidentTypeStatistics() {
        try {
            Map<String, Integer> stats = analyticsService.getIncidentTypeStatistics();
            return ResponseEntity.ok(stats);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch incident type statistics: " + e.getMessage()));
        }
    }

    @GetMapping("/severity-stats")
    public ResponseEntity<?> getSeverityStatistics() {
        try {
            Map<String, Integer> stats = analyticsService.getSeverityStatistics();
            return ResponseEntity.ok(stats);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch severity statistics: " + e.getMessage()));
        }
    }

    @GetMapping("/status-stats")
    public ResponseEntity<?> getStatusStatistics() {
        try {
            Map<String, Integer> stats = analyticsService.getStatusStatistics();
            return ResponseEntity.ok(stats);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch status statistics: " + e.getMessage()));
        }
    }

    @GetMapping("/critical-count")
    public ResponseEntity<?> getCriticalIncidentCount() {
        try {
            Integer count = analyticsService.getCriticalIncidentCount();
            return ResponseEntity.ok(Map.of("criticalCount", count));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch critical incident count: " + e.getMessage()));
        }
    }

    @GetMapping("/open-count")
    public ResponseEntity<?> getOpenIncidentCount() {
        try {
            Integer count = analyticsService.getOpenIncidentCount();
            return ResponseEntity.ok(Map.of("openCount", count));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch open incident count: " + e.getMessage()));
        }
    }

    @GetMapping("/resolved-count")
    public ResponseEntity<?> getResolvedIncidentCount() {
        try {
            Integer count = analyticsService.getResolvedIncidentCount();
            return ResponseEntity.ok(Map.of("resolvedCount", count));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch resolved incident count: " + e.getMessage()));
        }
    }

    @GetMapping("/timeline")
    public ResponseEntity<?> getIncidentTimeline(@RequestParam(required = false, defaultValue = "30") Integer days) {
        try {
            List<Map<String, Object>> timeline = analyticsService.getIncidentTimeline(days);
            return ResponseEntity.ok(timeline);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch incident timeline: " + e.getMessage()));
        }
    }

    @GetMapping("/status-summary")
    public ResponseEntity<?> getStatusSummary() {
        try {
            Map<String, Object> summary = analyticsService.getStatusSummary();
            return ResponseEntity.ok(summary);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch status summary: " + e.getMessage()));
        }
    }

    @GetMapping("/top-reporters")
    public ResponseEntity<?> getTopReporters(@RequestParam(required = false, defaultValue = "10") Integer limit) {
        try {
            List<Map<String, Object>> reporters = analyticsService.getTopReporters(limit);
            return ResponseEntity.ok(reporters);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch top reporters: " + e.getMessage()));
        }
    }

    @GetMapping("/dashboard")
    public ResponseEntity<?> getDashboardSummary() {
        try {
            Map<String, Object> dashboard = analyticsService.getDashboardSummary();
            return ResponseEntity.ok(dashboard);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(Map.of("error", "Failed to fetch dashboard summary: " + e.getMessage()));
        }
    }
}

