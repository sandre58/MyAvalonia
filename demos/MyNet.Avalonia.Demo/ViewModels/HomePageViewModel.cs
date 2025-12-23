// -----------------------------------------------------------------------
// <copyright file="HomePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Demo.Resources;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class HomePageViewModel : PageViewModel
{
    protected override string CreateTitle() => DemoResources.Home;
}
