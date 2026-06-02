// -----------------------------------------------------------------------
// <copyright file="AvaloniaPropertyExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class AvaloniaPropertyExtensions
{
    extension<T>(AvaloniaProperty<T> property)
    {
        /// <summary>
        /// Sets the specified value for the given Avalonia property on each of the provided objects. If an object in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="value">The value to set for the property.</param>
        /// <param name="objects">The objects on which to set the property value.</param>
        public void SetValue(T value, params AvaloniaObject?[] objects)
        {
            foreach (var t in objects)
            {
                _ = t?.SetValue(property, value);
            }
        }

        /// <summary>
        /// Sets the specified value for the given Avalonia property on each of the provided objects. If an object in the list is null, it will be skipped without throwing an exception.
        /// </summary>
        /// <param name="value">The value to set for the property.</param>
        /// <param name="objects">The objects on which to set the property value.</param>
        /// <typeparam name="TControl">The type of the controls in the collection.</typeparam>
        public void SetValue<TControl>(T value, IEnumerable<TControl?> objects)
            where TControl : AvaloniaObject
        {
            foreach (var @object in objects)
            {
                _ = @object?.SetValue(property, value);
            }
        }
    }

    extension(AvaloniaProperty<bool> property)
    {
        /// <summary>
        /// Adds a class handler to the property changed event of the specified Avalonia boolean property, which sets the specified pseudo-class on the control based on the new value of the property. Optionally, a routed event can be raised when the property changes.
        /// </summary>
        /// <param name="pseudoClass">The pseudo-class to apply when the property value changes.</param>
        /// <param name="routedEvent">An optional routed event to raise when the property value changes.</param>
        /// <typeparam name="TControl">The type of the control on which the property is defined.</typeparam>
        public void AffectsPseudoClass<TControl>(string pseudoClass, RoutedEvent<RoutedEventArgs>? routedEvent = null)
            where TControl : Control
        {
            var pseudoClass2 = pseudoClass;
            var routedEvent2 = routedEvent;
            _ = property.Changed.AddClassHandler((TControl control, AvaloniaPropertyChangedEventArgs<bool> args) => OnPropertyChanged(control, args, pseudoClass2, routedEvent2));
        }

        /// <summary>
        /// Adds a class handler to the property changed event of the specified Avalonia boolean property, which sets the specified pseudo-class on the control based on the new value of the property. Optionally, a routed event can be raised when the property changes.
        /// </summary>
        /// <param name="pseudoClass">The pseudo-class to apply when the property value changes.</param>
        /// <param name="routedEvent">An optional routed event to raise when the property value changes.</param>
        /// <typeparam name="TControl">The type of the control on which the property is defined.</typeparam>
        /// <typeparam name="TArgs">The type of the routed event args.</typeparam>
        public void AffectsPseudoClass<TControl, TArgs>(string pseudoClass, RoutedEvent<TArgs>? routedEvent = null)
            where TControl : Control
            where TArgs : RoutedEventArgs, new()
        {
            var pseudoClass2 = pseudoClass;
            var routedEvent2 = routedEvent;
            _ = property.Changed.AddClassHandler((TControl control, AvaloniaPropertyChangedEventArgs<bool> args) => OnPropertyChanged(control, args, pseudoClass2, routedEvent2));
        }
    }

    private static void OnPropertyChanged<TControl, TArgs>(TControl control, AvaloniaPropertyChangedEventArgs<bool> args, string pseudoClass, RoutedEvent<TArgs>? routedEvent)
        where TControl : Control
        where TArgs : RoutedEventArgs, new()
    {
        PseudoClassesExtensions.Set(control.Classes, pseudoClass, args.NewValue.Value);

        if (routedEvent != null)
        {
            control.RaiseEvent(new TArgs { RoutedEvent = routedEvent });
        }
    }
}
