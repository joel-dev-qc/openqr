# OpenQR — Agent Setup Instructions

## Projet

**OpenQR** est une solution open source de gestion de QR Codes.  
Elle est composée d'une API REST en .NET 10 et d'une interface admin en Blazor Interactive Auto.

---

## Stack et Architecture

### API Backend
- **Framework** : .NET 10
- **Architecture** : Clean Architecture
- **Messaging** : `martinothamar/Mediator` (source generator) — **PAS MediatR**
- **Base de données** : MongoDB avec `MongoDB.Driver` v3.x
- **Authentification** : JWT + Rôles (`admin` / `user`)

### Interface Admin
- **Framework** : Blazor Interactive Auto
- **UI** : MudBlazor
- **Auth** : JWT côté client

### Structure des projets
```
OpenQR.Domain/          # Entités, Value Objects, Interfaces, Enums
OpenQR.Application/     # Use Cases, Mediator, DTOs, Validators
OpenQR.Infrastructure/  # MongoDB, Repositories, Services
OpenQR.API/             # Controllers, Middleware, Auth, JWT
OpenQR.Web/             # Blazor Interactive Auto + MudBlazor
```

---

## Prérequis

- .NET 10 SDK
- MongoDB (local ou Docker)
- Docker (optionnel, pour MongoDB)

---

## Lancer le projet

```bash
# Restaurer les dépendances
dotnet restore

# Builder la solution
dotnet build

# Lancer l'API
dotnet run --project OpenQR.API

# Lancer l'interface web
dotnet run --project OpenQR.Web
```

---

## Commandes importantes

```bash
# Build
dotnet build OpenQR.sln

# Tests
dotnet test OpenQR.sln

# Restauration des packages
dotnet restore OpenQR.sln

# Build en mode Release
dotnet build OpenQR.sln -c Release

# Publier l'API
dotnet publish OpenQR.API -c Release -o ./publish/api

# Publier le Web
dotnet publish OpenQR.Web -c Release -o ./publish/web
```

---

## Packages NuGet clés

| Package | Version | Usage |
|---|---|---|
| `Mediator.SourceGenerator` | 3.x | Source generator mediator |
| `Mediator.Abstractions` | 3.x | Abstractions mediator |
| `MongoDB.Driver` | 3.x | Driver MongoDB |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.x | Auth JWT |
| `MudBlazor` | latest | UI Blazor |

> Toutes les versions sont centralisées dans `Directory.Packages.props`.

---

## Conventions importantes

- Champs privés : `_camelCase`
- Namespaces : file-scoped
- Async : toujours suffix `Async` + `ConfigureAwait(false)`
- Commits : Conventional Commits (`feat:`, `fix:`, `docs:`, etc.)
- **Jamais MediatR** — utiliser `martinothamar/Mediator` uniquement

---

## Contribuer

1. Fork le projet
2. Créer une branche `feat/ma-fonctionnalite`
3. Committer avec Conventional Commits
4. Ouvrir une Pull Request vers `main`

---

## Configuration MongoDB (développement)

```bash
# Démarrer MongoDB avec Docker
docker run -d -p 27017:27017 --name openqr-mongo mongo:latest
```

Configuration dans `appsettings.Development.json` :
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "openqr"
  }
}
```
