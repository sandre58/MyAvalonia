// -----------------------------------------------------------------------
// <copyright file="LabelPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class LabelPageViewModel : ControlCatalogViewModel
{
    public LabelPageViewModel()
        : base(nameof(Label),
            [
                new ControlThemeBuilder()
                    .AddVariants("opacity-high", "opacity-medium", "opacity-low", "text-helper", "text-watermark")
                    .AddAllRoles()
                    .AddSizes("font-xs", "font-sm", "font-md", "font-lg", "font-xl", "font-h6", "font-h5", "font-h4", "font-h3", "font-h2", "font-h1"),

                new ControlThemeBuilder(kind: "kind-badge")
                    .AddShapes("shape-circle")
                    .AddVariants("variant-light", "variant-outlined", "variant-text", "shadow-control")
                    .AddAllRoles()
                    .AddSizes("size-sm", "size-md", "size-lg"),

                new ControlThemeBuilder(kind: "kind-code")
            ])
    {
    }

    /// <inheritdoc/>
    public override IconData Icon => IconData.TagText;
}
