# OpenQR Prompt Templates

A collection of reusable prompt templates for common OpenQR development tasks.

---

## 1. Create a MongoDB Model

```
Create a Mongoose model for [ModelName] in `src/models/[modelName].model.js`.

Requirements:
- Fields: [list fields with types and constraints]
- Enable timestamps (createdAt, updatedAt)
- Add indexes on: [list indexed fields]
- Export the model as the default export

Follow the conventions in `.github/copilot-instructions.md`.
```

### Example

```
Create a Mongoose model for QrCode in `src/models/qrCode.model.js`.

Requirements:
- Fields: url (String, required), shortCode (String, required, unique),
  format (String, enum: ['png', 'svg'], default: 'png'),
  scans (Number, default: 0)
- Enable timestamps (createdAt, updatedAt)
- Add indexes on: shortCode, createdAt
- Export the model as the default export
```

---

## 2. Generate QR Code Logic

```
Write a service function `generate[Name]QrCode` in `src/services/qrService.js` that:

1. Accepts the following parameters: [list parameters]
2. Uses the `qrcode` npm package to generate a QR code
3. Supports options: size=[default], errorCorrectionLevel=[default], type=[png|svg|base64]
4. Saves the QR code metadata to MongoDB using the QrCode model
5. Returns [describe return value]
6. Throws a descriptive error if generation or persistence fails
```

---

## 3. Write an API Endpoint

```
Create a REST endpoint [METHOD] [/path] in `src/routes/[resource].routes.js`.

Behaviour:
- [Describe what the endpoint does]
- Request body / query params: [list expected inputs]
- Success response (HTTP [code]): [describe response shape]
- Error responses: [list error codes and reasons]

Implementation:
- Add a controller function in `src/controllers/[resource].controller.js`
- Delegate business logic to `src/services/[resource]Service.js`
- Validate the input before processing
- Use the central error handler for unexpected errors
```

### Example

```
Create a REST endpoint POST /api/qr in `src/routes/qr.routes.js`.

Behaviour:
- Generates a new QR code for the provided URL
- Request body: { url: string (required), format: 'png' | 'svg' (optional, default 'png') }
- Success response (HTTP 201): { success: true, data: { shortCode, qrImage, createdAt } }
- Error responses: 400 if url is missing, 500 for unexpected errors

Implementation:
- Add a controller function in `src/controllers/qr.controller.js`
- Delegate generation to `src/services/qrService.js`
- Validate the input before processing
- Use the central error handler for unexpected errors
```

---

## 4. Write Tests

### Unit Test

```
Write a Jest unit test file `tests/unit/[module].test.js` for `src/[path]/[module].js`.

Cover the following cases:
1. [Happy path description]
2. [Edge case description]
3. [Error / failure case description]

- Mock all external dependencies (MongoDB, `qrcode`, etc.)
- Use `beforeEach` / `afterEach` to reset mocks
- Follow the AAA pattern (Arrange, Act, Assert)
```

### Integration Test

```
Write a Jest integration test file `tests/integration/[resource].routes.test.js` for the
[METHOD] [/path] endpoint.

Setup:
- Use `mongodb-memory-server` to spin up an in-memory MongoDB instance
- Start the Express app before all tests and close it after

Test cases:
1. [Success scenario]
2. [Validation error scenario]
3. [Not found / conflict scenario]

Assert HTTP status codes, response bodies, and database state where applicable.
```

---

## 5. Add Input Validation

```
Add express-validator middleware to validate the request for [METHOD] [/path].

Validate:
- [field]: [rule description]
- [field]: [rule description]

Return HTTP 400 with `{ success: false, errors: [...] }` if validation fails.
Place the validation middleware in `src/middleware/validate[Resource].js`.
```
