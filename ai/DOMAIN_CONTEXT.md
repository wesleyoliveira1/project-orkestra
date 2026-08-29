---
title: Domain Context
version: 2.0.0
status: Approved
owner: Project Orkestra
author: Wesley Oliveira
last_updated: 2026-08-29

related:
  - PROJECT_CONTEXT.md
  - ARCHITECTURE_GUIDE.md
  - ENGINEERING_PRINCIPLES.md
---

# Domain Context

## Purpose

This document defines the business domain of Project Orkestra.

It establishes the ubiquitous language shared by developers, product owners and AI assistants.

Every business rule implemented in the system should be based on the concepts described here.

---

# Domain Overview

Project Orkestra is a multi-tenant workforce and operations management platform.

Its primary objective is to simplify employee management, scheduling, organizational structure and operational activities for companies with one or more business units.

The platform is intentionally domain-agnostic, allowing organizations from different industries to use the same operational model.

Examples include:

- Pharmacies
- Retail stores
- Restaurants
- Clinics
- Offices
- Service providers
- Franchises

---

# Core Business Concepts

The following concepts define the ubiquitous language of Project Orkestra.

---

# Tenant

A Tenant represents an isolated customer within the SaaS platform.

Each tenant owns all its data.

Examples:

- Company A
- Company B

Tenants never share data.

Responsibilities:

- Data isolation
- Licensing
- Subscription
- Global configuration

Attributes:

- **CNPJ**: Unique Brazilian business registration for the Tenant (root legal entity)

---

# Organization

An Organization represents a company operating inside a Tenant.

Each Organization has its own CNPJ.

Organizations within the same Tenant may represent different legal entities within the same group or business structure.

Responsibilities:

- Business information
- Business Units management
- Organizational structure
- Roles
- Operational settings

Relationships:

Tenant (1) → (N) Organization

Attributes:

- **CNPJ**: Unique Brazilian business registration for the Organization
- **Status**: Active or Inactive

---

# Business Unit

A Business Unit represents a physical location or operational branch.

Examples:

- Store
- Pharmacy
- Office
- Clinic
- Warehouse

Each Business Unit belongs to exactly one Organization.

Each Business Unit has its own CNPJ.

## Matrix vs. Branch Concept

In Brazilian business structure, a "matriz" (matrix/headquarters) is the primary location and a "filial" (branch) is a secondary location.

In Project Orkestra, this concept is implemented through **CNPJ structure**:

- The **matriz** is a Business Unit with CNPJ ending in `/0001` (standard suffix for headquarters)
- Each **filial** is a Business Unit with its own unique CNPJ (ending in `/0002`, `/0003`, etc.)

Why CNPJ at 3 levels?

- **Tenant CNPJ**: Root legal entity of the SaaS customer (may not change often)
- **Organization CNPJ**: Separate legal entity within the Tenant group (e.g., different holding companies or business structures)
- **Business Unit CNPJ**: Individual location/branch CNPJ (enables local compliance, taxation, and operational independence)

This structure allows organizations with complex hierarchies to accurately represent their legal structure in the system.

Responsibilities:

- Employee allocation
- Local schedules
- Local holidays
- Operational metrics
- Local business rules

Relationships:

Organization (1) → (N) BusinessUnit

Attributes:

- **CNPJ**: Unique Brazilian business registration for the Business Unit
- **Status**: Active or Inactive

---

# Employee

Represents a worker employed by a Business Unit.

## Employee Assignment Model

Each Employee belongs to **exactly one Business Unit** through the `BusinessUnitId` field.

This is a deliberate architectural decision:

- **Single Assignment**: An employee has one primary business unit assignment at any given time
- **No Many-to-Many Relationship**: The relationship between Employee and BusinessUnit is 1-to-N only
- **Explicit Transfer Operation**: If an employee transfers to a different business unit, this is an explicit operation (`TransferToBusinessUnit`) that updates the `BusinessUnitId`
- **Temporary Coverage**: Sporadic or temporary work across multiple business units (e.g., covering for a colleague at another location) is **not modeled in the core Employee entity**. This concern is deferred to **Epic 4: Scheduling**, where shift-level assignments will handle temporary coverage through the scheduling system.

## Tenant Access Pattern

Employee does **not** store a `TenantId` directly.

Instead, the Tenant is derived transitively:

```
Employee.BusinessUnitId
  ↓
BusinessUnit.OrganizationId
  ↓
Organization.TenantId
```

This design:

- Reduces data duplication
- Ensures data consistency (Employee always belongs to the Organization of its BusinessUnit)
- Simplifies migrations and transfers
- Prevents orphaned records

Repository queries that need tenant filtering should perform the transitive join.

## Employment Status

Employee status is flexible and supports multiple states:

- **Active**: Employee is working normally
- **Inactive**: Employee is not part of the company (terminated, retired)
- **Vacation**: Employee is on planned vacation
- **FreeDay**: Employee has a day off (used for flexible scheduling)
- **License**: Employee is on special leave (medical, sabbatical, etc.)

Filtering employees by status is always possible. Queries accepting `EmployeeStatus` parameters should accept a **list of statuses** (not a single value) to enable flexible filtering. The default filter is `[Active]`.

## Role and Permission (Future)

Employee does not directly model authorization roles or responsibilities.

Usage patterns like "Manager who supervises multiple Business Units" or "Lead who coordinates across locations" will be solved by a dedicated **Role and Permission system** (planned for a later epic).

This keeps Employee modeling simple and allows permission structures to evolve independently.

Employee stores information such as:

- Personal information (Name, CPF, Email, Phone, Address)
- Employment details (Status, Hire Date)
- Business Unit Assignment
- Working shift
- Position
- Assigned manager (if applicable)

Relationships:

BusinessUnit (1) → (N) Employee

Future versions may include:

- Documents
- Certifications
- Emergency contacts
- Payroll integration

---

# Role

Represents an authorization profile inside the application.

Initial roles:

Platform Admin

Organization Admin

Manager

Employee

Roles define responsibilities.

Permissions define actions.

---

# Permission

Represents an action allowed inside the system.

Examples:

Create Employee

Edit Employee

Delete Employee

Create Schedule

Approve Schedule

View Dashboard

Generate Reports

Permissions are assigned through Roles.

---

# Team

A Team groups employees working together.

Examples:

Morning Team

Night Team

Customer Service

Warehouse

A Team is optional.

Employees may belong to one or more Teams in future versions.

---

# Position

Represents the employee's professional function.

Examples:

Manager

Cashier

Sales Associate

Pharmacist

Supervisor

Position describes work responsibility.

It does not define permissions.

---

# Shift

Represents the employee's standard working period.

Examples:

Morning

Afternoon

Night

Custom

A Shift defines expected working hours.

---

# Work Schedule

Defines how an employee works over time.

Examples:

Monday to Friday

Weekend Rotation

Night Rotation

Flexible Schedule

Schedules are generated based on business rules.

---

# Rotation Cycle

Represents recurring scheduling logic.

Example:

One weekend working

Two weekends off

Repeat

Future versions may support custom rotation engines.

---

# Calendar

Represents the visual scheduling interface.

Displays:

- Working days
- Days off
- Holidays
- Vacations
- Shift assignments

The Calendar does not generate schedules.

It visualizes schedules.

---

# Holiday

Represents a non-working day.

Holiday behavior depends on organizational policies.

Example:

If an employee's scheduled day off falls on a holiday, the day off may be automatically reassigned.

Holiday policies are configurable.

---

# Leave

Represents temporary employee absence.

Examples:

Vacation

Medical Leave

Personal Leave

Training

Leaves affect scheduling.

---

# Attendance Event

Represents any attendance-related occurrence.

Examples:

Absence

Medical Certificate

Late Arrival

Extra Hours

Early Leave

Attendance Events may generate payroll adjustments in future versions.

---

# Payroll Adjustment

Represents any modification applied to employee compensation.

Examples:

Bonus

Absence Deduction

Cash Register Difference

Commission

Penalty

Payroll calculations are outside the MVP.

Only adjustment records are managed initially.

---

# Notification

Represents a system-generated communication.

Examples:

Schedule Updated

Employee Created

Holiday Changed

Approval Required

Notifications may be delivered through:

- In-App
- Email
- Push Notifications
- External integrations

---

# Dashboard

Represents operational metrics.

Examples:

Total Employees

Employees on Leave

Upcoming Holidays

Shift Distribution

Business Unit Overview

Financial dashboards are outside the MVP.

---

# Audit Log

Represents immutable records of important actions.

Examples:

Employee Created

Role Updated

Schedule Approved

Business Unit Deleted

Audit Logs should never be edited.

---

# Feature Flag

Represents runtime feature availability.

Used for:

- Gradual rollout
- Experimental features
- Beta testing

Business rules must not permanently depend on Feature Flags.

---

# Domain Relationships

The current business hierarchy is:

```
Tenant
    └── Organization
            ├── Business Unit
            │       ├── Team
            │       ├── Employee
            │       └── Schedule
            │
            ├── Roles
            ├── Permissions
            └── Dashboard
```

---

# Business Rules

## Organization

An Organization must belong to one Tenant.

---

## Business Unit

Every Business Unit belongs to exactly one Organization.

---

## Employee

Every Employee belongs to one Organization.

An Employee may be assigned to one or more Business Units.

---

## Roles

Roles define permissions.

Employees may have only one active role in the MVP.

Future versions may support multiple roles.

---

## Scheduling

Schedules are generated according to predefined business rules.

Manual adjustments are allowed only for authorized users.

---

## Calendar

Calendar data is derived from schedules.

The Calendar is not responsible for business decisions.

---

## Permissions

Permissions are granted through Roles.

Permissions are never assigned directly to employees in the MVP.

---

## Holidays

Holiday behavior depends on organizational configuration.

Organizations may define local holidays.

---

## Notifications

Notifications should never modify business state.

They only communicate events.

---

# Domain Events (Future)

Future versions may introduce domain events such as:

EmployeeCreated

EmployeeUpdated

EmployeeTransferred

ScheduleGenerated

ScheduleApproved

LeaveRequested

LeaveApproved

BusinessUnitCreated

RoleAssigned

NotificationSent

These events may later support integrations and background processing.

---

# Domain Boundaries

Project Orkestra intentionally does not include:

- ERP
- Accounting
- Inventory
- CRM
- Sales
- Purchasing
- Financial Management

These systems may integrate with Orkestra in the future.

---

# Ubiquitous Language

The following terms should always be used consistently:

Employee

Organization

Business Unit

Shift

Schedule

Role

Permission

Leave

Holiday

Dashboard

Notification

Audit Log

Avoid introducing synonyms that create ambiguity.

---

# Domain Principles

Business rules belong to the Domain.

Technical implementation details must never influence domain terminology.

The domain model should remain stable even if frameworks, databases or infrastructure change.

The business language is the foundation upon which the entire architecture of Project Orkestra is built.
