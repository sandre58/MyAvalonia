// -----------------------------------------------------------------------
// <copyright file="ThemeBrushExtension.cs" company="Stéphane ANDRE">
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
public class ThemeBrushExtension : ThemeBrushExtensionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeBrushExtension"/> class with the specified resource path.
    /// </summary>
    /// <param name="path">The resource path for the theme brush.</param>
    public ThemeBrushExtension(string path) => Path = path;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeBrushExtension"/> class.
    /// </summary>
    public ThemeBrushExtension() { }

    /// <summary>
    /// Gets or sets the resource path for the theme brush.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    public RelativeSource RelativeSource { get; set; } = new RelativeSource(RelativeSourceMode.Self);

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the theme brush with the specified options.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush with the specified opacity and contrast settings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => new Binding(Path)
    {
        Mode = BindingMode.OneWay,
        RelativeSource = RelativeSource,
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
