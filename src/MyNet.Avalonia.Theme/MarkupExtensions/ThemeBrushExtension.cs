// -----------------------------------------------------------------------
// <copyright file="ThemeBrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using MyNet.Avalonia.Theme.Converters;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to theme brushes with optional opacity, contrast, darken, and lighten settings.
/// Allows XAML to reference theme brushes by path and apply opacity, contrast, or color transformations dynamically.
/// </summary>
public class ThemeBrushExtension(string path) : MarkupExtension
{
    /// <summary>
    /// Gets or sets the resource path for the theme brush.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = path;

    /// <summary>
    /// Gets or sets the named opacity value from the <see cref="Opacity"/> enum.
    /// </summary>
    public Opacity? Opacity { get; set; }

    /// <summary>
    /// Gets or sets a custom opacity value as a string (e.g., "0.5" or a resource key).
    /// </summary>
    public string? CustomOpacity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use the contrast brush for accessibility.
    /// </summary>
    public bool Contrast { get; set; }

    /// <summary>
    /// Gets or sets a value indicating how much to darken the brush color (0.0 to 1.0).
    /// </summary>
    public double? Darken { get; set; }

    /// <summary>
    /// Gets or sets a value indicating how much to lighten the brush color (0.0 to 1.0).
    /// </summary>
    public double? Lighten { get; set; }

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the theme brush with the specified options.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush with the specified opacity and contrast settings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var converter = new ThemeBrushConverter()
        {
            Opacity = Opacity?.ToString() ?? CustomOpacity,
            Contrast = Contrast,
            Darken = Darken,
            Lighten = Lighten
        };

        return new ReflectionBindingExtension(Path)
        {
            Converter = converter
        }.ProvideValue(serviceProvider);
    }
}
