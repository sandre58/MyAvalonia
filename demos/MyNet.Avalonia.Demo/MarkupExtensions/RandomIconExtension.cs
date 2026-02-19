// -----------------------------------------------------------------------
// <copyright file="RandomIconExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

/// <summary>
/// Markup extension for creating and binding to a themed icon in XAML.
/// Allows specifying the icon data (geometry key), size category, or explicit size for consistent icon rendering in the UI.
/// </summary>
internal sealed class RandomIconExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RandomIconExtension"/> class with the specified icon data key and size category.
    /// </summary>
    /// <param name="size">The predefined icon size category.</param>
    public RandomIconExtension(IconSize size) => DefinedSize = size;

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomIconExtension"/> class with the specified icon data key and size category.
    /// </summary>
    public RandomIconExtension() { }

    /// <summary>
    /// Gets or sets the predefined icon size category.
    /// </summary>
    [ConstructorArgument("size")]
    public IconSize? DefinedSize { get; set; }

    /// <summary>
    /// Gets or sets an explicit icon size in device-independent units. If set, overrides <see cref="DefinedSize"/>.
    /// </summary>
    public double? Size { get; set; }

    /// <summary>
    /// Provides the value for the markup extension, returning a themed <see cref="PathIcon"/> with the specified geometry and size.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A <see cref="PathIcon"/> instance configured with the specified icon data and size.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var result = new PathIcon
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Focusable = false,
            Opacity = 1
        };

        var data = RandomGenerator.Enum<IconData>().ToGeometry();
        _ = result.SetValue(PathIcon.DataProperty, data);
        if (Size.HasValue)
        {
            result.Width = Size.Value;
            result.Height = Size.Value;
        }
        else if (DefinedSize.HasValue)
        {
            result.Classes.Add($"size-{DefinedSize.ToString()?.ToLower(CultureInfo.CurrentCulture)}");
        }

        return result;
    }
}
