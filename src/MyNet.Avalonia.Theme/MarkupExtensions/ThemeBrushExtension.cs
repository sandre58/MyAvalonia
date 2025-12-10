// -----------------------------------------------------------------------
// <copyright file="ThemeBrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Markup.Xaml;

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
    /// Provides the resource path for the theme brush.
    /// </summary>
    /// <returns>The resource path string.</returns>
    protected override string ProvidePath() => Path;

    /// <summary>
    /// Provides the relative source for the binding.
    /// </summary>
    /// <returns>The <see cref="RelativeSource"/> instance.</returns>
    protected override RelativeSource? ProvideRelativeSource() => RelativeSource;
}
