// -----------------------------------------------------------------------
// <copyright file="IChoicesEditorMetadata.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

/// <summary>
/// Provides metadata for a choices editor, which is a specific type of editor that allows users to select from a predefined set of options. This interface can be implemented to define the structure and behavior of editors that present choices to users, such as combo boxes or list boxes. By implementing this interface, developers can create custom metadata that describes the available choices and how they should be displayed in the user interface.
/// </summary>
internal interface IChoicesEditorMetadata : IEditorMetadata
{
    /// <summary>
    /// Gets the available choices for selection.
    /// </summary>
    /// <remarks>The returned array contains the set of options that can be presented to the user. The
    /// specific contents and types of the objects in the array may vary depending on the context in which the property
    /// is used.</remarks>
    object?[] Choices { get; }
}
