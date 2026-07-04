# Controls & overlay dialogs

**Package:** [MyNet.Avalonia.Controls](../../src/MyNet.Avalonia.Controls/README.md)

Custom Avalonia controls, attached behaviors, and **overlay dialog hosting** (usable with or without MyNet.UI).

Styled appearance requires [Theme controls](theme-controls.md). Registration: `AddMyNetAvaloniaControls()` for color-picker and message `.resx` resources.

## XML namespace

```xml
xmlns:controls="http://mynet.com/avalonia/controls"
```

## Control catalog

| Area | Notable types |
|------|----------------|
| Layout & chrome | `Card`, `Divider`, `ElasticWrapPanel`, `OverflowStackPanel`, `TitleBlock`, `EmptyState`, `Banner` |
| Forms | `Form`, `FormGroup`, `FormItem`, `TagBox`, `MultiComboBox` |
| Date & time | `Calendar`, `CalendarDatePickerEx`, `TimePickerEx`, `Clock`, `TimeView` |
| Color | `ColorPickerEx`, `ColorEyeDropper`, `StandardColorPalette`, `DarkColorPalette`, `LightColorPalette` |
| Data & nav | `Pagination`, `CodeBlock`, `NavigationMenu`, `NavigationMenuItem` |
| Feedback | `Avatar`, `Badge`, `Loader`, `BusyIndicator`, `Ripple` |
| Dialogs | `ContentDialog`, `OverlayDialog`, `OverlayDialogHost` |
| Icons | `ExtendedIcon`, `MaterialIcon` |
| Placeholder swap | `PlaceholderContentControl` — content or watermark placeholder (`variant-watermark`, optional `IconAssist` + `PlaceholderText`) |
| Behaviors | `ItemsBehavior`, `FocusBehavior`, `PopupBehavior`, `DataGridBehavior`, … |

### Empty state vs placeholder swap

| Scenario | Control | Notes |
|----------|---------|--------|
| Page / list / panel with **no alternate content** | `EmptyState` | Title, subtitle, optional actions; do not nest inside `PlaceholderContentControl` by default |
| **Same slot** shows real content or empty hint (picker pane, search popup) | `PlaceholderContentControl` | `variant-watermark` + optional icon (`MaterialIconKind`); popup search adds `PlaceholderAssist` |
| Compact watermark | `PlaceholderContentControl` | `variant-watermark` without icon — italic caption text only |


Live demos: [Showcase](../../demos/MyNet.Avalonia.Showcase/) pages (Calendar, ColorPicker, Navigation, Dialogs, …).

## Examples

### Avatar

```xml
<controls:Avatar Source="/Assets/avatar.png" Width="64" Height="64" Initials="JD" />
```

### Color picker

```xml
<controls:ColorPickerEx SelectedColor="{Binding Accent, Mode=TwoWay}" />
```

### Navigation menu

```xml
<controls:NavigationMenu ItemsSource="{Binding MenuItems}" />
```

### Custom styling

Target the `controls` xmlns:

```xml
<Style Selector="controls|Avatar">
  <Setter Property="BorderThickness" Value="2" />
</Style>
```

---

## Overlay dialog host

`OverlayDialogHost` + `OverlayDialogHostManager` provide in-window modal/non-modal overlays **without** requiring MyNet.UI (Extended presenters use the same host).

### 1. Declare a host on the shell

```xml
<controls:OverlayDialogHost HostId="{x:Static controls:OverlayDialogHostManager.MainHostId}" />
```

Use `MainHostId` for the primary window host, or a custom `HostId` for secondary surfaces.

### 2. Top-level registration

Hosts register with `OverlayDialogHostManager` when attached to the visual tree. Presenters resolve `(hostId, topLevelKey)` before showing content.

Extended's `OverlayDialogPresenter` returns `CanPresent == false` until the host exists — declare the host in XAML **before** showing production overlay dialogs.

### 3. With MyNet.UI (Extended)

See [Extended host](extended-host.md) — `DialogOptionsFactory.ForOverlay(..., hostId: OverlayDialogHostManager.MainHostId)`.

Showcase: [`MainWindow.axaml`](../../demos/MyNet.Avalonia.Showcase/Views/MainWindow.axaml), Dialogs page.

---

## Testing

| Project | Scope |
|---------|-------|
| `tests/MyNet.Avalonia.Controls.Tests` | Pure logic (calendar, pagination, validation) |
| `tests/MyNet.Avalonia.Controls.Headless.Tests` | `[AvaloniaFact]` template/layout (Calendar, Badge, OverlayDialog, …) |

```bash
dotnet test tests/MyNet.Avalonia.Controls.Tests
dotnet test tests/MyNet.Avalonia.Controls.Headless.Tests
```

[Extended host](extended-host.md) · [Theming](theming.md) · [Package README](../../src/MyNet.Avalonia.Controls/README.md)
