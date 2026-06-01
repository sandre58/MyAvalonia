
<div align="center">
  <img src="../../assets/MyAvalonia.png" width="96" alt="MyAvalonia">
</div>

# MyNet.Avalonia.Geography

Optional Avalonia satellite package for [MyNet.Geography](https://www.nuget.org/packages/MyNet.Geography). Use it when your UI needs country pick lists without pulling geography into the core `MyNet.Avalonia` package.

## Installation

```bash
dotnet add package MyNet.Avalonia.Geography
```

## XAML

Register the `geo` XML namespace (included automatically when referencing this assembly):

```xml
xmlns:geo="http://mynet.com/avalonia/geography"

<ComboBox ItemsSource="{geo:Countries}" />
```

## Code

```csharp
using MyNet.Avalonia.Geography;

var countries = CountrySource.GetAllOrderedByDisplay();
```

## Country bindings

`CountryConverter` resolves a `Country` (or `CultureInfo`) to codes, localized names, or flag bitmaps:

```xml
<Image Source="{Binding Converter={x:Static my:CountryConverter.To24}}" />
<TextBlock Text="{Binding Converter={x:Static my:CountryConverter.ToDisplayName}}" />
```

Requires the `MyNet.Geography.Resources` package (referenced transitively by this project).
