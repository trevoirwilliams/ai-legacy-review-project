# Modernization Scenarios

This project is not a migration course project. Modernization is included because legacy code review often includes reviewing upgrade recommendations, dependency changes, cloud readiness, language modernization, and agent-generated pull requests.

## Baseline

ClaimDesk Operations API starts as a .NET 6 application using C# 10-era syntax. This gives learners a realistic inherited application that requires platform review before modernization.

## Local modernization review targets

| Scenario | Repository target | Review goal |
|---|---|---|
| .NET 6 to .NET 10 | all project files and `global.json` | Review unsupported runtime risk, package compatibility, build impact, and test evidence. |
| C# 10-era syntax to current C# | domain and service classes | Review whether syntax modernization improves clarity without changing behavior. |
| Verbose backing fields | `InsuranceClaim` | Review possible modernization toward newer property patterns where appropriate. |
| Collection initialization | entity collections and service methods | Review older `new List<T>()` style against newer collection expressions after upgrade. |
| Newtonsoft.Json to System.Text.Json | `ClaimService` package usage | Review behavior compatibility, public response contracts, and tests. |
| System.Data.SqlClient to Microsoft.Data.SqlClient | Infrastructure package reference | Review package replacement, connection behavior, encryption defaults, and integration tests. |
| Local configuration to safer configuration | `appsettings.json` | Review connection strings, environment configuration, and cloud deployment readiness. |
| Local file path to cloud storage | `DocumentStorage:RootPath` | Review local file system assumptions and Azure Blob/File Storage readiness. |
| SMTP-style email modernization | `Email` settings | Review email notification assumptions and replacement with managed services. |
| Sparse tests to characterization tests | test projects | Review current behavior before refactoring or modernization. |

## Cloud readiness review targets

- identity and access management
- managed identity readiness
- Key Vault readiness
- storage modernization readiness
- telemetry and logging modernization
- cache readiness
- background processing readiness

## Agentic modernization PR review

When reviewing AI-generated modernization work, require:

1. scoped issue or prompt
2. generated plan
3. affected files list
4. build evidence
5. test evidence
6. dependency and CVE evidence
7. security review notes
8. rollback plan
9. human approval decision
