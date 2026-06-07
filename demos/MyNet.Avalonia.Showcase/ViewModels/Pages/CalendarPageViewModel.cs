// -----------------------------------------------------------------------
// <copyright file="CalendarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class CalendarPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Calendar), commands, [
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined)
        .AddVariant(CssClass.ShadowSurface)
        .AddItemsStandardVariants()
        .AddVariant(CssClass.ShadowItems)
        .AddThemeRoles()
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumValue<CalendarSelectionMode, ListBoxEditor>((x, y) =>
            {
                switch (x)
                {
                    case Calendar calendar:
                        calendar.SetValue(Calendar.SelectionModeProperty, y);
                        break;
                    case Controls.Calendar calendar1:
                        calendar1.SetValue(Controls.Calendar.SelectionModeProperty, y);
                        break;
                }
            },
            CalendarSelectionMode.SingleDate,
            x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case CalendarSelectionMode.None:
                        y.WithIcon(MaterialIconKind.CalendarBlank);
                        break;
                    case CalendarSelectionMode.SingleDate:
                        y.WithIcon(MaterialIconKind.Calendar);
                        break;
                    case CalendarSelectionMode.SingleRange:
                        y.WithIcon(MaterialIconKind.CalendarRange);
                        break;
                    case CalendarSelectionMode.MultipleRange:
                        y.WithIcon(MaterialIconKind.CalendarMultiselect);
                        break;
                }
            })
        .AddValueAction((x, y) =>
            {
                switch (x)
                {
                    case Calendar calendar:
                        calendar.SetValue(Calendar.AllowTapRangeSelectionProperty, y);
                        break;
                    case Controls.Calendar calendar1:
                        calendar1.SetValue(Controls.Calendar.AllowTapRangeSelectionProperty, y);
                        break;
                }
            },
            false,
            x => x.DisplayName(nameof(SettingsResources.AllowTapRangeSelection))
                .Of<ToggleSwitchEditor>())
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Calendar;
}
