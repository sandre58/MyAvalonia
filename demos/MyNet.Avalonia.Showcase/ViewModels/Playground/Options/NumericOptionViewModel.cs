// -----------------------------------------------------------------------
// <copyright file="NumericOptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// Represents a view model for a numeric option, providing properties for minimum, maximum, and increment values to define the range and step of the numeric input. This abstract class serves as a base for specific numeric option view models, such as sliders or spin boxes, allowing for consistent handling of numeric options across different types of controls in the user interface.
/// </summary>
/// <typeparam name="T">The type of the numeric value.</typeparam>
/// <param name="definition">The control option definition that supplies metadata and configuration for the numeric option.</param>
/// <param name="displayNameFunc">A provider that returns the display name for the numeric option, allowing for dynamic or context-sensitive naming.</param>
/// <param name="minimum">The minimum allowed value for the numeric option.</param>
/// <param name="maximum">The maximum allowed value for the numeric option.</param>
/// <param name="increment">The increment step value for the numeric option.</param>
internal abstract class NumericOptionViewModel<T>(IControlOptionDefinition definition, IProvideValue<string> displayNameFunc, T minimum, T maximum, T increment) : ValueOptionViewModel<T>(definition, definition.DefaultValue, displayNameFunc)
{
    /// <summary>
    /// Gets the minimum allowed value for the numeric option. This property is initialized to 0 by default, but can be set to any desired minimum value as needed. The minimum value defines the lower bound for the numeric input, ensuring that users cannot enter a value below this threshold.
    /// </summary>
    public T Minimum { get; init; } = minimum;

    /// <summary>
    /// Gets the maximum allowed value for the numeric option. This property is initialized to 100 by default, but can be set to any desired maximum value as needed. The maximum value defines the upper bound for the numeric input, ensuring that users cannot enter a value above this threshold.
    /// </summary>
    public T Maximum { get; init; } = maximum;

    /// <summary>
    /// Gets the increment step value for the numeric option. This property is initialized to 1 by default, but can be set to any desired increment value as needed. The increment value determines the step size for increasing or decreasing the numeric input, allowing users to adjust the value in defined increments when using controls such as sliders or spin boxes.
    /// </summary>
    public T Increment { get; init; } = increment;
}

/// <summary>
/// Represents a view model for a slider-based numeric option, providing logic for managing its value and display within
/// a user interface.
/// </summary>
/// <remarks>This class is sealed and cannot be inherited. It is intended for use with numeric values that are
/// manipulated via a slider control in the UI.</remarks>
/// <param name="definition">The control option definition that supplies metadata and configuration for the slider option.</param>
/// <param name="displayNameFunc">A provider that returns the display name for the slider option, allowing for dynamic or context-sensitive naming.</param>
internal sealed class SliderOptionViewModel(IControlOptionDefinition definition, IProvideValue<string> displayNameFunc) : NumericOptionViewModel<decimal>(definition, displayNameFunc, 0, 100, 1);

/// <summary>
/// Represents a view model for a slider-based numeric option, providing logic for managing its value and display within
/// a user interface.
/// </summary>
/// <remarks>This class is sealed and cannot be inherited. It is intended for use with numeric values that are
/// manipulated via a slider control in the UI.</remarks>
/// <param name="definition">The control option definition that supplies metadata and configuration for the slider option.</param>
/// <param name="displayNameFunc">A provider that returns the display name for the slider option, allowing for dynamic or context-sensitive naming.</param>
internal sealed class IntSliderOptionViewModel(IControlOptionDefinition definition, IProvideValue<string> displayNameFunc) : NumericOptionViewModel<int>(definition, displayNameFunc, 0, 100, 1);

/// <summary>
/// Represents a view model for a NumericUpDown-based numeric option, providing logic for managing its value and display within
/// a user interface.
/// </summary>
/// <remarks>This class is sealed and cannot be inherited. It is intended for use with numeric values that are
/// manipulated via a slider control in the UI.</remarks>
/// <param name="definition">The control option definition that supplies metadata and configuration for the slider option.</param>
/// <param name="displayNameFunc">A provider that returns the display name for the slider option, allowing for dynamic or context-sensitive naming.</param>
internal sealed class NumericUpDownOptionViewModel(IControlOptionDefinition definition, IProvideValue<string> displayNameFunc) : NumericOptionViewModel<decimal>(definition, displayNameFunc, 0, 100, 1);

/// <summary>
/// Represents a view model for a NumericUpDown-based numeric option, providing logic for managing its value and display within
/// a user interface.
/// </summary>
/// <remarks>This class is sealed and cannot be inherited. It is intended for use with numeric values that are
/// manipulated via a slider control in the UI.</remarks>
/// <param name="definition">The control option definition that supplies metadata and configuration for the slider option.</param>
/// <param name="displayNameFunc">A provider that returns the display name for the slider option, allowing for dynamic or context-sensitive naming.</param>
internal sealed class IntNumericUpDownOptionViewModel(IControlOptionDefinition definition, IProvideValue<string> displayNameFunc) : NumericOptionViewModel<int>(definition, displayNameFunc, 0, 100, 1);
