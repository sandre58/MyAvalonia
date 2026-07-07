// -----------------------------------------------------------------------
// <copyright file="IToolBarOverflowEngine.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Strategy contract for toolbar overflow resolution.
/// Completely independent from <see cref="IToolBarLayoutEngine"/> —
/// receives all items and returns the split between visible and overflow.
/// </summary>
public interface IToolBarOverflowEngine
{
    /// <summary>
    /// Phase 2 — Overflow: single-pass resolution that splits <paramref name="input"/>
    /// into visible and overflow item sets based on available size and priority.
    /// </summary>
    ToolBarOverflowResult Resolve(ToolBarOverflowInput input);
}
