// -----------------------------------------------------------------------
// <copyright file="AdornedContentControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Lays out an adornment (icon, glyph, etc.) beside primary content on any side.
/// </summary>
public class AdornedContentControl : ContentControl
{
    #region Adornment

    public static readonly StyledProperty<object?> AdornmentProperty =
        AvaloniaProperty.Register<AdornedContentControl, object?>(nameof(Adornment));

    public object? Adornment
    {
        get => GetValue(AdornmentProperty);
        set => SetValue(AdornmentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> AdornmentTemplateProperty =
        AvaloniaProperty.Register<AdornedContentControl, IDataTemplate?>(nameof(AdornmentTemplate));

    public IDataTemplate? AdornmentTemplate
    {
        get => GetValue(AdornmentTemplateProperty);
        set => SetValue(AdornmentTemplateProperty, value);
    }

    public static readonly StyledProperty<Position> AdornmentPositionProperty =
        AvaloniaProperty.Register<AdornedContentControl, Position>(nameof(AdornmentPosition));

    public Position AdornmentPosition
    {
        get => GetValue(AdornmentPositionProperty);
        set => SetValue(AdornmentPositionProperty, value);
    }

    public static readonly StyledProperty<double> AdornmentOpacityProperty =
        AvaloniaProperty.Register<AdornedContentControl, double>(nameof(AdornmentOpacity), 0.7d);

    public double AdornmentOpacity
    {
        get => GetValue(AdornmentOpacityProperty);
        set => SetValue(AdornmentOpacityProperty, value);
    }

    public static readonly StyledProperty<double> AdornmentSizeProperty =
        AvaloniaProperty.Register<AdornedContentControl, double>(nameof(AdornmentSize), 15.0d);

    public double AdornmentSize
    {
        get => GetValue(AdornmentSizeProperty);
        set => SetValue(AdornmentSizeProperty, value);
    }

    #endregion

    #region Spacing

    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<AdornedContentControl, double>(nameof(Spacing), 5.0d);

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    #endregion

    #region Content icon sizing

    public static readonly StyledProperty<double> ContentIconSizeProperty =
        AvaloniaProperty.Register<AdornedContentControl, double>(nameof(ContentIconSize), 18.0d);

    public double ContentIconSize
    {
        get => GetValue(ContentIconSizeProperty);
        set => SetValue(ContentIconSizeProperty, value);
    }

    #endregion
}
