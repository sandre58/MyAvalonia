// -----------------------------------------------------------------------
// <copyright file="IStyleProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;

/// <summary>
/// Provides an interface for generating and monitoring the styling configuration of a control based on the current
/// theme and related factors.
/// </summary>
/// <remarks>Implementations of this interface enable dynamic updates to a control's appearance in response to
/// theme changes or other configuration triggers. Subscribers can listen for configuration changes to update UI
/// elements accordingly.</remarks>
internal interface IStyleProvider
{
    /// <summary>
    /// Builds the control configuration based on the current theme and other factors.
    /// </summary>
    /// <returns>The control configuration representing the current state of the control's styling.</returns>
    ControlStyle BuildStyle();

    /// <summary>
    /// Occurs when the configuration of the control changes.
    /// </summary>
    /// <remarks>This event is triggered whenever the control's configuration is updated, allowing subscribers
    /// to respond to changes in settings or parameters.</remarks>
    event EventHandler<ControlStyle>? StyleChanged;
}
