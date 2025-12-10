// -----------------------------------------------------------------------
// <copyright file="ThemeBrushExtensionBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Theme.Converters.Internals;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Base markup extension for binding to theme brushes with optional opacity, contrast, darken, and lighten settings.
/// Allows XAML to reference theme brushes by resource path and apply color transformations dynamically.
/// </summary>
public abstract class ThemeBrushExtensionBase : MarkupExtension
{
    /// <summary>
    /// Gets or sets the named opacity value from the <see cref="Opacity"/> enum.
    /// </summary>
    public Opacity? Opacity { get; set; }

    /// <summary>
    /// Gets or sets a custom opacity value as a string (e.g., "0.5" or a resource key).
    /// </summary>
    public string? CustomOpacity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use the contrast brush for accessibility.
    /// </summary>
    public bool Contrast { get; set; }

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the theme brush with the specified options.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush with the specified opacity and contrast settings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => new Binding(ProvidePath())
    {
        Mode = BindingMode.OneWay,
        RelativeSource = ProvideRelativeSource(),
        Converter = ThemeConverter.Default,
        ConverterParameter = ProvideBrushParameters(),
        NameScope = new WeakReference<INameScope?>(serviceProvider.GetService<INameScope>()),
        TypeResolver = (x, y) => ResolveType(serviceProvider, x, y),
    };

    /// <summary>
    /// Provides the parameters for the theme brush conversion, including opacity and contrast.
    /// </summary>
    /// <returns>A <see cref="ThemeBrushParameters"/> instance.</returns>
    protected virtual ThemeBrushParameters? ProvideBrushParameters() => new(Opacity?.ToString() ?? CustomOpacity, Contrast);

    /// <summary>
    /// Provides the resource path for the theme brush.
    /// </summary>
    /// <returns>The resource path string.</returns>
    protected virtual string ProvidePath() => string.Empty;

    /// <summary>
    /// Provides the relative source for the binding.
    /// </summary>
    /// <returns>The <see cref="RelativeSource"/> instance, or null.</returns>
    protected virtual RelativeSource? ProvideRelativeSource() => null;

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
