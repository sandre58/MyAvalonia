// -----------------------------------------------------------------------
// <copyright file="RipplePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class RipplePageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Ripple), commands, [
    new ControlThemeBuilder()
        .AddProperty(Ripple.IsCenteredProperty, false, x => x.DisplayName(nameof(SettingsResources.IsCentered)))
        .AddProperty(Ripple.IsActiveProperty, true, x => x.DisplayName(nameof(SettingsResources.IsActive)))
        .AddProperty(Ripple.UseTransitionsProperty, true, x => x.DisplayName(nameof(SettingsResources.UseTransitions)))
        .AddProperty(Ripple.RippleOpacityProperty, 0.6, x => x.DisplayName(nameof(SettingsResources.Opacity)).Of<SliderEditor>(editor => editor.WithRange(0m, 1m).WithIncrement(0.05m)))
        .AddProperty(Ripple.SizeMultiplierProperty, 1, x => x.DisplayName(nameof(SettingsResources.SizeMultiplier)).Of<SliderEditor>(editor => editor.WithRange(1m, 3m).WithIncrement(0.1m)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.RadiusOutline;
}
