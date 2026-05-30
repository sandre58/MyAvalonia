// -----------------------------------------------------------------------
// <copyright file="ClassContext.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Avalonia.Controls;

namespace MyNet.Avalonia.Theme.Classes.Registry.States;

/// <summary>
/// Represents a utility class for managing state associated with controls in a structured manner. This class provides a context for updating and applying state changes to controls, allowing for temporary modifications that can be easily reverted if necessary. The ClassContext is designed to work with any control type and state class, making it a flexible tool for managing control-specific state in a consistent way across the application.
/// </summary>
internal static class ClassContext
{
    public static ClassContext<TControl, TState> Create<TControl, TState>(TControl control)
        where TControl : Control
        where TState : class, new() => new(control);
}

/// <summary>
/// Represents the context for managing class state associated with a specific control. This class provides mechanisms
/// to update and apply state changes in a controlled manner, ensuring that state modifications can be reverted if necessary.
/// </summary>
/// <typeparam name="TControl">The type of control for which the state is managed.</typeparam>
/// <typeparam name="TState">The type of state being managed.</typeparam>
/// <param name="control">The control instance associated with this context.</param>
internal sealed class ClassContext<TControl, TState>(TControl control)
    where TControl : Control
    where TState : class, new()
{
    /// <summary>
    /// Gets the control associated with this instance.
    /// </summary>
    /// <remarks>Use this property to access the underlying control for further manipulation or to retrieve
    /// its state. The control is initialized when the instance is created and remains unchanged for the lifetime of the
    /// instance.</remarks>
    public TControl Control { get; } = control;

    /// <summary>
    /// Gets the state associated with the control. This state is retrieved or created using the ClassState utility, ensuring that
    /// each control has its own independent state instance.
    /// </summary>
    public TState State { get; } = ClassState.GetOrCreate<TState>(control);

    /// <summary>
    /// Updates the current state by applying the specified update action and then applies the updated state to the
    /// control.
    /// </summary>
    /// <remarks>This method captures a snapshot of the current state before applying the update. Disposing
    /// the returned object reverts the state to its original value and updates the control accordingly. This can be
    /// useful for temporary state changes that need to be reverted.</remarks>
    /// <param name="update">An action that modifies the current state. This action should perform any desired changes to the state object.</param>
    /// <param name="apply">An action that applies the updated state to the control, allowing the control to reflect the changes made to the
    /// state.</param>
    /// <returns>An IDisposable that, when disposed, restores the state to its previous value and reapplies it to the control.</returns>
    public IDisposable Update(Action<TState> update, Action<TControl, TState> apply)
    {
        var snapshot = Snapshot(State);

        update(State);

        apply(Control, State);

        return Disposable.Create(() =>
        {
            Restore(State, snapshot);
            apply(Control, State);
        });
    }

    /// <summary>
    /// Takes a snapshot of the current state by creating a dictionary that maps property names to their corresponding values. This allows for
    /// restoring the state to its previous values if needed.  Only properties with a public setter are captured;
    /// get-only properties (such as <see cref="BindingGroup"/> or collection fields) are intentionally skipped.
    /// </summary>
    /// <param name="state">The state object to snapshot.</param>
    /// <returns>A dictionary mapping property names to their current values.</returns>
    private static Dictionary<string, object?> Snapshot(TState state)
    {
        var dict = new Dictionary<string, object?>();

        foreach (var p in typeof(TState).GetProperties())
        {
            if (p.CanWrite)
                dict[p.Name] = p.GetValue(state);
        }

        return dict;
    }

    /// <summary>
    /// Restores the property values of the specified state object from the provided snapshot.
    /// </summary>
    /// <remarks>Only properties present in the snapshot and that have a public setter are restored.
    /// Get-only properties are skipped, matching the behavior of <see cref="Snapshot"/>.</remarks>
    /// <param name="state">The object whose properties are to be set. Must be of type TState and have settable properties corresponding to
    /// the keys in the snapshot.</param>
    /// <param name="snapshot">A dictionary containing property names and their corresponding values to assign to the state object. Each key
    /// should match a property name on the state object.</param>
    private static void Restore(TState state, Dictionary<string, object?> snapshot)
    {
        foreach (var p in typeof(TState).GetProperties())
        {
            if (p.CanWrite && snapshot.TryGetValue(p.Name, out var value))
                p.SetValue(state, value);
        }
    }
}
