// -----------------------------------------------------------------------
// <copyright file="ThemeRoleExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme.Converters;

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
    public ColorType Type { get; set; } = ColorType.Primary;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with the specified color type.
    /// </summary>
    /// <param name="type">The palette color type to use.</param>
    public ThemeRoleExtension(ColorType type) => Type = type;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with Primary as default.
    /// </summary>
    public ThemeRoleExtension() { }

    protected override ThemeBrushParameters? ProvideBrushParameters() => new ThemeRoleParameters(Type, Opacity?.ToString() ?? CustomOpacity, Contrast);

    protected override RelativeSource? ProvideRelativeSource() => new(RelativeSourceMode.Self);
}
