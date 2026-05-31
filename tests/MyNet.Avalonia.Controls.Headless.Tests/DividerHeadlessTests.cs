// -----------------------------------------------------------------------
// <copyright file="DividerHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.VisualTree;
using FluentAssertions;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class DividerHeadlessTests
{
    [AvaloniaFact]
    public void ApplyTemplate_RendersWithTheme()
    {
        var divider = new Divider
        {
            Content = new TextBlock { Text = "Section" },
        };

        HeadlessControlHost.Show(divider, new(320, 48));

        divider.GetVisualDescendants().OfType<TextBlock>().Should().ContainSingle(x => x.Text == "Section");
    }

    [AvaloniaFact]
    public void Orientation_CanBeChanged()
    {
        var divider = new Divider { Orientation = Orientation.Vertical };

        HeadlessControlHost.Show(divider, new(48, 320));

        divider.Orientation.Should().Be(Orientation.Vertical);
    }
}
