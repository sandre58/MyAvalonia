# Control Creation Guide

End-to-end checklist for a new MyNet control. Reference: `Card.cs` + `Custom/Card.axaml`.

## Step 1 — Design

- Single responsibility (see `04-control-design.mdc`)
- Choose base: `RegionControl`, `TemplatedControl`, `ContentControl`, built-in Avalonia base

## Step 2 — C# (`src/MyNet.Avalonia.Controls/`)

```
Controls/{Domain}/{Name}.cs
```

- `StyledProperty` for all bindable UI state
- `[PseudoClasses(...)]` + handlers using `PseudoClassName` constants
- `[TemplatePart("PART_*", typeof(...))]` for each part
- Optional: `{Name}.Keyboard.cs` partial for focus/keyboard
- **Zero `.axaml` in this project**

## Step 3 — ControlTheme (`src/MyNet.Avalonia.Theme.Controls/Custom/`)

```
Custom/{Name}.axaml
```

```xml
<ControlTheme x:Key="{x:Type my:Name}" TargetType="my:Name">
```

- Use `MyNet.*` resource keys and `{my:ThemeContext ...}` markup
- Named variants: `MyNet.Theme.{Name}.{Variant}` with `BasedOn`

## Step 4 — Register theme

Add `<ResourceInclude>` in `Custom/_index.axaml`.

## Step 5 — DI (if translations needed)

`AddMyNetAvaloniaControls()` registers control `.resx` — not themes.

## Step 6 — Showcase demo

- `{Name}PageViewModel` + `{Name}Page.axaml`
- Entry in `Composition/PagesCatalog.cs`
- Register VM in `AppComposition`

## Step 7 — Tests

- Unit: `tests/MyNet.Avalonia.Controls.Tests/`
- Headless template: `tests/MyNet.Avalonia.Controls.Headless.Tests/`

## XAML namespace

`xmlns:my="http://mynet.com/avalonia"`
