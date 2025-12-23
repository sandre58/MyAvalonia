// -----------------------------------------------------------------------
// <copyright file="AvatarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class AvatarPageViewModel : AutoBuildPageViewModel
{
    public AvatarPageViewModel()
        : base(nameof(Avatar), [
            new ControlThemeBuilder(defaultContentType: ContentType.Icon)
            .AddLayouts("Circle")
            .AddStyles("Shadow")
            .AddAllRoles()
            .AddSizes("ExtraSmall", "Small", "Medium", "Large", "ExtraLarge")
        ])
    { }
}
