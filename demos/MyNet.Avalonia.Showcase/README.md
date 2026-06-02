# MyNet.Avalonia.Showcase

Application de référence pour le stack **MyNet** sur Avalonia : contrôles thémés, navigation, shell (culture / thème), validation, dialogs et toasts.

Hôte desktop : `MyNet.Avalonia.Showcase.Desktop`.

## Démarrage

```bash
dotnet run --project demos/MyNet.Avalonia.Showcase.Desktop
```

## Enregistrement DI (ordre recommandé)

À reproduire dans une application consommatrice (voir `App.axaml.cs`) :

1. `AddGlobalization` → `AddLocalization` → `AddInflection` → `AddHumanizer`
2. `AddMyNetAvalonia` → `AddMyNetAvaloniaControls` → `AddMyNetAvaloniaExtended`
3. `AddAvaloniaTheming` → `AddAvaloniaClipboard` → `AddAvaloniaAppCommands` → `AddAvaloniaScheduler`
4. `AddNotifications` → `AddToasting` → `AddAvaloniaToasting` → `AddBusy`
5. `AddNavigation` → `AddViewLocators` → `AddAvaloniaNavigation` → `AddShell`
6. Ressources `.resx` de l’app via `AddTranslationResource`
7. `IThemeBrushService` = `MyTheme.Current`

Après `BuildServiceProvider()` :

- `UseGlobalization` → `UseLocalization` → **`ValidationLocalization.Configure()`** → `UseDisplayText`
- `UseThemeManager` → `UseAvaloniaNavigation` → `UseClipboard`
- `MyNetThemeBootstrap.Initialize(this)` dans `App.Initialize`
- `MyNetThemeBootstrap.LoadTheme(this)` avant la fenêtre principale
- `<my:MyTheme />` dans `Application.Styles` (pas seulement en merged dictionary)

## À copier en production

| Pattern | Où dans le showcase |
|--------|----------------------|
| Shell + navigation | `MainWindow`, `MainView`, `MainViewModel` |
| Pages `INavigationPage` | `PageViewModel` |
| Commandes UI thread | `ICommandFactory` / `AvaloniaCommandFactory` |
| Thème runtime | `ShellThemeViewModel`, `MyTheme.Current` |
| Localisation | `{my:Loc}`, `AddTranslationResource` |
| Formulaire + validation | `FormViewModel`, `FormViewModelValidator`, page **Form** |
| Listes filtrées / paging | `IconsPageViewModel` |

## Spécifique à la démo (ne pas copier tel quel)

| Choix | Raison |
|-------|--------|
| ViewModels de pages en **singleton** | État du playground conservé entre visites |
| `ReflectionBinding` (menu groupes, quelques pages) | Contournement de portée de `x:DataType` dans templates imbriqués |
| Pas de virtualisation des listes | Simplicité du catalogue ; la page Icônes utilise la **pagination** |
| `ProvidePages()` monolithique | Registre explicite de toutes les pages démo |
| Moteur `ThemeBuilder` / playground | Outil interactif du showcase, hors packages MyNet |

## Validation (exemple officiel)

```csharp
// Constructeur du ViewModel
_validation = this.UseValidation(new FormViewModelValidator());
_validation.ErrorsChanged += (_, e) => ErrorsChanged?.Invoke(this, e);

// Démarrage app
ValidationLocalization.Configure();
```

Le ViewModel expose `INotifyDataErrorInfo` via `IValidationAware` pour que `DataValidationErrors` des contrôles MyNet affiche les erreurs inline.

## Documentation MyNet

- [Guides MyNet](https://github.com/sandre58/MyNet/tree/main/docs/guides)
- Packages Avalonia : `src/MyNet.Avalonia*/README.md` dans ce dépôt
