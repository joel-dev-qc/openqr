# API Reference

The OpenQR REST API follows versioning convention `/api/v1/...`.

Interactive documentation is available via **Scalar** at `/scalar` when running the API.

## Authentication

All protected endpoints require a JWT Bearer token.

```http
Authorization: Bearer <token>
```

### Roles

| Role | Access |
|------|--------|
| `admin` | Full access — create, read, update, delete |
| `user` | Read-only access to own QR codes |

### Get a Token

```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "your-password"
}
```

**Response**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresAt": "2026-03-01T00:00:00Z"
}
```

---

## QR Codes

### List QR Codes

```http
GET /api/v1/qrcodes
Authorization: Bearer <token>
```

### Get QR Code

```http
GET /api/v1/qrcodes/{id}
Authorization: Bearer <token>
```

### Create QR Code

```http
POST /api/v1/qrcodes
Authorization: Bearer <token>
Content-Type: application/json

{
  "url": "https://example.com",
  "label": "My link"
}
```

### Delete QR Code

```http
DELETE /api/v1/qrcodes/{id}
Authorization: Bearer <token>
```

---

## Health

```http
GET /health
```

Returns health status of all registered checks (MongoDB, etc.).

---

## Error Responses

All errors follow RFC 9457 Problem Details format:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "The URL field is required.",
  "traceId": "00-abc123..."
}
```
