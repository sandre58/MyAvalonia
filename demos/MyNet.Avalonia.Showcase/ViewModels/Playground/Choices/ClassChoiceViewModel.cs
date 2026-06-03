// -----------------------------------------------------------------------
// <copyright file="ClassChoiceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Theme.Classes;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Choices;

/// <summary>
/// View model for a CSS class option, which includes the class name and a display name for UI representation. This view model inherits from the generic OptionViewModel, allowing it to hold a string value representing the CSS class. The display name is provided through an <see cref="IObservableValue{T}"/> interface, enabling dynamic retrieval of the display name based on the class name or other criteria. This design allows for flexible and reusable UI components that can represent various CSS classes with appropriate display names in the theme builder application.
/// </summary>
/// <param name="class">The CSS class name represented by this option.</param>
/// <param name="displayName">A provider that supplies the display name for the CSS class, enabling localization or dynamic naming.</param>
/// <param name="icon">Optional icon data associated with the CSS class, used for UI representation.</param>
internal sealed class ClassChoiceViewModel(CssClass @class, IObservableValue<string> displayName, object? icon = null) : ChoiceViewModel<CssClass>(@class, displayName, icon);
