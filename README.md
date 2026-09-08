# Project Orkestra

> Modern Workforce & Operations Management Platform

## Status

Under Development

## Vision

Project Orkestra is a modular, multi-tenant platform designed to simplify workforce, operations and business unit management for organizations with one or more business units.

### Current Stage

**Epic 3: Core Domain** - Complete ✅

Foundation includes:

- Tenant, Organization, BusinessUnit, Employee domain entities
- Application-layer use cases with comprehensive business logic
- Repository integration with MongoDB
- 75+ unit and integration tests with 100% pass rate

## Tech Stack

### Frontend

- React 18+
- TypeScript
- Vite
- CSS/Tailwind

### Backend

- ASP.NET Core (.NET 10)
- C# 14+
- MongoDB with MongoDB.Driver
- xUnit (testing)
- NSubstitute (mocking)

### Infrastructure

- Docker & Docker Compose
- MongoDB (database)
- Redis (cache, future)

## Quick Start

### Prerequisites

- Docker and Docker Compose
- .NET 10 SDK
- Node.js 18+

### Setup & Run

**1. Start MongoDB**

```bash
docker compose up -d
```

**2. Restore Dependencies**

```bash
cd backend
dotnet restore
```

**3. Build the Solution**

```bash
dotnet build
```

**4. Run Tests**

See [Testing](#testing) section below.

**5. Start the API**

```bash
dotnet run --project src/ProjectOrkestra.Api/ProjectOrkestra.Api.csproj
```

The API will be available at `https://localhost:5001/api/v1`

### Frontend Setup

```bash
cd frontend
npm install
npm run dev
```

The frontend will be available at `http://localhost:5173`

## Testing

Project Orkestra includes comprehensive tests at multiple layers.

### Unit Tests

Test application-layer use cases and domain logic:

```bash
cd backend
dotnet test tests/ProjectOrkestra.UnitTests/ProjectOrkestra.UnitTests.csproj
```

### Integration Tests

Test repository implementations with real MongoDB connection:

```bash
cd backend
dotnet test tests/ProjectOrkestra.IntegrationTests/ProjectOrkestra.IntegrationTests.csproj
```

**Note**: Integration tests require MongoDB running (via `docker compose up`)

### Run All Tests

```bash
cd backend
dotnet test
```

### Test Results

Current status: **31 Integration Tests + 51 Unit Tests = 82 Tests - All Passing ✅**

Tests cover:

- Organization creation, listing, renaming, status transitions
- Business Unit operations and filtering
- Employee management and status transitions
- Repository persistence and filtering
- MongoDB integration and data isolation

## Project Structure

```
project-orkestra/
├── ai/                          # AI documentation and guidance
│   ├── DOMAIN_CONTEXT.md       # Business domain definitions
│   ├── ARCHITECTURE_GUIDE.md   # Architectural principles
│   ├── CODING_STANDARDS.md     # Code style guidelines
│   └── ...
├── backend/
│   ├── src/
│   │   ├── ProjectOrkestra.Api/           # ASP.NET Core controllers
│   │   ├── ProjectOrkestra.Application/   # Use cases & business logic
│   │   ├── ProjectOrkestra.Domain/        # Domain entities & rules
│   │   └── ProjectOrkestra.Infrastructure # Repositories & external services
│   └── tests/
│       ├── ProjectOrkestra.UnitTests/       # Application layer tests
│       └── ProjectOrkestra.IntegrationTests # Repository integration tests
├── frontend/
│   ├── src/
│   │   ├── components/  # Reusable React components
│   │   ├── features/    # Feature modules
│   │   ├── pages/       # Page components
│   │   └── services/    # API communication
│   └── public/          # Static assets
└── docs/                # Project documentation
```

## Architecture

Project Orkestra follows **Clean Architecture** with a **Modular Monolith** approach:

### Layers

```
Presentation (Controllers)
       ↓
Application (Use Cases)
       ↓
Domain (Entities & Rules)
       ↓
Infrastructure (Repositories & Services)
```

### Key Principles

- **Business rules independent of frameworks** (Domain Layer constraint)
- **Dependencies point inward** (Dependency Injection only)
- **Multi-tenant by design** (Data isolation mandatory)
- **API versioning** (`/api/v1/...`, `/api/v2/...`)
- **Centralized exception handling** (ProblemDetails responses)

See [ai/ARCHITECTURE_GUIDE.md](ai/ARCHITECTURE_GUIDE.md) for detailed architectural patterns.

## Domain Model

The domain follows a hierarchical structure:

```
Tenant (root isolation boundary)
   ↓
Organization (legal entity, has CNPJ)
   ↓
BusinessUnit (physical location, has CNPJ)
   ↓
Employee (worker, belongs to exactly one BusinessUnit)
```

Key Design Decisions:

- **CNPJ at 3 levels** (Tenant, Organization, BusinessUnit) - enables complex organizational hierarchies
- **Employee-to-BusinessUnit**: 1-to-N relationship, not N-to-N - simplifies modeling, explicit transfers
- **No TenantId on Employee** - derived transitively through BusinessUnit → Organization → Tenant
- **Employee Status** - flexible list (Active, Inactive, Vacation, FreeDay, License)
- **Roles/Permissions** - future system for complex authorization patterns

See [ai/DOMAIN_CONTEXT.md](ai/DOMAIN_CONTEXT.md) for detailed domain definitions.

## API Endpoints

All endpoints follow RESTful conventions with `/api/v1/` versioning:

```
Organizations
  GET    /api/v1/organizations
  POST   /api/v1/organizations
  PUT    /api/v1/organizations/{id}
  DELETE /api/v1/organizations/{id}

Business Units
  GET    /api/v1/business-units
  POST   /api/v1/business-units
  PUT    /api/v1/business-units/{id}
  DELETE /api/v1/business-units/{id}

Employees
  GET    /api/v1/employees
  POST   /api/v1/employees
  PUT    /api/v1/employees/{id}
  DELETE /api/v1/employees/{id}
```

## Roadmap

1. **Foundation** (Sprint 0) - ✅ Complete
   - Core domain entities
   - Repository pattern
   - Application use cases

2. **MVP** (Upcoming)
   - API controllers
   - Authentication/Authorization
   - Frontend dashboard

3. **Workforce** (Future)
   - Employee management UI
   - Document storage
   - Certifications tracking

4. **Scheduling** (Future)
   - Shift management
   - Schedule generation
   - Coverage optimization

5. **Payroll** (Future)
   - Hours calculation
   - Payroll processing
   - Reports

6. **Reports** (Future)
   - Analytics dashboard
   - Export capabilities
   - Business intelligence
