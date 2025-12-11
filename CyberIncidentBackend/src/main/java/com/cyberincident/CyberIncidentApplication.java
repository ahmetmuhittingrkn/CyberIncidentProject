package com.cyberincident;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.jdbc.DataSourceAutoConfiguration;

@SpringBootApplication(exclude = {DataSourceAutoConfiguration.class})
public class CyberIncidentApplication {

    public static void main(String[] args) {
        SpringApplication.run(CyberIncidentApplication.class, args);
        System.out.println("\n========================================");
        System.out.println("Cyber Incident API is running!");
        System.out.println("API Base URL: http://localhost:8080/api");
        System.out.println("========================================\n");
    }
}

