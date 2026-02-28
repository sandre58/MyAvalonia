// -----------------------------------------------------------------------
// <copyright file="ViewModelWarmUpService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using MyNet.Avalonia.Helpers;
using MyNet.UI.Locators;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia.Extended.WarmUp;

/// <summary>
/// Implements a service responsible for warming up view models asynchronously.
/// This service loads view models in batches to optimize performance and responsiveness.
/// </summary>
/// <remarks>
/// The service supports event notifications at different stages of the warm-up process:
/// - WarmUpRequested: Raised when the warm-up process is initiated
/// - WarmUpProgress: Raised as each view model is loaded
/// - WarmUpCompleted: Raised when all view models have been loaded.
/// </remarks>
/// <param name="viewModelLocator">The view model service locator used to retrieve view model instances.</param>
public class ViewModelWarmUpService(IViewModelLocator viewModelLocator) : IWarmUpService
{
    /// <summary>
    /// Occurs when a warm-up request is initiated.
    /// </summary>
    public event EventHandler<WarmUpRequestedEventArgs>? WarmUpRequested;

    /// <summary>
    /// Occurs when progress is made during the warm-up process.
    /// </summary>
    public event EventHandler<WarmUpProgressEventArgs>? WarmUpProgress;

    /// <summary>
    /// Occurs when the warm-up process is completed.
    /// </summary>
    public event EventHandler<WarmUpCompletedEventArgs>? WarmUpCompleted;

    /// <summary>
    /// Asynchronously warms up view models with optional delay before starting.
    /// The process loads view models in batches of 3 to optimize performance while maintaining UI responsiveness.
    /// </summary>
    /// <param name="objectTypes">The types of view model to warm up.</param>
    /// <param name="delayMs">The delay in milliseconds before starting the warm-up process. Default is 0.</param>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Raises the WarmUpRequested event
    /// 2. Applies the optional delay if specified
    /// 3. Processes view models in batches of 3 in parallel
    /// 4. Yields to the UI thread between batches to maintain responsiveness
    /// 5. Raises the WarmUpProgress event for each completed view model
    /// 6. Raises the WarmUpCompleted event upon completion.
    /// </remarks>
    public async Task WarmUpAsync(IEnumerable<Type> objectTypes, int delayMs = 0)
    {
        var types = objectTypes.ToList();
        var stopwatch = Stopwatch.StartNew();

        // Raise the WarmUpRequested event
        var args = new WarmUpRequestedEventArgs(types, delayMs);
        WarmUpRequested?.Invoke(this, args);

        if (delayMs > 0)
        {
            await Task.Delay(delayMs).ConfigureAwait(false);
        }

        var successCount = 0;
        var failureCount = 0;

        using (PerformanceMonitor.Measure($"[ViewModelWarmUpService] Starting warm-up for {types.Count} pages", category: PerformanceCategory.Pages))
        {
            // Process pages in batches of 3, loading each batch in parallel
            const int batchSize = 3;
            for (var i = 0; i < types.Count; i += batchSize)
            {
                var batch = types.GetRange(i, Math.Min(batchSize, types.Count - i));

                // Load all ViewModels in the batch in parallel
                await Task.WhenAll(batch.Select(async type =>
                {
                    var (isSuccess, createdObject) = await WarmUpAsync(type).ConfigureAwait(false);
                    if (isSuccess)
                    {
                        successCount++;
                    }
                    else
                    {
                        failureCount++;
                    }

                    // Raise the WarmUpProgress event
                    var completedCount = successCount + failureCount;
                    var progressArgs = new WarmUpProgressEventArgs(
                        type,
                        createdObject,
                        completedCount,
                        types.Count,
                        (completedCount / (double)types.Count) * 100);
                    WarmUpProgress?.Invoke(this, progressArgs);
                })).ConfigureAwait(false);

                // Yield to UI thread between batches
                if (i + batchSize < types.Count)
                {
                    await Task.Delay(1).ConfigureAwait(false);
                }
            }
        }

        stopwatch.Stop();

        // Raise the WarmUpCompleted event
        var completedArgs = new WarmUpCompletedEventArgs(types.Count, successCount, failureCount, stopwatch.ElapsedMilliseconds);
        WarmUpCompleted?.Invoke(this, completedArgs);

        PerformanceMonitor.Debug($"[ViewModelWarmUpService] Warm-up completed for {types.Count} pages (Success: {successCount}, Failed: {failureCount})", PerformanceCategory.Pages);
    }

    /// <summary>
    /// Asynchronously pre-loads a single view model and its DataTemplate.
    /// Creates the ViewModel on the UI thread to avoid thread-dependent resource issues.
    /// </summary>
    /// <param name="objectType">The type of view model to warm up.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a tuple containing:
    /// - bool: A boolean indicating whether the warm-up was successful
    /// - object: The created instance of the view model, or null if creation failed.
    /// </returns>
    /// <remarks>
    /// This method invokes the actual ViewModel creation on the UI thread using InvokeAsync
    /// to ensure proper initialization of UI-dependent resources and to maintain proper
    /// sequencing of operations. Any exceptions during the process are logged and the
    /// method returns (true, createdObject) to indicate that the operation completed
    /// (regardless of exceptions, as they are handled within the UI thread invocation).
    /// </remarks>
    private async Task<WarmUpResult> WarmUpAsync(Type objectType)
    {
        try
        {
            using (PerformanceMonitor.Measure($"[ViewModelWarmUpService] Warming up {objectType.Name}", category: PerformanceCategory.Pages))
            {
                try
                {
                    object? createdObject = null;

                    // Use InvokeAsync to wait for the UI thread execution
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        try
                        {
                            createdObject = viewModelLocator.Get(objectType);
                            PerformanceMonitor.Debug($"[ViewModelWarmUpService] Created {objectType.Name}", PerformanceCategory.Pages);
                        }
                        catch (Exception ex)
                        {
                            LogManager.Warning($"[ViewModelWarmUpService] Error creating {objectType.Name}: {ex.Message}");
                        }
                    },
                    DispatcherPriority.Background);

                    return new WarmUpResult(true, createdObject);
                }
                catch (Exception ex)
                {
                    LogManager.Warning($"[ViewModelWarmUpService] Error warming up {objectType.Name}: {ex.Message}");
                    return new WarmUpResult(false, null);
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Warning($"[ViewModelWarmUpService] Error in warm-up process for {objectType.Name}: {ex.Message}");
            return new WarmUpResult(false, null);
        }
    }
}

internal record struct WarmUpResult(bool IsSucceed, object? Object);
