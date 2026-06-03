// -----------------------------------------------------------------------
// <copyright file="CornerRadiusExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating standard corner radius values based on the <see cref="CornerSize"/> enumeration.
/// </summary>
public class CornerRadiusExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CornerRadiusExtension"/> class.
    /// </summary>
    public CornerRadiusExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CornerRadiusExtension"/> class with the specified size.
    /// </summary>
    /// <param name="size">The standard corner radius size to use.</param>
    public CornerRadiusExtension(CornerSize size) => Size = size;

    /// <summary>
    /// Gets or sets the standard corner radius size to use.
    /// </summary>
    [ConstructorArgument("size")]
    public CornerSize Size { get; set; } = CornerSize.Md;

    /// <summary>
    /// Provides the value for the markup extension, returning a <see cref="double"/> with the specified size.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A <see cref="double"/> instance configured with the specified values.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => ThemeResources.Corners.Get(Size).Value;
}
