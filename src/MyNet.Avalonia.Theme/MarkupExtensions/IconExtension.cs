// -----------------------------------------------------------------------
// <copyright file="IconExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Extensions;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating and binding to a themed icon in XAML.
/// Allows specifying the icon data (geometry key), size category, or explicit size for consistent icon rendering in the UI.
/// </summary>
public abstract class IconExtension<TIcon> : MarkupExtension
where TIcon : IconElement
{
    /// <summary>
    /// Gets or sets the predefined icon size category.
    /// </summary>
    [ConstructorArgument("size")]
    public IconSize? DefinedSize { get; set; }

    /// <summary>
    /// Gets or sets an explicit icon size in device-independent units. If set, overrides <see cref="DefinedSize"/>.
    /// </summary>
    public double? Size { get; set; }

    /// <summary>
    /// Gets or sets the icon background brush.
    /// </summary>
    public IBrush? Background { get; set; }

    /// <summary>
    /// Gets or sets a binding for the icon background. Use this when data binding is required.
    /// </summary>
    public BindingBase? BackgroundBinding { get; set; }

    /// <summary>
    /// Gets or sets the icon border brush.
    /// </summary>
    public IBrush? BorderBrush { get; set; }

    /// <summary>
    /// Gets or sets a binding for the icon border. Use this when data binding is required.
    /// </summary>
    public BindingBase? BorderBrushBinding { get; set; }

    /// <summary>
    /// Gets or sets the vertical alignment of the content.
    /// </summary>
    public VerticalAlignment? VerticalAlignment { get; set; }

    /// <summary>
    /// Gets or sets the horizontal alignment of the content.
    /// </summary>
    public HorizontalAlignment? HorizontalAlignment { get; set; }

    /// <summary>
    /// Gets or sets the class names to apply to the element.
    /// </summary>
    public string? Classes { get; set; }

    /// <summary>
    /// When implemented in a derived class, builds and returns the specific icon element based on the provided properties and bindings.
    /// </summary>
    /// <returns>Returns an icon.</returns>
    protected abstract TIcon BuildIcon();

    /// <summary>
    /// Provides the value for the markup extension, returning a themed icon with the specified geometry and size.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>An instance configured with the specified icon data and size.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var result = BuildIcon();

        if (Size.HasValue)
        {
            result.Height = Size.Value;
            result.Width = Size.Value;
        }
        else if (DefinedSize.HasValue)
        {
            result.AddClasses(CssClass.Size(DefinedSize.ToString()));
        }

        // Background: binding takes precedence
        if (BackgroundBinding is not null)
            result.Bind(TemplatedControl.BackgroundProperty, BackgroundBinding);
        else if (Background is not null)
            result.Background = Background;

        // Background: binding takes precedence
        if (BorderBrushBinding is not null)
            result.Bind(TemplatedControl.BorderBrushProperty, BorderBrushBinding);
        else if (BorderBrush is not null)
            result.BorderBrush = BorderBrush;

        if (VerticalAlignment is not null)
            result.VerticalAlignment = VerticalAlignment.Value;
        if (HorizontalAlignment is not null)
            result.HorizontalAlignment = HorizontalAlignment.Value;

        if (!string.IsNullOrWhiteSpace(Classes))
            result.Classes.AddRange(global::Avalonia.Controls.Classes.Parse(Classes!));

        return result;
    }
}
