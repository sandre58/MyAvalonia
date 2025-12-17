// -----------------------------------------------------------------------
// <copyright file="ForegroundExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
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
    /// <returns>A binding to the theme brush with the specified opacity and contrast settings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => new Binding("(TextElement.Foreground)")
    {
        Mode = BindingMode.OneWay,
        RelativeSource = new RelativeSource(AncestorType is not null ? RelativeSourceMode.FindAncestor : RelativeSourceMode)
        {
            AncestorType = AncestorType
        },
        Converter = ThemeConverter.Default,
        ConverterParameter = new ThemeBrushParameters(Opacity?.ToString() ?? CustomOpacity, Contrast),
        NameScope = new WeakReference<INameScope?>(serviceProvider.GetService<INameScope>()),
        TypeResolver = (x, y) => ResolveType(serviceProvider, x, y),
    };

    /// <summary>
    /// Resolves a type from the XAML type resolver service.
    /// </summary>
    /// <param name="ctx">The service provider context.</param>
    /// <param name="namespacePrefix">The namespace prefix (optional).</param>
    /// <param name="type">The type name to resolve.</param>
    /// <returns>The resolved <see cref="Type"/>.</returns>
    private static Type ResolveType(IServiceProvider ctx, string? namespacePrefix, string type)
    {
        var tr = ctx.GetRequiredService<IXamlTypeResolver>();
        var name = string.IsNullOrEmpty(namespacePrefix) ? type : $"{namespacePrefix}:{type}";
        return tr.Resolve(name);
    }
}
