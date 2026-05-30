// -----------------------------------------------------------------------
// <copyright file="ListBoxEditor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Provides a builder for configuring and creating a list box editor with customizable choices and selection options.
/// </summary>
/// <remarks>Use this class to fluently specify the available choices and whether multiple selections are allowed
/// when constructing a list box editor. The builder pattern enables chaining configuration methods before generating
/// the final editor metadata.</remarks>
internal sealed class ListBoxEditor : ListEditor<ListBoxEditor>
{
    private bool _allowMultipleSelection;

    /// <summary>
    /// Enables support for selecting multiple items in the list box editor.
    /// </summary>
    /// <remarks>Call this method before rendering the list box editor to allow users to select more than one
    /// item. This setting affects the selection behavior of the editor and may impact how selected values are
    /// processed.</remarks>
    /// <returns>The current instance of the <see cref="ListBoxEditor"/>, allowing for method chaining.</returns>
    public ListBoxEditor AllowMultipleSelection()
    {
        _allowMultipleSelection = true;
        return this;
    }

    /// <summary>
    /// Builds and returns an instance of <see cref="IEditorMetadata"/> containing the configured choices and selection options for the list box editor.
    /// </summary>
    /// <returns>An instance of <see cref="IEditorMetadata"/> representing the configured list box editor.</returns>
    public override IEditorMetadata Build() => new ListBoxEditorMetadata([..Choices], _allowMultipleSelection);
}

/// <summary>
/// Represents metadata for a list box editor, including the available choices and selection options.
/// </summary>
/// <remarks>This metadata is used to configure the behavior of the list box editor, particularly in scenarios
/// where user selection is required.</remarks>
/// <param name="Choices">The collection of choices available for selection in the list box editor. This collection must not be null or empty.</param>
/// <param name="AllowMultipleSelection">Indicates whether multiple selections are allowed in the list box. Defaults to <see langword="false"/>, meaning only
/// a single selection is permitted.</param>
internal sealed record ListBoxEditorMetadata(object?[] Choices, bool AllowMultipleSelection = false) : IChoicesEditorMetadata;
