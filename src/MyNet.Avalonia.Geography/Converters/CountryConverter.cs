// -----------------------------------------------------------------------
// <copyright file="CountryConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using MyNet.Geography;
using MyNet.Geography.Resources;
using MyNet.Humanizer.Facade;

namespace MyNet.Avalonia.Geography.Converters;

/// <summary>
/// Converts a <see cref="Country"/> or <see cref="CultureInfo"/> value to country codes, display names, or flag bitmaps.
/// </summary>
public sealed class CountryConverter : IValueConverter
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
        To128,
    }

    /// <summary>Gets a converter that returns the ISO 3166-1 alpha-2 code in upper case.</summary>
    public static CountryConverter ToAlpha2 { get; } = new(Mode.Alpha2);

    /// <summary>Gets a converter that returns the ISO 3166-1 alpha-3 code in upper case.</summary>
    public static CountryConverter ToAlpha3 { get; } = new(Mode.Alpha3);

    /// <summary>Gets a converter that returns the localized country display name.</summary>
    public static CountryConverter ToDisplayName { get; } = new(Mode.DisplayName);

    /// <summary>Gets a converter that returns the numeric ISO code.</summary>
    public static CountryConverter ToIso { get; } = new(Mode.Iso);

    /// <summary>Gets a converter that returns a 16×16 flag bitmap.</summary>
    public static CountryConverter To16 { get; } = new(Mode.To16);

    /// <summary>Gets a converter that returns a 24×24 flag bitmap.</summary>
    public static CountryConverter To24 { get; } = new(Mode.To24);

    /// <summary>Gets a converter that returns a 32×32 flag bitmap.</summary>
    public static CountryConverter To32 { get; } = new(Mode.To32);

    /// <summary>Gets a converter that returns a 48×48 flag bitmap.</summary>
    public static CountryConverter To48 { get; } = new(Mode.To48);

    /// <summary>Gets a converter that returns a 64×64 flag bitmap.</summary>
    public static CountryConverter To64 { get; } = new(Mode.To64);

    /// <summary>Gets a converter that returns a 128×128 flag bitmap.</summary>
    public static CountryConverter To128 { get; } = new(Mode.To128);

    private readonly Mode _mode;

    private CountryConverter(Mode mode) => _mode = mode;

    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => ResolveCountry(value) is not { } country
        ? AvaloniaProperty.UnsetValue
        : _mode switch
        {
            Mode.Alpha2 => country.Alpha2.ToUpperInvariant(),
            Mode.Alpha3 => country.Alpha3.ToUpperInvariant(),
            Mode.DisplayName => country.Humanize(),
            Mode.Iso => country.Iso.ToString(culture),
            Mode.To16 => GetFlag(country, FlagSize.Pixel16),
            Mode.To24 => GetFlag(country, FlagSize.Pixel24),
            Mode.To32 => GetFlag(country, FlagSize.Pixel32),
            Mode.To48 => GetFlag(country, FlagSize.Pixel48),
            Mode.To64 => GetFlag(country, FlagSize.Pixel64),
            Mode.To128 => GetFlag(country, FlagSize.Pixel128),
            _ => AvaloniaProperty.UnsetValue,
        };

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => AvaloniaProperty.UnsetValue;

    private static Country? ResolveCountry(object? value) => value switch
    {
        Country country => country,
        CultureInfo cultureInfo => cultureInfo.GetCountry(),
        _ => null,
    };

    private static Bitmap GetFlag(Country country, FlagSize size)
    {
        using var memoryStream = country.GetFlag(size);
        return new(memoryStream);
    }
}
