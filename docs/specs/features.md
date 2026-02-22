# Fonctionnalités — OpenQR

## Légende des statuts

| Statut | Description |
|---|---|
| 🟢 **done** | Fonctionnalité implémentée |
| 🟡 **in progress** | En cours de développement |
| 🔵 **planned** | Planifiée, pas encore démarrée |

---

## 1. Génération de QR codes

| Fonctionnalité | Description | Statut |
|---|---|---|
| QR code de type URL | Génère un QR code pointant vers une URL web | 🔵 planned |
| QR code de type texte | Génère un QR code encodant du texte libre | 🔵 planned |
| QR code de type vCard | Génère un QR code encodant une fiche contact (nom, téléphone, email, etc.) | 🔵 planned |
| QR code de type WiFi | Génère un QR code permettant de se connecter à un réseau WiFi | 🔵 planned |
| QR code de type email | Génère un QR code pré-remplissant un email | 🔵 planned |
| QR code de type SMS | Génère un QR code pré-remplissant un SMS | 🔵 planned |
| QR code de type géolocalisation | Génère un QR code encodant des coordonnées GPS | 🔵 planned |
| Export en PNG / SVG | Permet de télécharger le QR code généré en image | 🔵 planned |

---

## 2. Gestion des QR codes (CRUD)

| Fonctionnalité | Description | Statut |
|---|---|---|
| Création d'un QR code | Crée et persiste un QR code en base de données | 🔵 planned |
| Lecture d'un QR code | Récupère les informations et l'image d'un QR code existant | 🔵 planned |
| Mise à jour d'un QR code | Modifie le contenu ou les paramètres d'un QR code existant | 🔵 planned |
| Suppression d'un QR code | Supprime définitivement un QR code | 🔵 planned |
| Listage des QR codes | Récupère la liste des QR codes d'un utilisateur avec pagination | 🔵 planned |

---

## 3. QR codes dynamiques vs statiques

| Fonctionnalité | Description | Statut |
|---|---|---|
| QR code statique | Le contenu est encodé directement dans le QR code et ne peut pas être modifié | 🔵 planned |
| QR code dynamique | Le QR code pointe vers une URL de redirection ; le contenu cible peut être modifié sans regénérer le QR code | 🔵 planned |
| Activation / désactivation | Permet d'activer ou désactiver un QR code dynamique | 🔵 planned |

---

## 4. Tracking des scans (analytics)

| Fonctionnalité | Description | Statut |
|---|---|---|
| Comptage des scans | Enregistre chaque scan d'un QR code dynamique | 🔵 planned |
| Données par scan | Enregistre l'horodatage, l'adresse IP (anonymisée), le user-agent et la localisation approximative | 🔵 planned |
| Statistiques agrégées | Fournit le nombre total de scans, les scans par jour/semaine/mois | 🔵 planned |
| Répartition géographique | Affiche d'où proviennent les scans (pays, région) | 🔵 planned |
| Répartition par appareil | Distingue les scans selon le type d'appareil (mobile, desktop, tablette) | 🔵 planned |

---

## 5. Gestion des utilisateurs et authentification

| Fonctionnalité | Description | Statut |
|---|---|---|
| Inscription | Création d'un compte utilisateur avec email et mot de passe | 🔵 planned |
| Connexion | Authentification et délivrance d'un token de session | 🔵 planned |
| Déconnexion | Invalidation du token de session | 🔵 planned |
| Profil utilisateur | Consultation et mise à jour des informations de profil | 🔵 planned |
| Gestion des rôles | Différents niveaux d'accès (admin, utilisateur standard, etc.) | 🔵 planned |
| Plans / quotas | Limitation du nombre de QR codes selon le plan de l'utilisateur | 🔵 planned |

---

## 6. Personnalisation visuelle des QR codes

| Fonctionnalité | Description | Statut |
|---|---|---|
| Couleur des modules | Personnalisation de la couleur des points du QR code | 🔵 planned |
| Couleur de fond | Personnalisation de la couleur de fond du QR code | 🔵 planned |
| Intégration d'un logo | Ajout d'un logo ou d'une image au centre du QR code | 🔵 planned |
| Forme des modules | Choix de la forme des points (carré, rond, etc.) | 🔵 planned |
| Niveau de correction d'erreur | Choix du niveau de correction (L, M, Q, H) | 🔵 planned |

---

## 7. API et intégration

| Fonctionnalité | Description | Statut |
|---|---|---|
| API REST documentée | Tous les endpoints documentés avec exemples de requêtes/réponses | 🔵 planned |
| Authentification par token | Accès à l'API via un token JWT ou une clé API | 🔵 planned |
| Versionnage de l'API | Les endpoints sont versionnés (`/api/v1/...`) pour assurer la compatibilité | 🔵 planned |
| Webhooks | Notification en temps réel lors d'un scan (TODO: à confirmer) | 🔵 planned |
