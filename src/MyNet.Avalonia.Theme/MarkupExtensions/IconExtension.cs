// -----------------------------------------------------------------------
// <copyright file="IconExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using MyNet.Avalonia.Theme.Enums;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating and binding to a themed icon in XAML.
/// Allows specifying the icon data (geometry key), size category, or explicit size for consistent icon rendering in the UI.
/// </summary>
public class IconExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IconExtension"/> class with the specified icon data key.
    /// </summary>
    /// <param name="data">The geometry resource key for the icon.</param>
    public IconExtension(string data) => Data = data;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconExtension"/> class with the specified icon data key and size category.
    /// </summary>
    /// <param name="data">The geometry resource key for the icon.</param>
    /// <param name="size">The predefined icon size category.</param>
    public IconExtension(string data, IconSize size)
    {
        Data = data;
        DefinedSize = size;
    }

    /// <summary>
    /// Gets or sets the geometry resource key for the icon.
    /// </summary>
    [ConstructorArgument("data")]
    public string Data { get; set; }

    /// <summary>
    /// Gets or sets the predefined icon size category. Default is <see cref="IconSize.Default"/>.
    /// </summary>
    [ConstructorArgument("size")]
    public IconSize DefinedSize { get; set; } = IconSize.Default;

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

        var data = new StaticResourceExtension(ThemeResourceKeyFactory.Geometry(Data)).ProvideValue(serviceProvider);
        _ = result.SetValue(PathIcon.DataProperty, data);
        if (Size.HasValue)
        {
            result.Width = Size.Value;
            result.Height = Size.Value;
        }
        else
        {
            result.Classes.Add(DefinedSize.ToString());
        }

        return result;
    }
}
