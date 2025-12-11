package com.cyberincident.service;

import com.cyberincident.repository.AnalyticsRepository;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class AnalyticsService {

    private final AnalyticsRepository analyticsRepository;

    public AnalyticsService(AnalyticsRepository analyticsRepository) {
        this.analyticsRepository = analyticsRepository;
    }

    public Map<String, Integer> getIncidentTypeStatistics() {
        return analyticsRepository.getIncidentCountByType();
    }

    public Map<String, Integer> getSeverityStatistics() {
        return analyticsRepository.getIncidentCountBySeverity();
    }

    public Map<String, Integer> getStatusStatistics() {
        return analyticsRepository.getIncidentCountByStatus();
    }

    public Integer getCriticalIncidentCount() {
        Integer count = analyticsRepository.getCriticalIncidentCount();
        return count != null ? count : 0;
    }

    public Integer getOpenIncidentCount() {
        Integer count = analyticsRepository.getOpenIncidentCount();
        return count != null ? count : 0;
    }

    public Integer getResolvedIncidentCount() {
        Integer count = analyticsRepository.getResolvedIncidentCount();
        return count != null ? count : 0;
    }

    public List<Map<String, Object>> getIncidentTimeline(Integer days) {
        if (days == null || days <= 0) {
            days = 30; // Default to 30 days
        }
        if (days > 365) {
            days = 365; // Maximum 1 year
        }
        return analyticsRepository.getIncidentTimeline(days);
    }

    public Map<String, Object> getStatusSummary() {
        return analyticsRepository.getStatusSummary();
    }

    public List<Map<String, Object>> getTopReporters(Integer limit) {
        if (limit == null || limit <= 0) {
            limit = 10; // Default to top 10
        }
        if (limit > 100) {
            limit = 100; // Maximum 100
        }
        return analyticsRepository.getTopReporters(limit);
    }

    public Map<String, Object> getDashboardSummary() {
        Map<String, Object> summary = new HashMap<>();
        
        summary.put("statusSummary", getStatusSummary());
        summary.put("severityStats", getSeverityStatistics());
        summary.put("typeStats", getIncidentTypeStatistics());
        summary.put("criticalCount", getCriticalIncidentCount());
        summary.put("openCount", getOpenIncidentCount());
        summary.put("resolvedCount", getResolvedIncidentCount());
        
        return summary;
    }
}

