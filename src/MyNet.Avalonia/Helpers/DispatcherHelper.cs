// -----------------------------------------------------------------------
// <copyright file="DispatcherHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace MyNet.Avalonia.Helpers;

/// <summary>
/// Provides helper methods for safely executing actions and functions on the Avalonia UI thread.
/// Ensures thread-safe access to UI components and simplifies dispatcher usage.
/// </summary>
public static class DispatcherHelper
{
    /// <summary>
    /// Executes the specified action on the UI thread. If already on the UI thread, executes immediately; otherwise, posts to the dispatcher.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="priority">Optional dispatcher priority.</param>
    public static void Post(Action action, DispatcherPriority? priority = null)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            action();
        }
        else
        {
            Dispatcher.UIThread.Post(action, priority ?? DispatcherPriority.Normal);
        }
    }

    /// <summary>
    /// Executes the specified action asynchronously on the UI thread. If already on the UI thread, executes immediately; otherwise, invokes asynchronously via the dispatcher.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="priority">Optional dispatcher priority.</param>
    /// <returns>A task that completes when the action has executed.</returns>
    public static Task InvokeAsync(Action action, DispatcherPriority? priority = null)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            action();
            return Task.CompletedTask;
        }
        else
        {
            var tcs = new TaskCompletionSource<object?>();
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            },
            priority ?? DispatcherPriority.Normal);
            return tcs.Task;
        }
    }

    /// <summary>
    /// Executes the specified function asynchronously on the UI thread and returns its result. If already on the UI thread, executes immediately; otherwise, invokes asynchronously via the dispatcher.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="priority">Optional dispatcher priority.</param>
    /// <returns>A task containing the result of the function.</returns>
    public static Task<T> InvokeAsync<T>(Func<T> func, DispatcherPriority? priority = null)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return Task.FromResult(func());
        }
        else
        {
            var tcs = new TaskCompletionSource<T>();
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            },
            priority ?? DispatcherPriority.Normal);
            return tcs.Task;
        }
    }

    /// <summary>
    /// Executes the specified function synchronously on the UI thread and returns its result. If already on the UI thread, executes immediately; otherwise, blocks until the result is available.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="priority">Optional dispatcher priority.</param>
    /// <returns>The result of the function.</returns>
    public static T Invoke<T>(Func<T> func, DispatcherPriority? priority = null) => InvokeAsync(func, priority).GetAwaiter().GetResult();
}
