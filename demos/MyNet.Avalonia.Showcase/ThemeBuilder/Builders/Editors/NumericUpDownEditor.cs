// -----------------------------------------------------------------------
// <copyright file="NumericUpDownEditor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Represents a builder for creating metadata for a combo box editor, allowing to configure the choices available in the combo box. This builder provides a method to add choices to the combo box, and the Build method creates an immutable ComboBoxEditorMetadata instance based on the configured choices. The resulting metadata can be used in the theme builder to define the editor for options that require a combo box selection, providing a way to specify the available options for users to choose from in the UI.
/// </summary>
internal sealed class NumericUpDownEditor : IEditorBuilder
{
    private decimal _minimum;
    private decimal _maximum = 100;
    private decimal _increment = 1;
    private IObservableValue<string>? _suffixFunc;

    /// <summary>
    /// Specifies the minimum value for the NumericUpDown editor.
    /// </summary>
    /// <param name="minimum">The minimum value for the NumericUpDown editor.</param>
    /// <returns>The current instance of the <see cref="NumericUpDownEditor"/>, enabling method chaining.</returns>
    public NumericUpDownEditor WithMinimum(decimal minimum)
    {
        _minimum = minimum;
        return this;
    }

    /// <summary>
    /// Specifies the maximum value for the NumericUpDown editor.
    /// </summary>
    /// <param name="maximum">The maximum value for the NumericUpDown editor.</param>
    /// <returns>The current instance of the <see cref="NumericUpDownEditor"/>, enabling method chaining.</returns>
    public NumericUpDownEditor WithMaximum(decimal maximum)
    {
        _maximum = maximum;
        return this;
    }

    /// <summary>
    /// Sets the minimum and maximum allowable values for the NumericUpDown editor.
    /// </summary>
    /// <remarks>Use this method to define the valid range of values that users can select with the NumericUpDown.
    /// Setting appropriate bounds helps ensure that user input remains within expected limits.</remarks>
    /// <param name="minimum">The lowest value that the NumericUpDown can represent. Must be less than or equal to <paramref name="maximum"/>.</param>
    /// <param name="maximum">The highest value that the NumericUpDown can represent. Must be greater than or equal to <paramref name="minimum"/>.</param>
    /// <returns>The current <see cref="NumericUpDownEditor"/> instance, enabling method chaining.</returns>
    public NumericUpDownEditor WithRange(decimal minimum, decimal maximum)
    {
        _minimum = minimum;
        _maximum = maximum;
        return this;
    }

    /// <summary>
    /// Specifies the increment value for the NumericUpDown editor.
    /// </summary>
    /// <param name="increment">The increment value for the NumericUpDown editor.</param>
    /// <returns>The current instance of the <see cref="NumericUpDownEditor"/>, enabling method chaining.</returns>
    public NumericUpDownEditor WithIncrement(decimal increment)
    {
        _increment = increment;
        return this;
    }

    /// <summary>
    /// Sets the suffix for the choice using the specified value provider.
    /// </summary>
    /// <remarks>Use this method to assign a dynamic suffix that can change based on runtime context or
    /// localization requirements.</remarks>
    /// <param name="displaySuffixFunc">An object that provides the suffix as a string. This provider is invoked to retrieve the suffix when
    /// needed.</param>
    /// <returns>The current instance of the ChoiceMetadataBuilder to enable method chaining.</returns>
    public NumericUpDownEditor DisplaySuffix(IObservableValue<string> displaySuffixFunc)
    {
        _suffixFunc = displaySuffixFunc;
        return this;
    }

    /// <summary>
    /// Sets the suffix for the choice using the specified value provider.
    /// </summary>
    /// <remarks>Use this method to assign a dynamic suffix that can change based on runtime context or
    /// localization requirements.</remarks>
    /// <param name="displaySuffixFunc">An object that provides the suffix as a string. This provider is invoked to retrieve the suffix when
    /// needed.</param>
    /// <returns>The current instance of the ChoiceMetadataBuilder to enable method chaining.</returns>
    public NumericUpDownEditor DisplaySuffix(Func<string?> displaySuffixFunc)
    {
        _suffixFunc = new CultureBoundValue<string>(displaySuffixFunc);
        return this;
    }

    /// <summary>
    /// Sets the suffix for the choice using the specified resource key.
    /// </summary>
    /// <remarks>This method is typically used to define a user-friendly name for the choice that can be
    /// translated based on the application's localization settings.</remarks>
    /// <param name="resourceKey">The resource key used to retrieve the suffix from a localization source.</param>
    /// <returns>The current instance of the ChoiceMetadataBuilder, allowing for method chaining.</returns>
    public NumericUpDownEditor DisplaySuffix(string resourceKey)
    {
        _suffixFunc = new LocalizedString(resourceKey);
        return this;
    }

    /// <summary>
    /// Creates and returns editor metadata for a NumericUpDown editor using the configured minimum, maximum, and increment values.
    /// </summary>
    /// <remarks>Call this method after configuring the minimum, maximum, and increment values to generate the appropriate metadata for a
    /// NumericUpDown editor. The returned metadata can be used to describe editor options in UI frameworks or property
    /// grids.</remarks>
    /// <returns>An instance of <see cref="IEditorMetadata"/> that describes the metadata for a NumericUpDown editor initialized with
    /// the specified minimum, maximum, and increment values.</returns>
    public IEditorMetadata Build() => new NumericUpDownEditorMetadata(_minimum, _maximum, _increment, _suffixFunc);
}

/// <summary>
/// Represents metadata for a NumericUpDown editor, including the minimum, maximum, and increment values for the NumericUpDown.
/// </summary>
/// <remarks>This record is intended to provide the necessary data for rendering a NumericUpDown in a user interface.
/// Ensure that the Minimum, Maximum, and Increment values are set appropriately before using this metadata.</remarks>
/// <param name="Minimum">The minimum value for the NumericUpDown editor.</param>
/// <param name="Maximum">The maximum value for the NumericUpDown editor.</param>
/// <param name="Increment">The increment value for the NumericUpDown editor.</param>
/// <param name="Suffix">The suffix to be displayed alongside the numeric value in the NumericUpDown editor.</param>
internal sealed record NumericUpDownEditorMetadata(decimal Minimum, decimal Maximum, decimal Increment, IObservableValue<string>? Suffix) : IEditorMetadata;
