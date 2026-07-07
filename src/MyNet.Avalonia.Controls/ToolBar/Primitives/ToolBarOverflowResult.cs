// -----------------------------------------------------------------------
// <copyright file="ToolBarOverflowResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Result of <see cref="IToolBarOverflowEngine.Resolve"/>.
/// <see cref="OverflowItems"/> contains the toolbar child states moved out of the main strip.
/// Each state's <see cref="ToolBarChildState.PopupItem"/> is the data fed to the overflow popup.
/// </summary>
public sealed record ToolBarOverflowResult(
    IReadOnlyList<ToolBarChildState> VisibleItems,
    IReadOnlyList<ToolBarChildState> OverflowItems,
    bool HasOverflow);
