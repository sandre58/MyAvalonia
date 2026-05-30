// -----------------------------------------------------------------------
// <copyright file="KeyGesturesConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Resolves platform-standard keyboard shortcuts from the <see cref="ConverterParameter"/>.
/// </summary>
/// <remarks>
/// Pass one of: <c>copy</c>, <c>cut</c>, <c>paste</c>, <c>selectall</c>, <c>undo</c>, <c>redo</c>.
/// Reads hotkeys from <see cref="Application.Current"/> at conversion time.
/// </remarks>
/// <example>
/// <code>
/// &lt;MenuItem InputGesture="{Binding ., Converter={x:Static my:KeyGesturesConverter.Default}, ConverterParameter=copy}" /&gt;
/// </code>
/// </example>
public class KeyGesturesConverter : IValueConverter
{
    /// <summary>
    /// Gets the default singleton instance.
    /// </summary>
    public static readonly KeyGesturesConverter Default = new();

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not string kind)
            return null;

        var config = Application.Current?.PlatformSettings?.HotkeyConfiguration;
        if (config is null)
            return null;

        var gestures = kind switch
        {
            "copy" => config.Copy,
            "cut" => config.Cut,
            "paste" => config.Paste,
            "selectall" => config.SelectAll,
            "undo" => config.Undo,
            "redo" => config.Redo,
            _ => null
        };

        return gestures?.FirstOrDefault();
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
