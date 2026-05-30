// -----------------------------------------------------------------------
// <copyright file="ButtonEditor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;
using MyNet.Avalonia.Theme.Theming.Core;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Represents a builder for creating metadata for a Button editor, which is used to configure the behavior and appearance of a button control in the user interface. This builder implements the <see cref="IEditorBuilder"/> interface, providing a method to build the metadata for the Button editor. The resulting metadata can be used in the theme builder to define how the Button editor should function within the application, allowing for customization of its properties and behavior as needed.
/// </summary>
internal sealed class ButtonEditor : IEditorBuilder
{
    private ThemeRole _role;

    /// <summary>
    /// Sets the role of the Button editor, which defines the thematic role or purpose of the button in the user interface. The role can be used to determine the styling and behavior of the button based on its intended use, such as primary actions, secondary actions, or other contextual roles. This method allows for configuring the role of the Button editor, enabling customization of its appearance and functionality in the application.
    /// </summary>
    /// <param name="role">The thematic role to assign to the Button editor.</param>
    /// <returns>The current instance of <see cref="ButtonEditor"/> for method chaining.</returns>
    public ButtonEditor WithRole(ThemeRole role)
    {
        _role = role;
        return this;
    }

    /// <summary>
    /// Builds the metadata for a Button editor, creating an instance of <see cref="ButtonEditorMetadata"/> that represents the configuration for the Button editor. This method is responsible for constructing the metadata based on the settings defined in the builder, allowing for customization of the Button editor's behavior and appearance in the user interface. The resulting metadata can then be used in the theme builder to define how the Button editor should function within the application.
    /// </summary>
    /// <returns>An instance of <see cref="ButtonEditorMetadata"/> that describes the metadata for the Button editor.</returns>
    public IEditorMetadata Build() => new ButtonEditorMetadata(_role);
}

/// <summary>
/// Represents the metadata for a Button editor, which includes the minimum, maximum, and increment values that define the behavior of the slider editor. This record implements the <see cref="IEditorMetadata"/> interface, providing a concrete implementation for describing the metadata associated with a Button editor in a theming system. The properties of this record allow for configuring the range and step values for the slider editor, enabling customization of its behavior in the user interface.
/// </summary>
internal sealed record ButtonEditorMetadata(ThemeRole Role) : IEditorMetadata;
