# GitHub Copilot Instructions for OpenQR

## Project Overview

**OpenQR** is an open-source QR code solution that allows users to generate, manage, and track QR codes. The project exposes a REST API and persists data using **MongoDB**.

## Tech Stack

- **Database**: MongoDB (accessed via Mongoose ODM)
- **Runtime**: Node.js
- **API Layer**: REST (Express.js)
- **QR Code Generation**: `qrcode` npm package (or equivalent)
- **Testing**: Jest

## Project Structure

```
openqr/
├── src/
│   ├── models/        # Mongoose schemas and models
│   ├── routes/        # Express route handlers
│   ├── controllers/   # Business logic
│   ├── services/      # QR code generation and external integrations
│   └── utils/         # Shared helpers and utilities
├── tests/             # Unit and integration tests
└── .github/           # GitHub Copilot configuration
```

## Coding Conventions

- Use **ES modules** (`import`/`export`) throughout the codebase.
- Prefer `async`/`await` over raw Promises or callbacks.
- Validate all incoming request data before processing.
- Return consistent JSON responses: `{ success: true, data: ... }` on success and `{ success: false, error: "..." }` on failure.
- Use environment variables for all secrets and connection strings (never hard-code credentials).
- Keep controllers thin — delegate business logic to service functions.

## MongoDB Guidelines

- Define schemas in `src/models/` using **Mongoose**.
- Always specify `timestamps: true` on schemas to track `createdAt` and `updatedAt`.
- Use descriptive field names and add JSDoc comments to schema definitions.
- Index fields that are frequently queried (e.g., short codes, user IDs).
- Handle MongoDB connection errors gracefully and log them clearly.

## QR Code Guidelines

- Encapsulate all QR code generation logic inside `src/services/qrService.js`.
- Support configurable options (size, error correction level, format).
- Store the generated QR code metadata (URL, short code, creation date) in MongoDB.

## Testing Guidelines

- Write unit tests for services and utilities.
- Write integration tests for API routes using a MongoDB in-memory server (e.g., `mongodb-memory-server`).
- Name test files `*.test.js` and place them under `tests/`.
- Aim for meaningful coverage of happy paths and error cases.

## General Best Practices

- Follow the **Single Responsibility Principle**: each module or function does one thing well.
- Prefer descriptive variable names over short abbreviations.
- Add error handling middleware in Express for centralised error responses.
- Log meaningful messages (use a logger like `pino` or `winston` — not raw `console.log` in production code).
