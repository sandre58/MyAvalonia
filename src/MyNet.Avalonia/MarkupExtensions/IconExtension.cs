// -----------------------------------------------------------------------
// <copyright file="IconExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using MyNet.Avalonia.Enums;
using MyNet.Utilities;

namespace MyNet.Avalonia.MarkupExtensions;

public class IconExtension : MarkupExtension
{
    private static string _geometryPattern = string.Empty;

    public static void Initialize(string geometryPattern) => _geometryPattern = geometryPattern;

    public IconExtension(string data) => Data = data;

    public IconExtension(string data, IconSize size)
    {
        Data = data;
        DefinedSize = size;
    }

    [ConstructorArgument("data")]
    public string Data { get; set; }

    [ConstructorArgument("size")]
    public IconSize DefinedSize { get; set; } = IconSize.Default;

    public double? Size { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var result = new PathIcon
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Focusable = false,
            Opacity = 1
        };

        var data = new StaticResourceExtension(!string.IsNullOrEmpty(_geometryPattern) ? _geometryPattern.FormatWith(Data) : Data).ProvideValue(serviceProvider);
        _ = result.SetValue(PathIcon.DataProperty, data);
        if (Size.HasValue)
        {
            result.Width = Size.Value;
            result.Height = Size.Value;
        }
        else
        {
            result.Classes.Add(DefinedSize.ToString());
        }

        return result;
    }
}
