---
title: Documentation Guide
version: 1.0.0
status: Approved
owner: Project Orkestra
author: Wesley Oliveira
last_updated: 2026-08-05

related:
  - DEVELOPMENT_WORKFLOW.md
  - ARCHITECTURE_GUIDE.md
  - CODING_STANDARDS.md
  - AI_ASSISTANT_GUIDE.md
---

# Documentation Guide

## Purpose

This document defines the documentation standards adopted by Project Orkestra.

Documentation is considered part of the software.

A feature is only complete when its implementation and documentation accurately represent the current behavior of the system.

This guide applies to both human contributors and AI assistants.

---

# Documentation Principles

Documentation should be:

- Accurate
- Clear
- Concise
- Up to date
- Easy to navigate
- Easy to maintain

Documentation exists to communicate knowledge, not to satisfy a process.

---

# Documentation Philosophy

Documentation should answer the following questions:

- What does this feature do?
- Why does it exist?
- How does it work?
- How should it be used?
- What are its limitations?

If documentation cannot answer these questions, it is incomplete.

---

# Documentation Language

All documentation must be written in English.

Exceptions:

- Legal documents.
- Third-party documentation.
- External references written in another language.

---

# Documentation Format

Documentation should use Markdown.

Every document should include:

- Clear title
- Purpose
- Scope
- Relevant sections
- References (when applicable)

Use headings to organize content.

Avoid large blocks of text.

---

# Documentation Structure

Project documentation is organized under the `docs/` directory.

Example:

```
docs/

ai/

architecture/

adr/

backend/

frontend/

api/

product/

deployment/

runbooks/
```

Each directory has a specific responsibility.

---

# Types of Documentation

Project Orkestra distinguishes different documentation categories.

## Product Documentation

Describes:

- Vision
- Goals
- Personas
- Roadmap
- Features

Audience:

Developers and stakeholders.

---

## Architecture Documentation

Describes:

- System architecture
- Module responsibilities
- Design decisions
- Dependencies

Audience:

Software engineers.

---

## API Documentation

Describes:

- Endpoints
- Requests
- Responses
- Authentication
- Error codes

Swagger is the primary API reference.

Additional documentation should explain business behavior rather than endpoint syntax.

---

## AI Documentation

Provides context for AI assistants.

Examples:

- Architecture Guide
- Coding Standards
- Engineering Principles

Its purpose is to improve consistency across AI-generated contributions.

---

## ADR (Architecture Decision Records)

Significant architectural decisions should be recorded as ADRs.

Examples:

- Why MongoDB?
- Why Clean Architecture?
- Why Modular Monolith?

Each ADR should describe:

- Context
- Decision
- Consequences
- Alternatives considered

---

## Runbooks

Runbooks describe operational procedures.

Examples:

- Local environment setup
- Deployment
- Incident recovery
- Database restore
- Cache invalidation

Runbooks are operational documents.

---

# Documentation by Development Phase

Every phase produces documentation.

## Discovery

- Product notes
- Business requirements

---

## Architecture

- Architecture diagrams
- ADRs
- Technical decisions

---

## Development

- Code comments (only when necessary)
- API documentation
- README updates

---

## Deployment

- Deployment guide
- Environment variables
- Infrastructure documentation

---

# README Standards

Every major directory should contain a README.md.

Example:

```
backend/

README.md

frontend/

README.md

docs/

README.md
```

README files should explain:

- Purpose
- Structure
- How to use

Avoid implementation details.

---

# Code Comments

Prefer self-explanatory code.

Comments should explain:

- Why
- Business rules
- Trade-offs

Do not explain obvious code.

Bad:

```csharp
// Increment counter
counter++;
```

Good:

```csharp
// Weekend shifts follow the company's rotating schedule policy.
```

---

# Diagrams

Prefer diagrams when explaining architecture.

Recommended diagram types:

- C4 Model
- Sequence Diagrams
- Flowcharts
- Entity Relationship Diagrams

Diagrams should complement documentation rather than replace it.

---

# Screenshots

Screenshots may be included when documenting:

- User interfaces
- Workflows
- Dashboards

Keep screenshots up to date.

Remove outdated images.

---

# Images

Store documentation assets under:

```
docs/assets/
```

Suggested structure:

```
assets/

images/

diagrams/

screenshots/

logos/
```

Avoid embedding large binary files elsewhere in the repository.

---

# API Documentation

Swagger provides endpoint documentation.

Additional documentation should explain:

- Business rules
- Authorization
- Usage examples
- Common scenarios

Do not duplicate Swagger unnecessarily.

---

# Architecture Documentation

Architecture documentation should evolve with the system.

Whenever architecture changes:

- Update diagrams.
- Update Architecture Guide.
- Create ADRs when appropriate.

Architecture documentation should never become outdated.

---

# Documentation Ownership

The developer implementing a feature is responsible for updating its documentation.

Documentation ownership belongs to the implementation, not to a separate team.

---

# AI-Generated Documentation

AI may assist with documentation.

However:

- Verify technical accuracy.
- Remove redundant information.
- Ensure consistency.
- Review examples.

Documentation generated by AI must follow the same quality standards as human-written documentation.

---

# Versioning

Documentation evolves together with the software.

Major architectural changes should update:

- Version
- Date
- Related documents

Avoid maintaining multiple conflicting versions of the same document.

---

# Documentation Review

Documentation should be reviewed together with the code.

Review checklist:

- Is it accurate?
- Is it complete?
- Is it understandable?
- Is it still relevant?
- Does it reflect the implementation?

---

# Pull Requests

Documentation updates should be included in the same Pull Request as the implementation whenever possible.

Avoid separate documentation-only Pull Requests for feature work.

Documentation and implementation should remain synchronized.

---

# Documentation Checklist

Before merging a feature, verify:

- README updated (if necessary).
- API documentation updated.
- Architecture documentation updated.
- AI documentation updated.
- Examples updated.
- Screenshots updated (if applicable).

---

# Definition of Good Documentation

Good documentation is:

- Accurate
- Current
- Useful
- Discoverable
- Easy to maintain

It should reduce the need for verbal explanations.

---

# Documentation Anti-Patterns

Avoid:

- Outdated documentation.
- Duplicate information.
- Overly verbose explanations.
- Empty templates.
- Unmaintained diagrams.
- Large documents mixing unrelated topics.

Documentation should be modular and focused.

---

# Continuous Improvement

Documentation is a living asset.

Whenever new knowledge is gained:

- Improve existing documents.
- Clarify ambiguous sections.
- Remove obsolete information.
- Keep the documentation aligned with the software.

Documentation should evolve at the same pace as the project.

---

# Final Principle

If a developer cannot understand a feature by reading the documentation, the documentation has failed.

Project Orkestra values documentation as an integral part of software engineering, ensuring that knowledge remains accessible, maintainable and transferable throughout the lifetime of the project.
