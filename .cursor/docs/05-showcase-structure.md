# Showcase Structure

The demo lives in `demos/MyNet.Avalonia.Showcase/`. Platform heads (Desktop, Browser, iOS, Android) are thin shells.

## Key files

| File | Role |
|------|------|
| `App.axaml` | Theme registration + resource dictionaries |
| `App.axaml.cs` | DI bootstrap, main window |
| `Composition/AppComposition.cs` | ServiceCollection root |
| `Composition/PagesCatalog.cs` | Explicit page registry |

## Navigation model

No convention-based discovery. Every page is manually registered:

```csharp
new PageAssociation(typeof(HomePageViewModel), typeof(HomePage), MaterialIconKind.Home)
```

Menu uses `LazyPageMenuItem` — resolves ViewModel from DI on first access.

## Folder layout

```
ViewModels/          # Shell + page ViewModels
  Base/PageViewModel.cs
  Pages/*PageViewModel.cs
Pages/               # ContentPage AXAML demos
Views/               # Shell chrome, dialogs, playground
ThemeBuilder/        # Runtime ControlTheme explorer
Resources/           # Localized .resx per feature
```

## Adding a demo page

1. Create `{Name}PageViewModel` (inherit `PageViewModel` or `ShowcaseViewModel`)
2. Create `{Name}Page.axaml` with `x:DataType`
3. Add to `PagesCatalog.cs` menu section
4. Register ViewModel singleton in `AppComposition.RegisterPageViewModels`

## DI registration flow

```
AppComposition.Build()
  → RegisterServices (AddMyNetAvalonia*, AddUi, AddFakers)
  → RegisterTranslations
  → RegisterPageViewModels
  → InitializePageMappings (ITypeResolver view ↔ viewmodel)
```

Showcase patterns are **not** framework API — do not copy into `src/`.
