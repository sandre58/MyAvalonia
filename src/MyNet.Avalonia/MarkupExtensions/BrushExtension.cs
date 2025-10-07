// -----------------------------------------------------------------------
// <copyright file="BrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using MyNet.Avalonia.Converters;
using MyNet.Utilities;

namespace MyNet.Avalonia.MarkupExtensions;

public class BrushExtension(string path) : MarkupExtension
{
    [ConstructorArgument("path")]
    public string Path { get; set; } = path;

    public double? Opacity { get; set; }

    public bool? Contrast { get; set; }

    public double? Darken { get; set; }

    public double? Lighten { get; set; }

    public RelativeSource? RelativeSource { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var converter = BrushConverter.Default;
        object? converterParameter = null;

        if (Opacity.HasValue && Contrast.IsTrue())
        {
            converter = BrushConverter.ContrastAndOpacity;
            converterParameter = Opacity.Value;
        }
        else if (Opacity.HasValue)
        {
            converter = BrushConverter.Opacity;
            converterParameter = Opacity.Value;
        }
        else if (Contrast.HasValue)
        {
            converter = BrushConverter.Contrast;
        }
        else if (Darken.HasValue)
        {
            converter = BrushConverter.Darken;
        }
        else if (Lighten.HasValue)
        {
            converter = BrushConverter.Lighten;
        }

        return new ReflectionBindingExtension(Path)
        {
            RelativeSource = RelativeSource,
            Converter = converter,
            ConverterParameter = converterParameter
        }.ProvideValue(serviceProvider);
    }
}
