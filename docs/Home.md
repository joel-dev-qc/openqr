# OpenQR — Wiki

**OpenQR** is an open-source QR Code management solution built on .NET 10 and Blazor.

## What is OpenQR?

OpenQR provides a full REST API and a Blazor admin interface for creating, managing, and tracking QR codes. It follows Clean Architecture principles and is designed to be self-hosted.

## Features

- 🔗 Create and manage QR codes via REST API
- 📊 Admin dashboard with Blazor Interactive Auto
- 🔐 JWT-based authentication with role support (`admin` / `user`)
- 🍃 MongoDB persistence via MongoDB.Driver v3
- 📡 OpenTelemetry tracing and Serilog structured logging
- 🌡️ Health checks endpoint

## Tech Stack

| Layer | Technology |
|-------|-----------|
| API Framework | .NET 10 / ASP.NET Core |
| Messaging | `martinothamar/Mediator` |
| Database | MongoDB (Driver v3.x) |
| Auth | JWT Bearer |
| Admin UI | Blazor Interactive Auto + MudBlazor |
| Observability | Serilog + OpenTelemetry |

## Quick Links

- [Getting Started](Getting-Started)
- [Architecture](Architecture)
- [API Reference](API-Reference)
- [Configuration](Configuration)
- [Development Guide](Development-Guide)
- [Contributing](Contributing)
