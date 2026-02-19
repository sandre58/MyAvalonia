// -----------------------------------------------------------------------
// <copyright file="ThemeExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Metadata;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to a theme brush by resource path.
/// Allows XAML to reference theme brushes directly using a simple path string.
/// </summary>
/// <example>
/// <code>
/// <Border Background="{theme Path=Primary}" />
/// </code>
/// </example>
public class ThemeExtension(string path) : ThemeBrushExtensionBase
{
    /// <summary>
    /// Gets or sets the resource path for the theme brush.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = path;

    /// <summary>
    /// Provides the value for the markup extension, returning the theme brush for the specified path.
    /// If opacity or contrast is specified, returns a transformed brush; otherwise, returns a dynamic resource reference.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>The theme brush instance for the specified path.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var hasTransform = Opacity.HasValue || !string.IsNullOrWhiteSpace(CustomOpacity) || Contrast || Darken.HasValue || Lighten.HasValue;
        return hasTransform
            ? MyTheme.Current.GetBrush(Path, Opacity?.ToString() ?? CustomOpacity, Contrast, Darken, Lighten)
            : new DynamicResourceExtension(ThemeResourceKeyFactory.Brush(Path)).ProvideValue(serviceProvider);
    }
}
