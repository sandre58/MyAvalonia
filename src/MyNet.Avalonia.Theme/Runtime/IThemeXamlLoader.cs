// -----------------------------------------------------------------------
// <copyright file="IThemeXamlLoader.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Loads compiled theme XAML into a <see cref="MyTheme"/> instance.
/// </summary>
internal interface IThemeXamlLoader
{
    void Load(IServiceProvider? serviceProvider, object themeRoot);
}
