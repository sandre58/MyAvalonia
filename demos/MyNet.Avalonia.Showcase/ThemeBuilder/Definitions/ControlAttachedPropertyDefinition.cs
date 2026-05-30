// -----------------------------------------------------------------------
// <copyright file="ControlAttachedPropertyDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Theme.Theming;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of a control attached property, which includes the associated Avalonia attached property and an optional default value. This record extends the <see cref="ControlPropertyDefinition{T}"/> record, providing a concrete implementation for defining attached properties of controls in a theming system. The <see cref="Property"/> parameter represents the Avalonia attached property that this definition is associated with, while the <see cref="DefaultValue"/> parameter allows for specifying a default value for this property when it is not explicitly set. The record also overrides the XAML key generation to include a prefix, which can be used to differentiate attached properties from regular properties in XAML resource dictionaries.
/// </summary>
/// <typeparam name="T">The type of the Avalonia property.</typeparam>
/// <param name="Property">The Avalonia attached property associated with this definition.</param>
/// <param name="Prefix">The prefix used in the XAML key to differentiate attached properties from regular properties.</param>
/// <param name="DefaultValue">The default value for this attached property.</param>
/// <param name="OnValueChanged">The delegate that defines the action to be executed when the value of the control changes, accepting both the control and an optional value as parameters.</param>
internal sealed record ControlAttachedPropertyDefinition<T>(AvaloniaProperty<T> Property, string Prefix = ThemeResourceKeyFactory.XamlPrefix, T? DefaultValue = default, Action<Control, object?>? OnValueChanged = null)
    : ControlPropertyDefinition<T>(Property, DefaultValue, OnValueChanged)
{
    /// <summary>
    /// Gets the XAML key that uniquely identifies the attached property in XAML markup.
    /// </summary>
    /// <remarks>The XAML key is formatted as 'Prefix:OwnerType.PropertyName' and is used to reference the
    /// property in XAML for binding and resource resolution purposes.</remarks>
    protected override string GetXamlKey() => $"{Prefix}:{Property.OwnerType.Name}.{Property.Name}";
}
