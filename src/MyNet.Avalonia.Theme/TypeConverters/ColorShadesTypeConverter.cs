// -----------------------------------------------------------------------
// <copyright file="ColorShadesTypeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Theming.Palettes;

namespace MyNet.Avalonia.Theme.TypeConverters;

#region ColorShadesTypeConverter

/// <summary>
/// TypeConverter for ColorShades, enabling conversion from hex color strings in XAML.
/// Supports formats like "#124378" or "#FF124378".
/// </summary>
public class ColorShadesTypeConverter : TypeConverter
{
    /// <summary>
    /// Returns whether this converter can convert from the specified source type.
    /// </summary>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <summary>
    /// Converts from a hex color string to a ColorShades instance.
    /// </summary>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string colorString)
        {
            try
            {
                var color = Color.Parse(colorString);
                return new ColorShades(color);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Cannot convert '{colorString}' to ColorShades. Expected a valid hex color format (e.g., '#124378' or '#FF124378').", ex);
            }
        }

        return base.ConvertFrom(context, culture, value);
    }
}

#endregion
