// -----------------------------------------------------------------------
// <copyright file="ChoiceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Choices;

/// <summary>
/// View model for an option, which includes a value and a display name for UI representation. This view model is generic, allowing it to hold a value of any type specified by the type parameter <typeparamref name="T"/>. The display name is provided through an <see cref="IObservableValue{T}"/> interface, enabling dynamic retrieval of the display name based on the value or other criteria. This design allows for flexible and reusable UI components that can represent various options with appropriate display names in the theme builder application.
/// </summary>
/// <typeparam name="T">The type of the value represented by this option.</typeparam>
/// <param name="value">The value of the option.</param>
/// <param name="displayNameFunc">A provider that supplies the display name for the option, enabling localization or dynamic naming.</param>
/// <param name="icon">Optional icon data associated with the CSS class, used for UI representation.</param>
internal class ChoiceViewModel<T>(T? value, IObservableValue<string> displayNameFunc, object? icon = null) : ObservableObject, IChoiceViewModel
{
    /// <summary>
    /// Gets the display name to show for this option in the UI. This property is read-only and is initialized through the constructor. The display name is intended for display purposes in UI components such as forms or property grids, and it can be dynamically provided based on the value or other criteria through the use of the <see cref="IObservableValue{T}"/> interface.
    /// </summary>
    public IObservableValue<string> DisplayName { get; } = displayNameFunc;

    /// <summary>
    /// Gets the value of the option. This property is read-only and is initialized through the constructor. The value represents the underlying data or selection associated with this option, and it can be of any type specified by the generic type parameter <typeparamref name="T"/>.
    /// </summary>
    public virtual T? Value { get; } = value;

    // <inheritdoc/>
    object? IChoiceViewModel.Value => Value;

    /// <summary>
    /// Gets the optional icon data associated with the CSS class, which can be used for visual representation in the UI. This property allows for an icon to be displayed alongside the class name, enhancing the user experience by providing a visual cue related to the CSS class. The icon data is typically defined as an instance of <see cref="MaterialIconKind"/>, which encapsulates information about the icon to be displayed, such as its source, size, and other relevant properties.
    /// </summary>
    public object? Icon { get; } = icon;
}
