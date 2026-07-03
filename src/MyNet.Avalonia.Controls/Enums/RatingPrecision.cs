// -----------------------------------------------------------------------
// <copyright file="RatingPrecision.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Enums;

/// <summary>
/// Defines how rating values are quantized during user input.
/// </summary>
public enum RatingPrecision
{
    /// <summary>
    /// Values snap to whole numbers (1, 2, 3…).
    /// </summary>
    Integer,

    /// <summary>
    /// Values snap to the nearest half step (1, 1.5, 2…).
    /// </summary>
    Half,

    /// <summary>
    /// Values follow the pointer position with a fine step (0.1).
    /// </summary>
    Continuous
}
