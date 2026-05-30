// -----------------------------------------------------------------------
// <copyright file="ToggleSwitchEditor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Represents a builder for creating metadata for a ToggleSwitch editor, which is used to configure the behavior and appearance of a toggle switch control in the user interface. This builder implements the <see cref="IEditorBuilder"/> interface, providing a method to build the metadata for the ToggleSwitch editor. The resulting metadata can be used in the theme builder to define how the ToggleSwitch editor should function within the application, allowing for customization of its properties and behavior as needed.
/// </summary>
internal sealed class ToggleSwitchEditor : IEditorBuilder
{
    /// <summary>
    /// Builds the metadata for a ToggleSwitch editor, creating an instance of <see cref="ToggleSwitchEditorMetadata"/> that represents the configuration for the ToggleSwitch editor. This method is responsible for constructing the metadata based on the settings defined in the builder, allowing for customization of the ToggleSwitch editor's behavior and appearance in the user interface. The resulting metadata can then be used in the theme builder to define how the ToggleSwitch editor should function within the application.
    /// </summary>
    /// <returns>An instance of <see cref="ToggleSwitchEditorMetadata"/> that describes the metadata for the ToggleSwitch editor.</returns>
    public IEditorMetadata Build() => new ToggleSwitchEditorMetadata();
}

/// <summary>
/// Represents the metadata for a ToggleSwitch editor, which includes the minimum, maximum, and increment values that define the behavior of the slider editor. This record implements the <see cref="IEditorMetadata"/> interface, providing a concrete implementation for describing the metadata associated with a ToggleSwitch editor in a theming system. The properties of this record allow for configuring the range and step values for the slider editor, enabling customization of its behavior in the user interface.
/// </summary>
internal sealed record ToggleSwitchEditorMetadata : IEditorMetadata;
