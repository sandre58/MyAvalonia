// -----------------------------------------------------------------------
// <copyright file="ThemeBrushExtensionBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Markup.Xaml;
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
}
