// -----------------------------------------------------------------------
// <copyright file="ClockHandLengthConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Theme.Converters;

public class ClockHandLengthConverter(double ratio) : IValueConverter
{
    public static ClockHandLengthConverter Hour { get; } = new(1 - 0.6);

    public static ClockHandLengthConverter Minute { get; } = new(0.6);

    public static ClockHandLengthConverter Second { get; } = new(0.7);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is double d ? d * ratio / 2 : 0.0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
