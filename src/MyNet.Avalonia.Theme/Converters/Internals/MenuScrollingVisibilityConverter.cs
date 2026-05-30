// -----------------------------------------------------------------------
// <copyright file="MenuScrollingVisibilityConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Theme.Converters.Internals;

// A more robust converter for menu scrolling visibility
internal sealed class MenuScrollingVisibilityConverter : IMultiValueConverter
{
    private const double Epsilon = 1.0; // tolerance in pixels

    public static readonly MenuScrollingVisibilityConverter Default = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter == null ||
            values is not [ScrollBarVisibility visibility, double offset, double extent, double viewport])
        {
            return AvaloniaProperty.UnsetValue;
        }

        if (visibility != ScrollBarVisibility.Auto)
            return visibility == ScrollBarVisibility.Visible;

        // FIX 1: ignore tiny differences
        if (extent <= viewport + Epsilon)
            return false;

        double target;

        switch (parameter)
        {
            case double d:
                target = d;
                break;
            case string s:
                target = double.Parse(s, NumberFormatInfo.InvariantInfo);
                break;
            default:
                return AvaloniaProperty.UnsetValue;
        }

        var maxOffset = extent - viewport;

        // FIX 2: avoid division instability
        if (maxOffset <= Epsilon)
            return false;

        var percent = Math.Clamp(offset * 100.0 / maxOffset, 0, 100);

        // FIX 3: tolerance on edge detection
        return Math.Abs(percent - target) >= 1.0;
    }
}
