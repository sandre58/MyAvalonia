// -----------------------------------------------------------------------
// <copyright file="ControlClassToggleDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Avalonia.Theme.Classes;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of a control class toggle option, which includes a CSS class and an optional default state. This record implements the <see cref="IControlOptionDefinition"/> interface, providing a concrete implementation for defining options that allow users to toggle a specific CSS class in a theming system. The <see cref="Class"/> parameter represents the CSS class associated with the toggle option, while the <see cref="DefaultValue"/> parameter allows for specifying the default state of the toggle option.
/// </summary>
/// <param name="Class">The CSS class associated with the toggle option.</param>
/// <param name="DefaultValue">The default state of the toggle option.</param>
/// <param name="OnValueChanged">The delegate that defines the action to be executed when the value of the control changes, accepting both the control and an optional value as parameters.</param>
internal sealed record ControlClassToggleDefinition(CssClass Class, bool DefaultValue = false, Action<Control, object?>? OnValueChanged = null) : IControlOptionDefinition, IProvideClasses
{
    /// <summary>
    /// Gets the default value for the control option, which can be used to initialize the option to a standard state.
    /// </summary>
    /// <remarks>The default value may vary depending on the specific implementation of the control option. It
    /// is important to check this value before setting a custom option to ensure compatibility.</remarks>
    object IControlOptionDefinition.DefaultValue => DefaultValue;

    /// <summary>
    /// Provides the CSS class name as an array if the toggle is enabled, or an empty array if it is disabled. This method checks the provided value and returns the appropriate CSS class name based on the toggle state.
    /// </summary>
    /// <param name="value">The value indicating whether the toggle is enabled or disabled.</param>
    /// <returns>An array containing the CSS class name if the toggle is enabled, or an empty array if it is disabled.</returns>
    public string[] ProvideClasses(object? value) => value is true ? [Class.ToString()] : [];
}
