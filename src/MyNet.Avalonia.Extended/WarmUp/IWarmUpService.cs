// -----------------------------------------------------------------------
// <copyright file="IWarmUpService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyNet.Avalonia.Extended.WarmUp;

/// <summary>
/// Defines a service responsible for warming up objects.
/// </summary>
public interface IWarmUpService
{
    /// <summary>
    /// Raised when a warm-up request is initiated, providing details about the requested object types and any specified delay.
    /// </summary>
    event EventHandler<WarmUpRequestedEventArgs> WarmUpRequested;

    /// <summary>
    /// Raised to report progress during the warm-up process.
    /// </summary>
    event EventHandler<WarmUpProgressEventArgs> WarmUpProgress;

    /// <summary>
    /// Raised when the warm-up process is completed.
    /// </summary>
    event EventHandler<WarmUpCompletedEventArgs> WarmUpCompleted;

    /// <summary>
    /// Warms up the specified object types, optionally with a delay in milliseconds.
    /// </summary>
    /// <param name="objectTypes">The types of objects to warm up.</param>
    /// <param name="delayMs">The delay in milliseconds before warming up the objects.</param>
    Task WarmUpAsync(IEnumerable<Type> objectTypes, int delayMs = 0);
}
