# ClaimDesk Operations API - Copilot Instructions

This repository is used for AI-assisted legacy code review training. Treat the application as an inherited business system, not as a greenfield application.

## Technology expectations

- The baseline application targets .NET 6 and uses C# 10-era syntax.
- Do not upgrade projects to .NET 10 unless the modernization task explicitly asks for it.
- When modernization is requested, assess and plan the upgrade before changing code.
- Review C# language modernization separately from behavior-changing refactoring.
- Keep examples compatible with ASP.NET Core Web API and EF Core.
- Prefer small, reviewable changes over broad rewrites.
- Do not invent business rules. Flag assumptions explicitly.
- Preserve existing behavior unless a change is explicitly requested.

## Review expectations

When reviewing code, inspect for:

- correctness and edge cases
- object-level authorization
- input validation
- sensitive data exposure
- unsafe logging
- inconsistent error handling
- data access risks
- dependency and modernization risks
- unsupported runtime risk
- missing tests
- regression risk
- pull request readiness

## Output expectations

When asked for a review, use this format:

1. Scope reviewed
2. Findings
3. Evidence from code
4. Risk level
5. Suggested remediation
6. Tests required
7. Open assumptions

Do not claim a finding is confirmed unless the code or tool output supports it.
