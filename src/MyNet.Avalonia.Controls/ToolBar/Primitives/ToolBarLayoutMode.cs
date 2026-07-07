// -----------------------------------------------------------------------
// <copyright file="ToolBarLayoutMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Selects the layout strategy used by <see cref="ToolBar"/>.
/// Each mode maps to an <see cref="IToolBarLayoutEngine"/> implementation via <see cref="ToolBarLayoutEngineFactory"/>.
/// </summary>
public enum ToolBarLayoutMode
{
    /// <summary>Horizontal left-to-right placement with adaptive overflow menu.</summary>
    Standard,

    /// <summary>Icon-only items, no label text regardless of available space.</summary>
    Compact,

    /// <summary>Fluent-style layout with smooth compact transition (Phase 2).</summary>
    Fluent,

    /// <summary>Multi-row ribbon layout (Phase 2).</summary>
    Ribbon
}
