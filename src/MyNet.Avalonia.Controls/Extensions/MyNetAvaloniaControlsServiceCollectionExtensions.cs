// -----------------------------------------------------------------------
// <copyright file="MyNetAvaloniaControlsServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Resources;
using MyNet.Avalonia.Converters;
using MyNet.Globalization;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Registers MyNet.Avalonia.Controls translation resources and converter hooks.
/// </summary>
public static class MyNetAvaloniaControlsServiceCollectionExtensions
{
    private static int _convertersRegistered;

    /// <summary>
    /// Contributes controls translation resources and registers Avalonia-specific string converters.
    /// </summary>
    public static IServiceCollection AddMyNetAvaloniaControls(this IServiceCollection services)
    {
        services.AddTranslationResource(nameof(ColorPickerResources), ColorPickerResources.ResourceManager);
        services.AddTranslationResource(nameof(MessagesResources), MessagesResources.ResourceManager);

        if (Interlocked.Exchange(ref _convertersRegistered, 1) == 0)
        {
            StringConverter.RegisterTypeConverter<DateContext>((dateContext, format, _, _, culture) => !string.IsNullOrEmpty(format)
                ? DateTimeConverter.Default.Convert(dateContext.ToDate(), format, culture)?.ToString()
                : dateContext.ToString());
        }

        return services;
    }
}
