# Template — Nouveau Use Case

Créer un nouveau use case dans `OpenQR.Application` en suivant les conventions du projet.

## Paramètres

- **Nom du use case** : [NOM]
- **Type** : [Command / Query]
- **Entité concernée** : [ENTITE]
- **Description** : [DESCRIPTION]

## Fichiers à créer

### 1. Command ou Query

```csharp
// OpenQR.Application/[ENTITE]/[TYPE]s/[NOM][TYPE].cs
namespace OpenQR.Application.[ENTITE].[TYPE]s;

public sealed record [NOM][TYPE]([PARAMS]) : IRequest<[RESULT]>;
```

### 2. Handler

```csharp
// OpenQR.Application/[ENTITE]/[TYPE]s/[NOM][TYPE]Handler.cs
namespace OpenQR.Application.[ENTITE].[TYPE]s;

public sealed class [NOM][TYPE]Handler : IRequestHandler<[NOM][TYPE], [RESULT]>
{
    private readonly I[ENTITE]Repository _repository;

    public [NOM][TYPE]Handler(I[ENTITE]Repository repository)
    {
        _repository = repository;
    }

    public async ValueTask<[RESULT]> Handle([NOM][TYPE] request, CancellationToken cancellationToken)
    {
        // TODO: implement
        throw new NotImplementedException();
    }
}
```

### 3. DTO (si nécessaire)

```csharp
// OpenQR.Application/[ENTITE]/DTOs/[ENTITE]Dto.cs
namespace OpenQR.Application.[ENTITE].DTOs;

public sealed record [ENTITE]Dto(
    string Id,
    // other properties
    DateTimeOffset CreatedAt
);
```

## Conventions à respecter

- Namespaces file-scoped
- `ConfigureAwait(false)` sur tous les awaits
- Suffix `Async` sur les méthodes asynchrones
- Records immuables pour Commands/Queries/DTOs
- Utiliser `IRequest<T>` et `IRequestHandler<TRequest, TResponse>` de `Mediator.Abstractions`
- **Jamais MediatR** — utiliser `martinothamar/Mediator` uniquement
