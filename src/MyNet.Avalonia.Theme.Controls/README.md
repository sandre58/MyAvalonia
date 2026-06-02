# MyNet.Avalonia.Theme.Controls

Control themes and DataGrid columns for the MyNet Avalonia design system.

## Reference

```bash
dotnet add package MyNet.Avalonia.Theme.Controls
```

Also requires `MyNet.Avalonia.Theme`, `MyNet.Avalonia.Controls`, and `MyNet.Avalonia`.

## Startup (required)

Add `<my:MyTheme />` in `Application.Styles`, then use `MyNetThemeBootstrap`:

```csharp
using MyNet.Avalonia.Theme.Controls;

public override void Initialize()
    => MyNetThemeBootstrap.Initialize(this);

public override void OnFrameworkInitializationCompleted()
{
    MyNetThemeBootstrap.LoadTheme(this);
    // show main window…
}
```

`Initialize` registers utility classes and loads application XAML. `LoadTheme` calls `MyTheme.Current.EnsureLoaded()` then attaches the precompiled catalog.

Do **not** use `<StyleInclude Source="avares://MyNet.Avalonia.Theme.Controls/..." />` in `App.axaml` for the full catalog: Avalonia would load ~100 XAML files on the UI thread during `AvaloniaXamlLoader.Load`, before tokens are ready, which blocks startup.

In DEBUG builds, `LoadTheme` verifies the catalog is attached when `attachCatalog` is `true`.

### Advanced (manual steps)

```csharp
ThemeControlsHost.Register();
AvaloniaXamlLoader.Load(this);
// …
MyTheme.Current.EnsureLoaded();
ThemeControlsHost.AttachCatalog(this);
```

## What gets loaded

1. `Resources/DataTemplates.axaml`
2. **Catalog** — Foundation → Standard → Custom

## Project layout

```
Foundation/
Standard/
Custom/
ThemeControlsCatalog.axaml   # precompiled entry point (AttachCatalog)
DataGrid/
```
