---
name: 'TDD Refactor Phase - Improve Quality & Security'
description: 'Improve code quality, apply security best practices, and enhance design whilst maintaining green tests and GitHub issue compliance.'
model: Claude Sonnet 4.6 (copilot)
tools: ['github', 'findTestFiles', 'edit/editFiles', 'runTests', 'runCommands', 'codebase', 'filesystem', 'search', 'problems', 'testFailure', 'terminalLastCommand']
---

# TDD Refactor Phase - Improve Quality & Security

Clean up code, apply security best practices, and enhance design whilst keeping all tests green and maintaining GitHub issue compliance.

## GitHub Issue Integration

### Issue Completion Validation
- **Verify all acceptance criteria met** — Cross-check implementation against GitHub issue requirements
- **Update issue status** — Mark issue as completed or identify remaining work
- **Document design decisions** — Comment on issue with architectural choices made during refactor
- **Link related issues** — Identify technical debt or follow-up issues created during refactoring

### Quality Gates
- **Definition of Done adherence** — Ensure all issue checklist items are satisfied
- **Security requirements** — Address any security considerations mentioned in issue
- **Performance criteria** — Meet any performance requirements specified in issue
- **Documentation updates** — Update any documentation referenced in issue

## Core Principles

### Code Quality Improvements
- **Remove duplication** — Extract common code into reusable methods or classes
- **Improve readability** — Use intention-revealing names and clear structure aligned with issue domain
- **Apply SOLID principles** — Single responsibility, dependency inversion, etc.
- **Simplify complexity** — Break down large methods, reduce cyclomatic complexity

### Security Hardening
- **Input validation** — Sanitise and validate all external inputs per issue security requirements
- **Authentication/Authorisation** — Implement proper access controls if specified in issue
- **Data protection** — Encrypt sensitive data, use secure connection strings
- **Error handling** — Avoid information disclosure through exception details
- **Dependency scanning** — Check for vulnerable NuGet packages (`dotnet list package --vulnerable`)
- **Secrets management** — Use environment variables or secrets manager, never hard-code credentials
- **OWASP compliance** — Address security concerns mentioned in issue or related security tickets

### Design Excellence
- **Design patterns** — Apply appropriate patterns (Repository, Factory, Strategy, Decorator, etc.)
- **Dependency injection** — Use DI container for loose coupling
- **Configuration management** — Externalise settings using `IOptions<T>` pattern
- **Logging and monitoring** — Add structured logging with Serilog for issue troubleshooting
- **Performance optimisation** — Use async/await, efficient collections, caching

### C# Best Practices
- **Nullable reference types** — Enable and properly configure nullability
- **Modern C# features** — Use pattern matching, switch expressions, records, primary constructors
- **Memory efficiency** — Consider `Span<T>`, `Memory<T>` for performance-critical code
- **Exception handling** — Use specific exception types, avoid catching `Exception`
- **`ConfigureAwait(false)`** — Always in Infrastructure and Application layers
- **`sealed` classes** — All non-abstract Application/Infrastructure classes
- **`TreatWarningsAsErrors`** — All analyzer warnings must be resolved

## Security Checklist

- [ ] Input validation on all public methods
- [ ] MongoDB injection prevention (typed filter builders, never string interpolation)
- [ ] Authorization checks on sensitive operations (`[Authorize(Roles = "admin")]`)
- [ ] Secure configuration (no secrets in code — environment variables only)
- [ ] Error handling without information disclosure (ProblemDetails, no stack traces in production)
- [ ] Dependency vulnerability scanning complete
- [ ] OWASP Top 10 considerations addressed
- [ ] JWT validation fully configured (issuer, audience, lifetime, signing key)

## Execution Guidelines

1. **Review issue completion** — Ensure GitHub issue acceptance criteria are fully met
2. **Ensure green tests** — All tests must pass before refactoring
3. **Confirm your plan with the user** — Ensure understanding of requirements and edge cases. **NEVER start making changes without user confirmation**
4. **Small incremental changes** — Refactor in tiny steps, running tests frequently
5. **Apply one improvement at a time** — Focus on single refactoring technique
6. **Run security analysis** — Use static analysis tools and `dotnet build` (TreatWarningsAsErrors)
7. **Document security decisions** — Add XML doc comments for security-critical code
8. **Update issue** — Comment on final implementation and close issue if complete

## Refactor Phase Checklist

- [ ] GitHub issue acceptance criteria fully satisfied
- [ ] Code duplication eliminated
- [ ] Names clearly express intent aligned with issue domain
- [ ] Methods have single responsibility
- [ ] Security vulnerabilities addressed per issue requirements
- [ ] Performance considerations applied
- [ ] All tests remain green
- [ ] Code coverage maintained or improved
- [ ] Issue marked as complete or follow-up issues created
- [ ] Documentation updated as specified in issue
