// -----------------------------------------------------------------------
// <copyright file="IMyTheme.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Theming;

/// <summary>
/// Defines the contract for a theme implementation, providing access to primary and accent color palettes and the current theme name.
/// </summary>
public interface IMyTheme
{
    /// <summary>
    /// Gets or sets the primary color palette for the theme.
    /// </summary>
    ColorShades Primary { get; set; }

    /// <summary>
    /// Gets or sets the accent color palette for the theme.
    /// </summary>
    ColorShades Accent { get; set; }

    /// <summary>
    /// Gets or sets the current theme name or identifier.
    /// </summary>
    string? Theme { get; set; }
}
