// -----------------------------------------------------------------------
// <copyright file="OutlinedIconPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class OutlinedIconPageViewModel : ControlCatalogViewModel
{
    public OutlinedIconPageViewModel()
        : base(nameof(OutlinedIcon), [
            new ControlThemeBuilder()
            .AddVariants("variant-light", "variant-outlined")
            .AddAllRoles()
            .AddSizes("size-xs", "size-sm", "size-md", "size-lg", "size-xl")
        ])
    { }

    /// <inheritdoc/>
    public override IconData Icon => IconData.Shape;
}
