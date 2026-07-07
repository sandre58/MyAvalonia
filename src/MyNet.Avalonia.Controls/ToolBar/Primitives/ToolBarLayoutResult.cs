// -----------------------------------------------------------------------
// <copyright file="ToolBarLayoutResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Result of <see cref="IToolBarLayoutEngine.Arrange"/>.
/// Contains the explicit <see cref="ArrangedItem"/> positions for every visible item
/// and the total <see cref="UsedSize"/> occupied by the arranged items.
/// </summary>
public sealed record ToolBarLayoutResult(
    IReadOnlyList<ArrangedItem> ArrangedItems,
    Size UsedSize);
