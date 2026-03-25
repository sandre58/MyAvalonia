// -----------------------------------------------------------------------
// <copyright file="PageTransitionType.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Classes.Enums;

/// <summary>
/// Enumerates standard page transition types for use in navigation.
/// Each value represents a predefined transition animation style.
/// </summary>
public enum PageTransitionType
{
    /// <summary>
    /// No transition animation.
    /// </summary>
    None,

    /// <summary>
    /// Slide transition animation.
    /// </summary>
    Slide,

    /// <summary>
    /// Crossfade transition animation.
    /// </summary>
    Crossfade,

    /// <summary>
    /// Composite transition combining multiple effects.
    /// </summary>
    Composite
}
