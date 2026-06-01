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
                          HostId="{x:Static my:OverlayDialogHostManager.MainHostId}"
                          IsTopLevel="True"
                          IsModalStatusReporter="True"
                          HorizontalAlignment="Stretch"
                          VerticalAlignment="Stretch" />
</Grid>
```

Registering happens automatically when the host attaches to the visual tree (`HostId` + top-level hash).

Resolve the host from code:

```csharp
var topLevelKey = OverlayDialogHostManager.GetTopLevelKey(TopLevel.GetTopLevel(window));
var host = OverlayDialogHostManager.GetHost("main", topLevelKey);
```

Pass the same `HostId` and `TopLevelKey` (stable key from `GetTopLevelKey`) through MyNet.UI dialog options when using `MyNet.Avalonia.Extended`.

## Automatic top-level host

When `GetHost(id, hash)` finds no registered host:

1. **If `id` is not null** — returns `null` (no auto-creation). You must declare a host with that `HostId`.
2. **If `id` is null** — may create an `OverlayDialogHost` on the target `Window`:
   - If `window.Content` is a `Panel`, the host is added as a child.
   - Otherwise `window.Content` is wrapped in a new `Grid` (original content + host). This changes the root visual type; prefer declaring a host in XAML when you control the window template.

Target window selection:

- When `topLevelKey` is set: first open window whose `GetTopLevelKey(window)` matches.
- Otherwise: `MainWindow`, or the last window in the desktop lifetime.

## Title and chrome

Set `OverlayDialog.Title` for the header text. `IsCloseButtonVisible` toggles the template close button.

`CanResize` is reserved for a future release and currently has no effect.

## Host lookup rules

| `id` | `topLevelKey` | Behaviour |
|------|---------------|-----------|
| set | set | Exact key `(id, topLevelKey)` |
| set | null | All hosts with matching `HostId` (error if ambiguous) |
| null | set | All hosts with matching top-level key |
| null | null | All hosts; if exactly one `IsTopLevel` host exists, use it; else try auto-creation |

`TopLevelKey` in Extended `OverlayDialogOptions` must be `OverlayDialogHostManager.GetTopLevelKey(topLevel)` for the target window (not `GetHashCode()`).

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

## Layer order

Use explicit methods on `OverlayDialog`:

- `BringForward()` / `SendBackward()`
- `BringToFront()` / `SendToBack()`

The host listens to `LayerChanged` and updates Z-index.

## Recalling dialog content

`OverlayDialogHost.Recall<T>()` walks the stack from top to bottom and returns the first `Content` assignable to `T` (including derived types).

## Showcase integration

The Avalonia showcase registers `HostId="{x:Static my:OverlayDialogHostManager.MainHostId}"` on `MainWindow` and passes the same id to `DialogOptions.ForOverlay(..., hostId: OverlayDialogHostManager.MainHostId)`.
