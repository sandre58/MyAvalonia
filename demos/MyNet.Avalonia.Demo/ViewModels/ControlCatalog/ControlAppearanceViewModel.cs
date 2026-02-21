// -----------------------------------------------------------------------
// <copyright file="ControlAppearanceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Styling;
using DynamicData.Binding;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Observable;
using MyNet.Utilities;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

/// <summary>
/// View model for managing control appearance settings including themes, variants, roles, sizes, and shapes.
/// </summary>
internal sealed class ControlAppearanceViewModel : ObservableObject
{
    private readonly IEnumerable<ControlThemeDefinition> _themes;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlAppearanceViewModel"/> class.
    /// </summary>
    /// <param name="themes">The collection of available theme definitions.</param>
    public ControlAppearanceViewModel(IEnumerable<ControlThemeDefinition> themes)
    {
        var controlThemeDefinitions = themes.ToList();
        _themes = controlThemeDefinitions;
        AvailableThemes = controlThemeDefinitions.Skip(1).ToObservableCollection();
        Reset();

        Disposables.AddRange(
            [
                this.WhenPropertyChanged(x => x.SelectedTheme).Subscribe(_ => ResetFrom(GetActiveThemeDefinition())),
                SelectedVariants.ToObservableChangeSet().Subscribe(_ => OnPropertyChanged(nameof(ComputedClasses)))
            ]);
    }

    /// <summary>
    /// Gets the collection of available themes.
    /// </summary>
    public ObservableCollection<ControlThemeDefinition> AvailableThemes { get; }

    /// <summary>
    /// Gets the collection of available variants.
    /// </summary>
    public ObservableCollection<VariantDefinition> AvailableVariants { get; } = [];

    /// <summary>
    /// Gets the collection of available roles.
    /// </summary>
    public ObservableCollection<RoleDefinition> AvailableRoles { get; } = [];

    /// <summary>
    /// Gets the collection of available roles for items.
    /// </summary>
    public ObservableCollection<RoleDefinition> AvailableItemsRoles { get; } = [];

    /// <summary>
    /// Gets the collection of available sizes.
    /// </summary>
    public ObservableCollection<SizeDefinition> AvailableSizes { get; } = [];

    /// <summary>
    /// Gets the collection of available shapes.
    /// </summary>
    public ObservableCollection<ShapeDefinition> AvailableShapes { get; } = [];

    /// <summary>
    /// Gets or sets the currently selected theme.
    /// </summary>
    [AlsoNotifyFor(nameof(ActiveTheme))]
    public ControlThemeDefinition? SelectedTheme { get; set; }

    /// <summary>
    /// Gets the active control theme based on the selected theme.
    /// </summary>
    public ControlTheme? ActiveTheme => GetActiveThemeDefinition()?.Theme;

    /// <summary>
    /// Gets the collection of currently selected variants.
    /// </summary>
    public ObservableCollection<VariantDefinition> SelectedVariants { get; } = [];

    /// <summary>
    /// Gets or sets the currently selected role.
    /// </summary>
    [AlsoNotifyFor(nameof(ActiveRole))]
    public RoleDefinition? SelectedRole { get; set; }

    /// <summary>
    /// Gets or sets the currently selected role for items.
    /// </summary>
    [AlsoNotifyFor(nameof(ActiveItemsRole))]
    public RoleDefinition? SelectedItemsRole { get; set; }

    /// <summary>
    /// Gets or sets the currently selected size.
    /// </summary>
    [AlsoNotifyFor(nameof(ComputedClasses))]
    public SizeDefinition? SelectedSize { get; set; }

    /// <summary>
    /// Gets or sets the currently selected shape.
    /// </summary>
    [AlsoNotifyFor(nameof(ComputedClasses))]
    public ShapeDefinition? SelectedShape { get; set; }

    /// <summary>
    /// Gets the active theme role based on the selected role.
    /// </summary>
    public ThemeRole ActiveRole => SelectedRole?.Role ?? ThemeRole.Default;

    /// <summary>
    /// Gets the active theme role based on the selected role for items.
    /// </summary>
    public ThemeRole ActiveItemsRole => SelectedItemsRole?.Role ?? ThemeRole.Default;

    /// <summary>
    /// Gets the computed CSS classes based on selected variants, size, and shape.
    /// </summary>
    public string[] ComputedClasses
    {
        get
        {
            var classes = new List<string>();
            var activeTheme = GetActiveThemeDefinition();

            if (activeTheme is not null && !string.IsNullOrEmpty(activeTheme.Kind))
                classes.Add(activeTheme.Kind);

            if (SelectedShape is not null)
                classes.Add(SelectedShape.Class);

            foreach (var variant in SelectedVariants)
                classes.AddRange(variant.Classes);

            if (SelectedSize is not null)
                classes.Add(SelectedSize.Class);

            return [.. classes.NotNullOrEmpty().Distinct()];
        }
    }

    /// <summary>
    /// Retrieves the currently selected theme definition, or returns the first available theme if none is selected.
    /// </summary>
    /// <returns>The active <see cref="ControlThemeDefinition"/> instance if a theme is selected; otherwise, the first theme in
    /// the collection, or <see langword="null"/> if no themes are available.</returns>
    public ControlThemeDefinition? GetActiveThemeDefinition() => SelectedTheme ?? _themes.FirstOrDefault();

    /// <summary>
    /// Resets the appearance settings to default values.
    /// </summary>
    public void Reset()
    {
        SelectedTheme = null;
        ResetFrom(GetActiveThemeDefinition());
    }

    /// <summary>
    /// Resets the appearance settings based on a specific theme definition.
    /// </summary>
    /// <param name="definition">The theme definition to reset from.</param>
    private void ResetFrom(ControlThemeDefinition? definition)
    {
        SelectedVariants.Clear();
        SelectedSize = null;
        SelectedShape = null;
        SelectedRole = null;
        SelectedItemsRole = null;

        AvailableVariants.Set(definition?.Variants ?? []);
        AvailableShapes.Set(definition?.Shapes ?? []);
        AvailableRoles.Set(definition?.Roles ?? []);
        AvailableItemsRoles.Set(definition?.ItemsRoles ?? []);
        AvailableSizes.Set(definition?.Sizes ?? []);

        SelectedRole = AvailableRoles.FirstOrDefault();
        SelectedItemsRole = AvailableItemsRoles.FirstOrDefault();
    }
}
