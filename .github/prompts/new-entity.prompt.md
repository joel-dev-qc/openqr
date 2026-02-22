# Template — Nouvelle Entité Domain

Créer une nouvelle entité dans `OpenQR.Domain` en suivant les conventions du projet.

## Paramètres

- **Nom de l'entité** : [NOM]
- **Description** : [DESCRIPTION]
- **Propriétés** : [LISTE DES PROPRIETES]

## Fichiers à créer

### 1. Entité

```csharp
// OpenQR.Domain/[NOM]/[NOM].cs
namespace OpenQR.Domain.[NOM];

public sealed class [NOM]
{
    public string Id { get; private set; } = default!;
    // autres propriétés

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private [NOM]() { } // Pour MongoDB/désérialisation

    public static [NOM] Create([PARAMS])
    {
        // validation
        return new [NOM]
        {
            Id = Guid.NewGuid().ToString(),
            // initialisation des propriétés
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }
}
```

### 2. Interface du Repository

```csharp
// OpenQR.Domain/[NOM]/I[NOM]Repository.cs
namespace OpenQR.Domain.[NOM];

public interface I[NOM]Repository
{
    Task<[NOM]?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<[NOM]>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync([NOM] entity, CancellationToken cancellationToken = default);
    Task UpdateAsync([NOM] entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
```

### 3. Implémentation du Repository (Infrastructure)

```csharp
// OpenQR.Infrastructure/[NOM]/[NOM]Repository.cs
namespace OpenQR.Infrastructure.[NOM];

public sealed class [NOM]Repository : I[NOM]Repository
{
    private readonly IMongoCollection<Domain.[NOM].[NOM]> _collection;

    public [NOM]Repository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Domain.[NOM].[NOM]>("[nom_collection]");
    }

    public async Task<Domain.[NOM].[NOM]?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    // autres méthodes...
}
```

## Conventions à respecter

- Namespaces file-scoped
- Entité avec constructeur privé et factory method statique `Create`
- Interface du repository dans le Domain
- Implémentation du repository dans Infrastructure
- `ConfigureAwait(false)` sur tous les awaits
- `is null` / `is not null` pour les null checks
- Champs privés en `_camelCase`
