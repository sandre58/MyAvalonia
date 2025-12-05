// -----------------------------------------------------------------------
// <copyright file="ThemeRoleExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme.Converters;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to role-based theme brushes with optional modifiers.
/// Resolves the brush based on the control's theme role and palette color type, applying optional effects such as opacity, contrast, darken, and lighten.
/// </summary>
public class ThemeRoleExtension : MarkupExtension
{
    /// <summary>
    /// Gets or sets the palette color type to use (Primary, Secondary, Tertiary). Default is Primary.
    /// </summary>
    [ConstructorArgument("type")]
    public ColorType Type { get; set; } = ColorType.Primary;

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
    /// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with the specified color type.
    /// </summary>
    /// <param name="type">The palette color type to use.</param>
    public ThemeRoleExtension(ColorType type) => Type = type;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with Primary as default.
    /// </summary>
    public ThemeRoleExtension() { }

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the control with a converter that resolves the role-based theme brush.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush for the specified role and color type with modifiers.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var converter = new ThemeRoleConverter
        {
            ColorType = Type,
            Opacity = Opacity?.ToString() ?? CustomOpacity,
            Contrast = Contrast,
            Darken = Darken,
            Lighten = Lighten
        };

        // Simply bind to the control itself (Self)
        // The converter will extract the Role from the control's attached property
        return new Binding
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.Self),
            Converter = converter
        };
    }
}
