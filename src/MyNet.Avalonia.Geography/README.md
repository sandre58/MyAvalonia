<div align="center">

# MyNet.Avalonia.Geography

<img src="../../assets/MyAvaloniaGeography.png" alt="MyNet.Avalonia.Geography" width="96" height="96" />

*Avalonia markup and helpers for MyNet.Geography: country lists, culture and country templates, and CulturePicker.*

</div>

<div align="center">

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Geography)](https://www.nuget.org/packages/MyNet.Avalonia.Geography)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/10.0)

</div>

---

## Features

| Feature | Description |
| :------ | :---------- |
| **Markup** | {geo:Countries} and geography XML namespace |
| **Templates** | Country and culture data templates for lists and menus |
| **CulturePicker** | Flag dropdown with checkable culture menu |
| **Converters** | CountryConverter for codes, names, and flag bitmaps |

---

## Installation

```bash
dotnet add package MyNet.Avalonia.Geography
```

```csharp
services.AddMyNetAvaloniaGeography();
```

Merge `GeographyDataTemplates.axaml` and `Themes/Generic.axaml` — see guide below.

## Quick start

```xml
xmlns:geo="http://mynet.com/avalonia/geography"

<ComboBox ItemsSource="{geo:Countries}"
          ItemTemplate="{StaticResource MyNet.DataTemplate.Country.Xs}" />
```



---
## Documentation

| Guide | Topic |
|-------|-------|
| [Geography (Avalonia)](../../docs/guides/geography-avalonia.md) | Templates, `CulturePicker`, converters |
| [MyNet Geography](https://github.com/sandre58/MyNet/blob/main/docs/guides/geography.md) | Core country model |
| [Showcase](../../demos/MyNet.Avalonia.Showcase/) | Geography / culture pages |



---
## Related packages

- [MyNet.Geography](https://www.nuget.org/packages/MyNet.Geography) · [MyNet.Geography.Resources](https://www.nuget.org/packages/MyNet.Geography.Resources)
---

<div align="center">

<sub>

Copyright © 2016-2026 - Stéphane ANDRE. All Rights Reserved.

<br/>

Released under the [MIT License](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE).

</sub>

</div>
