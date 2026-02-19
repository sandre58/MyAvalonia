// -----------------------------------------------------------------------
// <copyright file="TimeViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class TimeViewPageViewModel : ControlCatalogViewModel
{
    public TimeViewPageViewModel()
        : base(nameof(TimeView),
            [
                new ControlThemeBuilder()
                    .AddThemeRoles(),
            ])
    {
    }
}
