# Theming Guide

## Application startup order

```xml
<Application.Styles>
  <my:MyTheme />
  <my:ThemeControlsCatalog />
  <StyleInclude Source="avares://MyNet.Avalonia.Extended/Themes/Generic.axaml" />
  <!-- app-specific styles last -->
</Application.Styles>
```

In code: `MyTheme.Current.EnsureLoaded()` during app init.

## MyTheme

- Root: `MyNet.Avalonia.Theme/MyTheme.axaml`
- Variants: Dark, Light, DarkBlue, HighContrast
- Dynamic Primary/Accent with animated transitions
- `ThemeVersion` property for binding invalidation after palette change

## Design tokens (`Tokens/`)

Merged via `Tokens/_index.axaml`:

- `MyNet.Color.*`, `MyNet.Font.*`, `MyNet.Spacing.*`, `MyNet.Corners.*`

## ControlTheme structure

| Layer | Path | Content |
|-------|------|---------|
| Foundation | `Foundation/` | RegionLayout, InputChrome, Ripple |
| Standard | `Standard/` | Restyled Avalonia built-ins (Button, TextBox, …) |
| Custom | `Custom/` | MyNet controls |
| Presets | `Presets/` | ListBox presets, etc. |

## Key naming

- Implicit theme: `{x:Type my:Card}`
- Variant: `MyNet.Theme.Card.Interactive`
- Template: `MyNet.Card.Template.Body.Horizontal`
- CSS classes: `variant-light`, `size-md` via `CssClass` registries

## Markup extensions

`{my:ThemeContext Surface.Level1}`, `{my:ThemeRole Background}`, `{my:Foreground Opacity=High}`

## Assists

- Theme: `VariantAssist`, `ShadowAssist`, `ValidationAssist`
- Controls: `IconAssist`, `DataGridAssist`, `DrawerAssist`

Human guide: `docs/guides/theming.md`
