# Configuration

Configuration is driven by `appsettings.json` / `appsettings.{Environment}.json` and environment variables.

## OpenQR.API — `appsettings.json`

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  },
  "MongoDB": {
    "DatabaseName": "openqr"
  },
  "Jwt": {
    "Secret": "your-256-bit-secret-key",
    "Issuer": "openqr-api",
    "Audience": "openqr-clients",
    "ExpirationMinutes": 60
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  },
  "OpenTelemetry": {
    "Endpoint": "http://localhost:4317"
  }
}
```

## Environment Variables

Environment variables override `appsettings.json` using the standard ASP.NET Core double-underscore convention.

| Variable | Overrides |
|----------|-----------|
| `ConnectionStrings__MongoDB` | MongoDB connection string |
| `MongoDB__DatabaseName` | MongoDB database name |
| `Jwt__Secret` | JWT signing secret |
| `Jwt__ExpirationMinutes` | Token lifetime in minutes |
| `OpenTelemetry__Endpoint` | OTLP collector endpoint |

## MongoDB

OpenQR uses **MongoDB.Driver v3.x**.

Collections:

| Collection | Purpose |
|-----------|---------|
| `qr_codes` | QR code documents |
| `users` | User accounts |

## JWT

- Algorithm: `HS256`
- The `Jwt__Secret` must be at least 32 characters
- Roles are embedded in the token claims

## Environments

| Environment | Appsettings file |
|------------|-----------------|
| Development | `appsettings.Development.json` |
| Production | `appsettings.json` + environment variables |

> **Never commit secrets.** Use environment variables or a secrets manager in production.
