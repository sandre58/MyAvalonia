// -----------------------------------------------------------------------
// <copyright file="IControlPropertyDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of a control property, which includes the associated Avalonia property and the ability to convert its value to a XAML representation. This interface extends both <see cref="IControlOptionDefinition"/> and <see cref="IIsXamlProperty"/>, indicating that it defines a setting for a control and can be serialized into XAML format. The <see cref="Property"/> property provides access to the underlying Avalonia property, allowing for integration with Avalonia's styling and theming system.
/// </summary>
internal interface IControlPropertyDefinition : IControlOptionDefinition, IProvideStyleProperty
{
    /// <summary>
    /// Gets the type of value that this option expects.
    /// </summary>
    Type TargetType { get; }

    /// <summary>
    /// Gets the Avalonia property associated with this member.
    /// </summary>
    AvaloniaProperty Property { get; }
}
