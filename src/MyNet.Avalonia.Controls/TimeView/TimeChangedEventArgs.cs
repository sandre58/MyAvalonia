// -----------------------------------------------------------------------
// <copyright file="TimeChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class TimeChangedEventArgs(TimeSpan? oldTime, TimeSpan? newTime) : RoutedEventArgs
{
    public TimeSpan? OldTime { get; } = oldTime;

    public TimeSpan? NewTime { get; } = newTime;
}
