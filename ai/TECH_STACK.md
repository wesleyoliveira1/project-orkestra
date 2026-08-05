---
title: Technology Stack
version: 1.0.0
status: Approved
owner: Project Orkestra
author: Wesley Oliveira
last_updated: 2026-08-05

related:
  - PROJECT_CONTEXT.md
  - ARCHITECTURE_GUIDE.md
  - CODING_STANDARDS.md
  - ENGINEERING_PRINCIPLES.md
---

# Technology Stack

## Purpose

This document defines the official technology stack adopted by Project Orkestra.

It explains which technologies are used, why they were selected, and how they should be applied throughout the project.

The technology stack should remain stable during the MVP unless there is a strong architectural justification for change.

---

# Technology Principles

Technologies are selected based on the following criteria:

- Maintainability
- Long-term support (LTS)
- Community adoption
- Developer experience
- Production readiness
- Scalability
- Alignment with the project's learning objectives

New technologies should only be introduced when they clearly solve an existing problem.

---

# High-Level Stack

| Layer          | Technology                                     |
| -------------- | ---------------------------------------------- |
| Frontend       | React 18                                       |
| Language       | TypeScript 5                                   |
| Build Tool     | Webpack                                        |
| Transpiler     | Babel                                          |
| Backend        | ASP.NET Core (.NET 8)                          |
| Language       | C#                                             |
| Database       | MongoDB                                        |
| Cache          | Redis                                          |
| Authentication | JWT Bearer + OIDC                              |
| Testing        | Jest, React Testing Library, xUnit, Playwright |
| Documentation  | Swagger (OpenAPI)                              |
| Infrastructure | Docker                                         |
| CI/CD          | GitHub Actions                                 |
| Source Control | Git + GitHub                                   |

---

# Frontend

## React 18

Purpose

Build modern, component-based user interfaces.

Reasons for selection

- Mature ecosystem
- Excellent TypeScript support
- Component architecture
- Large community
- Industry standard
- Alignment with professional experience

Guidelines

- Functional Components only.
- Composition over inheritance.
- Small reusable components.
- Business logic outside UI.

---

## TypeScript 5

Purpose

Provide static typing and improve code quality.

Reasons for selection

- Better developer experience
- Compile-time validation
- Safer refactoring
- Improved maintainability

Guidelines

- Avoid `any`.
- Prefer explicit types.
- Use interfaces and type aliases appropriately.
- Enable strict mode.

---

## Webpack

Purpose

Bundle frontend assets.

Reasons for selection

- Matches current professional environment.
- Mature ecosystem.
- Highly configurable.

Future versions may evaluate Vite if justified.

---

## Babel

Purpose

Transpile modern JavaScript and TypeScript features.

Babel should remain a build concern only.

Application code should not depend on Babel-specific features.

---

# Frontend State Management

## React Context

Used for:

- Authentication
- Logged-in user
- Global application configuration
- Theme
- Localization

Avoid using Context as a general-purpose state store.

---

## Jotai

Used for lightweight shared state.

Examples

- Wizards
- Multi-step forms
- Filters
- UI state
- Temporary workflow state

Prefer Jotai over Context for local shared state.

---

# Feature Flags

## LaunchDarkly

Purpose

Enable runtime feature management.

Primary use cases

- Incremental rollout
- Beta features
- A/B testing
- Safe deployments

Business logic should not permanently depend on Feature Flags.

An abstraction layer should be created to allow future replacement if necessary.

---

# Routing

## React Router DOM

Responsible for:

- Navigation
- Protected routes
- Nested layouts
- Lazy loading

Follow route-based code organization whenever practical.

---

# Backend

## ASP.NET Core (.NET 8)

Purpose

Build RESTful APIs and backend services.

Reasons for selection

- LTS
- High performance
- Strong dependency injection
- Excellent tooling
- Enterprise adoption

The API follows Clean Architecture principles.

---

## C#

Primary backend language.

Use modern language features compatible with .NET 8.

Examples

- Records
- Pattern Matching
- Nullable Reference Types
- Async/Await
- Primary Constructors (when appropriate)

---

# Database

## MongoDB

Purpose

Primary persistence layer.

Reasons for selection

- Flexible document model
- Fast development
- Excellent .NET integration
- Suitable for evolving domain models

Use MongoDB.Driver.

Avoid exposing MongoDB documents directly to the Application layer.

---

# Cache

## Redis

Purpose

Distributed caching.

Use cases

- Frequently accessed data
- User sessions
- Temporary values
- Performance optimization

Redis should never become the primary data source.

---

# Authentication

## JWT Bearer

Responsible for API authentication.

Tokens should contain only essential claims.

Never expose sensitive information.

---

## OpenID Connect (OIDC)

Responsible for user authentication.

The application should support external Identity Providers.

Authentication should remain provider-agnostic whenever possible.

---

# API Documentation

## Swagger / OpenAPI

Purpose

Document REST APIs.

Swagger should expose:

- Endpoints
- Request models
- Response models
- Authentication
- Status codes

Swagger is not a replacement for business documentation.

---

# Serialization

## Newtonsoft.Json

Used because of compatibility with MongoDB and existing enterprise patterns.

Use custom converters only when necessary.

Serialization rules should remain centralized.

---

# Testing

## xUnit

Backend unit testing framework.

Used for

- Domain
- Application
- Business rules

---

## Jest

Frontend unit testing framework.

Used for

- Components
- Hooks
- Utilities

---

## React Testing Library

Used to validate UI behavior from the user's perspective.

Avoid testing implementation details.

---

## Playwright

Primary end-to-end testing framework.

Used for critical user journeys.

Examples

- Login
- Employee creation
- Role management
- Schedule visualization

Prefer Playwright over Selenium.

---

# Infrastructure

## Docker

Purpose

Provide consistent development and deployment environments.

Containers should be created for:

- Backend
- Frontend
- MongoDB
- Redis

Development and production environments should remain as similar as possible.

---

# CI/CD

## GitHub Actions

Responsible for:

- Build
- Tests
- Lint
- Formatting validation
- Docker image generation
- Deployment

Pipeline should execute automatically for Pull Requests and merges into the main branch.

---

# Version Control

## Git

Branch strategy

GitHub Flow

Commit strategy

Conventional Commits

Pull Requests are mandatory before merging into `main`.

---

# Development Environment

Recommended tools

IDE

- Visual Studio Code

Extensions

- GitHub Copilot
- C# Dev Kit
- ESLint
- Prettier
- Docker
- Playwright
- GitLens

---

# Code Quality

Recommended tools

Frontend

- ESLint
- Prettier

Backend

- EditorConfig
- dotnet format

All formatting checks should run automatically in CI.

---

# Future Technologies

The following technologies may be introduced in future releases if justified.

Observability

- OpenTelemetry
- Grafana
- Prometheus

Messaging

- RabbitMQ
- Azure Service Bus

Background Processing

- Hangfire
- Quartz.NET

Cloud

- Azure
- AWS

Storage

- Azure Blob Storage
- Amazon S3

Search

- Elasticsearch

Realtime

- SignalR

These technologies should only be adopted when they solve concrete business or architectural needs.

---

# Technology Evaluation

Before introducing a new technology, evaluate:

- Does it solve a real problem?
- Is it production ready?
- Is it actively maintained?
- Does it reduce complexity?
- Does the team understand it?
- Does it fit the current architecture?

Avoid introducing technologies solely because they are popular.

---

# Technology Evolution

Project Orkestra values stability.

Technology changes should be evolutionary rather than disruptive.

Whenever a technology is replaced:

- Document the reason.
- Evaluate migration cost.
- Consider long-term maintenance.
- Update all related documentation.

---

# Summary

Project Orkestra intentionally adopts a modern but conservative technology stack.

The objective is not to use the newest technologies available, but to build a maintainable, scalable and production-ready application while reinforcing professional software engineering practices.

Technology should always support the architecture and the business domain—not define them.
