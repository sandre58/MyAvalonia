// -----------------------------------------------------------------------
// <copyright file="RatingValueHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.Internals.Rating;

internal static class RatingValueHelper
{
    public static double GetStep(RatingPrecision precision) =>
        precision switch
        {
            RatingPrecision.Integer => 1,
            RatingPrecision.Half => 0.5,
            RatingPrecision.Continuous => 0.1,
            _ => 1
        };

    public static double Snap(double value, RatingPrecision precision, double minimum, double maximum)
    {
        value = value.SafeClamp(minimum, maximum);

        if (precision == RatingPrecision.Continuous)
            return Math.Round(value, 1, MidpointRounding.AwayFromZero);

        var step = GetStep(precision);
        var snapped = (Math.Round(((value - minimum) / step), MidpointRounding.AwayFromZero) * step) + minimum;
        return snapped.SafeClamp(minimum, maximum);
    }

    public static double GetFillRatio(double value, int index) =>
        (value - (index - 1)).SafeClamp(0, 1);

    public static double ValueFromItemPosition(
        int index,
        double fraction,
        RatingPrecision precision,
        double minimum,
        double maximum)
    {
        if (precision == RatingPrecision.Integer)
            return ((double)index).SafeClamp(minimum, maximum);

        if (precision == RatingPrecision.Half)
        {
            var value = fraction <= 0.5 ? index - 0.5 : index;
            return value.SafeClamp(minimum, maximum);
        }

        return Snap((index - 1) + fraction, precision, minimum, maximum);
    }

    public static double Increment(
        double value,
        double delta,
        RatingPrecision precision,
        double minimum,
        double maximum) =>
        Snap(value + delta, precision, minimum, maximum);

    public static double GetEffectiveMinimum(bool isClearable, double minimum) =>
        isClearable ? minimum : Math.Max(minimum, minimum <= 0 ? 1 : minimum);

    public static double GetPointerFraction(bool isHorizontal, double width, double height, double x, double y)
    {
        if (width <= 0 || height <= 0)
            return 1;

        return isHorizontal
            ? (x / width).SafeClamp(0, 1)
            : (y / height).SafeClamp(0, 1);
    }

    public static double GetPointerFractionInContent(
        bool isHorizontal,
        double contentSize,
        double itemWidth,
        double itemHeight,
        double paddingLeft,
        double paddingTop,
        double paddingRight,
        double paddingBottom,
        double x,
        double y)
    {
        if (contentSize <= 0)
            return 1;

        if (isHorizontal)
        {
            var localX = x - paddingLeft;
            return (localX / contentSize).SafeClamp(0, 1);
        }

        var localY = y - paddingTop;
        return (localY / contentSize).SafeClamp(0, 1);
    }
}
