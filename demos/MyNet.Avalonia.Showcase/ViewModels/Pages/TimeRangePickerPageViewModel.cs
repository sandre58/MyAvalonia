// -----------------------------------------------------------------------
// <copyright file="TimeRangePickerPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;
using MyNet.UI.Resources;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class TimeRangePickerPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(TimeRangePickerEx), commands, [
    new ControlThemeBuilder()
        .AddAllVariants()
        .AddThemeRoles()
        .AddDefaultSizes()
        .AddValueAction((x, y) => x.SetValue(TimeRangePickerEx.AllowOvernightProperty, y),
            false,
            x => x.DisplayName(nameof(SettingsResources.AllowOvernight)).Of<ToggleSwitchEditor>())
        .AddValueAction((x, y) => x.SetValue(TimeRangePickerEx.ShowOvernightIndicatorProperty, y),
            false,
            x => x.DisplayName(nameof(SettingsResources.ShowOvernightIndicator)).Of<ToggleSwitchEditor>())
        .AddValueAction((x, y) => x.SetValue(TimeRangePickerEx.InvalidRangeBehaviorProperty, (bool)y ? TimeRangeInvalidBehavior.ReportError : TimeRangeInvalidBehavior.Swap),
            false,
            x => x.DisplayName(nameof(SettingsResources.InvalidRangeReportError)).Of<ToggleSwitchEditor>())
        .AddAction<TimeRangePickerEx>(x => x.Clear(),
            x => x.DisplayName(nameof(UiResources.Clear)).WithIcon(MaterialIconKind.TrashCan))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ClockTimeFourOutline;
}
