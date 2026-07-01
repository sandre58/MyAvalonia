# Control Creation Guide

End-to-end checklist for a new MyNet control. Reference: `Card.cs` + `Custom/Card.axaml`.

## Four pillars (mandatory review)

Every new control must address all four before merge. Do not ship API + theme only.

### Design

- Single responsibility and base type choice (see `04-control-design.mdc`)
- ControlTheme with `MyNet.*` tokens, pseudo-classes, named variants
- Visual states: normal, hover, pressed, disabled, focused

### Keyboard

- Define focus model: `Focusable`, `IsTabStop`, tab order inside composite controls
- Handle expected keys (Enter, Esc, arrows, Space, F4 for popups, etc.)
- On open/close: where focus lands and returns
- Extract to `{Name}.Keyboard.cs` partial when logic is non-trivial (`TextPicker.Keyboard.cs`, `DateTimePickerEx.Keyboard.cs`)
- Pickers: follow `pickers.md` and `picker-interaction-contract.md`

### Mouse

- Pointer interactions: `OnPointerPressed` / `Moved` / `Released` or routed handlers
- Hover and pressed pseudo-classes; cursor when interaction is implied
- Popups: light-dismiss, click-outside, toggle zones
- Adequate hit targets for interactive parts

### Automation

- Static ctor: `AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<T>(...)`
- Update `AutomationProperties.SetName` when value or context changes (`Clock.cs`, `TextPicker.cs`)
- Name must be meaningful for screen readers and headless/UI tests — no hardcoded user-facing strings

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
- Four pillars in C#: keyboard (`{Name}.Keyboard.cs` when non-trivial), pointer handlers, `AutomationProperties` in static ctor
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
- Design pillar: style all interactive states (hover, pressed, disabled, focused)

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
