# Modèles de données — OpenQR

Ce document décrit les schémas MongoDB pour les entités principales de l'application OpenQR. Les schémas sont présentés sous forme de pseudo-schéma Mongoose avec des exemples JSON.

---

## 1. Entité `QRCode`

Représente un QR code créé par un utilisateur.

### Schéma Mongoose (pseudo-code)

```js
{
  _id: ObjectId,                  // Identifiant unique MongoDB
  userId: ObjectId,               // Référence vers l'utilisateur propriétaire (ref: 'User')
  type: String,                   // Type de QR code : 'url' | 'text' | 'vcard' | 'wifi' | 'email' | 'sms' | 'geo'
  content: String,                // Contenu encodé dans le QR code (URL, texte, etc.)
  isDynamic: Boolean,             // true = QR code dynamique (URL de redirection), false = statique
  redirectUrl: String,            // URL de redirection (uniquement pour les QR codes dynamiques)
  isActive: Boolean,              // Indique si le QR code est actif (pour les QR codes dynamiques)
  scanCount: Number,              // Nombre total de scans enregistrés
  customization: {
    foregroundColor: String,      // Couleur des modules (ex: '#000000')
    backgroundColor: String,      // Couleur de fond (ex: '#FFFFFF')
    logoUrl: String,              // URL du logo intégré au centre (optionnel)
    moduleShape: String,          // Forme des modules : 'square' | 'rounded' | 'circle'
    errorCorrectionLevel: String, // Niveau de correction d'erreur : 'L' | 'M' | 'Q' | 'H'
  },
  createdAt: Date,                // Date de création
  updatedAt: Date,                // Date de dernière modification
}
```

### Exemple JSON

```json
{
  "_id": "64f1a2b3c4d5e6f7a8b9c0d1",
  "userId": "64f1a2b3c4d5e6f7a8b9c0a0",
  "type": "url",
  "content": "https://example.com",
  "isDynamic": true,
  "redirectUrl": "https://openqr.io/r/abc123",
  "isActive": true,
  "scanCount": 42,
  "customization": {
    "foregroundColor": "#1a1a2e",
    "backgroundColor": "#ffffff",
    "logoUrl": "https://cdn.example.com/logo.png",
    "moduleShape": "rounded",
    "errorCorrectionLevel": "M"
  },
  "createdAt": "2024-01-15T10:30:00.000Z",
  "updatedAt": "2024-02-01T08:15:00.000Z"
}
```

---

## 2. Entité `User`

Représente un utilisateur enregistré dans l'application.

### Schéma Mongoose (pseudo-code)

```js
{
  _id: ObjectId,          // Identifiant unique MongoDB
  email: String,          // Adresse email (unique, indexée)
  passwordHash: String,   // Mot de passe hashé (bcrypt ou argon2)
  displayName: String,    // Nom d'affichage (optionnel)
  role: String,           // Rôle : 'user' | 'admin'
  plan: String,           // Plan d'abonnement : 'free' | 'pro' | 'enterprise'
  qrCodeLimit: Number,    // Nombre maximum de QR codes autorisés selon le plan
  isActive: Boolean,      // Indique si le compte est actif
  createdAt: Date,        // Date de création du compte
  updatedAt: Date,        // Date de dernière modification
  lastLoginAt: Date,      // Date de la dernière connexion
}
```

### Exemple JSON

```json
{
  "_id": "64f1a2b3c4d5e6f7a8b9c0a0",
  "email": "utilisateur@example.com",
  "passwordHash": "$2b$10$...",
  "displayName": "Jean Dupont",
  "role": "user",
  "plan": "free",
  "qrCodeLimit": 10,
  "isActive": true,
  "createdAt": "2024-01-10T09:00:00.000Z",
  "updatedAt": "2024-01-10T09:00:00.000Z",
  "lastLoginAt": "2024-02-20T14:22:00.000Z"
}
```

---

## 3. Entité `Scan`

Représente un événement de scan enregistré pour un QR code dynamique.

### Schéma Mongoose (pseudo-code)

```js
{
  _id: ObjectId,          // Identifiant unique MongoDB
  qrCodeId: ObjectId,     // Référence vers le QR code scanné (ref: 'QRCode')
  timestamp: Date,        // Horodatage du scan
  ipAddress: String,      // Adresse IP anonymisée (ex: '192.168.1.xxx')
  userAgent: String,      // User-agent du navigateur / appareil
  device: {
    type: String,         // Type d'appareil : 'mobile' | 'tablet' | 'desktop' | 'unknown'
    os: String,           // Système d'exploitation détecté (ex: 'iOS', 'Android', 'Windows')
    browser: String,      // Navigateur détecté (ex: 'Chrome', 'Safari')
  },
  location: {
    country: String,      // Code pays ISO 3166-1 alpha-2 (ex: 'FR', 'CA')
    region: String,       // Région ou province (optionnel)
    city: String,         // Ville (optionnel)
  },
}
```

### Exemple JSON

```json
{
  "_id": "64f1a2b3c4d5e6f7a8b9c0e5",
  "qrCodeId": "64f1a2b3c4d5e6f7a8b9c0d1",
  "timestamp": "2024-02-20T14:35:22.000Z",
  "ipAddress": "192.168.1.xxx",
  "userAgent": "Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15",
  "device": {
    "type": "mobile",
    "os": "iOS",
    "browser": "Safari"
  },
  "location": {
    "country": "FR",
    "region": "Île-de-France",
    "city": "Paris"
  }
}
```

---

## Index MongoDB recommandés

```js
// QRCode : recherche par utilisateur
db.qrcodes.createIndex({ userId: 1, createdAt: -1 });

// QRCode : recherche par URL de redirection (QR codes dynamiques)
db.qrcodes.createIndex({ redirectUrl: 1 }, { unique: true, sparse: true });

// User : recherche par email (unique)
db.users.createIndex({ email: 1 }, { unique: true });

// Scan : recherche par QR code pour les analytics
db.scans.createIndex({ qrCodeId: 1, timestamp: -1 });
```

---

## Notes

- Les mots de passe ne sont **jamais** stockés en clair ; seul le hash est persisté.
- Les adresses IP sont **anonymisées** avant stockage pour respecter la vie privée (dernier octet masqué).
- Les champs `createdAt` et `updatedAt` sont gérés automatiquement par Mongoose (`timestamps: true`).
- Les relations entre collections utilisent des `ObjectId` (pas de jointures, conformément au paradigme MongoDB).
