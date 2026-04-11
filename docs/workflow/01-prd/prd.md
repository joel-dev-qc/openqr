# PRD: OpenQR - Enterprise QR Code Management Platform

## 1. Product overview

### 1.1 Document title and version

- PRD: OpenQR - Enterprise QR Code Management Platform
- Version: 1.1.0

### 1.2 Product summary

OpenQR est une plateforme enterprise de gestion de QR codes conçue pour les grandes organisations média et de streaming OTT (Over-The-Top). Elle expose une API REST haute performance construite sur .NET 10 avec Clean Architecture, permettant aux applications clientes autorisées (web, iOS, Android, systèmes de diffusion) de générer, récupérer et servir des QR codes à grande échelle — ciblant des millions d'utilisateurs simultanés et des pics de charge élevés lors des diffusions en direct.

La plateforme supporte trois types distincts de QR codes adaptés aux besoins d'une plateforme de streaming : **statique** (URL encodée directement dans le QR, mise en cache CDN de façon immuable), **dynamique** (le QR encode une URL courte gérée par OpenQR dont la destination est modifiable sans régénérer l'image QR — idéal pour les diffusions en direct) et **personnalisé utilisateur** (généré à la demande pour chaque utilisateur avec une URL ou un token unique, sans cache). Le service de redirection intégré est l'infrastructure centrale qui rend les QR dynamiques possibles et capture les événements de scan pour l'analytique.

Elle introduit un modèle organisationnel multi-projets, permettant à une entreprise de gérer des projets indépendants sous un même périmètre organisationnel, chacun avec sa propre clé API, ses catégories, ses tags et ses gabarits visuels — tout en partageant une image de marque centralisée au niveau organisation. Une interface d'administration construite avec Blazor Interactive Auto et MudBlazor offre aux administrateurs les outils pour gérer les projets, gabarits, prévisualiser les codes en temps réel, créer des QR en masse et consulter un tableau de bord analytique. La plateforme est déployable sur Kubernetes avec support de domaines personnalisés pour les URLs de redirection.

## 2. Goals

### 2.1 Business goals

- Livrer une API QR code enterprise prête pour la production, capable de servir des millions d'utilisateurs simultanés à l'échelle d'une plateforme de streaming média.
- Supporter plusieurs projets indépendants au sein d'une même organisation pour centraliser la gestion QR à travers les marques tout en partageant une image de marque organisationnelle commune.
- Permettre la modification de la destination d'un QR code sans régénérer ni réimprimer l'image, via un service de redirection intégré gérant des URLs courtes — critique pour les environnements de diffusion en direct.
- Permettre la distribution CDN des QR codes statiques et dynamiques pour minimiser la charge sur l'origin et réduire le time-to-first-byte.
- Fournir une base pouvant évoluer vers une offre SaaS multi-tenant en V2 sans migration de données.
- Réduire l'effort d'intégration pour les équipes techniques clientes en standardisant la génération QR derrière une API unique et bien documentée.

### 2.2 User goals

- **Développeurs de projets** : Intégrer la génération de QR codes dans leurs systèmes (broadcast, web, mobile) avec un seul appel API authentifié par clé API, sans gérer la logique de rendu QR ni le service de redirection eux-mêmes.
- **Équipes de contenu** : Modifier la destination d'un QR code déjà diffusé (ex. : changer la page de destination d'une émission en cours) depuis l'UI admin sans intervention technique.
- **Utilisateurs finaux (téléspectateurs)** : Scanner des QR codes affichés à l'écran qui se chargent rapidement, sont visuellement brandés au projet et redirigent de façon fiable vers la bonne destination.
- **Administrateurs d'organisation** : Gérer les projets, gabarits de marque globaux et permissions depuis une interface admin propre et responsive sans intervention des développeurs.
- **Administrateurs de projet** : Personnaliser l'apparence visuelle des QR codes pour leur projet via des gabarits avec prévisualisation en temps réel et gérer leur propre domaine de redirection personnalisé.

### 2.3 Non-goals

- **Isolation de données multi-tenant stricte** (bases de données ou schémas séparés par organisation) : reporté à la V2.
- **Gestion de la facturation et des abonnements** : reporté à la V2.
- **Routage conditionnel post-scan** (redirection différente selon l'appareil, l'heure ou la géolocalisation) : non pertinent pour le cas d'usage streaming ; reporté à la V2.
- **Intégrations avec des outils marketing externes** (Meta Pixel, Google Ads, Zapier, Salesforce, Canva) : reporté à la V2.
- **Analytics en temps réel** (latence sous la seconde) : les analytics sont asynchrones avec un délai acceptable de quelques minutes.
- **Onboarding organisation en libre-service** : la création d'organisation est gérée par un administrateur global en V1.

## 3. User personas

### 3.1 Key user types

- Administrateur global
- Administrateur d'organisation
- Administrateur de projet (lecture/écriture)
- Observateur de projet (lecture seule)
- Application cliente API (automatisée, non-humaine)
- Utilisateur final / scanner de QR (consommateur)

### 3.2 Basic persona details

- **Maxime — Administrateur global** : Un ingénieur plateforme interne de l'entreprise média qui a un accès complet à toutes les organisations, projets et paramètres système. Il crée les organisations, provisionne les projets, gère les clés API et surveille la santé de la plateforme.
- **Sophie — Administratrice d'organisation** : Une cheffe de produit numérique qui gère tous les projets sous son organisation. Elle peut créer/modifier des projets, assigner des administrateurs et définir des gabarits de marque globaux partagés entre tous les projets.
- **Étienne — Administrateur de projet (lecture/écriture)** : Un développeur ou chef de produit responsable d'un projet spécifique. Il gère les catégories QR, les gabarits de projet, et peut générer et consulter les QR codes de son projet.
- **Nadia — Observatrice de projet (lecture seule)** : Une analyste d'affaires assignée à un projet qui peut consulter les QR codes, tableaux de bord analytiques et configurations de gabarits sans pouvoir effectuer de modifications.
- **App Streaming — Client API** : Une application iOS/web automatisée qui appelle l'API REST OpenQR en utilisant une clé API pour demander la génération de QR codes à l'exécution pour des pages de contenu dynamique.
- **Utilisateur final / Scanner QR** : Un téléspectateur d'une plateforme de streaming qui scanne un QR code depuis son écran TV ou du matériel imprimé. Il ignore l'existence d'OpenQR ; il s'attend simplement à une redirection rapide et fiable vers le bon contenu.

### 3.3 Role-based access

- **global-admin** : Accès complet à toutes les organisations, tous les projets, tous les paramètres, tous les gabarits et toutes les analytics. Peut créer/supprimer des organisations et des projets. Peut gérer toutes les clés API.
- **org-admin** : Accès complet à tous les projets de son organisation assignée. Peut créer/modifier/supprimer des projets au sein de l'organisation. Peut gérer les gabarits globaux (niveau organisation). Ne peut pas accéder aux autres organisations.
- **project-admin** : Accès complet en lecture/écriture à un projet spécifique ou un ensemble de projets assignés. Peut gérer les catégories, les gabarits de projet et consulter les analytics de ses projets. Ne peut pas accéder aux autres projets.
- **project-viewer** : Accès en lecture seule aux projets assignés. Peut consulter les QR codes, gabarits, catégories et tableaux de bord analytiques. Ne peut pas créer, modifier ou supprimer de ressource.
- **Client API (clé API)** : Scopé à un seul projet. Peut appeler les endpoints de génération et récupération de QR. Ne peut pas accéder aux ressources admin ni aux données des autres projets.

## 4. Functional requirements

- **Service de redirection et URLs courtes** (Priorité : Haute)
  - Fournir un service de redirection HTTP intégré qui intercepte les scans de QR codes dynamiques, enregistre l'événement de scan et redirige vers la destination configurée.
  - Émettre une URL courte unique par QR code dynamique au format `https://{domain}/{shortCode}` (ex. : `https://qr.monprojet.io/abc123`).
  - Supporter des domaines personnalisés par organisation ou projet (ex. : `qr.monprojet.io`, `qr.autreprojet.io`) configurables dans l'UI admin.
  - Le domaine par défaut est celui de la plateforme (`qr.openqr.io` ou équivalent configuré) ; les projets peuvent surcharger avec leur domaine personnalisé.
  - Les redirections HTTP retournent le code `302 Found` avec le header `Location` ; les URLs courtes ne sont pas mises en cache par le navigateur pour permettre la mise à jour de la destination.

- **Génération QR — Type statique** (Priorité : Haute)
  - Accepter une URL en entrée et retourner un QR code qui encode directement l'URL cible (sans passer par le service de redirection).
  - Mettre en cache le QR généré indéfiniment sur le CDN avec un header `Cache-Control: immutable`.
  - Idéal pour les URLs permanentes qui ne changeront jamais (ex. : landing page pérenne, page institutionnelle).
  - Supporter l'invalidation manuelle du cache via un appel API admin qui déclenche un purge CDN.
  - Stocker les métadonnées QR dans MongoDB avec le `projectId` source, `urlHash` et `slug`.

- **Génération QR — Type dynamique** (Priorité : Haute)
  - Générer un QR code qui encode l'URL courte OpenQR du service de redirection, permettant de changer la destination à tout moment sans régénérer ni réimprimer le QR.
  - Stocker la destination (URL cible) dans MongoDB séparément de l'image QR ; la modification de la destination est une opération d'écriture simple qui prend effet immédiatement sur le prochain scan.
  - Mettre en cache l'image QR sur le CDN (la structure du QR ne change pas, seule la destination dans MongoDB change).
  - Idéal pour les QR codes affichés en direct (broadcast TV, affichages) dont la destination peut changer entre les émissions.
  - Capturer les événements de scan au niveau du service de redirection pour l'analytique.

- **Génération QR — Type personnalisé utilisateur** (Priorité : Haute)
  - Accepter une URL contenant des données spécifiques à l'utilisateur (ex. : un token ou ID utilisateur intégré dans le chemin ou query string de l'URL).
  - Générer le QR code à la demande pour chaque requête ; ne pas mettre en cache au niveau CDN (`Cache-Control: no-store`).
  - Supporter un temps d'expiration configurable pour le QR généré (`expiresIn` en secondes, défaut : 3600).
  - Un QR personnalisé expiré retourne HTTP 410 Gone lorsqu'on y accède après l'expiration.
  - Retourner le QR de façon synchrone dans la réponse HTTP.

- **Formats de sortie multiples** (Priorité : Haute)
  - Supporter les formats de sortie : SVG, PNG, WebP, JPG et PDF sélectionnables via un paramètre `format`.
  - Résolution par défaut pour les formats raster : 512×512 pixels, configurable via un paramètre `size`.
  - Retourner les headers `Content-Type` appropriés pour chaque format.
  - Les formats PDF et SVG sont particulièrement utiles pour l'impression haute résolution (broadcast, affichage physique).

- **Score de scannabilité** (Priorité : Moyenne)
  - Calculer et afficher un score de scannabilité (0–100) pour chaque QR code généré, basé sur le contraste des couleurs, la densité des données et la taille du logo central.
  - Afficher un indicateur visuel (vert/orange/rouge) dans l'UI admin et dans la réponse API pour alerter si le score est insuffisant (< 70).
  - Le score est recalculé lors de toute modification du gabarit visuel.

- **Modèle organisationnel multi-projets** (Priorité : Haute)
  - Supporter plusieurs projets au sein d'une organisation, chacun avec un nom unique, un slug, une clé API et un domaine de redirection personnalisé optionnel.
  - Permettre à chaque projet d'avoir son propre ensemble de catégories et de tags QR pour organiser les codes générés.
  - Stocker tous les QR codes avec leur `projectId` d'origine quel que soit le projet dont la clé API a été utilisée.
  - Indexer les documents MongoDB sur `(projectId, urlHash)`, `(projectId, slug)` et `(organizationId, projectId)`.

- **Tags et catégories** (Priorité : Moyenne)
  - Supporter des catégories (taxonomie hiérarchique avec nom unique par projet) pour organiser les QR codes par type de contenu ou campagne.
  - Supporter des tags libres (liste de chaînes, non unique) pour un étiquetage flexible et transversal des QR codes.
  - Permettre le filtrage par tag et catégorie dans la liste QR et les analytics.

- **Création de QR codes en masse** (Priorité : Moyenne)
  - Permettre la création de plusieurs QR codes en une seule opération via import CSV ou appel API batch (`POST /api/v1/qr/batch`).
  - Le fichier CSV supporte les colonnes : `url`, `type`, `format`, `templateId`, `category`, `tags`, `slug` (optionnel).
  - Retourner un rapport d'import indiquant les succès, les erreurs de validation et les doublons.
  - Limite configurable de QR codes par import (défaut : 500 par batch).

- **Authentification par clé API** (Priorité : Haute)
  - Émettre une clé API par projet au format `proj_live_{random}`.
  - Valider les clés API en mémoire (IMemoryCache) avec une latence cible inférieure à 1 ms.
  - Appliquer un rate limiting par clé API pour protéger la plateforme des abus.
  - Supporter la rotation de clé API (émettre une nouvelle clé, déprécier l'ancienne avec une période de grâce configurable).

- **Authentification admin OAuth2/OIDC avec MFA** (Priorité : Haute)
  - Intégrer Google comme fournisseur OAuth2/OIDC principal pour l'UI admin.
  - Supporter l'extensibilité pour des fournisseurs supplémentaires (Azure AD, Okta) sans modifications du cœur applicatif.
  - Émettre un JWT interne après le callback OAuth2 réussi, intégrant les rôles utilisateur et les scopes de projets assignés.
  - Stocker la session dans un cookie HttpOnly pour l'interface admin Blazor.
  - Supporter l'authentification multi-facteurs (MFA/TOTP) comme couche additionnelle optionnelle pour les comptes admin.

- **Gabarits visuels avec frame CTA** (Priorité : Haute)
  - Supporter des gabarits de niveau organisation (globaux) partagés entre tous les projets d'une organisation.
  - Supporter des gabarits de niveau projet scoped à un projet spécifique, surchargeant ou complétant le gabarit global.
  - Propriétés configurables du gabarit : logo central, couleur de premier plan, couleur d'arrière-plan, border radius des modules, style des points (carré, rond, dots), style des coins, frame CTA (texte d'appel à l'action autour du QR, ex. : "Scannez pour regarder").
  - Aucune limite sur le nombre de gabarits par projet.
  - Les gabarits sont sélectionnables au moment de la génération QR via un paramètre `templateId`.

- **Domaines personnalisés** (Priorité : Moyenne)
  - Permettre à chaque organisation ou projet de configurer un domaine personnalisé pour les URLs courtes de redirection.
  - Le service de redirection valide que le domaine personnalisé est bien configuré (vérification DNS CNAME) avant activation.
  - La configuration du domaine personnalisé est gérée dans l'UI admin par un org-admin ou project-admin.

- **Détection d'anomalies de scan** (Priorité : Basse)
  - Détecter les patterns de scans anormaux par QR code (ex. : pic de volume inhabituel, scans répétés depuis la même localisation en boucle rapide).
  - Alerter l'administrateur du projet via une notification dans l'UI admin lorsqu'une anomalie est détectée.
  - Les anomalies sont journalisées avec les métriques associées pour investigation.

- **CRUD Admin — Projets** (Priorité : Haute)
  - Créer, lire, mettre à jour et supprimer des projets au sein d'une organisation.
  - Gérer les clés API de projet (afficher, faire tourner, révoquer).
  - Configurer le domaine personnalisé du projet.
  - Assigner et retirer des administrateurs et observateurs de projet.

- **CRUD Admin — Catégories et Tags** (Priorité : Moyenne)
  - Créer, lire, mettre à jour et supprimer des catégories QR au sein d'un projet.
  - Gérer les tags depuis la vue liste QR (ajout/suppression inline).
  - Assigner une catégorie à un QR code au moment de la génération ou via mise à jour.

- **CRUD Admin — Gabarits** (Priorité : Haute)
  - Créer, lire, mettre à jour et supprimer des gabarits visuels au niveau organisation et projet.
  - Prévisualiser le gabarit appliqué à un QR code exemple en temps réel dans l'UI admin.

- **Modification de la destination d'un QR dynamique** (Priorité : Haute)
  - Permettre à un project-admin de modifier la destination (URL cible) d'un QR dynamique depuis l'UI admin sans régénérer l'image QR.
  - La modification prend effet immédiatement sur le prochain scan sans purge CDN (l'image QR ne change pas).
  - Journaliser l'historique des changements de destination avec l'identité de l'acteur et le timestamp.

- **Liste et recherche QR** (Priorité : Moyenne)
  - Lister tous les QR codes d'un projet avec pagination et filtrage par catégorie, type, tags et plage de dates.
  - Rechercher des QR codes par URL, slug ou tag.

- **Analytics et suivi des scans** (Priorité : Moyenne)
  - Enregistrer chaque événement de scan QR de façon asynchrone (fire-and-forget via canal interne) avec : QR ID, timestamp, type d'appareil (mobile/tablette/desktop), géolocalisation approximative (pays/région).
  - Traiter les événements de scan en arrière-plan avec un délai acceptable de quelques minutes.
  - Exposer un tableau de bord analytique dans l'UI admin Blazor montrant les comptes de scans, répartition par appareils, distribution géographique et tendances dans le temps.

- **Tableau de bord admin** (Priorité : Moyenne)
  - Afficher des métriques clés par projet : total QR codes, total scans, top 10 QR codes par nombre de scans.
  - Filtrer les analytics par plage de dates et projet.

## 5. User experience

### 5.1 Entry points & first-time user flow

- Un développeur d'application cliente reçoit sa clé API de projet depuis un administrateur d'organisation via l'UI admin.
- Le développeur lit la documentation API et effectue son premier appel `POST /api/v1/qr` avec le header `X-API-Key`.
- En cas de succès, il reçoit l'image QR, le `shortCode` et l'URL courte de redirection (pour les QR dynamiques) et commence à l'intégrer dans son pipeline de diffusion.
- Pour l'UI admin, un administrateur navigue vers l'app Blazor, clique sur "Se connecter avec Google", complète le flux OAuth2 (avec MFA si activé) et arrive sur le tableau de bord.
- Un org-admin en première connexion voit un guide de configuration : créer votre premier projet, configurer un domaine personnalisé, configurer un gabarit global et copier la clé API.

### 5.2 Core experience

- **Génération QR (API)** : L'application cliente envoie une requête POST avec l'URL cible, le type QR, le format souhaité et un `templateId` optionnel. L'API répond en moins de 50 ms (p95) pour les types mis en cache et en moins de 200 ms pour les QR personnalisés utilisateur à la demande. La réponse inclut l'image QR et, pour les QR dynamiques, l'URL courte de redirection.
  - Les réponses mises en cache sur le CDN sont servies directement depuis le CDN sur les requêtes suivantes, offrant une latence sous 10 ms aux clients finaux.
- **Modification de destination à chaud (UI Admin)** : Pour un QR dynamique déjà diffusé, un administrateur peut naviguer vers le QR dans l'UI admin et modifier l'URL cible. La modification prend effet immédiatement sur le prochain scan, sans toucher à l'image QR déjà en diffusion.
- **Prévisualisation de gabarit (UI Admin)** : Lorsqu'un administrateur crée ou modifie un gabarit (incluant la frame CTA), la page admin Blazor affiche une prévisualisation en direct d'un QR code exemple avec le style configuré, se mettant à jour en temps réel à chaque changement de paramètre ainsi que le score de scannabilité.
- **Tableau de bord analytique (UI Admin)** : L'administrateur sélectionne un projet et une plage de dates. Les métriques de scan sont affichées via des graphiques et cartes de synthèse, donnant une visibilité sur la performance des QR et la géographie de l'audience.
- **Gestion des clés API (UI Admin)** : Les administrateurs de projet peuvent voir la clé API masquée, la copier dans le presse-papiers et initier une rotation (émettant une nouvelle clé pendant que l'ancienne reste valide pendant une période de grâce configurable).

### 5.3 Advanced features & edge cases

- Si un QR code est demandé pour une URL déjà générée et présente dans le CDN, l'API retourne l'URL CDN existante sans re-générer l'image, sauf pour les QR personnalisés utilisateur qui sont toujours générés à nouveau.
- Si un gabarit est supprimé alors que des QR codes y font référence, les QR codes existants conservent un snapshot du gabarit au moment de la génération ; ils ne cassent pas.
- Les clients API soumis au rate limiting reçoivent une réponse `429 Too Many Requests` avec un header `Retry-After`.
- Si un fournisseur OAuth2 est indisponible lors de la connexion admin, l'erreur est affichée via une notification MudBlazor `ISnackbar` conviviale plutôt qu'une page 500 générique.
- Si l'ingestion des analytics prend du retard, les événements de scan sont mis en buffer dans le canal interne ; aucun événement n'est perdu dans des conditions normales de fonctionnement.
- Les QR codes personnalisés expirés retournent une réponse `410 Gone` lorsqu'un consommateur tente d'y accéder après l'expiration.

### 5.4 UI/UX highlights

- **Standard de design** : l'interface admin Blazor suit le standard **Material Design 3** (MD3 — [m3.material.io](https://m3.material.io/)) tel qu'implémenté par MudBlazor. Un design token system (palette couleur, typographie, shape, spacing) est défini dans `docs/design-system/` et appliqué uniformément à tous les composants. Les thèmes clair et sombre sont tous deux supportés.
- Prévisualisation QR en temps réel dans l'éditeur de gabarits avec score de scannabilité inline et indicateur `MudProgressLinear` pendant le re-rendu.
- `MudDataGrid` pour toutes les vues en liste (QR codes, projets, gabarits, catégories) avec pagination et tri côté serveur ; colonne de tags avec chips cliquables pour filtrage rapide.
- Notifications `ISnackbar` pour toutes les opérations CRUD en succès ou en erreur.
- Dialogues de confirmation `IDialogService` avant toute action destructrice (suppression de projet, révocation de clé API, suppression de gabarit, changement de destination d'un QR dynamique en production).
- Composants `AuthorizeView` pour afficher conditionnellement les actions d'édition/suppression selon le rôle de l'utilisateur.
- Badge d'alerte visible sur les QR codes dont le score de scannabilité est insuffisant (< 70).
- Indicateur de statut de domaine personnalisé (en attente DNS / actif / erreur) dans les paramètres du projet.
- Mise en page responsive avec le système de grille MudBlazor, conçu desktop-first mais utilisable sur tablettes.

## 6. Narrative

Une développeure dans l'équipe de diffusion d'une plateforme de streaming prépare une émission en direct. Elle génère à l'avance un QR code dynamique via l'API OpenQR avec le gabarit visuel de son projet et le domaine personnalisé `qr.monprojet.io`. Le QR encode l'URL courte `https://qr.monprojet.io/show-xyz`. L'image QR est mise en cache sur le CDN et intégrée dans les graphiques de diffusion. Pendant l'émission, des millions de téléspectateurs voient le QR à l'écran et le scannent — OpenQR redirige chacun vers la page de l'émission en cours, avec une latence inférieure à 10 ms. En cours de diffusion, l'équipe réalise que la page de destination initiale a un problème ; un project-admin se connecte à l'UI admin, modifie la destination du QR dynamique en deux clics — sans régénérer une nouvelle image, sans intervention technique, sans interrompre la diffusion. Le lendemain matin, la cheffe d'édition consulte le tableau de bord analytique, constate que 47 000 appareils uniques ont scanné le QR — 78% depuis mobile — et utilise ces données pour planifier le prochain segment interactif.

## 7. Success metrics

### 7.1 User-centric metrics

- Latence API de génération QR : p50 < 20 ms, p95 < 50 ms pour les types mis en cache ; p95 < 200 ms pour les types personnalisés utilisateur.
- Latence du service de redirection (scan → destination) : p95 < 10 ms.
- Taux de hit CDN pour les images QR statiques et dynamiques : ≥ 95%.
- Temps de chargement des pages de l'UI admin : < 2 secondes sur une connexion haut débit standard.
- Fraîcheur des données du tableau de bord analytique : événements de scan visibles dans le tableau de bord dans les 5 minutes suivant l'occurrence.
- Latence de validation des clés API : < 1 ms (cache en mémoire).

### 7.2 Business metrics

- Nombre de projets actifs par organisation.
- Total de QR codes générés par jour/semaine/mois.
- Total de scans trackés par projet.
- Disponibilité API : SLA mensuel de 99,9%.
- Temps d'intégration pour un nouveau client API : < 30 minutes en utilisant la documentation.

### 7.3 Technical metrics

- Latence des requêtes MongoDB sur les index composites : < 5 ms pour les lectures indexées.
- Temps de réponse d'auto-scaling des pods Kubernetes : nouveaux pods prêts dans les 60 secondes suivant un pic de trafic.
- Délai de traitement du pipeline analytique : < 5 minutes sous charge normale.
- Taux d'erreur (réponses 5xx) : < 0,1% de tous les appels API.
- Utilisation mémoire pour le cache en mémoire des clés API : < 50 MB par pod.

## 8. Technical considerations

### 8.1 Integration points

- **Service de redirection** : Un composant HTTP middleware dédié (`.NET 10 minimal API` ou middleware `Carter`) dans l'API gère les scans entrants sur les domaines personnalisés ou le domaine par défaut. Il résout le `shortCode` depuis IMemoryCache (avec fallback MongoDB), capture l'événement de scan dans le canal analytique et émet la réponse `302 Found` en moins de 5 ms.
- **CDN** : Toutes les images QR statiques et dynamiques sont stockées et servies via un CDN (ex. : Cloudflare, Azure CDN). L'API déclenche le purge du cache CDN lors d'une invalidation manuelle. Les URLs de redirection (`302`) ne sont jamais mises en cache CDN.
- **Fournisseurs OAuth2/OIDC** : Google est le fournisseur principal ; l'intégration utilise les documents de découverte OIDC standards, facilitant l'ajout d'Azure AD ou Okta ultérieurement.
- **MongoDB** : Toutes les métadonnées QR, configurations de projets, gabarits, catégories, tags et événements analytiques sont stockés dans MongoDB avec le driver officiel MongoDB.Driver v3.x.
- **Cache en mémoire (IMemoryCache)** : Les clés API, destinations de QR dynamiques et gabarits fréquemment accédés sont mis en cache en mémoire par pod pour atteindre une latence de résolution sous la milliseconde sur le hot path de redirection.
- **Canal analytique interne** : Une queue producteur/consommateur `System.Threading.Channels` découple l'enregistrement des événements de scan du hot path de redirection.
- **Kubernetes** : L'application est déployée comme un ou plusieurs Deployments Kubernetes (API, Web, Redirect Service si séparé) avec auto-scaling horizontal des pods basé sur les métriques CPU/RPS.

### 8.2 Data storage & privacy

- Tous les documents MongoDB incluent les champs `organizationId` et `projectId` à la création pour permettre une isolation multi-tenant future sans migration de données.
- Les QR codes personnalisés utilisateur encodent les identifiants utilisateur (tokens ou IDs) dans l'URL elle-même ; OpenQR ne stocke pas d'informations personnellement identifiables au-delà de ce qui est intégré dans l'URL par l'application appelante.
- Les événements analytiques stockent uniquement la géolocalisation approximative (pays/région), le type d'appareil et le timestamp — aucune adresse IP n'est persistée après la résolution géo.
- Les clés API sont stockées sous forme hashée dans MongoDB ; seul le préfixe (`proj_live_`) est stocké en clair à des fins d'affichage.
- La suppression logique (soft delete) est utilisée pour les projets et gabarits pour préserver les pistes d'audit et prévenir les pertes de données accidentelles.

### 8.3 Scalability & performance

- Scaling horizontal via Kubernetes : les pods API stateless s'adaptent indépendamment selon la charge.
- Le CDN décharge la majorité du service des images QR pour les types statiques et dynamiques, réduisant les requêtes origin à quasi zéro en régime stable.
- Le hot path de redirection (`shortCode` → destination) est distinct du hot path de génération QR et est optimisé séparément via IMemoryCache, ciblant une latence < 5 ms p95.
- Les index MongoDB composites sur `(projectId, urlHash)`, `(projectId, slug)` et `(organizationId, projectId)` assurent des performances de lecture constantes à l'échelle.
- Les analytics sont traitées de façon asynchrone via un canal in-process et écrites par batch dans MongoDB, garantissant zéro impact sur le hot path de génération QR.
- IMemoryCache pour les clés API et gabarits élimine les lookups MongoDB répétés sur les chemins critiques.
- L'architecture est conçue pour une future couche Redis (cache distribué) en remplacement d'IMemoryCache lorsque le scaling horizontal nécessitera une cohérence du cache entre les pods.

### 8.5 Design system

- L'UI admin Blazor est construite sur **Material Design 3** (MD3) via MudBlazor. Un design system documenté dans `docs/design-system/` définit les tokens de couleur (MD3 color roles : `primary`, `secondary`, `tertiary`, `error`, `surface`, `on-*`), l'échelle typographique (Display / Headline / Title / Body / Label), la shape scale (border radius) et les règles d'élévation.
- Le thème MudBlazor est configuré via `MudTheme` avec `PaletteLight` et `PaletteDark` mappant les color roles MD3 aux tokens MudBlazor correspondants.
- Toute déviation de MudBlazor par rapport aux specs MD3 est documentée dans le design system avec le workaround appliqué.
- Le design system est la source de vérité pour toute décision visuelle ; les développeurs l'implémentent, les designers s'y référent.

### 8.4 Potential challenges

- **Latence du service de redirection** : Le hot path de redirection est critique (scan en direct lors d'une diffusion). Le cache IMemoryCache des destinations doit être warm ; une invalidation agressive lors d'un changement de destination est nécessaire pour garantir la cohérence immédiate entre les pods.
- **Validation DNS des domaines personnalisés** : La vérification qu'un CNAME est correctement configuré avant d'activer un domaine personnalisé requiert un polling DNS asynchrone ; les erreurs de configuration doivent être clairement signalées à l'administrateur.
- **Complexité de l'invalidation CDN** : Invalider de façon fiable les entrées CDN pour les QR codes statiques nécessite une intégration soignée avec l'API de purge du fournisseur CDN ; les échecs doivent être réessayés gracieusement.
- **Rotation de clé API avec période de grâce** : Implémenter une rotation sûre où l'ancienne et la nouvelle clé sont valides simultanément nécessite une gestion rigoureuse de l'état dans le cache de validation.
- **Snapshot du gabarit à la génération QR** : Stocker un snapshot des styles de gabarit au moment de la génération QR ajoute une surcharge de stockage mais est nécessaire pour éviter de casser les images QR existantes lors des mises à jour de gabarits.
- **Backpressure analytique** : Si le worker analytique en arrière-plan prend du retard lors de pics de trafic extrêmes, le buffer du canal interne pourrait se remplir ; une stratégie drop-oldest sur un canal borné prévient la pression mémoire.
- **Cohérence du cache IMemoryCache entre pods** : En configuration multi-pod, la modification d'une destination de QR dynamique doit invalider le cache de tous les pods ; une stratégie d'expiration courte (TTL 30s) ou une future couche Redis assure la cohérence éventuelle.
- **Évolution multi-tenant** : Bien que `organizationId` soit inclus dans tous les documents dès le début, la transition vers une isolation stricte des tenants (bases de données séparées) en V2 nécessitera tout de même des changements au niveau applicatif pour le routage et la gestion des connexions.

## 9. Milestones & sequencing

### 9.1 Project estimate

- Large : 12–16 semaines pour une V1 complète avec toutes les exigences fonctionnelles, le service de redirection, les domaines personnalisés, l'UI admin et les analytics.

### 9.2 Team size & composition

- Petite équipe (3–5 personnes) : 2 développeurs .NET backend, 1 développeur Blazor/frontend, 1 ingénieur DevOps/Kubernetes (temps partiel), 1 product owner / QA.

### 9.3 Suggested phases

- **Phase 1 — Foundation & Core API** (Semaines 1–3)
  - Scaffolding du projet : Clean Architecture, configuration MongoDB, manifests Kubernetes de base.
  - Middleware d'authentification par clé API.
  - Endpoint de génération QR statique (SVG + PNG + WebP + JPG + PDF).
  - Intégration CDN pour le cache des QR statiques.
  - Modèle de données projet et organisation de base dans MongoDB.

- **Phase 2 — Service de redirection & QR Dynamique** (Semaines 4–6)
  - Service de redirection HTTP : résolution `shortCode` → destination, capture événement scan, réponse `302`.
  - Génération QR dynamique : URL courte + destination éditable dans MongoDB.
  - Endpoint de modification de destination à chaud (`PATCH /api/v1/qr/{id}/destination`).
  - QR personnalisé utilisateur (pas de cache CDN, expiration configurable).
  - Modèle de gabarit visuel avec frame CTA : gabarits de niveau organisation et projet.
  - Score de scannabilité calculé à la génération.
  - Stockage de snapshot de gabarit.

- **Phase 3 — UI Admin Core** (Semaines 7–9)
  - Authentification OAuth2/OIDC Google pour l'admin Blazor avec support MFA.
  - JWT interne avec rôles et scopes de projets.
  - CRUD admin : organisations, projets, clés API, catégories, tags.
  - CRUD admin : gabarits avec prévisualisation QR en temps réel et score de scannabilité.
  - Gestion des domaines personnalisés (configuration + validation DNS).
  - Contrôle d'accès basé sur les rôles dans l'UI (`AuthorizeView`).

- **Phase 4 — Bulk Operations, Analytics & Dashboard** (Semaines 10–12)
  - Import CSV en masse pour la création de QR codes.
  - Tracking asynchrone des événements de scan via canal interne.
  - Worker en arrière-plan pour les écritures batch analytiques dans MongoDB.
  - Tableau de bord analytique dans l'UI admin : comptes de scans, répartition par appareils, distribution géographique.
  - Détection d'anomalies de scan (alertes dans l'UI admin).

- **Phase 5 — Hardening & Production Readiness** (Semaines 13–16)
  - Rate limiting par clé API.
  - Rotation de clé API avec période de grâce.
  - Endpoint admin d'invalidation du cache CDN.
  - Tests de performance et validation des index MongoDB (particulièrement le hot path de redirection).
  - Configuration de l'auto-scaler horizontal Kubernetes.
  - Revue de sécurité et audit OWASP Top 10.
  - Documentation API (OpenAPI/Swagger).

## 10. User stories

### 10.1. Générer un QR code statique via l'API

- **ID**: OQR-001
- **Description**: En tant qu'application cliente API, je veux envoyer une URL et recevoir un QR code statique afin de pouvoir afficher un QR code immuable identique à tous les utilisateurs pour une URL donnée.
- **Acceptance criteria**:
  - `POST /api/v1/qr` avec un header `X-API-Key` valide, `url`, `type: static` et `format: svg|png|webp|jpg|pdf` retourne HTTP 200 avec l'image QR ou une URL CDN.
  - La même URL avec le même format et le même gabarit retourne toujours la même image QR.
  - La réponse inclut une URL CDN et un header `Cache-Control: immutable` pour les formats raster.
  - Si le QR existe déjà dans le CDN, l'API retourne l'URL CDN existante sans régénérer l'image.
  - La réponse inclut le score de scannabilité (0–100) et un avertissement si ce score est inférieur à 70.
  - Le temps de réponse est inférieur à 50 ms au p95 pour les requêtes mises en cache.

### 10.2. Générer un QR code dynamique avec URL courte via l'API

- **ID**: OQR-002
- **Description**: En tant qu'application cliente API, je veux générer un QR code dynamique dont la destination est modifiable à tout moment afin de pouvoir changer la page de destination d'un QR code déjà diffusé sans régénérer ni remplacer l'image QR.
- **Acceptance criteria**:
  - `POST /api/v1/qr` avec `type: dynamic` et une URL de destination retourne HTTP 200 avec l'image QR et le `shortCode` correspondant à l'URL courte de redirection.
  - L'URL courte retournée est du format `https://{domaine-projet}/{shortCode}`.
  - L'image QR encode l'URL courte et non l'URL de destination directe.
  - La même demande avec la même URL destination retourne le même `shortCode` (idempotence).
  - Le QR est stocké dans MongoDB avec le `projectId`, `shortCode` et `destinationUrl`.
  - Le temps de réponse est inférieur à 50 ms au p95 pour les requêtes mises en cache.

### 10.3. Générer un QR code personnalisé utilisateur via l'API

- **ID**: OQR-003
- **Description**: En tant qu'application cliente API, je veux générer un QR code unique par utilisateur encodant une URL spécifique à l'utilisateur afin que chaque utilisateur reçoive une expérience personnalisée lors du scan.
- **Acceptance criteria**:
  - `POST /api/v1/qr` avec `type: personalized` et une URL contenant un token ou ID utilisateur retourne HTTP 200 avec l'image QR inline (pas d'URL CDN).
  - La réponse inclut `Cache-Control: no-store`.
  - Le QR a un temps d'expiration configurable spécifié dans la requête (`expiresIn` en secondes) ; défaut 3600 secondes.
  - La demande d'un QR personnalisé déclenche toujours une génération ; il n'est jamais servi depuis le cache.
  - Un QR personnalisé expiré retourne HTTP 410 Gone lorsqu'on y accède après l'expiration.

### 10.4. Sélectionner le format de sortie QR

- **ID**: OQR-004
- **Description**: En tant qu'application cliente API, je veux demander un QR code en format SVG, PNG, WebP, JPG ou PDF afin d'utiliser le format le plus approprié à mon contexte de rendu (écran, impression, broadcast).
- **Acceptance criteria**:
  - Le champ `format` accepte `svg`, `png`, `webp`, `jpg` et `pdf`.
  - Les réponses SVG retournent `Content-Type: image/svg+xml`.
  - Les réponses PNG retournent `Content-Type: image/png` à 512×512 pixels par défaut.
  - Les réponses WebP retournent `Content-Type: image/webp` à 512×512 pixels par défaut.
  - Les réponses JPG retournent `Content-Type: image/jpeg` à 512×512 pixels par défaut.
  - Les réponses PDF retournent `Content-Type: application/pdf` avec le QR centré sur une page A4, adapté à l'impression haute résolution.
  - Un paramètre `size` permet de surcharger la résolution raster (ex. : `size=1024`).
  - Une valeur `format` invalide retourne HTTP 400 Bad Request avec un message d'erreur descriptif.

### 10.5. Authentifier les appels API avec une clé API

- **ID**: OQR-005
- **Description**: En tant qu'application cliente API, je veux authentifier mes requêtes avec une clé API afin que seuls les projets autorisés puissent générer ou récupérer des QR codes.
- **Acceptance criteria**:
  - Tous les endpoints API requièrent le header `X-API-Key: proj_live_{token}`.
  - Une clé API manquante ou invalide retourne HTTP 401 Unauthorized.
  - Une clé API valide est résolue en mémoire en moins de 1 ms en moyenne.
  - Une clé API est scopée à exactement un projet ; les appels aux ressources d'autres projets avec cette clé retournent HTTP 403 Forbidden.
  - Les clés API révoquées retournent HTTP 401 immédiatement après révocation.

### 10.6. Appliquer un rate limiting par clé API

- **ID**: OQR-006
- **Description**: En tant qu'opérateur de plateforme, je veux appliquer des limites de débit par clé API afin qu'aucun client ne puisse submerger la plateforme.
- **Acceptance criteria**:
  - Chaque clé API a une limite de requêtes par minute configurable (défaut : 1000 RPM).
  - Les requêtes dépassant la limite retournent HTTP 429 Too Many Requests avec un header `Retry-After`.
  - Les compteurs de rate limit se réinitialisent sur une fenêtre glissante.
  - La configuration du rate limit est gérable par projet dans l'UI admin.

### 10.7. Appliquer un gabarit visuel à un QR code

- **ID**: OQR-007
- **Description**: En tant qu'application cliente API, je veux spécifier un ID de gabarit lors de la génération d'un QR code afin que le QR code résultant utilise le style de marque configuré par mon organisation ou projet.
- **Acceptance criteria**:
  - Le corps de la requête accepte un champ `templateId` optionnel.
  - Si `templateId` est fourni, le QR est rendu en utilisant le logo, couleurs et styles points/coins de ce gabarit.
  - Si `templateId` est omis, l'API utilise le gabarit par défaut du projet (si défini) ou un QR code simple.
  - Si `templateId` référence un gabarit inexistant ou inaccessible, HTTP 404 est retourné.
  - Le style de gabarit utilisé au moment de la génération est capturé comme snapshot dans le document QR.

### 10.8. Se connecter à l'UI admin via Google OAuth2 avec MFA

- **ID**: OQR-008
- **Description**: En tant qu'administrateur, je veux me connecter à l'UI admin Blazor avec mon compte Google et optionnellement un second facteur (MFA) afin de sécuriser l'accès à la plateforme de gestion.
- **Acceptance criteria**:
  - La page de connexion admin affiche un bouton "Se connecter avec Google".
  - Cliquer sur le bouton redirige l'utilisateur vers le flux OAuth2/OIDC de Google.
  - Après une authentification OAuth2 réussie, si le MFA est activé pour le compte, l'utilisateur est invité à entrer son code TOTP (ex. : Google Authenticator) avant d'accéder au tableau de bord.
  - Après authentification complète, l'utilisateur est redirigé vers le tableau de bord admin.
  - Un JWT interne contenant les rôles utilisateur, les scopes de projets et le flag `mfa_verified` est émis et stocké dans un cookie HttpOnly.
  - Si le compte Google de l'utilisateur n'est pas autorisé, HTTP 403 est affiché avec un message convivial.
  - La session expire après une période configurable (défaut : 8 heures) et redirige l'utilisateur vers la page de connexion.

### 10.9. Gérer les projets en tant qu'administrateur d'organisation

- **ID**: OQR-009
- **Description**: En tant qu'administrateur d'organisation, je veux créer, modifier et supprimer des projets dans mon organisation afin de structurer la gestion QR par marque ou produit.
- **Acceptance criteria**:
  - L'UI admin fournit une section "Projets" listant tous les projets de l'organisation.
  - Un projet peut être créé avec un nom, un slug et une description optionnelle.
  - Un projet peut être modifié (nom, description) ou supprimé de façon logique (soft delete).
  - La suppression d'un projet nécessite un dialogue de confirmation via `IDialogService`.
  - Chaque projet a exactement une clé API affichée (masquée) avec des options pour copier et faire tourner.
  - La création d'un projet génère automatiquement une clé API au format `proj_live_{random}`.
  - Le slug doit être unique au sein de l'organisation ; les slugs dupliqués retournent une erreur de validation.

### 10.10. Faire tourner une clé API de projet

- **ID**: OQR-010
- **Description**: En tant qu'administrateur d'organisation, je veux faire tourner la clé API d'un projet afin de pouvoir invalider une clé compromise tout en laissant aux applications clientes le temps de se mettre à jour.
- **Acceptance criteria**:
  - L'action de rotation de clé API émet une nouvelle clé immédiatement et marque l'ancienne comme "en rotation" avec une période de grâce (défaut : 24h, configurable).
  - Pendant la période de grâce, l'ancienne et la nouvelle clé sont toutes deux valides.
  - Après la période de grâce, l'ancienne clé est automatiquement révoquée.
  - L'UI admin affiche le statut de rotation et l'heure d'expiration de la période de grâce.
  - Une notification `ISnackbar` confirme la rotation réussie.

### 10.11. Gérer les catégories et tags QR au sein d'un projet

- **ID**: OQR-011
- **Description**: En tant qu'administrateur de projet, je veux créer et gérer des catégories et des tags au sein de mon projet afin d'organiser les QR codes de façon structurée (catégories) et flexible (tags libres).
- **Acceptance criteria**:
  - L'UI admin permet de créer, renommer et supprimer des catégories au sein d'un projet.
  - La suppression d'une catégorie contenant des QR codes affiche un avertissement et permet la réassignation ou la suppression en masse.
  - Les catégories sont listées dans un `MudDataGrid` avec les colonnes : nom, nombre de QR, date de création.
  - Un nom de catégorie doit être unique au sein du projet ; les doublons retournent une erreur de validation.
  - Les tags sont gérés directement depuis la vue détail ou la liste QR (chips input avec autocomplétion des tags existants du projet).
  - Les tags sont libres (aucune contrainte d'unicité) et peuvent être appliqués à plusieurs QR codes simultanément.

### 10.12. Créer un gabarit visuel de niveau organisation

- **ID**: OQR-012
- **Description**: En tant qu'administrateur d'organisation, je veux créer un gabarit global pour mon organisation afin que tous les projets puissent utiliser un style de marque cohérent par défaut.
- **Acceptance criteria**:
  - L'UI admin fournit un éditeur de gabarit avec des champs pour : logo central (upload d'image), couleur de premier plan, couleur d'arrière-plan, border radius des modules, style des points (carré, rond, dots), style des coins et frame CTA (texte autour du QR, ex. : "Scannez pour regarder").
  - Une prévisualisation QR en direct se rend en temps réel à mesure que les propriétés du gabarit changent, avec le score de scannabilité affiché en permanence.
  - Sauvegarder le gabarit le rend disponible à tous les projets de l'organisation.
  - Un gabarit d'organisation peut être défini comme défaut de l'organisation, appliqué quand aucun `templateId` n'est spécifié et qu'aucun défaut de projet n'est défini.
  - Les noms de gabarits doivent être uniques au sein de l'organisation.

### 10.13. Créer un gabarit visuel de niveau projet

- **ID**: OQR-013
- **Description**: En tant qu'administrateur de projet, je veux créer un gabarit spécifique à mon projet afin que mes QR codes aient un look personnalisé différent du défaut de l'organisation.
- **Acceptance criteria**:
  - Les paramètres du projet dans l'UI admin incluent une section "Gabarits" pour les gabarits de niveau projet.
  - Un éditeur de gabarit de projet supporte les mêmes champs de personnalisation que l'éditeur de gabarit d'organisation.
  - Un gabarit de projet peut être défini comme défaut du projet.
  - Les gabarits de projet ne sont pas visibles ni utilisables par d'autres projets.
  - Il n'y a pas de limite sur le nombre de gabarits par projet.

### 10.14. Prévisualiser un QR code avec un gabarit en temps réel

- **ID**: OQR-014
- **Description**: En tant qu'administrateur de projet, je veux prévisualiser un QR code avec un gabarit sélectionné appliqué en temps réel afin de vérifier le résultat visuel avant publication.
- **Acceptance criteria**:
  - L'éditeur de gabarit inclut un panneau de prévisualisation qui affiche un QR code généré avec les paramètres actuels du gabarit.
  - La prévisualisation se met à jour dans les 500 ms suivant tout changement de propriété de gabarit.
  - Un indicateur `MudProgressLinear` est affiché pendant le rendu de la prévisualisation.
  - La prévisualisation utilise une URL placeholder si aucune URL spécifique n'est fournie.

### 10.15. Consulter la liste des QR codes générés dans un projet

- **ID**: OQR-015
- **Description**: En tant qu'administrateur de projet, je veux voir une liste paginée de tous les QR codes générés pour mon projet afin de les auditer, rechercher et gérer.
- **Acceptance criteria**:
  - La liste QR est affichée dans un `MudDataGrid` avec les colonnes : slug, URL/shortCode, type, format, catégorie, tags (chips), date de création, nombre de scans, score de scannabilité.
  - La liste supporte la pagination côté serveur (défaut 25 par page).
  - Un filtrage est disponible par type (statique/dynamique/personnalisé), catégorie, tag et plage de dates.
  - Un champ de recherche filtre les QR codes par URL, slug ou tag.
  - Sélectionner un QR code ouvre une vue détail montrant l'URL complète (ou le shortCode + destination pour les QR dynamiques), le gabarit utilisé, l'historique des changements de destination et une image de prévisualisation inline.

### 10.16. Invalider manuellement un QR code mis en cache sur le CDN

- **ID**: OQR-016
- **Description**: En tant qu'administrateur d'organisation, je veux invalider manuellement un QR code mis en cache sur le CDN afin que le contenu mis à jour puisse être servi sans attendre l'expiration naturelle du cache.
- **Acceptance criteria**:
  - La vue détail du QR code dans l'UI admin inclut un bouton "Invalider le cache" (visible pour org-admin et global-admin).
  - Cliquer sur le bouton déclenche un purge CDN pour l'URL CDN du QR spécifique et marque le QR comme nécessitant une régénération.
  - Une notification `ISnackbar` confirme le succès ou signale un échec avec une suggestion de réessai.
  - L'invalidation du cache est journalisée avec l'identité de l'acteur et le timestamp.

### 10.17. Consulter les analytics de scan d'un projet

- **ID**: OQR-017
- **Description**: En tant qu'administrateur de projet, je veux consulter les analytics de mes QR codes afin de comprendre l'engagement, l'utilisation des appareils et la distribution géographique.
- **Acceptance criteria**:
  - Le tableau de bord analytique affiche le total des scans, les QR codes uniques scannés et le top 10 des QR codes par nombre de scans pour une période sélectionnée.
  - Un graphique de répartition des appareils affiche le pourcentage de scans depuis mobile, tablette et desktop.
  - Une vue de distribution géographique affiche les scans par pays et région.
  - Le sélecteur de plage de dates permet de filtrer par les 7 derniers jours, 30 jours, 90 jours ou une plage personnalisée.
  - Les données analytiques sont actualisées avec un délai maximum de 5 minutes à partir du moment du scan.

### 10.18. Tracker un événement de scan QR de façon asynchrone

- **ID**: OQR-018
- **Description**: En tant que système, je veux enregistrer les événements de scan de façon asynchrone afin que les données analytiques soient collectées sans impacter la performance de génération QR.
- **Acceptance criteria**:
  - Lorsqu'un QR code est accédé, un événement de scan est écrit dans un canal interne `System.Threading.Channels` sans bloquer la réponse HTTP.
  - Un worker en arrière-plan consomme les événements depuis le canal et les écrit par batch dans MongoDB.
  - Les événements de scan incluent : QR ID, timestamp (UTC), type d'appareil, pays et région.
  - Les adresses IP ne sont pas persistées ; la géolocalisation est résolue au moment de l'écriture et jetée.
  - Le worker en arrière-plan traite les événements dans les 5 minutes sous charge normale.
  - Si le canal est plein, les événements sont abandonnés selon une stratégie drop-oldest pour prévenir la pression mémoire.

### 10.19. Assigner un rôle d'administrateur de projet

- **ID**: OQR-019
- **Description**: En tant qu'administrateur d'organisation, je veux assigner un rôle d'administrateur de projet à un utilisateur afin qu'il puisse gérer son projet spécifique de façon indépendante.
- **Acceptance criteria**:
  - L'UI admin permet d'inviter un utilisateur à un projet par adresse e-mail avec un rôle `project-admin` ou `project-viewer`.
  - L'utilisateur invité reçoit un accès accordé lors de sa prochaine connexion via OAuth2.
  - Un org-admin peut changer le rôle d'un utilisateur ou retirer son accès à tout moment.
  - La page de paramètres du projet affiche la liste de tous les utilisateurs avec accès et leurs rôles.
  - Un utilisateur avec le rôle `project-viewer` peut naviguer mais ne peut modifier aucune ressource du projet.

### 10.20. Accéder à toutes les organisations en tant qu'administrateur global

- **ID**: OQR-020
- **Description**: En tant qu'administrateur global, je veux accéder à toutes les organisations et projets depuis l'UI admin afin de fournir un support et une supervision au niveau de la plateforme.
- **Acceptance criteria**:
  - Un administrateur global voit toutes les organisations et tous les projets dans la navigation admin.
  - Un administrateur global peut créer et supprimer des organisations.
  - Un administrateur global peut créer des projets au sein de n'importe quelle organisation.
  - Un administrateur global peut voir et faire tourner la clé API de n'importe quel projet.
  - Toutes les actions d'un administrateur global sont journalisées avec l'identité de l'acteur et le timestamp à des fins d'audit.

### 10.21. Modifier la destination d'un QR code dynamique à chaud

- **ID**: OQR-021
- **Description**: En tant qu'administrateur de projet, je veux modifier la destination d'un QR code dynamique déjà diffusé sans régénérer l'image QR afin de corriger ou mettre à jour le contenu cible en production, même pendant une diffusion en direct.
- **Acceptance criteria**:
  - La vue détail d'un QR dynamique dans l'UI admin inclut un champ "Destination" éditable directement.
  - La modification de la destination est enregistrée via `PATCH /api/v1/qr/{id}/destination` et prend effet immédiatement sur le prochain scan (pas de purge CDN requis — l'image QR ne change pas).
  - Le service de redirection invalide le cache IMemoryCache du `shortCode` concerné lors de la mise à jour.
  - Un dialogue de confirmation `IDialogService` est présenté avant la modification d'un QR dont le nombre de scans récents est élevé.
  - L'historique de chaque changement de destination est journalisé (ancienne URL, nouvelle URL, acteur, timestamp) et affiché dans la vue détail.
  - Une notification `ISnackbar` confirme la modification réussie.

### 10.22. Configurer un domaine personnalisé pour un projet

- **ID**: OQR-022
- **Description**: En tant qu'administrateur de projet, je veux configurer un domaine personnalisé (ex. : `qr.monprojet.io`) pour les URLs courtes de redirection de mon projet afin que les QR codes diffusés portent l'identité de marque de mon produit.
- **Acceptance criteria**:
  - Les paramètres du projet dans l'UI admin incluent une section "Domaine personnalisé".
  - L'administrateur saisit un sous-domaine (ex. : `qr.monprojet.io`) et l'UI affiche les instructions CNAME à configurer chez son fournisseur DNS.
  - Le système vérifie automatiquement la présence du CNAME via polling DNS asynchrone et met à jour le statut (en attente / actif / erreur).
  - Une fois actif, tous les nouveaux QR codes dynamiques du projet utilisent le domaine personnalisé dans leur URL courte.
  - Les QR codes dynamiques existants créés avant l'activation continuent de fonctionner sur l'ancien domaine.
  - Si le CNAME est supprimé, le statut passe en erreur et une notification `ISnackbar` alerte l'administrateur ; le trafic bascule sur le domaine par défaut de la plateforme.

### 10.23. Créer des QR codes en masse via import CSV

- **ID**: OQR-023
- **Description**: En tant qu'administrateur de projet, je veux importer un fichier CSV pour créer plusieurs QR codes en une seule opération afin de provisionner en masse des QR codes pour une saison de contenu ou une série d'émissions.
- **Acceptance criteria**:
  - L'UI admin fournit un bouton "Importer CSV" sur la vue liste QR.
  - Le fichier CSV supporte les colonnes : `url`, `type` (static/dynamic/personalized), `format`, `templateId`, `category`, `tags`, `slug` (optionnel).
  - Un modèle CSV vide est téléchargeable depuis l'UI admin.
  - L'import est traité de façon asynchrone ; l'UI affiche une barre de progression jusqu'à la complétion.
  - Un rapport d'import est affiché à la fin : nombre de QR créés, lignes ignorées (doublons), lignes en erreur avec le motif.
  - La limite par import est de 500 QR codes ; une tentative dépassant cette limite retourne HTTP 422 avec un message explicatif.
  - L'endpoint API `POST /api/v1/qr/batch` accepte le même payload en JSON pour les imports programmatiques.

### 10.24. Scanner et rediriger via le service de redirection

- **ID**: OQR-024
- **Description**: En tant qu'utilisateur final, je veux scanner un QR code dynamique affiché à l'écran et être redirigé vers la bonne destination afin d'accéder immédiatement au contenu cible sans délai perceptible.
- **Acceptance criteria**:
  - Lorsqu'un utilisateur scanne un QR dynamique, son appareil accède à l'URL courte (ex. : `https://qr.monprojet.io/abc123`).
  - Le service de redirection résout le `shortCode` depuis IMemoryCache (< 1 ms) et émet une réponse `302 Found` avec le header `Location` pointant vers la destination configurée.
  - L'événement de scan est enregistré de façon asynchrone (type d'appareil, pays/région, timestamp) sans bloquer la redirection.
  - La latence totale de la redirection (scan → premier byte de la destination) est inférieure à 10 ms au p95.
  - Si le `shortCode` n'existe pas, le service retourne HTTP 404 avec une page d'erreur branded du projet.
  - Si le QR est expiré (type personnalisé), le service retourne HTTP 410 Gone.

### 10.25. Consulter le score de scannabilité d'un QR code

- **ID**: OQR-025
- **Description**: En tant qu'administrateur de projet, je veux voir le score de scannabilité d'un QR code généré afin de vérifier qu'il sera correctement lisible dans les conditions de diffusion (broadcast TV, impression, affichage).
- **Acceptance criteria**:
  - Chaque QR code généré retourne un score de scannabilité (0–100) dans la réponse API et dans la vue détail de l'UI admin.
  - Un score ≥ 70 est affiché en vert, 50–69 en orange, < 50 en rouge avec une recommandation d'amélioration (ex. : augmenter le contraste, réduire la taille du logo).
  - L'éditeur de gabarit recalcule et affiche le score en temps réel à chaque changement de propriété visuelle.
  - Un badge d'avertissement visible est affiché dans la liste QR pour les codes avec un score < 70.

### 10.26. Détecter des anomalies de scan

- **ID**: OQR-026
- **Description**: En tant qu'administrateur de projet, je veux être notifié lorsque des patterns de scans anormaux sont détectés sur un QR code afin de pouvoir investiguer d'éventuels abus ou erreurs de configuration.
- **Acceptance criteria**:
  - Le système détecte les patterns anormaux : pic de volume dépassant 5× la moyenne des 7 derniers jours, ou plus de 100 scans depuis la même localisation en moins de 60 secondes.
  - Une alerte est créée dans la base de données et affichée comme notification dans le tableau de bord admin du projet concerné.
  - L'alerte inclut : le QR code concerné, le type d'anomalie, le volume observé, la période de détection et les métriques associées.
  - Les alertes peuvent être marquées comme "ignorée" ou "résolue" par un project-admin.
  - Aucune action automatique n'est prise (pas de blocage automatique) en V1 ; la détection est purement informative.
