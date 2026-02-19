// -----------------------------------------------------------------------
// <copyright file="ClockPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ClockPageViewModel : ControlCatalogViewModel
{
    public ClockPageViewModel()
        : base(nameof(Clock), [
            new ControlThemeBuilder()
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-transparent", "shadow-surface")
            .AddThemeRoles()
            .AddItemsThemeRoles()
        ])
    { }
}
