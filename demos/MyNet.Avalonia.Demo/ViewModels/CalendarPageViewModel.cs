// -----------------------------------------------------------------------
// <copyright file="CalendarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class CalendarPageViewModel : AutoBuildPageViewModel
{
    public CalendarPageViewModel()
        : base(nameof(Calendar), [
            new ControlThemeBuilder().AddRoles([ThemeRole.Accent, ThemeRole.Inverse])
        ])
    { }
}
