// -----------------------------------------------------------------------
// <copyright file="ToolBarItemViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

/// <summary>
/// Demo item model bound to <see cref="ToolBarItem"/> via <c>ItemsSource</c> + <c>ItemTemplate</c>.
/// </summary>
internal sealed class ToolBarItemViewModel
{
    public string Title { get; init; } = string.Empty;

    public MaterialIconKind? Icon { get; init; }

    public required ICommand Command { get; init; }

    public ToolBarOverflowPriority OverflowPriority { get; init; } = ToolBarOverflowPriority.Normal;

    public bool IsEnabled { get; init; } = true;
}
