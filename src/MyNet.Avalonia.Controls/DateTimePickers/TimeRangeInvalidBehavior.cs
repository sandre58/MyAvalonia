// -----------------------------------------------------------------------
// <copyright file="TimeRangeInvalidBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Defines how <see cref="TimeRangePickerEx"/> handles a start time after the end time when overnight ranges are disabled.
/// </summary>
public enum TimeRangeInvalidBehavior
{
    /// <summary>
    /// Swaps start and end times (aligned with <see cref="DateRangePickerEx"/>).
    /// </summary>
    Swap,

    /// <summary>
    /// Reports a data validation error and keeps the last committed value.
    /// </summary>
    ReportError
}
