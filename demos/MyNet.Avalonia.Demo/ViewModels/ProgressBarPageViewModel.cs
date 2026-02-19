// -----------------------------------------------------------------------
// <copyright file="ProgressBarPageViewModel.cs" company="Stéphane ANDRE">
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

internal sealed class ProgressBarPageViewModel : ControlCatalogViewModel
{
    public ProgressBarPageViewModel()
        : base(nameof(ProgressBar),
            [
                new ControlThemeBuilder()
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-control")
                    .AddDefaultSizes()
                    .AddDefaultRoles(),

                new ControlThemeBuilder("Circular")
                    .AddVariants("variant-light", "shadow-control")
                    .AddDefaultSizes()
                    .AddDefaultRoles()
            ])
    {
        Playground.ClassProviders.AddRange([PositionClassProvider]);

        Disposables.AddRange(
            [
                PositionClassProvider.WhenPropertyChanged(x => x.SelectedClass).Subscribe(_ => ShowProgressText = !string.IsNullOrEmpty(PositionClassProvider.SelectedClass)),
                this.WhenPropertyChanged(x => x.Orientation).Subscribe(_ => UpdateLatyout()),
                Playground.Appearance.WhenPropertyChanged(x => x.SelectedTheme).Subscribe(_ => UpdateLatyout())
            ]);
    }

    private void UpdateLatyout()
    {
        if (Playground.Appearance.SelectedTheme != null)
        {
            Width = 120;
            Height = 120;
        }
        else
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    Width = 250;
                    Height = double.NaN;
                    break;

                case Orientation.Vertical:
                    Width = double.NaN;
                    Height = 250;
                    break;
            }
        }
    }

    public bool ShowProgressText { get; set; } = true;

    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    public double Width { get; set; } = 250;

    public double Height { get; set; } = double.NaN;

    public ClassProvider PositionClassProvider { get; set; } = new("position-center");
}
