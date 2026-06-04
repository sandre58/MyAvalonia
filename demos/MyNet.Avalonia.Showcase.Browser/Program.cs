// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.Skia;
using MyNet.Avalonia.Showcase.Composition;

namespace MyNet.Avalonia.Showcase.Browser;

internal static class Program
{
    private static Task Main()
    {
        ShowcaseApp.ConfigureForPortableHost();
        return BuildAvaloniaApp()
            .WithInterFont()
            .UseSkia()
            .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}
