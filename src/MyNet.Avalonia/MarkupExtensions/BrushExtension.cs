// -----------------------------------------------------------------------
// <copyright file="BrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Metadata;
using MyNet.Avalonia.Converters;

namespace MyNet.Avalonia.MarkupExtensions;

public class BrushExtension(string path) : MarkupExtension
{
    /// <summary>
    /// Gets or sets the path for the brush.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = path;

    /// <summary>
    /// Gets or sets the opacity value.
    /// </summary>
    public double? Opacity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use the contrast brush for accessibility.
    /// </summary>
    public bool? Contrast { get; set; }

    /// <summary>
    /// Gets or sets a value indicating how much to darken the brush color (0.0 to 1.0).
    /// </summary>
    public double? Darken { get; set; }

    /// <summary>
    /// Gets or sets a value indicating how much to lighten the brush color (0.0 to 1.0).
    /// </summary>
    public double? Lighten { get; set; }

    public RelativeSource? RelativeSource { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var converter = new BrushConverter()
        {
            Opacity = Opacity,
            Contrast = Contrast ?? false,
            Darken = Darken,
            Lighten = Lighten
        };

        return new ReflectionBindingExtension(Path)
        {
            RelativeSource = RelativeSource,
            Converter = converter
        }.ProvideValue(serviceProvider);
    }
}
