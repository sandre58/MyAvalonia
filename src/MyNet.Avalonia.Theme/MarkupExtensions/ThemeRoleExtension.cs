// -----------------------------------------------------------------------
// <copyright file="ThemeRoleExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Converters.Internals;
using MyNet.Avalonia.Theme.MarkupExtensions.Helpers;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to role-based theme brushes with optional modifiers.
/// Resolves the brush based on the control's theme role and palette color type, applying optional effects such as opacity, contrast, darken, and lighten.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with the specified variant brush.
/// </remarks>
/// <param name="variant">The variant brush to use.</param>
public class ThemeRoleExtension(VariantBrush variant) : ThemeBrushExtensionBase
{
    /// <summary>
    /// Gets or sets the variant brush type to use (Background, BorderBrush, Foreground, Primary). Default is Primary.
    /// </summary>
    [ConstructorArgument("variant")]
    public VariantBrush VariantBrush { get; set; } = variant;

    /// <summary>
    /// Gets or sets the path to provide role.
    /// </summary>
    public string Role { get; set; } = $"(my:{nameof(ThemeAssist)}.Role)";

    /// <summary>
    /// Gets or sets a value indicating whether to ignore the foreground of the parent control when resolving the theme brush. Default is false.
    /// </summary>
    public bool IgnoreForegroundParent { get; set; }

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    public RelativeSource RelativeSource { get; set; } = new(RelativeSourceMode.Self);

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var result = new MultiBinding
        {
            Converter = ThemeConverter.Default,
            ConverterParameter = new ThemeBrushParameters(Opacity?.ToString() ?? CustomOpacity, Contrast, Darken, Lighten),
            Bindings =
            {
                ThemeBindingHelper.Create(Role, RelativeSource, serviceProvider),
                ThemeBindingHelper.Create($"(my:{nameof(VariantAssist)}.Default{VariantBrush})", RelativeSource, serviceProvider)
            }
        };

        if (!IgnoreForegroundParent)
            result.Bindings.Add(ThemeBindingHelper.CreateParentForeground(serviceProvider));

        return result;
    }
}

/// <summary>
/// Enumerates variant brush types.
/// </summary>
public enum VariantBrush
{
    /// <summary>
    /// Background variant brush.
    /// </summary>
    Background,

    /// <summary>
    /// Border variant brush.
    /// </summary>
    BorderBrush,

    /// <summary>
    /// Foreground variant brush.
    /// </summary>
    Foreground,

    /// <summary>
    /// Primary variant brush.
    /// </summary>
    Primary
}
