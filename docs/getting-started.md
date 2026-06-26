# Getting started with MyAvalonia

Bootstrap a cross-platform Avalonia app with the MyNet stack: themed controls, globalization markup, and optional MVVM shell services from **MyNet.UI**.

**Reference app:** [`demos/MyNet.Avalonia.Showcase`](../demos/MyNet.Avalonia.Showcase/) — [`AppComposition.cs`](../demos/MyNet.Avalonia.Showcase/Composition/AppComposition.cs), [`App.axaml`](../demos/MyNet.Avalonia.Showcase/App.axaml).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Avalonia project (desktop or single-view)
- **MyNet** packages when using markup, shell, or dialogs (`MyNet.UI`, `MyNet.Globalization`, …)

## Package stacks

### Minimal (markup + converters only)

Theme-agnostic core — no MyNet visual theme:

```bash
dotnet add package MyNet.Avalonia
dotnet add package MyNet.UI
dotnet add package MyNet.Globalization
```

See [Markup & converters](guides/markup-and-converters.md).

### Full MyNet look (recommended)

Themed controls + tokens + catalog:

```bash
dotnet add package MyNet.Avalonia
dotnet add package MyNet.Avalonia.Controls
dotnet add package MyNet.Avalonia.Theme
dotnet add package MyNet.Avalonia.Theme.Controls
```

See [Theming](guides/theming.md) and [Theme controls](guides/theme-controls.md).

### MVVM application (Showcase stack)

Adds dialogs, toasts, navigation, clipboard, theme preferences:

```bash
# … full look packages above, plus:
dotnet add package MyNet.UI
dotnet add package MyNet.Avalonia.Extended
```

See [Extended host](guides/extended-host.md) and [MyNet UI guides](https://github.com/sandre58/MyNet/tree/main/docs/guides).

## Application bootstrap

### 1. `App.axaml` — load styles before resources

Styles must load **before** `Application.Resources` so `{my:Theme}` and tokens resolve in app markup.

```xml
<Application xmlns:my="http://mynet.com/avalonia"
             RequestedThemeVariant="Light">
    <Application.Styles>
        <my:MyTheme />
        <my:ThemeControlsCatalog />
        <!-- When using Extended / Geography: -->
        <StyleInclude Source="avares://MyNet.Avalonia.Extended/Themes/Generic.axaml" />
        <StyleInclude Source="avares://MyNet.Avalonia.Geography/Themes/Generic.axaml" />
    </Application.Styles>
</Application>
```

### 2. `App.axaml.cs` — preload theme

```csharp
public override void Initialize()
{
    AvaloniaXamlLoader.Load(this);
    MyTheme.Current.EnsureLoaded();
}
```

### 3. Dependency injection

```csharp
using MyNet.Avalonia;
using MyNet.Avalonia.Clipboard.Extensions;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended;
using MyNet.Avalonia.Theme;
using MyNet.UI;
using MyNet.UI.Theming;

TopLevel? TopLevelProvider() => /* main window or single-view root */;

var services = new ServiceCollection();
services.AddUi(b => b.WithSupportedCultures(/* … */))
    .AddMyNetAvaloniaColors()
    .AddMyNetAvaloniaControls()
    .AddMyNetAvaloniaExtended(TopLevelProvider)
    .AddSingleton<IThemeBrushService>(MyTheme.Current);

var provider = services.BuildServiceProvider();
provider.UseUi();
provider.UseMyNetAvaloniaClipboard();
provider.UseMyNetAvaloniaExtended();
```

Register `.resx` files with `AddTranslationResource` (Showcase: `RegisterTranslations` in `AppComposition`).

### 4. Shell XAML — overlay host (dialogs)

```xml
xmlns:controls="http://mynet.com/avalonia/controls"

<controls:OverlayDialogHost HostId="{x:Static controls:OverlayDialogHostManager.MainHostId}" />
```

## Dependency layering

```text
┌─────────────────────────────────────────────┐
│  Your app (Views, ViewModels, App.axaml)    │
└─────────────────────┬───────────────────────┘
                      │
┌─────────────────────▼───────────────────────┐
│  MyNet.Avalonia.Extended                    │
│  Dialog / toast / nav / IThemeService       │
└─────────────────────┬───────────────────────┘
                      │
        ┌─────────────┼─────────────┐
        ▼             ▼             ▼
  Theme.Controls   Controls      Theme
        │             │             │
        └─────────────┴─────────────┘
                      │
                      ▼
              MyNet.Avalonia (markup, converters)
                      │
                      ▼
              MyNet.UI + MyNet.Globalization
```

## Next steps

| Goal | Guide |
|------|-------|
| Brand colors, dark mode, `{my:Theme}` | [Theming](guides/theming.md) |
| Control catalog, overlay dialogs | [Controls & overlays](guides/controls-and-overlays.md) |
| Content dialogs, toasts | [Extended host](guides/extended-host.md) |
| `{my:Loc}`, converters | [Markup & converters](guides/markup-and-converters.md) |
| Country / culture picker | [Geography (Avalonia)](guides/geography-avalonia.md) |

[Guides index](guides/README.md) · [Documentation index](index.md)
