// -----------------------------------------------------------------------
// <copyright file="CountryToBitmapConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using MyNet.Utilities.Geography;
using MyNet.Utilities.Geography.Extensions;

namespace MyNet.Avalonia.Demo.Converters;

public class CountryToBitmapConverter(FlagSize size) : IValueConverter
{
    public static CountryToBitmapConverter To16 { get; } = new(FlagSize.Pixel16);

    public static CountryToBitmapConverter To24 { get; } = new(FlagSize.Pixel24);

    public static CountryToBitmapConverter To32 { get; } = new(FlagSize.Pixel32);

    public static CountryToBitmapConverter To48 { get; } = new(FlagSize.Pixel48);

    public static CountryToBitmapConverter To64 { get; } = new(FlagSize.Pixel64);

    public static CountryToBitmapConverter To128 { get; } = new(FlagSize.Pixel128);

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Country country || country.GetFlag(size) is not { } flag) return AvaloniaProperty.UnsetValue;
        using var memoryStream = new MemoryStream(flag);
        return new Bitmap(memoryStream);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new InvalidOperationException();
}
