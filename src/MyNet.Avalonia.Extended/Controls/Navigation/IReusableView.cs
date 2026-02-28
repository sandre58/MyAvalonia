// -----------------------------------------------------------------------
// <copyright file="IReusableView.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Extended.Controls;

/// <summary>
/// Provides an interface for views that can be reused, allowing them to reset their state when necessary.
/// </summary>
public interface IReusableView
{
    /// <summary>
    /// Resets the view to its initial state, allowing it to be reused without retaining any previous state or data.
    /// </summary>
    void Reset();
}
