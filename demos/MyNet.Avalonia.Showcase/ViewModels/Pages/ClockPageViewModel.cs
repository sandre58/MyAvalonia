// -----------------------------------------------------------------------
// <copyright file="ClockPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ClockPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Clock), commands, [
    new ControlThemeBuilder()
        .AddAllVariants()
        .AddVariant(CssClass.ShadowSurface)
        .AddThemeRoles()
        .AddItemsThemeRoles()
        .AddProperty(Clock.IsSmoothProperty, false, x => x.DisplayName(nameof(SettingsResources.IsSmooth)))
        .AddProperty(Clock.LiveUpdateProperty, false, x => x.DisplayName(nameof(SettingsResources.LiveUpdate)))
        .AddValueAction((x, y) =>
            {
                if (y is not IEnumerable values)
                    return;

                var result = values.OfType<TimeComponent>().ToList();
                x.SetCurrentValue(Clock.ShowHourHandProperty, result.Contains(TimeComponent.Hour));
                x.SetCurrentValue(Clock.ShowMinuteHandProperty, result.Contains(TimeComponent.Minute));
                x.SetCurrentValue(Clock.ShowSecondHandProperty, result.Contains(TimeComponent.Second));
            },
            new ObservableCollection<TimeComponent> { TimeComponent.Hour, TimeComponent.Minute, TimeComponent.Second },
            x => x.DisplayName(nameof(SettingsResources.Hands))
                .Of<ListBoxEditor>(editor => editor.AddChoice(TimeComponent.Hour, builder => builder.DisplayName(nameof(SettingsResources.Hours)))
                    .AddChoice(TimeComponent.Minute, builder => builder.DisplayName(nameof(SettingsResources.Minutes)))
                    .AddChoice(TimeComponent.Second, builder => builder.DisplayName(nameof(SettingsResources.Seconds)))
                    .AllowMultipleSelection()))
        .AddValueAction((x, y) =>
            {
                if (y is not IEnumerable values)
                    return;

                var result = values.OfType<TimeComponent>().ToList();
                x.SetCurrentValue(Clock.ShowHourTicksProperty, result.Contains(TimeComponent.Hour));
                x.SetCurrentValue(Clock.ShowMinuteTicksProperty, result.Contains(TimeComponent.Minute));
            },
            new ObservableCollection<TimeComponent> { TimeComponent.Hour, TimeComponent.Minute },
            x => x.DisplayName(nameof(SettingsResources.Ticks))
                .Of<ListBoxEditor>(editor => editor.AddChoice(TimeComponent.Hour, builder => builder.DisplayName(nameof(SettingsResources.Hours)))
                    .AddChoice(TimeComponent.Minute, builder => builder.DisplayName(nameof(SettingsResources.Minutes)))
                    .AllowMultipleSelection()))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Clock;
}
