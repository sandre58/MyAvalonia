// -----------------------------------------------------------------------
// <copyright file="ThemeControlsCatalog.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace MyNet.Avalonia.Theme.Controls;

/// <summary>
/// Precompiled control-theme catalog. Attach via <see cref="ThemeControlsHost.AttachCatalog"/> after <see cref="MyNet.Avalonia.Theme.MyTheme"/> has loaded.
/// </summary>
internal partial class ThemeControlsCatalog : Styles
{
    public ThemeControlsCatalog() => AvaloniaXamlLoader.Load(this);
}
