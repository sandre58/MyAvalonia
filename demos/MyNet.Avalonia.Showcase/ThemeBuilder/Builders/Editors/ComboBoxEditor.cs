// -----------------------------------------------------------------------
// <copyright file="ComboBoxEditor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Represents a builder for creating metadata for a combo box editor, allowing to configure the choices available in the combo box. This builder provides a method to add choices to the combo box, and the Build method creates an immutable ComboBoxEditorMetadata instance based on the configured choices. The resulting metadata can be used in the theme builder to define the editor for options that require a combo box selection, providing a way to specify the available options for users to choose from in the UI.
/// </summary>
internal sealed class ComboBoxEditor : ListEditor<ComboBoxEditor>
{
    private bool _allowNullValue;

    /// <summary>
    /// Allows the combo box editor to accept a null value as a valid selection. When this method is called, it sets the internal flag to indicate that null values are permitted, which can be useful in scenarios where the user may want to indicate "no selection" or "not applicable" in the combo box. By default, if this method is not called, the combo box will not allow null values and will require the user to select one of the provided choices.
    /// </summary>
    /// <returns>The current instance of <see cref="ComboBoxEditor"/> to allow for method chaining when configuring the editor.</returns>
    public ComboBoxEditor AllowNullValue()
    {
        _allowNullValue = true;
        return this;
    }

    /// <summary>
    /// Creates and returns editor metadata for a ComboBox editor using the configured choices.
    /// </summary>
    /// <remarks>Call this method after configuring the choices to generate the appropriate metadata for a
    /// ComboBox editor. The returned metadata can be used to describe editor options in UI frameworks or property
    /// grids.</remarks>
    /// <returns>An instance of <see cref="IEditorMetadata"/> that describes the metadata for a ComboBox editor initialized with
    /// the specified choices.</returns>
    public override IEditorMetadata Build() => new ComboBoxEditorMetadata([..Choices], _allowNullValue);
}

/// <summary>
/// Represents metadata for a ComboBox editor, including the available choices for selection.
/// </summary>
/// <remarks>This record is intended to provide the necessary data for rendering a ComboBox in a user interface.
/// Ensure that the Choices collection is populated with valid options before using this metadata.</remarks>
/// <param name="Choices">The collection of choices available for the ComboBox editor. This collection must not be null and can contain any
/// object type.</param>
/// <param name="AllowNullValue">Indicates whether the ComboBox editor allows a null value as a valid selection. When set to <see langword="true"/>, the editor permits null values, allowing users to indicate "no selection" or "not applicable".</param>
internal sealed record ComboBoxEditorMetadata(object?[] Choices, bool AllowNullValue) : IChoicesEditorMetadata;
