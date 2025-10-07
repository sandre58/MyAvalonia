// -----------------------------------------------------------------------
// <copyright file="NullFallbackConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;
using MyNet.Utilities;

namespace MyNet.Avalonia.Converters;

public class NullFallbackConverter : IValueConverter, IMultiValueConverter
{
    public static NullFallbackConverter Default => new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => GetValueOrFallback([value, parameter]);

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => GetValueOrFallback(values);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => GetValueOrFallback([value, parameter]);

    private static object GetValueOrFallback(IEnumerable<object?> values) => values.NotNull().FirstOrDefault() ?? BindingOperations.DoNothing;
}
