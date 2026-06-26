# Theme catalog — design tokens

**Reference** for layout, spacing, typography, motion, and shadow tokens merged from `Tokens/_index.axaml`. Variant-independent — same keys in Dark and Light.

For color tokens, see [Theme catalog — colors](theme-catalog-colors.md).

**Source:** [`src/MyNet.Avalonia.Theme/Tokens/`](../../src/MyNet.Avalonia.Theme/Tokens/)

---

## Access patterns

### XAML (static resource)

```xml
<Border CornerRadius="{StaticResource MyNet.Corners.Control}"
        Padding="{StaticResource MyNet.Padding.Surface}" />
```

### Code (`ThemeResources`)

```csharp
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Classes.Enums;

var gap = ThemeResources.Spacing.Get(SpacingSize.Md).Value;
var radius = ThemeResources.Corners.Control.Value;
var duration = ThemeResources.Animation.Opacity.Value;
```

### Utility classes

Many tokens map to CSS classes — see [Theme catalog — utility classes](theme-catalog-utility-classes.md):

```xml
<StackPanel Classes="gap-md p-lg rounded-md" />
```

---

## Spacing scale

From `Tokens/Spacing.axaml`:

| Key | Value (px) | Enum (`SpacingSize`) |
|-----|------------|----------------------|
| `MyNet.Spacing.None` | 0 | `None` |
| `MyNet.Spacing.Xxs` | 1 | `Xxs` |
| `MyNet.Spacing.Xs` | 2 | `Xs` |
| `MyNet.Spacing.Sm` | 5 | `Sm` |
| `MyNet.Spacing.Md` | 10 | `Md` |
| `MyNet.Spacing.Lg` | 16 | `Lg` |
| `MyNet.Spacing.Xl` | 24 | `Xl` |
| `MyNet.Spacing.Xxl` | 32 | `Xxl` |
| `MyNet.Spacing.Xxxl` | 48 | `Xxxl` |

Utility prefixes: `p-*`, `m-*`, `gap-*`, `px-*`, `py-*`, … (see utility-classes guide).

### Component padding presets

| Key | Use |
|-----|-----|
| `MyNet.Padding.Button.Sm/Md/Lg` | Button internal padding |
| `MyNet.Padding.Input.*` | Text field / input chrome |
| `MyNet.Padding.Surface` | Card / panel padding |
| `MyNet.Padding.Surface.Header.*` | Section headers |
| `MyNet.Padding.MenuItem` | Menu items |
| `MyNet.Padding.Popup` / `ToolTip` | Floating UI |
| `MyNet.Margin.Form.Item` / `Form.Group` | Form layout |

---

## Corner radius

From `Tokens/Layout.axaml`:

| Key | Value |
|-----|-------|
| `MyNet.Corners.None` | 0 |
| `MyNet.Corners.Xs` … `Xl` | 2, 4, 8, 12, 16 |
| `MyNet.Corners.Round` | 9999 (pill) |
| `MyNet.Corners.Control` | 3 (inputs, buttons) |
| `MyNet.Corners.Surface` | 4 (cards) |
| `MyNet.Corners.Dialog` | 8 |
| `MyNet.Corners.Popup` / `ToolTip` | 4 |

Utility: `rounded-*`, `border-*` (width tokens).

---

## Sizes (control dimensions)

Grouped under `MyNet.Height.*`, `MyNet.Width.*`, `MyNet.Size.*` in `Layout.axaml`:

| Family | Examples | Utility class |
|--------|----------|---------------|
| Button heights | `Height.Button.Sm/Md/Lg` | `size-sm`, `size-md`, `size-lg` |
| Input heights | `Height.Input.Sm/Md/Lg` | `size-*` on inputs |
| Icon sizes | `Size.Icon.Xs` … `Xxxl` | `icon-*` |
| Avatar | `Size.Avatar.Xs` … `Xl` | themed in Controls catalog |
| Loader / progress | `Size.Loader.*`, `Height.ProgressBar.*` | control themes |
| Glyph | `Size.Glyph.Xs` … `Xxxl` | typography |

---

## Typography

From `Tokens/Typography.axaml` (via `ThemeResources.Font`):

| Token family | Keys |
|--------------|------|
| Font sizes | `MyNet.Font.Size.Xs` … `Xxxl` → utility `font-*` |
| Font weights | `MyNet.Font.Weight.Header`, body weights |
| Line heights | paired with size tokens |

Text utilities: `text-helper`, `text-watermark`, `text-wrap`, `header-helper`, …

---

## Shadows & elevation

From `Tokens/Visual.axaml`:

| Key | Depth |
|-----|-------|
| `MyNet.Shadow.Surface` | Depth1 |
| `MyNet.Shadow.Control` | Depth2 |
| `MyNet.Shadow.Popup` / `ToolTip` | Depth2 |
| `MyNet.Shadow.Dialog` | Depth3 |

Utility: `shadow-surface`, `shadow-control`, `shadow-header`, `shadow-items`.

---

## Motion (durations)

From `Tokens/Visual.axaml`:

| Key | Duration |
|-----|----------|
| `MyNet.Animation.Instant` | 0 |
| `MyNet.Animation.Fast` | 150 ms |
| `MyNet.Animation.Default` | 300 ms |
| `MyNet.Animation.Slow` | 350 ms |
| `MyNet.Animation.Opacity` | 150 ms |
| `MyNet.Animation.Slide` / `Slide.Modal` | 300 / 250 ms |
| `MyNet.Animation.Fade` | 300 ms |

`MyTheme.ColorTransitionDuration` defaults to **150 ms** (`Fast`) for brand color changes.

---

## Geometries

From `Tokens/Geometries.axaml` — shared path data for chevrons:

`MyNet.Geometry.ChevronDown`, `ChevronUp`, `ChevronLeft`, `ChevronRight`, double-chevron variants.

Used internally by control themes (expanders, menus).

---

## Component opacity modifiers

From `Tokens/Visual.axaml` (alpha compositing on semantic colors):

`MyNet.Opacity.Foreground.Secondary` (0.7), `Foreground.Tertiary` (0.5), `Surface.Border` (0.12), `Control.Border` (0.14), etc.

Distinct from **interaction** opacity levels in [color catalog](theme-catalog-colors.md#interaction-opacity-levels).

---

## See also

- [Theme catalog — colors](theme-catalog-colors.md)
- [Theme catalog — utility classes](theme-catalog-utility-classes.md)
- [Theme controls](theme-controls.md) — how tokens apply in control templates
