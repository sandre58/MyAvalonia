// -----------------------------------------------------------------------
// <copyright file="ThemeBrushExtensionBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Base markup extension for binding to theme brushes with optional opacity, contrast, darken, and lighten settings.
/// Allows XAML to reference theme brushes by resource path and apply color transformations dynamically.
/// </summary>
public abstract class ThemeBrushExtensionBase : MarkupExtension
{
    private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

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
    /// Gets or sets the darken factor to make the brush darker (value between 0.0 and 1.0).
    /// </summary>
    public double? Darken { get; set; }

    /// <summary>
    /// Gets or sets the lighten factor to make the brush lighter (value between 0.0 and 1.0).
    /// </summary>
    public double? Lighten { get; set; }

    /// <summary>
    /// Resolves a type from the XAML type resolver service with caching for improved performance.
    /// </summary>
    /// <param name="ctx">The service provider context.</param>
    /// <param name="namespacePrefix">The namespace prefix (optional).</param>
    /// <param name="type">The type name to resolve.</param>
    /// <returns>The resolved <see cref="Type"/>.</returns>
    protected static Type ResolveType(IServiceProvider ctx, string? namespacePrefix, string type)
    {
        var name = string.IsNullOrEmpty(namespacePrefix) ? type : $"{namespacePrefix}:{type}";

        if (TypeCache.TryGetValue(name, out var cachedType))
            return cachedType;

        var tr = ctx.GetRequiredService<IXamlTypeResolver>();
        var resolvedType = tr.Resolve(name);
        TypeCache.TryAdd(name, resolvedType);
        return resolvedType;
    }
}
