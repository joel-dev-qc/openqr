# Spécification API — OpenQR

Tous les endpoints sont préfixés par `/api/v1`. Les réponses sont au format **JSON**.

## Authentification

Les endpoints protégés nécessitent un token JWT passé dans l'en-tête HTTP :

```
Authorization: Bearer <token>
```

---

## QR Codes

### `POST /api/v1/qr` — Créer un QR code

Crée un nouveau QR code et le persiste en base de données.

**Authentification requise** : oui

**Corps de la requête**

```json
{
  "type": "url",
  "content": "https://example.com",
  "isDynamic": true,
  "customization": {
    "foregroundColor": "#000000",
    "backgroundColor": "#ffffff",
    "moduleShape": "square",
    "errorCorrectionLevel": "M"
  }
}
```

| Champ | Type | Requis | Description |
|---|---|---|---|
| `type` | `string` | oui | Type de QR code (`url`, `text`, `vcard`, `wifi`, `email`, `sms`, `geo`) |
| `content` | `string` | oui | Contenu à encoder dans le QR code |
| `isDynamic` | `boolean` | non | `true` pour un QR code dynamique (défaut : `false`) |
| `customization` | `object` | non | Options de personnalisation visuelle |

**Réponse — `201 Created`**

```json
{
  "id": "64f1a2b3c4d5e6f7a8b9c0d1",
  "type": "url",
  "content": "https://example.com",
  "isDynamic": true,
  "redirectUrl": "https://openqr.io/r/abc123",
  "isActive": true,
  "scanCount": 0,
  "imageUrl": "https://openqr.io/qr/64f1a2b3c4d5e6f7a8b9c0d1.png",
  "createdAt": "2024-02-20T10:00:00.000Z"
}
```

---

### `GET /api/v1/qr/:id` — Obtenir un QR code

Récupère les informations d'un QR code par son identifiant.

**Authentification requise** : oui

**Paramètres de chemin**

| Paramètre | Type | Description |
|---|---|---|
| `id` | `string` | Identifiant MongoDB du QR code |

**Réponse — `200 OK`**

```json
{
  "id": "64f1a2b3c4d5e6f7a8b9c0d1",
  "type": "url",
  "content": "https://example.com",
  "isDynamic": true,
  "redirectUrl": "https://openqr.io/r/abc123",
  "isActive": true,
  "scanCount": 42,
  "customization": { "foregroundColor": "#000000", "backgroundColor": "#ffffff" },
  "imageUrl": "https://openqr.io/qr/64f1a2b3c4d5e6f7a8b9c0d1.png",
  "createdAt": "2024-02-20T10:00:00.000Z",
  "updatedAt": "2024-02-21T08:00:00.000Z"
}
```

**Erreurs**

| Code | Description |
|---|---|
| `404 Not Found` | QR code introuvable |
| `403 Forbidden` | Le QR code n'appartient pas à l'utilisateur connecté |

---

### `PUT /api/v1/qr/:id` — Mettre à jour un QR code

Met à jour le contenu ou les paramètres d'un QR code existant.

**Authentification requise** : oui

**Paramètres de chemin**

| Paramètre | Type | Description |
|---|---|---|
| `id` | `string` | Identifiant MongoDB du QR code |

**Corps de la requête** *(tous les champs sont optionnels)*

```json
{
  "content": "https://nouvelle-url.com",
  "isActive": false,
  "customization": {
    "foregroundColor": "#1a1a2e"
  }
}
```

**Réponse — `200 OK`** : objet QR code mis à jour (même format que `GET /api/v1/qr/:id`)

---

### `DELETE /api/v1/qr/:id` — Supprimer un QR code

Supprime définitivement un QR code et toutes ses données de scan associées.

**Authentification requise** : oui

**Paramètres de chemin**

| Paramètre | Type | Description |
|---|---|---|
| `id` | `string` | Identifiant MongoDB du QR code |

**Réponse — `204 No Content`** : aucun corps de réponse

---

### `GET /api/v1/qr` — Lister les QR codes

Récupère la liste paginée des QR codes de l'utilisateur connecté.

**Authentification requise** : oui

**Paramètres de requête (query)**

| Paramètre | Type | Défaut | Description |
|---|---|---|---|
| `page` | `number` | `1` | Numéro de page |
| `limit` | `number` | `20` | Nombre d'éléments par page |
| `type` | `string` | — | Filtrer par type de QR code |
| `isDynamic` | `boolean` | — | Filtrer par type statique/dynamique |

**Réponse — `200 OK`**

```json
{
  "data": [ /* tableau d'objets QR code */ ],
  "pagination": {
    "page": 1,
    "limit": 20,
    "total": 57,
    "totalPages": 3
  }
}
```

---

### `GET /api/v1/qr/:id/scans` — Obtenir les statistiques de scan

Retourne les données analytiques de scan pour un QR code dynamique.

**Authentification requise** : oui

**Paramètres de chemin**

| Paramètre | Type | Description |
|---|---|---|
| `id` | `string` | Identifiant MongoDB du QR code |

**Paramètres de requête (query)**

| Paramètre | Type | Défaut | Description |
|---|---|---|---|
| `from` | `string` (ISO 8601) | — | Date de début de la période |
| `to` | `string` (ISO 8601) | — | Date de fin de la période |
| `groupBy` | `string` | `day` | Agrégation : `hour`, `day`, `week`, `month` |

**Réponse — `200 OK`**

```json
{
  "qrCodeId": "64f1a2b3c4d5e6f7a8b9c0d1",
  "totalScans": 42,
  "timeline": [
    { "date": "2024-02-20", "count": 15 },
    { "date": "2024-02-21", "count": 27 }
  ],
  "byCountry": [
    { "country": "FR", "count": 30 },
    { "country": "CA", "count": 12 }
  ],
  "byDevice": [
    { "type": "mobile", "count": 35 },
    { "type": "desktop", "count": 7 }
  ]
}
```

---

## Authentification

### `POST /api/v1/auth/register` — Inscription

Crée un nouveau compte utilisateur.

**Authentification requise** : non

**Corps de la requête**

```json
{
  "email": "utilisateur@example.com",
  "password": "motDePasseSecurisé123!",
  "displayName": "Jean Dupont"
}
```

| Champ | Type | Requis | Description |
|---|---|---|---|
| `email` | `string` | oui | Adresse email valide et unique |
| `password` | `string` | oui | Mot de passe (min. 8 caractères) |
| `displayName` | `string` | non | Nom d'affichage |

**Réponse — `201 Created`**

```json
{
  "id": "64f1a2b3c4d5e6f7a8b9c0a0",
  "email": "utilisateur@example.com",
  "displayName": "Jean Dupont",
  "plan": "free",
  "createdAt": "2024-02-20T10:00:00.000Z"
}
```

**Erreurs**

| Code | Description |
|---|---|
| `409 Conflict` | Un compte existe déjà avec cet email |
| `422 Unprocessable Entity` | Données invalides (email mal formé, mot de passe trop court, etc.) |

---

### `POST /api/v1/auth/login` — Connexion

Authentifie un utilisateur et retourne un token JWT.

**Authentification requise** : non

**Corps de la requête**

```json
{
  "email": "utilisateur@example.com",
  "password": "motDePasseSecurisé123!"
}
```

**Réponse — `200 OK`**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-02-27T10:00:00.000Z",
  "user": {
    "id": "64f1a2b3c4d5e6f7a8b9c0a0",
    "email": "utilisateur@example.com",
    "displayName": "Jean Dupont",
    "plan": "free"
  }
}
```

**Erreurs**

| Code | Description |
|---|---|
| `401 Unauthorized` | Email ou mot de passe incorrect |

---

## Format des erreurs

Toutes les erreurs retournent un corps JSON structuré :

```json
{
  "error": {
    "code": "QR_NOT_FOUND",
    "message": "Le QR code demandé est introuvable.",
    "status": 404
  }
}
```

---

## Codes d'erreur métier

| Code | Statut HTTP | Description |
|---|---|---|
| `QR_NOT_FOUND` | 404 | QR code introuvable |
| `QR_ACCESS_DENIED` | 403 | Accès refusé au QR code |
| `QR_LIMIT_REACHED` | 403 | Quota de QR codes atteint pour ce plan |
| `USER_ALREADY_EXISTS` | 409 | Email déjà utilisé |
| `INVALID_CREDENTIALS` | 401 | Identifiants incorrects |
| `VALIDATION_ERROR` | 422 | Données de la requête invalides |
| `INTERNAL_ERROR` | 500 | Erreur interne du serveur |
