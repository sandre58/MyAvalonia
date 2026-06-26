# Theming — engine (`MyTheme`)

**How the theme system works** — architecture, setup, lifecycle, APIs, MVVM integration, performance.

**Catalog references** (what is available):

| Catalog | Guide |
|---------|-------|
| Colors & brushes | [theme-catalog-colors.md](theme-catalog-colors.md) |
| Design tokens (spacing, corners, motion) | [theme-catalog-tokens.md](theme-catalog-tokens.md) |
| Utility CSS classes | [theme-catalog-utility-classes.md](theme-catalog-utility-classes.md) |
| Control templates | [theme-controls.md](theme-controls.md) |

**Packages:** [MyNet.Avalonia.Theme](../../src/MyNet.Avalonia.Theme/README.md) · [MyNet.Avalonia.Extended](../../src/MyNet.Avalonia.Extended/README.md) (optional)

For **MyNet.UI contracts** (`IThemeService`, preferences UI), see [MyNet theming guide](https://github.com/sandre58/MyNet/blob/main/docs/guides/theming.md).

**Reference app:** [Showcase Theme page](../../demos/MyNet.Avalonia.Showcase/Pages/ThemePage.axaml)

---

## Architecture

```text
App.axaml (Application.Styles)
  │
  ├─ <my:MyTheme /> ─────────────────────────────────────────────┐
  │     ThemeDictionaries (Dark/Light/…)  → MyNet.Color.*        │
  │     Tokens/_index.axaml               → MyNet.Spacing.*, …   │
  │     ClassesBootstrapper               → ClassRegistry          │
  │     ThemePaletteInjector              → MyNet.Brush.* (mutable)│
  │     BrushManager                      → transformed brush LRU│
  │                                                                 │
  └─ <my:ThemeControlsCatalog /> ── control templates (optional)  │
                                                                    │
  Controls consume:  {my:Theme …}  |  Classes="variant-solid …"  |  ThemeAssist.Role
```

Three runtime mechanisms:

| Mechanism | Trigger | Output |
|-----------|---------|--------|
| **Variant switch** | `MyTheme.Theme` / OS theme | Reload `ThemeDictionaries`, refresh semantic brushes |
| **Brand update** | `Primary` / `Accent` change | Regenerate `ColorShades` brushes in place |
| **Utility class** | `Classes="size-md"` added | `ClassRegistry` handler sets properties from tokens |

Brushes under `MyNet.Brush.*` are **mutable** `SolidColorBrush` — UI updates without rebinding.

---

## Setup

### Packages

| Need | Packages |
|------|----------|
| Engine only | `MyNet.Avalonia.Theme`, `MyNet.Avalonia` |
| Styled controls | + `Theme.Controls`, `Controls` |
| MVVM switching | + `Extended`, `MyNet.UI` |

[Getting started](../getting-started.md)

### App.axaml

```xml
<Application xmlns:my="http://mynet.com/avalonia">
    <Application.Styles>
        <my:MyTheme Primary="#1756BD" Accent="#FFAE18" />
        <my:ThemeControlsCatalog />
    </Application.Styles>
</Application>
```

1. **Styles before** `Application.Resources`
2. **`MyTheme` before** `ThemeControlsCatalog`
3. Brand colors accept hex strings in XAML

### App.axaml.cs

```csharp
public override void Initialize()
{
    AvaloniaXamlLoader.Load(this);
    MyTheme.Current.EnsureLoaded();
}
```

`EnsureLoaded()`:

- Subscribes to `ActualThemeVariantChanged`
- Loads base resources (lazy store → eager)
- Calls `ApplyVariantBrushes()`
- **Idempotent** for base load; safe on every startup

### Variant sync

```csharp
MyTheme.Current.Theme = "Dark";
// equivalent to syncing Application.Current.ActualThemeVariant
Application.Current!.RequestedThemeVariant = ThemeVariant.Dark;
```

Built-in keys: `Dark`, `Light`, `DarkBlue`, `HighContrast` — see [color catalog](theme-catalog-colors.md#theme-variants).

---

## Lifecycle & events

| API | When fired / updated |
|-----|----------------------|
| `ThemeChanged` | After primary, accent, or variant brush batch |
| `ThemeVersion` | Incremented after coordinated update — bind to force refresh |
| `ColorTransitionDuration` | Default 150 ms for animated color changes |
| `TransitionIsEnabled` | Toggle animations |

Internal coordinators:

- `ThemeChangeCoordinator` — batches/defer nested updates
- `ThemeVariantCoordinator` — dictionaries + custom `RegisterThemeProvider`
- `ThemePaletteInjector` — brand + semantic brush registration
- `BrushManager` / `BrushSet` — LRU for opacity/contrast transforms (default cap **48**)

### Custom themes

```csharp
MyTheme.Current.RegisterThemeProvider(customVariantPalette);
MyTheme.Current.ApplyTheme(completeThemePalette); // Primary + Accent + variant atomically
```

---

## Markup extensions (overview)

Namespace: `xmlns:my="http://mynet.com/avalonia"`

| Extension | Resolves from | Details |
|-----------|---------------|---------|
| `{my:Theme Path}` | `MyNet.Brush.{Path}` | [Color catalog](theme-catalog-colors.md#markup--brush-bindings) |
| `{my:ThemeRole VariantBrush}` | Role + variant brush | Requires `ThemeAssist.Role`, often `has-role` |
| `{my:ThemeContext Path}` | Context + path | Uses `ThemeAssist.Context` |
| `{my:ThemeBrush …}` | Full binding + `RelativeSource` | Advanced scenarios |
| `{my:Thickness …}`, `{my:Icon …}` | Token-backed | [Token catalog](theme-catalog-tokens.md) |

Transform parameters on all brush extensions: `Opacity`, `CustomOpacity`, `Contrast`, `Darken`, `Lighten`.

---

## `IThemeBrushService`

`MyTheme` implements the brush service contract:

```csharp
services.AddSingleton<IThemeBrushService>(MyTheme.Current)
    .AddAvaloniaTheming();
```

| Method | Purpose |
|--------|---------|
| `SetTheme(name)` | Switch variant |
| `SetPrimary(color, fg?)` / `SetAccent(...)` | Brand update |
| `SetTheme(name, primary, accent, …)` | Atomic |
| `GetThemePalette()` | Snapshot for diagnostics UI |
| `GetBrush(path, opacity?, contrast?, …)` | Transformed brush |

---

## MVVM (`IThemeService`)

Extended `ThemeService` maps **MyNet.UI** `Theme` → `IThemeBrushService`:

```csharp
themeService.ApplyTheme(UiTheme.Dark);
themeService.ApplyPrimary("#1756BD");
themeService.ApplyBaseTheme(ThemeVariantProvider.DarkBlue);
```

Register bases for preferences:

```csharp
services.GetRequiredService<IThemeBaseRegistry>()
    .Register(new ThemeBase(ThemeVariantProvider.DarkBlue, isDark: true, isHighContrast: false));
```

Extensions: `AddBaseExtension` / `AddPrimaryExtension` / `AddAccentExtension` push app-specific resources on change.

Showcase: [`ThemePageViewModel`](../../demos/MyNet.Avalonia.Showcase/ViewModels/Pages/ThemePageViewModel.cs), `AppComposition.InitializeTheme`.

---

## Utility class engine

1. `ClassesBootstrapper.Initialize()` registers all `*ClassRegistry` handlers (once)
2. Control gets `Classes="variant-solid size-md"`
3. `ClassRegistry` invokes handler → sets properties from tokens/brushes
4. Removing class disposes handler subscription

Programmatic layering: `ClassesAssist` — see [utility class catalog](theme-catalog-utility-classes.md#programmatic-classes-classesassist).

Role/context attached properties (`ThemeAssist`) auto-sync CSS classes and propagate to popups.

---

## Performance

**Startup:** `EnsureLoaded()` during splash; use `<my:ThemeControlsCatalog />` not hundreds of `StyleInclude`.

**Diagnostics:**

```csharp
ThemeDiagnostics.ConfigureFromEnvironment(); // MYNET_THEME_PERF=1
ThemeDiagnostics.EnableDefaultCategories();
```

| Option | Default |
|--------|---------|
| `BrushManagerOptions.PrewarmThemeOpacityLevels` | `true` |
| `BrushSetOptions.TransformedBrushCapacity` | `48` |

```bash
dotnet run -c Release --project benchmarks/MyNet.Avalonia.Theme.Benchmarks
dotnet test tests/MyNet.Avalonia.Theme.Tests
```

---

## Anti-patterns

| Don't | Do |
|-------|-----|
| Merge `MyTheme.axaml` as loose `ResourceDictionary` | `<my:MyTheme />` in `Application.Styles` |
| Static brush on `MyNet.Brush.*` keys | `{my:Theme …}` or runtime API |
| Manual dictionary swap for dark mode | `MyTheme.Theme` / `IThemeService` |
| `{my:ThemeRole}` without role | `ThemeAssist.Role` + `has-role` |
| Skip `EnsureLoaded()` on large apps | Preload at startup |

---

## Troubleshooting

| Symptom | Check |
|---------|-------|
| `Cannot locate MyTheme` | Missing `<my:MyTheme />` |
| Null `{my:Theme}` | Styles after resources; call `EnsureLoaded()` |
| Utility class no-op | Typo; `MyTheme` not loaded (no bootstrap) |
| Colors wrong in overlay | `ThemeAssist.Context`, `Surface.Overlay` — [colors](theme-catalog-colors.md) |
| Variant change ignored | Binding static color not `{my:Theme}` |

---

## See also

- [Theme catalog — colors](theme-catalog-colors.md)
- [Theme catalog — tokens](theme-catalog-tokens.md)
- [Theme catalog — utility classes](theme-catalog-utility-classes.md)
- [Theme controls](theme-controls.md)
- [Extended host](extended-host.md)
- [Package README](../../src/MyNet.Avalonia.Theme/README.md)
