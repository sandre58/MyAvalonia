// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// <copyright file="BooleanOptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// ViewModel for a toggle switch option, which represents a boolean value that can be toggled on or off. This class inherits from <see cref="ValueOptionViewModel{T}"/>, which provides the necessary functionality to manage the state and display of the toggle switch option in the UI.
/// </summary>
/// <param name="definition">The definition of the control option.</param>
/// <param name="displayNameFunc">A function that provides the display name for the option.</param>
internal abstract class BooleanOptionViewModel(IControlOptionDefinition definition, IObservableValue<string> displayNameFunc) : ValueOptionViewModel<bool>(definition, definition.DefaultValue, displayNameFunc);

/// <summary>
/// ViewModel for a toggle switch option, which represents a boolean value that can be toggled on or off. This class inherits from <see cref="ValueOptionViewModel{T}"/>, which provides the necessary functionality to manage the state and display of the toggle switch option in the UI.
/// </summary>
/// <param name="definition">The definition of the control option.</param>
/// <param name="displayNameFunc">A function that provides the display name for the option.</param>
internal sealed class ToggleSwitchOptionViewModel(IControlOptionDefinition definition, IObservableValue<string> displayNameFunc) : BooleanOptionViewModel(definition, displayNameFunc);
