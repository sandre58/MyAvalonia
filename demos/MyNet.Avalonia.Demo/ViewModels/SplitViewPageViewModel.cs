// -----------------------------------------------------------------------
// <copyright file="SplitViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class SplitViewPageViewModel : ControlCatalogViewModel
{
    public SplitViewPageViewModel()
        : base(nameof(SplitView), [
            new ControlThemeBuilder()
            .AddVariants("variant-solid", "shadow-surface")
            .AddThemeRoles()
        ])
    { }
}
