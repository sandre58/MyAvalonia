// -----------------------------------------------------------------------
// <copyright file="ToolBarLayoutContext.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Layout;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Immutable dependency container passed from <see cref="ToolBar"/> to <see cref="ToolBarPanel"/>.
/// This is the ONLY contract between the two classes — ToolBarPanel must not
/// reference ToolBar directly or use FindAncestorOfType.
/// </summary>
internal sealed class ToolBarLayoutContext
{
    internal required IToolBarLayoutEngine Engine { get; init; }

    internal required IToolBarOverflowEngine OverflowEngine { get; init; }

    internal required Orientation Orientation { get; init; }

    internal required double ItemSpacing { get; init; }

    internal required ToolBarOverflowMode OverflowMode { get; init; }

    /// <summary>
    /// Width reserved for the overflow toggle when <see cref="OverflowMode"/> is not <see cref="ToolBarOverflowMode.None"/>.
    /// Matches the themed <c>MinWidth</c> of <c>PART_OverflowButton</c>.
    /// </summary>
    internal double OverflowButtonReserveWidth { get; init; }

    /// <summary>
    /// Maps a strip container to the data object displayed in the overflow popup.
    /// Provided by <see cref="ToolBar"/> — the panel never calls <c>IndexFromContainer</c>.
    /// </summary>
    internal required Func<Control, object?> ResolvePopupItem { get; init; }
}
