---
title: Coding Standards
version: 1.0.0
status: Approved
owner: Project Orkestra
author: Wesley Oliveira
last_updated: 2026-08-05

related:
  - ARCHITECTURE_GUIDE.md
  - ENGINEERING_PRINCIPLES.md
  - DEVELOPMENT_WORKFLOW.md
  - COPILOT_INSTRUCTIONS.md
---

# Coding Standards

## Purpose

This document defines the coding conventions adopted by Project Orkestra.

The objective is to ensure that every code contribution is:

- Readable
- Maintainable
- Consistent
- Testable
- Scalable

These standards apply to both human developers and AI-generated code.

---

# General Principles

Code is read far more often than it is written.

Always optimize for readability.

Prefer explicit code over clever code.

Write code that another developer can understand in six months without additional explanations.

---

# Naming

Names should clearly communicate intent.

Avoid abbreviations.

Good

```
EmployeeRepository
CalculateWorkingDays
OrganizationService
```

Bad

```
EmpRepo
CalcWD
OrgSvc
```

---

# Files

One public class per file.

File name must match the main type.

Good

```
Employee.cs

EmployeeRepository.cs

EmployeeController.cs
```

---

# Classes

Classes should have a single responsibility.

Avoid God Objects.

Prefer small cohesive classes.

---

# Methods

Methods should do one thing.

Recommended maximum size:

20–30 lines.

If a method becomes difficult to understand, split it.

---

# Parameters

Avoid methods with many parameters.

Maximum recommended:

```
3 parameters
```

If more data is required, create an object.

Example

Good

```
CreateEmployee(CreateEmployeeRequest request)
```

Instead of

```
CreateEmployee(
    string firstName,
    string lastName,
    DateTime birthDate,
    string phone,
    string email,
    ...
)
```

---

# Variables

Prefer meaningful variable names.

Good

```
employee

workingDays

availableShift
```

Bad

```
x

temp

obj

data
```

---

# Constants

Avoid magic numbers.

Bad

```
salary += 500;
```

Good

```
const decimal BonusAmount = 500;
```

---

# Comments

Code should explain itself whenever possible.

Use comments only to explain:

- Why
- Trade-offs
- Business rules

Do not explain obvious code.

Bad

```
// Increment i
i++;
```

Good

```
// Employee bonus follows company policy introduced in 2026.
```

---

# Regions

Do not use #region.

Prefer small files instead.

---

# Nullability

Nullable Reference Types must remain enabled.

Never suppress warnings without justification.

Prefer explicit null handling.

---

# Exception Handling

Never swallow exceptions.

Bad

```
catch
{
}
```

Good

```
catch(Exception ex)
{
    logger.LogError(ex, ...);
    throw;
}
```

---

# Asynchronous Programming

Prefer async/await.

Never block asynchronous code.

Avoid:

```
.Result

.Wait()
```

Prefer

```
await
```

---

# Dependency Injection

Always use Constructor Injection.

Avoid property injection.

Never use Service Locator.

---

# Interfaces

Depend on abstractions.

Example

```
IEmployeeRepository
```

Instead of

```
MongoEmployeeRepository
```

---

# Dependency Direction

High-level modules must never depend on low-level modules.

Follow Clean Architecture dependency rules.

---

# C# Standards

## Language Version

Use the latest stable version supported by .NET 8.

---

## Namespaces

Use file-scoped namespaces.

Good

```
namespace ProjectOrkestra.Application.Employees;
```

---

## Access Modifiers

Always declare access modifiers explicitly.

Avoid implicit accessibility.

---

# Properties

Prefer init properties whenever possible.

Use records for immutable data.

---

# Records

Use records for:

- DTOs
- Commands
- Queries
- Responses

Use classes for:

- Entities
- Services
- Repositories

---

# Enums

Prefer enums only for stable values.

If values become configurable, use entities instead.

---

# Extension Methods

Use sparingly.

Do not hide business logic inside extension methods.

---

# LINQ

Prefer readable LINQ.

Avoid nested LINQ expressions.

Prefer multiple statements over unreadable chains.

---

# React Standards

Use Functional Components only.

Do not create Class Components.

---

# Component Size

Keep components focused.

Recommended maximum:

200 lines.

Extract child components whenever appropriate.

---

# Hooks

Custom Hooks should encapsulate reusable logic.

Avoid duplicated useEffect logic.

---

# State Management

React Context

Use only for:

- Authentication
- Logged User
- Theme
- Global Configuration

Jotai

Use for:

- Local shared state
- Wizards
- Temporary state

Avoid using Context as a global store.

---

# Props

Prefer explicit props.

Avoid passing unnecessary data.

---

# Components

Prefer composition over inheritance.

Keep components reusable.

---

# Styling

Prefer modular styling.

Avoid inline styles except for dynamic values.

---

# TypeScript

Never use

```
any
```

Prefer

```
unknown
```

when type is uncertain.

Always define interfaces.

---

# Folder Organization

Frontend

```
components/

pages/

hooks/

contexts/

atoms/

services/

types/

utils/
```

Backend

```
Api/

Application/

Domain/

Infrastructure/
```

---

# REST API Standards

Use nouns.

Good

```
/employees
```

Bad

```
/getEmployees
```

---

Return appropriate HTTP status codes.

200

201

204

400

401

403

404

409

422

500

---

# Logging

Use structured logging.

Never log:

- Passwords
- JWT Tokens
- Personal sensitive data

---

# Testing

Every feature must have tests.

Backend

- Unit Tests
- Integration Tests

Frontend

- Jest
- React Testing Library

E2E

- Playwright

Bug fixes should include regression tests whenever applicable.

---

# Git

Follow Conventional Commits.

Examples

```
feat:

fix:

docs:

refactor:

test:

build:

ci:

perf:

style:

chore:
```

---

# Pull Requests

A Pull Request should be:

- Small
- Focused
- Reviewable

Avoid mixing unrelated changes.

---

# AI Generated Code

AI-generated code is never trusted automatically.

Every generated code must be:

- Reviewed
- Understood
- Tested
- Refactored when necessary

Never merge code that you do not understand.

---

# Code Review Checklist

Before merging, verify:

- Naming is clear.
- No duplicated logic.
- No architectural violations.
- Business rules are respected.
- Tests pass.
- Documentation updated.
- No dead code.
- No commented code.
- No TODO left behind.

---

# Definition of Clean Code

A clean codebase is:

- Easy to read
- Easy to change
- Easy to test
- Easy to review
- Easy to extend

Project Orkestra values maintainability over clever implementations.

Every contribution should leave the codebase better than it was found.
