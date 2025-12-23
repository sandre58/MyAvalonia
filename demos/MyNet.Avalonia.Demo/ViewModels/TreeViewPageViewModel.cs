// -----------------------------------------------------------------------
// <copyright file="TreeViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class TreeViewPageViewModel : AutoBuildPageViewModel
{
    public TreeViewPageViewModel()
        : base(nameof(TreeView), [
            new ControlThemeBuilder()
        ])
    { }
}
