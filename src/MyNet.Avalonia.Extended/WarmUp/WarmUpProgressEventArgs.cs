// -----------------------------------------------------------------------
// <copyright file="WarmUpProgressEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Extended.WarmUp;

/// <summary>
/// Provides data for the WarmUpProgress event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WarmUpProgressEventArgs"/> class.
/// </remarks>
/// <param name="currentType">The type currently being warmed up.</param>
/// <param name="currentObject">The object currently being warmed up.</param>
/// <param name="completedCount">The number of types completed.</param>
/// <param name="totalCount">The total number of types to warm up.</param>
/// <param name="percentProgress">The percentage of progress (0-100).</param>
public class WarmUpProgressEventArgs(Type currentType, object? currentObject, int completedCount, int totalCount, double percentProgress) : EventArgs
{
    /// <summary>
    /// Gets the type currently being warmed up.
    /// </summary>
    public Type CurrentType { get; } = currentType ?? throw new ArgumentNullException(nameof(currentType));

    /// <summary>
    /// Gets the object currently being warmed up.
    /// </summary>
    public object? CurrentObject { get; } = currentObject;

    /// <summary>
    /// Gets the number of types that have been completed.
    /// </summary>
    public int CompletedCount { get; } = completedCount;

    /// <summary>
    /// Gets the total number of types to warm up.
    /// </summary>
    public int TotalCount { get; } = totalCount;

    /// <summary>
    /// Gets the percentage of progress (0-100).
    /// </summary>
    public double PercentProgress { get; } = Math.Clamp(percentProgress, 0, 100);

    /// <summary>
    /// Gets the timestamp when the progress update occurred.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
