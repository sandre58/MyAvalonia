// -----------------------------------------------------------------------
// <copyright file="IconConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Extensions;

namespace MyNet.Avalonia.Theme.Converters;

/// <summary>
/// Provides a value converter that converts between Geometry or IconData and Icon based on the specified conversion
/// mode.
/// </summary>
/// <remarks>This class supports two modes of conversion: Mode.FromGeometry for converting Geometry to Icon, and
/// Mode.FromIconData for converting IconData to Icon. The mode is specified at the time of instantiation, allowing for
/// flexible conversion based on the input type. The ConvertBack method is not supported and always returns
/// AvaloniaProperty.UnsetValue.</remarks>
public sealed class IconConverter : IValueConverter
{
    /// <summary>
    /// Private enumeration to specify the conversion mode for the IconConverter. The Mode enum defines two values: FromGeometry, which indicates that the converter will convert a Geometry to an Icon, and FromIconData, which indicates that the converter will convert an IconData to an Icon. This enum is used internally to determine how the Convert method processes the input value based on the specified mode of conversion.
    /// </summary>
    private enum Mode
    {
        FromGeometry,

        FromIconData,

        ToGeometry
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IconConverter"/> class that converts a <see cref="Geometry"/> to an <see cref="Icon"/>. This converter is designed to take a Geometry object as input and produce an Icon object as output, allowing for the use of vector graphics in icon representations within the Avalonia UI framework. The conversion logic is implemented in the Convert method, which checks the input type and performs the appropriate conversion based on the specified mode.
    /// </summary>
    public static readonly IconConverter FromGeometry = new(Mode.FromGeometry);

    /// <summary>
    /// Initializes a new instance of the <see cref="IconConverter"/> class that converts an <see cref="IconData"/> to an <see cref="Icon"/>. This converter is designed to take an IconData object as input and produce an Icon object as output, allowing for the use of predefined icon data in icon representations within the Avalonia UI framework. The conversion logic is implemented in the Convert method, which checks the input type and performs the appropriate conversion based on the specified mode.
    /// </summary>
    public static readonly IconConverter FromIconData = new(Mode.FromIconData);

    /// <summary>
    /// Initializes a new instance of the <see cref="IconConverter"/> class that converts an <see cref="IconData"/> to a <see cref="Geometry"/>. This converter is designed to take an IconData object as input and produce a Geometry object as output, allowing for the use of predefined icon data in vector graphic representations within the Avalonia UI framework. The conversion logic is implemented in the Convert method, which checks the input type and performs the appropriate conversion based on the specified mode.
    /// </summary>
    public static readonly IconConverter ToGeometry = new(Mode.ToGeometry);

    private readonly Mode _mode;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconConverter"/> class with the specified mode. The mode determines how the converter will convert the input value to an <see cref="Icon"/>. If the mode is <see cref="Mode.FromGeometry"/>, the converter will expect a <see cref="Geometry"/> as input and convert it to an <see cref="Icon"/>. If the mode is <see cref="Mode.FromIconData"/>, the converter will expect an <see cref="IconData"/> as input and convert it to an <see cref="Icon"/>. This design allows for flexible conversion based on different types of input data while maintaining a clear separation of conversion logic based on the specified mode.
    /// </summary>
    /// <param name="mode">The mode that determines how the converter will process the input value.</param>
    private IconConverter(Mode mode) => _mode = mode;

    /// <summary>
    /// Converts a <see cref="Geometry"/> or <see cref="IconData"/> to an <see cref="Icon"/> based on the specified mode.
    /// </summary>
    /// <param name="value">The value to be converted, either a <see cref="Geometry"/> or <see cref="IconData"/>.</param>
    /// <param name="targetType">The target type of the conversion. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter for the conversion. This parameter is not used.</param>
    /// <param name="culture">The culture information for the conversion. This parameter is not used.</param>
    /// <returns>The converted <see cref="Icon"/> if the input is valid; otherwise, <see cref="AvaloniaProperty.UnsetValue"/>.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => _mode switch
            {
                Mode.FromGeometry => value is Geometry geometry ? geometry.ToIcon() : AvaloniaProperty.UnsetValue,
                Mode.FromIconData => value is IconData iconData ? iconData.ToIcon() : AvaloniaProperty.UnsetValue,
                Mode.ToGeometry => value is IconData iconData ? iconData.ToGeometry() : AvaloniaProperty.UnsetValue,
                _ => AvaloniaProperty.UnsetValue
            };

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
