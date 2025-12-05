// -----------------------------------------------------------------------
// <copyright file="PaletteExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Converters;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to palette-based theme brushes with optional modifiers.
/// Resolves the brush based on the control's palette color type (Primary, Secondary, Tertiary) and applies optional effects such as opacity, contrast, darken, and lighten.
/// </summary>
public class PaletteExtension : MarkupExtension
{
    /// <summary>
    /// Gets or sets the palette color type to use (Primary, Secondary, Tertiary). If not set, uses the control's attached property.
    /// </summary>
    [ConstructorArgument("type")]
    public ColorType Type { get; set; }

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
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    public RelativeSource RelativeSource { get; set; } = new RelativeSource(RelativeSourceMode.Self);

    /// <summary>
    /// Initializes a new instance of the <see cref="PaletteExtension"/> class with the specified color type.
    /// </summary>
    /// <param name="type">The palette color type to use.</param>
    public PaletteExtension(ColorType type) => Type = type;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaletteExtension"/> class.
    /// </summary>
    public PaletteExtension() { }

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the palette-based theme brush with the specified modifiers.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush for the specified palette color type and modifiers.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var converter = new ThemeBrushConverter
        {
            Opacity = Opacity?.ToString() ?? CustomOpacity,
            Contrast = Contrast,
            Darken = Darken,
            Lighten = Lighten
        };

        return new ReflectionBindingExtension($"(my:{nameof(PaletteAssist)}.{Type})")
        {
            RelativeSource = RelativeSource,
            Converter = converter
        }.ProvideValue(serviceProvider);
    }
}

/// <summary>
/// Enumerates palette color types for use with the <see cref="PaletteExtension"/>.
/// </summary>
public enum ColorType
{
    /// <summary>
    /// Primary palette color.
    /// </summary>
    Primary,

    /// <summary>
    /// Secondary palette color.
    /// </summary>
    Secondary,

    /// <summary>
    /// Tertiary palette color.
    /// </summary>
    Tertiary
}
