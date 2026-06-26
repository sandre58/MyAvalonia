# MyAvalonia documentation

MyAvalonia is the **Avalonia UI** layer of the MyNet suite: themed controls, markup extensions, and host adapters for **MyNet.UI** contracts (dialogs, navigation, toasts, theming).

**Packages:** six NuGet packages under `src/MyNet.Avalonia.*` · **Companion repo:** [MyNet](https://github.com/sandre58/MyNet) (MVVM, globalization, UI abstractions)

## Documentation map

```text
docs/
├── index.md                 ← you are here
├── getting-started.md         ★ Bootstrap a MyNet + Avalonia app
├── TODO.md
├── guides/
│   ├── README.md
│   ├── theming.md                 ★ MyTheme engine (how it works)
│   ├── theme-catalog-colors.md    ★ Reference — colors & brushes
│   ├── theme-catalog-tokens.md    ★ Reference — spacing, motion, …
│   ├── theme-catalog-utility-classes.md ★ Reference — CSS classes
│   ├── theme-controls.md          ★ Reference — control templates
│   ├── controls-and-overlays.md
│   ├── extended-host.md       ★ DI, dialogs, toasts, IThemeService
│   ├── markup-and-converters.md
│   └── geography-avalonia.md
└── releases/
```

## Start here

| Audience | Read first |
|----------|------------|
| New consumer | [Getting started](getting-started.md) → [Guides index](guides/README.md) |
| Theme / design tokens | [Theming engine](guides/theming.md) → [catalogs](guides/README.md#theme-system) |
| MVVM shell (dialogs, nav) | [Extended host](guides/extended-host.md) + [MyNet UI guides](https://github.com/sandre58/MyNet/tree/main/docs/guides) |
| NuGet package summary | README in each `src/MyNet.Avalonia.*/README.md` |
| Runnable reference | [MyNet.Avalonia.Showcase](../demos/MyNet.Avalonia.Showcase/) |

## Package index

| Package | README | Primary guide |
|---------|--------|---------------|
| MyNet.Avalonia | [README](../src/MyNet.Avalonia/README.md) | [Markup & converters](guides/markup-and-converters.md) |
| MyNet.Avalonia.Controls | [README](../src/MyNet.Avalonia.Controls/README.md) | [Controls & overlays](guides/controls-and-overlays.md) |
| MyNet.Avalonia.Theme | [README](../src/MyNet.Avalonia.Theme/README.md) | [Theming](guides/theming.md) |
| MyNet.Avalonia.Theme.Controls | [README](../src/MyNet.Avalonia.Theme.Controls/README.md) | [Theme controls](guides/theme-controls.md) |
| MyNet.Avalonia.Extended | [README](../src/MyNet.Avalonia.Extended/README.md) | [Extended host](guides/extended-host.md) |
| MyNet.Avalonia.Geography | [README](../src/MyNet.Avalonia.Geography/README.md) | [Geography (Avalonia)](guides/geography-avalonia.md) |

[Backlog](TODO.md) · [MyNet documentation](https://github.com/sandre58/MyNet/blob/main/docs/index.md)
