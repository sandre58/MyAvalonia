// -----------------------------------------------------------------------
// <copyright file="HomePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Showcase.ViewModels.Base;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class HomePageViewModel : PageViewModel
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Home;
}
