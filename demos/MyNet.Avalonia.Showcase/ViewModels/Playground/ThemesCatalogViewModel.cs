// -----------------------------------------------------------------------
// <copyright file="ThemesCatalogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData.Binding;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Observable;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground;

/// <summary>
/// Represents a view model for the themes catalog. Exposes the full matrix of Variants, Roles,
/// ItemsRoles, Sizes and Shapes available for the selected <see cref="ControlThemeViewModel"/>.
/// </summary>
internal sealed class ThemesCatalogViewModel : ObservableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThemesCatalogViewModel"/> class.
    /// </summary>
    public ThemesCatalogViewModel(ObservableCollection<ControlThemeViewModel> themes)
    {
        Themes = new(themes);

        Disposables.Add(this.WhenPropertyChanged(x => x.SelectedTheme).Subscribe(_ => RefreshItems()));

        SelectedTheme = Themes.FirstOrDefault();
    }

    /// <summary>
    /// Gets the collection of available control themes.
    /// </summary>
    public ReadOnlyObservableCollection<ControlThemeViewModel> Themes { get; }

    /// <summary>
    /// Gets or sets the currently active theme.
    /// </summary>
    public ControlThemeViewModel? SelectedTheme { get; set; }

    /// <summary>
    /// Gets the catalog items for each Variant.
    /// </summary>
    public IReadOnlyList<CatalogSectionItem> VariantItems { get; private set; } = [];

    /// <summary>
    /// Gets the catalog items for each Role.
    /// </summary>
    public IReadOnlyList<CatalogSectionItem> RoleItems { get; private set; } = [];

    /// <summary>
    /// Gets the catalog items for each ItemsRole.
    /// </summary>
    public IReadOnlyList<CatalogSectionItem> ItemsRoleItems { get; private set; } = [];

    /// <summary>
    /// Gets the catalog items for each Size.
    /// </summary>
    public IReadOnlyList<CatalogSectionItem> SizeItems { get; private set; } = [];

    /// <summary>
    /// Gets the catalog items for each Shape.
    /// </summary>
    public IReadOnlyList<CatalogSectionItem> ShapeItems { get; private set; } = [];

    /// <summary>
    /// Refreshes the available items for the currently selected theme, including variants, roles, item roles, sizes,
    /// and shapes.
    /// </summary>
    /// <remarks>If no theme is selected, all item collections are cleared. This method should be called when
    /// the selected theme changes to ensure that the UI reflects the current theme's available options.</remarks>
    private void RefreshItems()
    {
        if (SelectedTheme is null)
        {
            VariantItems = [];
            RoleItems = [];
            ItemsRoleItems = [];
            SizeItems = [];
            ShapeItems = [];
            return;
        }

        var def = SelectedTheme.Definition;

        VariantItems = [.. SelectedTheme.AvailableVariants.Select(v => new CatalogSectionItem(build(extraClass: v.Value, contenDefaultValue: v.Value?.Name), v.DisplayName.Value))];
        RoleItems = [.. SelectedTheme.AvailableRoles.Select(r => new CatalogSectionItem(build(role: r.Value, contenDefaultValue: r.Value.ToString()), r.DisplayName.Value))];
        ItemsRoleItems = [.. SelectedTheme.AvailableItemsRoles.Select(r => new CatalogSectionItem(build(itemsRole: r.Value, contenDefaultValue: r.Value.ToString()), r.DisplayName.Value))];
        SizeItems = [.. SelectedTheme.AvailableSizes.Select(s => new CatalogSectionItem(build(extraClass: s.Value, contenDefaultValue: s.Value?.Name), s.DisplayName.Value))];
        ShapeItems = [.. SelectedTheme.AvailableShapes.Select(s => new CatalogSectionItem(build(extraClass: s.Value, contenDefaultValue: s.Value?.Name), s.DisplayName.Value))];

        ControlStyle build(ThemeRole role = ThemeRole.Default, ThemeRole itemsRole = ThemeRole.Default, CssClass? extraClass = null, string? contenDefaultValue = null)
        {
            var classes = new List<string>();
            if (def.Kind is not null) classes.Add(def.Kind.ToString());
            if (extraClass is not null) classes.Add(extraClass.ToString());

            var properties = new List<StyleProperty>
            {
                StyleProperty.FromProperty(ThemeAssist.RoleProperty, role),
                StyleProperty.FromProperty(ItemsAssist.RoleProperty, itemsRole)
            };

            if (def.ContentDefinition is not null)
            {
                var content = def.ContentDefinition.ContentProviderType switch
                {
                    ContentProviderType.Text => (object?)contenDefaultValue,
                    ContentProviderType.Icon => RandomGenerator.Enum<MaterialIconKind>(),
                    _ => null
                };

                properties.Add(StyleProperty.FromProperty(def.ContentDefinition.Property, content));
            }

            return new()
            {
                Classes = classes,
                Theme = def.Theme,
                Properties = properties
            };
        }
    }
}

/// <summary>
/// Represents a single control instance in the themes catalog, pairing a <see cref="ControlStyle"/>
/// with a human-readable display name shown as a label below the rendered control.
/// </summary>
internal sealed class CatalogSectionItem(ControlStyle definition, string displayName)
{
    /// <summary>
    /// Gets the control definition used to render the control preview.
    /// </summary>
    public ControlStyle Definition { get; } = definition;

    /// <summary>
    /// Gets the label displayed below the rendered control.
    /// </summary>
    public string DisplayName { get; } = displayName;
}
