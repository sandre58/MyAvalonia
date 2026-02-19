// -----------------------------------------------------------------------
// <copyright file="SpacingExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Enums;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating standard spacing values based on the <see cref="SpacingSize"/> enumeration.
/// </summary>
public class SpacingExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpacingExtension"/> class.
    /// </summary>
    public SpacingExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpacingExtension"/> class with the specified size.
    /// </summary>
    /// <param name="size">The standard spacing size to use.</param>
    public SpacingExtension(SpacingSize size) => Size = size;

    /// <summary>
    /// Gets or sets the standard thickness size to use.
    /// </summary>
    [ConstructorArgument("size")]
    public SpacingSize Size { get; set; } = SpacingSize.None;

    /// <summary>
    /// Provides the value for the markup extension, returning a <see cref="double"/> with the specified size.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A <see cref="double"/> instance configured with the specified values.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => (double)Size;
}
