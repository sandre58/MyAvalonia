// -----------------------------------------------------------------------
// <copyright file="CornerRadiusConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Unified corner radius conversions: double to <see cref="CornerRadius"/>, corner adjustments, and corner radius to double.
/// </summary>
/// <remarks>
/// Use the static members on <see cref="CornerRadiusFromDoubleConverter"/>, <see cref="CornerRadiusAdjustConverter"/>,
/// and <see cref="CornerRadiusToDoubleConverter"/> from XAML (<c>x:Static</c> does not support nested types).
/// </remarks>
/// <example>
/// <code>
/// CornerRadius="{Binding CornerRadius, Converter={x:Static my:CornerRadiusAdjustConverter.Left}}"
/// RadiusX="{Binding CornerRadius, Converter={x:Static my:CornerRadiusToDoubleConverter.TopLeft}}"
/// </code>
/// </example>
public sealed class CornerRadiusConverter : IValueConverter
{
    internal CornerRadiusConverter(CornerRadiusOperation operation) => _operation = operation;

    private readonly CornerRadiusOperation _operation;

    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => _operation switch
        {
            CornerRadiusOperation.FromDoubleAll or CornerRadiusOperation.FromDoubleLeft or CornerRadiusOperation.FromDoubleTop
                or CornerRadiusOperation.FromDoubleRight or CornerRadiusOperation.FromDoubleBottom
                => ConvertFromDouble(value, _operation),
            CornerRadiusOperation.ToDoubleTopLeft or CornerRadiusOperation.ToDoubleTopRight
                or CornerRadiusOperation.ToDoubleBottomLeft or CornerRadiusOperation.ToDoubleBottomRight
                => ConvertToDouble(value, _operation),
            _ => value is CornerRadius cornerRadius ? AdjustCornerRadius(cornerRadius, _operation) : 0
        };

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => _operation is CornerRadiusOperation.FromDoubleAll or CornerRadiusOperation.FromDoubleLeft or CornerRadiusOperation.FromDoubleTop
            or CornerRadiusOperation.FromDoubleRight or CornerRadiusOperation.FromDoubleBottom
            ? throw new NotSupportedException()
            : AvaloniaProperty.UnsetValue;

    [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", Justification = "All cases are handled explicitly, and the default case throws an exception for invalid operations.")]
    private static CornerRadius ConvertFromDouble(object? value, CornerRadiusOperation operation)
    {
        var val = Math.Max(0, (double?)value ?? 0);
        return operation switch
        {
            CornerRadiusOperation.FromDoubleLeft => new(val, 0, 0, val),
            CornerRadiusOperation.FromDoubleRight => new(0, val, val, 0),
            CornerRadiusOperation.FromDoubleTop => new(val, val, 0, 0),
            CornerRadiusOperation.FromDoubleBottom => new(0, 0, val, val),
            CornerRadiusOperation.FromDoubleAll => new(val),
            _ => throw new InvalidOperationException()
        };
    }

    private static double ConvertToDouble(object? value, CornerRadiusOperation operation)
        => value is CornerRadius cornerRadius
            ? operation switch
            {
                CornerRadiusOperation.ToDoubleTopLeft => cornerRadius.TopLeft,
                CornerRadiusOperation.ToDoubleTopRight => cornerRadius.TopRight,
                CornerRadiusOperation.ToDoubleBottomLeft => cornerRadius.BottomLeft,
                CornerRadiusOperation.ToDoubleBottomRight => cornerRadius.BottomRight,
                _ => 0
            }
            : 0;

    private static CornerRadius AdjustCornerRadius(CornerRadius cornerRadius, CornerRadiusOperation operation)
        => operation switch
        {
            CornerRadiusOperation.AdjustLeft => new(cornerRadius.TopLeft, 0, 0, cornerRadius.BottomLeft),
            CornerRadiusOperation.AdjustTop => new(cornerRadius.TopLeft, cornerRadius.TopRight, 0, 0),
            CornerRadiusOperation.AdjustRight => new(0, cornerRadius.TopRight, cornerRadius.BottomRight, 0),
            CornerRadiusOperation.AdjustBottom => new(0, 0, cornerRadius.BottomRight, cornerRadius.BottomLeft),
            _ => default
        };

    internal enum CornerRadiusOperation
    {
        FromDoubleAll,
        FromDoubleLeft,
        FromDoubleTop,
        FromDoubleRight,
        FromDoubleBottom,
        AdjustLeft,
        AdjustTop,
        AdjustRight,
        AdjustBottom,
        ToDoubleTopLeft,
        ToDoubleTopRight,
        ToDoubleBottomLeft,
        ToDoubleBottomRight
    }
}

/// <summary>
/// Converts a numeric value to a <see cref="CornerRadius"/>.
/// </summary>
public static class CornerRadiusFromDoubleConverter
{
    /// <summary>Applies the value uniformly to all four corners.</summary>
    public static readonly IValueConverter All = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.FromDoubleAll);

    /// <summary>Sets top-left and top-right radius.</summary>
    public static readonly IValueConverter Top = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.FromDoubleTop);

    /// <summary>Sets top-left and bottom-left radius.</summary>
    public static readonly IValueConverter Left = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.FromDoubleLeft);

    /// <summary>Sets top-right and bottom-right radius.</summary>
    public static readonly IValueConverter Right = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.FromDoubleRight);

    /// <summary>Sets bottom-left and bottom-right radius.</summary>
    public static readonly IValueConverter Bottom = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.FromDoubleBottom);
}

/// <summary>
/// Adjusts an existing <see cref="CornerRadius"/> by zeroing corners on one side.
/// </summary>
public static class CornerRadiusAdjustConverter
{
    /// <summary>Clears the right corners, keeping top-left and bottom-left.</summary>
    public static readonly IValueConverter Left = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.AdjustLeft);

    /// <summary>Clears the bottom corners, keeping top-left and top-right.</summary>
    public static readonly IValueConverter Top = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.AdjustTop);

    /// <summary>Clears the left corners, keeping top-right and bottom-right.</summary>
    public static readonly IValueConverter Right = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.AdjustRight);

    /// <summary>Clears the top corners, keeping bottom-left and bottom-right.</summary>
    public static readonly IValueConverter Bottom = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.AdjustBottom);
}

/// <summary>
/// Extracts a single corner component from a <see cref="CornerRadius"/> as a <see cref="double"/>.
/// </summary>
public static class CornerRadiusToDoubleConverter
{
    public static readonly IValueConverter TopLeft = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.ToDoubleTopLeft);

    public static readonly IValueConverter TopRight = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.ToDoubleTopRight);

    public static readonly IValueConverter BottomLeft = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.ToDoubleBottomLeft);

    public static readonly IValueConverter BottomRight = new CornerRadiusConverter(CornerRadiusConverter.CornerRadiusOperation.ToDoubleBottomRight);
}
