// -----------------------------------------------------------------------
// <copyright file="CountBasedLifetimeSupervisor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using MyNet.Avalonia.UI.Toasting.Lifetime.Clear;

namespace MyNet.Avalonia.UI.Toasting.Lifetime;

public sealed class CountBasedLifetimeSupervisor(MaximumToastsCount maximumToastsCount) : IToastLifetimeSupervisor
{
    private readonly int _maximumToastsCount = maximumToastsCount.Count;
    private readonly ToastsList _toasts = [];

    public void PushToast(Toast toast)
    {
        if (_disposed)
        {
            Debug.WriteLine($"Warn CountBasedLifetimeSupervisor {this}.{nameof(PushToast)} is already disposed");
            return;
        }

        var numberOfToastsToClose = Math.Max(_toasts.Count - _maximumToastsCount, 0);

        var toastsToRemove = _toasts
            .OrderBy(x => x.Key)
            .Take(numberOfToastsToClose)
            .Select(x => x.Value)
            .ToList();

        foreach (var n in toastsToRemove)
            CloseToast(n.Toast);

        _ = _toasts.Add(toast);
        toast.CloseRequest += ToastClosedCallback;
        RequestShowToast(new ShowToastEventArgs(toast));
    }

    public void CloseToast(Toast toast)
    {
        _ = _toasts.TryRemove(toast.GetHashCode(), out var removedToast);

        if (removedToast is null)
            return;
        removedToast.Toast.Close();
        removedToast.Toast.CloseRequest -= ToastClosedCallback;
    }

    private void ToastClosedCallback(object? sender, EventArgs e) => RequestCloseToast(new CloseToastEventArgs((Toast)sender!));

    private void RequestShowToast(ShowToastEventArgs e) => ShowToastRequested?.Invoke(this, e);

    private void RequestCloseToast(CloseToastEventArgs e) => CloseToastRequested?.Invoke(this, e);

    private bool _disposed;

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _toasts.Clear();
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
