// -----------------------------------------------------------------------
// <copyright file="ProgressBarIntermediateOffsetConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Theme.Converters;

internal sealed class ProgressBarIntermediateOffsetConverter : IValueConverter
{
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => (double)value! * System.Convert.ToDouble(parameter, NumberFormatInfo.InvariantInfo);

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
