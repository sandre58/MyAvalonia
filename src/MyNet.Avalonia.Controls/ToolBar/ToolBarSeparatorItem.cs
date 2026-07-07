// -----------------------------------------------------------------------
// <copyright file="ToolBarSeparatorItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Marker item for <see cref="ToolBar.ItemsSource"/> that materializes as a <see cref="ToolBarSeparator"/>.
/// Use this instead of placing live <see cref="ToolBarSeparator"/> controls in bound collections.
/// </summary>
public sealed class ToolBarSeparatorItem;
