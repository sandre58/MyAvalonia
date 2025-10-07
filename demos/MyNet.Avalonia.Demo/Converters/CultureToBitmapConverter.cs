// -----------------------------------------------------------------------
// <copyright file="CultureToBitmapConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using MyNet.Utilities.Localization.Extensions;

namespace MyNet.Avalonia.Demo.Converters;

public class CultureToBitmapConverter : IValueConverter
{
    public static CultureToBitmapConverter Default { get; } = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not CultureInfo cultureInfo || cultureInfo.GetImage() is not { } flag) return AvaloniaProperty.UnsetValue;
        using var memoryStream = new MemoryStream(flag);
        return new Bitmap(memoryStream);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new InvalidOperationException();
}
