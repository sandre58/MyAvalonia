// -----------------------------------------------------------------------
// <copyright file="ButtonSpinnerPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Controls.Assists;
using MyNet.Humanizer.Facade;
using MyNet.Observable.Behaviors.Metadata.Attributes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ButtonSpinnerPageViewModel : ShowcaseViewModel
{
    public ButtonSpinnerPageViewModel(ICommandFactory commands)
        : base(nameof(ButtonSpinner),
            commands,
            [
                new ControlThemeBuilder()
                    .AddShapes(CssClass.ShapeCircle)
                    .AddDefaultVariants()
                    .AddVariants(CssClass.ShadowControl)
                    .AddDefaultSizes()
                    .AddDefaultRoles()
                    .AddValueAction(
                        (x, y) => x.SetValue(SpinnerAssist.LayoutProperty, y is SpinnerLayout layout ? layout : SpinnerLayout.Horizontal),
                        SpinnerLayout.Horizontal,
                        x => x.DisplayName(nameof(SettingsResources.ButtonsLayout)).Of<ListBoxEditor>(editor =>
                            editor.AddChoices(Enum.GetValues<SpinnerLayout>(), (value, y) => y.DisplayName(() => value.Humanize()))))
                    .AddValueAction(
                        (x, y) => x.SetValue(SpinnerAssist.SwitchButtonsProperty, ((bool?)y).GetValueOrDefault()),
                        false,
                        x => x.DisplayName(nameof(SettingsResources.SwitchButtons)).Of<ToggleSwitchEditor>()),

                new ControlThemeBuilder("Underline")
                    .AddDefaultSizes()
                    .AddDefaultRoles()
                    .AddValueAction(
                        (x, y) => x.SetValue(SpinnerAssist.LayoutProperty, y is SpinnerLayout layout ? layout : SpinnerLayout.Horizontal),
                        SpinnerLayout.Horizontal,
                        x => x.DisplayName(nameof(SettingsResources.ButtonsLayout)).Of<ListBoxEditor>(editor =>
                            editor.AddChoices(Enum.GetValues<SpinnerLayout>(), (value, y) => y.DisplayName(() => value.Humanize()))))
                    .AddValueAction(
                        (x, y) => x.SetValue(SpinnerAssist.SwitchButtonsProperty, ((bool?)y).GetValueOrDefault()),
                        false,
                        x => x.DisplayName(nameof(SettingsResources.SwitchButtons)).Of<ToggleSwitchEditor>())
            ])
    {
        IncreaseCommand = commands.Create(IncreaseDate);
        DecreaseCommand = commands.Create(DecreaseDate);
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ButtonCursor;

    public ICommand IncreaseCommand { get; }

    public ICommand DecreaseCommand { get; }

    [UpdateOnCultureChanged]
    public DateTime Date { get; set => SetProperty(ref field, value); } = DateTime.Now;

    public void IncreaseDate() => Date = Date.AddDays(1);

    public void DecreaseDate() => Date = Date.AddDays(-1);
}
