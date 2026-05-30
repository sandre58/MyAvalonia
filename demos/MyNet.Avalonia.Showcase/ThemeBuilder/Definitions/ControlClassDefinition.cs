// -----------------------------------------------------------------------
// <copyright file="ControlClassDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using MyNet.Avalonia.Theme.Classes;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents a control option definition for selecting CSS classes to apply to a control. This definition allows for specifying a default CSS class or a collection of CSS classes that can be applied to the control, enabling dynamic styling based on user selection. The implementation of this definition provides a mechanism for converting the selected value(s) into their string representations, which can then be used to apply the appropriate CSS classes to the control in the user interface.
/// </summary>
/// <param name="DefaultValue">The default CSS class to be selected when the option is not explicitly set.</param>
/// <param name="OnValueChanged">The delegate that defines the action to be executed when the value of the control changes, accepting both the control and an optional value as parameters.</param>
internal sealed record ControlClassDefinition(CssClass? DefaultValue = null, Action<Control, object?>? OnValueChanged = null) : IControlOptionDefinition, IProvideClasses
{
    /// <summary>
    /// Gets the default value for the control option, which can be used to initialize the option to a standard state.
    /// </summary>
    /// <remarks>The default value may vary depending on the specific implementation of the control option. It
    /// is important to check this value before setting a custom option to ensure compatibility.</remarks>
    object? IControlOptionDefinition.DefaultValue => DefaultValue;

    /// <summary>
    /// Converts the specified object to its string representation and returns it as a single-element array.
    /// </summary>
    /// <param name="value">The object to convert. If the object is null, an empty array is returned.</param>
    /// <returns>An array containing the string representation of the provided object, or an empty array if the object is null.</returns>
    public string[] ProvideClasses(object? value) => value switch
    {
        null => [],
        CssClass cssClass => [cssClass.ToString()],
        IEnumerable<CssClass> cssClasses => [.. cssClasses.Select(c => c.ToString())],
        IEnumerable<string> cssClassesStr => [.. cssClassesStr],
        _ => [value.ToString() ?? string.Empty]
    };
}
