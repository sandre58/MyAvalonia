// -----------------------------------------------------------------------
// <copyright file="ThemeBindingHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Data;
using Avalonia.Data.Converters;
using MyNet.Avalonia.Converters;

namespace MyNet.Avalonia.Theme.MarkupExtensions.Helpers;

/// <summary>
/// Creates standard <see cref="Binding"/> instances for theme markup extensions (avoids <see cref="ReflectionBinding"/>).
/// </summary>
internal static class ThemeBindingHelper
{
    public static Binding Create(string path, RelativeSource relativeSource, IServiceProvider? serviceProvider = null)
        => new(path)
        {
            Mode = BindingMode.OneWay,
            RelativeSource = relativeSource,
            TypeResolver = serviceProvider is null ? null : (x, y) => ThemeBrushExtensionBase.ResolveType(serviceProvider, x, y)
        };

    public static Binding CreateParentForeground(IServiceProvider serviceProvider)
        => Create("Parent.(TextElement.Foreground)", new(RelativeSourceMode.Self), serviceProvider);

    public static Binding CreateConstantSource(object source)
        => new()
        {
            Mode = BindingMode.OneTime,
            Source = source
        };

    /// <summary>
    /// Binds to <see cref="MyTheme.ThemeVersion"/> so converters re-run after palette updates.
    /// </summary>
    public static Binding CreateThemeVersion()
        => new(nameof(MyTheme.ThemeVersion))
        {
            Mode = BindingMode.OneWay,
            Source = MyTheme.Current
        };

    /// <summary>
    /// Adds the brush path binding and a <see cref="MyTheme.ThemeVersion"/> trigger to a <see cref="MultiBinding"/>.
    /// </summary>
    public static void AddBrushSourceAndThemeVersion(
        MultiBinding multiBinding,
        string path,
        RelativeSource relativeSource,
        IServiceProvider serviceProvider)
    {
        multiBinding.Bindings.Add(Create(path, relativeSource, serviceProvider));
        multiBinding.Bindings.Add(CreateThemeVersion());
    }

    /// <summary>
    /// Binds to an ancestor <see cref="TextElement.FontSize"/> scaled for watermark/helper text.
    /// </summary>
    public static Binding CreateScaledAncestorFontSize(double scaleFactor)
        => CreateAncestorBinding(
            "(TextElement.FontSize)",
            typeof(Control),
            ResolveTextElementType,
            MathConverter.Multiply,
            scaleFactor);

    public static Binding CreateAncestorBinding(
        string path,
        Type ancestorType,
        Func<string?, string, Type>? typeResolver,
        IValueConverter? converter = null,
        object? converterParameter = null)
        => new(path)
        {
            Mode = BindingMode.OneWay,
            RelativeSource = new(RelativeSourceMode.FindAncestor) { AncestorType = ancestorType },
            TypeResolver = typeResolver,
            Converter = converter,
            ConverterParameter = converterParameter
        };

    private static Type ResolveTextElementType(string? @namespace, string typeName)
        => typeName switch
        {
            nameof(TextElement) => typeof(TextElement),
            _ => throw new InvalidOperationException($"Cannot resolve type '{typeName}'")
        };
}
