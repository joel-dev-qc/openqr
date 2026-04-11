# Étape 04 — Senior Cloud Architect — Diagrammes & Architecture Cloud

**Statut** : ⏳ À faire  
**Agent** : [Senior Cloud Architect](.github/agents/arch.agent.md)

## Objectif

Produire les diagrammes d'architecture (C4, séquences, déploiement) et valider l'architecture cloud/Kubernetes avant l'implémentation. Cet agent ne génère pas de code.

## Input requis

- [PRD v1.1.0](../01-prd/prd.md)
- [Revue d'architecture](../03-architect-review/architecture-review.md)

## Diagrammes à produire (Mermaid)

- **C4 Level 1** — Vue système (OpenQR, CDN, MongoDB, OAuth2, clients)
- **C4 Level 2** — Vue conteneurs (API, Web Blazor, Redirect Service, Worker analytique, MongoDB)
- **C4 Level 3** — Vue composants de l'API (endpoints, handlers Mediator, repositories)
- **Séquence** — Génération QR statique (client → API → CDN)
- **Séquence** — Scan QR dynamique (utilisateur → Redirect Service → IMemoryCache → MongoDB → 302)
- **Séquence** — Modification destination à chaud (admin → API → IMemoryCache invalidation)
- **Déploiement** — Architecture Kubernetes (pods, HPA, ingress, CDN, MongoDB Atlas)

## NFR à valider

- Latence redirection: p95 < 10 ms
- Disponibilité: 99,9% SLA mensuel
- Scaling: nouveaux pods prêts en < 60 secondes
- Cache CDN hit rate ≥ 95%

## Livrables attendus

- `diagrams/` — Fichiers Mermaid (.md) pour chaque diagramme
- `nfr.md` — Validation des non-functional requirements

## Prochaine étape

→ [05 — Technical Writing](../05-technical-writing/README.md)
