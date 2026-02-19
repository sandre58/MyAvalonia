// -----------------------------------------------------------------------
// <copyright file="HyperLinkButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class HyperLinkButtonPageViewModel : ControlCatalogViewModel
{
    public HyperLinkButtonPageViewModel()
        : base(nameof(HyperlinkButton),
            [
                new ControlThemeBuilder()
                    .AddVariants("variant-text")
                    .AddDefaultRoles()
            ])
    {
    }
}
