# MyNet.Avalonia.Theme 1.1.0

**Date:** 2026-05-31  
**Précédent sur NuGet.org:** 1.0.42

## Résumé

Version orientée **performance**, **diagnostics** et **robustesse** du moteur de thème MyNet pour Avalonia 12 / .NET 10.

## Nouveautés

### Performance (P0)

- Bindings markup `{my:Theme}`, `{my:ThemeRole}`, `{my:ThemeContext}`, `{my:ThemeBrush}` : passage de `ReflectionBinding` à `Binding` standard via `ThemeBindingHelper`.
- Classes utilitaires : suppression du style global `:is(Control)` + `UseRegisteredClasses=True` ; activation **lazy** via `ClassesAssist` au `Loaded` et lorsque une classe enregistrée est détectée.
- `ClassRegistry.ContainsRegisteredClass()` pour éviter le coût sur tous les contrôles.

### Diagnostics (P1)

- `PerformanceMonitor` autonome (`Trace`, catégories, seuils warn/error).
- `ThemeDiagnostics` : variable `MYNET_THEME_PERF=1`, intégration Showcase.
- Tests unitaires : `MyNet.Avalonia.Theme.Tests` (19 tests).

### Cache brushes (P2-1)

- LRU sur les brushes transformés par `BrushSet` (défaut : 48 entrées).
- `BrushSetOptions.TransformedBrushCapacity` configurable.
- Éviction synchronisée avec `BrushManager` (`ConditionalWeakTable.Remove`).

### Benchmarks (P2-3)

- Projet `MyNet.Avalonia.Theme.Benchmarks` (BenchmarkDotNet).
- Workflow CI `.github/workflows/benchmarks.yml` (non bloquant).

## Documentation

- `README.md` du package réécrit (setup `Application.Styles`, API `MyTheme.Current`, diagnostics, anti-patterns).

## Dépendances

- `MyNet.Avalonia`
- `MyNet.Avalonia.Controls` (package unique — pas de split Theme/Controls)
- Avalonia 12.0.4 (transitive)

## Migration depuis 1.0.x

1. Conserver `<my:MyTheme />` dans `Application.Styles`.
2. Appeler `MyTheme.Current.EnsureLoaded()` au démarrage si ce n’est pas déjà fait.
3. Aucun changement de namespace public obligatoire.
4. Comportement des classes utilitaires : identique visuellement ; promotion lazy possible sur le premier frame après `Loaded` pour les contrôles avec classes `variant-*`, `size-*`, etc.
5. Optionnel : `BrushSetOptions.TransformedBrushCapacity = 64` si l’application génère beaucoup de combinaisons d’opacité uniques.

## Pack local

```bash
dotnet pack src/MyNet.Avalonia.Theme/MyNet.Avalonia.Theme.csproj -c Release -p:Version=1.1.0 -o packages
```

## Publication NuGet

```bash
dotnet nuget push packages/MyNet.Avalonia.Theme.1.1.0.nupkg -s https://api.nuget.org/v3/index.json -k <API_KEY>
dotnet nuget push packages/MyNet.Avalonia.Theme.1.1.0.snupkg -s https://api.nuget.org/v3/index.json -k <API_KEY>
```
