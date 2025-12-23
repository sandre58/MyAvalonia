// -----------------------------------------------------------------------
// <copyright file="ProgressBarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ProgressBarPageViewModel : AutoBuildPageViewModel
{
    public ProgressBarPageViewModel()
        : base(nameof(ProgressBar), [
            new ControlThemeBuilder()
            .AddLayouts("Circle")
            .AddStyles("Shadow")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large")
        ])
    { }
}
