// -----------------------------------------------------------------------
// <copyright file="NavigationMenuIconWidthConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Theme.Converters.Internals;

internal sealed class NavigationMenuIconWidthConverter : IMultiValueConverter
{
    public static NavigationMenuIconWidthConverter Default { get; } = new();

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        => values[0] is double collapsedWidth && values[1] is Thickness padding && values[2] is Thickness margin
            ? collapsedWidth - padding.Left - padding.Right - margin.Left - margin.Right
            : AvaloniaProperty.UnsetValue;
}
