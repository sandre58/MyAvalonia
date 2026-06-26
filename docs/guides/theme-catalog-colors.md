# Theme catalog — colors & brushes

**Reference** for semantic colors, brand palettes, roles, and brush keys. For how the engine applies them, see [Theming (engine)](theming.md).

**Source files:** [`Themes/Dark.axaml`](../../src/MyNet.Avalonia.Theme/Themes/Dark.axaml) · [`Themes/Light.axaml`](../../src/MyNet.Avalonia.Theme/Themes/Light.axaml) · runtime injection in `ThemePaletteInjector`

---

## Key convention

| Layer | Pattern | Example |
|-------|---------|---------|
| Static color (variant dictionary) | `MyNet.Color.{Path}` | `MyNet.Color.Surface.Level1` |
| Runtime brush (prefer in UI) | `MyNet.Brush.{Path}` | `MyNet.Brush.Surface.Level1` |
| Brand shades | `MyNet.Brush.Primary.Shade500` | Generated from `ColorShades` |

Markup shorthand: `{my:Theme Surface.Level1}` → `MyNet.Brush.Surface.Level1`.

---

## Theme variants

Each variant is a `ResourceDictionary` merged under `MyTheme` `ThemeDictionaries`:

| Variant key | File | Base |
|-------------|------|------|
| `Dark` | `Themes/Dark.axaml` | `ThemeVariant.Dark` |
| `Light` | `Themes/Light.axaml` | `ThemeVariant.Light` |
| `DarkBlue` | `Themes/DarkBlue.axaml` | `ThemeVariantProvider.DarkBlue` |
| `HighContrast` | `Themes/HighContrast.axaml` | `ThemeVariantProvider.HighContrast` |

Values differ per variant; **key names are stable** across Dark/Light.

---

## Semantic colors (per variant)

### Surfaces

| Key | Typical use |
|-----|-------------|
| `Surface.Application` | Window / app background |
| `Surface.Level0` | Chrome, title bar |
| `Surface.Level1` | Cards, containers |
| `Surface.Level2` | Control fill |
| `Surface.Popup` | Menus, dropdowns, flyouts |
| `Surface.Overlay` | Modal scrim content (busy, dialog panel) |
| `Surface.Inverse` | Tooltips, inverse panels |
| `Surface.Border` | Surface outline |

### Foregrounds

| Key | Typical use |
|-----|-------------|
| `Foreground.Primary` | Body text |
| `Foreground.Secondary` | Secondary labels |
| `Foreground.Tertiary` | Hints, disabled text |
| `Foreground.Inverse` | Text on inverse surfaces |

### Controls & chrome

| Key | Typical use |
|-----|-------------|
| `Control.Border` | Input border default |
| `Control.Border.Hover` | Input border hover |
| `Control.Border.Focus` | Focus ring color |
| `Divider` | Separators |
| `Overlay.Background` | Modal dimming layer |
| `Validation.Error` | Inline validation |
| `Button.Close.Hover` | Window close hover |

### Semantic roles (fixed hues per variant)

| Key | Role |
|-----|------|
| `Success` | Positive feedback |
| `Error` | Errors, destructive |
| `Warning` | Caution |
| `Information` | Info messages |
| `Neutral` | Neutral badges |

At runtime, role colors also exist as **`ColorShades`** on `ThemeVariantPalette` (with generated tones).

### Domain-specific

| Group | Keys |
|-------|------|
| **CodeBlock** | `Unknown`, `Comment`, `Keyword`, `String`, … (syntax highlighting) |
| **Gender** | `Female`, `Male` |

---

## Brand colors (`Primary` / `Accent`)

Set on `<my:MyTheme />` or at runtime:

```xml
<my:MyTheme Primary="#1756BD" Accent="#FFAE18" />
```

`ColorShades` generates Material-style tones:

| Shade | Role |
|-------|------|
| 50–400 | Lighter backgrounds |
| 500 (`Base`) | Main brand color |
| 600–900 | Darker emphasis |
| `Foreground` | Auto contrasting text |

Runtime brush keys include `Primary`, `Primary.Shade600`, `Primary.Foreground`, `Accent`, etc.

```csharp
MyTheme.Current.Primary = new ColorShades(Color.Parse("#1756BD"));
MyTheme.Current.Accent = new ColorShades(Colors.Orange);
```

---

## Interaction opacity levels

Defined in variant dictionaries (e.g. `Dark.axaml`):

| Key | Default (Dark) | Use |
|-----|----------------|-----|
| `Opacity.High` | 0.70 | Strong overlay |
| `Opacity.Medium` | 0.56 | Helper text |
| `Opacity.Low` | 0.45 | Watermarks |
| `Opacity.Scrim` | 0.60 | Modal scrim |
| `Opacity.Disabled` | 0.38 | Disabled state |
| `Opacity.Hover` | 0.10 | Hover wash |
| `Opacity.Pressed` | 0.24 | Pressed state |
| `Opacity.Focus` | 0.16 | Focus wash |
| `Opacity.Overlay` | 0.08 | Light overlay |

Use in markup: `{my:Theme Primary, Opacity=Hover}` or `{my:Theme Surface.Level1, Opacity=Medium}`.

---

## `ThemeAssist` — roles & context

### `ThemeRole` (`my:ThemeAssist.Role`)

Maps control to a **semantic palette** for `{my:ThemeRole}` and `variant-*` classes:

| Value | Palette source |
|-------|----------------|
| `Default` | Theme surfaces |
| `Primary` / `Accent` | Brand `ColorShades` |
| `Success` / `Warning` / `Error` / `Information` / `Neutral` | Semantic colors |
| `Inverse` | Inverse surface/foreground |
| `Contrast` | High-contrast accent |

```xml
<Button Classes="variant-solid" my:ThemeAssist.Role="Primary" Content="Save" />
<TextBlock my:ThemeAssist.Role="Error" Text="{Binding Error}" />
```

Add `Classes="has-role"` when binding `{my:ThemeRole Foreground}` or `{my:ThemeRole Background}`.

### `ThemeContext` (`my:ThemeAssist.Context`)

| Value | Effect |
|-------|--------|
| `Default` | Standard surface resolution |
| `Contrast` | Contrast surface palette (headers, inverse strips) |

Propagates to flyouts, context menus, and attached flyouts. Showcase: `PageHeader` style uses `Context="Contrast"`.

---

## Markup — brush bindings

Namespace: `xmlns:my="http://mynet.com/avalonia"`

```xml
<!-- Direct path → MyNet.Brush.* -->
<Border Background="{my:Theme Surface.Popup}" />
<Button Background="{my:Theme Primary}" Foreground="{my:Theme Primary.Foreground}" />

<!-- Role-relative -->
<Border Classes="has-role" Background="{my:ThemeRole Background}" my:ThemeAssist.Role="Primary" />

<!-- Context-relative path -->
<Border Background="{my:ThemeContext Surface.Popup}" />

<!-- Transforms -->
<Border Background="{my:Theme Primary, Opacity=Medium, Contrast=True, Darken=0.05}" />
```

| Parameter | Role |
|-----------|------|
| `Opacity` | Named level (`Hover`, `Medium`, …) |
| `CustomOpacity` | `"0.5"` or resource key |
| `Contrast` | Companion contrast color |
| `Darken` / `Lighten` | 0.0–1.0 |

---

## Code access

```csharp
// Mutable brushes
var brush = MyTheme.Current.GetBrush("Surface.Level1");
var faded = MyTheme.Current.GetBrush("Primary", opacityKey: "Medium", contrast: true);

// Full palette snapshot (Showcase Theme page)
var palette = MyTheme.Current.GetThemePalette();
```

---

## See also

- [Theming (engine)](theming.md) — lifecycle, `EnsureLoaded`, MVVM
- [Theme catalog — tokens](theme-catalog-tokens.md) — spacing, corners, motion
- [Theme catalog — utility classes](theme-catalog-utility-classes.md) — `variant-solid`, `has-role`
- [Showcase Theme page](../../demos/MyNet.Avalonia.Showcase/Pages/ThemePage.axaml)
