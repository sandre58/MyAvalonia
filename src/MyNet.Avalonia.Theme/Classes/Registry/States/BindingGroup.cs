// -----------------------------------------------------------------------
// <copyright file="BindingGroup.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;

namespace MyNet.Avalonia.Theme.Classes.Registry.States;

/// <summary>
/// Manages a group of <see cref="IDisposable"/> bindings (typically returned by
/// <c>SetProperty</c>) that belong to the same logical section of a control's
/// visual state.
/// <para>
/// Call <see cref="Reset"/> before re-applying a section: it disposes every
/// previously tracked binding so the underlying properties fall back to their
/// lower-priority values.  Then use <see cref="Add"/> to track the new bindings.
/// </para>
/// </summary>
/// <remarks>
/// This class is intended for use inside state objects of class registries
/// (e.g. <c>ControlState</c> in <c>VariantClassRegistry</c>) where
/// <c>SetProperty</c> creates bindings at <c>BindingPriority.StyleTrigger</c>
/// that cannot be removed by <c>ClearValue</c>.
/// </remarks>
internal sealed class BindingGroup : IDisposable
{
    private CompositeDisposable _bindings = [];

    /// <summary>
    /// Adds a binding disposable to this group so it will be disposed on the
    /// next <see cref="Reset"/> or <see cref="Dispose"/> call.
    /// </summary>
    /// <param name="binding">The disposable returned by <c>SetProperty</c>.</param>
    public void Add(IDisposable binding) => _bindings.Add(binding);

    /// <summary>
    /// Disposes all currently tracked bindings and prepares the group for a
    /// new set of bindings.  Call this at the beginning of every apply method
    /// before adding new bindings.
    /// </summary>
    public void Reset()
    {
        _bindings.Dispose();
        _bindings = [];
    }

    /// <summary>
    /// Disposes all currently tracked bindings.  After this call the group
    /// should not be reused.
    /// </summary>
    public void Dispose() => _bindings.Dispose();
}
