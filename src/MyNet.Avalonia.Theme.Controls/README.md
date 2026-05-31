# MyNet.Avalonia.Theme.Controls

Control themes and DataGrid columns for the MyNet Avalonia design system.

## Reference

```bash
dotnet add package MyNet.Avalonia.Theme.Controls
```

Also requires `MyNet.Avalonia.Theme`, `MyNet.Avalonia.Controls`, and `MyNet.Avalonia`.

## Startup (required)

Register control themes **once**, before `MyTheme` is loaded from XAML:

```csharp
using MyNet.Avalonia.Theme.Controls;

public override void Initialize()
{
    ThemeControlsHost.Register();
    AvaloniaXamlLoader.Load(this);
}
```

Then keep `<my:MyTheme />` in `Application.Styles` as usual.

## What gets merged

1. `Resources/DataTemplates.axaml`
2. **Catalog** — Foundation → Standard → Custom → color picker, DataGrid, extended date/time families

## Project layout

```
Foundation/   # Ripple, TextField, IconContentControl, …
Standard/     # Button, TextBox, … (Avalonia types, MyNet templates)
Custom/       # Badge, Form, NavigationMenu, …
Modules/      # grouped includes from Catalog/_index.axaml
Catalog/
DataGrid/     # themed columns (C#)
```
