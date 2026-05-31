// -----------------------------------------------------------------------
// <copyright file="TimeFormatToCellShiftConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MyNet.Primitives.Temporal;

namespace MyNet.Avalonia.Theme.Controls.Converters.Internals;

internal sealed class TimeFormatToCellShiftConverter : IValueConverter
{
    public static TimeFormatToCellShiftConverter Default { get; } = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value switch
    {
        TimeFormat.TwelveHour => 1,
        TimeFormat.TwentyFourHour => 0,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
