// -----------------------------------------------------------------------
// <copyright file="ForegroundExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Converters.Internals;
using MyNet.Avalonia.Theme.MarkupExtensions.Helpers;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for referencing theme brushes in XAML with optional opacity, contrast, darken, and lighten settings.
/// Allows XAML to bind to theme brushes by resource path and apply color transformations dynamically.
/// </summary>
public class ForegroundExtension : ThemeBrushExtensionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForegroundExtension"/> class.
    /// </summary>
    /// <param name="relativeSourceMode">The relative source mode for the binding.</param>
    public ForegroundExtension(RelativeSourceMode relativeSourceMode) => RelativeSourceMode = relativeSourceMode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForegroundExtension"/> class.
    /// </summary>
    public ForegroundExtension() { }

    /// <summary>
    /// Gets or sets the relative source for the binding. Default is self.
    /// </summary>
    [ConstructorArgument("relativeSourceMode")]
    public RelativeSourceMode RelativeSourceMode { get; set; } = RelativeSourceMode.Self;

    /// <summary>
    /// Gets or sets the ancestor type for the relative source binding, if applicable.
    /// </summary>
    public Type? AncestorType { get; set; }

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var relativeSource = new RelativeSource(AncestorType is not null ? RelativeSourceMode.FindAncestor : RelativeSourceMode)
        {
            AncestorType = AncestorType
        };
        var binding = ThemeBindingHelper.Create("(TextElement.Foreground)", relativeSource, serviceProvider);

        binding.FallbackValue = null;
        binding.Converter = ThemeConverter.Default;
        binding.ConverterParameter = new ThemeBrushParameters(Opacity?.ToString() ?? CustomOpacity, Contrast, Darken, Lighten);
        return binding;
    }
}
