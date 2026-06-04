// -----------------------------------------------------------------------
// <copyright file="PlaygroundTestHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
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

            RunOnStaThread(() => AppBuilder.Configure<ShowcaseTestApp>()
                    .UseHeadless(new())
                    .SetupWithoutStarting());

            _avaloniaInitialized = true;
        }
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "STA thread is required for Avalonia initialization.")]
    private static void RunOnStaThread(Action action)
    {
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                error = ex;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (error is not null)
            throw error;
    }
}
