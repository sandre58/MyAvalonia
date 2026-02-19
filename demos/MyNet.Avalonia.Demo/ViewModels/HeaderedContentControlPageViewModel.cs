// -----------------------------------------------------------------------
// <copyright file="HeaderedContentControlPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Primitives;
using DynamicData;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class HeaderedContentControlPageViewModel : ControlCatalogViewModel
{
    public HeaderedContentControlPageViewModel()
        : base(nameof(HeaderedContentControl),
            [
                new ControlThemeBuilder()
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text", "variant-underline", "variant-headered", "variant-transparent", "shadow-surface", "shadow-header")
                    .AddAllRoles()
                    .AddDefaultSizes()
                    .AddSizes(["header-watermark", "header-sub-caption", "header-caption", "header-h1", "header-h2", "header-h3", "header-h4", "header-h5", "header-h6"]),

            new ControlThemeBuilder(null, "kind-label")
                    .AddVariants("variant-watermark")
                    .AddDefaultSizes()
                    .AddSizes(["header-watermark", "header-sub-caption", "header-caption", "header-h1", "header-h2", "header-h3", "header-h4", "header-h5", "header-h6"])
            ]) => Playground.ClassProviders.AddRange([PositionClassProvider, HeaderAlignmentClassProvider]);

    public ClassProvider PositionClassProvider { get; } = new("position-top");

    public ClassProvider HeaderAlignmentClassProvider { get; } = new("align-header-left");
}
