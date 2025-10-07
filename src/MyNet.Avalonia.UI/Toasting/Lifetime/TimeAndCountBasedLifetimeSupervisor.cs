// -----------------------------------------------------------------------
// <copyright file="TimeAndCountBasedLifetimeSupervisor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MyNet.Avalonia.UI.Toasting.Lifetime.Clear;
using MyNet.UI.Toasting.Settings;

namespace MyNet.Avalonia.UI.Toasting.Lifetime;

public sealed class TimeAndCountBasedLifetimeSupervisor(TimeSpan duration, MaximumToastsCount maximumToastsCount) : IToastLifetimeSupervisor
{
    private readonly int _maximumNotificationsCount = maximumToastsCount.Count;
    private readonly Interval _interval = new();

    private readonly ToastsList _toasts = [];
    private Queue<Toast>? _toastPending;

    public void PushToast(Toast toast)
    {
        if (_disposed)
        {
            Debug.WriteLine($"Warn NotificationNotifications {this}.{nameof(PushToast)} is already disposed");
            return;
        }

        if (!_interval.IsRunning)
            TimerStart();

        if (_toasts.Count == _maximumNotificationsCount)
        {
            _toastPending ??= new Queue<Toast>();
            _toastPending.Enqueue(toast);
            return;
        }

        var numberOfNotificationsToClose = Math.Max(_toasts.Count - _maximumNotificationsCount + 1, 0);

        var notificationsToRemove = _toasts
            .OrderBy(x => x.Key)
            .Take(numberOfNotificationsToClose)
            .Select(x => x.Value)
            .ToList();

        foreach (var t in notificationsToRemove)
            CloseToast(t.Toast);

        _ = _toasts.Add(toast);
        toast.CloseRequest += ToastClosedCallback;

        RequestShowToast(new ShowToastEventArgs(toast));
    }

    public void CloseToast(Toast toast)
    {
        _ = _toasts.TryRemove(toast.GetHashCode(), out var removedNotification);

        if (removedNotification is not null)
        {
            removedNotification.Toast.Close();
            removedNotification.Toast.CloseRequest -= ToastClosedCallback;
        }

        if (_toastPending == null || _toastPending.Count == 0)
            return;
        var not = _toastPending.Dequeue();
        PushToast(not);
    }

    private bool _disposed;

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _interval.Stop();
        _toasts.Clear();
        _toastPending?.Clear();
    }

    private void ToastClosedCallback(object? sender, EventArgs e) => RequestCloseToast(new CloseToastEventArgs((Toast)sender!));

    private void RequestShowToast(ShowToastEventArgs e) => ShowToastRequested?.Invoke(this, e);

    private void RequestCloseToast(CloseToastEventArgs e) => CloseToastRequested?.Invoke(this, e);

    private void TimerStart() => _interval.Invoke(TimeSpan.FromMilliseconds(200), OnTimerTick);

    private void TimerStop() => _interval.Stop();

    private void OnTimerTick()
    {
        var now = DateTime.Now.TimeOfDay;

        var toastsToRemove = _toasts
            .Where(x => x.Value.Toast.Settings.ClosingStrategy is ToastClosingStrategy.AutoClose or ToastClosingStrategy.Both && !x.Value.Toast.IsLocked && x.Value.CreatedDate + duration <= now)
            .Select(x => x.Value)
            .ToList();

        foreach (var t in toastsToRemove)
            CloseToast(t.Toast);

        if (_toasts.IsEmpty)
            TimerStop();
    }

    public void ClearToasts(IClearStrategy clearStrategy)
    {
        var toasts = clearStrategy.GetToastsToRemove(_toasts.Select(x => x.Value.Toast));
        foreach (var toast in toasts)
        {
            CloseToast(toast);
        }
    }

    public event EventHandler<ShowToastEventArgs>? ShowToastRequested;

    public event EventHandler<CloseToastEventArgs>? CloseToastRequested;
}
