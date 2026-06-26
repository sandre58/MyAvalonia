// -----------------------------------------------------------------------
// <copyright file="BusyIndicatorPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
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
using MyNet.UI.Loading;
using MyNet.UI.Loading.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class BusyIndicatorPageViewModel : ShowcaseViewModel
{
    private static int _demoDurationSeconds = 5;

    public BusyIndicatorPageViewModel(ICommandFactory commands)
        : base(nameof(BusyIndicator), commands, [
            FillThemeBuilder(new ControlThemeBuilder().AddVariants(ControlVariant.Text)),
            FillThemeBuilder(new ControlThemeBuilder().WithKind(CssClass.KindCard).AddDefaultVariants())
        ])
    {
        DemoBusyService = new BusyService();

        StartCommand = commands.Create(async () => await StartAsync().ConfigureAwait(true));
        TestClickCommand = commands.Create(RegisterTestClick);
        RunIndeterminateCommand = commands.Create(async () => await RunIndeterminateAsync().ConfigureAwait(true));
        RunDeterminateCommand = commands.Create(async () => await RunDeterminateAsync().ConfigureAwait(true));
        RunProgressionCommand = commands.Create(async () => await RunProgressionAsync().ConfigureAwait(true));
        RunNestedCommand = commands.Create(async () => await RunNestedAsync().ConfigureAwait(true));
        RunDownloadCommand = commands.Create(async () => await RunDownloadAsync().ConfigureAwait(true));
    }

    public IBusyService DemoBusyService { get; }

    public ICommand StartCommand { get; }

    public ICommand TestClickCommand { get; }

    public ICommand RunIndeterminateCommand { get; }

    public ICommand RunDeterminateCommand { get; }

    public ICommand RunProgressionCommand { get; }

    public ICommand RunNestedCommand { get; }

    public ICommand RunDownloadCommand { get; }

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

    private static ControlThemeBuilder FillThemeBuilder(ControlThemeBuilder builder) => builder
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
            (_, y) => _demoDurationSeconds = Convert.ToInt32(y ?? 5, CultureInfo.CurrentCulture),
            5,
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

    private async Task StartAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(_demoDurationSeconds)).ConfigureAwait(true);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RegisterTestClick() => TestClickCount++;

    private async Task RunIndeterminateAsync()
    {
        try
        {
            await DemoBusyService.RunAsync<IndeterminateBusy>(async (busy, cancellationToken) =>
            {
                busy.CanCancel = true;
                busy.Message = BusyIndicatorPageResources.ServiceIndeterminateMessage;

                for (var step = 1; step <= 8 && !cancellationToken.IsCancellationRequested; step++)
                {
                    busy.Message = string.Format(
                        CultureInfo.CurrentCulture,
                        BusyIndicatorPageResources.ServiceIndeterminateStepFormat,
                        step);

                    await Task.Delay(TimeSpan.FromMilliseconds(600), cancellationToken).ConfigureAwait(true);
                }
            }).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested by the user; nothing to clean up in this demo.
        }
    }

    private async Task RunDeterminateAsync()
    {
        try
        {
            await DemoBusyService.RunAsync<DeterminateBusy>(async (busy, cancellationToken) =>
            {
                busy.Message = BusyIndicatorPageResources.ServiceDeterminateMessage;
                busy.Minimum = 0;
                busy.Maximum = 100;
                busy.Value = 0;

                for (var value = 0; value <= 100 && !cancellationToken.IsCancellationRequested; value++)
                {
                    busy.Value = value;
                    await Task.Delay(TimeSpan.FromMilliseconds(45), cancellationToken).ConfigureAwait(true);
                }
            }).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested by the user; nothing to clean up in this demo.
        }
    }

    private async Task RunProgressionAsync()
    {
        try
        {
            await DemoBusyService.RunAsync<ProgressionBusy>(async (busy, cancellationToken) =>
            {
                busy.CanCancel = true;
                busy.Report(0, BusyIndicatorPageResources.ServiceProgressionStep1);
                await Task.Delay(TimeSpan.FromMilliseconds(900), cancellationToken).ConfigureAwait(true);
                busy.Report(0.35, BusyIndicatorPageResources.ServiceProgressionStep2);
                await Task.Delay(TimeSpan.FromMilliseconds(900), cancellationToken).ConfigureAwait(true);
                busy.Report(0.7, BusyIndicatorPageResources.ServiceProgressionStep3);
                await Task.Delay(TimeSpan.FromMilliseconds(900), cancellationToken).ConfigureAwait(true);
                busy.Report(1, BusyIndicatorPageResources.ServiceProgressionStep4);
                await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken).ConfigureAwait(true);
            }).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested by the user; nothing to clean up in this demo.
        }
    }

    private async Task RunDownloadAsync()
    {
        const double totalMegabytes = 48.0;

        try
        {
            await DemoBusyService.RunAsync<DownloadBusy>(async (busy, cancellationToken) =>
            {
                busy.CanCancel = true;
                busy.FileName = BusyIndicatorPageResources.ServiceDownloadFileName;

                for (var value = 0; value <= 100 && !cancellationToken.IsCancellationRequested; value += 2)
                {
                    var fraction = value / 100d;
                    var received = totalMegabytes * fraction;

                    busy.Percentage = fraction;
                    busy.Sizes = string.Format(
                        CultureInfo.CurrentCulture,
                        BusyIndicatorPageResources.ServiceDownloadSizeFormat,
                        received,
                        totalMegabytes);
                    busy.Speed = string.Format(
                        CultureInfo.CurrentCulture,
                        BusyIndicatorPageResources.ServiceDownloadSpeedFormat,
                        4.5 + ((value % 7) * 0.3));

                    await Task.Delay(TimeSpan.FromMilliseconds(80), cancellationToken).ConfigureAwait(true);
                }
            }).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested by the user; nothing to clean up in this demo.
        }
    }

    private async Task RunNestedAsync()
    {
        try
        {
            await DemoBusyService.RunAsync<IndeterminateBusy>(async (outer, cancellationToken) =>
            {
                outer.Message = BusyIndicatorPageResources.ServiceNestedOuter;
                await Task.Delay(TimeSpan.FromMilliseconds(700), cancellationToken).ConfigureAwait(true);

                using (DemoBusyService.Begin<ProgressionBusy>(cancellationToken))
                {
                    var inner = DemoBusyService.GetCurrent<ProgressionBusy>()
                        ?? throw new InvalidOperationException("Expected an active progression scope.");

                    inner.CanCancel = true;
                    inner.Report(0.15, BusyIndicatorPageResources.ServiceNestedInner);
                    await Task.Delay(TimeSpan.FromMilliseconds(1100), cancellationToken).ConfigureAwait(true);
                    inner.Report(0.85, BusyIndicatorPageResources.ServiceNestedInnerResume);
                    await Task.Delay(TimeSpan.FromMilliseconds(900), cancellationToken).ConfigureAwait(true);
                }

                outer.Message = BusyIndicatorPageResources.ServiceNestedResumed;
                await Task.Delay(TimeSpan.FromMilliseconds(700), cancellationToken).ConfigureAwait(true);
            }).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested by the user; nothing to clean up in this demo.
        }
    }
}
