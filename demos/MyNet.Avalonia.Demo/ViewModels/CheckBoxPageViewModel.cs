// -----------------------------------------------------------------------
// <copyright file="CheckBoxPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class CheckBoxPageViewModel : AutoBuildPageViewModel
{
    public CheckBoxPageViewModel()
        : base(nameof(CheckBox), [
            new ControlThemeBuilder()
            .AddLayouts("Circle", "Alternate")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large")
        ])
    { }
}
