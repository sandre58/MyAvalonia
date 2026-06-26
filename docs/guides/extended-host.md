# Extended host (Avalonia adapters)

**Package:** [MyNet.Avalonia.Extended](../../src/MyNet.Avalonia.Extended/README.md)

Avalonia implementations of **MyNet.UI** host contracts: dialog presenters, toast overlay, navigation host, clipboard, scheduler, commands, and `IThemeService` → `MyTheme`.

Read MyNet guides for **contracts and view models**:

- [Dialogs](https://github.com/sandre58/MyNet/blob/main/docs/guides/dialogs.md)
- [Notifications & toasts](https://github.com/sandre58/MyNet/blob/main/docs/guides/notifications-and-toasts.md)
- [Navigation](https://github.com/sandre58/MyNet/blob/main/docs/guides/navigation.md)
- [Theming (IThemeService)](https://github.com/sandre58/MyNet/blob/main/docs/guides/theming.md)

## Architecture

```text
┌─────────────────────────────────────────┐
│  MyNet.UI (IContentDialogService,       │
│  INavigationClient, IToastManager, …)   │
└──────────────────┬──────────────────────┘
                   │ implemented by
┌──────────────────▼──────────────────────┐
│  MyNet.Avalonia.Extended                │
│  · OverlayDialogPresenter               │
│  · WindowDialogPresenter                │
│  · AvaloniaToastHost                    │
│  · Avalonia navigation bootstrap        │
│  · ThemeService → IThemeBrushService    │
└──────────────────┬──────────────────────┘
                   │ uses
┌──────────────────▼──────────────────────┐
│  MyNet.Avalonia.Controls                │
│  OverlayDialogHost, ContentDialog, …    │
└─────────────────────────────────────────┘
```

---

## Full registration

```csharp
using MyNet.Avalonia.Extended;
using MyNet.Avalonia.Theme;
using MyNet.UI;
using MyNet.UI.Theming;

services.AddUi(/* cultures, shell, toasting options */)
    .AddMyNetAvaloniaExtended(() => mainWindow)
    .AddSingleton<IThemeBrushService>(MyTheme.Current);

var provider = services.BuildServiceProvider();
provider.UseUi();
provider.UseMyNetAvaloniaClipboard();
provider.UseMyNetAvaloniaExtended();
```

`AddMyNetAvaloniaExtended` chains:

| Extension | Registers |
|-----------|-----------|
| `AddAvaloniaAppCommands` | `IAppCommandsService` |
| `AddAvaloniaScheduler` | Rx `IScheduler` / `ISchedulerProvider` |
| `AddAvaloniaCommands` | `ICommandFactory` |
| `AddAvaloniaTheming` | `IThemeService` |
| `AddAvaloniaClipboard` | `IClipboardService` |
| `AddAvaloniaToasting` | `AvaloniaToastHost`, notifications |
| `AddAvaloniaDialogs` | Overlay + window presenters |
| `AddAvaloniaNavigation` | Navigation host |

Pick individual `AddAvalonia*` methods if you do not need the full bundle.

### App.axaml styles

```xml
<StyleInclude Source="avares://MyNet.Avalonia.Extended/Themes/Generic.axaml" />
```

---

## Dialogs

**Prerequisites:** `AddUi` with dialogs + view locators, `AddAvaloniaDialogs`, `OverlayDialogHost` in shell XAML.

```csharp
using MyNet.Avalonia.Controls.Dialogs.Overlay;
using MyNet.Avalonia.Extended.Dialogs;

var result = await contentDialogService.ShowAsync(
    viewModel,
    DialogOptionsFactory.ForOverlay(
        viewModel,
        isModal: true,
        overlayOptions: new OverlayDialogOptions
        {
            CanLightDismiss = true,
            TopLevelKey = OverlayDialogHostManager.GetTopLevelKey(mainWindow)
        },
        hostId: OverlayDialogHostManager.MainHostId));
```

Window modal:

```csharp
await contentDialogService.ShowAsync(
    viewModel,
    DialogOptionsFactory.ForWindow(viewModel, isModal: true));
```

More detail: [`src/MyNet.Avalonia.Extended/Dialogs/README.md`](../../src/MyNet.Avalonia.Extended/Dialogs/README.md).

---

## Toast notifications

After `AddMyNetAvaloniaExtended` + `UseMyNetAvaloniaExtended()` (resolves `AvaloniaToastHost`):

```csharp
public class SaveViewModel(INotificationPublisher notifications)
{
    public async Task SaveAsync()
    {
        await repository.SaveAsync();
        notifications.PublishSuccess("Saved");
    }
}
```

Customize with `IAvaloniaToastContentContributor`. Showcase: `ShowcaseCustomNotificationToastContentContributor`.

Configure defaults via `AddUi(b => b.ConfigureToasting(...))` — see [MyNet notifications guide](https://github.com/sandre58/MyNet/blob/main/docs/guides/notifications-and-toasts.md).

---

## Theming

`ThemeService` drives `MyTheme` through `IThemeBrushService` — do not replace `ResourceDictionary` sources manually.

```csharp
themeService.ApplyTheme(UiTheme.Dark);
themeService.ApplyPrimary("#1756BD");
themeService.ApplyBaseTheme(ThemeVariantProvider.DarkBlue);
```

Register theme bases on `IThemeBaseRegistry` during startup (Showcase: `InitializeTheme` in `AppComposition`).

See [Theming (Avalonia)](theming.md).

---

## Clipboard

`IClipboardService` exposes `CopyTextAsync` / `CopyAsync`. After `UseMyNetAvaloniaClipboard()`, XAML and commands may use static `ClipboardManager`.

---

## Busy state

- **App-wide:** inject `IBusyService` from MyNet.UI (`AddUi`) — `RunAsync`, scope tokens.
- **Control:** `<extended:BusyServiceIndicator />` or local `controls:BusyIndicator`.

---

## Reference

- Showcase composition: [`AppComposition.cs`](../../demos/MyNet.Avalonia.Showcase/Composition/AppComposition.cs)
- Dialogs page: [`DialogPageViewModel.cs`](../../demos/MyNet.Avalonia.Showcase/ViewModels/Pages/DialogPageViewModel.cs)

[Controls & overlays](controls-and-overlays.md) · [Getting started](../getting-started.md)
