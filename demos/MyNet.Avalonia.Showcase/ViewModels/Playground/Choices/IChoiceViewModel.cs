// -----------------------------------------------------------------------
// <copyright file="IChoiceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Choices;

/// <summary>
/// Represents a non-generic view model for an option, which includes a value and a display name for UI representation. This view model is designed to hold a value of any type as an object, allowing for flexibility in scenarios where the type of the value is not known at compile time. The display name is provided through an <see cref="IProvideValue{T}"/> interface, enabling dynamic retrieval of the display name based on the value or other criteria. This design allows for flexible and reusable UI components that can represent various options with appropriate display names in the theme builder application, even when the specific type of the value is not predetermined.
/// </summary>
internal interface IChoiceViewModel
{
    /// <summary>
    /// Gets the display name to show for this option in the UI. This property is read-only and is initialized through the constructor. The display name is intended for display purposes in UI components such as forms or property grids, and it can be dynamically provided based on the value or other criteria through the use of the <see cref="IProvideValue{T}"/> interface.
    /// </summary>
    IProvideValue<string> DisplayName { get; }

    /// <summary>
    /// Gets the value of the option. This property is read-only and is initialized through the constructor. The value represents the underlying data or selection associated with this option.
    /// </summary>
    object? Value { get; }

    /// <summary>
    /// Gets the optional icon data associated with the CSS class, which can be used for visual representation in the UI. This property allows for an icon to be displayed alongside the class name, enhancing the user experience by providing a visual cue related to the CSS class. The icon data is typically defined as an instance of <see cref="MaterialIconKind"/>, which encapsulates information about the icon to be displayed, such as its source, size, and other relevant properties.
    /// </summary>
    object? Icon { get; }
}
