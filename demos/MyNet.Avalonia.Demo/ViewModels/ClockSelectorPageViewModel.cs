// -----------------------------------------------------------------------
// <copyright file="ClockSelectorPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ClockSelectorPageViewModel : ControlCatalogViewModel
{
    public ClockSelectorPageViewModel()
        : base(nameof(ClockSelector), [
            new ControlThemeBuilder()
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-transparent", "shadow-surface")
            .AddThemeRoles()
            .AddItemsThemeRoles()
        ])
    { }
}
