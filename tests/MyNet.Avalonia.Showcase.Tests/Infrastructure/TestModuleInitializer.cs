// -----------------------------------------------------------------------
// <copyright file="TestModuleInitializer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace MyNet.Avalonia.Showcase.Tests.Infrastructure;

internal static class TestModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize() => PlaygroundTestHost.EnsureInitialized();
}
