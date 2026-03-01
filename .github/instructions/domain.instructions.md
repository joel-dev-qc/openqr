---
applyTo: "OpenQR.Domain/**/*.cs"
---

# Domain Layer Instructions — OpenQR

## Règle absolue : zéro dépendance externe
OpenQR.Domain ne référence aucun package NuGet d'infrastructure.
Pas de MongoDB, Serilog, MediatR, ASP.NET Core.

## Entités
- `sealed class` avec constructeur privé + factory method `Create(...)`
- Propriétés `private set` uniquement
- Méthodes de domaine pour les mutations

## Value Objects
- `sealed record` avec validation dans le constructeur
- Factory methods statiques `New()` et `From(...)`

## Interfaces (Ports)
Définies dans OpenQR.Domain ou OpenQR.Application, jamais dans OpenQR.Infrastructure.

## Domain Events
`sealed record` implémentant `INotification` de Mediator.Abstractions.