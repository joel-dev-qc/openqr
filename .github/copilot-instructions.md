# GitHub Copilot Instructions — OpenQR

## Projet

**OpenQR** est une solution open source de gestion de QR Codes.  
API REST en .NET 10 + interface admin Blazor Interactive Auto.

---

## Stack Technique

### API
| Élément | Choix |
|---|---|
| Framework | .NET 10 |
| Architecture | Clean Architecture |
| Pattern messaging | `martinothamar/Mediator` (`Mediator.SourceGenerator` + `Mediator.Abstractions`) |
| Base de données | MongoDB (`MongoDB.Driver` v3.x) |
| Auth | JWT + Rôles (`admin` / `user`) |

### UI Admin
| Élément | Choix |
|---|---|
| Framework | Blazor Interactive Auto |
| UI Components | MudBlazor |
| Architecture | Clean Architecture côté client |
| Communication | HTTP Client REST → API Core |
| Auth | JWT (token côté client, rôles) |

---

## Structure des projets

```
OpenQR.Domain/          # Entités, Value Objects, Interfaces, Enums
OpenQR.Application/     # Use Cases, Mediator, DTOs, Validators
OpenQR.Infrastructure/  # MongoDB, Repositories, Services
OpenQR.API/             # Controllers, Middleware, Auth, JWT
OpenQR.Web/             # Blazor Interactive Auto + MudBlazor
```

### Structure OpenQR.Web

```
OpenQR.Web/
├── Core/               # Interfaces, modèles, abstractions
├── Infrastructure/     # Services HTTP, Auth, gestion JWT
├── Components/         # Composants MudBlazor réutilisables
└── Pages/              # Pages Blazor (Dashboard, QR, Users...)
```

---

## Conventions de code

### Nommage
- Champs privés : `_camelCase` (ex: `_userRepository`)
- Namespaces : file-scoped (`namespace OpenQR.Domain;`)
- Interfaces : préfixe `I` (ex: `IQrCodeRepository`)
- Méthodes async : suffix `Async` obligatoire (ex: `GetByIdAsync`)
- DTOs : suffix `Dto` (ex: `QrCodeDto`)
- Commands/Queries Mediator : suffix `Command` / `Query` / `Handler`

### Style C#
- `var` quand le type est évident (ex: `var list = new List<string>()`)
- Style Allman pour les accolades (toujours sur une nouvelle ligne)
- Null checks : `is null` / `is not null` (pas `== null`)
- Pattern matching préféré aux casts explicites
- `using` declarations préférées (pas de blocs `using { }`)
- Expression-bodied members pour les propriétés simples et one-liners

### Async/Await
- Suffix `Async` obligatoire sur toutes les méthodes asynchrones
- Jamais de `.Result` ou `.Wait()` — toujours `await`

### Médiateur (IMPORTANT)
- Utiliser **`martinothamar/Mediator`** (packages `Mediator.SourceGenerator` et `Mediator.Abstractions`)
- **JAMAIS** `MediatR` de jbogard
- Utiliser `IRequest<T>` et `IRequestHandler<TRequest, TResponse>` de `Mediator.Abstractions`

---

## Pattern Mediator avec martinothamar/Mediator

```csharp
// Command
public sealed record CreateQrCodeCommand(string Url, string UserId) : IRequest<QrCodeDto>;

// Handler
public sealed class CreateQrCodeCommandHandler : IRequestHandler<CreateQrCodeCommand, QrCodeDto>
{
    private readonly IQrCodeRepository _repository;

    public CreateQrCodeCommandHandler(IQrCodeRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<QrCodeDto> Handle(CreateQrCodeCommand request, CancellationToken cancellationToken)
    {
        // implementation
        await _repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        return dto;
    }
}
```

---

## MongoDB avec MongoDB.Driver v3.x

```csharp
// Repository
public sealed class QrCodeRepository : IQrCodeRepository
{
    private readonly IMongoCollection<QrCode> _collection;

    public QrCodeRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<QrCode>("qr_codes");
    }

    public async Task<QrCode?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
```

---

## JWT Auth

- Rôles : `admin` / `user`
- Token JWT stocké côté client (Blazor)
- Utiliser `[Authorize(Roles = "admin")]` sur les endpoints admin
- Utiliser `Microsoft.AspNetCore.Authentication.JwtBearer` v10.x

---

## Conventions de commit (Conventional Commits)

```
feat: add QR code creation endpoint
fix: resolve MongoDB connection issue
docs: update API documentation
refactor: extract QrCode validation logic
test: add unit tests for CreateQrCodeCommand
chore: update NuGet packages
```

---

## Packages NuGet clés

Toutes les versions sont gérées centralement dans `Directory.Packages.props`.

| Package | Usage |
|---|---|
| `Mediator.SourceGenerator` | Source generator pour le mediator |
| `Mediator.Abstractions` | Abstractions du mediator |
| `MongoDB.Driver` | Driver MongoDB v3.x |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | Auth JWT |
| `MudBlazor` | Composants UI Blazor |

---

## Règles importantes

1. `TreatWarningsAsErrors=true` — tous les warnings sont des erreurs
2. Toujours utiliser `ConfigureAwait(false)` dans les couches non-UI
3. Namespaces file-scoped obligatoires
4. Champs privés en `_camelCase`
5. `martinothamar/Mediator` uniquement — jamais `MediatR`
6. MongoDB.Driver v3.x uniquement
7. .NET 10 uniquement

---

## Skills

Les skills suivants définissent des bonnes pratiques spécialisées à appliquer dans ce projet.

- [C# Async Programming Best Practices](.github/skills/csharp-async/SKILL.md)
- [C# Documentation Best Practices](.github/skills/csharp-docs/SKILL.md)
- [XUnit Best Practices](.github/skills/csharp-xunit/SKILL.md)
- [.NET/C# Best Practices](.github/skills/dotnet-best-practices/SKILL.md)
- [Clean Code Principles](.github/skills/clean-code/SKILL.md)
- [SOLID Principles — OpenQR](.github/skills/dotnet-solid/SKILL.md)
- [Make Skill Template](.github/skills/make-skill-template/SKILL.md)
