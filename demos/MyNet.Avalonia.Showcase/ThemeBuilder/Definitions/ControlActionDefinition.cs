// -----------------------------------------------------------------------
// <copyright file="ControlActionDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of a control action, which includes an action delegate that can be executed on a control. This record implements the <see cref="IControlOptionDefinition"/> interface, providing a concrete implementation for defining actions that can be performed on controls in a theming system. The <see cref="Action"/> parameter represents the delegate that defines the action to be executed on the control. The record also provides implementations for the members of the interface it implements, such as determining the target type and providing a default value for the action option.
/// </summary>
/// <param name="Action">The delegate that defines the action to be executed on the control.</param>
internal sealed record ControlActionDefinition(Action<Control> Action) : IControlOptionDefinition
{
    /// <summary>
    /// Gets the default value for the control option as determined by the associated action.
    /// </summary>
    /// <remarks>The default value may vary depending on the specific implementation of the control option.
    /// Review the context in which this property is used to understand its implications.</remarks>
    object? IControlOptionDefinition.DefaultValue => null;

    /// <summary>
    /// Gets the action that is invoked when the value of the control changes.
    /// </summary>
    /// <remarks>This property allows subscribers to respond to changes in the control's value. The action
    /// receives the control instance and the new value as parameters. If no action is assigned, no callback is invoked
    /// when the value changes.</remarks>
    Action<Control, object?>? IControlOptionDefinition.OnValueChanged => null;
}
