// -----------------------------------------------------------------------
// <copyright file="ThemeControlsCatalog.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Controls.Classes;

namespace MyNet.Avalonia.Theme.Controls;

/// <summary>
/// Precompiled control-theme catalog. Attach via <see cref="ThemeControlsHost.AttachCatalog"/> after <see cref="MyNet.Avalonia.Theme.MyTheme"/> has loaded.
/// </summary>
public sealed class ThemeControlsCatalog : Styles
{
    public ThemeControlsCatalog()
    {
        ClassesBootstrapper.Initialize();
        AvaloniaXamlLoader.Load(this);
    }
}
