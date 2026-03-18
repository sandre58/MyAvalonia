// -----------------------------------------------------------------------
// <copyright file="ControlVariant.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Theme.Classes.Enums;

/// <summary>
/// Specifies the visual style variants that can be applied to a control. This enumeration supports combining multiple
/// styles using bitwise operations.
/// </summary>
/// <remarks>Each value represents a distinct appearance option, such as solid, light, outlined, or text. The
/// enumeration is marked with the <see cref="System.FlagsAttribute"/>, allowing multiple styles to be combined to
/// achieve composite visual effects.</remarks>
[Flags]
public enum ControlVariant
{
    /// <summary>
    /// Represents a value indicating that no specific option is selected.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents a value indicating that no specific option is selected.
    /// </summary>
    Default = 1 << 0,

    /// <summary>
    /// Represents a transparent shape type in the bitwise shape enumeration.
    /// </summary>
    Transparent = 1 << 1,

    /// <summary>
    /// Represents a solid shape type in the bitwise shape enumeration.
    /// </summary>
    /// <remarks>This value can be combined with other shape types using bitwise operations to specify
    /// multiple shape characteristics.</remarks>
    Solid = 1 << 2,

    /// <summary>
    /// Represents the light setting, which can be combined with other values to configure the appearance or behavior of
    /// a control.
    /// </summary>
    /// <remarks>This value is typically used as a flag in bitwise operations to enable or check for the light
    /// variant in a set of control variants.</remarks>
    Light = 1 << 3,

    /// <summary>
    /// Represents a visual element that is displayed in an outlined style.
    /// </summary>
    Outlined = 1 << 4,

    /// <summary>
    /// Specifies the text option in the bitwise enumeration.
    /// </summary>
    /// <remarks>This value can be combined with other enumeration values using bitwise operations to enable
    /// multiple options simultaneously.</remarks>
    Text = 1 << 5
}
