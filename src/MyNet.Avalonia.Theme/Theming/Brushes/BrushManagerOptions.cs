// -----------------------------------------------------------------------
// <copyright file="BrushManagerOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Theming.Brushes;

/// <summary>
/// Global options for <see cref="BrushManager"/> registration behavior.
/// </summary>
public static class BrushManagerOptions
{
    /// <summary>
    /// Standard theme opacity levels from design tokens (Dark/Light variants).
    /// </summary>
    public static readonly double[] ThemeOpacityLevels =
    [
        0.7, 0.56, 0.45, 0.40, 0.38, 0.32, 0.24, 0.16, 0.12, 0.08
    ];

    /// <summary>
    /// Gets or sets a value indicating whether newly registered brush sets pre-create transformed brushes for <see cref="ThemeOpacityLevels"/>.
    /// </summary>
    public static bool PrewarmThemeOpacityLevels { get; set; } = true;
}
