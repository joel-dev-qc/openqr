# OpenQR — Workflow de développement

Ce fichier décrit l'ordre d'exécution des étapes pour développer OpenQR du PRD à la production, en utilisant les agents GitHub Copilot disponibles dans ce repo.

---

## Ordre d'exécution

```
01-prd  ✅  → 02-plan-mode → 03-architect-review → 04-cloud-architecture
                                                              ↓
08-security-review ← 07-tdd-refactor ← 06-implementation ← 05-technical-writing
      ↑___________________________|  (boucle par phase)
```

---

## Étapes & statuts

| # | Étape | Agent | Statut |
|---|---|---|---|
| 01 | [PRD](docs/workflow/01-prd/README.md) | Create PRD | ✅ Complété |
| 02 | [Plan Mode](docs/workflow/02-plan-mode/README.md) | Plan Mode - Strategic Planning | ⏳ À faire |
| 03 | [Architect Review](docs/workflow/03-architect-review/README.md) | SE: Architect | ⏳ À faire |
| 04 | [Cloud Architecture](docs/workflow/04-cloud-architecture/README.md) | Senior Cloud Architect | ⏳ À faire |
| 05 | [Technical Writing](docs/workflow/05-technical-writing/README.md) | SE: Tech Writer | ⏳ À faire |
| 06 | [Implémentation TDD](docs/workflow/06-implementation/README.md) | Agent par défaut | ⏳ À faire |
| 07 | [TDD Refactor](docs/workflow/07-tdd-refactor/README.md) | TDD Refactor Phase | ⏳ À faire (par phase) |
| 08 | [Security Review](docs/workflow/08-security-review/README.md) | SE: Security | ⏳ À faire (par release) |

---

## Règle de boucle

Les étapes **06 → 07 → 08** se répètent à la fin de **chaque phase d'implémentation** (5 phases au total).  
Les étapes **01 à 05** sont exécutées **une seule fois** en début de projet.

---

## Structure des livrables

```
docs/workflow/
├── 01-prd/
│   ├── README.md
│   └── prd.md                    ✅ PRD v1.1.0 (26 user stories)
├── 02-plan-mode/
│   ├── README.md
│   └── plan.md                   ← à produire
├── 03-architect-review/
│   ├── README.md
│   ├── architecture-review.md    ← à produire
│   └── adr/                      ← ADRs à produire
├── 04-cloud-architecture/
│   ├── README.md
│   ├── nfr.md                    ← à produire
│   └── diagrams/                 ← diagrammes Mermaid à produire
├── 05-technical-writing/
│   └── README.md
├── 06-implementation/
│   ├── README.md
│   ├── phase-1/                  ← notes & logs Phase 1
│   ├── phase-2/
│   ├── phase-3/
│   ├── phase-4/
│   └── phase-5/
├── 07-tdd-refactor/
│   └── README.md
└── 08-security-review/
    ├── README.md
    └── security-report.md        ← à produire avant chaque release
```

---

## Prochaine action

**Étape 02 — Plan Mode** : Ouvrir GitHub Copilot Chat, sélectionner l'agent **"Plan Mode - Strategic Planning & Architecture"** et fournir le [PRD](docs/workflow/01-prd/prd.md) comme contexte.
