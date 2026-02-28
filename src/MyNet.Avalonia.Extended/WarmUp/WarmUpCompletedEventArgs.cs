// -----------------------------------------------------------------------
// <copyright file="WarmUpCompletedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Extended.WarmUp;

/// <summary>
/// Provides data for the WarmUpCompleted event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WarmUpCompletedEventArgs"/> class.
/// </remarks>
/// <param name="totalCount">The total number of types warmed up.</param>
/// <param name="successCount">The number of types successfully warmed up.</param>
/// <param name="failureCount">The number of types that failed to warm up.</param>
/// <param name="elapsedMilliseconds">The total time elapsed during the warm-up process in milliseconds.</param>
public class WarmUpCompletedEventArgs(int totalCount, int successCount, int failureCount, long elapsedMilliseconds) : EventArgs
{
    /// <summary>
    /// Gets the total number of types that were processed.
    /// </summary>
    public int TotalCount { get; } = totalCount;

    /// <summary>
    /// Gets the number of types successfully warmed up.
    /// </summary>
    public int SuccessCount { get; } = successCount;

    /// <summary>
    /// Gets the number of types that failed to warm up.
    /// </summary>
    public int FailureCount { get; } = failureCount;

    /// <summary>
    /// Gets the total time elapsed during the warm-up process in milliseconds.
    /// </summary>
    public long ElapsedMilliseconds { get; } = elapsedMilliseconds;

    /// <summary>
    /// Gets the timestamp when the warm-up was completed.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets a value indicating whether the warm-up completed successfully without any failures.
    /// </summary>
    public bool IsSuccessful => FailureCount == 0;
}
