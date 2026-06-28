# Architecture Overview

## What this repo is

A **modular Avalonia UI framework** — 6 NuGet packages + Showcase demo. Not Clean Architecture / DDD.

## Package dependency graph

```
MyNet.Avalonia (core)
  ├── MyNet.Avalonia.Controls (C# only)
  ├── MyNet.Avalonia.Theme (tokens, MyTheme)
  │     └── MyNet.Avalonia.Theme.Controls (ControlTheme AXAML)
  │           └── MyNet.Avalonia.Extended (dialogs, nav, toast)
  └── MyNet.Avalonia.Geography (satellite)
```

## Critical split

| Package | Contains | Must NOT contain |
|---------|----------|------------------|
| Controls | `.cs` logic, StyledProperty, pseudo-classes | `.axaml`, business logic |
| Theme.Controls | ControlTheme, templates, Assists | Control behavior logic |
| Extended | MyNet.UI Avalonia adapters | Domain models |
| Showcase | MVVM demo, DI composition | — (not published) |

## What we do NOT build in `src/`

- Domain entities, repositories, application services
- ViewModels (except UI-model types like palettes)
- Persistence, networking, business rules

## External companion

MVVM shell, globalization, validation: **MyNet** packages (separate repo). See `10-mynet-companion.md`.

## Startup pattern (consumers)

```xml
<Application.Styles>
  <my:MyTheme />
  <my:ThemeControlsCatalog />
</Application.Styles>
```

Deep principles: `.cursor/reference/architecture.md`
