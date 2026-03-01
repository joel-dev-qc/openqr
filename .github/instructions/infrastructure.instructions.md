---
applyTo: "OpenQR.Infrastructure/**/*.cs"
---

# Infrastructure Layer Instructions — OpenQR

## MongoDB Driver v3.x
- Primary constructor pour l'injection de IMongoDatabase
- `IAsyncEnumerable<T>` pour les listes (curseur MongoDB)
- `ConfigureAwait(false)` sur tous les awaits

## DI Registration
Toujours via une méthode d'extension `AddInfrastructure(this IServiceCollection, IConfiguration)`.

## Logging
Utiliser `ILogger<T>` de Microsoft.Extensions.Logging uniquement.
Serilog est configuré uniquement dans OpenQR.API/Program.cs.

## ConfigureAwait(false)
Obligatoire sur TOUS les awaits dans cette couche.