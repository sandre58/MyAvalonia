// -----------------------------------------------------------------------
// <copyright file="BadgePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class BadgePageViewModel : ControlCatalogViewModel
{
    public BadgePageViewModel()
        : base(nameof(Badge), [
            new ControlThemeBuilder()
            .AddShapes("shape-circle")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-transparent", "shadow-control")
            .AddAllRoles()
            .AddDefaultSizes()
        ])
    { }

    /// <inheritdoc/>
    public override IconData Icon => IconData.CheckboxBlankBadge;
}
