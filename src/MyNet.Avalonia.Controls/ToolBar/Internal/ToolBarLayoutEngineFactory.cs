// -----------------------------------------------------------------------
// <copyright file="ToolBarLayoutEngineFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Maps a <see cref="ToolBarLayoutMode"/> enum value to the corresponding
/// <see cref="IToolBarLayoutEngine"/> implementation.
/// Compact/Fluent/Ribbon are stubs in Phase 1 — all map to <see cref="StandardToolBarLayoutEngine"/>.
/// </summary>
internal static class ToolBarLayoutEngineFactory
{
    internal static IToolBarLayoutEngine Create(ToolBarLayoutMode mode)
        => mode switch
        {
            ToolBarLayoutMode.Compact => new StandardToolBarLayoutEngine(), // Phase 2: CompactToolBarLayoutEngine
            ToolBarLayoutMode.Fluent => new StandardToolBarLayoutEngine(), // Phase 2: FluentToolBarLayoutEngine
            ToolBarLayoutMode.Ribbon => new StandardToolBarLayoutEngine(), // Phase 2: RibbonToolBarLayoutEngine
            _ => new StandardToolBarLayoutEngine(),
        };
}
