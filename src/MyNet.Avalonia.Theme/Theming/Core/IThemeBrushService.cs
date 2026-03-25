// -----------------------------------------------------------------------
// <copyright file="IThemeBrushService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using MyNet.Avalonia.Theme.Theming.Palettes;

namespace MyNet.Avalonia.Theme.Theming.Core;

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

    /// <summary>
    /// Gets the name of the theme currently applied to the application.
    /// </summary>
    /// <remarks>If no theme has been set or applied, this method returns null. The returned value can be used
    /// to determine the active visual style or to conditionally apply theme-specific logic.</remarks>
    /// <returns>A string containing the name of the current theme, or null if no theme is applied.</returns>
    string? GetTheme();

    /// <summary>
    /// Gets the theme variant colors associated with the current theme.
    /// </summary>
    /// <remarks>This method retrieves the color settings that define the appearance of the theme. If the
    /// current theme does not have associated variant colors, the method will return null.</remarks>
    /// <returns>A ThemeVariantPalette object containing the colors for the current theme variant, or null if no variant colors
    /// are available.</returns>
    ThemeVariantPalette? GetThemePalette();

    /// <summary>
    /// Gets the primary color shade currently configured for the application.
    /// </summary>
    /// <remarks>This method retrieves the primary color shade that is in use. If a primary color has not been
    /// defined, the method returns <see langword="null"/>.</remarks>
    /// <returns>A nullable <see cref="ColorShades"/> value representing the primary color shade, or <see langword="null"/> if no
    /// primary color is set.</returns>
    ColorShades? GetPrimary();

    /// <summary>
    /// Gets the accent color used in the application, if one is set.
    /// </summary>
    /// <returns>A nullable <see cref="ColorShades"/> representing the accent color, or <see langword="null"/> if no accent color
    /// is set.</returns>
    ColorShades? GetAccent();

    /// <summary>
    /// Sets the application's theme to the specified value.
    /// </summary>
    /// <remarks>Changing the theme may affect the appearance of the user interface elements. Ensure that the
    /// specified theme is supported by the application.</remarks>
    /// <param name="theme">The name of the theme to apply. This value must correspond to a valid theme identifier.</param>
    void SetTheme(string theme);

    /// <summary>
    /// Sets the primary color and optionally the foreground color for the UI element.
    /// </summary>
    /// <remarks>This method updates the visual appearance of the UI element based on the specified colors.
    /// Ensure that the provided colors are valid and contrast well for accessibility.</remarks>
    /// <param name="color">The primary color to apply to the UI element. This color determines the main visual appearance.</param>
    /// <param name="foreground">An optional foreground color to use for the UI element. If null, the default foreground color is applied.</param>
    void SetPrimary(Color color, Color? foreground);

    /// <summary>
    /// Sets the accent color for user interface elements and optionally specifies a foreground color for text or icons.
    /// </summary>
    /// <remarks>This method updates the application's visual theme. Ensure that the provided colors are
    /// suitable for visibility and accessibility within the user interface.</remarks>
    /// <param name="color">The accent color to apply to the application's user interface elements. The color should be within the valid
    /// range for UI display.</param>
    /// <param name="foreground">An optional foreground color used for text or icons. If <paramref name="foreground"/> is <see langword="null"/>,
    /// a default foreground color will be used.</param>
    void SetAccent(Color color, Color? foreground);

    /// <summary>
    /// Sets the application's theme along with optional primary and accent colors and their respective foreground colors.
    /// </summary>
    /// <param name="theme">The name of the theme to apply. This value must correspond to a valid theme identifier.</param>
    /// <param name="primary">An optional primary color to apply to the theme.</param>
    /// <param name="accent">An optional accent color to apply to the theme.</param>
    /// <param name="primaryForeground">An optional foreground color for the primary color.</param>
    /// <param name="accentForeground">An optional foreground color for the accent color.</param>
    void SetTheme(string theme, Color primary, Color accent, Color? primaryForeground = null, Color? accentForeground = null);
}
