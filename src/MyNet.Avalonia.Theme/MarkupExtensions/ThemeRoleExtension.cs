// -----------------------------------------------------------------------
// <copyright file="ThemeRoleExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Converters.Internals;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to role-based theme brushes with optional modifiers.
/// Resolves the brush based on the control's theme role and palette color type, applying optional effects such as opacity, contrast, darken, and lighten.
/// </summary>
public class ThemeRoleExtension : ThemeBrushExtensionBase
{
    /// <summary>
    /// Gets or sets the variant brush type to use (Background, BorderBrush, Foreground, Primary). Default is Primary.
    /// </summary>
    [ConstructorArgument("variant")]
    public VariantBrush? VariantBrush { get; set; }

    /// <summary>
    /// Gets or sets the path to provide role.
    /// </summary>
    public string Role { get; set; } = $"(my:{nameof(ThemeAssist)}.Role)";

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with the specified variant brush.
    /// </summary>
    /// <param name="variant">The variant brush to use.</param>
    public ThemeRoleExtension(VariantBrush variant) => VariantBrush = variant;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeRoleExtension"/> class with Primary as default.
    /// </summary>
    public ThemeRoleExtension() { }

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    public RelativeSource RelativeSource { get; set; } = new RelativeSource(RelativeSourceMode.Self);

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the theme brush with the specified options.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush with the specified opacity and contrast settings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => new MultiBinding
    {
        Bindings =
        {
            new ReflectionBinding(Role)
            {
                Mode = BindingMode.OneWay,
                RelativeSource = RelativeSource,
                TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
            },
            VariantBrush.HasValue ? new ReflectionBinding($"(my:{nameof(VariantAssist)}.Default{VariantBrush})")
            {
                Mode = BindingMode.OneWay,
                RelativeSource = RelativeSource,
                TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
            }
            : CompiledBinding.Create<object, IBrush?>(_ => null),
            new ReflectionBinding
            {
                Mode = BindingMode.OneWay,
                RelativeSource = RelativeSource
            },
            new ReflectionBinding("(TextElement.Foreground)")
            {
                Mode = BindingMode.OneWay,
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = typeof(Control)
                },
                TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
            }
        },
        Converter = ThemeConverter.Default,
        ConverterParameter = new ThemeRoleParameters(Opacity?.ToString() ?? CustomOpacity, Contrast, Darken, Lighten)
    };
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
