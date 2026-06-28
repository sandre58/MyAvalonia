// -----------------------------------------------------------------------
// <copyright file="DateRangePickerPageViewModel.cs" company="Stéphane ANDRE">
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

internal sealed class DateRangePickerPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(DateRangePickerEx), commands, [
    new ControlThemeBuilder()
        .AddAllVariants()
        .AddThemeRoles()
        .AddDefaultSizes()
        .AddValueAction((x, y) => x.SetValue(DateRangePickerEx.AllowTapRangeSelectionProperty, y),
            true,
            x => x.DisplayName(nameof(SettingsResources.AllowTapRangeSelection)).Of<ToggleSwitchEditor>())
        .AddAction<DateRangePickerEx>(x => x.Clear(),
            x => x.DisplayName(nameof(UiResources.Clear)).WithIcon(MaterialIconKind.TrashCan))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.CalendarRange;
}
