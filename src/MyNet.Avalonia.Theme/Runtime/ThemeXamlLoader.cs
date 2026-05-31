// -----------------------------------------------------------------------
// <copyright file="ThemeXamlLoader.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Default implementation that loads theme XAML via <see cref="AvaloniaXamlLoader"/>.
/// </summary>
internal sealed class ThemeXamlLoader : IThemeXamlLoader
{
    public void Load(IServiceProvider? serviceProvider, object themeRoot)
    {
        if (themeRoot is MyTheme theme)
            theme.LoadXamlCore(serviceProvider);
        else
            AvaloniaXamlLoader.Load(serviceProvider, themeRoot);
    }
}
