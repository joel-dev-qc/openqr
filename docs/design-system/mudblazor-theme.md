# OpenQR — MudBlazor Theme (Material Design 3)

Implémentation C# du thème MudBlazor mappant les color roles MD3 définis dans [tokens.md](./tokens.md).

---

## 1. Enregistrement du thème

Dans `OpenQR.Web/Program.cs` :

```csharp
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 300;
    config.SnackbarConfiguration.ShowTransitionDuration = 300;
});
```

Dans `MainLayout.razor` :

```razor
@inject IThemeService ThemeService

<MudThemeProvider Theme="OpenQrTheme.Default" IsDarkMode="@_isDarkMode" />
<MudPopoverProvider />
<MudDialogProvider FullWidth="true" MaxWidth="MaxWidth.Small" />
<MudSnackbarProvider />
```

---

## 2. Définition du MudTheme

```csharp
// OpenQR.Web/Themes/OpenQrTheme.cs
namespace OpenQR.Web.Themes;

public static class OpenQrTheme
{
    public static readonly MudTheme Default = new()
    {
        PaletteLight = new PaletteLight
        {
            // MD3 Primary
            Primary = "#1B5ECC",
            PrimaryContrastText = "#FFFFFF",
            PrimaryDarken = "#004599",    // primary-container (dark)
            PrimaryLighten = "#D6E4FF",   // primary-container (light)

            // MD3 Secondary
            Secondary = "#545F71",
            SecondaryContrastText = "#FFFFFF",
            SecondaryDarken = "#3D4758",
            SecondaryLighten = "#D8E3F8",

            // MD3 Tertiary (mapped to Tertiary)
            Tertiary = "#6B538C",
            TertiaryContrastText = "#FFFFFF",

            // MD3 Error
            Error = "#BA1A1A",
            ErrorContrastText = "#FFFFFF",

            // MD3 Surface / Background
            Background = "#F8F9FF",
            BackgroundGray = "#E0E2EC",   // surface-variant
            Surface = "#F8F9FF",
            DrawerBackground = "#E8EDFA",
            DrawerText = "#191C20",
            DrawerIcon = "#43474E",

            // MD3 On-Surface / Text
            TextPrimary = "#191C20",      // on-surface
            TextSecondary = "#43474E",    // on-surface-variant
            TextDisabled = "rgba(25, 28, 32, 0.38)",

            // MD3 Outline
            Divider = "#C3C6CF",          // outline-variant
            DividerLight = "#E0E2EC",     // surface-variant

            // Lines / Input borders
            LinesDefault = "#73777F",     // outline
            LinesInputs = "#73777F",

            // Action states (MD3 state layers)
            ActionDefault = "rgba(25, 28, 32, 0.87)",
            ActionDisabled = "rgba(25, 28, 32, 0.26)",
            ActionDisabledBackground = "rgba(25, 28, 32, 0.12)",

            // AppBar
            AppbarBackground = "#1B5ECC",
            AppbarText = "#FFFFFF",

            // Overlay
            OverlayDark = "rgba(0, 0, 0, 0.5)",
            OverlayLight = "rgba(255, 255, 255, 0.7)",

            // Dark mode toggle chip
            Dark = "#111318",
            DarkContrastText = "#E2E2E9",
            DarkDarken = "#0A0D11",
            DarkLighten = "#1A1D23",
        },

        PaletteDark = new PaletteDark
        {
            // MD3 Primary (dark)
            Primary = "#AAC7FF",
            PrimaryContrastText = "#002F6C",
            PrimaryDarken = "#D6E4FF",    // on-primary-container
            PrimaryLighten = "#004599",   // primary-container

            // MD3 Secondary (dark)
            Secondary = "#BCC7DC",
            SecondaryContrastText = "#263141",
            SecondaryDarken = "#D8E3F8",
            SecondaryLighten = "#3D4758",

            // MD3 Tertiary (dark)
            Tertiary = "#D4BBFF",
            TertiaryContrastText = "#3B1F5B",

            // MD3 Error (dark)
            Error = "#FFB4AB",
            ErrorContrastText = "#690005",

            // MD3 Surface / Background (dark)
            Background = "#111318",
            BackgroundGray = "#43474E",   // surface-variant
            Surface = "#111318",
            DrawerBackground = "#1A1D23",
            DrawerText = "#E2E2E9",
            DrawerIcon = "#C3C6CF",

            // MD3 On-Surface / Text (dark)
            TextPrimary = "#E2E2E9",
            TextSecondary = "#C3C6CF",
            TextDisabled = "rgba(226, 226, 233, 0.38)",

            // MD3 Outline (dark)
            Divider = "#43474E",          // outline-variant
            DividerLight = "#43474E",

            LinesDefault = "#8D9199",     // outline
            LinesInputs = "#8D9199",

            ActionDefault = "rgba(226, 226, 233, 0.87)",
            ActionDisabled = "rgba(226, 226, 233, 0.26)",
            ActionDisabledBackground = "rgba(226, 226, 233, 0.12)",

            AppbarBackground = "#1A1D23",
            AppbarText = "#E2E2E9",

            OverlayDark = "rgba(0, 0, 0, 0.7)",
            OverlayLight = "rgba(255, 255, 255, 0.1)",

            Dark = "#E2E2E9",
            DarkContrastText = "#111318",
        },

        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Inter", "system-ui", "sans-serif"],
                FontSize = "14px",
                FontWeight = "400",
                LineHeight = "1.4",
                LetterSpacing = "normal",
            },
            H1 = new H1Typography   // Display Large
            {
                FontFamily = ["Inter", "system-ui", "sans-serif"],
                FontSize = "57px",
                FontWeight = "400",
                LineHeight = "1.12",
                LetterSpacing = "-0.25px",
            },
            H2 = new H2Typography   // Display Medium
            {
                FontSize = "45px",
                FontWeight = "400",
                LineHeight = "1.15",
            },
            H3 = new H3Typography   // Display Small
            {
                FontSize = "36px",
                FontWeight = "400",
                LineHeight = "1.22",
            },
            H4 = new H4Typography   // Headline Large
            {
                FontSize = "32px",
                FontWeight = "400",
                LineHeight = "1.25",
            },
            H5 = new H5Typography   // Headline Medium
            {
                FontSize = "28px",
                FontWeight = "400",
                LineHeight = "1.28",
            },
            H6 = new H6Typography   // Headline Small
            {
                FontSize = "24px",
                FontWeight = "400",
                LineHeight = "1.33",
            },
            Subtitle1 = new Subtitle1Typography     // Title Large
            {
                FontSize = "22px",
                FontWeight = "700",
                LineHeight = "1.27",
            },
            Subtitle2 = new Subtitle2Typography     // Title Medium
            {
                FontSize = "16px",
                FontWeight = "500",
                LineHeight = "1.5",
                LetterSpacing = "0.15px",
            },
            Body1 = new Body1Typography     // Body Large
            {
                FontSize = "16px",
                FontWeight = "400",
                LineHeight = "1.5",
                LetterSpacing = "0.5px",
            },
            Body2 = new Body2Typography     // Body Medium
            {
                FontSize = "14px",
                FontWeight = "400",
                LineHeight = "1.43",
                LetterSpacing = "0.25px",
            },
            Button = new ButtonTypography   // Label Large
            {
                FontSize = "14px",
                FontWeight = "500",
                LineHeight = "1.43",
                LetterSpacing = "0.1px",
                TextTransform = "none",     // MD3 n'utilise PAS de majuscules forcées
            },
            Caption = new CaptionTypography     // Label Small / Body Small
            {
                FontSize = "12px",
                FontWeight = "400",
                LineHeight = "1.33",
                LetterSpacing = "0.4px",
            },
            Overline = new OverlineTypography   // Label Medium
            {
                FontSize = "12px",
                FontWeight = "500",
                LineHeight = "1.33",
                LetterSpacing = "0.5px",
                TextTransform = "none",
            },
        },

        LayoutProperties = new LayoutProperties
        {
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "260px",
            DefaultBorderRadius = "8px",    // MD3 Small shape
            AppbarHeight = "64px",
        },

        Shadows = new Shadow(),  // Conserver les ombres par défaut MudBlazor

        ZIndex = new ZIndex(),   // Conserver les z-index par défaut
    };
}
```

---

## 3. Variables CSS custom properties

À ajouter dans `OpenQR.Web/wwwroot/css/app.css` pour accéder aux tokens MD3 directement en CSS :

```css
/* === OpenQR Design Tokens — Material Design 3 === */
:root {
  /* Primary */
  --color-primary: #1B5ECC;
  --color-on-primary: #FFFFFF;
  --color-primary-container: #D6E4FF;
  --color-on-primary-container: #001947;

  /* Secondary */
  --color-secondary: #545F71;
  --color-on-secondary: #FFFFFF;
  --color-secondary-container: #D8E3F8;
  --color-on-secondary-container: #111C2B;

  /* Tertiary */
  --color-tertiary: #6B538C;
  --color-on-tertiary: #FFFFFF;
  --color-tertiary-container: #EEDCFF;
  --color-on-tertiary-container: #250047;

  /* Error */
  --color-error: #BA1A1A;
  --color-on-error: #FFFFFF;
  --color-error-container: #FFDAD6;
  --color-on-error-container: #410002;

  /* Surface */
  --color-surface: #F8F9FF;
  --color-on-surface: #191C20;
  --color-surface-variant: #E0E2EC;
  --color-on-surface-variant: #43474E;
  --color-background: #F8F9FF;
  --color-on-background: #191C20;

  /* Outline */
  --color-outline: #73777F;
  --color-outline-variant: #C3C6CF;

  /* Sémantiques OpenQR */
  --color-score-good: #1B6F3A;
  --color-score-warn: #985A00;
  --color-score-bad: #BA1A1A;
  --color-status-active: #1B6F3A;
  --color-status-pending: #985A00;
  --color-status-error: #BA1A1A;

  /* Shape */
  --shape-extra-small: 4px;
  --shape-small: 8px;
  --shape-medium: 12px;
  --shape-large: 16px;
  --shape-extra-large: 28px;

  /* Spacing */
  --spacing-1: 4px;
  --spacing-2: 8px;
  --spacing-3: 12px;
  --spacing-4: 16px;
  --spacing-6: 24px;
  --spacing-8: 32px;
  --spacing-12: 48px;
}

[data-mud-theme="dark"] {
  --color-primary: #AAC7FF;
  --color-on-primary: #002F6C;
  --color-primary-container: #004599;
  --color-on-primary-container: #D6E4FF;

  --color-secondary: #BCC7DC;
  --color-on-secondary: #263141;
  --color-secondary-container: #3D4758;
  --color-on-secondary-container: #D8E3F8;

  --color-tertiary: #D4BBFF;
  --color-on-tertiary: #3B1F5B;
  --color-tertiary-container: #523B73;
  --color-on-tertiary-container: #EEDCFF;

  --color-error: #FFB4AB;
  --color-on-error: #690005;
  --color-error-container: #93000A;
  --color-on-error-container: #FFDAD6;

  --color-surface: #111318;
  --color-on-surface: #E2E2E9;
  --color-surface-variant: #43474E;
  --color-on-surface-variant: #C3C6CF;
  --color-background: #111318;
  --color-on-background: #E2E2E9;

  --color-outline: #8D9199;
  --color-outline-variant: #43474E;

  --color-score-good: #4CAF79;
  --color-score-warn: #FFB74D;
  --color-score-bad: #FFB4AB;
  --color-status-active: #4CAF79;
  --color-status-pending: #FFB74D;
  --color-status-error: #FFB4AB;
}
```

---

## 4. Import de la police Inter

Dans `OpenQR.Web/wwwroot/index.html` (ou `App.razor`) :

```html
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;700&display=swap" rel="stylesheet">
```

---

## 5. Règles d'utilisation

### À faire

- Utiliser `var(--color-primary)` en CSS pour les couleurs directes, jamais de valeur hex hardcodée.
- Utiliser les composants MudBlazor avec `Color="Color.Primary"` / `Color="Color.Secondary"` etc.
- Appliquer `Variant=Variant.Filled` pour les boutons d'action principale (CTA).
- Appliquer `Variant=Variant.Outlined` pour les boutons secondaires.
- Appliquer `Variant=Variant.Text` pour les actions tertiaires / destructives confirmées.

### À ne pas faire

- Ne pas hardcoder des couleurs hex dans les composants Razor — utiliser les color roles.
- Ne pas utiliser `TextTransform="uppercase"` sur les boutons — MD3 n'utilise plus les majuscules forcées.
- Ne pas utiliser `Elevation > 3` sauf cas exceptionnel justifié dans le code (commentaire requis).
- Ne pas créer de couleurs custom hors des tokens définis dans [tokens.md](./tokens.md) sans mise à jour du document.
