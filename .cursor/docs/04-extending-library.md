# Extending the Library

## When to add to existing package vs new package

| Scenario | Action |
|----------|--------|
| New control + theme | Controls + Theme.Controls (always both) |
| Dialog/nav/toast adapter | `MyNet.Avalonia.Extended` |
| Geography UI | `MyNet.Avalonia.Geography` |
| New cross-cutting concern | Evaluate if it belongs in core `MyNet.Avalonia` |

## Extended DI chain

Entry: `AddMyNetAvaloniaExtended(topLevelProvider)` in `Extended/Extensions/ServiceCollectionExtensions.cs`

```
AddAvaloniaAppCommands()
  → AddAvaloniaScheduler()
  → AddAvaloniaCommands()      // ICommandFactory → AvaloniaCommandFactory
  → AddAvaloniaTheming()
  → AddAvaloniaClipboard()
  → AddAvaloniaToasting()
  → AddAvaloniaDialogs()
  → AddAvaloniaNavigation()
```

Each feature has its own `Extensions/ServiceCollectionExtensions.cs`.

## Pattern for new Extended feature

1. Interface in **MyNet.UI** (external) if platform-agnostic
2. Avalonia implementation in `Extended/{Feature}/`
3. `ServiceCollectionExtensions` for registration
4. Theme in `Extended/Themes/` if visual
5. Showcase demo page
6. Tests in `Extended.Tests` or `Extended.Headless.Tests`

## Controls package DI

`AddMyNetAvaloniaControls()` — translation resources only. No service locator in controls.

## Do not

- Create abstractions when Avalonia or MyNet.UI already provides the mechanism
- Add demo-only dependencies to `src/` projects

Human guide: `docs/guides/extended-host.md`
