// -----------------------------------------------------------------------
// <copyright file="ListBoxPageViewModel.cs" company="Stéphane ANDRE">
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
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ListBoxPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(ListBox), commands, [
    new ControlThemeBuilder()
        .AddShapes(CssClass.ShapeItemsCircle)
        .AddStandardVariants()
        .AddVariant(CssClass.ShadowSurface)
        .AddItemsStandardVariants()
        .AddVariant(CssClass.ShadowItems)
        .AddVariants(CssClass.Vertical, CssClass.Horizontal, CssClass.Uniform, CssClass.Wrap)
        .AddThemeRoles()
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<SelectionMode, ListBoxEditor>(ListBox.SelectionModeProperty,
            SelectionMode.Single,
            x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
            x => x.AllowMultipleSelection(),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case SelectionMode.Single:
                        y.WithIcon(MaterialIconKind.Check);
                        break;
                    case SelectionMode.AlwaysSelected:
                        y.WithIcon(MaterialIconKind.CheckboxMarked);
                        break;
                    case SelectionMode.Toggle:
                        y.WithIcon(MaterialIconKind.ToggleSwitch);
                        break;
                    case SelectionMode.Multiple:
                        y.WithIcon(MaterialIconKind.CheckAll);
                        break;
                }
            }),

    new ControlThemeBuilder()
        .WithKind("cards")
        .AddShapes(CssClass.ShapeItemsCircle)
        .AddStandardVariants()
        .AddVariant(CssClass.ShadowSurface)
        .AddItemsStandardVariants()
        .AddVariant(CssClass.ShadowItems)
        .AddVariants(CssClass.Vertical, CssClass.Horizontal, CssClass.Uniform, CssClass.Wrap)
        .AddThemeRoles()
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<SelectionMode, ListBoxEditor>(ListBox.SelectionModeProperty,
            SelectionMode.Single,
            x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
            x => x.AllowMultipleSelection(),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case SelectionMode.Single:
                        y.WithIcon(MaterialIconKind.Check);
                        break;
                    case SelectionMode.AlwaysSelected:
                        y.WithIcon(MaterialIconKind.CheckboxMarked);
                        break;
                    case SelectionMode.Toggle:
                        y.WithIcon(MaterialIconKind.ToggleSwitch);
                        break;
                    case SelectionMode.Multiple:
                        y.WithIcon(MaterialIconKind.CheckAll);
                        break;
                }
            }),

    new ControlThemeBuilder()
        .WithKind("toggle")
        .AddStandardVariants()
        .AddVariant(CssClass.ShadowSurface)
        .AddItemsStandardVariants()
        .AddVariant(CssClass.ShadowItems)
        .AddVariants(CssClass.Vertical, CssClass.Horizontal, CssClass.Uniform, CssClass.Wrap)
        .AddDefaultRoles()
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<SelectionMode, ListBoxEditor>(ListBox.SelectionModeProperty,
            SelectionMode.Single,
            x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
            x => x.AllowMultipleSelection(),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case SelectionMode.Single:
                        y.WithIcon(MaterialIconKind.Check);
                        break;
                    case SelectionMode.AlwaysSelected:
                        y.WithIcon(MaterialIconKind.CheckboxMarked);
                        break;
                    case SelectionMode.Toggle:
                        y.WithIcon(MaterialIconKind.ToggleSwitch);
                        break;
                    case SelectionMode.Multiple:
                        y.WithIcon(MaterialIconKind.CheckAll);
                        break;
                }
            }),

    new ControlThemeBuilder("Tabs")
        .AddStandardVariants()
        .AddVariant(CssClass.ShadowSurface)
        .AddItemsStandardVariants()
        .AddVariant(CssClass.ShadowItems)
        .AddVariants(CssClass.Vertical, CssClass.Horizontal, CssClass.Uniform, CssClass.Wrap)
        .AddThemeRoles()
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<SelectionMode, ListBoxEditor>(ListBox.SelectionModeProperty,
            SelectionMode.Single,
            x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
            x => x.AllowMultipleSelection(),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case SelectionMode.Single:
                        y.WithIcon(MaterialIconKind.Check);
                        break;
                    case SelectionMode.AlwaysSelected:
                        y.WithIcon(MaterialIconKind.CheckboxMarked);
                        break;
                    case SelectionMode.Toggle:
                        y.WithIcon(MaterialIconKind.ToggleSwitch);
                        break;
                    case SelectionMode.Multiple:
                        y.WithIcon(MaterialIconKind.CheckAll);
                        break;
                }
            }),

    new ControlThemeBuilder("Icon")
        .AddVariants(CssClass.Vertical, CssClass.Horizontal, CssClass.Uniform, CssClass.Wrap)
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<SelectionMode, ListBoxEditor>(ListBox.SelectionModeProperty,
            SelectionMode.Single,
            x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
            x => x.AllowMultipleSelection(),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case SelectionMode.Single:
                        y.WithIcon(MaterialIconKind.Check);
                        break;
                    case SelectionMode.AlwaysSelected:
                        y.WithIcon(MaterialIconKind.CheckboxMarked);
                        break;
                    case SelectionMode.Toggle:
                        y.WithIcon(MaterialIconKind.ToggleSwitch);
                        break;
                    case SelectionMode.Multiple:
                        y.WithIcon(MaterialIconKind.CheckAll);
                        break;
                }
            }),

    new ControlThemeBuilder("Indicator")
        .AddVariants(CssClass.Vertical, CssClass.Horizontal, CssClass.Uniform, CssClass.Wrap)
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<SelectionMode, ListBoxEditor>(ListBox.SelectionModeProperty,
            SelectionMode.Single,
            x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
            x => x.AllowMultipleSelection(),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case SelectionMode.Single:
                        y.WithIcon(MaterialIconKind.Check);
                        break;
                    case SelectionMode.AlwaysSelected:
                        y.WithIcon(MaterialIconKind.CheckboxMarked);
                        break;
                    case SelectionMode.Toggle:
                        y.WithIcon(MaterialIconKind.ToggleSwitch);
                        break;
                    case SelectionMode.Multiple:
                        y.WithIcon(MaterialIconKind.CheckAll);
                        break;
                }
            }),

    new ControlThemeBuilder()
        .WithKind(CssClass.KindFocus)
        .AddVariants(CssClass.Vertical, CssClass.Horizontal, CssClass.Uniform, CssClass.Wrap)
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<SelectionMode, ListBoxEditor>(ListBox.SelectionModeProperty,
            SelectionMode.Single,
            x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
            x => x.AllowMultipleSelection(),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case SelectionMode.Single:
                        y.WithIcon(MaterialIconKind.Check);
                        break;
                    case SelectionMode.AlwaysSelected:
                        y.WithIcon(MaterialIconKind.CheckboxMarked);
                        break;
                    case SelectionMode.Toggle:
                        y.WithIcon(MaterialIconKind.ToggleSwitch);
                        break;
                    case SelectionMode.Multiple:
                        y.WithIcon(MaterialIconKind.CheckAll);
                        break;
                }
            })
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ListBox;
}
