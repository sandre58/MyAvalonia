// -----------------------------------------------------------------------
// <copyright file="ToolBarOverflowInput.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Input data for <see cref="IToolBarOverflowEngine.Resolve"/>.
/// <see cref="AvailableSize"/> is the <c>finalSize</c> received by <c>ArrangeOverride</c>.
/// <see cref="OverflowButtonReserveWidth"/> is subtracted in adaptive mode so the strip
/// does not oscillate when the overflow toggle becomes visible.
/// </summary>
public sealed record ToolBarOverflowInput(
    IReadOnlyList<ToolBarChildState> AllItems,
    Size AvailableSize,
    double Spacing,
    ToolBarOverflowMode OverflowMode,
    double OverflowButtonReserveWidth = 0);
