// -----------------------------------------------------------------------
// <copyright file="ButtonSpinnerPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Showcase.ViewModels.Pages;

namespace MyNet.Avalonia.Showcase.Pages;

internal sealed partial class ButtonSpinnerPage : ContentPage
{
    public ButtonSpinnerPage() => InitializeComponent();
}

internal sealed class ButtonSpinnerPageViewModelProxy : AvaloniaObject
{
    public static readonly StyledProperty<ButtonSpinnerPageViewModel?> DataProperty =
        AvaloniaProperty.Register<ButtonSpinnerPageViewModelProxy, ButtonSpinnerPageViewModel?>(nameof(Data));

    public ButtonSpinnerPageViewModel? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}
