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
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text", "variant-underline", "variant-header", "variant-header-light", "variant-header-outlined", "variant-header-text", "variant-transparent", "shadow-surface", "shadow-header")
                    .AddAllRoles()
                    .AddDefaultSizes()
                    .AddSizes("header-xs", "header-sm", "header-md", "header-lg", "header-xl", "header-h6", "header-h5", "header-h4", "header-h3", "header-h2", "header-h1"),

            new ControlThemeBuilder(null, "kind-label")
                    .AddVariants("variant-watermark")
                    .AddDefaultSizes()
                    .AddSizes("header-xs", "header-sm", "header-md", "header-lg", "header-xl", "header-h6", "header-h5", "header-h4", "header-h3", "header-h2", "header-h1")
            ]) => Playground.ClassProviders.AddRange([PositionClassProvider, HeaderAlignmentClassProvider]);

    public ClassProvider PositionClassProvider { get; } = new("position-top");

    public ClassProvider HeaderAlignmentClassProvider { get; } = new("align-header-left");
}
