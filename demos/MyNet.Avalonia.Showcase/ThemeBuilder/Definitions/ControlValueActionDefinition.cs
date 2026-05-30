// -----------------------------------------------------------------------
// <copyright file="ControlValueActionDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of a control value action, which includes an action delegate that can be executed on a control with an optional parameter. This record implements the <see cref="IControlOptionDefinition"/> interface, providing a concrete implementation for defining actions that can be performed on controls in a theming system, where the action may require an additional value to be passed when executed. The <see cref="Action"/> parameter represents the delegate that defines the action to be executed on the control, accepting both the control and an optional value as parameters. The record also provides implementations for the members of the interface it implements, such as determining the target type and providing a default value for the action option.
/// </summary>
/// <param name="OnValueChanged">The delegate that defines the action to be executed on the control, accepting both the control and an optional value as parameters.</param>
/// <param name="DefaultValue">The default value for the control option, which may be used when the action is executed without a specific value.</param>
internal sealed record ControlValueActionDefinition(Action<Control, object?> OnValueChanged, object? DefaultValue = null) : IControlOptionDefinition;
