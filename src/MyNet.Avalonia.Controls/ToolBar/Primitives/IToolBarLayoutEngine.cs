// -----------------------------------------------------------------------
// <copyright file="IToolBarLayoutEngine.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia;
using Avalonia.Layout;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Strategy contract for deterministic toolbar layout.
/// Implementations are responsible for calculating sizes and positions only —
/// no overflow decisions and no Arrange calls on child controls.
/// </summary>
public interface IToolBarLayoutEngine
{
    /// <summary>
    /// Phase 1 — Measure: returns the total desired size for <paramref name="allItems"/>
    /// (no overflow filtering, all items included).
    /// </summary>
    Size Measure(IReadOnlyList<ToolBarChildState> allItems, Orientation orientation, double spacing);

    /// <summary>
    /// Phase 3 — Arrange: computes explicit <see cref="Rect"/> positions
    /// for each item in <paramref name="input"/>.<see cref="ToolBarArrangeInput.VisibleItems"/>.
    /// Only visible (post-overflow) items are included.
    /// </summary>
    ToolBarLayoutResult Arrange(ToolBarArrangeInput input);
}
