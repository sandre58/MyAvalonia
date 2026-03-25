// -----------------------------------------------------------------------
// <copyright file="SliderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Layout;
using DynamicData.Binding;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class SliderPageViewModel : ControlCatalogViewModel
{
    public SliderPageViewModel()
        : base(nameof(Slider),
            [
                new ControlThemeBuilder()
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-control")
                    .AddDefaultRoles()
            ]) => Disposables.AddRange(
            [
                this.WhenPropertyChanged(x => x.Orientation).Subscribe(_ => UpdateLatyout())
            ]);

    /// <inheritdoc/>
    public override IconData Icon => IconData.TuneVariant;

    private void UpdateLatyout()
    {
        switch (Orientation)
        {
            case Orientation.Horizontal:
                Width = 250;
                Height = 80;
                break;

            case Orientation.Vertical:
                Width = 80;
                Height = 250;
                break;
        }
    }

    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    public double Width { get; set; } = 250;

    public double Height { get; set; } = 80;
}
