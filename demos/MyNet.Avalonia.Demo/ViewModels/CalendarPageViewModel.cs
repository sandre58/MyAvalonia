// -----------------------------------------------------------------------
// <copyright file="CalendarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class CalendarPageViewModel : ControlCatalogViewModel
{
    public CalendarPageViewModel()
        : base(nameof(Calendar),
            [
                new ControlThemeBuilder()
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-transparent", "shadow-surface", "variant-solid-items", "variant-light-items", "variant-outlined-items", "variant-text-items")
                    .AddThemeRoles()
                    .AddItemsThemeRoles()
                    .AddDefaultSizes()
            ])
    { }
}
