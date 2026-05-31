// -----------------------------------------------------------------------
// <copyright file="LoadedLifecycleExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

public static class LoadedLifecycleExtensions
{
    public static void OnLoading<T>(this AvaloniaObject? avaloniaObject, Action<T> onLoadAction, Action<T>? onUnloadAction = null)
        where T : Control
    {
        if (avaloniaObject is not T element) return;

        if (element.IsLoaded)
        {
            onLoadAction(element);
            element.Unloaded -= onUnloaded;
            element.Unloaded += onUnloaded;
        }
        else
        {
            element.Loaded -= onLoaded;
            element.Loaded += onLoaded;
        }

        void onLoaded(object? sender, RoutedEventArgs e)
        {
            onLoadAction(element);
            element.Loaded -= onLoaded;
            element.Unloaded -= onUnloaded;
            element.Unloaded += onUnloaded;
        }

        void onUnloaded(object? sender, RoutedEventArgs e)
        {
            onUnloadAction?.Invoke(element);
            element.Unloaded -= onUnloaded;
            element.Loaded -= onLoaded;
            element.Loaded += onLoaded;
        }
    }
}
