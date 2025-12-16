package com.cyberincident.model;

import com.fasterxml.jackson.annotation.JsonFormat;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import java.time.LocalDateTime;

public class Incident {
    private Integer incidentId;

    @NotBlank(message = "Title is required")
    private String title;

    @NotBlank(message = "Description is required")
    private String description;

    @NotBlank(message = "Incident type is required")
    private String incidentType;

    @NotBlank(message = "Severity level is required")
    private String severityLevel;

    @NotNull(message = "Incident date is required")
    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss")
    private LocalDateTime incidentDate;

    private String status;

    @NotNull(message = "Reporter ID is required")
    private Integer reporterId;

    private String reporterName;

    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss")
    private LocalDateTime createdAt;

    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss")
    private LocalDateTime updatedAt;

    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss")
    private LocalDateTime resolvedAt;

    // IOC ve Analist takip alanları
    private String iocs; // Bulunan IOC'ler (IP, domain, hash vb.)
    private String openedByAnalyst; // Incident'ı açan analist
    private String closedByAnalyst; // Incident'ı kapatan analist

    // Constructors
    public Incident() {
    }

    public Incident(Integer incidentId, String title, String description, String incidentType,
            String severityLevel, LocalDateTime incidentDate, String status,
            Integer reporterId, String reporterName, LocalDateTime createdAt,
            LocalDateTime updatedAt, LocalDateTime resolvedAt) {
        this.incidentId = incidentId;
        this.title = title;
        this.description = description;
        this.incidentType = incidentType;
        this.severityLevel = severityLevel;
        this.incidentDate = incidentDate;
        this.status = status;
        this.reporterId = reporterId;
        this.reporterName = reporterName;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
        this.resolvedAt = resolvedAt;
    }

    // Getters and Setters
    public Integer getIncidentId() {
        return incidentId;
    }

    public void setIncidentId(Integer incidentId) {
        this.incidentId = incidentId;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getIncidentType() {
        return incidentType;
    }

    public void setIncidentType(String incidentType) {
        this.incidentType = incidentType;
    }

    public String getSeverityLevel() {
        return severityLevel;
    }

    public void setSeverityLevel(String severityLevel) {
        this.severityLevel = severityLevel;
    }

    public LocalDateTime getIncidentDate() {
        return incidentDate;
    }

    public void setIncidentDate(LocalDateTime incidentDate) {
        this.incidentDate = incidentDate;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public Integer getReporterId() {
        return reporterId;
    }

    public void setReporterId(Integer reporterId) {
        this.reporterId = reporterId;
    }

    public String getReporterName() {
        return reporterName;
    }

    public void setReporterName(String reporterName) {
        this.reporterName = reporterName;
    }

    public LocalDateTime getCreatedAt() {
        return createdAt;
    }

    public void setCreatedAt(LocalDateTime createdAt) {
        this.createdAt = createdAt;
    }

    public LocalDateTime getUpdatedAt() {
        return updatedAt;
    }

    public void setUpdatedAt(LocalDateTime updatedAt) {
        this.updatedAt = updatedAt;
    }

    public LocalDateTime getResolvedAt() {
        return resolvedAt;
    }

    public void setResolvedAt(LocalDateTime resolvedAt) {
        this.resolvedAt = resolvedAt;
    }

    public String getIocs() {
        return iocs;
    }

    public void setIocs(String iocs) {
        this.iocs = iocs;
    }

    public String getOpenedByAnalyst() {
        return openedByAnalyst;
    }

    public void setOpenedByAnalyst(String openedByAnalyst) {
        this.openedByAnalyst = openedByAnalyst;
    }

    public String getClosedByAnalyst() {
        return closedByAnalyst;
    }

    public void setClosedByAnalyst(String closedByAnalyst) {
        this.closedByAnalyst = closedByAnalyst;
    }

    @Override
    public String toString() {
        return "Incident{" +
                "incidentId=" + incidentId +
                ", title='" + title + '\'' +
                ", incidentType='" + incidentType + '\'' +
                ", severityLevel='" + severityLevel + '\'' +
                ", status='" + status + '\'' +
                ", reporterId=" + reporterId +
                ", reporterName='" + reporterName + '\'' +
                '}';
    }
}
