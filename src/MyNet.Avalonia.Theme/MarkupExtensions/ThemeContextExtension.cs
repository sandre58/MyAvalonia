// -----------------------------------------------------------------------
// <copyright file="ThemeContextExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Converters.Internals;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to role-based theme brushes with optional modifiers.
/// Resolves the brush based on the control's theme role and palette color type, applying optional effects such as opacity, contrast, darken, and lighten.
/// </summary>
public class ThemeContextExtension(string path) : ThemeBrushExtensionBase
{
    /// <summary>
    /// Gets or sets the resource path for the theme brush.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = path;

    /// <summary>
    /// Gets or sets the path to provide context.
    /// </summary>
    public string Context { get; set; } = $"(my:{nameof(ThemeAssist)}.Context)";

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    public RelativeSource RelativeSource { get; set; } = new RelativeSource(RelativeSourceMode.Self);

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the theme brush with the specified options.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush with the specified opacity and contrast settings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => new MultiBinding
    {
        Bindings =
        {
            new ReflectionBinding(Context)
            {
                Mode = BindingMode.OneWay,
                RelativeSource = RelativeSource,
                TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
            },
            new ReflectionBinding
            {
                Mode = BindingMode.OneWay,
                RelativeSource = new RelativeSource(RelativeSourceMode.Self)
            },
            new ReflectionBinding("(TextElement.Foreground)")
            {
                Mode = BindingMode.OneWay,
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = typeof(Control)
                },
                TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
            }
        },
        Converter = ThemeConverter.Default,
        ConverterParameter = new ThemeContextParameters(Path, Opacity?.ToString() ?? CustomOpacity, Contrast, Darken, Lighten)
    };
}
