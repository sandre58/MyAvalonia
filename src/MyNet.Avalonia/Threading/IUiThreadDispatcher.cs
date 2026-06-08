// -----------------------------------------------------------------------
// <copyright file="IUiThreadDispatcher.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace MyNet.Avalonia.Threading;

/// <summary>
/// Defines an interface for dispatching actions to the UI thread.
/// </summary>
public interface IUiThreadDispatcher
{
    /// <summary>
    /// Invokes the specified asynchronous action on the UI thread.
    /// </summary>
    /// <param name="action">The asynchronous action to invoke.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InvokeAsync(Func<Task> action);

    /// <summary>
    /// Posts the specified action to be executed on the UI thread without awaiting its completion.
    /// </summary>
    /// <param name="action">The action to post.</param>
    void Post(Action action);
}
