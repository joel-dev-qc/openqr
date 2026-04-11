# OpenQR Design System

## Vue d'ensemble

Le design system d'OpenQR est fondé sur le standard **Material Design 3** (MD3 — [m3.material.io](https://m3.material.io/)) implémenté via **MudBlazor**.

Ce document est la source de vérité pour toute décision visuelle de l'interface admin Blazor. Les développeurs l'implémentent, les designers s'y réfèrent.

---

## Documents

| Document | Contenu |
|---|---|
| [tokens.md](./tokens.md) | Color roles, typographie, shape, spacing, elevation |
| [mudblazor-theme.md](./mudblazor-theme.md) | Implémentation C# du `MudTheme` avec `PaletteLight` / `PaletteDark` |

---

## Principes fondamentaux

### Material Design 3

MD3 introduit le concept de **color roles** plutôt que de simples couleurs brutes. Chaque rôle a une fonction sémantique précise :

- `primary` — Composants interactifs principaux (boutons, FAB, liens actifs)
- `on-primary` — Texte/icônes sur fond `primary`
- `primary-container` — Conteneurs moins saillants (chips selected, nav rail active)
- `on-primary-container` — Texte/icônes sur `primary-container`
- `secondary` — Composants secondaires, informations complémentaires
- `tertiary` — Accents contrastants pour équilibrer la composition
- `error` — États d'erreur
- `surface` — Fond des cards, sheets, menus
- `on-surface` — Texte principal sur surface
- `surface-variant` — Surfaces alternatives légèrement teintées
- `outline` — Bordures et séparateurs

### Thèmes clair et sombre

Les deux thèmes sont supportés et définis dans le `MudTheme`. MudBlazor expose `PaletteLight` et `PaletteDark` — les tokens sont mappés dans [mudblazor-theme.md](./mudblazor-theme.md).

### Composants MudBlazor privilégiés

| Besoin | Composant |
|---|---|
| Listes / tableaux | `MudDataGrid` avec pagination serveur |
| Notifications | `ISnackbar` |
| Confirmations destructives | `IDialogService` |
| Chargement | `MudProgressLinear` / `MudSkeleton` |
| Navigation | `MudNavMenu` + `MudNavLink` |
| Filtres / chips | `MudChip` / `MudChipSet` |
| Autorisation | `AuthorizeView` |

---

## Déviations de MudBlazor par rapport à MD3

| Écart connu | Workaround |
|---|---|
| MudBlazor n'expose pas nativement `primary-container` / `on-primary-container` | Utiliser `PaletteLight.PrimaryDarken` / `PaletteLight.PrimaryLighten` avec override CSS ciblé |
| `MudButton` Variant=Filled utilise `primary` correctement, mais `Variant=Outlined` ignore `outline` role | Appliquer `Class="border-outline"` avec variable CSS `--mud-palette-lines-default` |
| Elevation MD3 (tonal color overlay) non supportée nativement | Simuler via `MudPaper Elevation=1` + couleur de fond `surface-variant` |

---

## Structure dans le projet

```
OpenQR.Web/
├── wwwroot/
│   └── css/
│       └── app.css         # Variables CSS custom properties (color tokens)
└── Components/
    └── Layout/
        └── MainLayout.razor  # MudThemeProvider avec le thème OpenQR
```
