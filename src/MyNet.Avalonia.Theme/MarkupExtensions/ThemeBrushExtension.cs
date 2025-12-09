// -----------------------------------------------------------------------
// <copyright file="ThemeBrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to theme brushes with optional opacity, contrast, darken, and lighten settings.
/// Allows XAML to reference theme brushes by path and apply opacity, contrast, or color transformations dynamically.
/// </summary>
public class ThemeBrushExtension : ThemeBrushExtensionBase
{
    public ThemeBrushExtension(string path) => Path = path;

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

    protected override string ProvidePath() => Path;

    protected override RelativeSource? ProvideRelativeSource() => RelativeSource;
}
