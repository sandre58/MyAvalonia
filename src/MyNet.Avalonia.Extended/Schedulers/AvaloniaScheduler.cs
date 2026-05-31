// -----------------------------------------------------------------------
// <copyright file="AvaloniaScheduler.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using Avalonia.Threading;
using MyNet.Globalization.Culture;

namespace MyNet.Avalonia.Extended.Schedulers;

/// <summary>
/// Represents an object that schedules units of work on the Avalonia UI dispatcher.
/// </summary>
/// <param name="cultureContext">Provides the application culture to apply on the UI thread before executing work.</param>
/// <param name="priority">The dispatcher priority used when posting work items.</param>
public sealed class AvaloniaScheduler(ICultureContext cultureContext, DispatcherPriority priority)
    : LocalScheduler, ISchedulerPeriodic
{
    /// <summary>
    /// Limits reentrant inline schedules on the UI thread to prevent stack overflows.
    /// </summary>
    private const int MaxReentrantSchedules = 32;

    private readonly ICultureContext _cultureContext = cultureContext ?? throw new ArgumentNullException(nameof(cultureContext));
    private int _reentrancyGuard;

    /// <summary>
    /// Gets a fallback scheduler for non-DI scenarios such as unit tests.
    /// Prefer resolving <see cref="AvaloniaScheduler"/> from dependency injection.
    /// </summary>
    public static AvaloniaScheduler Current => field ??= new(new ThreadCultureContext(), DispatcherPriority.Render);

    /// <summary>
    /// Initializes a new instance of the <see cref="AvaloniaScheduler"/> class with the default dispatcher priority.
    /// </summary>
    /// <param name="cultureContext">Provides the application culture to apply on the UI thread before executing work.</param>
    public AvaloniaScheduler(ICultureContext cultureContext)
        : this(cultureContext, DispatcherPriority.Render)
    {
    }

    /// <summary>
    /// Gets the priority at which work items will be dispatched.
    /// </summary>
    public DispatcherPriority Priority { get; } = priority;

    /// <inheritdoc />
    public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        => Schedule(state, TimeSpan.Zero, action);

    /// <inheritdoc />
    public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var normalizedDueTime = Scheduler.Normalize(dueTime);
        if (normalizedDueTime.Ticks == 0)
        {
            if (!Dispatcher.UIThread.CheckAccess() || _reentrancyGuard >= MaxReentrantSchedules)
                return PostOnDispatcher(state, action);

            try
            {
                _reentrancyGuard++;
                ApplyThreadCulture();
                return action(this, state);
            }
            finally
            {
                _reentrancyGuard--;
            }
        }

        var d = new MultipleAssignmentDisposable();
        var timer = new DispatcherTimer(Priority);

        timer.Tick += (_, _) =>
        {
            var currentTimer = Interlocked.Exchange(ref timer, null);
            try
            {
                ApplyThreadCulture();
                d.Disposable = action(this, state);
            }
            finally
            {
                currentTimer.Stop();
                action = (_, _) => Disposable.Empty;
            }
        };

        timer.Interval = normalizedDueTime;
        timer.Start();

        d.Disposable = Disposable.Create(() =>
        {
            var currentTimer = Interlocked.Exchange(ref timer, null);
            currentTimer.Stop();
            action = (_, _) => Disposable.Empty;
        });

        return d;
    }

    /// <summary>
    /// Schedules a periodic piece of work on the dispatcher, using a <see cref="DispatcherTimer"/> object.
    /// </summary>
    /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
    /// <param name="state">Initial state passed to the action upon the first iteration.</param>
    /// <param name="period">Period for running the work periodically.</param>
    /// <param name="action">Action to be executed, potentially updating the state.</param>
    /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
    public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(period, TimeSpan.Zero);
        ArgumentNullException.ThrowIfNull(action);

        var timer = new DispatcherTimer(Priority);
        var state1 = state;

        timer.Tick += (_, _) =>
        {
            ApplyThreadCulture();
            state1 = action(state1);
        };

        timer.Interval = period;
        timer.Start();

        return Disposable.Create(() =>
        {
            var currentTimer = Interlocked.Exchange(ref timer, null);
            currentTimer.Stop();
            action = x => x;
        });
    }

    private SingleAssignmentDisposable PostOnDispatcher<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
        var d = new SingleAssignmentDisposable();

        Dispatcher.UIThread.Post(() =>
            {
                if (d.IsDisposed)
                    return;

                ApplyThreadCulture();
                d.Disposable = action(this, state);
            },
            Priority);

        return d;
    }

    private void ApplyThreadCulture()
    {
        var culture = _cultureContext.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}
