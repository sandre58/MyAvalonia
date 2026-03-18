// -----------------------------------------------------------------------
// <copyright file="CornerSize.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Classes.Enums;

/// <summary>
/// Enumerates standard corner sizes for use in the theming system.
/// Each value represents a predefined corner size for consistent layout styling.
/// </summary>
public enum CornerSize
{
    /// <summary>
    /// No corner rounding.
    /// </summary>
    None,

    /// <summary>
    /// Small corner rounding.
    /// </summary>
    Xs,

    /// <summary>
    /// Default (small) corner rounding.
    /// </summary>
    Sm,

    /// <summary>
    /// Medium corner rounding.
    /// </summary>
    Md,

    /// <summary>
    /// Large corner rounding.
    /// </summary>
    Lg,

    /// <summary>
    /// Extra large corner rounding.
    /// </summary>
    Xl,

    /// <summary>
    /// Rounds a numeric value to the nearest integer, with midpoint values rounded away from zero.
    /// </summary>
    /// <remarks>This method is useful in scenarios where consistent and predictable rounding behavior is
    /// required, such as financial or statistical calculations. The rounding strategy ensures that values exactly
    /// halfway between two integers are always rounded to the integer with the greater absolute value.</remarks>
    Round
}
