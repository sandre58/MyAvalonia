// -----------------------------------------------------------------------
// <copyright file="GlobalizationTestFixture.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Geography;
using MyNet.Globalization;
using MyNet.Globalization.Culture;
using MyNet.Humanizer;

namespace MyNet.Avalonia.Controls.Tests.Infrastructure;

/// <summary>
/// Initializes MyNet globalization services required by Humanizer-based helpers (e.g. <see cref="Icons.MaterialIconCatalog"/>).
/// </summary>
public sealed class GlobalizationTestFixture : IDisposable
{
    private readonly ServiceProvider _services;

    public GlobalizationTestFixture()
    {
        var collection = new ServiceCollection();
        collection.AddGlobalization()
            .AddLocalization()
            .AddInflection()
            .AddHumanizer()
            .AddGeographyLocalization();

        _services = collection.BuildServiceProvider();
        _services.UseGlobalization();
        _services.UseLocalization();
        _services.UseDisplayText();
    }

    public void SetCulture(CultureInfo culture) =>
        _services.GetRequiredService<ICultureService>().SetCulture(culture);

    public void SetFrenchCulture() => SetCulture(SupportedCultures.French);

    public void SetEnglishCulture() => SetCulture(SupportedCultures.English);

    public void Dispose() => _services.Dispose();
}
