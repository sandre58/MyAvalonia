// -----------------------------------------------------------------------
// <copyright file="Interval.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Threading;

namespace MyNet.Avalonia.UI.Toasting.Lifetime;

public class Interval : IInterval
{
    private DispatcherTimer? _timer;

    public bool IsRunning => _timer is { IsEnabled: true };

    public void Invoke(TimeSpan frequency, Action action)
    {
        _timer = new DispatcherTimer
        {
            Interval = frequency,
            IsEnabled = true
        };
        _timer.Tick += (_, _) => action();
        _timer.Start();
    }

    public void Stop()
    {
        _timer?.Stop();
        _timer = null;
    }
}
