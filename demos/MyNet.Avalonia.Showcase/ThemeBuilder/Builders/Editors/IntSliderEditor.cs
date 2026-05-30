// -----------------------------------------------------------------------
// <copyright file="IntSliderEditor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Represents a builder for creating metadata for a combo box editor, allowing to configure the choices available in the combo box. This builder provides a method to add choices to the combo box, and the Build method creates an immutable ComboBoxEditorMetadata instance based on the configured choices. The resulting metadata can be used in the theme builder to define the editor for options that require a combo box selection, providing a way to specify the available options for users to choose from in the UI.
/// </summary>
internal sealed class IntSliderEditor : IEditorBuilder
{
    private int _minimum;
    private int _maximum = 100;
    private int _increment = 1;

    /// <summary>
    /// Specifies the minimum value for the slider editor.
    /// </summary>
    /// <param name="minimum">The minimum value for the slider editor.</param>
    /// <returns>The current instance of the <see cref="SliderEditor"/>, enabling method chaining.</returns>
    public IntSliderEditor WithMinimum(int minimum)
    {
        _minimum = minimum;
        return this;
    }

    /// <summary>
    /// Specifies the maximum value for the slider editor.
    /// </summary>
    /// <param name="maximum">The maximum value for the slider editor.</param>
    /// <returns>The current instance of the <see cref="SliderEditor"/>, enabling method chaining.</returns>
    public IntSliderEditor WithMaximum(int maximum)
    {
        _maximum = maximum;
        return this;
    }

    /// <summary>
    /// Sets the minimum and maximum allowable values for the slider editor.
    /// </summary>
    /// <remarks>Use this method to define the valid range of values that users can select with the slider.
    /// Setting appropriate bounds helps ensure that user input remains within expected limits.</remarks>
    /// <param name="minimum">The lowest value that the slider can represent. Must be less than or equal to <paramref name="maximum"/>.</param>
    /// <param name="maximum">The highest value that the slider can represent. Must be greater than or equal to <paramref name="minimum"/>.</param>
    /// <returns>The current <see cref="SliderEditor"/> instance, enabling method chaining.</returns>
    public IntSliderEditor WithRange(int minimum, int maximum)
    {
        _minimum = minimum;
        _maximum = maximum;
        return this;
    }

    /// <summary>
    /// Specifies the increment value for the slider editor.
    /// </summary>
    /// <param name="increment">The increment value for the slider editor.</param>
    /// <returns>The current instance of the <see cref="SliderEditor"/>, enabling method chaining.</returns>
    public IntSliderEditor WithIncrement(int increment)
    {
        _increment = increment;
        return this;
    }

    /// <summary>
    /// Creates and returns editor metadata for a Slider editor using the configured minimum, maximum, and increment values.
    /// </summary>
    /// <remarks>Call this method after configuring the minimum, maximum, and increment values to generate the appropriate metadata for a
    /// Slider editor. The returned metadata can be used to describe editor options in UI frameworks or property
    /// grids.</remarks>
    /// <returns>An instance of <see cref="IEditorMetadata"/> that describes the metadata for a Slider editor initialized with
    /// the specified minimum, maximum, and increment values.</returns>
    public IEditorMetadata Build() => new IntSliderEditorMetadata(_minimum, _maximum, _increment);
}

/// <summary>
/// Represents metadata for a Slider editor, including the minimum, maximum, and increment values for the slider.
/// </summary>
/// <remarks>This record is intended to provide the necessary data for rendering a Slider in a user interface.
/// Ensure that the Minimum, Maximum, and Increment values are set appropriately before using this metadata.</remarks>
/// <param name="Minimum">The minimum value for the slider editor.</param>
/// <param name="Maximum">The maximum value for the slider editor.</param>
/// <param name="Increment">The increment value for the slider editor.</param>
internal sealed record IntSliderEditorMetadata(int Minimum, int Maximum, int Increment) : IEditorMetadata;
