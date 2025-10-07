// -----------------------------------------------------------------------
// <copyright file="IInterval.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.UI.Toasting.Lifetime;

public interface IInterval
{
    bool IsRunning { get; }

    void Invoke(TimeSpan frequency, Action action);

    void Stop();
}
