// -----------------------------------------------------------------------
// <copyright file="ThicknessConverter.cs" company="Stéphane ANDRE">
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
/// Unified thickness conversions: double to <see cref="Thickness"/>, thickness adjustments, and thickness to double.
/// </summary>
/// <example>
/// <code>
/// &lt;!-- Spacing on the right only --&gt;
/// Margin="{Binding IconSpacing, Converter={x:Static my:ThicknessConverter.FromDouble.Right}}"
///
/// &lt;!-- Remove inner border between split buttons --&gt;
/// BorderThickness="{Binding BorderThickness, Converter={x:Static my:ThicknessConverter.Adjust.RemoveRight}}"
/// </code>
/// </example>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Provides a clear API for different conversion types (from double, adjustments, to double) while keeping the implementation details hidden.")]
public sealed class ThicknessConverter : IValueConverter
{
    private readonly ThicknessOperation _operation;

    private ThicknessConverter(ThicknessOperation operation) => _operation = operation;

    /// <summary>
    /// Converts a numeric value to a <see cref="Thickness"/>.
    /// </summary>
    public static class FromDouble
    {
        /// <summary>Applies the value uniformly to all four sides.</summary>
        public static readonly IValueConverter All = new ThicknessConverter(ThicknessOperation.FromDoubleAll);

        /// <summary>Sets left thickness only.</summary>
        public static readonly IValueConverter Left = new ThicknessConverter(ThicknessOperation.FromDoubleLeft);

        /// <summary>Sets top thickness only.</summary>
        public static readonly IValueConverter Top = new ThicknessConverter(ThicknessOperation.FromDoubleTop);

        /// <summary>Sets right thickness only.</summary>
        public static readonly IValueConverter Right = new ThicknessConverter(ThicknessOperation.FromDoubleRight);

        /// <summary>Sets bottom thickness only.</summary>
        public static readonly IValueConverter Bottom = new ThicknessConverter(ThicknessOperation.FromDoubleBottom);

        /// <summary>Sets left and right thickness.</summary>
        public static readonly IValueConverter Horizontal = new ThicknessConverter(ThicknessOperation.FromDoubleHorizontal);

        /// <summary>Sets top and bottom thickness.</summary>
        public static readonly IValueConverter Vertical = new ThicknessConverter(ThicknessOperation.FromDoubleVertical);
    }

    /// <summary>
    /// Adjusts an existing <see cref="Thickness"/> value.
    /// </summary>
    /// <remarks>
    /// <c>Remove*</c> zeroes one side; <c>Add*To*</c> transfers thickness from one side to another
    /// (for example, <see cref="AddLeftToRight"/> adds left padding to the right margin).
    /// </remarks>
    public static class Adjust
    {
        public static readonly IValueConverter Left = new ThicknessConverter(ThicknessOperation.ExtractLeft);
        public static readonly IValueConverter Top = new ThicknessConverter(ThicknessOperation.ExtractTop);
        public static readonly IValueConverter Right = new ThicknessConverter(ThicknessOperation.ExtractRight);
        public static readonly IValueConverter Bottom = new ThicknessConverter(ThicknessOperation.ExtractBottom);
        public static readonly IValueConverter RemoveLeft = new ThicknessConverter(ThicknessOperation.RemoveLeft);
        public static readonly IValueConverter RemoveTop = new ThicknessConverter(ThicknessOperation.RemoveTop);
        public static readonly IValueConverter RemoveRight = new ThicknessConverter(ThicknessOperation.RemoveRight);
        public static readonly IValueConverter RemoveBottom = new ThicknessConverter(ThicknessOperation.RemoveBottom);
        public static readonly IValueConverter AddLeftToRight = new ThicknessConverter(ThicknessOperation.AddLeftToRight);
        public static readonly IValueConverter AddLeftToTop = new ThicknessConverter(ThicknessOperation.AddLeftToTop);
        public static readonly IValueConverter AddLeftToBottom = new ThicknessConverter(ThicknessOperation.AddLeftToBottom);
        public static readonly IValueConverter AddRightToLeft = new ThicknessConverter(ThicknessOperation.AddRightToLeft);
        public static readonly IValueConverter AddRightToTop = new ThicknessConverter(ThicknessOperation.AddRightToTop);
        public static readonly IValueConverter AddRightToBottom = new ThicknessConverter(ThicknessOperation.AddRightToBottom);
        public static readonly IValueConverter AddBottomToTop = new ThicknessConverter(ThicknessOperation.AddBottomToTop);
        public static readonly IValueConverter AddBottomToLeft = new ThicknessConverter(ThicknessOperation.AddBottomToLeft);
        public static readonly IValueConverter AddBottomToRight = new ThicknessConverter(ThicknessOperation.AddBottomToRight);
        public static readonly IValueConverter AddTopToBottom = new ThicknessConverter(ThicknessOperation.AddTopToBottom);
        public static readonly IValueConverter AddTopToLeft = new ThicknessConverter(ThicknessOperation.AddTopToLeft);
        public static readonly IValueConverter AddTopToRight = new ThicknessConverter(ThicknessOperation.AddTopToRight);
    }

    /// <summary>
    /// Extracts a single side from a <see cref="Thickness"/> as a double.
    /// </summary>
    public static class ToDouble
    {
        public static readonly IValueConverter Left = new ThicknessConverter(ThicknessOperation.ToDoubleLeft);
        public static readonly IValueConverter Top = new ThicknessConverter(ThicknessOperation.ToDoubleTop);
        public static readonly IValueConverter Right = new ThicknessConverter(ThicknessOperation.ToDoubleRight);
        public static readonly IValueConverter Bottom = new ThicknessConverter(ThicknessOperation.ToDoubleBottom);
    }

    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => _operation switch
        {
            ThicknessOperation.FromDoubleAll or ThicknessOperation.FromDoubleLeft or ThicknessOperation.FromDoubleTop
                or ThicknessOperation.FromDoubleRight or ThicknessOperation.FromDoubleBottom
                or ThicknessOperation.FromDoubleHorizontal or ThicknessOperation.FromDoubleVertical
                => ConvertFromDouble(value, _operation),
            ThicknessOperation.ToDoubleLeft or ThicknessOperation.ToDoubleTop or ThicknessOperation.ToDoubleRight
                or ThicknessOperation.ToDoubleBottom
                => ConvertToDouble(value, _operation),
            _ => value is Thickness thickness ? AdjustThickness(thickness, _operation) : 0
        };

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => _operation is ThicknessOperation.FromDoubleAll or ThicknessOperation.FromDoubleLeft or ThicknessOperation.FromDoubleTop
            or ThicknessOperation.FromDoubleRight or ThicknessOperation.FromDoubleBottom
            or ThicknessOperation.FromDoubleHorizontal or ThicknessOperation.FromDoubleVertical
            ? throw new NotSupportedException()
            : AvaloniaProperty.UnsetValue;

    [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", Justification = "All enum values are handled explicitly, and the default case is unreachable.")]
    private static Thickness ConvertFromDouble(object? value, ThicknessOperation operation)
    {
        var val = Math.Max(0, (double?)value ?? 0);
        return operation switch
        {
            ThicknessOperation.FromDoubleLeft => new(val, 0, 0, 0),
            ThicknessOperation.FromDoubleRight => new(0, 0, val, 0),
            ThicknessOperation.FromDoubleTop => new(0, val, 0, 0),
            ThicknessOperation.FromDoubleBottom => new(0, 0, 0, val),
            ThicknessOperation.FromDoubleHorizontal => new(val, 0, val, 0),
            ThicknessOperation.FromDoubleVertical => new(0, val, 0, val),
            ThicknessOperation.FromDoubleAll => new(val),
            _ => throw new InvalidOperationException()
        };
    }

    private static double ConvertToDouble(object? value, ThicknessOperation operation)
        => value is Thickness thickness
            ? operation switch
            {
                ThicknessOperation.ToDoubleLeft => thickness.Left,
                ThicknessOperation.ToDoubleTop => thickness.Top,
                ThicknessOperation.ToDoubleRight => thickness.Right,
                ThicknessOperation.ToDoubleBottom => thickness.Bottom,
                _ => 0
            }
            : 0;

    private static Thickness AdjustThickness(Thickness thickness, ThicknessOperation operation)
        => operation switch
        {
            ThicknessOperation.ExtractLeft => new(thickness.Left),
            ThicknessOperation.ExtractTop => new(thickness.Top),
            ThicknessOperation.ExtractRight => new(thickness.Right),
            ThicknessOperation.ExtractBottom => new(thickness.Bottom),
            ThicknessOperation.RemoveLeft => new(0, thickness.Top, thickness.Right, thickness.Bottom),
            ThicknessOperation.RemoveTop => new(thickness.Left, 0, thickness.Right, thickness.Bottom),
            ThicknessOperation.RemoveRight => new(thickness.Left, thickness.Top, 0, thickness.Bottom),
            ThicknessOperation.RemoveBottom => new(thickness.Left, thickness.Top, thickness.Right, 0),
            ThicknessOperation.AddLeftToRight => new(thickness.Left, thickness.Top, thickness.Right + thickness.Left, thickness.Bottom),
            ThicknessOperation.AddLeftToTop => new(thickness.Left, thickness.Top + thickness.Left, thickness.Right, thickness.Bottom),
            ThicknessOperation.AddLeftToBottom => new(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom + thickness.Left),
            ThicknessOperation.AddRightToLeft => new(thickness.Left + thickness.Right, thickness.Top, thickness.Right, thickness.Bottom),
            ThicknessOperation.AddRightToTop => new(thickness.Left, thickness.Top + thickness.Right, thickness.Right, thickness.Bottom),
            ThicknessOperation.AddRightToBottom => new(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom + thickness.Right),
            ThicknessOperation.AddBottomToTop => new(thickness.Left, thickness.Top + thickness.Bottom, thickness.Right, thickness.Bottom),
            ThicknessOperation.AddBottomToLeft => new(thickness.Left + thickness.Bottom, thickness.Top, thickness.Right, thickness.Bottom),
            ThicknessOperation.AddBottomToRight => new(thickness.Left, thickness.Top, thickness.Right + thickness.Bottom, thickness.Bottom),
            ThicknessOperation.AddTopToBottom => new(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom + thickness.Top),
            ThicknessOperation.AddTopToLeft => new(thickness.Left + thickness.Top, thickness.Top, thickness.Right, thickness.Bottom),
            ThicknessOperation.AddTopToRight => new(thickness.Left, thickness.Top, thickness.Right + thickness.Top, thickness.Bottom),
            _ => default
        };

    private enum ThicknessOperation
    {
        FromDoubleAll,
        FromDoubleLeft,
        FromDoubleTop,
        FromDoubleRight,
        FromDoubleBottom,
        FromDoubleHorizontal,
        FromDoubleVertical,
        ExtractLeft,
        ExtractTop,
        ExtractRight,
        ExtractBottom,
        RemoveLeft,
        RemoveTop,
        RemoveRight,
        RemoveBottom,
        AddLeftToRight,
        AddLeftToTop,
        AddLeftToBottom,
        AddRightToLeft,
        AddRightToTop,
        AddRightToBottom,
        AddBottomToTop,
        AddBottomToLeft,
        AddBottomToRight,
        AddTopToBottom,
        AddTopToLeft,
        AddTopToRight,
        ToDoubleLeft,
        ToDoubleTop,
        ToDoubleRight,
        ToDoubleBottom
    }
}
