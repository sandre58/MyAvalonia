// -----------------------------------------------------------------------
// <copyright file="ForegroundExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Converters.Internals;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for referencing theme brushes in XAML with optional opacity, contrast, darken, and lighten settings.
/// Allows XAML to bind to theme brushes by resource path and apply color transformations dynamically.
/// </summary>
public class ForegroundExtension : ThemeBrushExtensionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForegroundExtension"/> class.
    /// </summary>
    /// <param name="relativeSourceMode">The relative source mode for the binding.</param>
    public ForegroundExtension(RelativeSourceMode relativeSourceMode) => RelativeSourceMode = relativeSourceMode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForegroundExtension"/> class.
    /// </summary>
    public ForegroundExtension() { }

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    [ConstructorArgument("relativeSourceMode")]
    public RelativeSourceMode RelativeSourceMode { get; set; } = RelativeSourceMode.Self;

    /// <summary>
    /// Gets or sets the ancestor type for the relative source binding, if applicable.
    /// </summary>
    public Type? AncestorType { get; set; }

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the theme brush with the specified options.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush with the specified opacity, contrast, darken, and lighten settings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => new ReflectionBinding("(TextElement.Foreground)")
    {
        Mode = BindingMode.OneWay,
        RelativeSource = new RelativeSource(AncestorType is not null ? RelativeSourceMode.FindAncestor : RelativeSourceMode)
        {
            AncestorType = AncestorType
        },
        Converter = ThemeConverter.Default,
        ConverterParameter = new ThemeBrushParameters(Opacity?.ToString() ?? CustomOpacity, Contrast, Darken, Lighten),
        TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
    };
}
