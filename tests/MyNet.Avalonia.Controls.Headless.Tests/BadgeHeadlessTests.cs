// -----------------------------------------------------------------------
// <copyright file="BadgeHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using FluentAssertions;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class BadgeHeadlessTests
{
    [AvaloniaFact]
    public void ApplyTemplate_CreatesBadgeContainer()
    {
        var badge = new Badge
        {
            Header = "3",
            Content = new TextBlock { Text = "Messages" },
        };

        HeadlessControlHost.Show(badge, new(240, 120));

        HeadlessControlHost.FindByName<Panel>(badge, Badge.PartBadgeContainer).Should().NotBeNull();
    }

    [AvaloniaFact]
    public void CornerPosition_UpdatesBadgeContainerAlignment()
    {
        var badge = new Badge
        {
            Header = "1",
            CornerPosition = CornerPosition.BottomLeft,
            Content = new TextBlock { Text = "Item" },
        };

        HeadlessControlHost.Show(badge, new(240, 120));

        var container = HeadlessControlHost.FindByName<Panel>(badge, Badge.PartBadgeContainer);
        container.Should().NotBeNull();
        container!.HorizontalAlignment.Should().Be(global::Avalonia.Layout.HorizontalAlignment.Left);
        container.VerticalAlignment.Should().Be(global::Avalonia.Layout.VerticalAlignment.Bottom);
    }
}
