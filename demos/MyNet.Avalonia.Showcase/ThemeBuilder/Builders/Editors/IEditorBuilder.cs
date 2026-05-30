// -----------------------------------------------------------------------
// <copyright file="IEditorBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Represents a builder for creating editor metadata, which defines the configuration and properties of an editor used in the theme builder application. Implementations of this interface are responsible for constructing instances of <see cref="IEditorMetadata"/> based on specific requirements and configurations defined in the builder. By implementing this interface, developers can create custom editors with tailored metadata to be used in the theme builder's options and settings, allowing for flexible and extensible editor configurations within the application.
/// </summary>
internal interface IEditorBuilder
{
    /// <summary>
    /// Builds and returns an instance of <see cref="IEditorMetadata"/> based on the configuration defined in the builder. This method is responsible for constructing the editor metadata, which may include properties such as editor type, validation rules, display settings, and other relevant information that defines how the editor should behave and appear within the theme builder application. The resulting metadata can then be used to configure and customize the editor's functionality and appearance in the application's user interface.
    /// </summary>
    /// <returns>An instance of <see cref="IEditorMetadata"/> representing the configured editor metadata.</returns>
    IEditorMetadata Build();
}
