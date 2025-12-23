// -----------------------------------------------------------------------
// <copyright file="FieldsPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class FieldsPageViewModel : AutoBuildPageViewModel
{
    public FieldsPageViewModel()
    : base("Fields", [
        new ControlThemeBuilder()
            .AddStyles("Outlined", "Outlined Transparent", "Circle", "Circle Outlined", "Circle Outlined Transparent", "Underline")
    ])
    { }
}
