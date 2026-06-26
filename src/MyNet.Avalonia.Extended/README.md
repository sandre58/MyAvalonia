<div align="center">

# MyNet.Avalonia.Extended

<img src="../../assets/MyAvaloniaExtended.png" alt="MyNet.Avalonia.Extended" width="96" height="96" />

*High-level UI components and services for Avalonia: dialogs, toasts, busy indicators, clipboard, and MyNet.UI integration.*

</div>

<div align="center">

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Extended)](https://www.nuget.org/packages/MyNet.Avalonia.Extended)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/10.0)

</div>

---

## Features

| Feature | Description |
| :------ | :---------- |
| **Dialogs** | Overlay and window presenters for MyNet.UI content dialogs |
| **Toasts** | Avalonia toast host with INotificationPublisher |
| **Services** | Theme, clipboard, and navigation helpers |
| **Busy indicators** | Loading states for long-running operations |

---

## Installation

```bash
dotnet add package MyNet.Avalonia.Extended
```

Typical stack: full Theme packages + `MyNet.UI` (see [Getting started](../../docs/getting-started.md)).

```xml
<StyleInclude Source="avares://MyNet.Avalonia.Extended/Themes/Generic.axaml" />
```

## Quick start

```csharp
services.AddUi(/* … */)
    .AddMyNetAvaloniaExtended(() => mainWindow)
    .AddSingleton<IThemeBrushService>(MyTheme.Current);

provider.UseUi();
provider.UseMyNetAvaloniaClipboard();
provider.UseMyNetAvaloniaExtended();
```

Overlay dialog:

```csharp
await contentDialogService.ShowAsync(vm,
    DialogOptionsFactory.ForOverlay(vm, isModal: true,
        hostId: OverlayDialogHostManager.MainHostId));
```



---
## Documentation

| Guide | Topic |
|-------|-------|
| [Extended host](../../docs/guides/extended-host.md) | DI, dialogs, toasts, theming |
| [Controls & overlays](../../docs/guides/controls-and-overlays.md) | `OverlayDialogHost` |
| [MyNet dialogs](https://github.com/sandre58/MyNet/blob/main/docs/guides/dialogs.md) | `IContentDialogService` contracts |
| [Showcase](../../demos/MyNet.Avalonia.Showcase/) | Dialogs, Notifications pages |



---
## Related packages

- [MyNet.UI](https://www.nuget.org/packages/MyNet.UI) · [MyNet.Avalonia.Controls](../MyNet.Avalonia.Controls/README.md)
---

<div align="center">

<sub>

Copyright © 2016-2026 - Stéphane ANDRE. All Rights Reserved.

<br/>

Released under the [MIT License](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE).

</sub>

</div>
