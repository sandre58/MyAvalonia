// -----------------------------------------------------------------------
// <copyright file="ThemeRoleExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Theme.Assists;
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
    /// Gets or sets the path to provide role.
    /// </summary>
    public string Role { get; set; } = $"(my:{nameof(ThemeAssist)}.Role)";

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
            new Binding(Role)
            {
                Mode = BindingMode.OneWay,
                RelativeSource = RelativeSource,
                NameScope = new WeakReference<INameScope?>(serviceProvider.GetService<INameScope>()),
                TypeResolver = (x, y) => ResolveType(serviceProvider, x, y),
            },
            new Binding($"(my:{nameof(PaletteAssist)}.{Type})")
            {
                Mode = BindingMode.OneWay,
                RelativeSource = RelativeSource,
                NameScope = new WeakReference<INameScope?>(serviceProvider.GetService<INameScope>()),
                TypeResolver = (x, y) => ResolveType(serviceProvider, x, y),
            },
            new Binding()
            {
                Mode = BindingMode.OneWay,
                RelativeSource = RelativeSource
            }
        },
        Converter = ThemeConverter.Default,
        ConverterParameter = new ThemeRoleParameters(Type, Opacity?.ToString() ?? CustomOpacity, Contrast)
    };

    /// <summary>
    /// Resolves a type from the XAML type resolver service.
    /// </summary>
    /// <param name="ctx">The service provider context.</param>
    /// <param name="namespacePrefix">The namespace prefix (optional).</param>
    /// <param name="type">The type name to resolve.</param>
    /// <returns>The resolved <see cref="Type"/>.</returns>
    private static Type ResolveType(IServiceProvider ctx, string? namespacePrefix, string type)
    {
        var tr = ctx.GetRequiredService<IXamlTypeResolver>();
        var name = string.IsNullOrEmpty(namespacePrefix) ? type : $"{namespacePrefix}:{type}";
        return tr.Resolve(name);
    }
}
