// -----------------------------------------------------------------------
// <copyright file="AvaloniaSchedulerProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Concurrency;
using MyNet.UI.Threading;

namespace MyNet.Avalonia.Extended.Schedulers;

/// <summary>
/// Provides Avalonia UI and background schedulers for <see cref="MyNet.UI.Commands"/> and related UI services.
/// </summary>
/// <param name="uiScheduler">The Avalonia UI scheduler instance.</param>
public sealed class AvaloniaSchedulerProvider(AvaloniaScheduler uiScheduler) : ISchedulerProvider
{
    /// <inheritdoc />
    public IScheduler Background { get; } = TaskPoolScheduler.Default;

    /// <inheritdoc />
    public IScheduler Ui { get; } = uiScheduler ?? throw new ArgumentNullException(nameof(uiScheduler));
}
