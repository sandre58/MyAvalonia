# Markup extensions & converters

**Package:** [MyNet.Avalonia](../../src/MyNet.Avalonia/README.md)

Theme-agnostic Avalonia helpers: globalization markup, value converters, color registry, clipboard facade, application resource access.

**Requires MyNet host services** for localization — not included in this package alone.

## Prerequisites (globalization)

Markup extensions `{my:Loc}` and `{my:Display}` need **MyNet.Globalization** + **MyNet.UI** configured on the host:

```csharp
services.AddUi(/* supported cultures */)
    .AddMyNetAvaloniaColors();

var provider = services.BuildServiceProvider();
provider.UseUi(); // UseGlobalization, UseLocalization, UseDisplayText, …
```

Register app `.resx` with `AddTranslationResource`. See [MyNet globalization guide](https://github.com/sandre58/MyNet/blob/main/docs/guides/globalization.md).

Optional: `AddMyNetAvaloniaControls()` when using date/time converters tied to control resources.

---

## XML namespace

```xml
xmlns:my="http://mynet.com/avalonia"
```

---

## Markup extensions

| Extension | Use case |
| --------- | -------- |
| `{my:Loc Key}` | Static `.resx` key (`Style`, `Filename`, `Format`, `Casing`) |
| `{my:LocObject Key}` | Returns `LocalizedString` for object properties |
| `{my:Display Path}` | Bound value with culture/time zone refresh |
| `{my:DisplayTextBlock Path}` | `Display` wrapped in a `TextBlock` |

```xml
<TextBlock Text="{my:Loc Welcome}" />
<TextBlock Text="{my:Loc ItemCount, Format=ItemsCount, Style=Abbreviation}" />
<TextBlock Text="{my:Display Count, Format=ItemsCount, Quantity=True}" />
<Button my:FormItem.Label="{my:LocObject Field_Email}" />
```

`TranslationOptions` (`Style`, `Quantity`) and `LetterCasing` follow MyNet.Globalization rules.

Theme-specific markup (`{my:Theme}`, …) lives in **Theme** — see [Theming](theming.md).

---

## Value converters

Exposed on the `my` namespace (`MyNet.Avalonia.Converters`):

```xml
<Border IsVisible="{Binding Status, Converter={x:Static my:EqualsConverter.IsEquals}, ConverterParameter=Active}" />
<TextBlock Text="{Binding Width, Converter={x:Static my:MathConverter.Add}, ConverterParameter=10}" />
```

| Category | Types |
| -------- | ----- |
| Logic | `EqualsConverter`, `MathConverter`, `MathComparisonConverter`, `NullConverter`, `NullFallbackConverter`, `NullToDefaultConverter`, `EnumConverter`, `ListConverter`, `IntToDecimalConverter` |
| Layout | `ThicknessConverter`, `CornerRadiusConverter`, `TransformConverter` |
| Brushes | `ColorConverter`, `BrushConverter`, `LinearGradientConverter`, `RadialGradientConverter` |
| Localization | `StringConverter`, `DateTimeConverter` |
| Input | `KeyGesturesConverter`, `ValidationErrorMessageConverter` |

Many converters expose nested static instances (e.g. `EqualsConverter.IsEquals`, `MathConverter.Add`).

---

## Colors

`AddMyNetAvaloniaColors()` registers `IColorRegistry` and localized color name resources for pickers/palettes in **Controls**.

---

## Application resources

```csharp
using MyNet.Avalonia.Resources;

var brush = ApplicationResources.GetResource<SolidColorBrush>("SomeKey");
```

---

## Clipboard

Registration lives in **Extended** (`AddAvaloniaClipboard`). Wire the static facade after build:

```csharp
provider.UseMyNetAvaloniaClipboard();
await ClipboardManager.CopyTextAsync("text");
```

See [Extended host](extended-host.md).

---

## Related MyNet packages

| Package | Role |
|---------|------|
| [MyNet.Globalization](https://www.nuget.org/packages/MyNet.Globalization) | Cultures, `.resx`, inflection |
| [MyNet.Observable](https://www.nuget.org/packages/MyNet.Observable) | `LocalizedString`, observable models |
| [MyNet.Humanizer](https://www.nuget.org/packages/MyNet.Humanizer) | Display formatters for enums, dates, lists |
| [MyNet.UI](https://www.nuget.org/packages/MyNet.UI) | Shell, navigation, dialog contracts |

[Getting started](../getting-started.md) · [Package README](../../src/MyNet.Avalonia/README.md)
