// -----------------------------------------------------------------------
// <copyright file="FontSizeExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating standard font size values based on the <see cref="FontSize"/> enumeration.
/// </summary>
public class FontSizeExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FontSizeExtension"/> class.
    /// </summary>
    public FontSizeExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontSizeExtension"/> class with the specified size.
    /// </summary>
    /// <param name="size">The standard font size to use.</param>
    public FontSizeExtension(FontSize size) => Size = size;

    /// <summary>
    /// Gets or sets the standard font size to use.
    /// </summary>
    [ConstructorArgument("size")]
    public FontSize Size { get; set; } = FontSize.Md;

    /// <summary>
    /// Provides the value for the markup extension, returning a <see cref="double"/> with the specified size.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A <see cref="double"/> instance configured with the specified values.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => ThemeResources.Font.Size.Get(Size).Value;
}
