
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
