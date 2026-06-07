// -----------------------------------------------------------------------
// <copyright file="SplitViewPageViewModel.cs" company="Stéphane ANDRE">
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

internal sealed class SplitViewPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(SplitView), commands, [
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Solid)
        .AddVariant(CssClass.ShadowSurface)
        .AddThemeRoles()
        .AddProperty(SplitView.IsPaneOpenProperty, false, x => x.DisplayName(nameof(SettingsResources.IsPaneOpen)))
        .AddProperty(SplitView.UseLightDismissOverlayModeProperty, false, x => x.DisplayName(nameof(SettingsResources.UseOverlay)))
        .AddEnumProperty<SplitViewDisplayMode, ComboBoxEditor>(SplitView.DisplayModeProperty, SplitViewDisplayMode.Overlay, x => x.DisplayName(nameof(SettingsResources.DisplayMode)))
        .AddEnumProperty<SplitViewPanePlacement, ListBoxEditor>(SplitView.PanePlacementProperty,
            SplitViewPanePlacement.Left,
            x => x.DisplayName(nameof(SettingsResources.PopupPlacement)),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case SplitViewPanePlacement.Left:
                        y.WithIcon(MaterialIconKind.GamepadCircleLeft);
                        break;
                    case SplitViewPanePlacement.Right:
                        y.WithIcon(MaterialIconKind.GamepadCircleRight);
                        break;
                    case SplitViewPanePlacement.Top:
                        y.WithIcon(MaterialIconKind.GamepadCircleUp);
                        break;
                    case SplitViewPanePlacement.Bottom:
                        y.WithIcon(MaterialIconKind.GamepadCircleDown);
                        break;
                }
            })
        .AddProperty(SplitView.CompactPaneLengthProperty, 50, x => x.DisplayName(nameof(SettingsResources.CompactPaneLength)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 100)))
        .AddProperty(SplitView.OpenPaneLengthProperty, 300, x => x.DisplayName(nameof(SettingsResources.OpenPaneLength)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(100, 500)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ViewSplitVertical;
}
