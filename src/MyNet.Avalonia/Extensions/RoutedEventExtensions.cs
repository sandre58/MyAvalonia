// -----------------------------------------------------------------------
// <copyright file="RoutedEventExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia.Interactivity;

namespace MyNet.Avalonia.Extensions;

public static class RoutedEventExtensions
{
    public static void AddHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        foreach (var t in controls)
        {
            t?.AddHandler(routedEvent, handler);
        }
    }

    public static void AddHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params TControl?[] controls)
        where TControl : Interactive
        where TArgs : RoutedEventArgs
    {
        foreach (var t in controls)
        {
            t?.AddHandler(routedEvent, handler);
        }
    }

    public static void AddHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        foreach (var t in controls)
        {
            t?.AddHandler(routedEvent, handler, strategies, handledEventsToo);
        }
    }

    public static void AddHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false,
        params TControl?[] controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        foreach (var t in controls)
        {
            t?.AddHandler(routedEvent, handler, strategies, handledEventsToo);
        }
    }

    public static void AddHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        IEnumerable<TControl?> controls,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        foreach (var t in controls)
        {
            t?.AddHandler(routedEvent, handler, strategies, handledEventsToo);
        }
    }

    public static void RemoveHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        foreach (var t in controls)
        {
            t?.RemoveHandler(routedEvent, handler);
        }
    }

    public static void RemoveHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params TControl?[] controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        foreach (var t in controls)
        {
            t?.RemoveHandler(routedEvent, handler);
        }
    }

    public static void RemoveHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        IEnumerable<TControl?> controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        foreach (var t in controls)
        {
            t?.RemoveHandler(routedEvent, handler);
        }
    }

    public static IDisposable AddDisposableHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        var list = new List<IDisposable>(controls.Length);
        list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler)).OfType<IDisposable>());

        var result = new CompositeDisposable(list);
        return result;
    }

    public static IDisposable AddDisposableHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params TControl?[] controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        var list = new List<IDisposable>(controls.Length);
        list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler)).OfType<IDisposable>());

        var result = new CompositeDisposable(list);
        return result;
    }

    public static IDisposable AddDisposableHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        var list = new List<IDisposable>(controls.Length);
        list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo)).OfType<IDisposable>());

        var result = new CompositeDisposable(list);
        return result;
    }

    public static IDisposable AddDisposableHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false,
        params TControl?[] controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        var list = new List<IDisposable>(controls.Length);
        list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo)).OfType<IDisposable>());

        var result = new CompositeDisposable(list);
        return result;
    }

    public static IDisposable AddDisposableHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        IEnumerable<TControl> controls,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        // list is not initialized with controls.Count() to avoid multiple enumeration
        var list = controls.Select(t => t.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo)).ToList();

        var result = new CompositeDisposable(list);
        return result;
    }
}
