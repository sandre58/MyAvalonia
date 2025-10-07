// -----------------------------------------------------------------------
// <copyright file="EnumConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public class EnumConverter : IValueConverter
{
    /// <summary>
    /// Return a unique instance of <see cref="EnumConverter"/>.
    /// </summary>
    public static readonly EnumConverter Any = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter == null || value == null)
        {
            return false;
        }

        var val = parameter is IEnumerable parameters
            ? parameters.Cast<object>().Any(parameter2 =>
                System.Convert.ToInt32(parameter2, culture) == System.Convert.ToInt32(value, culture))
            : System.Convert.ToInt32(parameter, culture) == System.Convert.ToInt32(value, culture);

        return val;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var val = value != null && (parameter == null || !(bool)value)
            ? AvaloniaProperty.UnsetValue
            : parameter is IEnumerable parameters ? parameters.Cast<object>().FirstOrDefault() : parameter;

        return val;
    }
}
