// -----------------------------------------------------------------------
// <copyright file="ToolBarOverflowEntry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Material.Icons;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Lightweight data object for overflow popup items when toolbar children are declared
/// directly as <see cref="ToolBarItem"/> controls (not via <c>ItemsSource</c>).
/// Never hosts a live control — safe to bind in <see cref="ToolBar.OverflowItemTemplate"/>.
/// </summary>
public sealed class ToolBarOverflowEntry
{
    public object? Header { get; init; }

    public object? Icon { get; init; }

    public IDataTemplate? HeaderTemplate { get; init; }

    public ICommand? Command { get; init; }

    public bool IsEnabled { get; init; } = true;

    public bool IsSeparator { get; init; }

    internal static ToolBarOverflowEntry FromItem(ToolBarItem item) => new()
    {
        Header = DetachContent(item.Header),
        Icon = DetachContent(item.Icon),
        HeaderTemplate = item.HeaderTemplate,
        Command = item.Command,
        IsEnabled = item.IsEnabled,
    };

    internal static ToolBarOverflowEntry Separator { get; } = new() { IsSeparator = true };

    /// <summary>
    /// Copies display content without reusing live <see cref="Control"/> instances from the toolbar strip.
    /// </summary>
    private static object? DetachContent(object? content) => content switch
    {
        MaterialIcon { Kind: { } kind } => kind,
        Control => null,
        _ => content,
    };
}
