# System guides (MyAvalonia)

Topic-oriented guides for Avalonia-specific subsystems. Each guide links to package READMEs under `src/` and, where relevant, to **MyNet** guides for shared MVVM contracts.

## General

| Guide | Packages | Summary |
|-------|----------|---------|
| [Getting started](../getting-started.md) | All | Package stack, `App.axaml`, DI bootstrap |
| [Controls & overlays](controls-and-overlays.md) | Controls | Control catalog, `OverlayDialogHost` |
| [Extended host](extended-host.md) | Extended | Dialogs, toasts, navigation, `IThemeService` adapters |
| [Markup & converters](markup-and-converters.md) | Avalonia | `{my:Loc}`, `{my:Display}`, value converters |
| [Geography (Avalonia)](geography-avalonia.md) | Geography | `{geo:Countries}`, templates, `CulturePicker` |

## Theme system

Split intentionally: **engine** vs **catalogs** (reference).

| Guide | Type | Summary |
|-------|------|---------|
| [Theming — engine](theming.md) | How it works | `MyTheme` architecture, setup, lifecycle, API, MVVM, perf |
| [Catalog — colors & brushes](theme-catalog-colors.md) | Reference | Variants, semantic colors, `ColorShades`, roles, markup |
| [Catalog — design tokens](theme-catalog-tokens.md) | Reference | Spacing, corners, sizes, typography, shadows, motion |
| [Catalog — utility classes](theme-catalog-utility-classes.md) | Reference | `variant-*`, `size-*`, `p-*`, `shadow-*`, … |
| [Theme controls](theme-controls.md) | Reference | `ThemeControlsCatalog`, Foundation / Standard / Custom |

**Read order:** [engine](theming.md) → pick catalogs as needed → [theme-controls](theme-controls.md) for styled controls.

## MyNet guides (shared contracts)

MyAvalonia implements Avalonia presenters for **MyNet.UI**. Read these in [MyNet/docs/guides](https://github.com/sandre58/MyNet/tree/main/docs/guides):

| MyNet guide | Used with MyAvalonia |
|-------------|----------------------|
| [UI](https://github.com/sandre58/MyNet/blob/main/docs/guides/ui.md) | Locators, shell overview |
| [Navigation](https://github.com/sandre58/MyNet/blob/main/docs/guides/navigation.md) | `INavigationClient` + `AddAvaloniaNavigation` |
| [Dialogs](https://github.com/sandre58/MyNet/blob/main/docs/guides/dialogs.md) | `IContentDialogService` + overlay/window presenters |
| [Notifications & toasts](https://github.com/sandre58/MyNet/blob/main/docs/guides/notifications-and-toasts.md) | `INotificationPublisher` + `AvaloniaToastHost` |
| [Theming (contracts)](https://github.com/sandre58/MyNet/blob/main/docs/guides/theming.md) | `IThemeService` — implemented by Extended + `MyTheme` |
| [Globalization](https://github.com/sandre58/MyNet/blob/main/docs/guides/globalization.md) | Required for `{my:Loc}` / `{my:Display}` |
| [Geography](https://github.com/sandre58/MyNet/blob/main/docs/guides/geography.md) | Core country model behind Geography Avalonia |

All guides are in **English**.

[Documentation index](../index.md) · [Showcase demo](../../demos/MyNet.Avalonia.Showcase/)
