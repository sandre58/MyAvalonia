// -----------------------------------------------------------------------
// <copyright file="TimeInputCompletedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Interactivity;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Describes how a <see cref="Primitives.TimeSelectorBase"/> input session completed.
/// </summary>
public enum TimeInputCompletionMode
{
    /// <summary>User pressed Enter on the last selectable component.</summary>
    EnterKey,

    /// <summary>Field advanced automatically (digit entry or clock drag) with no next component.</summary>
    FieldAdvance,
}

/// <summary>
/// Event data for <see cref="Primitives.TimeSelectorBase.InputCompleted"/>.
/// </summary>
public sealed class TimeInputCompletedEventArgs : RoutedEventArgs
{
    public TimeInputCompletedEventArgs(RoutedEvent routedEvent)
        : base(routedEvent)
    {
    }

    /// <summary>
    /// Gets how the input session completed.
    /// </summary>
    public required TimeInputCompletionMode Mode { get; init; }
}
