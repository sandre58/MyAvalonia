# MyNet.Avalonia.Showcase

Application de référence pour le stack **MyNet** sur Avalonia : contrôles thémés, navigation, shell (culture / thème), validation, dialogs et toasts.

Hôte desktop : `MyNet.Avalonia.Showcase.Desktop`.

## Démarrage

```bash
dotnet run --project demos/MyNet.Avalonia.Showcase.Desktop
```

## Bootstrap Avalonia (`App.axaml`)

Dans `Application.Styles`, dans cet ordre :

1. `<my:MyTheme />`
2. `<my:ThemeControlsCatalog />` — le catalogue est déclaré ici pour ce showcase (chargement au `AvaloniaXamlLoader.Load`). Les apps consommatrices peuvent préférer l’attachement différé documenté dans `MyNet.Avalonia.Theme.Controls` si le démarrage le permet.
3. `<StyleInclude Source="avares://MyNet.Avalonia.Extended/Themes/Generic.axaml" />`
4. Styles spécifiques à l’application

Dans `App.Initialize` : `AvaloniaXamlLoader.Load(this)` puis `MyTheme.Current.EnsureLoaded()`.

Avant la fenêtre principale : `MyTheme.Current.ApplyVariantBrushes()`.

## Composition et DI

La composition (services, pages, menu) est centralisée dans `Composition/AppComposition.cs`. `App.axaml.cs` ne fait que le bootstrap Avalonia et l’affichage des fenêtres.

### Enregistrement (ordre recommandé)

1. `AddGlobalization` → `AddLocalization` → `AddInflection` → `AddHumanizer`
2. `AddFakers` → `AddBusy` → `AddShell`
3. `AddAvaloniaColors` → `AddMyNetAvaloniaControls` → `AddMyNetAvaloniaExtended(topLevelProvider)`
4. Ressources `.resx` du showcase via `AddResources()`
5. `IThemeBrushService` = `MyTheme.Current`
6. Enregistrer les types de `PageViewModel` en singleton (état du playground)

`AddMyNetAvaloniaExtended` enregistre aussi navigation, clipboard, dialogs, toasting, commandes et scheduler (voir `MyNet.Avalonia.Extended`).

### Après `BuildServiceProvider()`

- `UseGlobalization` → `UseLocalization` → **`ValidationLocalization.Configure()`** → `UseDisplayText`
- `UseFakers` → `UseThemeManager` → `UseAvaloniaClipboard` → `UseMyNetAvaloniaExtended()` (navigation)

### Top-level (desktop et browser)

`AddMyNetAvaloniaExtended` reçoit un `Func<TopLevel?>` :

- **Desktop** : `IClassicDesktopStyleApplicationLifetime.MainWindow`
- **Browser** : `TopLevel.GetTopLevel(singleView.MainView)` une fois la vue racine assignée

### Shell (`MainView` / `MainWindow`)

| Élément | Desktop | Browser |
|---------|---------|---------|
| Menu + navigation | `MainView` | `MainView` |
| Marque + historique + culture / thème | `ShellPrimaryChrome` + `ShellTitleBarChrome` dans la barre de titre (`MainWindow`) | même bandeau en haut de `MainView` (`ShowShellChromeInView`) |
| Overlay dialogs + busy | `MainView` (`OverlayDialogHost`, `BusyServiceIndicator`) | idem |

`MainWindow` ne contient plus que le chrome fenêtre Avalonia + `MainView`.

### Menu navigation

Les entrées du menu sont des `LazyPageMenuItem` : le `PageViewModel` n’est résolu depuis le conteneur DI qu’à l’ouverture de la page (clic menu ou navigation). Les types restent en singleton une fois créés.
Le clic sur une entrée appelle `NavigateCommand` avec `NavigationTarget`, puis `SelectedMenuItem` est resynchronisé sur l’état réel de navigation via `INavigationService.StateChanged`.

Le menu est construit par `AppComposition.ConfigureMainViewModel()` (pas dans `App.axaml.cs`). Un en-tête de section (`MenuSectionViewModel` → `NavigationMenuItem.IsSeparator`, titre + icône) précède le catalogue de composants via `PagesCatalog.CreateMenuItems`. Le rail n’affiche que le repli du menu et la recherche (`MenuSearchText` / `MenuFilter`). La marque et l’historique Précédent / Suivant sont dans `ShellPrimaryChrome` (barre de titre desktop avec `ReserveTitleBarSafeArea`, bandeau browser).

### Aperçu designer (DEBUG)

`MainView` et `MainWindow` utilisent `d:DataContext="{x:Static composition:MainViewDesignData.MainViewModel}"` : même DI et catalogue de pages que au runtime.

## À copier en production

| Pattern | Où dans le showcase |
|--------|----------------------|
| Shell + navigation | `MainWindow`, `MainView`, `MainViewModel` |
| Composition DI | `Composition/AppComposition.cs` |
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
| `<my:ThemeControlsCatalog />` dans `App.axaml` | Requis pour ce host showcase ; pas le seul pattern possible |
| Sous-menus pilotés par `IMenuItemViewModel.Items` | Permet les bindings compilés sans `ReflectionBinding` dans le menu |
| Pas de virtualisation des listes | Simplicité du catalogue ; la page Icônes utilise la **pagination** |
| `PagesCatalog` explicite | Registre de toutes les pages démo |
| Moteur `ThemeBuilder` / playground | Outil interactif du showcase, hors packages MyNet |

## Test plan (browser / WASM)

Après `dotnet run --project demos/MyNet.Avalonia.Showcase.Browser` :

- [ ] Bandeau culture + thème visible en haut de `MainView`
- [ ] Navigation menu : ouverture d’une page racine et d’une page dans un groupe
- [ ] Page **Dialogs** : overlay modal et non modal
- [ ] Page **Notifications** : toasts visibles
- [ ] Changement de culture : libellés menu / page mis à jour
- [ ] Bascule clair/sombre : contrôles MyNet cohérents

## Validation (exemple officiel)

```csharp
// Constructeur du ViewModel
_validation = this.UseValidation(new FormViewModelValidator());
_validation.ErrorsChanged += (_, e) => ErrorsChanged?.Invoke(this, e);

// Démarrage app
ValidationLocalization.Configure();
```

Le ViewModel expose `INotifyDataErrorInfo` via `IValidationAware` pour que `DataValidationErrors` des contrôles MyNet affiche les erreurs inline.

## Documentation

- [MyAvalonia docs](../../docs/index.md) · [Guides](../../docs/guides/README.md) · [Getting started](../../docs/getting-started.md)
- [Guides MyNet](https://github.com/sandre58/MyNet/tree/main/docs/guides) (MVVM contracts)
- Package READMEs : `src/MyNet.Avalonia*/README.md`
