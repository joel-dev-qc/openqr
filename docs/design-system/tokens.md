# OpenQR — Design Tokens (Material Design 3)

Référence des tokens de design appliqués dans l'UI admin OpenQR, selon le standard MD3.

---

## 1. Palette de couleurs — Color Roles MD3

### Couleur source (seed color)

La palette MD3 est dérivée d'une couleur source unique via l'algorithme HCT (Hue, Chroma, Tone).

| Token | Valeur | Notes |
|---|---|---|
| `seed-color` | `#1B6EF3` | Bleu tech, évoque confiance et précision |

> Les valeurs ci-dessous sont générées via [Material Theme Builder](https://m3.material.io/theme-builder) à partir de la seed color `#1B6EF3`.

---

### Thème clair (`PaletteLight`)

| Role MD3 | Token OpenQR | Hex | Usage |
|---|---|---|---|
| `primary` | `--color-primary` | `#1B5ECC` | Boutons principaux, FAB, liens actifs |
| `on-primary` | `--color-on-primary` | `#FFFFFF` | Texte/icônes sur fond primary |
| `primary-container` | `--color-primary-container` | `#D6E4FF` | Chips selected, nav active bg |
| `on-primary-container` | `--color-on-primary-container` | `#001947` | Texte sur primary-container |
| `secondary` | `--color-secondary` | `#545F71` | Composants secondaires |
| `on-secondary` | `--color-on-secondary` | `#FFFFFF` | Texte sur secondary |
| `secondary-container` | `--color-secondary-container` | `#D8E3F8` | Badges, tags secondaires |
| `on-secondary-container` | `--color-on-secondary-container` | `#111C2B` | Texte sur secondary-container |
| `tertiary` | `--color-tertiary` | `#6B538C` | Accents QR score visuel |
| `on-tertiary` | `--color-on-tertiary` | `#FFFFFF` | Texte sur tertiary |
| `tertiary-container` | `--color-tertiary-container` | `#EEDCFF` | Bg indicateurs score |
| `on-tertiary-container` | `--color-on-tertiary-container` | `#250047` | Texte sur tertiary-container |
| `error` | `--color-error` | `#BA1A1A` | Erreurs, validations, alertes |
| `on-error` | `--color-on-error` | `#FFFFFF` | Texte sur error |
| `error-container` | `--color-error-container` | `#FFDAD6` | Bg messages d'erreur |
| `on-error-container` | `--color-on-error-container` | `#410002` | Texte sur error-container |
| `surface` | `--color-surface` | `#F8F9FF` | Fond cards, menus, sheets |
| `on-surface` | `--color-on-surface` | `#191C20` | Texte principal |
| `surface-variant` | `--color-surface-variant` | `#E0E2EC` | Surfaces alternatives |
| `on-surface-variant` | `--color-on-surface-variant` | `#43474E` | Labels, placeholders |
| `outline` | `--color-outline` | `#73777F` | Bordures, séparateurs |
| `outline-variant` | `--color-outline-variant` | `#C3C6CF` | Séparateurs discrets |
| `background` | `--color-background` | `#F8F9FF` | Fond global de la page |
| `on-background` | `--color-on-background` | `#191C20` | Texte sur fond global |

### Thème sombre (`PaletteDark`)

| Role MD3 | Token OpenQR | Hex |
|---|---|---|
| `primary` | `--color-primary` | `#AAC7FF` |
| `on-primary` | `--color-on-primary` | `#002F6C` |
| `primary-container` | `--color-primary-container` | `#004599` |
| `on-primary-container` | `--color-on-primary-container` | `#D6E4FF` |
| `secondary` | `--color-secondary` | `#BCC7DC` |
| `on-secondary` | `--color-on-secondary` | `#263141` |
| `secondary-container` | `--color-secondary-container` | `#3D4758` |
| `on-secondary-container` | `--color-on-secondary-container` | `#D8E3F8` |
| `tertiary` | `--color-tertiary` | `#D4BBFF` |
| `on-tertiary` | `--color-on-tertiary` | `#3B1F5B` |
| `tertiary-container` | `--color-tertiary-container` | `#523B73` |
| `on-tertiary-container` | `--color-on-tertiary-container` | `#EEDCFF` |
| `error` | `--color-error` | `#FFB4AB` |
| `on-error` | `--color-on-error` | `#690005` |
| `error-container` | `--color-error-container` | `#93000A` |
| `on-error-container` | `--color-on-error-container` | `#FFDAD6` |
| `surface` | `--color-surface` | `#111318` |
| `on-surface` | `--color-on-surface` | `#E2E2E9` |
| `surface-variant` | `--color-surface-variant` | `#43474E` |
| `on-surface-variant` | `--color-on-surface-variant` | `#C3C6CF` |
| `outline` | `--color-outline` | `#8D9199` |
| `outline-variant` | `--color-outline-variant` | `#43474E` |
| `background` | `--color-background` | `#111318` |
| `on-background` | `--color-on-background` | `#E2E2E9` |

---

## 2. Typographie — Type Scale MD3

MD3 définit 5 niveaux avec 3 tailles chacun (Large / Medium / Small).

| Role | Font | Weight | Size | Line Height | Usage |
|---|---|---|---|---|---|
| Display Large | Inter | 400 | 57sp | 64 | Titres héros (non utilisé en admin) |
| Display Medium | Inter | 400 | 45sp | 52 | — |
| Display Small | Inter | 400 | 36sp | 44 | — |
| Headline Large | Inter | 400 | 32sp | 40 | Titre de page principal |
| Headline Medium | Inter | 400 | 28sp | 36 | Titre de section |
| Headline Small | Inter | 400 | 24sp | 32 | Sous-titre |
| Title Large | Inter | 700 | 22sp | 28 | Titre de card, modal |
| Title Medium | Inter | 500 | 16sp | 24 | Titre de champ groupé |
| Title Small | Inter | 500 | 14sp | 20 | Label de section |
| Body Large | Inter | 400 | 16sp | 24 | Corps de texte principal |
| Body Medium | Inter | 400 | 14sp | 20 | Corps de texte secondaire |
| Body Small | Inter | 400 | 12sp | 16 | Captions, meta |
| Label Large | Inter | 500 | 14sp | 20 | Boutons, tabs |
| Label Medium | Inter | 500 | 12sp | 16 | Chips, badges |
| Label Small | Inter | 500 | 11sp | 16 | Overlines |

**Police principale** : `Inter` (Google Fonts) — fallback : `system-ui, sans-serif`.

---

## 3. Shape Scale — Border Radius

MD3 définit une shape scale en 5 niveaux.

| Niveau | Valeur | Composants MudBlazor correspondants |
|---|---|---|
| None | `0px` | Dividers |
| Extra Small | `4px` | `MudTextField`, `MudSelect`, petites chips |
| Small | `8px` | `MudButton`, `MudCheckBox`, `MudRadio` |
| Medium | `12px` | `MudCard`, `MudPaper` (elevation 1) |
| Large | `16px` | `MudDialog`, menus contextuels |
| Extra Large | `28px` | `MudFab`, `MudDrawer` bottom sheet |
| Full | `50%` | `MudChip` pill, avatars |

---

## 4. Spacing

Basé sur une grille de base `4px` :

| Token | Valeur | Usage |
|---|---|---|
| `spacing-1` | `4px` | Gap minimal entre icônes et texte |
| `spacing-2` | `8px` | Padding interne compact (chips, labels) |
| `spacing-3` | `12px` | Padding boutons |
| `spacing-4` | `16px` | Padding card standard |
| `spacing-5` | `20px` | — |
| `spacing-6` | `24px` | Gap entre sections |
| `spacing-8` | `32px` | Margin sections majeures |
| `spacing-12` | `48px` | Padding page |

---

## 5. Elevation

MD3 utilise des **tonal color overlays** plutôt que des ombres portées pures. L'overlay est appliqué à la couleur `surface` avec l'opacité `primary`.

| Niveau | Overlay opacity | Équivalent MudBlazor |
|---|---|---|
| Level 0 | 0% | `Elevation=0` |
| Level 1 | 5% | `Elevation=1` — cards standard |
| Level 2 | 8% | `Elevation=2` — menus, dropdowns |
| Level 3 | 11% | `Elevation=3` — dialogues |
| Level 4 | 12% | `Elevation=4` — nav drawer |
| Level 5 | 14% | `Elevation=5` — FAB |

> MudBlazor utilise des box-shadows CSS pour l'élévation. Pour simuler les overlays tonals MD3, combiner `MudPaper` avec un fond `surface-variant`.

---

## 6. États d'interaction

MD3 définit des **state layers** superposées aux composants selon l'état.

| État | Opacity |
|---|---|
| Hover | 8% |
| Focused | 12% |
| Pressed | 12% |
| Dragged | 16% |
| Disabled (composant) | 38% opacity sur le contenu |
| Disabled (container) | 12% opacity sur le fond |

MudBlazor gère ces états nativement via ses styles CSS. Aucun override manuel requis pour les états de base.

---

## 7. Couleurs sémantiques additionnelles

Couleurs non issues du palette MD3 mais définies pour les besoins fonctionnels OpenQR :

| Token | Hex (clair) | Hex (sombre) | Usage |
|---|---|---|---|
| `--color-score-good` | `#1B6F3A` | `#4CAF79` | Score scannabilité ≥ 70 |
| `--color-score-warn` | `#985A00` | `#FFB74D` | Score scannabilité 50–69 |
| `--color-score-bad` | `#BA1A1A` | `#FFB4AB` | Score scannabilité < 50 |
| `--color-status-active` | `#1B6F3A` | `#4CAF79` | Domaine personnalisé actif |
| `--color-status-pending` | `#985A00` | `#FFB74D` | DNS en attente |
| `--color-status-error` | `#BA1A1A` | `#FFB4AB` | Erreur domaine / anomalie |
