# Geography (Avalonia)

**Package:** [MyNet.Avalonia.Geography](../../src/MyNet.Avalonia.Geography/README.md)

Avalonia layer for [MyNet.Geography](https://www.nuget.org/packages/MyNet.Geography): country lists, flag/country data templates, `CulturePicker`, and `CountryConverter`.

Core geography model and flags: [MyNet geography guide](https://github.com/sandre58/MyNet/blob/main/docs/guides/geography.md).

## When to use

Add this package when the UI needs country pick lists, culture display templates, or a compact culture selector — without pulling geography into core `MyNet.Avalonia`.

**Depends on:** `MyNet.Avalonia` (`{my:Display}`, converters). **Styled controls** need [Theme controls](theme-controls.md).

---

## Installation & DI

```bash
dotnet add package MyNet.Avalonia.Geography
```

```csharp
services.AddMyNetAvaloniaGeography(); // AddGeographyLocalization + AddGeographyFlags
```

---

## App.axaml

```xml
<Application xmlns:geo="http://mynet.com/avalonia/geography"
             xmlns:my="http://mynet.com/avalonia">
    <Application.Styles>
        <my:MyTheme />
        <my:ThemeControlsCatalog />
        <StyleInclude Source="avares://MyNet.Avalonia.Geography/Themes/Generic.axaml" />
    </Application.Styles>
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceInclude Source="avares://MyNet.Avalonia.Geography/Resources/GeographyDataTemplates.axaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

---

## Data templates

| Key | Use |
|-----|-----|
| `MyNet.DataTemplate.Country.Xs` | Country + 16px flag in lists |
| `MyNet.DataTemplate.Country.Xl` | Country tile (64px flag) |
| `MyNet.DataTemplate.CultureInfo.Xs` | Culture + flag + title in menus |
| `MyNet.DataTemplate.CultureInfo.Flag` | Culture flag only (compact button) |

```xml
<ComboBox ItemsSource="{geo:Countries}"
          ItemTemplate="{StaticResource MyNet.DataTemplate.Country.Xs}"
          SelectedItem="{Binding Country}" />
```

---

## CulturePicker

```xml
<geo:CulturePicker Cultures="{Binding Cultures}"
                   SelectedCulture="{Binding SelectedCulture}"
                   SelectCultureCommand="{Binding SelectCultureCommand}"
                   ToolTipText="Language"
                   AutomationLabel="Language" />
```

With **MyNet.UI** shell:

```xml
<geo:CulturePicker Cultures="{Binding Culture.Cultures}"
                   SelectedCulture="{Binding Culture.SelectedCulture}"
                   SelectCultureCommand="{Binding Culture.ChangeCultureCommand}" />
```

---

## Code

```csharp
using MyNet.Avalonia.Geography;

var countries = CountrySource.GetAllOrderedByDisplay();
```

---

## CountryConverter

`xmlns:my="http://mynet.com/avalonia"`:

```xml
<Image Source="{Binding Country, Converter={x:Static my:CountryConverter.To24}}" />
<TextBlock Text="{Binding Country, Converter={x:Static my:CountryConverter.ToDisplayName}}" />
```

Flag PNGs from **MyNet.Geography.Resources** (transitive reference).

---

## Related packages

| Package | Role |
|---------|------|
| [MyNet.Geography](https://www.nuget.org/packages/MyNet.Geography) | ISO countries, addresses |
| [MyNet.Geography.Resources](https://www.nuget.org/packages/MyNet.Geography.Resources) | Embedded flag assets |
| [MyNet.Geography.Localization](https://www.nuget.org/packages/MyNet.Geography.Localization) | Localized country names |

[Getting started](../getting-started.md) · [Showcase](../../demos/MyNet.Avalonia.Showcase/)
