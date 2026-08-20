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

| Layer          | Technology                                       |
| -------------- | ------------------------------------------------ |
| Frontend       | React 18                                         |
| Language       | TypeScript 5                                     |
| Build Tool     | Vite                                             |
| Transpiler     | SWC                                              |
| Backend        | ASP.NET Core (.NET 10)                           |
| Language       | C#                                               |
| Database       | MongoDB                                          |
| Cache          | Redis                                            |
| Authentication | JWT Bearer + OIDC                                |
| Testing        | Vitest, React Testing Library, Playwright, xUnit |
| Documentation  | Swagger (OpenAPI)                                |
| Infrastructure | Docker                                           |
| CI/CD          | GitHub Actions                                   |
| Source Control | Git + GitHub                                     |

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

## Vite

Purpose

Provide a fast, modern development and build experience for the frontend.

Reasons for selection

- Excellent performance for local development.
- Minimal configuration by default.
- First-class support for modern frontend frameworks.
- Easy integration with SWC and TailwindCSS.

Guidelines

- Use Vite project structure for the React application.
- Prefer Vite plugins for build-time integrations.

---

## SWC

Purpose

Enable fast TypeScript and JavaScript compilation.

Reasons for selection

- High build performance.
- Good compatibility with Vite.
- Reduces development feedback loop time.

Guidelines

- Use SWC for transpilation and bundling optimizations.
- Avoid relying on Babel-specific extensions.

---

## React Router

Purpose

Handle client-side navigation and route composition.

Reasons for selection

- Widely adopted and stable.
- Supports nested routes and route guards.
- Works well with React lazy loading.

Guidelines

- Organize routes using nested layouts.
- Keep route components small and focused.

---

## TanStack Query

Purpose

Manage server state and data fetching.

Reasons for selection

- Declarative data fetching.
- Built-in caching and request deduplication.
- Strong support for optimistic updates and polling.

Guidelines

- Use TanStack Query for API calls and server-state management.
- Keep queries and mutations close to domain features.

---

## Axios

Purpose

Perform HTTP requests from the frontend.

Reasons for selection

- Simple API for RESTful communication.
- Easy request/response interception.
- Works well with React Query.

Guidelines

- Use a centralized Axios client for API integration.
- Configure interceptors for auth and error handling.

---

## Jotai

Purpose

Manage lightweight shared state in the UI.

Reasons for selection

- Minimal and composable state management.
- Works well for local UI state.
- Complements, not replaces, React Context.

Guidelines

- Use Jotai for state shared across small component trees.
- Prefer React Context for global application state.

---

## React Context

Used for:

- Authentication
- Logged-in user
- Global application configuration
- Theme
- Localization

Avoid using Context as a general-purpose state store.

---

## React Hook Form

Purpose

Build performant, declarative form handling.

Reasons for selection

- Excellent performance for complex forms.
- Minimal re-renders.
- Strong TypeScript support.

Guidelines

- Use React Hook Form for form state and validation.
- Integrate Zod for schema validation.

---

## Zod

Purpose

Validate and parse form input and API payloads.

Reasons for selection

- Type-safe schema validation.
- Ergonomic integration with React Hook Form.
- Good developer experience.

Guidelines

- Use Zod schemas for form validation and API contract validation.
- Keep schemas close to the related feature.

---

## TailwindCSS

Purpose

Implement utility-first styling.

Reasons for selection

- Fast UI development.
- Consistent design system.
- Good integration with Vite.

Guidelines

- Use Tailwind utility classes for layout and styling.
- Prefer component-level abstractions for repeated patterns.

---

## shadcn/ui

Purpose

Provide a modern component library and design system.

Reasons for selection

- Ready-to-use accessible UI components.
- Works well with TailwindCSS.
- Accelerates UI development.

Guidelines

- Use shadcn/ui for common UI components.
- Extend components when customization is required.

---

## Lucide

Purpose

Provide iconography for the frontend.

Reasons for selection

- Simple React icon components.
- Lightweight and extensible.

Guidelines

- Use Lucide icons consistently across the UI.
- Prefer semantic icons for actions and status.

---

## TanStack Table

Purpose

Build data tables and grids.

Reasons for selection

- Flexible table rendering.
- Fine-grained control over columns and sorting.
- Works with React and TypeScript.

Guidelines

- Use TanStack Table for complex tabular interfaces.
- Keep table logic separate from presentation.

---

## FullCalendar

Purpose

Render calendar views and scheduling interfaces.

Reasons for selection

- Rich calendar UI components.
- Good support for events and dragging.

Guidelines

- Use FullCalendar for schedule visualization.
- Keep event data and calendar configuration decoupled.

---

## Recharts

Purpose

Display charts and dashboards.

Reasons for selection

- Declarative chart components.
- Good TypeScript support.

Guidelines

- Use Recharts for data visualization and metrics.
- Keep chart configuration and data transformation separated.

---

## Vitest

Purpose

Unit test the frontend.

Reasons for selection

- Fast test execution.
- Vite-native testing experience.

Guidelines

- Use Vitest for component and utility tests.
- Keep tests small and focused.

---

## React Testing Library

Purpose

Test React components from the user's perspective.

Reasons for selection

- Encourages accessibility-aware tests.
- Focuses on behavior over implementation.

Guidelines

- Use RTL for component integration tests.
- Prefer queries that resemble user interactions.

---

## Playwright

Purpose

End-to-end test user flows.

Reasons for selection

- Reliable browser automation.
- Cross-browser testing.

Guidelines

- Use Playwright for critical acceptance scenarios.
- Keep tests stable and maintainable.

---

## ESLint

Purpose

Enforce consistent code quality.

Reasons for selection

- Static analysis for JavaScript and TypeScript.
- Customizable rules.

Guidelines

- Use ESLint with recommended React and TypeScript rules.
- Fix lint issues as part of implementation.

---

## Prettier

Purpose

Ensure consistent code formatting.

Reasons for selection

- Enforces style without debate.
- Works with TypeScript and CSS.

Guidelines

- Use Prettier as the formatter for all frontend files.
- Integrate with ESLint where possible.

---

## Husky

Purpose

Run git hooks for quality checks.

Reasons for selection

- Prevent problematic commits.
- Enforce pre-commit workflows.

Guidelines

- Use Husky to run lint-staged and tests before commit.

---

## lint-staged

Purpose

Run checks only on staged files.

Reasons for selection

- Fast pre-commit validation.
- Reduces noise from full repo checks.

Guidelines

- Use lint-staged with ESLint and Prettier for changed files.

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

## ASP.NET Core (.NET 10)

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

Use modern language features compatible with .NET 10.

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
