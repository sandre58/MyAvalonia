// -----------------------------------------------------------------------
// <copyright file="ContentConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;
using MyNet.Avalonia.Theme.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Converters;

internal sealed class ContentConverter : IValueConverter
{
    public static readonly ContentConverter Default = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ContentProviderType contentType;
        string? defaultText = null;
        if (value is ControlDefinition controlDefinition)
        {
            contentType = controlDefinition.DefaultContentType;
            defaultText = controlDefinition.DisplayName;
        }
        else if (value is ContentProviderType contentProviderType)
        {
            contentType = contentProviderType;
        }
        else
        {
            return AvaloniaProperty.UnsetValue;
        }

        return contentType switch
        {
            ContentProviderType.Icon => RandomGenerator.Enum<IconData>().ToIcon(),
            ContentProviderType.Text => !string.IsNullOrEmpty(defaultText) ? defaultText : "Preview",
            _ => null
        };
    }

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
