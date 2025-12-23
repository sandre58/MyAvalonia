// -----------------------------------------------------------------------
// <copyright file="HyperLinkButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class HyperLinkButtonPageViewModel : AutoBuildPageViewModel
{
    public HyperLinkButtonPageViewModel()
        : base(nameof(HyperlinkButton), [
            new ControlThemeBuilder()
            .AddStyles("Text")
            .AddDefaultRoles()
        ])
    { }
}
