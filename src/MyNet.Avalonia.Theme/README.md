<div align="center">

# MyNet.Avalonia.Theme

<img src="../../assets/MyAvaloniaTheme.png" alt="MyNet.Avalonia.Theme" width="96" height="96" />

*Comprehensive theming system with custom styles, control templates, design tokens, and visual resources for Avalonia applications.*

</div>

<div align="center">

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Theme)](https://www.nuget.org/packages/MyNet.Avalonia.Theme)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/10.0)

</div>

---

## Features

| Feature | Description |
| :------ | :---------- |
| **MyTheme** | Runtime theme engine with Dark, Light, and HighContrast variants |
| **Markup** | {my:Theme}, {my:ThemeRole}, and utility CSS-like classes |
| **Performance** | Brush LRU cache, diagnostics, and BenchmarkDotNet suite |
| **Tokens** | Design tokens and mutable theme brushes |

---

## Installation

```bash
dotnet add package MyNet.Avalonia.Theme
dotnet add package MyNet.Avalonia.Theme.Controls   # styled controls
dotnet add package MyNet.Avalonia.Controls
dotnet add package MyNet.Avalonia
```

## Quick start

```xml
<Application xmlns:my="http://mynet.com/avalonia">
    <Application.Styles>
        <my:MyTheme />
        <my:ThemeControlsCatalog />
    </Application.Styles>
</Application>
```

```csharp
public override void Initialize()
{
    AvaloniaXamlLoader.Load(this);
    MyTheme.Current.EnsureLoaded();
}
```



---
## Documentation

| Guide | Topic |
|-------|-------|
| [Theming engine](../../docs/guides/theming.md) | How `MyTheme` works |
| [Color & brush catalog](../../docs/guides/theme-catalog-colors.md) | Semantic colors, roles, markup |
| [Token catalog](../../docs/guides/theme-catalog-tokens.md) | Spacing, corners, motion |
| [Utility classes](../../docs/guides/theme-catalog-utility-classes.md) | `variant-*`, `size-*`, … |
| [Theme controls](../../docs/guides/theme-controls.md) | Control template catalog |
| [Showcase](../../demos/MyNet.Avalonia.Showcase/) | Theme page, Playground |



---
## Related packages

- [MyNet.Avalonia.Theme.Controls](../MyNet.Avalonia.Theme.Controls/README.md) · [MyNet.Avalonia.Extended](../MyNet.Avalonia.Extended/README.md)
---

<div align="center">

<sub>

Copyright © 2016-2026 - Stéphane ANDRE. All Rights Reserved.

<br/>

Released under the [MIT License](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE).

</sub>

</div>
