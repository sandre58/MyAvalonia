// -----------------------------------------------------------------------
// <copyright file="PositionToAngleConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Theme.Converters;

public class PositionToAngleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is double d ? d * 3.6 : (object)0;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value is double d ? d / 3.6 : (object)0;
}
