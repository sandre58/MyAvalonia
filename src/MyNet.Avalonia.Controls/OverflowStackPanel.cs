// -----------------------------------------------------------------------
// <copyright file="OverflowStackPanel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls;

public class OverflowStackPanel : StackPanel
{
    public Panel? OverflowPanel { get; set; }

    public void MoveChildrenToOverflowPanel()
    {
        var children = Children.ToList();
        foreach (var child in children)
        {
            _ = Children.Remove(child);
            OverflowPanel?.Children.Add(child);
        }
    }

    public void MoveChildrenToMainPanel()
    {
        var children = OverflowPanel?.Children.ToList();
        if (children is null || children.Count == 0)
            return;
        foreach (var child in children)
        {
            _ = OverflowPanel?.Children.Remove(child);
            Children.Add(child);
        }
    }
}
