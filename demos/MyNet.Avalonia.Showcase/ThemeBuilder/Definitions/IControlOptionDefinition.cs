// -----------------------------------------------------------------------
// <copyright file="IControlOptionDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of a control option, which includes the target type and an optional default value. This interface is used to define settings or options for controls in a theming system, allowing for the specification of the type of value expected and providing a default value that can be used when no specific value is provided. The <see cref="TargetType"/> property indicates the type of value that this option expects, while the <see cref="DefaultValue"/> property provides a default value that can be used if no specific value is set for this option.
/// </summary>
internal interface IControlOptionDefinition
{
    /// <summary>
    /// Gets the default value for this option.
    /// </summary>
    object? DefaultValue { get; }

    /// <summary>
    /// Gets the action to invoke when the control's value changes, providing the control and the new value as
    /// parameters.
    /// </summary>
    /// <remarks>Subscribers can assign this action to handle value change events for the associated control.
    /// Ensure that the action is assigned appropriately to respond to value changes as needed.</remarks>
    Action<Control, object?>? OnValueChanged { get; }
}
