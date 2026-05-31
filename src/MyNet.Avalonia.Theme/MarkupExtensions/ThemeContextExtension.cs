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
using MyNet.Avalonia.Theme.MarkupExtensions.Helpers;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for binding to context-based theme brushes with optional modifiers.
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
                ThemeBindingHelper.Create(Context, RelativeSource, serviceProvider),
                ThemeBindingHelper.CreateConstantSource(Path)
            }
        };

        if (!IgnoreForegroundParent)
            result.Bindings.Add(ThemeBindingHelper.CreateParentForeground(serviceProvider));

        return result;
    }
}
