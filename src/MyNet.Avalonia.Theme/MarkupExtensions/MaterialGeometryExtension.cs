// -----------------------------------------------------------------------
// <copyright file="MaterialGeometryExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using Material.Icons;
using MyNet.Avalonia.Controls.Extensions;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating and binding to a themed geometry in XAML.
/// Allows specifying the icon data (geometry key), size category, or explicit size for consistent icon rendering in the UI.
/// </summary>
public class MaterialGeometryExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialGeometryExtension"/> class with the specified icon data key.
    /// </summary>
    public MaterialGeometryExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialGeometryExtension"/> class with the specified icon data key.
    /// </summary>
    /// <param name="kind">The geometry resource key for the icon.</param>
    public MaterialGeometryExtension(MaterialIconKind kind) => Kind = kind;

    /// <summary>
    /// Gets or sets the geometry resource key for the icon.
    /// </summary>
    [ConstructorArgument("kind")]
    public MaterialIconKind Kind { get; set; }

    /// <summary>
    /// Provides the value for the markup extension, returning a themed <see cref="Geometry"/> with the specified geometry and size.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A <see cref="TIcon"/> instance configured with the specified icon data and size.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => Kind.ToGeometry();
}
