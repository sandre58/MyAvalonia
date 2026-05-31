// -----------------------------------------------------------------------
// <copyright file="MaterialIconExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Metadata;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.MarkupExtensions;

namespace MyNet.Avalonia.Theme.Controls.MarkupExtensions;

/// <summary>
/// Markup extension for creating and binding to a themed icon in XAML.
/// Allows specifying the icon data (geometry key), size category, or explicit size for consistent icon rendering in the UI.
/// </summary>
public class MaterialIconExtension : IconExtension<MaterialIcon>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialIconExtension"/> class with the specified icon data key.
    /// </summary>
    public MaterialIconExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialIconExtension"/> class with the specified icon data key.
    /// </summary>
    /// <param name="kind">The geometry resource key for the icon.</param>
    public MaterialIconExtension(MaterialIconKind kind) => Kind = kind;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialIconExtension"/> class with the specified icon data key and size category.
    /// </summary>
    /// <param name="kind">The geometry resource key for the icon.</param>
    /// <param name="size">The predefined icon size category.</param>
    public MaterialIconExtension(MaterialIconKind kind, IconSize size)
    {
        Kind = kind;
        DefinedSize = size;
    }

    /// <summary>
    /// Gets or sets the geometry resource key for the icon.
    /// </summary>
    [ConstructorArgument("kind")]
    public MaterialIconKind Kind { get; set; }

    /// <summary>
    /// Gets or sets a binding for the icon kind. Use this when data binding is required.
    /// </summary>
    public BindingBase? KindBinding { get; set; }

    /// <summary>
    /// Gets or sets the animation to play. Provides IntelliSense autocomplete.
    /// </summary>
    public MaterialIconAnimation Animation { get; set; }

    /// <summary>
    /// Gets or sets a binding for the animation. Use this when data binding is required.
    /// </summary>
    public BindingBase? AnimationBinding { get; set; }

    // <inheritdoc />
    protected override MaterialIcon BuildIcon()
    {
        var result = new MaterialIcon();

        // Kind: binding takes precedence
        if (KindBinding is not null)
            result.Bind(MaterialIcon.KindProperty, KindBinding);
        else
            result.Kind = Kind;

        // Animation: binding takes precedence
        if (AnimationBinding is not null) result.Bind(MaterialIcon.AnimationProperty, AnimationBinding);
        else result.Animation = Animation;

        if (Size.HasValue)
            result.IconSize = Size.Value;

        return result;
    }
}
