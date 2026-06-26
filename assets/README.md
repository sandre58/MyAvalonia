# NuGet package icons

128×128 PNG files packed via `build/package.props` when `assets/$(PackageIcon)` exists.

Icons are rasterized by the **shared** [`MyNet.Tools.PackageIconGenerator`](../../MyNet/tools/MyNet.Tools.PackageIconGenerator/) using SVG sources from [`MyNet/tools/icon-svgs`](../../MyNet/tools/icon-svgs/) and the local manifest [`tools/package-icons.json`](../tools/package-icons.json).

Design rules and the full registry: [`MyNet/tools/icon-registry.md`](../../MyNet/tools/icon-registry.md).

## Regenerate

Requires `MyNet` cloned as a sibling of `MyAvalonia` (`../MyNet`).

```powershell
powershell -File tools/generate-package-icons.ps1
```

## Package icons

| File | Package | Label |
|------|---------|-------|
| `MyAvalonia.png` | MyNet.Avalonia | AV |
| `MyAvaloniaControls.png` | MyNet.Avalonia.Controls | AC |
| `MyAvaloniaExtended.png` | MyNet.Avalonia.Extended | AE |
| `MyAvaloniaGeography.png` | MyNet.Avalonia.Geography | AG |
| `MyAvaloniaTheme.png` | MyNet.Avalonia.Theme | AT |
| `MyAvaloniaThemeControls.png` | MyNet.Avalonia.Theme.Controls | TC |
