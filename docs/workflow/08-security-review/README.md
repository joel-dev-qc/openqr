# Étape 08 — SE: Security — Revue de sécurité

**Statut** : ⏳ À faire (avant chaque release publique)  
**Agent** : [SE: Security](.github/agents/se-security-reviewer.agent.md)

## Objectif

Effectuer une revue de sécurité complète basée sur l'OWASP Top 10, Zero Trust et les bonnes pratiques JWT/MongoDB avant toute mise en production.

## Quand l'utiliser

- Avant la release de chaque phase majeure
- Avant toute release publique
- Après l'ajout de nouveaux endpoints d'authentification ou d'autorisation

## Points de vérification prioritaires pour OpenQR

| Risque | Zone |
|---|---|
| Injection NoSQL | Requêtes MongoDB avec inputs utilisateur |
| Broken Authentication | Validation JWT, expiration, `mfa_verified` flag |
| Broken Access Control | Scoping clé API au projet, rôles `AuthorizeView` |
| Security Misconfiguration | CORS, headers sécurité, appsettings secrets |
| Cryptographic Failures | Stockage hashé des clés API, pas d'IP persistée |
| SSRF | Validation des URLs soumises pour les QR dynamiques |
| Rate Limiting bypass | Contournement du rate limit par rotation d'IP |
| Redirect abuse | Open redirect via le service de redirection (`302`) |

## Livrables attendus

- `security-report.md` — Rapport de revue avec findings et remédiation
- Issues GitHub créées pour chaque finding critique ou majeur

## Prochaine étape

→ Release de la phase ou retour à [06 — Implémentation](../06-implementation/README.md)
