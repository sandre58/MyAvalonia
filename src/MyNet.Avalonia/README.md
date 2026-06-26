<div align="center">

# MyNet.Avalonia

<img src="../../assets/MyAvalonia.png" alt="MyNet.Avalonia" width="96" height="96" />

*Theme-agnostic Avalonia core: globalization markup, value converters, bindings, clipboard abstractions, and color utilities.*

</div>

<div align="center">

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia)](https://www.nuget.org/packages/MyNet.Avalonia)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/10.0)

</div>

---

## Features

| Feature | Description |
| :------ | :---------- |
| **Markup** | {my:Loc}, {my:Display}, and globalization markup extensions |
| **Converters** | Logic, layout, brush, and localization value converters |
| **Bindings** | Avalonia binding helpers and application resources |
| **Clipboard** | DI-friendly clipboard abstractions and extensions |

---

## Installation

```bash
dotnet add package MyNet.Avalonia
```

Theme-agnostic core — no visual styles. For the MyNet look, add [Theme](../MyNet.Avalonia.Theme/README.md) packages (see [Getting started](../../docs/getting-started.md)).

## Quick start

Requires **MyNet.UI** + **MyNet.Globalization** on the host:

```csharp
services.AddUi(/* cultures */)
    .AddMyNetAvaloniaColors();
provider.UseUi();
provider.UseMyNetAvaloniaClipboard(); // optional, after Extended clipboard registration
```



---
## Documentation

| Guide | Topic |
|-------|-------|
| [Getting started](../../docs/getting-started.md) | Full app bootstrap |
| [Markup & converters](../../docs/guides/markup-and-converters.md) | `{my:Loc}`, `{my:Display}`, converters |
| [Guides index](../../docs/guides/README.md) | All system guides |
| [Showcase](../../demos/MyNet.Avalonia.Showcase/) | Runnable examples |



---
## Related packages

- [MyNet.UI](https://www.nuget.org/packages/MyNet.UI) · [MyNet.Globalization](https://www.nuget.org/packages/MyNet.Globalization) · [MyNet.Observable](https://www.nuget.org/packages/MyNet.Observable)
---

<div align="center">

<sub>

Copyright © 2016-2026 - Stéphane ANDRE. All Rights Reserved.

<br/>

Released under the [MIT License](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE).

</sub>

</div>
