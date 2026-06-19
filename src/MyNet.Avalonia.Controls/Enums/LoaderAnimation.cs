// -----------------------------------------------------------------------
// <copyright file="LoaderAnimation.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Enums;

/// <summary>
/// Visual style for <see cref="Loader"/>.
/// </summary>
public enum LoaderAnimation
{
    /// <summary>Rotating arc segment (Material-style).</summary>
    Circular,

    /// <summary>Full track with a rotating segment (Fluent-style).</summary>
    Ring,

    /// <summary>Three bouncing dots.</summary>
    Dots,

    /// <summary>Vertical bars with staggered scale animation.</summary>
    Bars,

    /// <summary>Pulsing filled circle.</summary>
    Pulse,
}
