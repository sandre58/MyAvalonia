// -----------------------------------------------------------------------
// <copyright file="RegionPresetsHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using FluentAssertions;
using Material.Icons;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class RegionPresetsHeadlessTests
{
    [AvaloniaFact]
    public void EmptyState_RendersTitleBlock()
    {
        var emptyState = new EmptyState
        {
            Title = "Nothing here",
            Subtitle = "Try again later",
            Leading = new MaterialIcon { Kind = MaterialIconKind.InboxOutline }
        };

        HeadlessControlHost.Show(emptyState, new(320, 240));

        var titleBlock = emptyState.GetVisualDescendants().OfType<TitleBlock>().FirstOrDefault();
        titleBlock.Should().NotBeNull();
        titleBlock.Title.Should().Be("Nothing here");
        titleBlock.Subtitle.Should().Be("Try again later");
    }
}
