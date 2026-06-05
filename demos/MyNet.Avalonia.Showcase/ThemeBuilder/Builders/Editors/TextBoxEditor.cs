// -----------------------------------------------------------------------
// <copyright file="TextBoxEditor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Represents a builder for creating metadata for a TextBox editor, which is used to configure the behavior and appearance of a text box control in the user interface. This builder implements the <see cref="IEditorBuilder"/> interface, providing a method to build the metadata for the TextBox editor. The resulting metadata can be used in the theme builder to define how the TextBox editor should function within the application, allowing for customization of its properties and behavior as needed.
/// </summary>
internal sealed class TextBoxEditor : IEditorBuilder
{
    private string? _value;
    private bool _isMultiline = false;
    private RandomizeText _randomizeText = RandomizeText.Words;

    /// <summary>
    /// Sets the initial value of the TextBox editor. This method allows you to specify the default text that will be displayed in the TextBox when it is rendered in the user interface. By providing a value, you can pre-populate the TextBox with specific content, which can be useful for scenarios where you want to provide a default input or guide the user on what to enter. The method returns the current instance of <see cref="TextBoxEditor"/> to allow for fluent configuration of additional properties if needed.
    /// </summary>
    /// <param name="value">The initial value to set for the TextBox editor.</param>
    /// <returns>The current instance of <see cref="TextBoxEditor"/> for fluent configuration.</returns>
    public TextBoxEditor WithValue(string value)
    {
        _value = value;
        return this;
    }

    public TextBoxEditor WithIsMultiline(bool isMultiline)
    {
        _isMultiline = isMultiline;
        return this;
    }

    /// <summary>
    /// Sets the randomization mode for the TextBox editor. This method allows you to specify how the text in the TextBox should be randomized, providing options such as randomizing words, sentences, or paragraphs. The method returns the current instance of <see cref="TextBoxEditor"/> to allow for fluent configuration of additional properties if needed.
    /// </summary>
    /// <param name="randomizeText">The randomization mode to set for the TextBox editor.</param>
    /// <returns>The current instance of <see cref="TextBoxEditor"/> for fluent configuration.</returns>
    public TextBoxEditor WithRandomizeText(RandomizeText randomizeText)
    {
        _randomizeText = randomizeText;

        if (_randomizeText is RandomizeText.Paragraph or RandomizeText.Sentence)
            _isMultiline = true;

        return this;
    }

    /// <summary>
    /// Builds the metadata for the TextBox editor, creating an instance of <see cref="TextBoxEditorMetadata"/> that represents the configuration for the TextBox editor. This method is responsible for constructing the metadata based on the settings defined in the builder, allowing for customization of the TextBox editor's behavior and appearance in the user interface. The resulting metadata can then be used in the theme builder to define how the TextBox editor should function within the application.
    /// </summary>
    /// <returns>An instance of <see cref="TextBoxEditorMetadata"/> that describes the metadata for the TextBox editor.</returns>
    public IEditorMetadata Build() => new TextBoxEditorMetadata(_value, _isMultiline, _randomizeText);
}

/// <summary>
/// Represents the metadata for a TextBox editor, which includes the minimum, maximum, and increment values that define the behavior of the slider editor. This record implements the <see cref="IEditorMetadata"/> interface, providing a concrete implementation for describing the metadata associated with a TextBox editor in a theming system. The properties of this record allow for configuring the range and step values for the slider editor, enabling customization of its behavior in the user interface.
/// </summary>
internal sealed record TextBoxEditorMetadata(string? Value, bool IsMultiline, RandomizeText RandomizeText) : IEditorMetadata;
