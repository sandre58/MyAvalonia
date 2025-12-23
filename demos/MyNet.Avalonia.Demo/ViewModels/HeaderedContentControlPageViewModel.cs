// -----------------------------------------------------------------------
// <copyright file="HeaderedContentControlPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class HeaderedContentControlPageViewModel : AutoBuildPageViewModel
{
    public HeaderedContentControlPageViewModel()
        : base(nameof(HeaderedContentControl), [
            new ControlThemeBuilder()
            .AddStyles("Light", "Outlined", "Text", "Shadow", "Headered")
            .AddCartesianStyles("Headered", "HeaderShadow")
            .AddCartesianStyles("Outlined", "Solid")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddCartesianStyles("Light", "Outlined", "Headered")
            .AddAllRoles()
        ])
    { }
}
