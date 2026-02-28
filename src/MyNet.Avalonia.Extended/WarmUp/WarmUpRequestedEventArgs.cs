// -----------------------------------------------------------------------
// <copyright file="WarmUpRequestedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MyNet.Avalonia.Extended.WarmUp;

/// <summary>
/// Provides data for the WarmUpRequested event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WarmUpRequestedEventArgs"/> class.
/// </remarks>
/// <param name="objectTypes">The types of objects to warm up.</param>
/// <param name="delayMs">The delay in milliseconds before warming up the objects.</param>
public class WarmUpRequestedEventArgs(IEnumerable<Type> objectTypes, int delayMs = 0) : EventArgs
{
    /// <summary>
    /// Gets the types of objects to warm up.
    /// </summary>
    public IEnumerable<Type> ObjectTypes { get; } = objectTypes ?? throw new ArgumentNullException(nameof(objectTypes));

    /// <summary>
    /// Gets the delay in milliseconds before warming up the objects.
    /// </summary>
    public int DelayMs { get; } = delayMs;

    /// <summary>
    /// Gets the timestamp when the warm-up was requested.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
