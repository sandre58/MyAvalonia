// -----------------------------------------------------------------------
// <copyright file="RandomIconExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Controls.MarkupExtensions;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

/// <summary>
/// Markup extension for creating and binding to a themed icon in XAML.
/// Allows specifying the icon data (geometry key), size category, or explicit size for consistent icon rendering in the UI.
/// </summary>
internal sealed class RandomIconExtension : MaterialIconExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RandomIconExtension"/> class with the specified icon data key and size category.
    /// </summary>
    /// <param name="size">The predefined icon size category.</param>
    public RandomIconExtension(IconSize size) => DefinedSize = size;

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomIconExtension"/> class with the specified icon data key and size category.
    /// </summary>
    public RandomIconExtension() => Kind = RandomGenerator.Current.Enum<MaterialIconKind>();
}
