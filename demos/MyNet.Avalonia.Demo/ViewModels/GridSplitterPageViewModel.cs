// -----------------------------------------------------------------------
// <copyright file="GridSplitterPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class GridSplitterPageViewModel : ControlCatalogViewModel
{
    public GridSplitterPageViewModel()
        : base(nameof(GridSplitter),
            [
                new ControlThemeBuilder()
            ])
    {
    }
}
