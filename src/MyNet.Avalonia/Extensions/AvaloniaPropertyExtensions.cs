// -----------------------------------------------------------------------
// <copyright file="AvaloniaPropertyExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MyNet.Avalonia.Extensions;

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class AvaloniaPropertyExtensions
{
    extension<T>(AvaloniaProperty<T> property)
    {
        public void SetValue(T value, params AvaloniaObject?[] objects)
        {
            foreach (var t in objects)
            {
                _ = t?.SetValue(property, value);
            }
        }

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
        public void AffectsPseudoClass<TControl>(string pseudoClass, RoutedEvent<RoutedEventArgs>? routedEvent = null)
            where TControl : Control
        {
            var pseudoClass2 = pseudoClass;
            var routedEvent2 = routedEvent;
            _ = property.Changed.AddClassHandler((TControl control, AvaloniaPropertyChangedEventArgs<bool> args) => OnPropertyChanged(control, args, pseudoClass2, routedEvent2));
        }

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
