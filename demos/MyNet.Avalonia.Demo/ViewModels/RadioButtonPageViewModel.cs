// -----------------------------------------------------------------------
// <copyright file="RadioButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class RadioButtonPageViewModel : ControlCatalogViewModel
{
    public RadioButtonPageViewModel()
        : base(nameof(RadioButton),
            [
                new ControlThemeBuilder()
                    .AddShapes("shape-circle", "shape-alternate")
                    .AddDefaultSizes()
                    .AddDefaultRoles()
            ])
    {
    }
}
