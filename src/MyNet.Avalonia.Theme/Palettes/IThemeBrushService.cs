// -----------------------------------------------------------------------
// <copyright file="IThemeBrushService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Defines an interface for a theme brush service that provides methods to retrieve brushes from theme resources based on various parameters such as resource path, opacity, contrast, darkening, and lightening factors. This service abstracts the retrieval and transformation of brushes, allowing for flexible theming scenarios and hot theme switching without the need for resource reloading. Implementations of this interface can utilize a brush management system to efficiently manage and transform brushes according to the specified parameters, ensuring a cohesive theming experience across the application.
/// </summary>
/// <remarks>
/// The service supports retrieving brushes by both resource path and brush instance, allowing for versatile usage in different theming contexts. The opacity resolver function enables dynamic resolution of opacity values, further enhancing the flexibility of the theming system.
/// </remarks>
public interface IThemeBrushService
{
    /// <summary>
    /// Gets a brush from the theme resources by path.
    /// </summary>
    /// <param name="path">The resource path for the brush.</param>
    /// <param name="opacityKey">Optional opacity key or value.</param>
    /// <param name="contrast">If true, returns the contrast brush for accessibility.</param>
    /// <param name="darken">Optional darken factor (value between 0.0 and 1.0).</param>
    /// <param name="lighten">Optional lighten factor (value between 0.0 and 1.0).</param>
    /// <returns>The brush instance.</returns>
    IBrush GetBrush(string path, string? opacityKey = null, bool contrast = false, double? darken = null, double? lighten = null);

    /// <summary>
    /// Gets a brush from the theme resources by brush instance, optionally with a specific opacity or contrast.
    /// </summary>
    /// <param name="brush">The brush instance to search for.</param>
    /// <param name="opacityKey">Optional opacity key or value.</param>
    /// <param name="contrast">If true, returns the contrast brush for accessibility.</param>
    /// <param name="darken">Optional darken factor (value between 0.0 and 1.0).</param>
    /// <param name="lighten">Optional lighten factor (value between 0.0 and 1.0).</param>
    /// <returns>The brush instance with the specified opacity or contrast.</returns>
    IBrush GetBrush(IBrush brush, string? opacityKey = null, bool contrast = false, double? darken = null, double? lighten = null);

    /// <summary>
    /// Gets the opacity value for a given opacity key, which can be a direct value or a reference to a resource. This method allows for dynamic resolution of opacity values based on theme parameters, enabling flexible theming scenarios where opacity can be defined in the theme resources or passed directly as a value.
    /// </summary>
    /// <param name="opacityKey">The opacity key to resolve.</param>
    /// <returns>The resolved opacity value, or null if not found.</returns>
    double? GetOpacity(string? opacityKey);

    ThemeVariantColors? GetThemeVariantColors();

    ColorShades GetPrimary();

    ColorShades GetAccent();

    void SetPrimary(string color, string? foreground);

    void SetAccent(string color, string? foreground);
}
