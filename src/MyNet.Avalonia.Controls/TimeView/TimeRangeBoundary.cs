// -----------------------------------------------------------------------
// <copyright file="TimeRangeBoundary.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Identifies which boundary is being edited in <see cref="TimeRangeView"/>.
/// </summary>
public enum TimeRangeBoundary
{
    /// <summary>
    /// The start time boundary.
    /// </summary>
    Start,

    /// <summary>
    /// The end time boundary.
    /// </summary>
    End,
}
