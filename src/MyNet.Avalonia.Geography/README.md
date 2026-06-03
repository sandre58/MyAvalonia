
<div align="center">
  <img src="../../assets/MyAvalonia.png" width="96" alt="MyAvalonia">
</div>

# MyNet.Avalonia.Geography

Optional Avalonia satellite package for [MyNet.Geography](https://www.nuget.org/packages/MyNet.Geography). Use it when your UI needs country pick lists, culture display templates, or a compact culture picker without pulling geography into the core `MyNet.Avalonia` package.

Depends on [`MyNet.Avalonia`](../MyNet.Avalonia/README.md) for `{my:Display}` and `EqualsConverter`. Styled controls such as `DropDownButton` require your app to load [`MyNet.Avalonia.Theme.Controls`](../MyNet.Avalonia.Theme.Controls/README.md) (e.g. `<my:ThemeControlsCatalog />`).

## Installation

```bash
dotnet add package MyNet.Avalonia.Geography
```

## Resources

Merge geography data templates in `App.axaml` (after theme styles):

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceInclude Source="avares://MyNet.Avalonia.Geography/Resources/GeographyDataTemplates.axaml" />
</ResourceDictionary.MergedDictionaries>
```

| Key | Use |
|-----|-----|
| `MyNet.DataTemplate.Country.Xs` | Country + 16px flag in lists |
| `MyNet.DataTemplate.Country.Xl` | Country tile (64px flag) |
| `MyNet.DataTemplate.CultureInfo.Xs` | Culture + flag + title in menus |
| `MyNet.DataTemplate.CultureInfo.Flag` | Culture flag only (compact button) |

## XAML

Register the `geo` XML namespace (included when referencing this assembly):

```xml
xmlns:geo="http://mynet.com/avalonia/geography"

<ComboBox ItemsSource="{geo:Countries}" />
```

## CulturePicker

Shell-agnostic culture selector (flag `DropDownButton` + checkable menu):

```xml
<geo:CulturePicker Cultures="{Binding Cultures}"
                   SelectedCulture="{Binding SelectedCulture}"
                   SelectCultureCommand="{Binding SelectCultureCommand}"
                   ToolTipText="Language"
                   AutomationLabel="Language" />
```

With `ShellCultureViewModel` from MyNet.UI:

```xml
<geo:CulturePicker Cultures="{Binding Culture.Cultures}"
                   SelectedCulture="{Binding Culture.SelectedCulture}"
                   SelectCultureCommand="{Binding Culture.ChangeCultureCommand}" />
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
