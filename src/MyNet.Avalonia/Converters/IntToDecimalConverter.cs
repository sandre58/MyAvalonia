// -----------------------------------------------------------------------
// <copyright file="IntToDecimalConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public class IntToDecimalConverter : IValueConverter
{
    public static readonly IntToDecimalConverter Default = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is int d ? System.Convert.ToDecimal(d) : null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value is decimal d ? System.Convert.ToInt32(d) : null;
}
