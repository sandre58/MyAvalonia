# MyNet.Avalonia.Theme.Controls

Control themes and DataGrid columns for the MyNet Avalonia design system.

## Reference

```bash
dotnet add package MyNet.Avalonia.Theme.Controls
```

Also requires `MyNet.Avalonia.Theme`, `MyNet.Avalonia.Controls`, and `MyNet.Avalonia`.

## Startup (required)

**1. Register utility classes** before loading application XAML:

```csharp
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Controls;

public override void Initialize()
{
    ThemeControlsHost.Register();
    AvaloniaXamlLoader.Load(this);
}
```

**2. Load `MyTheme`, then attach the catalog** (after the splash screen or before showing the main window):

```csharp
MyTheme.Current.EnsureLoaded();
ThemeControlsHost.AttachCatalog(this);
```

Do **not** use `<StyleInclude Source="avares://MyNet.Avalonia.Theme.Controls/..." />` in `App.axaml` for the full catalog: Avalonia would load ~100 XAML files on the UI thread during `AvaloniaXamlLoader.Load`, before tokens are ready, which blocks startup.

`AttachCatalog` uses a precompiled `ThemeControlsCatalog` (`x:Class`) and must run **after** `EnsureLoaded()`.

Keep `<my:MyTheme />` in `Application.Styles` as usual.

## What gets loaded

1. `Resources/DataTemplates.axaml`
2. **Catalog** — Foundation → Standard → Custom

## Project layout

```
Foundation/
Standard/
Custom/
Catalog/
ThemeControlsCatalog.axaml   # precompiled entry point (AttachCatalog)
DataGrid/
```
