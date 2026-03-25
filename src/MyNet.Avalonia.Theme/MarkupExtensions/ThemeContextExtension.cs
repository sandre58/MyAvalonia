// -----------------------------------------------------------------------
// <copyright file="ThemeContextExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Converters.Internals;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to role-based theme brushes with optional modifiers.
/// Resolves the brush based on the control's theme role and palette color type, applying optional effects such as opacity, contrast, darken, and lighten.
/// </summary>
public class ThemeContextExtension(string path) : ThemeBrushExtensionBase
{
    /// <summary>
    /// Gets or sets the resource path for the theme brush.
    /// </summary>
    [ConstructorArgument("path")]
    public string Path { get; set; } = path;

    /// <summary>
    /// Gets or sets the path to provide context.
    /// </summary>
    public string Context { get; set; } = $"(my:{nameof(ThemeAssist)}.Context)";

    /// <summary>
    /// Gets or sets a value indicating whether to ignore the foreground of the parent control when resolving the theme brush. Default is false.
    /// </summary>
    public bool IgnoreForegroundParent { get; set; }

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    public RelativeSource RelativeSource { get; set; } = new RelativeSource(RelativeSourceMode.Self);

    /// <summary>
    /// Provides the value for the markup extension, returning a binding to the theme brush with the specified options.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A binding to the theme brush with the specified opacity and contrast settings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var result = new MultiBinding
        {
            Converter = ThemeConverter.Default,
            ConverterParameter = new ThemeBrushParameters(Opacity?.ToString() ?? CustomOpacity, Contrast, Darken, Lighten),

            Bindings =
            {
                // Context binding to provide the theme context for resolving the theme brush, which may influence the palette color type used for resolving the brush.
                new ReflectionBinding(Context)
                {
                    Mode = BindingMode.OneWay,
                    RelativeSource = RelativeSource,
                    TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
                },

                // Resource path binding to provide the key for resolving the theme brush, which may be influenced by the context.
                new ReflectionBinding
                {
                    Mode = BindingMode.OneTime,
                    Source = Path
                }
            }
        };

        if (!IgnoreForegroundParent)
        {
            result.Bindings.Add(new ReflectionBinding("Parent.(TextElement.Foreground)")
            {
                Mode = BindingMode.OneWay,
                FallbackValue = null,
                RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                TypeResolver = (x, y) => ResolveType(serviceProvider, x, y)
            });
        }

        return result;
    }
}
