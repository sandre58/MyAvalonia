// -----------------------------------------------------------------------
// <copyright file="ToolBarChildState.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Immutable snapshot of a toolbar child after the Measure phase.
/// Carries layout metrics for overflow/layout engines and the popup data payload
/// (<see cref="PopupItem"/>), resolved once by <see cref="ToolBar"/>.
/// </summary>
public sealed record ToolBarChildState(
    Control Element,
    Size DesiredSize,
    ToolBarOverflowPriority OverflowPriority,
    bool IsSeparator,
    object? PopupItem);
