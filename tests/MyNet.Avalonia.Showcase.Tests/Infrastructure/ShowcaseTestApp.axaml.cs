// -----------------------------------------------------------------------
// <copyright file="ShowcaseTestApp.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme;

namespace MyNet.Avalonia.Showcase.Tests.Infrastructure;

public sealed partial class ShowcaseTestApp : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        MyTheme.Current.EnsureLoaded();
    }
}
