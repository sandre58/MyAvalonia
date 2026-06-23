// -----------------------------------------------------------------------
// <copyright file="BusyIndicatorPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class BusyIndicatorPageViewModel : ShowcaseViewModel
{
    private static decimal _demoDurationSeconds = 5m;

    public BusyIndicatorPageViewModel(ICommandFactory commands)
        : base(nameof(BusyIndicator), commands, [
            CreateThemeBuilder()
        ])
    {
        StartCommand = commands.Create(async () => await StartAsync().ConfigureAwait(true));
        TestClickCommand = commands.Create(RegisterTestClick);
    }

    public ICommand StartCommand { get; }

    public ICommand TestClickCommand { get; }

    public bool IsBusy
    {
        get;
        private set => SetProperty(ref field, value);
    }

    public int TestClickCount
    {
        get;
        private set => SetProperty(ref field, value);
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.TimerSand;

    private static ControlThemeBuilder CreateThemeBuilder(string? themeKey = null)
    {
        var builder = new ControlThemeBuilder(themeKey)
            .AddVariants(ControlVariant.Solid)
            .AddVariant(CssClass.Kind("minimal"))
            .AddDefaultRoles()
            .AddStandardSizes()
            .AddProperty(BusyIndicator.IsBlockingProperty, true, x => x.DisplayName(nameof(SettingsResources.IsBlocking)).Of<ToggleSwitchEditor>())
            .AddProperty(
                BusyIndicator.MessageProperty,
                string.Empty,
                x => x.DisplayName(nameof(SettingsResources.BusyMessage))
                    .Of<TextBoxEditor>(y => y.WithValue(BusyIndicatorPageResources.DefaultBusyMessage)))
            .AddProperty(
                BusyIndicator.OverlayOpacityProperty,
                1d,
                x => x.DisplayName(nameof(SettingsResources.Opacity)).Of<SliderEditor>(editor => editor.WithRange(0, 1).WithIncrement(0.05M)))
            .AddValueAction(
                (_, y) => _demoDurationSeconds = (decimal?)y ?? 5m,
                5m,
                x => x.DisplayName(nameof(SettingsResources.DisplayDuration))
                    .Of<IntNumericUpDownEditor>(editor => editor.WithRange(1, 15).WithIncrement(1).DisplaySuffix("s")))
            .AddEnumProperty<LoaderAnimation, ListBoxEditor>(
                BusyIndicator.AnimationProperty,
                LoaderAnimation.Circular,
                x => x.DisplayName(nameof(SettingsResources.Animation)),
                configureChoice: (animation, choice) => choice.WithIcon(animation switch
                {
                    LoaderAnimation.Circular => MaterialIconKind.Loading,
                    LoaderAnimation.Ring => MaterialIconKind.CircleOutline,
                    LoaderAnimation.Dots => MaterialIconKind.DotsHorizontal,
                    LoaderAnimation.Bars => MaterialIconKind.ChartBar,
                    LoaderAnimation.Pulse => MaterialIconKind.CircleOpacity,
                    _ => MaterialIconKind.Loading
                }));

        return builder;
    }

    private async Task StartAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            await Task.Delay(TimeSpan.FromSeconds((double)_demoDurationSeconds)).ConfigureAwait(true);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RegisterTestClick() => TestClickCount++;
}
