// -----------------------------------------------------------------------
// <copyright file="TimeViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class TimeViewPageViewModel() : ControlCatalogViewModel(nameof(TimeView),
    [
        new ControlThemeBuilder()
               .AddThemeRoles()
    ])
{
    /// <inheritdoc/>
    public override IconData Icon => IconData.ClockEdit;
}
