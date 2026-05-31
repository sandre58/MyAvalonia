// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.UI.Theming;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Theming;
#pragma warning restore IDE0130

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Avalonia theme services.
    /// </summary>
    /// <remarks>
    /// Requires an <see cref="IThemeBrushService"/> to be registered, typically as <c>MyTheme.Current</c>.
    /// </remarks>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAvaloniaTheming(this IServiceCollection services)
    {
        services.TryAddSingleton<IThemeBaseRegistry, ThemeVariantsRegistry>();
        services.TryAddSingleton<IThemeService, ThemeService>();
        return services;
    }
}
