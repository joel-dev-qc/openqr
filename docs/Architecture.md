# Architecture

OpenQR follows **Clean Architecture** — dependencies always point inward toward the domain.

## Layer Overview

```
┌──────────────────────────────────────────────────────┐
│                  OpenQR.Web                          │  Blazor Interactive Auto + MudBlazor
│         OpenQR.API  (Composition Root)               │  Carter endpoints, JWT, OpenAPI
├──────────────────────────────────────────────────────┤
│              OpenQR.Infrastructure                   │  MongoDB, repositories, external services
├──────────────────────────────────────────────────────┤
│               OpenQR.Application                     │  Use Cases, Mediator handlers, DTOs, Validators
├──────────────────────────────────────────────────────┤
│                 OpenQR.Domain                        │  Entities, Value Objects, Domain Events
└──────────────────────────────────────────────────────┘
```

## Dependency Rule

```
Domain ← Application ← Infrastructure
                  ↑           ↑
               API / Web (composition root)
```

No arrow may point **downward** toward Domain. `OpenQR.Application` has **zero** references to `OpenQR.Infrastructure`.

## Projects

### OpenQR.Domain
- Entities and Aggregate Roots
- Value Objects (`record` types)
- Domain Events
- Repository interfaces (ports)
- No external NuGet dependencies

### OpenQR.Application
- Mediator Commands and Queries (`IRequest<T>`)
- Mediator Handlers (`IRequestHandler<TRequest, TResponse>`)
- DTOs (`*Dto`)
- Validation (FluentValidation)
- Service interfaces (ports consumed by Infrastructure)

### OpenQR.Infrastructure
- MongoDB repositories (implementing Application interfaces)
- JWT service implementation
- External service adapters
- DI registration (`IServiceCollection` extensions)

### OpenQR.API
- Carter endpoint modules (`ICarterModule`)
- Middleware (ProblemDetails, Auth)
- OpenAPI / Scalar configuration
- Health checks
- Serilog + OpenTelemetry setup
- Composition root — wires all layers together

### OpenQR.Web
- Blazor Interactive Auto pages and components
- MudBlazor UI components
- HTTP clients (REST → API)
- Client-side JWT auth state management

```
OpenQR.Web/
├── Core/           # Interfaces, models, abstractions
├── Infrastructure/ # HTTP clients, auth services
├── Components/     # Reusable MudBlazor components
└── Pages/          # Dashboard, QR Codes, Users...
```

## Mediator Pattern

OpenQR uses [`martinothamar/Mediator`](https://github.com/martinothamar/Mediator) (source-generated, **not** MediatR).

```
CartER endpoint
    → sends IRequest<T> (Command or Query)
        → Mediator dispatches to IRequestHandler
            → Handler calls Application interfaces
                → Infrastructure implements those interfaces
```

## Key Design Decisions

| Decision | Rationale |
|----------|-----------|
| `martinothamar/Mediator` over MediatR | Source-generated, zero reflection, better performance |
| MongoDB.Driver v3 | Latest driver with async-first API |
| Carter over minimal APIs | Modular endpoint organization |
| Blazor Interactive Auto | Supports both SSR and WebAssembly interactivity |
| `sealed` classes everywhere | Prevents unintended inheritance; better JIT optimization |
| `ValueTask<T>` in handlers | Reduces allocations for frequent synchronous paths |
