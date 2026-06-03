// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// <copyright file="TextBoxOptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// ViewModel for a text box option, which represents a string value that can be edited by the user. This class inherits from <see cref="ValueOptionViewModel{T}"/>, which provides the necessary functionality to manage the state and display of the text box option in the UI.
/// </summary>
/// <param name="definition">The definition of the control option.</param>
/// <param name="displayNameFunc">A function that provides the display name for the option.</param>
internal sealed class TextBoxOptionViewModel(IControlOptionDefinition definition, IObservableValue<string> displayNameFunc) : ValueOptionViewModel<string>(definition, definition.DefaultValue, displayNameFunc);
