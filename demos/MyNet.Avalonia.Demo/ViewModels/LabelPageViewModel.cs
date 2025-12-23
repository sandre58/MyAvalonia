// -----------------------------------------------------------------------
// <copyright file="LabelPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class LabelPageViewModel : AutoBuildPageViewModel
{
    public LabelPageViewModel()
        : base(nameof(Label), [
            new ControlThemeBuilder()
            .AddStyles("Secondary", "Tertiary")
            .AddAllRoles()
            .AddAllSizes(),

            new ControlThemeBuilder("Tag")
            .AddLayouts("Circle")
            .AddStyles("Light", "Outlined", "Outlined Text", "Shadow")
            .AddCartesianStyles("Light", "Outlined")
            .AddAllRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Code")
        ])
    { }
}
