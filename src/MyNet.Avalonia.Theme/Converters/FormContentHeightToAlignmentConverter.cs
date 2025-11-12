// -----------------------------------------------------------------------
// <copyright file="FormContentHeightToAlignmentConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace MyNet.Avalonia.Theme.Converters;

internal sealed class FormContentHeightToAlignmentConverter : IValueConverter
{
    public static FormContentHeightToAlignmentConverter Default { get; } = new(32);

    public double Threshold { get; set; }

    public FormContentHeightToAlignmentConverter() => Threshold = 32;

    // ReSharper disable once ConvertToPrimaryConstructor
    // Justification: need to keep the default constructor for XAML
    public FormContentHeightToAlignmentConverter(double threshold) => Threshold = threshold;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is not double d ? VerticalAlignment.Center : d > Threshold ? VerticalAlignment.Top : VerticalAlignment.Center;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
