// -----------------------------------------------------------------------
// <copyright file="ListBoxPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ListBoxPageViewModel : AutoBuildPageViewModel
{
    public ListBoxPageViewModel()
        : base(nameof(ListBox), [
            new ControlThemeBuilder()
            .AddLayouts("Cards", "Cards Circle")
            .AddStyles("Solid", "Light", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Outlined", "Light")
            .AddCartesianStyles("Solid", "Shadow")
            .AddRoles([ThemeRole.Accent, ThemeRole.Inverse])
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Toggle")
            .AddStyles("Spacing", "Shadow", "Vertical")
            .AddCartesianStyles("Spacing", "Shadow", "Vertical")
            .AddRoles([ThemeRole.Accent, ThemeRole.Inverse])
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Tabs")
            .AddStyles("Vertical")
            .AddRoles([ThemeRole.Accent, ThemeRole.Inverse]),

            new ControlThemeBuilder("Icon")
            .AddStyles("Vertical")
            .AddRoles([ThemeRole.Accent, ThemeRole.Inverse])
            .AddSizes("Small", "Medium", "Large")
        ])
    { }
}
