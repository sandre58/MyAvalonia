// -----------------------------------------------------------------------
// <copyright file="ClassDiffEngine.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using MyNet.Avalonia.Theme.Classes.Registry;

namespace MyNet.Avalonia.Theme.Classes.Engine;

/// <summary>
/// Diff engine responsible for applying the differences between the current set of style classes of a control and a new set of classes. It identifies which classes have been added or removed and applies the corresponding actions to the control. This class is intended for internal use and works in conjunction with ClassActionEngine to manage the application of style changes efficiently.
/// </summary>
internal static class ClassDiffEngine
{
    /// <summary>
    /// Applies the specified set of class names to the given control, updating the runtime state to reflect added and
    /// removed classes.
    /// </summary>
    /// <remarks>This method updates the active actions in the runtime state by disposing actions associated
    /// with classes that are no longer present and creating new actions for any added classes. It is important to
    /// ensure that the provided state object accurately represents the current classes and actions for the control to
    /// avoid inconsistencies.</remarks>
    /// <param name="control">The control to which the class changes are applied.</param>
    /// <param name="state">The current runtime state that tracks active classes and their associated actions for the control.</param>
    /// <param name="newClasses">An enumerable collection of class names to be applied to the control. Any classes not present in this collection
    /// will be removed.</param>
    public static void ApplyDiff(StyledElement control, ClassesRuntimeState state, IEnumerable<string> newClasses)
    {
        var next = new HashSet<string>(newClasses);

        var added = next.Except(state.Classes).ToList();
        foreach (var r in state.Classes.Except(next).ToList())
        {
            if (state.ActiveActions.TryGetValue(r, out var d))
            {
                d.Dispose();
                state.ActiveActions.Remove(r);
            }
        }

        foreach (var a in added)
        {
            var action = ClassRegistry.Resolve(a);

            if (action != null)
            {
                state.ActiveActions[a] = action(control);
            }
        }

        state.Classes = next;
    }
}

/// <summary>
/// Represents the runtime state of a control's style classes, including the current set of class names and the active actions associated with those classes. This state is used by the ClassDiffEngine to determine which actions to apply or remove when the set of classes changes. The Hash property can be used to track changes in the class set for caching purposes, while the Classes and ActiveActions properties maintain the current state of applied classes and their corresponding actions. This record is intended for internal use within the theme engine.
/// </summary>
internal sealed record ClassesRuntimeState
{
    /// <summary>
    /// Gets or sets a hash value representing the current set of classes. This hash can be used to quickly determine if the set of classes has changed, allowing for efficient caching and retrieval of compiled actions associated with the classes. The hash should be updated whenever the set of classes changes to ensure that the cache remains accurate and effective.
    /// </summary>
    public ulong Hash { get; set; }

    /// <summary>
    /// Gets or sets the collection of class names associated with the object.
    /// </summary>
    /// <remarks>This property allows for the management of class names, which can be used for styling or
    /// categorization purposes. The collection is represented as a HashSet to ensure that class names are unique and to
    /// provide efficient lookups.</remarks>
    public HashSet<string> Classes { get; set; } = [];

    /// <summary>
    /// Gets a dictionary that contains the currently active actions, each associated with a unique identifier.
    /// </summary>
    /// <remarks>Each entry in the dictionary represents an action that is currently managed and can be
    /// disposed of when no longer needed. The keys are unique string identifiers for each action.</remarks>
    public Dictionary<string, IDisposable> ActiveActions { get; } = [];
}
