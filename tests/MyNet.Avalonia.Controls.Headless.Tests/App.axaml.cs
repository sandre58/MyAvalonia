// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Globalization;
using MyNet.Humanizer;

namespace MyNet.Avalonia.Controls.Headless.Tests;

[SuppressMessage("Maintainability", "CA1515:Envisager de rendre les types publics internes", Justification = "Used by Avalonia XAML")]
[SuppressMessage("ReSharper", "PartialTypeWithSinglePart", Justification = "Used by Avalonia XAML")]
public partial class HeadlessTestApp : Application
{
    private static bool _globalizationInitialized;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        EnsureGlobalizationServices();
        base.OnFrameworkInitializationCompleted();
    }

    internal static void EnsureGlobalizationServices()
    {
        if (_globalizationInitialized) return;

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
}
