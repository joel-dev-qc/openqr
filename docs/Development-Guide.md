# Development Guide

## Prerequisites

- .NET 10 SDK
- MongoDB 7+ (or Docker)
- Rider / VS Code / Visual Studio 2022+

## Project Structure

```
openqr/
├── src/
│   ├── OpenQR.Domain/
│   ├── OpenQR.Application/
│   ├── OpenQR.Infrastructure/
│   ├── OpenQR.API/
│   └── OpenQR.Web/
├── tests/                      # (coming soon)
├── docs/                       # This wiki
├── .github/
│   ├── copilot-instructions.md
│   ├── instructions/           # Copilot per-layer instructions
│   └── skills/                 # Copilot skills
├── Directory.Build.props
├── Directory.Packages.props
└── OpenQR.slnx
```

## Coding Conventions

### Naming

| Element | Convention | Example |
|---------|-----------|---------|
| Private fields | `_camelCase` | `_repository` |
| Interfaces | `I` prefix | `IQrCodeRepository` |
| Async methods | `Async` suffix | `GetByIdAsync` |
| DTOs | `Dto` suffix | `QrCodeDto` |
| Commands | `Command` suffix | `CreateQrCodeCommand` |
| Queries | `Query` suffix | `GetQrCodeQuery` |
| Handlers | `Handler` suffix | `CreateQrCodeHandler` |

### C# Style

- File-scoped namespaces: `namespace OpenQR.Application.Features.QrCodes;`
- Allman braces (always on new line)
- `is null` / `is not null` over `== null`
- `var` when type is obvious
- `using` declarations (not `using { }` blocks)
- Expression-bodied members for simple one-liners
- `sealed` on all non-abstract classes
- `primary constructors` for dependency injection

### Async Rules

- Always suffix async methods with `Async`
- Never use `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`
- Always `await` Task-returning methods
- Always `ConfigureAwait(false)` in Infrastructure and Application layers

## Adding a New Feature

### 1 — Domain

Add the entity or value object in `OpenQR.Domain`.

```csharp
// OpenQR.Domain/Entities/QrCode.cs
namespace OpenQR.Domain.Entities;

public sealed class QrCode
{
    public QrCodeId Id { get; private set; } = QrCodeId.New();
    public string Url { get; private set; }

    private QrCode() { } // MongoDB

    private QrCode(string url) => Url = url;

    public static QrCode Create(string url) => new(url);
}
```

### 2 — Application

Add the Command/Query and Handler in `OpenQR.Application`.

```csharp
// Command
public sealed record CreateQrCodeCommand(string Url, string UserId) : IRequest<QrCodeId>;

// Handler
public sealed class CreateQrCodeHandler(IQrCodeWriteRepository repository)
    : IRequestHandler<CreateQrCodeCommand, QrCodeId>
{
    public async ValueTask<QrCodeId> Handle(CreateQrCodeCommand command, CancellationToken ct)
    {
        var qrCode = QrCode.Create(command.Url);
        await repository.AddAsync(qrCode, ct).ConfigureAwait(false);
        return qrCode.Id;
    }
}
```

### 3 — Infrastructure

Implement the repository interface in `OpenQR.Infrastructure`.

```csharp
public sealed class MongoQrCodeRepository(IMongoCollection<QrCode> collection)
    : IQrCodeWriteRepository, IQrCodeReadRepository
{
    public async Task AddAsync(QrCode qr, CancellationToken ct)
        => await collection.InsertOneAsync(qr, cancellationToken: ct).ConfigureAwait(false);
}
```

### 4 — API Endpoint

Add a Carter module in `OpenQR.API/Endpoints/V1/`.

```csharp
public sealed class QrCodesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/qrcodes").RequireAuthorization();

        group.MapPost("/", async (CreateQrCodeRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(new CreateQrCodeCommand(req.Url, req.UserId), ct);
            return Results.Created($"/api/v1/qrcodes/{id}", id);
        });
    }
}
```

## Mediator — Important

OpenQR uses **`martinothamar/Mediator`**, not MediatR.

| ✅ Use | ❌ Never |
|--------|---------|
| `Mediator.Abstractions` | `MediatR` |
| `IRequest<T>` | `MediatR.IRequest<T>` |
| `ValueTask<T>` in handlers | `Task<T>` (prefer ValueTask) |

## Package Management

All versions are centralized in [`Directory.Packages.props`](../Directory.Packages.props). Never set a version directly in a `.csproj` — add it to `Directory.Packages.props` first.

## Build

```bash
dotnet build
```

`TreatWarningsAsErrors=true` is set globally — all analyzer warnings are build errors.

## Commit Convention

Follows [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: add QR code creation endpoint
fix: resolve MongoDB connection issue
docs: update API documentation
refactor: extract QrCode validation logic
test: add unit tests for CreateQrCodeCommand
chore: update NuGet packages
```
