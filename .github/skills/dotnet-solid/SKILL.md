---
name: dotnet-solid
description: 'Review, enforce, and refactor C#/.NET code to comply with SOLID principles (Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion). Use when asked to "apply SOLID", "check SOLID principles", "refactor for SOLID", "review class design", "fix tight coupling", "reduce god class", "too many responsibilities", "improve testability", "hard to unit test", "violates SRP", "violates DIP", "violates OCP", "violates LSP", "violates ISP", or "apply clean architecture in .NET". Tailored for the OpenQR solution using Clean Architecture layers (Domain, Application, Infrastructure, API, Web), Mediator pattern, MongoDB, Carter, MudBlazor, Serilog, and OpenTelemetry.'
compatibility: '.NET 10, C# 13, ASP.NET Core, Blazor — OpenQR solution architecture'
---

# SOLID Principles Enforcement — OpenQR (.NET / C#)

Apply, review, and enforce SOLID design principles on C#/.NET code in `${selection}`.
Scan for violations, explain *why* each is a violation, propose a refactored version
using idiomatic C# 13 / .NET 10, and align with the **OpenQR Clean Architecture layers**.

---

## 🏗️ OpenQR Architecture Reference

Before applying SOLID, understand which layer you are working in:

| Layer | Project | Responsibility |
|-------|---------|----------------|
| **Domain** | `OpenQR.Domain` | Entities, Value Objects, Domain Events, Aggregates — NO external dependencies |
| **Application** | `OpenQR.Application` | Use Cases (Mediator Handlers), Interfaces (ports), DTOs, Validation |
| **Infrastructure** | `OpenQR.Infrastructure` | MongoDB repositories, external services, JWT, adapters |
| **API** | `OpenQR.API` | Carter modules (endpoints), OpenAPI/Scalar, Health checks |
| **Web** | `OpenQR.Web` | Blazor/MudBlazor UI components |

> **SOLID violations in the wrong layer are architecture violations too.**
> Example: a Domain entity that depends on MongoDB = DIP + SRP violation.

---

## Workflow

1. **Identify** which layer(s) `${selection}` belongs to
2. **Scan** for violations using the checklist below (one section per principle)
3. **Report** each violation: principle name, file, line, reason
4. **Propose** a concrete refactored version in idiomatic C# 13
5. **Confirm** before applying any changes

---

## S — Single Responsibility Principle

> *A class should have only one reason to change.*

### ✅ Checks
- [ ] Does each class have exactly **one** responsibility?
- [ ] Is the class name a precise, single-purpose noun/verb?
- [ ] Are unrelated concerns (e.g., logging + business logic + persistence) mixed in the same class?
- [ ] Does a Mediator handler do more than orchestrate one use case?

### 🚫 Common OpenQR Violations
- A `QrCodeService` that generates QR codes **and** persists them **and** sends notifications
- A Carter module that contains business logic instead of delegating to Mediator
- A Domain entity with methods that call external services

### ✅ Idiomatic Fix — OpenQR style
```csharp
// ❌ Violation — handler does too much
public sealed class CreateQrCodeHandler(IMongoCollection<QrCode> collection, IEmailSender email)
    : IRequestHandler<CreateQrCodeCommand, QrCodeId>
{
    public async ValueTask<QrCodeId> Handle(CreateQrCodeCommand command, CancellationToken ct)
    {
        var qrCode = new QrCode(command.Url);
        await collection.InsertOneAsync(qrCode, cancellationToken: ct); // persistence
        await email.SendAsync(command.UserEmail, "QR created!");        // notification
        return qrCode.Id;
    }
}

// ✅ Compliant — single responsibility per class
public sealed class CreateQrCodeHandler(IQrCodeRepository repository, IMediator mediator)
    : IRequestHandler<CreateQrCodeCommand, QrCodeId>
{
    public async ValueTask<QrCodeId> Handle(CreateQrCodeCommand command, CancellationToken ct)
    {
        var qrCode = QrCode.Create(command.Url);       // Domain factory
        await repository.AddAsync(qrCode, ct);          // delegate persistence
        await mediator.Publish(new QrCodeCreatedEvent(qrCode.Id), ct); // raise event
        return qrCode.Id;
    }
}
// Notification handled separately by a QrCodeCreatedEventHandler
```

---

## O — Open/Closed Principle

> *Software entities should be open for extension, but closed for modification.*

### ✅ Checks
- [ ] Adding a new QR code type/format requires modifying existing classes?
- [ ] Are `switch`/`if-else` chains on type used to determine behavior?
- [ ] Can new behavior be added via new Mediator handlers or strategy implementations?

### 🚫 Common OpenQR Violations
- A `QrCodeGeneratorService` with `if (format == "png") ... else if (format == "svg") ...`
- Adding a new QR target type forces edits to an existing domain class

### ✅ Idiomatic Fix — strategy + DI registration
```csharp
// ❌ Violation
public class QrCodeRenderer
{
    public byte[] Render(QrCode qr, string format) => format switch
    {
        "png" => RenderPng(qr),
        "svg" => RenderSvg(qr),
        _     => throw new NotSupportedException()
    };
}

// ✅ Compliant — open for extension via new implementations
public interface IQrCodeRenderer
{
    string Format { get; }
    byte[] Render(QrCode qr);
}

public sealed class PngQrCodeRenderer : IQrCodeRenderer
{
    public string Format => "png";
    public byte[] Render(QrCode qr) { /* ... */ }
}

public sealed class SvgQrCodeRenderer : IQrCodeRenderer
{
    public string Format => "svg";
    public byte[] Render(QrCode qr) { /* ... */ }
}

// Registration in Infrastructure DI
services.AddKeyedScoped<IQrCodeRenderer, PngQrCodeRenderer>("png");
services.AddKeyedScoped<IQrCodeRenderer, SvgQrCodeRenderer>("svg");
```

---

## L — Liskov Substitution Principle

> *Subtypes must be substitutable for their base types without altering correctness.*

### ✅ Checks
- [ ] Do derived classes override methods in ways that break expected contracts?
- [ ] Do any subclasses throw `NotImplementedException` or `NotSupportedException`?
- [ ] Do subclasses strengthen preconditions or weaken postconditions?

### 🚫 Common OpenQR Violations
- A `ReadOnlyQrCodeRepository` that inherits `QrCodeRepository` but throws on `AddAsync`
- A domain Value Object subclass that changes equality semantics

### ✅ Idiomatic Fix — prefer interfaces over broken inheritance
```csharp
// ❌ Violation — breaks LSP contract
public class ReadOnlyQrCodeRepository : MongoQrCodeRepository
{
    public override Task AddAsync(QrCode qr, CancellationToken ct)
        => throw new NotSupportedException("Read-only!"); // Breaks substitutability
}

// ✅ Compliant — segregated interfaces (see also ISP below)
public interface IQrCodeReadRepository
{
    Task<QrCode?> GetByIdAsync(QrCodeId id, CancellationToken ct);
    IAsyncEnumerable<QrCode> GetAllAsync(CancellationToken ct);
}

public interface IQrCodeWriteRepository
{
    Task AddAsync(QrCode qr, CancellationToken ct);
    Task DeleteAsync(QrCodeId id, CancellationToken ct);
}

// Read-only implementation only implements what it can honour
public sealed class MongoQrCodeReadRepository(IMongoCollection<QrCode> collection)
    : IQrCodeReadRepository { /* ... */ }
```

---

## I — Interface Segregation Principle

> *Clients should not be forced to depend on interfaces they do not use.*

### ✅ Checks
- [ ] Are Application layer interfaces small and use-case focused?
- [ ] Does any handler depend on a repository interface with methods it never calls?
- [ ] Are Carter modules importing services with irrelevant methods?

### 🚫 Common OpenQR Violations
- A single `IQrCodeRepository` with 10 methods when a handler only needs `GetByIdAsync`
- A Blazor component injecting a full service when it only needs one query

### ✅ Idiomatic Fix — CQRS-aligned interfaces
```csharp
// ❌ Fat interface — forces all consumers to know about all operations
public interface IQrCodeRepository
{
    Task<QrCode?> GetByIdAsync(QrCodeId id, CancellationToken ct);
    IAsyncEnumerable<QrCode> GetAllAsync(CancellationToken ct);
    Task AddAsync(QrCode qr, CancellationToken ct);
    Task UpdateAsync(QrCode qr, CancellationToken ct);
    Task DeleteAsync(QrCodeId id, CancellationToken ct);
    Task<long> CountAsync(CancellationToken ct);
}

// ✅ Segregated — each handler depends only on what it needs
public interface IQrCodeReadRepository
{
    Task<QrCode?> GetByIdAsync(QrCodeId id, CancellationToken ct);
    IAsyncEnumerable<QrCode> GetAllAsync(CancellationToken ct);
    Task<long> CountAsync(CancellationToken ct);
}

public interface IQrCodeWriteRepository
{
    Task AddAsync(QrCode qr, CancellationToken ct);
    Task UpdateAsync(QrCode qr, CancellationToken ct);
    Task DeleteAsync(QrCodeId id, CancellationToken ct);
}

// Infrastructure implements both
public sealed class MongoQrCodeRepository(IMongoCollection<QrCode> collection)
    : IQrCodeReadRepository, IQrCodeWriteRepository { /* ... */ }
```

---

## D — Dependency Inversion Principle

> *Depend on abstractions, not on concretions. High-level modules must not depend on low-level modules.*

### ✅ Checks
- [ ] Does `OpenQR.Application` reference `OpenQR.Infrastructure` directly? (**Hard violation**)
- [ ] Are concrete classes instantiated with `new` inside handlers or domain classes?
- [ ] Are MongoDB types (`IMongoCollection<T>`) used directly in Application layer?
- [ ] Are all dependencies injected via **primary constructors** (C# 12+)?

### 🚫 Common OpenQR Violations
- A handler directly using `MongoClient` or `IMongoCollection<QrCode>` instead of a repository interface
- `new QrCodeGeneratorService()` inside a handler
- `OpenQR.Domain` referencing any NuGet package except pure abstractions

### ✅ Idiomatic Fix — ports and adapters aligned with OpenQR layers
```csharp
// ❌ Violation — Application depends directly on Infrastructure (MongoDB)
public sealed class GetQrCodeHandler(IMongoCollection<QrCode> collection)
    : IRequestHandler<GetQrCodeQuery, QrCodeDto?>
{
    public async ValueTask<QrCodeDto?> Handle(GetQrCodeQuery q, CancellationToken ct)
    {
        var result = await collection.Find(x => x.Id == q.Id).FirstOrDefaultAsync(ct);
        return result is null ? null : new QrCodeDto(result.Id, result.Url);
    }
}

// ✅ Compliant — Application depends on abstraction defined in Application layer

// OpenQR.Application/Interfaces/IQrCodeReadRepository.cs
public interface IQrCodeReadRepository
{
    Task<QrCode?> GetByIdAsync(QrCodeId id, CancellationToken ct);
}

// OpenQR.Application/Features/QrCodes/Queries/GetQrCodeHandler.cs
public sealed class GetQrCodeHandler(IQrCodeReadRepository repository)
    : IRequestHandler<GetQrCodeQuery, QrCodeDto?>
{
    public async ValueTask<QrCodeDto?> Handle(GetQrCodeQuery q, CancellationToken ct)
    {
        var qrCode = await repository.GetByIdAsync(q.Id, ct);
        return qrCode is null ? null : new QrCodeDto(qrCode.Id, qrCode.Url);
    }
}

// OpenQR.Infrastructure/Repositories/MongoQrCodeRepository.cs
public sealed class MongoQrCodeRepository(IMongoCollection<QrCode> collection)
    : IQrCodeReadRepository, IQrCodeWriteRepository
{
    public async Task<QrCode?> GetByIdAsync(QrCodeId id, CancellationToken ct)
        => await collection.Find(x => x.Id == id).FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
}

// OpenQR.Infrastructure/DependencyInjection.cs
services.AddScoped<IQrCodeReadRepository, MongoQrCodeRepository>();
services.AddScoped<IQrCodeWriteRepository, MongoQrCodeRepository>();
```

---

## 📋 Full SOLID Review Checklist — OpenQR

### Single Responsibility
- [ ] Each class has ONE reason to change
- [ ] Mediator handlers only orchestrate — no business logic leaking into endpoints
- [ ] Carter modules only map HTTP ↔ Mediator commands/queries
- [ ] Domain entities contain only domain behaviour — no I/O, no logging

### Open/Closed
- [ ] New QR code formats/types added via new classes, not by modifying existing ones
- [ ] Strategy/plugin patterns used for variable behaviour (renderers, validators, exporters)
- [ ] No `switch` on type in Application or Domain layers

### Liskov Substitution
- [ ] No `NotImplementedException` / `NotSupportedException` in overrides
- [ ] Repository subtypes fully honour the contracts of their base interfaces
- [ ] Domain Value Object subclasses preserve equality and comparison semantics

### Interface Segregation
- [ ] Repository interfaces split by read/write (CQRS alignment)
- [ ] Handlers depend only on the interface methods they actually use
- [ ] No god-interfaces with 10+ methods in `OpenQR.Application`

### Dependency Inversion
- [ ] `OpenQR.Application` has **zero** direct references to `OpenQR.Infrastructure`
- [ ] `OpenQR.Domain` has **zero** infrastructure or framework dependencies
- [ ] All dependencies injected via primary constructors (`public sealed class Foo(IDep dep)`)
- [ ] MongoDB types only appear in `OpenQR.Infrastructure`
- [ ] Serilog only configured at composition root (`OpenQR.API` / `OpenQR.Web`)

---

## 🔗 Dependency Flow — OpenQR (enforce strictly)

```
OpenQR.Domain
    ↑
OpenQR.Application  (defines interfaces/ports)
    ↑                        ↑
OpenQR.Infrastructure    OpenQR.API / OpenQR.Web
(implements ports)       (composition root + DI registration)
```

> ⚠️ Any arrow pointing **downward** toward Domain or **sideways** from Infrastructure to API
> is an architecture violation. Flag it alongside the SOLID violation.

---

## 🛠️ OpenQR Coding Conventions (enforce during SOLID review)

| Convention | Rule |
|-----------|------|
| **Primary constructors** | `public sealed class Foo(IDep dep)` — always for DI |
| **`sealed` classes** | All non-abstract Application/Infrastructure classes must be `sealed` |
| **`record` for Value Objects** | `public sealed record QrCodeId(Guid Value);` |
| **`sealed record` for DTOs/Commands** | `public sealed record CreateQrCodeCommand(string Url) : ICommand<QrCodeId>;` |
| **`async/await`** | All I/O must be async; use `ConfigureAwait(false)` in Infrastructure |
| **`IAsyncEnumerable`** | For streaming MongoDB results instead of `List<T>` |
| **File-scoped namespaces** | `namespace OpenQR.Application.Features.QrCodes;` |
| **`_camelCase` fields** | Private fields use `_camelCase` prefix |
| **`TreatWarningsAsErrors`** | **All analyzer warnings are build errors** — SOLID-related analyzers (CA1062, CA2007, SA*) block the build |

---

## 🌐 Web Layer (OpenQR.Web) — Blazor / MudBlazor SOLID Rules

> Blazor components are classes too — they must obey SOLID.

### SRP in Blazor Components
- A `.razor` component should do **one thing**: render UI and delegate logic
- Business logic, HTTP calls, and state management belong in **injected services**, not in `@code` blocks
- Extract complex state into a dedicated `ViewModel` or `StateContainer` class

```razor
@* ❌ Violation — business logic + HTTP + UI in one component *@
@code {
    private List<QrCodeDto> _qrCodes = [];

    protected override async Task OnInitializedAsync()
    {
        var response = await Http.GetAsync("/api/v1/qrcodes");
        var json = await response.Content.ReadAsStringAsync();
        _qrCodes = JsonSerializer.Deserialize<List<QrCodeDto>>(json)!;
        // + filtering, sorting, paging logic...
    }
}

@* ✅ Compliant — component delegates to an injected service *@
@inject IQrCodeApiClient QrCodeClient
@code {
    private IReadOnlyList<QrCodeDto> _qrCodes = [];

    protected override async Task OnInitializedAsync()
        => _qrCodes = await QrCodeClient.GetAllAsync();
}
```

### DIP in Blazor Components
- Always inject **interfaces**, never concrete HTTP clients or service implementations directly
- Define service interfaces in `OpenQR.Web/Core/Interfaces/`
- Implement them in `OpenQR.Web/Infrastructure/`

### ISP in Blazor Components
- Don't inject a service with 15 methods into a component that only needs 1
- Split service interfaces by feature area: `IQrCodeApiClient`, `IUserApiClient`

---

## 🧪 Tests as SOLID Violation Indicators

> If code is hard to test, it almost certainly violates SOLID. Use test friction as a diagnostic tool.

| Test Friction | Likely SOLID Violation |
|---|---|
| Must instantiate many real dependencies to test one class | **DIP** — missing abstractions; inject interfaces instead |
| Test setup exceeds 20 lines for a single unit | **SRP** — class has too many responsibilities |
| Changing one feature breaks unrelated tests | **SRP** / **OCP** — responsibilities are not isolated |
| Must use `new ConcreteClass()` in tests (no interface) | **DIP** — depends on concretion |
| Adding a new case requires modifying an existing test | **OCP** — behavior is not extensible |
| Subclass test fails when run against base class contract | **LSP** — subtype breaks the contract |
| Test imports an interface but mocks only 1 of 10 methods | **ISP** — interface is too fat |

```csharp
// ✅ Well-designed handler — trivial to test because of DIP + SRP
public sealed class CreateQrCodeHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsNewId()
    {
        // Arrange — only one mock needed = SRP + DIP respected
        var repository = Substitute.For<IQrCodeWriteRepository>();
        var handler = new CreateQrCodeHandler(repository);
        var command = new CreateQrCodeCommand("https://openqr.io", UserId: "user-1");

        // Act
        var id = await handler.Handle(command, CancellationToken.None);

        // Assert
        await repository.Received(1).AddAsync(Arg.Any<QrCode>(), Arg.Any<CancellationToken>());
        id.Should().NotBeEmpty();
    }
}
```

---

## 🧱 Composition over Inheritance

> Prefer composing behaviors from small focused objects over building deep class hierarchies.

- Inheritance creates tight coupling between base and derived classes — avoid it except for genuine "is-a" relationships
- Use **interfaces + constructor injection** to compose behaviors
- Use **extension methods** to add capabilities without inheritance
- Use **decorators** to add cross-cutting concerns (logging, caching, validation) without modifying classes

```csharp
// ❌ Inheritance-based — fragile, hard to test, violates OCP
public class LoggingQrCodeRepository : MongoQrCodeRepository
{
    protected override async Task AddAsync(QrCode qr, CancellationToken ct)
    {
        Console.WriteLine("Adding QR code...");
        await base.AddAsync(qr, ct);
    }
}

// ✅ Decorator pattern — composable, testable, respects OCP + DIP
public sealed class LoggingQrCodeRepositoryDecorator(IQrCodeWriteRepository inner, ILogger<LoggingQrCodeRepositoryDecorator> logger)
    : IQrCodeWriteRepository
{
    public async Task AddAsync(QrCode qr, CancellationToken ct)
    {
        logger.LogInformation("Adding QR code {Id}", qr.Id);
        await inner.AddAsync(qr, ct).ConfigureAwait(false);
    }
}
// Register in DI: Decorate<IQrCodeWriteRepository, LoggingQrCodeRepositoryDecorator>()
```

---

## 🔗 Related Skills

- [`dotnet-best-practices`](.github/skills/dotnet-best-practices/SKILL.md) — general .NET code quality and async patterns
- [`clean-code`](.github/skills/clean-code/SKILL.md) — Clean Code principles, code smells, and enterprise patterns
- [`csharp-xunit`](.github/skills/csharp-xunit/SKILL.md) — XUnit best practices for testing SOLID-compliant code
- [`csharp-async`](.github/skills/csharp-async/SKILL.md) — async/await patterns applied in Infrastructure and Application layers
