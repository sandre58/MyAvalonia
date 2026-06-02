// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.Avalonia.Clipboard;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Clipboard;
#pragma warning restore IDE0130

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers Avalonia clipboard services.
        /// </summary>
        /// <remarks>
        /// When <paramref name="enableFeedback"/> is true, requires <see cref="MyNet.UI.Notifications.INotificationPublisher"/>
        /// (typically via <c>AddNotifications()</c>).
        /// Call <c>UseClipboard()</c> on the built service provider to wire <see cref="ClipboardManager"/>.
        /// </remarks>
        /// <param name="topLevelProvider">Resolves the host top level, typically the main window.</param>
        /// <param name="enableFeedback">When true, publishes notification feedback after copy operations.</param>
        /// <returns>The same service collection for chaining.</returns>
        public IServiceCollection AddAvaloniaClipboard(Func<TopLevel?> topLevelProvider, bool enableFeedback = true)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(topLevelProvider);

            if (enableFeedback)
                services.TryAddSingleton<IClipboardFeedback, ToastClipboardFeedback>();

            services.TryAddSingleton<IClipboardService>(sp =>
            {
                var feedback = enableFeedback ? sp.GetRequiredService<IClipboardFeedback>() : null;
                return new ClipboardService(topLevelProvider, feedback);
            });

            return services;
        }
    }
}
