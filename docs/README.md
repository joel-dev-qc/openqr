# Documentation OpenQR

Bienvenue dans la documentation officielle du projet **OpenQR**, une solution open source de génération et de gestion de QR codes.

## Description du projet

OpenQR permet de créer, gérer et analyser des QR codes de différents types (URL, texte, vCard, WiFi, etc.). La solution repose sur **MongoDB** comme base de données et expose une API REST pour toutes les opérations.

## Table des matières

| Document | Description |
|---|---|
| [Vue d'ensemble](./specs/overview.md) | Présentation générale du projet, objectifs et stack technologique |
| [Fonctionnalités](./specs/features.md) | Liste détaillée des fonctionnalités prévues et leur statut |
| [Modèles de données](./specs/data-models.md) | Schémas MongoDB pour les entités principales |
| [Spécification API](./specs/api-spec.md) | Documentation des endpoints REST API |
| [Architecture](./specs/architecture.md) | Architecture technique, composants et choix technologiques |

## Structure du dossier `docs/`

```
docs/
├── README.md                  # Index de la documentation (ce fichier)
└── specs/
    ├── overview.md            # Vue d'ensemble de l'application
    ├── features.md            # Liste des fonctionnalités
    ├── data-models.md         # Modèles de données MongoDB
    ├── api-spec.md            # Spécification des endpoints API
    └── architecture.md        # Architecture technique
```

## Contribuer à la documentation

Pour contribuer à la documentation, veuillez ouvrir une *pull request* avec vos modifications. Toute la documentation est rédigée en **français** et au format **Markdown**.
