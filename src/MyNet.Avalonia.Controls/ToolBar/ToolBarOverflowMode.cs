// -----------------------------------------------------------------------
// <copyright file="ToolBarOverflowMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Controls how items that do not fit in the toolbar are handled.
/// </summary>
public enum ToolBarOverflowMode
{
    /// <summary>No overflow — all items are always visible (no overflow menu).</summary>
    None,

    /// <summary>Items that do not fit are moved to an overflow menu.</summary>
    OverflowMenu,

    /// <summary>Overflow menu is shown automatically when items do not fit (default).</summary>
    Adaptive
}
