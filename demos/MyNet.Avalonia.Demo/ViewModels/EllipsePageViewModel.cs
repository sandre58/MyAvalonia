// -----------------------------------------------------------------------
// <copyright file="EllipsePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Shapes;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class EllipsePageViewModel : ControlCatalogViewModel
{
    public EllipsePageViewModel()
        : base(nameof(Ellipse),
            [
                new ControlThemeBuilder(kind: "kind-card")
                    .AddVariants("variant-light", "variant-outlined", "variant-solid", "shadow-surface", "is-hover")
                    .AddAllRoles()
            ])
    {
    }
}
