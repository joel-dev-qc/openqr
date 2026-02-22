# Vue d'ensemble — OpenQR

## Description générale

**OpenQR** est une application open source de génération, gestion et suivi de QR codes. Elle offre une API REST complète permettant à des développeurs et des entreprises d'intégrer facilement la création et l'analyse de QR codes dans leurs propres produits.

## Problème résolu

La génération de QR codes est une opération courante dans de nombreux contextes (marketing, logistique, événementiel, etc.). La plupart des solutions existantes sont soit propriétaires, soit limitées en fonctionnalités. OpenQR répond à ce besoin en proposant :

- La **génération** de QR codes pour différents types de contenu (URL, texte libre, vCard, WiFi, etc.)
- La **gestion** complète des QR codes (création, lecture, mise à jour, suppression)
- Le **tracking** des scans pour obtenir des statistiques d'utilisation
- Le support des QR codes **dynamiques** (dont le contenu peut être modifié sans regénérer le code)

## Public cible

- **Développeurs** souhaitant intégrer la génération de QR codes dans leurs applications via une API
- **Entreprises** ayant besoin d'une solution auto-hébergée pour gérer leur parc de QR codes
- **Équipes marketing** désirant suivre les performances de leurs campagnes via QR codes

## Objectifs principaux

1. Fournir une **API REST simple et documentée** pour la gestion des QR codes
2. Permettre le **tracking des scans** avec des données analytiques détaillées
3. Supporter les **QR codes dynamiques** pour faciliter la gestion post-déploiement
4. Offrir des options de **personnalisation visuelle** (couleurs, logo, forme des modules)
5. Gérer l'**authentification et les utilisateurs** avec différents niveaux d'accès

## Stack technologique

| Composant | Technologie | Justification |
|---|---|---|
| Base de données | **MongoDB** | Flexibilité du schéma, adapté aux documents JSON, scalabilité |
| API | **À définir** (Node.js/Express ou autre) | TODO |
| Authentification | **À définir** (JWT, OAuth2, etc.) | TODO |
| Génération QR | **À définir** (librairie QR) | TODO |
| Hébergement | **À définir** | TODO |

## Contraintes et hypothèses

- La solution doit pouvoir s'auto-héberger facilement (idéalement via Docker)
- Les données analytiques de scan doivent être stockées de façon à respecter la vie privée des utilisateurs
- L'API doit être versionnée pour assurer la compatibilité ascendante

## Statut du projet

> **Phase actuelle** : Spécification et conception initiale
