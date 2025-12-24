// -----------------------------------------------------------------------
// <copyright file="GridSplitterPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class GridSplitterPageViewModel : AutoBuildPageViewModel
{
    public GridSplitterPageViewModel()
        : base(nameof(GridSplitter), [
            new ControlThemeBuilder()
        ])
    { }
}
