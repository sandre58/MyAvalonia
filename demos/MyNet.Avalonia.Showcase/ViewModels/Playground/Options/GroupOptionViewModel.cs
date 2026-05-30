// -----------------------------------------------------------------------
// <copyright file="GroupOptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Material.Icons;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// Represents a group of related options in the user interface, providing a display name, an optional icon, and a
/// collection of individual options.
/// </summary>
/// <remarks>This class is designed to facilitate the organization and management of related options, ensuring
/// that any changes to the options collection are reflected in the UI. The options collection is observable, so
/// modifications to the group are automatically propagated to the user interface.</remarks>
/// <param name="options">The collection of options that belong to this group. Each option is represented by an instance of OptionViewModel
/// and is used to organize related settings within the group. Cannot be null.</param>
/// <param name="displayNameFunc">A function that provides the display name for this group, used for display in the user interface. Cannot be null.</param>
/// <param name="icon">An optional icon associated with the group, used to enhance visual representation in the user interface. May be null
/// if no icon is desired.</param>
internal sealed class GroupOptionViewModel(ICollection<OptionViewModel> options, IProvideValue<string> displayNameFunc, MaterialIconKind? icon = null) : ObservableObject
{
    /// <summary>
    /// Gets the display name to show for this setting in the UI.
    /// </summary>
    public IProvideValue<string> DisplayName { get; } = displayNameFunc;

    /// <summary>
    /// Gets an optional icon associated with the setting, which can be used for visual representation in the user interface. The icon can be of any type, such as a string representing a resource path, an image object, or any other relevant representation depending on the UI framework being used. This property allows for enhanced visual cues when displaying the setting in the UI, making it easier for users to identify and understand the purpose of the setting at a glance.
    /// </summary>
    public MaterialIconKind? Icon { get; } = icon;

    /// <summary>
    /// Gets the collection of options that belong to this group. This property provides access to the individual settings that are part of the group, allowing for organized management and display of related options in the user interface. The collection is an observable collection, which means that any changes to the collection (such as adding or removing options) will automatically notify the UI to update accordingly, ensuring that the display remains consistent with the underlying data. Each option in the collection is represented by an instance of <see cref="OptionViewModel"/>, which provides the necessary functionality to manage the state and display of each individual setting within the group.
    /// </summary>
    public ObservableCollection<OptionViewModel> Options { get; } = new(options);
}
