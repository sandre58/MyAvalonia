# Theme catalog — utility classes

**Reference** for CSS-like classes registered by `ClassesBootstrapper` and applied lazily via `ClassRegistry`.

For **what happens when a class is applied** (lazy registration, `ClassesAssist` layers), see [Theming (engine)](theming.md).

**Source:** [`Classes/Registry/`](../../src/MyNet.Avalonia.Theme/Classes/Registry/) · [`CssClass.cs`](../../src/MyNet.Avalonia.Theme/Classes/CssClass.cs)

---

## Usage

```xml
<Button Classes="variant-solid size-md shadow-control" my:ThemeAssist.Role="Primary" />
<StackPanel Classes="flex-vertical gap-sm p-md" />
<TextBlock Classes="text-helper font-sm" />
```

Classes compose — order does not matter. Invalid class names are ignored silently.

Initialize once: `ClassesBootstrapper.Initialize()` (called by `MyTheme` and `ThemeControlsCatalog` constructors).

---

## Control variants (`variant-*`)

Require **`my:ThemeAssist.Role`** for color resolution (except `Default`).

| Class | `ControlVariant` | Visual |
|-------|------------------|--------|
| `variant-solid` | Solid | Filled background from role palette |
| `variant-light` | Light | Light fill |
| `variant-outlined` | Outlined | Border emphasis |
| `variant-text` | Text | Minimal chrome, text-only |
| `variant-transparent` | Transparent | No background |
| `variant-underline` | — | Underline style |
| `variant-watermark` | — | Watermark styling |
| `variant-header` | — | Header strip |
| `variant-items` | — | Item container (lists) |
| `variant-header-items` | — | Header item container |

Examples (Showcase):

```xml
<Button Content="Primary" Classes="variant-solid" my:ThemeAssist.Role="Primary" />
<Button Content="Text" Classes="variant-text" my:ThemeAssist.Role="Success" />
<ProgressBar Classes="variant-solid" my:ThemeAssist.Role="Error" />
```

---

## Roles & theme binding

| Class | Purpose |
|-------|---------|
| `has-role` | Enables `{my:ThemeRole Background/Foreground/BorderBrush/Primary}` on the control |

```xml
<TextBlock Classes="has-role"
           Foreground="{my:ThemeRole Foreground}"
           my:ThemeAssist.Role="Primary" />
```

---

## Size (`size-*`)

Maps to control dimension tokens (`SpacingSize` / layout heights):

| Class | Typical target |
|-------|------------------|
| `size-xs`, `size-sm`, `size-md`, `size-lg`, `size-xl`, `size-xxl`, `size-xxxl` | Buttons, inputs, chrome |

---

## Spacing — margin & padding

Prefix pattern (Tailwind-like):

| Prefix | Property |
|--------|----------|
| `m-*`, `mt-*`, `mb-*`, `ml-*`, `mr-*` | Margin |
| `mx-*`, `my-*` | Horizontal / vertical margin |
| `p-*`, `pt-*`, … | Padding |
| `px-*`, `py-*` | Horizontal / vertical padding |
| `gap-*`, `gapx-*`, `gapy-*` | Stack/panel spacing |

Suffix: `none`, `xxs`, `xs`, `sm`, `md`, `lg`, `xl`, `xxl`, `xxxl` — values from [spacing tokens](theme-catalog-tokens.md).

---

## Layout (`flex-*`, `align-*`)

| Class | Effect |
|-------|--------|
| `flex-horizontal`, `flex-vertical` | Orientation |
| `flex-wrap`, `flex-uniform` | Wrap / uniform grid |
| `align-center`, `align-left`, … | Horizontal alignment |
| `valign-middle`, `valign-top`, … | Vertical alignment |
| `align-content-*`, `valign-content-*` | Content alignment |
| `position-*` | Position hints |
| `is-stretch` | Stretch alignment |

---

## Visual effects

| Class | Effect |
|-------|--------|
| `shadow-control`, `shadow-surface`, `shadow-header`, `shadow-items` | Elevation ([shadow tokens](theme-catalog-tokens.md)) |
| `opacity-high`, `opacity-medium`, … | Opacity from theme |
| `shape-circle`, `shape-alternate`, `shape-items-circle` | Corner presets |
| `focus-rectangle`, `focus-ellipse`, `focus-hidden` | Focus chrome |
| `hidden`, `visible` | Visibility |

---

## Typography

| Class | Effect |
|-------|--------|
| `font-xs` … `font-xxl` | Font size tokens |
| `text-helper` | Smaller helper text + medium opacity |
| `text-watermark` | Watermark style |
| `text-wrap`, `text-underline`, `text-strikethrough` | Text decoration |
| `header-helper`, `header-watermark` | Header variants |
| `truncate-*` | Text truncation |

---

## Border & shape

| Class | Effect |
|-------|--------|
| `border-0`, `border-1`, … | Border thickness |
| `rounded-none`, `rounded-xs`, … `rounded-round` | Corner radius tokens |

---

## Structural kinds

| Class | Effect |
|-------|--------|
| `kind-card` | Card surface treatment |
| `kind-section` | Section container |
| `kind-focus` | Focus container kind |

Often combined: `kind-card variant-solid shadow-control`.

---

## State

| Class | Effect |
|-------|--------|
| `is-disablable` | Disabled opacity when `IsEnabled=false` |
| `use-transitions` | Enable theme transitions on control |
| `is-centered`, `align-centered` | Center alignment helpers |

---

## Icons (`icon-*`)

Size classes for `ExtendedIcon` / glyph layout — pairs with `MyNet.Size.Icon.*` tokens.

---

## Programmatic classes (`ClassesAssist`)

When code must add/replace classes without clobbering XAML `Classes`:

```csharp
ClassesAssist.AddClasses(control, "layerName", "variant-solid", "size-md");
ClassesAssist.ReplaceClasses(control, "layerName", "size-lg");
ClassesAssist.RemoveClasses(control, "layerName");
```

Layers allow stacked contributions (Showcase Playground uses this for live property editing).

Attached enum properties (e.g. `ThemeAssist.Role`) also sync CSS classes via `AvaloniaPropertyHelper.RegisterEnumProperty`.

---

## Registry map (implementation)

| Registry class | Registers |
|----------------|-----------|
| `VariantClassRegistry` | `variant-*`, `variant-header-*`, `variant-items-*` |
| `SpacingClassRegistry` | `p-*`, `m-*`, `gap-*` |
| `SizeClassRegistry` | `size-*` |
| `AlignClassRegistry` | `align-*`, `valign-*`, `flex-*` |
| `BorderClassRegistry` | `border-*`, `rounded-*` |
| `ShadowClassRegistry` | `shadow-*` |
| `OpacityClassRegistry` | `opacity-*` |
| `ShapeClassRegistry` | `shape-*` |
| `FocusClassRegistry` | `focus-*` |
| `TypographyClassRegistry` | `text-*`, `font-*`, `header-*` |
| `StateClassRegistry` | `is-*`, `use-*`, `has-role` |
| `AnimationClassRegistry` | motion-related classes |

---

## See also

- [Theme catalog — colors](theme-catalog-colors.md) — roles for `variant-*`
- [Theme catalog — tokens](theme-catalog-tokens.md) — values behind `p-md`, `rounded-sm`, …
- [Theming (engine)](theming.md) — `ClassRegistry` lazy activation
- [Showcase Playground](../../demos/MyNet.Avalonia.Showcase/Views/Playground/AppearanceView.axaml)
