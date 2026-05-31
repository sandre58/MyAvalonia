// -----------------------------------------------------------------------
// <copyright file="TestAppBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Headless;
using MyNet.Avalonia.Controls.Headless.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]
[assembly: AvaloniaTestIsolation(AvaloniaTestIsolationLevel.PerAssembly)]

namespace MyNet.Avalonia.Controls.Headless.Tests;

internal static class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<HeadlessTestApp>()
            .UseHeadless(new());
}
