// -----------------------------------------------------------------------
// <copyright file="GenderTypeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Converters;

public sealed class GenderTypeConverter : IValueConverter
{
    private enum Mode
    {
        Brush,

        Icon
    }

    public static readonly GenderTypeConverter Brush = new(Mode.Brush);
    public static readonly GenderTypeConverter Icon = new(Mode.Icon);

    private readonly Mode _mode;

    private GenderTypeConverter(Mode mode) => _mode = mode;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is not GenderType genderType
            ? AvaloniaProperty.UnsetValue
            : _mode switch
            {
                Mode.Brush => ThemeResources.GetBrush(genderType.ToString()),
                Mode.Icon => Enum.TryParse<IconData>($"Gender{genderType}", out var iconData) ? iconData.ToIcon() : IconData.GenderMaleFemale.ToIcon(),
                _ => AvaloniaProperty.UnsetValue,
            };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => AvaloniaProperty.UnsetValue;
}
