// -----------------------------------------------------------------------
// <copyright file="SelectableTextBlockPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class SelectableTextBlockPageViewModel : AutoBuildPageViewModel
{
    public SelectableTextBlockPageViewModel()
        : base(nameof(SelectableTextBlock), [
            new ControlThemeBuilder()
            .AddStyles("Secondary", "Tertiary", "Underline", "Delete", "Disablable")
            .AddDefaultRoles()
            .AddAllSizes()
        ])
    { }
}
