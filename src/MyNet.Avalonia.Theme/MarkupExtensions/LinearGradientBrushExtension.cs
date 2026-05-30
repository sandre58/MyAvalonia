// -----------------------------------------------------------------------
// <copyright file="LinearGradientBrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Metadata;
using MyNet.Avalonia.Converters;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating linear gradient brushes from a base color in XAML with customizable orientation and color variations.
/// Allows XAML to generate linear gradient brushes dynamically from theme colors or bound properties.
/// The gradient automatically updates when the theme changes.
/// </summary>
public class LinearGradientBrushExtension : ThemeBrushExtensionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinearGradientBrushExtension"/> class with the specified resource path.
    /// </summary>
    /// <param name="path">The resource path for the base color.</param>
    public LinearGradientBrushExtension(string path) => Path = path;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearGradientBrushExtension"/> class.
    /// </summary>
    public LinearGradientBrushExtension() { }

    /// <summary>
    /// Gets or sets the resource path for the base color.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    public RelativeSource RelativeSource { get; set; } = new(RelativeSourceMode.Self);

    /// <summary>
    /// Gets or sets the orientation of the gradient (horizontal, vertical, diagonal).
    /// </summary>
    public GradientOrientation Orientation { get; set; } = GradientOrientation.Horizontal;

    /// <summary>
    /// Gets or sets the lighten factor for the start color (0.0 to 1.0).
    /// </summary>
    public double? StartLighten { get; set; }

    /// <summary>
    /// Gets or sets the darken factor for the end color (0.0 to 1.0).
    /// </summary>
    public double? EndDarken { get; set; }

    /// <summary>
    /// Gets or sets an optional middle color for three-stop gradients.
    /// </summary>
    public Color? MiddleColor { get; set; }

    /// <summary>
    /// Gets or sets the lighten factor for the middle color (0.0 to 1.0).
    /// </summary>
    public double? MiddleLighten { get; set; }

    /// <summary>
    /// Gets or sets the darken factor for the middle color (0.0 to 1.0).
    /// </summary>
    public double? MiddleDarken { get; set; }

    /// <summary>
    /// Gets or sets the offset for the middle gradient stop (0.0 to 1.0, default 0.5).
    /// </summary>
    public double MiddleOffset { get; set; } = 0.5;

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to create a linear gradient brush from the base color.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding that creates a linear gradient brush with the specified parameters.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var parameters = new LinearGradientParameters(
            Orientation,
            StartLighten,
            EndDarken,
            MiddleColor,
            MiddleLighten,
            MiddleDarken,
            MiddleOffset);

        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = LinearGradientConverter.Default,
            ConverterParameter = parameters
        };

        multiBinding.Bindings.Add(new ReflectionBinding(Path)
        {
            Mode = BindingMode.OneWay,
            RelativeSource = RelativeSource,
            TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
        });

        // ThemeVersion changes after every theme update (after brushes are refreshed),
        // forcing the converter to re-run and pick up the new brush color.
        multiBinding.Bindings.Add(new ReflectionBinding(nameof(MyTheme.ThemeVersion))
        {
            Mode = BindingMode.OneWay,
            Source = MyTheme.Current
        });

        return multiBinding;
    }
}
