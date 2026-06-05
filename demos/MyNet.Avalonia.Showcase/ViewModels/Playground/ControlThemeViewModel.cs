// -----------------------------------------------------------------------
// <copyright file="ControlThemeViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Choices;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Options;
using MyNet.Collections;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground;

/// <summary>
/// View model for the control playground, which provides interactive testing and preview functionality.
/// </summary>
internal sealed class ControlThemeViewModel(ControlThemeDefinition definition, IObservableValue<string> displayName) : ObservableObject
{
    /// <summary>
    /// Gets the definition of the control theme, which contains the configuration and properties for the theme being previewed in the playground.
    /// </summary>
    public IObservableValue<string> DisplayName { get; } = displayName;

    /// <summary>
    /// Gets the definition of the control theme, which specifies the visual appearance and behavior of the control.
    /// </summary>
    /// <remarks>This property provides access to the theme definition used by the control, allowing
    /// customization of its visual style.</remarks>
    public ControlThemeDefinition Definition { get; } = definition;

    /// <summary>
    /// Gets the collection of available variants.
    /// </summary>
    public required IReadOnlyCollection<ClassChoiceViewModel> AvailableVariants { get; init; }

    /// <summary>
    /// Gets the collection of available roles.
    /// </summary>
    public required IReadOnlyCollection<RoleChoiceViewModel> AvailableRoles { get; init; }

    /// <summary>
    /// Gets the collection of available roles for items.
    /// </summary>
    public required IReadOnlyCollection<RoleChoiceViewModel> AvailableItemsRoles { get; init; }

    /// <summary>
    /// Gets the collection of available sizes.
    /// </summary>
    public required IReadOnlyCollection<ClassChoiceViewModel> AvailableSizes { get; init; }

    /// <summary>
    /// Gets the collection of available shapes.
    /// </summary>
    public required IReadOnlyCollection<ClassChoiceViewModel> AvailableShapes { get; init; }

    /// <summary>
    /// Gets the collection of available properties.
    /// </summary>
    public required IReadOnlyCollection<OptionViewModel> AvailableOptions { get; init; }

    /// <summary>
    /// Gets the collection of available groups.
    /// </summary>
    public required IReadOnlyCollection<GroupOptionViewModel> AvailableGroups { get; init; }

    /// <summary>
    /// Gets the collection of available items.
    /// </summary>
    public IReadOnlyCollection<object> AvailableItems => [.. AvailableOptions, .. AvailableGroups];

    /// <summary>
    /// Gets a value indicating whether to use the icon by default when rendering the control in the playground. This property allows toggling the default display of the icon, providing flexibility in how the control is presented during testing and previewing.
    /// </summary>
    public bool UseIconByDefault { get; init; }

    /// <summary>
    /// Computes the style properties based on the available options and their definitions. It filters the options to include only those that provide style properties, then transforms them into a collection of StyleProperty instances.
    /// </summary>
    /// <returns>A collection of StyleProperty instances representing the computed style properties.</returns>
    public IEnumerable<StyleProperty> ComputeStyleProperties()
        => AvailableOptions.Concat(AvailableGroups.SelectMany(x => x.Options)).OfType<ValueOptionViewModel>().Where(x => x.Definition is IProvideStyleProperty).Select(x => ((IProvideStyleProperty)x.Definition).ProvideStyleProperty(x.Value)).NotNull();

    /// <summary>
    /// Retrieves a collection of class names provided by available options that implement the IProvideClasses
    /// interface.
    /// </summary>
    /// <remarks>This method filters the available options to those that can provide classes and aggregates
    /// the results. Ensure that the available options are properly defined to utilize this method
    /// effectively.</remarks>
    /// <returns>An enumerable collection of strings representing the class names. The collection will be empty if no classes are
    /// provided by the available options.</returns>
    public IEnumerable<string> ComputeClasses()
        => AvailableOptions.Concat(AvailableGroups.SelectMany(x => x.Options)).OfType<ValueOptionViewModel>().Where(x => x.Definition is IProvideClasses).SelectMany(x => ((IProvideClasses)x.Definition).ProvideClasses(x.Value)).NotNullOrEmpty();

    /// <summary>
    /// Computes a collection of style actions based on the available options and their definitions.
    /// </summary>
    /// <remarks>This method creates actions for both control actions and custom options that have value
    /// change handlers. Only actions with valid handlers are included in the result.</remarks>
    /// <returns>An enumerable collection of StyleAction instances representing the computed actions. The collection contains
    /// only non-null actions.</returns>
    public IEnumerable<StyleAction> ComputeActions()
    {
        var actions = new List<StyleAction?>();

        // Custom actions
        actions.AddRange(AvailableOptions.Concat(AvailableGroups.SelectMany(x => x.Options)).OfType<ActionOptionViewModel>().Select(x => new StyleAction((control, _) => ((ControlActionDefinition)x.Definition).Action(control), x.ExecuteSubject)));

        // Custom options
        actions.AddRange(AvailableOptions.Concat(AvailableGroups.SelectMany(x => x.Options)).OfType<ValueOptionViewModel>().Where(x => x.Definition.OnValueChanged is not null).Select(x => new StyleAction(x.Definition.OnValueChanged, x.ValueChangedSubject)));

        return actions.NotNull();
    }
}
