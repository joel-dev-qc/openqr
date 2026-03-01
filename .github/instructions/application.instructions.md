---
applyTo: "OpenQR.Application/**/*.cs"
---

# Application Layer Instructions — OpenQR

## Règle : zéro référence à OpenQR.Infrastructure
Toujours passer par les interfaces définies dans cette couche ou OpenQR.Domain.

## Mediator — martinothamar/Mediator UNIQUEMENT
- Utiliser Mediator.Abstractions de martinothamar/Mediator
- JAMAIS MediatR de jbogard
- Commands : `sealed record` implémentant `IRequest<T>`
- Queries : `sealed record` implémentant `IRequest<T>`
- Handlers : `sealed class` implémentant `IRequestHandler<TRequest, TResponse>`
- Retour des handlers : `ValueTask<T>`

## DTOs
- `sealed record` uniquement
- Mappage via méthodes d'extension (pas AutoMapper)

## Organisation des dossiers
```
OpenQR.Application/
├── Features/
│   └── QrCodes/
│       ├── Commands/CreateQrCode/
│       └── Queries/GetQrCodeById/
├── Interfaces/
├── Mappings/
└── Behaviors/
```