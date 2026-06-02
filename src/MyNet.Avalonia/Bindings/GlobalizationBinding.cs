// -----------------------------------------------------------------------
// <copyright file="GlobalizationBinding.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Bindings;

/// <summary>
/// Factory for multi-bindings that re-evaluate when application culture or time zone changes.
/// </summary>
public static class GlobalizationBinding
{
    /// <summary>
    /// Creates a multi-binding with the value binding, converter, and optional culture/time zone sources.
    /// </summary>
    public static MultiBinding Create(
        BindingBase valueBinding,
        IMultiValueConverter converter,
        object? converterParameter = null,
        bool includeCulture = true,
        bool includeTimeZone = false)
    {
        var multiBinding = new MultiBinding
        {
            Converter = converter,
            ConverterParameter = converterParameter,
            Mode = BindingMode.OneWay
        };

        multiBinding.Bindings.Add(valueBinding);

        if (includeCulture)
            multiBinding.Bindings.Add(CreateCultureBinding());

        if (includeTimeZone)
            multiBinding.Bindings.Add(CreateTimeZoneBinding());

        return multiBinding;
    }

    /// <summary>
    /// Creates a binding to <see cref="GlobalizationBindingSource.Culture"/>.
    /// </summary>
    public static BindingBase CreateCultureBinding() => new ReflectionBinding(nameof(GlobalizationBindingSource.Culture))
    {
        Source = GlobalizationBindingSource.Instance,
        Mode = BindingMode.OneWay
    };

    /// <summary>
    /// Creates a binding to <see cref="GlobalizationBindingSource.TimeZone"/>.
    /// </summary>
    public static BindingBase CreateTimeZoneBinding() => new ReflectionBinding(nameof(GlobalizationBindingSource.TimeZone))
    {
        Source = GlobalizationBindingSource.Instance,
        Mode = BindingMode.OneWay
    };
}
