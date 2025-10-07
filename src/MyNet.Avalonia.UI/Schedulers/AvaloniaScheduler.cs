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
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.UI.Schedulers;

/// <summary>
/// Represents an object that schedules units of work on a dispatcher.
/// </summary>
public class AvaloniaScheduler(DispatcherPriority priority) : LocalScheduler, ISchedulerPeriodic
{
    /// <summary>
    /// Gets the scheduler that schedules work on the dispatcher for the current thread.
    /// </summary>
    public static AvaloniaScheduler Current => field ??= new AvaloniaScheduler();

    public AvaloniaScheduler()
        : this(DispatcherPriority.Render) { }

    /// <summary>
    /// Gets the priority at which work items will be dispatched.
    /// </summary>
    public DispatcherPriority Priority { get; } = priority;

    /// <summary>
    /// Schedules an action to be executed on the dispatcher.
    /// </summary>
    /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
    /// <param name="state">State passed to the action to be executed.</param>
    /// <param name="action">Action to be executed.</param>
    /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var d = new SingleAssignmentDisposable();

        Dispatcher.UIThread.Invoke(() =>
            {
                if (d.IsDisposed)
                    return;
                Thread.CurrentThread.CurrentCulture = GlobalizationService.Current.Culture;
                Thread.CurrentThread.CurrentUICulture = GlobalizationService.Current.Culture;

                d.Disposable = action(this, state);
            },
            Priority);

        return d;
    }

    /// <summary>
    /// Schedules an action to be executed after dueTime on the dispatcher, using a <see cref="DispatcherTimer"/> object.
    /// </summary>
    /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
    /// <param name="state">State passed to the action to be executed.</param>
    /// <param name="dueTime">Relative time after which to execute the action.</param>
    /// <param name="action">Action to be executed.</param>
    /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var dt = Scheduler.Normalize(dueTime);
        if (dt.Ticks == 0)
            return Schedule(state, action);

        var d = new MultipleAssignmentDisposable();

        var timer = new DispatcherTimer(Priority);

        timer.Tick += (_, _) =>
        {
            var t = Interlocked.Exchange(ref timer, null);
            try
            {
                Thread.CurrentThread.CurrentCulture = GlobalizationService.Current.Culture;
                Thread.CurrentThread.CurrentUICulture = GlobalizationService.Current.Culture;

                d.Disposable = action(this, state);
            }
            finally
            {
                t.Stop();
                action = (_, _) => Disposable.Empty;
            }
        };

        timer.Interval = dt;
        timer.Start();

        d.Disposable = Disposable.Create(() =>
        {
            var t = Interlocked.Exchange(ref timer, null);
            t.Stop();
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
            Thread.CurrentThread.CurrentCulture = GlobalizationService.Current.Culture;
            Thread.CurrentThread.CurrentUICulture = GlobalizationService.Current.Culture;

            state1 = action(state1);
        };

        timer.Interval = period;
        timer.Start();

        return Disposable.Create(() =>
        {
            var t = Interlocked.Exchange(ref timer, null);
            t.Stop();
            action = x => x;
        });
    }
}
