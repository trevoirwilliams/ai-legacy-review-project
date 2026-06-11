# ClaimDesk Operations API

ClaimDesk Operations API is a deliberately imperfect business application used for professional AI-assisted legacy code review training.

The system models an internal insurance claims workflow for a fictional property insurance company. It is intentionally framed as an inherited line-of-business application: it runs, it has real business behavior, and it contains enough debt, security concerns, and modernization opportunities to support a full review-first course.

## Business scenario

ClaimDesk supports claim intake, claim assignment, adjuster activity notes, document metadata, simple claim status transitions, and legacy reporting/export workflows.

The codebase includes intentional review targets for:

- codebase understanding and feature tracing
- technical debt and code smells
- API and service-layer review
- object-level authorization review
- input validation and error-handling review
- sensitive data and logging review
- dependency and package review
- characterization/unit/integration test generation
- refactoring planning
- modernization review
- cloud readiness review
- pull request review package preparation

## Technology stack

This repository starts as a **.NET 6 inherited baseline**.

- .NET 6
- C# 10 baseline syntax
- ASP.NET Core Web API
- Entity Framework Core 6
- SQLite for local development
- xUnit and FluentAssertions
- Newtonsoft.Json modernization target
- System.Data.SqlClient modernization target
- older coding constructs that can be reviewed during C# 14 modernization

One of the course activities is to use AI-assisted review workflows to assess, plan, implement, and validate modernization from .NET 6 to .NET 10 and from C# 10-era code toward current C# language features.

## Getting started

```bash
dotnet restore src/ClaimDesk.Api/ClaimDesk.Api.csproj
dotnet build src/ClaimDesk.Api/ClaimDesk.Api.csproj
dotnet run --project src/ClaimDesk.Api/ClaimDesk.Api.csproj
```

Run tests:

```bash
dotnet test tests/ClaimDesk.UnitTests/ClaimDesk.UnitTests.csproj
dotnet test tests/ClaimDesk.IntegrationTests/ClaimDesk.IntegrationTests.csproj
```

## Demo headers

The application uses demo-only current-user headers so review lessons can focus on authorization behavior without needing a full production identity provider.

```text
X-User-Id: 1
X-User-Role: Manager
```

Other seeded user IDs:

```text
1 = Manager
2 = Adjuster
3 = Adjuster
4 = CustomerService
```

This header-based identity mechanism is intentionally not production-grade and is part of the review material.

## Important course note

Do not present this project as a secure or modern reference implementation. It is a training case study. Learners should use AI to inspect it, document behavior, find risks, generate tests, and prepare evidence-backed review recommendations before making modernization changes.
