---
applyTo: "**/*.cs"
---

# C# Code Instructions — OpenQR

## Primary Constructors (C# 12+)
Toujours utiliser les primary constructors pour l'injection de dépendances :
```csharp
// ✅ Correct
public sealed class CreateQrCodeHandler(IQrCodeRepository repository, IMediator mediator)
    : IRequestHandler<CreateQrCodeCommand, QrCodeId> { }

// ❌ Incorrect
public sealed class CreateQrCodeHandler
{
    private readonly IQrCodeRepository _repository;
    public CreateQrCodeHandler(IQrCodeRepository repository) { _repository = repository; }
}
```

## Classes et Records
- Toutes les classes non-abstraites doivent être `sealed`
- DTOs et Commands : `sealed record`
- Value Objects : `sealed record` avec validation dans le constructeur
- Entités Domain : `sealed class` avec factory method statique

## Null Handling
- Toujours utiliser `is null` / `is not null` — jamais `== null`
- Utiliser l'opérateur `?.` et `??`

## Async/Await
- Suffix `Async` obligatoire sur TOUTES les méthodes asynchrones
- Jamais `.Result`, `.Wait()`, `.GetAwaiter().GetResult()`
- `ConfigureAwait(false)` dans toutes les couches non-UI (Domain, Application, Infrastructure)
- Pas de `ConfigureAwait` dans OpenQR.Web (Blazor)

## Namespaces
Toujours file-scoped : `namespace OpenQR.Application.Features.QrCodes.Commands;`

## Style Allman
Accolades toujours sur une nouvelle ligne pour les méthodes multi-lignes.
Expression-bodied pour les one-liners.