// -----------------------------------------------------------------------
// <copyright file="ListOptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Choices;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// Represents a view model for options that present a list of choices, such as combo boxes or list boxes. This abstract class serves as a base for specific implementations of list-based option view models, providing common functionality for managing the available choices and their display names. The constructor initializes the options collection and sets up the display name function, while derived classes can further customize the behavior and appearance of the option view model as needed.
/// </summary>
/// <param name="definition">The definition of the control option, providing metadata and behavior for the setting.</param>
/// <param name="defaultValue">The initial value for the setting.</param>
/// <param name="options">The collection of choices available for this option.</param>
/// <param name="displayNameFunc">A provider that supplies the display name for the option, enabling localization or dynamic naming.</param>
internal abstract class ListOptionViewModel(IControlOptionDefinition definition, object? defaultValue, ICollection<IChoiceViewModel> options, IProvideValue<string> displayNameFunc)
    : ValueOptionViewModel<object>(definition, defaultValue, displayNameFunc)
{
    /// <summary>
    /// Gets the collection of available choices for selection.
    /// </summary>
    /// <remarks>This property provides an observable collection of choice view models, enabling dynamic
    /// updates to the user interface when the set of options changes. It is suitable for data binding scenarios, such
    /// as populating dropdown lists or selection controls in the UI.</remarks>
    public ObservableCollection<IChoiceViewModel> Options { get; } = new(options);
}

/// <summary>
/// Represents a view model for a combo box option, providing the option's definition, available choices, display name
/// logic, and an optional default value.
/// </summary>
/// <remarks>This class is intended for internal use and derives from ListOptionViewModel, inheriting its core
/// functionality for managing list-based options.</remarks>
/// <param name="definition">The definition of the control option, specifying the characteristics and behavior of the combo box option.</param>
/// <param name="options">A collection of choice view models that represent the selectable options available in the combo box.</param>
/// <param name="displayNameFunc">A provider that supplies the display name for the combo box option, allowing for dynamic or context-sensitive
/// naming.</param>
internal sealed class ComboBoxOptionViewModel(IControlOptionDefinition definition, ICollection<IChoiceViewModel> options, IProvideValue<string> displayNameFunc)
    : ListOptionViewModel(definition, definition.DefaultValue, options, displayNameFunc)
{
    /// <summary>
    /// Gets or sets a value indicating whether null values are allowed.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the property allows null values to be assigned. If set to
    /// <see langword="false"/>, assigning a null value will result in an exception being thrown.</remarks>
    public bool AllowNullValue { get; set; }
}

/// <summary>
/// Represents a view model for a list box option, encapsulating the option's definition, available choices, display
/// name provider, and an optional default value for selection. Intended for use in scenarios where a list box control
/// is bound to a set of selectable options.
/// </summary>
/// <remarks>This class is intended for internal use to facilitate the binding and management of selectable
/// options within a list box control. It enables dynamic display names and supports specifying a default
/// selection.</remarks>
/// <param name="definition">The definition of the control option, specifying the characteristics and behavior of the list box option.</param>
/// <param name="options">A collection of choice view models that represent the selectable options available in the list box.</param>
/// <param name="displayNameFunc">A provider that supplies the display name for the option, allowing for dynamic or context-sensitive naming.</param>
/// <param name="allowMultipleValues">A value indicating whether multiple selections are allowed in the list box. The default is <see langword="false"/>, allowing only single selection.</param>
internal sealed class ListBoxOptionViewModel(IControlOptionDefinition definition, ICollection<IChoiceViewModel> options, IProvideValue<string> displayNameFunc, bool allowMultipleValues = false)
    : ListOptionViewModel(definition, allowMultipleValues && definition.DefaultValue is not IEnumerable ? new ObservableCollection<object?> { definition.DefaultValue } : definition.DefaultValue, options, displayNameFunc)
{
    /// <summary>
    /// Gets the selection mode for the list of options, determining whether single or multiple selections are allowed. The default value is set to <see cref="SelectionMode.Single"/>, indicating that only one option can be selected at a time. This property can be configured to allow multiple selections if needed, depending on the requirements of the specific control or user interface scenario.
    /// </summary>
    public SelectionMode SelectionMode { get; } = allowMultipleValues ? SelectionMode.Multiple | SelectionMode.Toggle : SelectionMode.Single;
}
