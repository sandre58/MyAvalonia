// -----------------------------------------------------------------------
// <copyright file="ExpandControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace MyNet.Avalonia.Controls.Primitives;

public class ExpandControl : ContentControl
{
    public static readonly StyledProperty<double> MultiplierProperty = AvaloniaProperty.Register<ExpandControl, double>(nameof(Multiplier));

    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<ExpandControl, Orientation>(nameof(Orientation));

    static ExpandControl()
    {
        AffectsArrange<ExpandControl>(MultiplierProperty, OrientationProperty);

        AffectsMeasure<ExpandControl>(MultiplierProperty, OrientationProperty);
    }

    public double Multiplier
    {
        get => GetValue(MultiplierProperty);
        set => SetValue(MultiplierProperty, value);
    }

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    protected override Size MeasureCore(Size availableSize)
    {
        var result = base.MeasureCore(availableSize);
        return result;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var result = base.ArrangeOverride(finalSize);
        return result;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var result = base.MeasureOverride(availableSize);

        var w = result.Width;
        var h = result.Height;

        switch (Orientation)
        {
            case Orientation.Horizontal:
                w *= Multiplier;
                break;

            case Orientation.Vertical:
                h *= Multiplier;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(availableSize));
        }

        return new Size(w, h);
    }
}
