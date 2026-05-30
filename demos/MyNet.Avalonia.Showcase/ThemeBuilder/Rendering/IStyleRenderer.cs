// -----------------------------------------------------------------------
// <copyright file="IStyleRenderer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;

/// <summary>
/// Defines a contract for applying styling configurations to a control at runtime.
/// </summary>
/// <remarks>Implementations of this interface should ensure that the provided configuration is valid and
/// appropriately modifies the control's appearance or behavior. This interface is intended for use in scenarios where
/// dynamic styling of controls is required.</remarks>
internal interface IStyleRenderer
{
    /// <summary>
    /// Applies the specified configuration to the control.
    /// </summary>
    /// <param name="control">The control to which the configuration will be applied.</param>
    /// <param name="configuration">The configuration settings to apply to the control.</param>
    void Apply(Control control, ControlStyle configuration);
}
