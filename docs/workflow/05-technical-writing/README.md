# Étape 05 — SE: Technical Writer — Documentation de base

**Statut** : ⏳ À faire  
**Agent** : [SE: Tech Writer](.github/agents/se-technical-writer.agent.md)

## Objectif

Produire la documentation technique de référence (API, architecture, getting started) pendant que l'architecture est fraîche, avant que le code ne soit écrit. La doc sera mise à jour au fil des phases d'implémentation.

## Input requis

- [PRD v1.1.0](../01-prd/prd.md)
- [Diagrammes d'architecture](../04-cloud-architecture/diagrams/)
- [ADRs](../03-architect-review/adr/)

## Documents à produire / mettre à jour

| Document | Destination | Description |
|---|---|---|
| API Reference | `docs/API-Reference.md` | Tous les endpoints avec exemples request/response |
| Architecture | `docs/Architecture.md` | Vue d'ensemble technique avec diagrammes C4 |
| Getting Started | `docs/Getting-Started.md` | Guide d'intégration pour un nouveau client API |
| Configuration | `docs/Configuration.md` | Variables d'environnement, appsettings |
| Development Guide | `docs/Development-Guide.md` | Setup local, conventions, workflow de contribution |

## Prochaine étape

→ [06 — Implémentation TDD](../06-implementation/README.md)
