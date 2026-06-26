<div align="center">

# MyNet.Avalonia.Controls

<img src="../../assets/MyAvaloniaControls.png" alt="MyNet.Avalonia.Controls" width="96" height="96" />

*Advanced controls and UI components for Avalonia applications: color pickers, data grids, custom cursors, and integration with MyNet libraries.*

</div>

<div align="center">

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Controls)](https://www.nuget.org/packages/MyNet.Avalonia.Controls)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/10.0)

</div>

---

## Features

| Feature | Description |
| :------ | :---------- |
| **Controls** | ColorPicker, Avatar, Calendar, DataGrid, NavigationMenu, and more |
| **Behaviors** | ItemsBehavior and elastic layout panels |
| **Overlay dialogs** | Modal and non-modal overlay presentation without MyNet.UI |
| **Cross-platform** | Windows, macOS, and Linux |

---

## Installation

```bash
dotnet add package MyNet.Avalonia.Controls
```

For styled controls, install [Theme](../../docs/guides/theming.md) + [Theme.Controls](../../docs/guides/theme-controls.md) and add `<my:MyTheme />` + `<my:ThemeControlsCatalog />` in `App.axaml`.

```csharp
services.AddMyNetAvaloniaControls(); // color picker + message .resx
```

## Quick start

```xml
xmlns:controls="http://mynet.com/avalonia/controls"

<controls:Avatar Width="64" Height="64" Initials="JD" />
<controls:ColorPickerEx SelectedColor="{Binding Accent, Mode=TwoWay}" />
```



---
## Documentation

| Guide | Topic |
|-------|-------|
| [Controls & overlays](../../docs/guides/controls-and-overlays.md) | Full catalog, overlay host |
| [Getting started](../../docs/getting-started.md) | App bootstrap |
| [Showcase](../../demos/MyNet.Avalonia.Showcase/) | Live control demos |



---
## Related packages

- [MyNet.Avalonia.Theme.Controls](https://www.nuget.org/packages/MyNet.Avalonia.Theme.Controls) — control themes
---

<div align="center">

<sub>

Copyright © 2016-2026 - Stéphane ANDRE. All Rights Reserved.

<br/>

Released under the [MIT License](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE).

</sub>

</div>
