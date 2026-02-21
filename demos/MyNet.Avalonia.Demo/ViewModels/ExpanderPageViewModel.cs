// -----------------------------------------------------------------------
// <copyright file="ExpanderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Layout;
using DynamicData.Binding;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ExpanderPageViewModel : ControlCatalogViewModel
{
    public ExpanderPageViewModel()
        : base(nameof(Expander),
            [
                new ControlThemeBuilder()
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text",  "variant-headered", "variant-transparent", "shadow-surface", "shadow-header")
                    .AddDefaultRoles()
                    .AddDefaultSizes()
                    .AddSizes("header-watermark", "header-sub-caption", "header-caption", "header-h1", "header-h2", "header-h3", "header-h4", "header-h5", "header-h6"),

                new ControlThemeBuilder("Button")
                    .AddShapes("shape-circle")
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text", "shadow-control")
                    .AddDefaultSizes()
                    .AddDefaultRoles()
            ])
    {
        Playground.ClassProviders.AddRange([HeaderAlignmentClassProvider]);

        Disposables.Add(this.WhenPropertyChanged(x => x.ExpandDirection).Subscribe(_ =>
        {
            switch (ExpandDirection)
            {
                case ExpandDirection.Down:
                    VerticalAlignment = VerticalAlignment.Top;
                    HorizontalAlignment = HorizontalAlignment.Center;
                    Width = 300;
                    Height = double.NaN;
                    break;

                case ExpandDirection.Up:
                    VerticalAlignment = VerticalAlignment.Bottom;
                    HorizontalAlignment = HorizontalAlignment.Center;
                    Width = 300;
                    Height = double.NaN;
                    break;

                case ExpandDirection.Left:
                    VerticalAlignment = VerticalAlignment.Center;
                    HorizontalAlignment = HorizontalAlignment.Right;
                    Width = double.NaN;
                    Height = 300;
                    break;

                case ExpandDirection.Right:
                    VerticalAlignment = VerticalAlignment.Center;
                    HorizontalAlignment = HorizontalAlignment.Left;
                    Width = double.NaN;
                    Height = 300;
                    break;
            }
        }));
    }

    public ClassProvider HeaderAlignmentClassProvider { get; } = new("align-header-left");

    public ExpandDirection ExpandDirection { get; set; } = ExpandDirection.Down;

    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;

    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

    public double Width { get; set; } = 300;

    public double Height { get; set; } = double.NaN;
}
