---
title: Architecture Guide
version: 2.0.0
status: Approved
owner: Project Orkestra
author: Wesley Oliveira
last_updated: 2026-08-29

related:
  - PROJECT_CONTEXT.md
  - DOMAIN_CONTEXT.md
  - TECH_STACK.md
  - CODING_STANDARDS.md
  - ENGINEERING_PRINCIPLES.md
---

# Architecture Guide

## Purpose

This document defines the architectural standards of Project Orkestra.

Every software component, feature, module and infrastructure decision must follow the principles described here.

This document is the architectural source of truth for both human developers and AI assistants.

---

# Architectural Goals

Project Orkestra is designed to be:

- Modular
- Maintainable
- Testable
- Scalable
- Secure
- Observable
- Cloud Ready
- Easy to evolve

The architecture must prioritize long-term maintainability over short-term implementation speed.

---

# High-Level Architecture

The system follows a Modular Monolith architecture.

Each module is developed independently while sharing the same deployment.

Future migration to microservices must be possible without major rewrites.

```
                    Frontend (React)

                           │

                    ASP.NET Core API

                           │

────────────────────────────────────────────────────────

                 Application Layer

────────────────────────────────────────────────────────

                   Domain Layer

────────────────────────────────────────────────────────

              Infrastructure Layer

────────────────────────────────────────────────────────

         MongoDB          Redis          External Services
```

---

# Architectural Style

Project Orkestra adopts:

- Clean Architecture
- Domain-Oriented Design
- Layered Architecture
- Dependency Injection
- REST APIs
- Modular Monolith

DDD tactical patterns should only be introduced when they reduce complexity.

Avoid implementing DDD patterns simply because they exist.

---

# Architectural Principles

## Rule 1

Business rules must never depend on frameworks.

The Domain Layer must not reference:

- ASP.NET Core
- MongoDB
- Redis
- Docker
- Swagger
- Newtonsoft.Json

Frameworks are implementation details.

---

## Rule 2

Dependencies always point inward.

Allowed dependency flow:

```
Presentation

↓

Application

↓

Domain

↓

Infrastructure
```

Never invert this dependency direction.

---

## Rule 3

The Domain Layer contains business knowledge.

It is responsible for:

- Business rules
- Domain entities
- Value Objects
- Domain services
- Business validation
- Domain events (future)

The Domain Layer must never perform:

- HTTP requests
- Database access
- Logging
- Authentication
- Serialization

---

## Rule 4

Application Layer coordinates use cases.

Responsibilities include:

- Executing use cases
- Validation
- Transactions
- Authorization
- Calling repositories
- Mapping DTOs

Business rules belong to the Domain.

---

## Rule 5

Infrastructure implements technical concerns.

Examples:

- MongoDB repositories
- Redis cache
- Authentication providers
- Email providers
- File Storage
- Logging
- Feature Flags

Infrastructure must never contain business logic.

---

## Rule 6

Presentation is responsible only for communication.

Controllers should:

- Validate request format
- Invoke Application services
- Return HTTP responses

Controllers must never:

- Access MongoDB directly
- Contain business rules
- Perform calculations
- Execute scheduling logic

Controllers should remain thin.

---

# Modular Architecture

Modules must be cohesive and loosely coupled.

Initial modules:

```
Identity

Administration

Organizations

Business Units

Employees

Scheduling

Calendar

Notifications

Dashboard

Audit

Settings
```

Modules communicate through Application contracts.

Avoid direct module dependencies whenever possible.

---

# Solution Structure

```
backend/

src/

ProjectOrkestra.Api

ProjectOrkestra.Application

ProjectOrkestra.Domain

ProjectOrkestra.Infrastructure
```

Future modules may be separated internally.

Example:

```
Application/

Employees/

Scheduling/

Identity/

Dashboard/
```

---

# Dependency Injection

Dependency Injection is mandatory.

Requirements:

- Constructor Injection only
- No Service Locator
- No static dependencies

Services should depend on abstractions.

---

# Repository Pattern

Repositories are abstractions.

Interfaces belong to Application or Domain.

Implementations belong to Infrastructure.

Example:

```
Application

IEmployeeRepository

↓

Infrastructure

MongoEmployeeRepository
```

Never inject MongoDB collections directly into business code.

---

# DTO Policy

DTOs belong to the Presentation/Application boundary.

DTOs must never enter the Domain.

Domain Entities must never inherit DTOs.

---

# Mapping

Mapping should remain explicit whenever practical.

Avoid unnecessary abstraction.

If object mapping becomes repetitive, evaluate introducing AutoMapper in a future iteration.

---

# Validation Strategy

Validation occurs at multiple levels.

Presentation

- Required fields
- Request format

Application

- Command validation
- Authorization
- Workflow validation

Domain

- Business rules
- Invariants

Infrastructure

- External integration validation

Never rely exclusively on frontend validation.

---

# Error Handling

Use centralized exception handling.

Every API error must return ProblemDetails.

Do not expose:

- Stack traces
- Internal exception messages
- Connection strings
- Database details

Unexpected errors should be logged.

---

# Logging

Structured logging only.

Every log should contain:

- Correlation ID
- Tenant ID
- Organization ID (when available)
- User ID
- Timestamp

Sensitive information must never be logged.

---

# Authentication

Authentication uses JWT Bearer.

Authentication answers:

Who are you?

Authorization answers:

What are you allowed to do?

These concerns must remain separated.

---

# Authorization

Authorization is Role-Based.

Roles:

- Platform Admin
- Organization Admin
- Manager
- Employee

Future support for Permission-Based Authorization is expected.

---

# Multi-Tenant Strategy

Project Orkestra is multi-tenant by design.

Every request belongs to one Tenant.

Every persisted document must include Tenant identification.

Data isolation is mandatory.

No tenant may access another tenant's data.

---

# Database Strategy

Primary database:

MongoDB

Cache:

Redis

MongoDB stores:

- Employees
- Organizations
- Business Units
- Schedules
- Roles

Redis stores:

- Sessions
- Cache
- Temporary data

Business logic must remain database agnostic.

---

# API Design

The API follows REST principles.

Guidelines:

- Use nouns
- Use plural resource names
- Version endpoints
- Return proper HTTP status codes
- Support pagination
- Support filtering
- Support sorting

---

# API Versioning

All endpoints are versioned using URL-based versioning.

Pattern:

```
/api/v{major}/{resource}
```

Examples:

```
GET  /api/v1/organizations
GET  /api/v1/employees
POST /api/v1/business-units
PUT  /api/v1/organizations/{id}
DELETE /api/v1/employees/{id}
```

Versioning Strategy:

- Increment **major** version when breaking changes occur (e.g., removing a field, changing response format)
- New endpoints always use the latest version
- Old versions may coexist during a deprecation period
- Deprecation timeline should be communicated to clients (typically 6-12 months notice)

---

# Exception Handling & HTTP Semantics

Project Orkestra uses centralized exception handling via middleware.

Exception-to-HTTP-Status Mapping:

| Exception Type                | HTTP Status | Reason                                                                              |
| ----------------------------- | ----------- | ----------------------------------------------------------------------------------- |
| `NotFoundException`           | 404         | Resource not found (e.g., Employee ID doesn't exist)                                |
| `BusinessRuleException`       | 400         | Business rule violated (e.g., cannot deactivate organization with active employees) |
| `ArgumentException`           | 400         | Invalid argument (e.g., invalid CNPJ format)                                        |
| `ArgumentNullException`       | 400         | Null argument (e.g., empty organization name)                                       |
| `UnauthorizedAccessException` | 403         | User lacks permission (authorization failure)                                       |
| `Unhandled Exception`         | 500         | Unexpected server error                                                             |

Response Format:

All errors return RFC 7807 ProblemDetails format:

```json
{
  "type": "https://orkestra.com/errors/not-found",
  "title": "Resource Not Found",
  "status": 404,
  "detail": "Employee with ID 'abc123' was not found",
  "instance": "/api/v1/employees/abc123"
}
```

Middleware Implementation:

The `ExceptionHandlingMiddleware` intercepts all exceptions and converts them to appropriate HTTP responses:

```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var response = new ProblemDetails()
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case NotFoundException nfe:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.Title = "Not Found";
                response.Status = StatusCodes.Status404NotFound;
                response.Detail = nfe.Message;
                break;

            case BusinessRuleException bre:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Title = "Business Rule Violation";
                response.Status = StatusCodes.Status400BadRequest;
                response.Detail = bre.Message;
                break;

            case ArgumentException ae:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Title = "Invalid Argument";
                response.Status = StatusCodes.Status400BadRequest;
                response.Detail = ae.Message;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Title = "Internal Server Error";
                response.Status = StatusCodes.Status500InternalServerError;
                response.Detail = "An unexpected error occurred";
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
```

Registration in Program.cs:

```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

Security Considerations:

- Never expose stack traces in production
- Never expose internal exception messages to clients
- Never expose connection strings or database details
- Log full exception details internally for debugging
- Include correlation IDs for tracing

Example:

```
GET

/api/v1/employees

POST

/api/v1/employees

GET

/api/v1/employees/{id}
```

---

# Versioning

APIs are versioned.

Example:

```
/api/v1/

```

Breaking changes require a new version.

---

# Testing Strategy

Every layer has its own tests.

Domain

- Unit Tests

Application

- Unit Tests

Infrastructure

- Integration Tests

API

- Integration Tests

Frontend

- Jest
- React Testing Library

User Flows

- Playwright

---

# Observability

Every service must expose:

- Health Checks
- Swagger
- Structured Logs

Future versions should include:

- OpenTelemetry
- Distributed Tracing
- Metrics

---

# Feature Flags

LaunchDarkly controls feature availability.

Feature Flags must be used for:

- Incremental rollout
- Experimental features
- A/B Testing

Business rules must not depend permanently on Feature Flags.

---

# Security

Always validate user input.

Never trust client-side validation.

Always authorize sensitive operations.

Protect against:

- Injection attacks
- Broken authentication
- Sensitive data exposure

Follow the OWASP Top 10 whenever applicable.

---

# Documentation

Every architectural decision must be documented through ADRs.

Every module must contain its own documentation.

Documentation evolves together with the software.

---

# Future Evolution

The architecture must support future implementation of:

- Event Bus
- Background Jobs
- File Storage
- AI Assistant
- Mobile Applications
- Public API
- Webhooks
- External Integrations

without requiring architectural redesign.

---

# Architectural Decision

Whenever multiple solutions are technically valid:

1. Prefer the simplest solution.
2. Prefer readability.
3. Prefer maintainability.
4. Prefer extensibility.
5. Optimize only after measuring.

Premature optimization is discouraged.

---

# Summary

Project Orkestra prioritizes:

- Clean Architecture
- Maintainability
- Modularity
- Scalability
- Testability
- Security
- Developer Experience

The software should be easy to understand, easy to test and easy to evolve for many years.
