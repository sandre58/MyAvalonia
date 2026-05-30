// -----------------------------------------------------------------------
// <copyright file="CountryConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Geography;
using MyNet.Utilities.Geography.Extensions;

namespace MyNet.Avalonia.Showcase.Converters;

internal sealed class CountryConverter : IValueConverter
{
    private enum Mode
    {
        Alpha2,

        Alpha3,

        DisplayName,

        Iso,

        To16,

        To24,

        To32,

        To48,

        To64,

        To128
    }

    private readonly Mode _mode;

    private CountryConverter(Mode mode) => _mode = mode;

    public static CountryConverter ToAlpha2 { get; } = new(Mode.Alpha2);

    public static CountryConverter ToAlpha3 { get; } = new(Mode.Alpha3);

    public static CountryConverter ToDisplayName { get; } = new(Mode.DisplayName);

    public static CountryConverter ToIso { get; } = new(Mode.Iso);

    public static CountryConverter To16 { get; } = new(Mode.To16);

    public static CountryConverter To24 { get; } = new(Mode.To24);

    public static CountryConverter To32 { get; } = new(Mode.To32);

    public static CountryConverter To48 { get; } = new(Mode.To48);

    public static CountryConverter To64 { get; } = new(Mode.To64);

    public static CountryConverter To128 { get; } = new(Mode.To128);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => GetCountry(value) is not { } country
        ? AvaloniaProperty.UnsetValue
        : _mode switch
        {
            Mode.Alpha2 => country.Alpha2.ApplyCase(LetterCasing.AllCaps),
            Mode.Alpha3 => country.Alpha3.ApplyCase(LetterCasing.AllCaps),
            Mode.DisplayName => country.GetDisplayName(),
            Mode.Iso => country.Iso.ToString(culture),
            Mode.To16 => GetFlag(country, FlagSize.Pixel16),
            Mode.To24 => GetFlag(country, FlagSize.Pixel24),
            Mode.To32 => GetFlag(country, FlagSize.Pixel32),
            Mode.To48 => GetFlag(country, FlagSize.Pixel48),
            Mode.To64 => GetFlag(country, FlagSize.Pixel64),
            Mode.To128 => GetFlag(country, FlagSize.Pixel128),
            _ => AvaloniaProperty.UnsetValue
        };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new InvalidOperationException();

    private static Country? GetCountry(object? value) => value is CultureInfo culture ? culture.GetCountry() : value as Country;

    private static Bitmap? GetFlag(Country country, FlagSize size)
    {
        if (country.GetFlag(size) is not { } flag) return null;
        using var memoryStream = new MemoryStream(flag);
        return new(memoryStream);
    }
}
