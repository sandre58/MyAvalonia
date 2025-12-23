// -----------------------------------------------------------------------
// <copyright file="RadioButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class RadioButtonPageViewModel : AutoBuildPageViewModel
{
    public RadioButtonPageViewModel()
        : base(nameof(RadioButton), [
            new ControlThemeBuilder()
            .AddLayouts("Circle", "Alternate")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large")
        ])
    { }
}
