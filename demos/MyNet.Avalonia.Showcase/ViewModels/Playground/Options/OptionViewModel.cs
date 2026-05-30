// -----------------------------------------------------------------------
// <copyright file="OptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Observable;
using MyNet.Observable.Attributes;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// Represents an abstract base class for editable settings that provides a display name for use in user interfaces.
/// </summary>
/// <remarks>Inherit from this class to implement view models for settings that can be edited and require a
/// user-friendly display name. The display name is intended for display purposes in UI components such as forms or
/// property grids.</remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="OptionViewModel"/> class with the specified control option definition, display name provider, and optional icon. The constructor sets up the necessary properties for the view model, including the control option definition that provides metadata and configuration for the setting, the display name provider that supplies a user-friendly name for the setting in the UI, and an optional icon that can be used for visual representation. The value of the setting is initialized to the default value defined in the control option definition, allowing for a consistent starting state when the view model is created.
/// </remarks>
/// <param name="definition">The control option definition associated with this setting.</param>
/// <param name="displayNameFunc">A provider that supplies the display name for the setting, used to present the setting in the UI. Cannot be null.</param>
/// <param name="icon">An optional icon associated with the setting, which can be used for visual representation in the user interface. The icon can be of any type, such as a string representing a resource path, an image object, or any other relevant representation depending on the UI framework being used. This property allows for enhanced visual cues when displaying the setting in the UI, making it easier for users to identify and understand the purpose of the setting at a glance.</param>
internal abstract class OptionViewModel(IControlOptionDefinition definition, IProvideValue<string> displayNameFunc, MaterialIconKind? icon = null) : EditableObject
{
    /// <summary>
    /// Gets the control option definition associated with this setting. This property provides access to the metadata and configuration defined for the control option, allowing the view model to interact with the underlying definition when necessary. The definition is strongly typed as <see cref="IControlOptionDefinition"/> to ensure that it adheres to the expected structure and behavior for control options in the theming system.
    /// </summary>
    public IControlOptionDefinition Definition { get; } = definition;

    /// <summary>
    /// Gets the display name to show for this setting in the UI.
    /// </summary>
    [CanSetIsModified(false)]
    public IProvideValue<string> DisplayName { get; } = displayNameFunc;

    /// <summary>
    /// Gets an optional icon associated with the setting, which can be used for visual representation in the user interface. The icon can be of any type, such as a string representing a resource path, an image object, or any other relevant representation depending on the UI framework being used. This property allows for enhanced visual cues when displaying the setting in the UI, making it easier for users to identify and understand the purpose of the setting at a glance.
    /// </summary>
    public MaterialIconKind? Icon { get; } = icon;
}
