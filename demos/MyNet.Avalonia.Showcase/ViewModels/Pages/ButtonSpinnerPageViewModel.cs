// -----------------------------------------------------------------------
// <copyright file="ButtonSpinnerPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ButtonSpinnerPageViewModel : ShowcaseViewModel
{
    public ButtonSpinnerPageViewModel()
        : base(nameof(ButtonSpinner),
        [
            new ControlThemeBuilder()
                .AddShapes(CssClass.ShapeCircle)
                .AddDefaultVariants()
                .AddVariants(CssClass.ShadowControl)
                .AddDefaultSizes()
                .AddDefaultRoles()
        ])
    {
        IncreaseCommand = CommandsManager.Create(IncreaseDate);
        DecreaseCommand = CommandsManager.Create(DecreaseDate);
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ButtonCursor;

    public ICommand IncreaseCommand { get; }

    public ICommand DecreaseCommand { get; }

    [UpdateOnCultureChanged]
    public DateTime Date { get; set; } = DateTime.Now;

    public void IncreaseDate() => Date = Date.AddDays(1);

    public void DecreaseDate() => Date = Date.AddDays(-1);
}
