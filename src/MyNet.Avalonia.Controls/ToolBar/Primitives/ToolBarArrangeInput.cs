// -----------------------------------------------------------------------
// <copyright file="ToolBarArrangeInput.cs" company="Stéphane ANDRE">
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
/// Input data for <see cref="IToolBarLayoutEngine.Arrange"/>.
/// Contains only post-overflow visible items; overflow items are not included.
/// </summary>
public sealed record ToolBarArrangeInput(
    IReadOnlyList<ToolBarChildState> VisibleItems,
    Size FinalSize,
    Orientation Orientation,
    double Spacing);
