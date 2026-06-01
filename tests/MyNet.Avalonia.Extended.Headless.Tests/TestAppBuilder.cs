// -----------------------------------------------------------------------
// <copyright file="TestAppBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Headless;

[assembly: AvaloniaTestApplication(typeof(MyNet.Avalonia.Extended.Headless.Tests.TestAppBuilder))]
[assembly: AvaloniaTestIsolation(AvaloniaTestIsolationLevel.PerAssembly)]

namespace MyNet.Avalonia.Extended.Headless.Tests;

internal static class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<ExtendedHeadlessTestApp>()
            .UseHeadless(new());
}
