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
| **SonarAnalyzer** | All SonarAnalyzer.CSharp warnings must be resolved before merge |

---

## 🔗 Related Skills

- `dotnet-best-practices` — general .NET code quality and async patterns
- `dotnet-design-pattern-review` — GoF pattern review (Command, Factory, Repository)
- `refactor` — general refactoring guidance
- `conventional-commit` — commit message after SOLID refactoring
