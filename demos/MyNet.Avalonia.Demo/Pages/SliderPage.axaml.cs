// -----------------------------------------------------------------------
// <copyright file="SliderPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class SliderPage : Page
{
    public SliderPage() => InitializeComponent();

    private void Orientation_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => this.ExecuteOnChildren<Slider>(x =>
        {
            if (x is ColorSlider) return;

            switch (Orientation.SelectedIndex)
            {
                case 0:
                    x.Width = 250;
                    x.Height = 80;
                    break;

                case 1:
                    x.Width = 80;
                    x.Height = 250;
                    break;
            }
        });
}
