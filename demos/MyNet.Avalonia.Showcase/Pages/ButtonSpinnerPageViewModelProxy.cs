// -----------------------------------------------------------------------
// <copyright file="ButtonSpinnerPageViewModelProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using MyNet.Avalonia.Showcase.ViewModels.Pages;

namespace MyNet.Avalonia.Showcase.Pages;

/// <summary>
/// Typed proxy used by ButtonSpinnerPage templates to keep strong-typed binding paths.
/// </summary>
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
