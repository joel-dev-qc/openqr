# Contributing

Contributions are welcome! Please read this guide before opening a pull request.

## Getting Started

1. Fork the repository
2. Create a feature branch: `git checkout -b feat/my-feature`
3. Make your changes following the [Development Guide](Development-Guide)
4. Run the tests: `dotnet test`
5. Commit using [Conventional Commits](https://www.conventionalcommits.org/)
6. Open a pull request against `main`

## Branch Naming

| Type | Pattern | Example |
|------|---------|---------|
| Feature | `feat/<name>` | `feat/qr-code-expiry` |
| Bug fix | `fix/<name>` | `fix/mongo-connection-leak` |
| Refactor | `refactor/<name>` | `refactor/solid-repository` |
| Docs | `docs/<name>` | `docs/api-reference` |
| Chore | `chore/<name>` | `chore/update-packages` |

## Pull Request Checklist

- [ ] Code follows the conventions in the [Development Guide](Development-Guide)
- [ ] `dotnet build` passes with no warnings (TreatWarningsAsErrors=true)
- [ ] `dotnet test` passes
- [ ] New public APIs have XML doc comments
- [ ] No `.Result`, `.Wait()`, or blocking calls
- [ ] `ConfigureAwait(false)` used in Infrastructure / Application layers
- [ ] No direct reference from Application → Infrastructure

## Reporting Issues

Please open a GitHub Issue with:
- Clear description of the problem
- Steps to reproduce
- Expected vs actual behavior
- .NET version, OS, MongoDB version

## Code of Conduct

Be respectful and constructive. We follow the [Contributor Covenant](https://www.contributor-covenant.org/).
