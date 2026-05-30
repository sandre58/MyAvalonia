// -----------------------------------------------------------------------
// <copyright file="ControlPropertyDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of a control property, which includes the associated Avalonia property and an optional default value. This record implements the <see cref="IControlPropertyDefinition"/> interface, providing a concrete implementation for defining properties of controls in a theming system. The <see cref="Property"/> parameter represents the Avalonia property that this definition is associated with, while the <see cref="DefaultValue"/> parameter allows for specifying a default value for this property when it is not explicitly set. The record also provides implementations for the members of the interfaces it implements, such as determining the target type and converting values to their XAML representation.
/// </summary>
/// <typeparam name="T">The type of the Avalonia property.</typeparam>
/// <param name="Property">The Avalonia property associated with this definition.</param>
/// <param name="DefaultValue">The default value for this property.</param>
/// <param name="OnValueChanged">The delegate that defines the action to be executed when the value of the control changes, accepting both the control and an optional value as parameters.</param>
internal record ControlPropertyDefinition<T>(AvaloniaProperty<T> Property, T? DefaultValue = default, Action<Control, object?>? OnValueChanged = null) : IControlPropertyDefinition
{
    /// <summary>
    /// Gets the type of the property associated with this instance.
    /// </summary>
    /// <remarks>This property retrieves the type information of the property defined in the associated
    /// Property object. It is useful for reflection and type checking.</remarks>
    public Type TargetType => Property.PropertyType;

    /// <summary>
    /// Gets the Avalonia property that is associated with this control property definition.
    /// </summary>
    AvaloniaProperty IControlPropertyDefinition.Property => Property;

    /// <summary>
    /// Gets the default value for the control option, which may be used to initialize the option's state.
    /// </summary>
    /// <remarks>The default value is typically used when the option is not explicitly set by the user. It can
    /// be null if no default value is defined.</remarks>
    object? IControlOptionDefinition.DefaultValue => DefaultValue;

    /// <summary>
    /// Gets the XAML resource key associated with this property, derived from the property's name.
    /// </summary>
    protected virtual string GetXamlKey() => Property.Name;

    /// <summary>
    /// Converts the specified object to its XAML string representation.
    /// </summary>
    /// <param name="value">The object to convert to a XAML string. If the object is null, an empty string is returned.</param>
    /// <returns>A string representing the XAML value of the specified object. Returns an empty string if the object is null.</returns>
    protected virtual string GetXamlValue(object? value) => value is IEnumerable enumerable ? string.Join(", ", enumerable.Cast<object>()) : value?.ToString() ?? string.Empty;

    /// <summary>
    /// Provides a <see cref="StyleProperty"/> instance based on the current property definition and the specified value. This method constructs a new <see cref="StyleProperty"/> using the associated Avalonia property, the provided value, and the XAML key and value derived from the property and value. The resulting <see cref="StyleProperty"/> can be used in styling scenarios to apply the defined property with the specified value to controls in a theming system.
    /// </summary>
    /// <param name="value">The value to be applied to the style property.</param>
    /// <returns>A <see cref="StyleProperty"/> instance representing the styled property with the specified value.</returns>
    public StyleProperty? ProvideStyleProperty(object? value)
        => !Equals(value, DefaultValue) ? new(Property,
                                              ProvideValue(value),
                                              GetXamlKey(),
                                              GetXamlValue(value))
        : null;

    /// <summary>
    /// Provides a value that is suitable for assignment to the associated Avalonia property, based on the specified input value. This method handles the conversion of the input value to the appropriate type expected by the property, including special handling for enum types and collections of enum values. If the input value is null, it returns the default value for the type. If the property is an enum and the input value is a collection, it combines the enum values using bitwise operations to produce a single enum value. For other types, it attempts to convert the input value to the target type using culture-specific formatting. This method ensures that the provided value is compatible with the property's expected type before it is applied in styling or control configuration scenarios.
    /// </summary>
    /// <param name="value">The input value to be converted.</param>
    /// <returns>The converted value suitable for the associated Avalonia property.</returns>
    protected virtual object? ProvideValue(object? value)
    {
        if (value is null)
            return default(T);

        if (typeof(T).IsEnum && value is IEnumerable enumerable)
        {
            long result = 0;

            foreach (var item in enumerable)
            {
                if (item is T enumValue)
                {
                    result |= Convert.ToInt64(enumValue, CultureInfo.CurrentCulture);
                }
            }

            return (T)Enum.ToObject(typeof(T), result);
        }

        return value is IConvertible ? Convert.ChangeType(value, typeof(T), CultureInfo.CurrentCulture) : value;
    }
}
