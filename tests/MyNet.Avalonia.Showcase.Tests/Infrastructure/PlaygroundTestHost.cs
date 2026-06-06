// -----------------------------------------------------------------------
// <copyright file="PlaygroundTestHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using Avalonia;
using Avalonia.Headless;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Globalization;
using MyNet.Humanizer;

namespace MyNet.Avalonia.Showcase.Tests.Infrastructure;

internal static class PlaygroundTestHost
{
    private static readonly Lock Sync = new();
    private static bool _globalizationInitialized;
    private static bool _avaloniaInitialized;

    public static void EnsureInitialized()
    {
        lock (Sync)
        {
            if (!_globalizationInitialized)
            {
                var services = new ServiceCollection()
                    .AddGlobalization()
                    .AddLocalization()
                    .AddInflection()
                    .AddHumanizer()
                    .BuildServiceProvider();

                services.UseGlobalization();
                services.UseLocalization();
                services.UseDisplayText();
                _globalizationInitialized = true;
            }

            if (_avaloniaInitialized)
                return;

            if (Application.Current is not null)
            {
                _avaloniaInitialized = true;
                return;
            }

            AppBuilder.Configure<ShowcaseTestApp>()
                    .UseHeadless(new())
                    .SetupWithoutStarting();

            _avaloniaInitialized = true;
        }
    }
}
