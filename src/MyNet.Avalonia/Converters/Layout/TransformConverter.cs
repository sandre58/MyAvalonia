// -----------------------------------------------------------------------
// <copyright file="TransformConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Builds Avalonia <see cref="Transform"/> objects from numeric binding values.
/// </summary>
/// <remarks>
/// Single-value instances create <see cref="ScaleTransform"/> or <see cref="TranslateTransform"/>.
/// <see cref="Group"/> (multi-binding) combines scale and translate from up to four double values:
/// scaleX, scaleY, translateX, translateY (missing values are skipped).
/// </remarks>
public sealed class TransformConverter : IValueConverter, IMultiValueConverter
{
    private enum Mode
    {
        Scale,

        ScaleY,

        ScaleX,

        Translate,

        TranslateX,

        TranslateY
    }

    /// <summary>Scales uniformly on X and Y from a single double.</summary>
    public static readonly TransformConverter Scale = new(Mode.Scale);

    /// <summary>Scales on the X axis only.</summary>
    public static readonly TransformConverter ScaleX = new(Mode.ScaleX);

    /// <summary>Scales on the Y axis only.</summary>
    public static readonly TransformConverter ScaleY = new(Mode.ScaleY);

    /// <summary>Translates uniformly on X and Y from a single double.</summary>
    public static readonly TransformConverter Translate = new(Mode.Translate);

    /// <summary>Translates on the X axis only.</summary>
    public static readonly TransformConverter TranslateX = new(Mode.TranslateX);

    /// <summary>Translates on the Y axis only.</summary>
    public static readonly TransformConverter TranslateY = new(Mode.TranslateY);

    /// <summary>
    /// Gets a multi-value converter that builds a <see cref="TransformGroup"/> from scale and translate components.
    /// </summary>
    public static readonly TransformConverter Group = new();

    private readonly Mode? _mode;

    private TransformConverter(Mode? transformType = null) => _mode = transformType;

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is double val ? _mode switch
    {
        Mode.Scale => new ScaleTransform { ScaleX = val, ScaleY = val },
        Mode.ScaleX => new ScaleTransform { ScaleX = val },
        Mode.ScaleY => new ScaleTransform { ScaleY = val },
        Mode.Translate => new TranslateTransform { X = val, Y = val },
        Mode.TranslateX => new TranslateTransform { X = val },
        Mode.TranslateY => new TranslateTransform { Y = val },
        _ => AvaloniaProperty.UnsetValue
    } : AvaloniaProperty.UnsetValue;

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var scaleX = getValue(values, 0);
        var scaleY = getValue(values, 1);
        var translateX = getValue(values, 2);
        var translateY = getValue(values, 3);

        var transformGroup = new TransformGroup();

        if (scaleX.HasValue || scaleY.HasValue)
        {
            var scaleTransform = new ScaleTransform();
            if (scaleX.HasValue)
                scaleTransform.ScaleX = scaleX.Value;
            if (scaleY.HasValue)
                scaleTransform.ScaleY = scaleY.Value;
            transformGroup.Children.Add(scaleTransform);
        }

        if (!translateX.HasValue && !translateY.HasValue)
            return transformGroup.Children.Count > 0 ? transformGroup : AvaloniaProperty.UnsetValue;
        var translateTransform = new TranslateTransform();
        if (translateX.HasValue)
            translateTransform.X = translateX.Value;
        if (translateY.HasValue)
            translateTransform.Y = translateY.Value;
        transformGroup.Children.Add(translateTransform);

        return transformGroup.Children.Count > 0 ? transformGroup : AvaloniaProperty.UnsetValue;

        static double? getValue(IList<object?> values, int index) => values.Count > index && values[index] is double val ? val : null;
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new InvalidOperationException();
}
