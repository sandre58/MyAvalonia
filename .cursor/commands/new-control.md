# New Avalonia Control

## Context

Target package: [Controls / Theme.Controls / Extended]
Control: [Name] — single responsibility: [description]

## Required references

@.cursor/docs/02-control-creation-guide.md
@.cursor/rules/04-control-design.mdc
Similar existing control: @[Path/Example.cs]

## Expected deliverables

1. C# class in `src/MyNet.Avalonia.Controls/[Domain]/`
2. ControlTheme in `src/MyNet.Avalonia.Theme.Controls/Custom/`
3. Entry in `_index.axaml`
4. Showcase page (ViewModel + Page + PagesCatalog)
5. Minimal headless test
6. No hardcoded strings — localization if UI text is shown

## Four pillars (verify before done)

- [ ] **Design** — single responsibility, ControlTheme, pseudo-classes, token-based states
- [ ] **Keyboard** — focus model, tab order, shortcuts; `{Name}.Keyboard.cs` if needed
- [ ] **Mouse** — pointer handlers, hover/pressed, popup dismiss if applicable
- [ ] **Automation** — `AutomationProperties` control type + dynamic name

## Constraints

- Zero `.axaml` in Controls
- StyledProperty + documented pseudo-classes
- Public API reviewed for NuGet consumers
