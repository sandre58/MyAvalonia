// -----------------------------------------------------------------------
// <copyright file="ToolBarOverflowPriority.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Controls when a <see cref="ToolBarItem"/> moves to the overflow menu.
/// Items with higher numeric values overflow first.
/// </summary>
public enum ToolBarOverflowPriority
{
    /// <summary>Item is always visible regardless of available space.</summary>
    NeverOverflow = 0,

    /// <summary>Standard item — overflows after Low-priority items.</summary>
    Normal = 1,

    /// <summary>Low-priority item — overflows before Normal items.</summary>
    Low = 2,

    /// <summary>Item is always in the overflow menu regardless of available space.</summary>
    AlwaysOverflow = 3
}
