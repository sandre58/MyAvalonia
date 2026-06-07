// -----------------------------------------------------------------------
// <copyright file="ControlStyle.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Theming;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;

/// <summary>
/// Represents the configuration settings for a control, including its theme, CSS classes, and property values.
/// </summary>
/// <remarks>Use this class to define the visual and behavioral aspects of a control in a user interface. The
/// configuration can be used to apply a specific theme, assign additional CSS classes for styling, and specify property
/// values that affect the control's appearance and functionality.</remarks>
internal sealed class ControlStyle
{
    /// <summary>
    /// Gets the theme applied to the control, which defines its visual appearance.
    /// </summary>
    /// <remarks>The theme can be set to customize the control's look and feel. If not specified, the control
    /// will use a default theme.</remarks>
    public ControlTheme? Theme { get; init; }

    /// <summary>
    /// Gets the key that identifies the current theme.
    /// </summary>
    /// <remarks>The theme key is used to apply specific styling and layout configurations based on the
    /// selected theme. It is important to ensure that the key corresponds to a valid theme definition.</remarks>
    public string? ThemeKey { get; init; }

    /// <summary>
    /// Gets the collection of class names associated with the current instance.
    /// </summary>
    /// <remarks>This property provides a read-only list of class names that can be used for categorization,
    /// styling, or grouping purposes. The collection is initialized to an empty list if no class names are
    /// specified.</remarks>
    public IReadOnlyList<string> Classes { get; init; } = [];

    /// <summary>
    /// Gets the collection of control property values, which represent the specific settings for the control's properties.
    /// </summary>
    public IReadOnlyList<StyleProperty> Properties { get; init; } = [];

    /// <summary>
    /// Gets the collection of control action values, which represent the interactive actions associated with the control, such as commands or event handlers. Each ControlActionValue contains a definition of the action and a Subject that can be used to trigger the action when it is invoked. This allows for dynamic interaction with the control based on user input or other events in the application.
    /// </summary>
    public IReadOnlyList<StyleAction> Actions { get; init; } = [];

    public override string ToString()
    {
        var themeInfo = Theme != null ? $"Theme (Key: {ThemeKey})" : "No theme";
        var classesInfo = Classes.Count > 0 ? $"Classes: {string.Join(", ", Classes)}" : "No classes";
        var propertiesInfo = Properties.Count > 0 ? $"Properties: {string.Join(", ", Properties)}" : "No properties";
        var actionsInfo = Actions.Count > 0 ? $"Actions: {Actions.Count} defined" : "No actions";
        return $"{themeInfo}; {classesInfo}; {propertiesInfo}; {actionsInfo}";
    }
}

/// <summary>
/// Represents a value for a control property, associating a ControlPropertyDefinition with its corresponding value.
/// </summary>
/// <param name="Property">The definition of the control property.</param>
/// <param name="Value">The value of the control property.</param>
/// <param name="XamlKey">The XAML key representation of the control property. If not provided, the default property name is used.</param>
/// <param name="XamlValue">The XAML value representation of the control property. If not provided, the default value's ToString method is used.</param>
internal sealed record StyleProperty(AvaloniaProperty Property, object? Value, string XamlKey, string XamlValue)
{
    /// <summary>
    /// Creates a new instance of the StyleProperty class using the specified AvaloniaProperty and its associated value.
    /// </summary>
    /// <remarks>This method is useful for creating style properties dynamically based on the provided
    /// AvaloniaProperty.</remarks>
    /// <param name="property">The AvaloniaProperty that defines the style property to be created.</param>
    /// <param name="value">The value associated with the specified property, which can be null.</param>
    /// <returns>A new StyleProperty instance initialized with the provided property and value.</returns>
    public static StyleProperty FromProperty(AvaloniaProperty property, object? value) => new(property, value, property.Name, value?.ToString());

    /// <summary>
    /// Creates a new StyleProperty instance that represents the specified attached property and its associated value.
    /// </summary>
    /// <remarks>The returned StyleProperty uses a resource key based on the owner type and property name,
    /// which can be useful for theming and resource lookups.</remarks>
    /// <typeparam name="T">The type of the value held by the attached property.</typeparam>
    /// <param name="property">The attached property to associate with the style property. Cannot be null.</param>
    /// <param name="value">The value to assign to the attached property. This value can be null.</param>
    /// <returns>A StyleProperty instance that encapsulates the attached property and its value.</returns>
    public static StyleProperty FromAttachedProperty<T>(AttachedProperty<T> property, object? value) => new(property, value, $"{ThemeResourceKeyFactory.XamlPrefix}:{property.OwnerType.Name}.{property.Name}", value?.ToString());
}

/// <summary>
/// Represents a control action value that associates a control action definition with a subject that holds a boolean
/// state.
/// </summary>
/// <param name="Action">The action to be executed when the control action is triggered.</param>
/// <param name="Subject">The subject that holds a boolean value, representing the current state or condition related to the control action.</param>
internal sealed record StyleAction(Action<Control, object?> Action, Subject<object?> Subject, object? CurrentValue = null);
