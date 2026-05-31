
<div align="center">
  <img src="../../assets/MyAvalonia.png" width="128" alt="MyAvalonia">
</div>

# MyNet.Avalonia

Theme-agnostic Avalonia core: globalization markup, value converters, bindings, clipboard abstractions, and color utilities. Visual styles live in [`MyNet.Avalonia.Theme`](../MyNet.Avalonia.Theme/README.md); optional geography markup in [`MyNet.Avalonia.Geography`](../MyNet.Avalonia.Geography/README.md).

[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia?style=for-the-badge)](https://www.nuget.org/packages/MyNet.Avalonia)

## Installation

```bash
dotnet add package MyNet.Avalonia
```

Register globalization in the host before using markup extensions:

```csharp
services.UseGlobalization();
```

## Packages

| Package | Role |
| ------- | ---- |
| **MyNet.Avalonia** | Converters, `{my:Loc}` / `{my:Display}`, bindings, extensions |
| [MyNet.Avalonia.Geography](../MyNet.Avalonia.Geography/README.md) | `{geo:Countries}` and country list helpers |
| [MyNet.Avalonia.Controls](../MyNet.Avalonia.Controls/) | Controls, behaviors (`ItemsBehavior`) |
| [MyNet.Avalonia.Theme](../MyNet.Avalonia.Theme/) | Control themes, styles, visual markup |

## Markup extensions (`my`)

Namespace: `xmlns:my="http://mynet.com/avalonia"`

| Extension | Use case |
| --------- | -------- |
| `{my:Loc Key}` | Static `.resx` key (`Style`, `Filename`, `Format`, `Casing`) |
| `{my:LocObject Key}` | Same as `Loc`, returns `LocalizedString` for object properties |
| `{my:Display Path}` | Bound value with culture/time zone refresh (`Style`, `Format`, `Quantity`, `Casing`) |
| `{my:DisplayTextBlock Path}` | `Display` wrapped in a `TextBlock` |

```xml
<TextBlock Text="{my:Loc Welcome}" />
<TextBlock Text="{my:Loc ItemCount, Format=ItemsCount, Style=Abbreviation}" />
<TextBlock Text="{my:Display Count, Format=ItemsCount, Quantity=True}" />
<Button my:FormItem.Label="{my:LocObject Field_Email}" />
```

Resource keys and format strings use MyNet.Globalization `TranslationOptions` (`Style`, `Quantity`). `LetterCasing` is applied after translation.

## Value converters

Common converters are exposed on the `my` XML namespace (`MyNet.Avalonia.Converters`):

```xml
<Border IsVisible="{Binding Status, Converter={x:Static my:EqualsConverter.IsEquals}, ConverterParameter=Active}" />
<TextBlock Text="{Binding Width, Converter={x:Static my:MathConverter.Add}, ConverterParameter=10}" />
```

Categories: logic (`Equals`, `Math`, `Null`), layout (`Thickness`, `CornerRadius`), brushes/gradients, localization (`StringConverter`, `DateTimeConverter`).

## Application resources

```csharp
using MyNet.Avalonia.Resources;

var brush = ApplicationResources.GetResource<SolidColorBrush>("PrimaryBrush");
```

## Clipboard

Register `IClipboardService` in DI and call `UseClipboard()` on the built `IServiceProvider`. Theme commands use `ClipboardManager` as a static facade.

## Related MyNet packages

- `MyNet.Globalization` — translation and culture
- `MyNet.Observable` — `LocalizedString`, `LocalizedEnum`
- `MyNet.Humanizer` — enum and smart enum display
- `System.Reactive` — optional reactive helpers

## License

MIT — see [LICENSE](../../LICENSE).
