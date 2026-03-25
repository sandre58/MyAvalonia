// -----------------------------------------------------------------------
// <copyright file="NavigationMenuPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

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

    /// <inheritdoc/>
    public override IconData Icon => IconData.Navigation;
}
