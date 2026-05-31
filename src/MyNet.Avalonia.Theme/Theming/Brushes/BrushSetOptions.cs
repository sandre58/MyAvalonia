// -----------------------------------------------------------------------
// <copyright file="BrushSetOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Theming.Brushes;

/// <summary>
/// Global options for <see cref="BrushSet"/> transformed-brush caching.
/// </summary>
public static class BrushSetOptions
{
    /// <summary>
    /// Default maximum number of transformed brushes (opacity / contrast / darken / lighten variants) cached per <see cref="BrushSet"/>.
    /// Main and contrast brushes are not counted toward this limit.
    /// </summary>
    public const int DefaultTransformedBrushCapacity = 48;

    /// <summary>
    /// Gets or sets the maximum transformed brushes cached per <see cref="BrushSet"/> for newly created sets.
    /// </summary>
    public static int TransformedBrushCapacity { get; set; } = DefaultTransformedBrushCapacity;
}
