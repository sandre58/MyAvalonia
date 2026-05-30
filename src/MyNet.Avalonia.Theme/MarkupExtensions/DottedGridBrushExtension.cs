// -----------------------------------------------------------------------
// <copyright file="DottedGridBrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Metadata;
using MyNet.Avalonia.Converters;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating a tiled dotted grid <see cref="Avalonia.Media.DrawingBrush"/> from a base brush or color in XAML.
/// Allows XAML to generate dotted grid drawing brushes dynamically from theme colors or bound properties.
/// The brush automatically updates when the theme changes.
/// </summary>
public class DottedGridBrushExtension : ThemeBrushExtensionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DottedGridBrushExtension"/> class with the specified resource path.
    /// </summary>
    /// <param name="path">The resource path for the base brush or color.</param>
    public DottedGridBrushExtension(string path) => Path = path;

    /// <summary>
    /// Initializes a new instance of the <see cref="DottedGridBrushExtension"/> class.
    /// </summary>
    public DottedGridBrushExtension() { }

    /// <summary>
    /// Gets or sets the resource path for the base brush or color.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    public RelativeSource RelativeSource { get; set; } = new(RelativeSourceMode.Self);

    /// <summary>
    /// Gets or sets the width and height of each tile in pixels. Default is 16.
    /// </summary>
    public double TileSize { get; set; } = 16.0;

    /// <summary>
    /// Gets or sets the radius of each dot ellipse in pixels. Default is 0.5.
    /// </summary>
    public double DotRadius { get; set; } = 0.5;

    /// <summary>
    /// Gets or sets the overall opacity of the drawing brush. Default is 1.0.
    /// </summary>
    public new double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to create a dotted grid drawing brush from the base brush or color.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding that creates a dotted grid drawing brush with the specified parameters.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var parameters = new DottedGridParameters(TileSize, DotRadius, Opacity);

        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = DottedGridBrushConverter.Default,
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
