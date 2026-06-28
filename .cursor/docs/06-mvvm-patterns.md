# MVVM Patterns (Showcase)

Applies to `demos/MyNet.Avalonia.Showcase/` only.

## Base types

| Type | Source | Use |
|------|--------|-----|
| `ObservableObject` | MyNet.Observable | All ViewModels |
| `PageViewModel` | Showcase | Navigable demo pages |
| `DialogViewModel<T>` | MyNet.UI | Dialogs (e.g. LoginDialog) |

## Commands

Inject `ICommandFactory` → `AvaloniaCommandFactory` (UI-thread `CanExecuteChanged`).

```csharp
SaveCommand = commandFactory.Create(async () => await SaveAsync());
```

## Compiled bindings

Every page AXAML must declare `x:DataType`:

```xml
<ContentPage x:DataType="vm:HomePageViewModel" ...>
```

## Localization in ViewModels

```csharp
[UpdateOnCultureChanged]
public string Title => field.Translate();
```

Register `.resx` in `AppComposition.RegisterTranslations`.

## Validation

`FormViewModel` sample: FluentValidation + `ValidationBehavior<T>` + `INotifyDataErrorInfo`.
Themed errors via `ValidationAssist.Theme` in AXAML.

## Culture / globalization host setup

```csharp
services.UseGlobalization();
// After build:
ValidationLocalization.Configure();
```

See also: `11-markup-extensions.md` for XAML markup patterns.

Deep conventions: `.cursor/reference/conventions.md`
