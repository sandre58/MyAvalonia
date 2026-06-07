// -----------------------------------------------------------------------
// <copyright file="ClockSelectorPageViewModel.cs" company="Stéphane ANDRE">
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
using MyNet.Avalonia.Theme.Classes;
using MyNet.Primitives.Temporal;
using MyNet.UI.Commands;
using MyNet.UI.Resources;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ClockSelectorPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(ClockSelector), commands, [
    new ControlThemeBuilder()
        .AddAllVariants()
        .AddVariant(CssClass.ShadowSurface)
        .AddThemeRoles()
        .AddItemsThemeRoles()
        .AddProperty(Controls.Primitives.TimeSelectorBase.ShowSecondsProperty, false, x => x.DisplayName(nameof(SettingsResources.UseSeconds)))
        .AddEnumProperty<TimeFormat, ListBoxEditor>(Controls.Primitives.TimeSelectorBase.TimeFormatProperty,
            TimeFormat.TwelveHour,
            x => x.DisplayName(nameof(SettingsResources.Format)),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case TimeFormat.TwelveHour:
                        y.WithIcon(MaterialIconKind.Hours12);
                        break;
                    case TimeFormat.TwentyFourHour:
                        y.WithIcon(MaterialIconKind.Hours24);
                        break;
                }
            })
        .AddAction<ClockSelector>(x =>
            {
                x.SelectedValue = null;
                x.SelectedComponent = TimeComponent.Hour;
            },
            x => x.DisplayName(nameof(UiResources.Restore)).WithIcon(MaterialIconKind.Restore))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ClockEdit;
}
