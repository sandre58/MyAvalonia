// -----------------------------------------------------------------------
// <copyright file="CountryToStringConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MyNet.Humanizer;
using MyNet.Utilities.Geography;
using MyNet.Utilities.Geography.Extensions;

namespace MyNet.Avalonia.Demo.Converters;

public class CountryToStringConverter : IValueConverter
{
    private enum Display
    {
        Alpha2,

        Alpha3,

        DisplayName,

        Iso
    }

    private readonly Display _display;

    public static CountryToStringConverter ToAlpha2 { get; } = new(Display.Alpha2);

    public static CountryToStringConverter ToAlpha3 { get; } = new(Display.Alpha3);

    public static CountryToStringConverter ToDisplayName { get; } = new(Display.DisplayName);

    public static CountryToStringConverter ToIso { get; } = new(Display.Iso);

    private CountryToStringConverter(Display display) => _display = display;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is not Country country
        ? string.Empty
        : (object)(_display switch
        {
            Display.Alpha2 => country.Alpha2.ApplyCase(LetterCasing.AllCaps),
            Display.Alpha3 => country.Alpha3.ApplyCase(LetterCasing.AllCaps),
            Display.DisplayName => country.GetDisplayName(),
            Display.Iso => country.Iso.ToString(culture),
            _ => string.Empty
        });

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new InvalidOperationException();
}
