<div align="center">

# MyNet.Avalonia.Theme.Controls

<img src="../../assets/MyAvaloniaThemeControls.png" alt="MyNet.Avalonia.Theme.Controls" width="96" height="96" />

*Control themes and DataGrid columns for the MyNet Avalonia design system (Foundation, Standard, Custom catalogs).*

</div>

<div align="center">

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Theme.Controls)](https://www.nuget.org/packages/MyNet.Avalonia.Theme.Controls)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/10.0)

</div>

---

## Features

| Feature | Description |
| :------ | :---------- |
| **Catalog** | Precompiled ThemeControlsCatalog Styles entry point |
| **Layout** | Foundation, Standard, and Custom control themes |
| **DataGrid** | Themed DataGrid columns and templates |
| **Startup** | Load after MyTheme in Application.Styles |

---

## Installation

```bash
dotnet add package MyNet.Avalonia.Theme.Controls
```

Requires `MyNet.Avalonia.Theme`, `MyNet.Avalonia.Controls`, and `MyNet.Avalonia`.

## Quick start

Load immediately after `<my:MyTheme />`:

```xml
<Application xmlns:my="http://mynet.com/avalonia">
    <Application.Styles>
        <my:MyTheme />
        <my:ThemeControlsCatalog />
    </Application.Styles>
</Application>
```



---
## Documentation

| Guide | Topic |
|-------|-------|
| [Theme controls](../../docs/guides/theme-controls.md) | Catalog layers, anti-patterns |
| [Theming](../../docs/guides/theming.md) | `MyTheme`, tokens |
| [Getting started](../../docs/getting-started.md) | Full stack |



---
## Related packages

- [MyNet.Avalonia.Theme](../MyNet.Avalonia.Theme/README.md) · [MyNet.Avalonia.Controls](../MyNet.Avalonia.Controls/README.md)
---

<div align="center">

<sub>

Copyright © 2016-2026 - Stéphane ANDRE. All Rights Reserved.

<br/>

Released under the [MIT License](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE).

</sub>

</div>
