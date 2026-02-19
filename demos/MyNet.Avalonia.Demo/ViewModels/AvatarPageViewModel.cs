// -----------------------------------------------------------------------
// <copyright file="AvatarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class AvatarPageViewModel : ControlCatalogViewModel
{
    public AvatarPageViewModel()
        : base(nameof(Avatar), [
            new ControlThemeBuilder(defaultContentType: ContentProviderType.Icon)
            .AddShapes("shape-circle")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text", "shadow-control")
            .AddAllRoles()
            .AddSizes("size-xs", "size-sm", "size-md", "size-lg", "size-xl")
        ])
    { }
}
