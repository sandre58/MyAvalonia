// -----------------------------------------------------------------------
// <copyright file="RoleChoiceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Theme.Theming.Core;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Choices;

/// <summary>
/// View model for a theme role option, which includes the theme role and a display name for UI representation. This view model inherits from the generic OptionViewModel, allowing it to hold a ThemeRole value. The display name is provided through an <see cref="IObservableValue{T}"/> interface, enabling dynamic retrieval of the display name based on the theme role or other criteria. This design allows for flexible and reusable UI components that can represent various theme roles with appropriate display names in the theme builder application.
/// </summary>
/// <param name="role">The theme role represented by this option.</param>
/// <param name="displayNameFunc">A provider that supplies the display name for the theme role, enabling localization or dynamic naming.</param>
internal sealed class RoleChoiceViewModel(ThemeRole role, IObservableValue<string> displayNameFunc) : ChoiceViewModel<ThemeRole>(role, displayNameFunc);
