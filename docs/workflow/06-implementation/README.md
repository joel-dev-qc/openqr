# Étape 06 — Implémentation (TDD par phase)

**Statut** : ⏳ À faire  
**Agent** : Agent par défaut (GitHub Copilot)

## Objectif

Implémenter le produit feature par feature, phase par phase, en suivant l'approche TDD (tests d'abord). Chaque phase se termine par un cycle Refactor → Security avant de passer à la suivante.

## Phases d'implémentation

| Phase | Semaines | Contenu |
|---|---|---|
| [Phase 1](./phase-1/) | 1–3 | Foundation & Core API |
| [Phase 2](./phase-2/) | 4–6 | Redirect Service & QR Dynamique |
| [Phase 3](./phase-3/) | 7–9 | UI Admin Core (Blazor + Auth) |
| [Phase 4](./phase-4/) | 10–12 | Bulk Operations, Analytics & Dashboard |
| [Phase 5](./phase-5/) | 13–16 | Hardening & Production Readiness |

## Workflow par phase

```
1. Lire le plan de la phase (../02-plan-mode/plan.md)
2. Écrire les tests (xUnit) pour la feature
3. Implémenter le code minimum pour faire passer les tests
4. Aller à l'étape 07 (TDD Refactor) avant de commencer la phase suivante
5. Aller à l'étape 08 (Security Review) avant chaque release
```

## Conventions de code (rappel)

- `martinothamar/Mediator` uniquement — jamais `MediatR`
- `ConfigureAwait(false)` dans toutes les couches non-UI
- Namespaces file-scoped obligatoires
- `TreatWarningsAsErrors=true`
- MongoDB.Driver v3.x uniquement

## Prochaine étape après chaque phase

→ [07 — TDD Refactor](../07-tdd-refactor/README.md)
