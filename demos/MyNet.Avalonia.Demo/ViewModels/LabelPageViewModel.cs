// -----------------------------------------------------------------------
// <copyright file="LabelPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class LabelPageViewModel : ControlCatalogViewModel
{
    public LabelPageViewModel()
        : base(nameof(Label),
            [
                new ControlThemeBuilder()
                    .AddVariants("opacity-high", "opacity-medium", "opacity-low")
                    .AddAllRoles()
                    .AddSizes("font-sub-caption", "font-caption", "h6", "h5", "h4", "h3", "h2", "h1"),

                new ControlThemeBuilder(kind: "kind-badge")
                    .AddShapes("shape-circle")
                    .AddVariants("variant-light", "variant-outlined", "variant-text", "shadow-control")
                    .AddAllRoles()
                    .AddSizes("size-sm", "size-md", "size-lg"),

                new ControlThemeBuilder(kind: "kind-code")
            ])
    {
    }
}
