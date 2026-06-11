# Review Targets

ClaimDesk Operations API includes intentional review targets for the AI-assisted legacy code review course.

## Codebase understanding

- Trace claim intake from `ClaimsController` to `ClaimService` and EF Core.
- Trace activity note retrieval from controller to service to database.
- Identify implicit business rules around managers, adjusters, and customer service users.

## Technical debt

- `ClaimService` mixes validation, authorization, logging, mapping, and persistence.
- DTO mapping is repeated manually.
- Validation is scattered and incomplete.
- Status transition rules are incomplete.
- Search and paging behavior needs review.

## Security and privacy

- Current user context comes from request headers.
- Controller endpoints do not use production authentication or authorization.
- Some service methods do not enforce object-level access consistently.
- Activity notes are read without validating access to the parent claim.
- Logging may expose claim details or note contents.
- Developer-oriented error responses expose too much detail.
- Claim DTOs expose customer contact details.

## Dependency and modernization

- `Newtonsoft.Json` is present as a serialization modernization review target.
- `System.Data.SqlClient` is present as a database client modernization review target.
- Local SQLite and local file path settings can be reviewed for cloud readiness.
- SMTP-style configuration can be reviewed for email modernization.

## Testing

- Unit tests are intentionally sparse.
- Integration tests cover only a basic happy path.
- There are missing tests for access control, validation, status transitions, search, paging, and error handling.

## Final workflow

Learners should produce:

- feature orientation report
- technical debt register
- security review report
- dependency review report
- test gap report
- modernization review plan
- pull request summary
- reviewer checklist
