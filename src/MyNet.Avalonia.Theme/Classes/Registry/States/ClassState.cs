// -----------------------------------------------------------------------
// <copyright file="ClassState.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace MyNet.Avalonia.Theme.Classes.Registry.States;

/// <summary>
/// Provides methods for managing per-control class state within the Avalonia framework.
/// </summary>
/// <remarks>This class enables the retrieval or creation of a state store associated with a specific control,
/// allowing each control to maintain its own independent state. It uses an attached property to store state
/// information, ensuring that state is scoped to the individual control instance.</remarks>
internal static class ClassState
{
    private static readonly AttachedProperty<ClassStateStore?> StateStoreProperty = AvaloniaProperty.RegisterAttached<Control, ClassStateStore?>("ClassStateStore", typeof(ClassState));

    /// <summary>
    /// Gets the state object of the specified type associated with the given control, or creates and associates a new
    /// instance if none exists.
    /// </summary>
    /// <remarks>If the control does not already have an associated state store, a new state store is created
    /// and attached to the control before retrieving or creating the state object.</remarks>
    /// <typeparam name="TState">The type of state to retrieve or create. Must have a parameterless constructor.</typeparam>
    /// <param name="control">The control for which the state is to be retrieved or created. This parameter must not be null.</param>
    /// <returns>The state object of type TState associated with the specified control. If no such state exists, a new instance
    /// is created and associated with the control.</returns>
    public static TState GetOrCreate<TState>(Control control)
        where TState : new()
    {
        var store = control.GetValue(StateStoreProperty);

        if (store == null)
        {
            store = new();
            control.SetValue(StateStoreProperty, store);
        }

        return store.GetOrCreate<TState>();
    }
}

/// <summary>
/// Provides a mechanism to retrieve or create state objects of a specified type.
/// </summary>
/// <remarks>This class stores state objects in a dictionary, allowing for efficient retrieval and creation. The
/// state objects are created using the default constructor of the specified type. If a state of the requested type
/// already exists, it is returned; otherwise, a new instance is created and stored.</remarks>
internal sealed class ClassStateStore
{
    private readonly Dictionary<Type, object> _states = [];

    /// <summary>
    /// Gets an existing instance of the specified state type, or creates and stores a new instance if one does not
    /// already exist.
    /// </summary>
    /// <remarks>This method caches created instances to avoid unnecessary allocations. Subsequent calls for
    /// the same state type will return the same instance. This method is not thread-safe.</remarks>
    /// <typeparam name="TState">The type of state to retrieve or create. Must be a reference type with a parameterless constructor.</typeparam>
    /// <returns>An instance of the specified state type. If an instance already exists, the existing instance is returned;
    /// otherwise, a new instance is created and returned.</returns>
    public TState GetOrCreate<TState>()
        where TState : new()
    {
        if (_states.TryGetValue(typeof(TState), out var state))
            return (TState)state;

        var newState = new TState();
        _states[typeof(TState)] = newState;

        return newState;
    }
}
