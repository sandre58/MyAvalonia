// -----------------------------------------------------------------------
// <copyright file="ThemeExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to a theme brush by resource path.
/// Allows XAML to reference theme brushes directly using a simple path string.
/// </summary>
/// <example>
/// <code>
/// <Border Background="{theme Path=Primary}" />
/// </code>
/// </example>
public class ThemeExtension(string path) : MarkupExtension
{
    /// <summary>
    /// Gets or sets the resource path for the theme brush.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = path;

    /// <summary>
    /// Provides the value for the markup extension, returning the theme brush for the specified path.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>The theme brush instance for the specified path.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => MyTheme.Current.GetBrush(Path);
}
