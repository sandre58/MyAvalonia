// -----------------------------------------------------------------------
// <copyright file="DateTimeScrollPickerSelectedValueChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Provides data for the <see cref="DateTimeScrollPickerEx.SelectedDateTimeChanged"/> event.
/// </summary>
public sealed class DateTimeScrollPickerSelectedValueChangedEventArgs(DateTime? oldValue, DateTime? newValue) : EventArgs
{
    /// <summary>
    /// Gets the previous selected value.
    /// </summary>
    public DateTime? OldValue { get; } = oldValue;

    /// <summary>
    /// Gets the new selected value.
    /// </summary>
    public DateTime? NewValue { get; } = newValue;
}
