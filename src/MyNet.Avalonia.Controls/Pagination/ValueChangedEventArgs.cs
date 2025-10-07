// -----------------------------------------------------------------------
// <copyright file="ValueChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class ValueChangedEventArgs<T>(RoutedEvent routedEvent, T? oldValue, T? newValue) : RoutedEventArgs(routedEvent)
    where T : struct, IComparable<T>
{
    public T? OldValue { get; } = oldValue;

    public T? NewValue { get; } = newValue;
}
