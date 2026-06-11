# Modernization Scenarios

This project is not a migration course project. Modernization is included because legacy code review often includes reviewing upgrade recommendations, dependency changes, cloud readiness, and agent-generated pull requests.

## Local modernization review targets

| Scenario | Repository target | Review goal |
|---|---|---|
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
