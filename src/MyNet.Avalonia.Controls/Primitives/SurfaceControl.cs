// -----------------------------------------------------------------------
// <copyright file="SurfaceControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls.Primitives;

public abstract class SurfaceControl : ContentControl
{
    #region Leading

    public static readonly StyledProperty<object?> LeadingProperty = AvaloniaProperty.Register<SurfaceControl, object?>(nameof(Leading));

    public object? Leading
    {
        get => GetValue(LeadingProperty);
        set => SetValue(LeadingProperty, value);
    }

    #endregion

    #region Header

    public static readonly StyledProperty<object?> HeaderProperty = AvaloniaProperty.Register<SurfaceControl, object?>(nameof(Header));

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    #endregion

    #region Trailing

    public static readonly StyledProperty<object?> TrailingProperty = AvaloniaProperty.Register<SurfaceControl, object?>(nameof(Trailing));

    public object? Trailing
    {
        get => GetValue(TrailingProperty);
        set => SetValue(TrailingProperty, value);
    }

    #endregion

    #region Actions

    public static readonly StyledProperty<object?> ActionsProperty = AvaloniaProperty.Register<SurfaceControl, object?>(nameof(Actions));

    public object? Actions
    {
        get => GetValue(ActionsProperty);
        set => SetValue(ActionsProperty, value);
    }

    #endregion
}
