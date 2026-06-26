# Theme controls catalog

**Package:** [MyNet.Avalonia.Theme.Controls](../../src/MyNet.Avalonia.Theme.Controls/README.md)

Precompiled Avalonia **Styles** catalog that themes MyNet and standard Avalonia controls. Loaded via `<my:ThemeControlsCatalog />` immediately after `<my:MyTheme />`.

Requires: **Theme**, **Controls**, **Avalonia** (see [Getting started](../getting-started.md)).

## What gets loaded

| Layer | Folder | Content |
|-------|--------|---------|
| Data templates | `Resources/` | Shared item templates |
| Foundation | `Foundation/` | Primitives: `Ripple`, `TextField`, chrome, layout |
| Standard | `Standard/` | Restyled built-in Avalonia controls |
| Custom | `Custom/` | Themes for `MyNet.Avalonia.Controls` |
| DataGrid | `DataGrid/` | Column types and grid chrome |

Entry point: `ThemeControlsCatalog.axaml` (compiled as `ThemeControlsCatalog` class).

## Setup

```xml
<Application xmlns:my="http://mynet.com/avalonia">
    <Application.Styles>
        <my:MyTheme />
        <my:ThemeControlsCatalog />
    </Application.Styles>
</Application>
```

```csharp
// App.axaml.cs — after AvaloniaXamlLoader.Load
MyTheme.Current.EnsureLoaded();
```

`ThemeControlsCatalog` constructor calls `ClassesBootstrapper.Initialize()` then loads precompiled XAML — registering utility classes and attaching hundreds of control themes in one pass.

## Optional companion styles

When using other MyAvalonia packages, merge their generic themes **after** the catalog:

```xml
<StyleInclude Source="avares://MyNet.Avalonia.Extended/Themes/Generic.axaml" />
<StyleInclude Source="avares://MyNet.Avalonia.Geography/Themes/Generic.axaml" />
```

## Why not StyleInclude per file?

Avalonia would parse ~100 XAML files on the UI thread during `AvaloniaXamlLoader.Load`, **before** `MyTheme` tokens and brushes are ready — blocking startup and breaking brush resolution.

The catalog is **precompiled** and ordered: Foundation → Standard → Custom.

## Project layout (source)

```text
src/MyNet.Avalonia.Theme.Controls/
├── Foundation/
├── Standard/
├── Custom/
├── DataGrid/
├── Resources/DataTemplates.axaml
└── ThemeControlsCatalog.axaml    ← single attach point
```

## Related

- [Theming](theming.md) — `MyTheme`, tokens, `{my:Theme*}`
- [Controls & overlays](controls-and-overlays.md) — controls styled by Custom layer
- [Showcase App.axaml](../../demos/MyNet.Avalonia.Showcase/App.axaml)

[Guides index](README.md)
