---
applyTo: "OpenQR.Web/**/*.{cs,razor}"
---

# Blazor / MudBlazor Instructions — OpenQR.Web

## Architecture
```
OpenQR.Web/
├── Core/           # Interfaces, modèles, DTOs client
├── Infrastructure/ # HttpClients, AuthService, JWT
├── Components/     # Composants MudBlazor réutilisables
└── Pages/          # Pages Blazor
```

## Composants Razor
- Toujours déclarer `@rendermode InteractiveAuto`
- Ordre : @page, @rendermode, @inject, @code
- Champs privés en `_camelCase`
- Gérer loading state avec `_isLoading` + `MudProgressLinear`
- Gérer les erreurs avec `ISnackbar`

## MudBlazor — Composants préférés
- Tableaux : `MudDataGrid`
- Formulaires : `MudForm` + `MudTextField`
- Notifications : `ISnackbar`
- Confirmations : `IDialogService`
- Loading : `MudProgressLinear` / `MudSkeleton`

## ConfigureAwait dans Blazor
Ne PAS utiliser `ConfigureAwait(false)` dans les composants Blazor.
Le contexte de synchronisation Blazor en a besoin pour les updates UI.

## Auth
Utiliser `AuthorizeView` pour les sections conditionnelles par rôle.
`[Authorize(Roles = "admin")]` sur les pages admin.