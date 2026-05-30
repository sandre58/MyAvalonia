// -----------------------------------------------------------------------
// <copyright file="AvaloniaObjectExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Helpers;

namespace MyNet.Avalonia.Extensions;

public static class AvaloniaObjectExtensions
{
    extension(AvaloniaObject obj)
    {
        /// <summary>
        /// Sets the specified Avalonia property on the given object to the provided value, supporting direct assignment,
        /// data binding, or markup extension resolution.
        /// </summary>
        /// <remarks>If the value is a BindingBase, the property is bound to the provided binding. If the value is
        /// a MarkupExtension, it is resolved using a service provider and the resulting binding is applied. Otherwise, the
        /// property is set directly to the specified value. Disposing the returned IDisposable will clear the property
        /// value, restoring its previous state.</remarks>
        /// <param name="property">The AvaloniaProperty that identifies the property to set. Cannot be null.</param>
        /// <param name="value">The value to assign to the property. This can be a direct value, a BindingBase for data binding, or a
        /// MarkupExtension to resolve a binding or value.</param>
        /// <param name="priority">The binding priority.</param>
        /// <returns>An IDisposable that, when disposed, clears the value of the specified property from the object.</returns>
        public IDisposable SetProperty(AvaloniaProperty property, object? value, BindingPriority priority = BindingPriority.StyleTrigger)
        {
            switch (value)
            {
                case BindingBase bindingBase:
                    {
                        if (bindingBase is Binding binding)
                        {
                            binding.Priority = priority;
                        }

                        return obj.Bind(property, bindingBase);
                    }

                case MarkupExtension markupExtension:
                    {
                        var serviceProvider = new MarkupServiceProvider(obj, property);
                        return obj.SetProperty(property, markupExtension.ProvideValue(serviceProvider), priority);
                    }

                case IObservable<object?> observable:
                    return obj.Bind(property, observable, priority);
            }

            if (value is not null)
            {
                var type = value.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IObservable<>))
                {
                    // Convert any IObservable<T> to IObservable<object?> and bind.
                    var converted =
                        System.Reactive.Linq.Observable.Select((dynamic)value,
                            (Func<dynamic, object?>)(object? (x) => x));
                    return obj.Bind(property, converted, priority);
                }
            }

            // Wrap the value in an observable and bind at the requested priority.
            // When the returned disposable is disposed, the value at this priority
            // is properly removed and the property falls back to lower priorities
            // (e.g. ControlTheme).
            var source = new BehaviorSubject<object?>(value);
            var bindingDisposable = obj.Bind(property, source, priority);
            return new CompositeDisposable(bindingDisposable, source);
        }

        public ResultDisposable TryBind(AvaloniaProperty property, BindingBase? binding)
            => binding == null
                ? new(Disposable.Empty, result: false)
                : new ResultDisposable(obj.Bind(property, binding), result: true);
    }

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

public sealed class ResultDisposable(IDisposable? disposable, bool result) : IDisposable
{
    public bool Result { get; } = result;

    public void Dispose() => disposable?.Dispose();
}
