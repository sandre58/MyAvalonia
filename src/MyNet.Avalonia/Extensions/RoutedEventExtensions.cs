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

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class RoutedEventExtensions
{
    extension<TArgs>(RoutedEvent<TArgs> routedEvent)
        where TArgs : RoutedEventArgs
    {
        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        public void AddHandler(EventHandler<TArgs> handler,
            params Interactive?[] controls)
        {
            foreach (var t in controls)
            {
                t?.AddHandler(routedEvent, handler);
            }
        }

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        /// <typeparam name="TControl">The type of the controls to which the event handler will be added.</typeparam>
        public void AddHandler<TControl>(EventHandler<TArgs> handler,
            params TControl?[] controls)
            where TControl : Interactive
        {
            foreach (var t in controls)
            {
                t?.AddHandler(routedEvent, handler);
            }
        }

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="strategies">The routing strategies to use when adding the handler.</param>
        /// <param name="handledEventsToo">Whether to handle events that have already been handled.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
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

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="strategies">The routing strategies to use when adding the handler.</param>
        /// <param name="handledEventsToo">Whether to handle events that have already been handled.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        /// <typeparam name="TControl">The type of the controls to which the event handler will be added.</typeparam>
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

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        /// <param name="strategies">The routing strategies to use when adding the handler.</param>
        /// <param name="handledEventsToo">Whether to handle events that have already been handled.</param>
        /// <typeparam name="TControl">The type of the controls to which the event handler will be added.</typeparam>
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

        /// <summary>
        /// Removes the specified event handler for the given routed event from each of the provided controls. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to remove.</param>
        /// <param name="controls">The controls from which the event handler will be removed.</param>
        public void RemoveHandler(EventHandler<TArgs> handler,
            params Interactive?[] controls)
        {
            foreach (var t in controls)
            {
                t?.RemoveHandler((RoutedEvent)routedEvent, handler);
            }
        }

        /// <summary>
        /// Removes the specified event handler for the given routed event from each of the provided controls. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to remove.</param>
        /// <param name="controls">The controls from which the event handler will be removed.</param>
        /// <typeparam name="TControl">The type of the controls from which the event handler will be removed.</typeparam>
        public void RemoveHandler<TControl>(EventHandler<TArgs> handler,
            params TControl?[] controls)
            where TControl : Interactive
        {
            foreach (var t in controls)
            {
                t?.RemoveHandler(routedEvent, (Delegate)handler);
            }
        }

        /// <summary>
        /// Removes the specified event handler for the given routed event from each of the provided controls. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to remove.</param>
        /// <param name="controls">The controls from which the event handler will be removed.</param>
        /// <typeparam name="TControl">The type of the controls from which the event handler will be removed.</typeparam>
        public void RemoveHandler<TControl>(EventHandler<TArgs> handler,
            IEnumerable<TControl?> controls)
            where TControl : Interactive
        {
            foreach (var t in controls)
            {
                t?.RemoveHandler(routedEvent, (Delegate)handler);
            }
        }

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls, and returns an IDisposable that can be used to remove the handlers when disposed. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        /// <returns>An IDisposable that can be used to remove the handlers when disposed.</returns>
        public IDisposable AddDisposableHandler(EventHandler<TArgs> handler,
            params Interactive?[] controls)
        {
            var list = new List<IDisposable>(controls.Length);
            list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler)).OfType<IDisposable>());

            var result = new CompositeDisposable(list);
            return result;
        }

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls, and returns an IDisposable that can be used to remove the handlers when disposed. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        /// <typeparam name="TControl">The type of the controls to which the event handler will be added.</typeparam>
        /// <returns>An IDisposable that can be used to remove the handlers when disposed.</returns>
        public IDisposable AddDisposableHandler<TControl>(EventHandler<TArgs> handler,
            params TControl?[] controls)
            where TControl : Interactive
        {
            var list = new List<IDisposable>(controls.Length);
            list.AddRange(controls.Select(t => t?.AddDisposableHandler(routedEvent, handler)).OfType<IDisposable>());

            var result = new CompositeDisposable(list);
            return result;
        }

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls, and returns an IDisposable that can be used to remove the handlers when disposed. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="strategies">The routing strategies to use for the event handler.</param>
        /// <param name="handledEventsToo">Whether to handle events that have already been handled.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        /// <returns>An IDisposable that can be used to remove the handlers when disposed.</returns>
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

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls, and returns an IDisposable that can be used to remove the handlers when disposed. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="strategies">The routing strategies to use for the event handler.</param>
        /// <param name="handledEventsToo">Whether to handle events that have already been handled.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        /// <typeparam name="TControl">The type of the controls.</typeparam>
        /// <returns>An IDisposable that can be used to remove the handlers when disposed.</returns>
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

        /// <summary>
        /// Adds the specified event handler for the given routed event to each of the provided controls, and returns an IDisposable that can be used to remove the handlers when disposed. If a control in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="controls">The controls to which the event handler will be added.</param>
        /// <param name="strategies">The routing strategies to use for the event handler.</param>
        /// <param name="handledEventsToo">Whether to handle events that have already been handled.</param>
        /// <typeparam name="TControl">The type of the controls.</typeparam>
        /// <returns>An IDisposable that can be used to remove the handlers when disposed.</returns>
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
