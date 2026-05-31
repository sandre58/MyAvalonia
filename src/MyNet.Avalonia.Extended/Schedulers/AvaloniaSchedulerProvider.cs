// -----------------------------------------------------------------------
// <copyright file="AvaloniaSchedulerProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reactive.Concurrency;
using MyNet.UI.Threading;

namespace MyNet.Avalonia.Extended.Schedulers;

/// <summary>
/// Provides Avalonia UI and background schedulers for <see cref="MyNet.UI.Commands"/> and related UI services.
/// </summary>
public sealed class AvaloniaSchedulerProvider : ISchedulerProvider
{
    /// <summary>
    /// Gets the default scheduler provider that dispatches UI work on the Avalonia dispatcher.
    /// </summary>
    public static AvaloniaSchedulerProvider Default { get; } = new();

    /// <inheritdoc />
    public IScheduler Background { get; } = TaskPoolScheduler.Default;

    /// <inheritdoc />
    public IScheduler Ui { get; } = AvaloniaScheduler.Current;
}
