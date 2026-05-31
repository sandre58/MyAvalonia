// -----------------------------------------------------------------------
// <copyright file="GlobalizationTestFixture.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Globalization;
using MyNet.Humanizer;

namespace MyNet.Avalonia.Controls.Tests.Infrastructure;

/// <summary>
/// Initializes MyNet globalization services required by Humanizer-based helpers (e.g. <see cref="Helpers.IconsHelper"/>).
/// </summary>
internal sealed class GlobalizationTestFixture : IDisposable
{
    private readonly ServiceProvider _services;

    public GlobalizationTestFixture()
    {
        var collection = new ServiceCollection();
        collection.AddGlobalization()
            .AddLocalization()
            .AddInflection()
            .AddHumanizer();

        _services = collection.BuildServiceProvider();
        _services.UseGlobalization();
        _services.UseLocalization();
        _services.UseDisplayText();
    }

    public void Dispose() => _services.Dispose();
}
