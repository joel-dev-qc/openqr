# Étape 07 — TDD Refactor — Qualité & Design

**Statut** : ⏳ À faire (après chaque phase d'implémentation)  
**Agent** : [TDD Refactor Phase - Improve Quality & Security](.github/agents/tdd-refactor.agent.md)

## Objectif

Améliorer la qualité du code, appliquer les principes SOLID, renforcer la sécurité et corriger les code smells — **tout en gardant les tests verts**. À exécuter à la fin de chaque phase d'implémentation.

## Quand l'utiliser

- Fin de la Phase 1 → Refactor Phase 1
- Fin de la Phase 2 → Refactor Phase 2
- Fin de la Phase 3 → Refactor Phase 3
- Fin de la Phase 4 → Refactor Phase 4
- Fin de la Phase 5 → Refactor Phase 5 (avant la release)

## Ce que l'agent vérifie

- **SOLID** : SRP (classes à responsabilité unique), DIP (injection de dépendances), OCP (extensibilité sans modification)
- **Clean Code** : nommage, longueur des méthodes, complexité cyclomatique
- **Async/Await** : `ConfigureAwait(false)`, pas de `.Result` ou `.Wait()`, suffix `Async`
- **Sécurité basique** : pas de secrets en clair, validation des inputs aux boundaries
- **Performance** : suppressions de lookups inutiles, requêtes MongoDB non-indexées

## Contrainte absolue

Les tests unitaires et d'intégration doivent rester verts après chaque refactoring.

## Prochaine étape

→ [08 — Security Review](../08-security-review/README.md) (avant chaque release)  
→ [06 — Implémentation](../06-implementation/README.md) (phase suivante)
