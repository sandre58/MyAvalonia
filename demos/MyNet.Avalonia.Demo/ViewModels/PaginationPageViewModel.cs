// -----------------------------------------------------------------------
// <copyright file="PaginationPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class PaginationPageViewModel : ControlCatalogViewModel
{
    public PaginationPageViewModel()
        : base(nameof(Pagination),
            [
                new ControlThemeBuilder()
                    .AddThemeRoles(),

                new ControlThemeBuilder("Compact")
                    .AddThemeRoles()
            ])
    {
    }

    /// <inheritdoc/>
    public override IconData Icon => IconData.PageLayoutBody;
}
