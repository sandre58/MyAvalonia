// -----------------------------------------------------------------------
// <copyright file="ThemeResourceProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;

namespace MyNet.Avalonia.Theme;

/// <summary>
/// Provides methods to retrieve themed resources such as brushes and opacity values using resource keys.
/// </summary>
public static class ThemeResourceProvider
{
    /// <summary>
    /// Gets a brush resource by its name from the theme resource dictionary.
    /// </summary>
    /// <param name="name">The name of the brush resource.</param>
    /// <returns>The brush resource if found; otherwise, null or a default value.</returns>
    public static IBrush GetBrush(string name) => ResourceLocator.GetResource<IBrush>(ThemeResourceKeyFactory.Brush(name));

    /// <summary>
    /// Gets an opacity value by its name from the theme resource dictionary.
    /// </summary>
    /// <param name="name">The name of the opacity resource.</param>
    /// <returns>The opacity value if found; otherwise, 0 or a default value.</returns>
    public static double GetOpacity(string name) => ResourceLocator.GetResource<double>(ThemeResourceKeyFactory.Opacity(name));
}
