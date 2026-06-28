# MyNet Companion Packages

MyAvalonia depends on the **MyNet** suite (separate repo: https://github.com/sandre58/MyNet).

Version pinned in `Directory.Packages.props` (currently **19.6.0**).

## Packages used by MyAvalonia

| Package | Role in this repo |
|---------|-------------------|
| MyNet.UI | Navigation, dialogs, shell ViewModels, ICommandFactory |
| MyNet.Observable | ObservableObject, ValidationBehavior, UpdateOnCultureChanged |
| MyNet.Globalization | Loc extensions, Translate(), culture services |
| MyNet.Primitives | Shared primitives (enums, etc.) |
| MyNet.Utilities | Logging helpers |
| MyNet.Humanizer | Human-readable formatting |
| MyNet.Geography | Country/culture data (Geography package) |

## Showcase-only packages

MyNet.Collections, MyNet.Fakers, Microsoft.Extensions.* — demo app only, not in framework packages.

## Key external interfaces (do not invent)

- `ICommandFactory`, `INavigationService`, `IBusyService`
- `DialogViewModel<T>`, `INavigationPage`
- `GlobalizationServices.Current`
- `LocalizedEnum`, `LocalizedSmartEnum`

## DI pattern (Showcase)

```csharp
services.AddUi(...)
    .AddMyNetAvaloniaExtended(topLevelProvider)
    .AddMyNetAvaloniaControls()
    ...
```

## Rule for AI

If an API is not found in this repo or MyNet docs, **ask** — do not hallucinate method signatures.

Getting started: `docs/getting-started.md`
