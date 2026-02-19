// -----------------------------------------------------------------------
// <copyright file="NavigationMenuPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class NavigationMenuPageViewModel : ControlCatalogViewModel
{
    public NavigationMenuPageViewModel()
        : base(nameof(NavigationMenu),
            [
                new ControlThemeBuilder()
                    .AddDefaultRoles()
            ])
    {
    }
}
