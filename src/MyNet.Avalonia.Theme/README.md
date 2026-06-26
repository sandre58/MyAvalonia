<div id="top"></div>

<!-- PROJECT INFO -->
<br />
<div align="center">
  <img src="MyAvalonia.png" width="128" alt="MyAvalonia">
</div>

<h1 align="center">My .NET Avalonia Theme</h1>

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Theme?style=for-the-badge)](https://www.nuget.org/packages/MyNet.Avalonia.Theme)

Comprehensive theming system with custom styles, control templates, and visual resources for consistent UI design in Avalonia applications.

[![.NET 10.0](https://img.shields.io/badge/.NET-10.0-purple)](#)
[![C#](https://img.shields.io/badge/language-C%23-blue)](#)
[![Cross Platform](https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey)](#)

---

## Installation

Full MyNet look (tokens + control themes + `Ripple` / `TextField` templates):

```bash
dotnet add package MyNet.Avalonia.Theme
dotnet add package MyNet.Avalonia.Theme.Controls
dotnet add package MyNet.Avalonia.Controls
```

`MyNet.Avalonia.Theme` alone provides the theme engine, design tokens, utility classes, and assists. Control themes require **`MyNet.Avalonia.Theme.Controls`** and an explicit startup call (see below).

## Basic setup

**1. Apply `MyTheme` in `Application.Styles`:**

```xml
<Application.Styles>
    <my:MyTheme />
</Application.Styles>
```

**2. Bootstrap from code** (requires `MyNet.Avalonia.Theme.Controls`):

```csharp
using MyNet.Avalonia.Theme.Controls;

public override void Initialize()
    => MyNetThemeBootstrap.Initialize(this);

public override void OnFrameworkInitializationCompleted()
{
    MyNetThemeBootstrap.LoadTheme(this);
    // show main window…
}
```

`MyTheme` embeds theme dictionaries (Dark, Light, HighContrast, …), design tokens, and utility styles. The control catalog must load **after** `EnsureLoaded()`, not via `StyleInclude` in `App.axaml`. See `MyNet.Avalonia.Theme.Controls` README for details.

## Runtime API

Access the active theme from code:

```csharp
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Theming.Palettes;

// Preload brushes and palettes (recommended during splash / startup)
MyTheme.Current.EnsureLoaded();

// Brand palettes (ColorShades with generated tones)
MyTheme.Current.Primary = new ColorShades(Color.Parse("#1756BD"));
MyTheme.Current.Accent = new ColorShades(Color.Parse("#10B981"));

// Switch built-in variant (Dark, Light, HighContrast, …)
MyTheme.Current.Theme = "Dark";
```

Brushes exposed as `MyNet.Brush.*` are **mutable** `SolidColorBrush` instances: updating `Primary` / `Accent` or the active variant updates colors in place without rebinding the whole UI.

### Custom variant registration

```csharp
MyTheme.Current.RegisterThemeProvider(customPalette);
MyTheme.Current.ApplyTheme(completeTheme);
```

## Markup extensions

Use the `my` XML namespace (`http://mynet.com/avalonia`):

```xml
<Button Background="{my:Theme Primary}"
        Foreground="{my:Theme Primary.Foreground}" />

<TextBlock Foreground="{my:ThemeRole Foreground}" Classes="has-role" />

<Border Background="{my:ThemeContext Surface.Popup}" />
```

Utility CSS-like classes (`variant-solid`, `size-md`, `gap-sm`, …) are applied on controls; the theme engine activates them lazily when a registered class is present.

## Performance diagnostics

`PerformanceMonitor` is disabled by default. Enable it when profiling:

```csharp
using MyNet.Avalonia.Theme.Diagnostics;

PerformanceMonitor.Enable(PerformanceCategory.Theme, PerformanceCategory.Brushes);
// or
ThemeDiagnostics.EnableDefaultCategories(); // Theme + Brushes
```

**Showcase demo**

- Open the **Theme** page and check **Performance diagnostics**, or
- Set environment variable `MYNET_THEME_PERF=1` before launch.

Traces are written to the debug output with the `[PERF]` prefix.

**PerfTest demo**

Compare **List (1000)** vs **Theme List (1000)** to measure theme binding and utility-class cost.

### Brush opacity prewarm

New brush registrations pre-create transforms for standard theme opacity levels (`BrushManagerOptions.PrewarmThemeOpacityLevels`, default `true`).

### Transformed brush LRU

Each `BrushSet` keeps at most **48** transformed brushes (opacity / contrast / darken / lighten variants) in an LRU cache. Main and contrast brushes are always retained. Tune globally:

```csharp
BrushSetOptions.TransformedBrushCapacity = 64;
```

Evictions are logged as `[BrushSet] Evicted transformed brush` when brush performance tracing is enabled.

### Micro-benchmarks (BenchmarkDotNet)

```bash
dotnet run -c Release --project benchmarks/MyNet.Avalonia.Theme.Benchmarks/MyNet.Avalonia.Theme.Benchmarks.csproj
```

CI runs the same job on `main` (non-blocking) and uploads Markdown/JSON reports as artifacts.

## What not to do

- Do not merge `MyTheme.axaml` as a loose `ResourceDictionary` only — you lose variant switching and brush registration.
- Do not override `MyNet.Brush.*` keys with static `SolidColorBrush` in XAML; they are created and updated by `MyTheme`.
- Do not expect `ThemeResources` / `ThemeChanged` from older samples — use `MyTheme.Current` and `IThemeBrushService` / `IThemeService` from `MyNet.Avalonia.Extended` when needed.

## Tests

Unit tests live in `tests/MyNet.Avalonia.Theme.Tests` (brush manager, class diff engine, class hasher, resource key factory).

```bash
dotnet test tests/MyNet.Avalonia.Theme.Tests/MyNet.Avalonia.Theme.Tests.csproj
```

## License

Copyright © Stéphane ANDRE.

Distributed under the MIT License. See [LICENSE](../../LICENSE) for details.
