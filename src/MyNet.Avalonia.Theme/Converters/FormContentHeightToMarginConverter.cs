// -----------------------------------------------------------------------
// <copyright file="FormContentHeightToMarginConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Theme.Converters;

public class FormContentHeightToMarginConverter : IValueConverter
{
    public static FormContentHeightToMarginConverter Default { get; } = new();

    public double Threshold { get; set; }

    public FormContentHeightToMarginConverter() => Threshold = 32;

    // ReSharper disable once ConvertToPrimaryConstructor
    // Justification: need to keep the default constructor for XAML
    public FormContentHeightToMarginConverter(double threshold) => Threshold = threshold;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is not double d ? new Thickness(0) : d > Threshold ? new Thickness(0, 8, 8, 0) : new Thickness(0, 0, 8, 0);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
