// -----------------------------------------------------------------------
// <copyright file="RoutedEventExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia.Interactivity;

namespace MyNet.Avalonia.Extensions;

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class RoutedEventExtensions
{
    extension<TArgs>(RoutedEvent<TArgs> routedEvent)
        where TArgs : RoutedEventArgs
    {
        public void AddHandler(EventHandler<TArgs> handler,
            params Interactive?[] controls)
        {
            foreach (var t in controls)
            {
                t?.AddHandler(routedEvent, handler);
            }
        }

        public void AddHandler<TControl>(EventHandler<TArgs> handler,
            params TControl?[] controls)
            where TControl : Interactive
        {
            foreach (var t in controls)
            {
                t?.AddHandler(routedEvent, handler);
            }
        }

        public void AddHandler(EventHandler<TArgs> handler,
            RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
            bool handledEventsToo = false,
            params Interactive?[] controls)
        {
            foreach (var t in controls)
            {
                t?.AddHandler((RoutedEvent)routedEvent, handler, strategies, handledEventsToo);
            }
        }

        public void AddHandler<TControl>(EventHandler<TArgs> handler,
            RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
            bool handledEventsToo = false,
            params TControl?[] controls)
            where TControl : Interactive
        {
            foreach (var t in controls)
            {
                t?.AddHandler(routedEvent, (Delegate)handler, strategies, handledEventsToo);
            }
        }

        public void AddHandler<TControl>(EventHandler<TArgs> handler,
            IEnumerable<TControl?> controls,
            RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
            bool handledEventsToo = false)
            where TControl : Interactive
        {
            foreach (var t in controls)
            {
                t?.AddHandler(routedEvent, (Delegate)handler, strategies, handledEventsToo);
            }
        }

        public void RemoveHandler(EventHandler<TArgs> handler,
            params Interactive?[] controls)
        {
            foreach (var t in controls)
            {
                t?.RemoveHandler((RoutedEvent)routedEvent, handler);
            }
        }

        public void RemoveHandler<TControl>(EventHandler<TArgs> handler,
            params TControl?[] controls)
            where TControl : Interactive
        {
            foreach (var t in controls)
            {
                t?.RemoveHandler(routedEvent, (Delegate)handler);
            }
        }

        public void RemoveHandler<TControl>(EventHandler<TArgs> handler,
            IEnumerable<TControl?> controls)
            where TControl : Interactive
        {
            foreach (var t in controls)
            {
                t?.RemoveHandler(routedEvent, (Delegate)handler);
            }
        }

        public IDisposable AddDisposableHandler(EventHandler<TArgs> handler,
            params Interactive?[] controls)
        {
            var list = new List<IDisposable>(controls.Length);
            list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler)).OfType<IDisposable>());

            var result = new CompositeDisposable(list);
            return result;
        }

        public IDisposable AddDisposableHandler<TControl>(EventHandler<TArgs> handler,
            params TControl?[] controls)
            where TControl : Interactive
        {
            var list = new List<IDisposable>(controls.Length);
            list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler)).OfType<IDisposable>());

            var result = new CompositeDisposable(list);
            return result;
        }

        public IDisposable AddDisposableHandler(EventHandler<TArgs> handler,
            RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
            bool handledEventsToo = false,
            params Interactive?[] controls)
        {
            var list = new List<IDisposable>(controls.Length);
            list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo)).OfType<IDisposable>());

            var result = new CompositeDisposable(list);
            return result;
        }

        public IDisposable AddDisposableHandler<TControl>(EventHandler<TArgs> handler,
            RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
            bool handledEventsToo = false,
            params TControl?[] controls)
            where TControl : Interactive
        {
            var list = new List<IDisposable>(controls.Length);
            list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo)).OfType<IDisposable>());

            var result = new CompositeDisposable(list);
            return result;
        }

        public IDisposable AddDisposableHandler<TControl>(EventHandler<TArgs> handler,
            IEnumerable<TControl> controls,
            RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
            bool handledEventsToo = false)
            where TControl : Interactive
        {
            // list is not initialized with controls.Count() to avoid multiple enumeration
            var list = controls.Select(t => t.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo)).ToList();

            var result = new CompositeDisposable(list);
            return result;
        }
    }
}
