// -----------------------------------------------------------------------
// <copyright file="ThemeRoleExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme.Converters.Internals;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to role-based theme brushes with optional modifiers.
/// Resolves the brush based on the control's theme role and palette color type, applying optional effects such as opacity, contrast, darken, and lighten.
/// </summary>
public class ThemeRoleExtension : ThemeBrushExtensionBase
{
    /// <summary>
    /// Gets or sets the palette color type to use (Primary, Secondary, Tertiary). Default is Primary.
    /// </summary>
    [ConstructorArgument("type")]
    public PaletteColor Type { get; set; } = PaletteColor.Primary;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with the specified color type.
    /// </summary>
    /// <param name="type">The palette color type to use.</param>
    public ThemeRoleExtension(PaletteColor type) => Type = type;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with Primary as default.
    /// </summary>
    public ThemeRoleExtension() { }

    /// <summary>
    /// Provides the parameters for the theme brush conversion, including palette color type, opacity, and contrast.
    /// </summary>
    /// <returns>A <see cref="ThemeRoleParameters"/> instance.</returns>
    protected override ThemeBrushParameters? ProvideBrushParameters() => new ThemeRoleParameters(Type, Opacity?.ToString() ?? CustomOpacity, Contrast);

    /// <summary>
    /// Provides the relative source for the binding. Always returns self.
    /// </summary>
    /// <returns>The <see cref="RelativeSource"/> instance.</returns>
    protected override RelativeSource? ProvideRelativeSource() => new(RelativeSourceMode.Self);
}
