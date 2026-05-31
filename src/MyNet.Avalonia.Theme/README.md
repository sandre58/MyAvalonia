
<div id="top"></div>

<!-- PROJECT INFO -->
<br />
<div align="center">
  <img src="../../assets/MyAvalonia.png" width="128" alt="MyAvalonia">
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

```bash
dotnet add package MyNet.Avalonia.Theme
```

## Basic setup

Apply `MyTheme` in **`Application.Styles`**, not in `MergedDictionaries`:

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:my="http://mynet.com/avalonia"
             RequestedThemeVariant="Default">
    <Application.Styles>
        <my:MyTheme />
    </Application.Styles>
</Application>
```

`MyTheme` embeds theme dictionaries (Dark, Light, HighContrast, …), design tokens, control themes, and utility styles.

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

<Border Background="{my:ThemeContext Surface.Level1}" />
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
