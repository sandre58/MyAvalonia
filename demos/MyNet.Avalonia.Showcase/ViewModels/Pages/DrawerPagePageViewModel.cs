// -----------------------------------------------------------------------
// <copyright file="DrawerPagePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class DrawerPagePageViewModel() : ShowcaseViewModel(nameof(DrawerPage),
[
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Solid)
        .AddVariant(CssClass.ShadowSurface)
        .AddThemeRoles()
        .AddProperty(DrawerPage.IsOpenProperty, false, x => x.DisplayName(nameof(SettingsResources.IsPaneOpen)))
        .AddEnumProperty<DrawerLayoutBehavior, ComboBoxEditor>(DrawerPage.DrawerLayoutBehaviorProperty, DrawerLayoutBehavior.Overlay, x => x.DisplayName(nameof(SettingsResources.DisplayMode)))
        .AddEnumProperty<DrawerBehavior, ComboBoxEditor>(DrawerPage.DrawerBehaviorProperty)
        .AddEnumProperty<DrawerPlacement, ListBoxEditor>(DrawerPage.DrawerPlacementProperty,
            DrawerPlacement.Left,
            x => x.DisplayName(nameof(SettingsResources.PopupPlacement)),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case DrawerPlacement.Left:
                        y.WithIcon(MaterialIconKind.GamepadCircleLeft);
                        break;
                    case DrawerPlacement.Right:
                        y.WithIcon(MaterialIconKind.GamepadCircleRight);
                        break;
                    case DrawerPlacement.Top:
                        y.WithIcon(MaterialIconKind.GamepadCircleUp);
                        break;
                    case DrawerPlacement.Bottom:
                        y.WithIcon(MaterialIconKind.GamepadCircleDown);
                        break;
                }
            })
        .AddProperty(DrawerPage.CompactDrawerLengthProperty, 50.0, x => x.DisplayName(nameof(SettingsResources.CompactPaneLength)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 100)))
        .AddProperty(DrawerPage.DrawerLengthProperty, 300.0, x => x.DisplayName(nameof(SettingsResources.OpenPaneLength)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(100, 500)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ViewSplitVertical;
}
