# Getting Started

## Prerequisites

| Tool | Minimum Version |
|------|----------------|
| .NET SDK | 10.0 |
| MongoDB | 7.0+ |
| Docker (optional) | 24+ |

## Clone

```bash
git clone https://github.com/your-org/openqr.git
cd openqr
```

## Configuration

Copy the example settings and fill in your values:

```bash
cp src/OpenQR.API/appsettings.Development.json.example src/OpenQR.API/appsettings.Development.json
```

> See [Configuration](Configuration) for all available settings.

## Run with Docker Compose (recommended)

```bash
docker compose up --build
```

This starts:
- MongoDB on port `27017`
- OpenQR API on port `5000`
- OpenQR Web on port `5001`

## Run Locally

```bash
# Start MongoDB (or use Atlas connection string in appsettings)
# Terminal 1 — API
cd src/OpenQR.API
dotnet run

# Terminal 2 — Web
cd src/OpenQR.Web
dotnet run
```

## First Steps

1. Open the admin UI at `http://localhost:5001`
2. Sign in with the default admin account (configured in `appsettings.Development.json`)
3. Create your first QR code from the Dashboard

## Run Tests

```bash
dotnet test
```
