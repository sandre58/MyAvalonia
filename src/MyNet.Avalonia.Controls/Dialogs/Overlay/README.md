# Overlay dialogs (`OverlayDialogHost`)

Infrastructure for modal and non-modal dialogs rendered above existing UI inside a `Window` or any visual tree that hosts an `OverlayDialogHost`.

## Components

| Type | Role |
|------|------|
| `OverlayDialogHost` | Canvas that stacks masks and `OverlayDialog` instances |
| `OverlayDialog` | Draggable dialog chrome (title area, close button, content) |
| `OverlayFeedbackElement` | Base type: `Close()`, `Closed` event, `ShowAsync<T>()` |
| `OverlayDialogHostManager` | Global registry and host resolution |

## Declaring a host (recommended for production)

Place a host in your window or page so layout and modal scope are explicit:

```xml
<Grid>
    <views:MainView />
    <my:OverlayDialogHost x:Name="MainDialogHost"
                          HostId="main"
                          IsTopLevel="True"
                          IsModalStatusReporter="True"
                          HorizontalAlignment="Stretch"
                          VerticalAlignment="Stretch" />
</Grid>
```

Registering happens automatically when the host attaches to the visual tree (`HostId` + top-level hash).

Resolve the host from code:

```csharp
var hash = TopLevel.GetTopLevel(window)?.GetHashCode();
var host = OverlayDialogHostManager.GetHost("main", hash);
```

Pass the same `HostId` and `TopLevelHashCode` through MyNet.UI dialog options when using `MyNet.Avalonia.Extended`.

## Automatic top-level host

When `GetHost(id, hash)` finds no registered host:

1. **If `id` is not null** — returns `null` (no auto-creation). You must declare a host with that `HostId`.
2. **If `id` is null** — may create an `OverlayDialogHost` on the target `Window`:
   - If `window.Content` is a `Panel`, the host is added as a child.
   - Otherwise `window.Content` is wrapped in a new `Grid` (original content + host). This changes the root visual type; prefer declaring a host in XAML when you control the window template.

Target window selection:

- When `hash` is set: first open window with `window.GetHashCode() == hash`.
- Otherwise: `MainWindow`, or the last window in the desktop lifetime.

## Host lookup rules

| `id` | `hash` | Behaviour |
|------|--------|-----------|
| set | set | Exact key `(id, hash)` |
| set | null | All hosts with matching `HostId` (error if ambiguous) |
| null | set | All hosts with matching hash |
| null | null | All hosts; if exactly one `IsTopLevel` host exists, use it; else try auto-creation |

`TopLevelHashCode` in Extended options should match `TopLevel.GetTopLevel(host)?.GetHashCode()` for the window that owns the host.

## Modal scope

Use attached properties to propagate “app is modal” to chrome outside the host:

```xml
<Window my:OverlayDialogHost.IsModalStatusScope="True">
    <!-- title bar can bind to IsInModalStatus on the scope element -->
</Window>
```

Set `IsModalStatusReporter="True"` on the host that should drive the scope.

## Closing dialogs

Always close via `Close()` on the dialog (or `OnElementClosing` in subclasses). The host listens for `Closed` and removes layers.

`OverlayDialog.Close()` dismisses with a `null` result. Message boxes and Extended content dialogs override `Close()` to return typed results.
