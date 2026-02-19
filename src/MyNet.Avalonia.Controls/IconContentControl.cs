// -----------------------------------------------------------------------
// <copyright file="IconContentControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls;

public class IconContentControl : ContentControl
{
    #region Icon

    /// <summary>
    /// Provides Icon Property.
    /// </summary>
    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<IconContentControl, object?>(nameof(Icon));

    /// <summary>
    /// Gets or sets the Icon property.
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    #endregion

    #region Spacing

    /// <summary>
    /// Provides Spacing Property.
    /// </summary>
    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<IconContentControl, double>(nameof(Spacing), 10.0d);

    /// <summary>
    /// Gets or sets the Spacing property.
    /// </summary>
    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    #endregion

    #region IconPosition

    /// <summary>
    /// Provides IconPosition Property.
    /// </summary>
    public static readonly StyledProperty<Position> IconPositionProperty = AvaloniaProperty.Register<IconContentControl, Position>(nameof(Position), Position.Left);

    /// <summary>
    /// Gets or sets the IconPosition property.
    /// </summary>
    public Position IconPosition
    {
        get => GetValue(IconPositionProperty);
        set => SetValue(IconPositionProperty, value);
    }

    #endregion

    #region IconOpacity

    /// <summary>
    /// Provides IconOpacity Property.
    /// </summary>
    public static readonly StyledProperty<double> IconOpacityProperty = AvaloniaProperty.Register<IconContentControl, double>(nameof(IconOpacity), 0.7d);

    /// <summary>
    /// Gets or sets the IconOpacity property.
    /// </summary>
    public double IconOpacity
    {
        get => GetValue(IconOpacityProperty);
        set => SetValue(IconOpacityProperty, value);
    }

    #endregion

    #region IconTemplate

    /// <summary>
    /// Provides IconTemplate Property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<IconContentControl, IDataTemplate?>(nameof(IconTemplate));

    /// <summary>
    /// Gets or sets the IconTemplate property.
    /// </summary>
    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    #endregion
}
