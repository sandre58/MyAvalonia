// -----------------------------------------------------------------------
// <copyright file="OverlayDialogOptionsMergeTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extended.Dialogs.Internal;
using MyNet.UI.Dialogs.MessageBox;
using Xunit;

namespace MyNet.Avalonia.Extended.Tests.Dialogs;

public class OverlayDialogOptionsMergeTests
{
    [Fact]
    public void MergeOptions_CombinesFullScreenAndLightDismissWithOr()
    {
        var merged = OverlayDialogBuilder.MergeOptions(
            new() { FullScreen = true, CanLightDismiss = true },
            new());

        merged.FullScreen.Should().BeTrue();
        merged.CanLightDismiss.Should().BeTrue();
    }

    [Fact]
    public void MergeOptions_KeepsBaseAnchorWhenOverrideIsCenter()
    {
        var merged = OverlayDialogBuilder.MergeOptions(
            new() { HorizontalAnchor = HorizontalPosition.Left },
            new() { HorizontalAnchor = HorizontalPosition.Center });

        merged.HorizontalAnchor.Should().Be(HorizontalPosition.Left);
    }

    [Fact]
    public void MergeOptions_UsesOverrideAnchorWhenNotCenter()
    {
        var merged = OverlayDialogBuilder.MergeOptions(
            new() { HorizontalAnchor = HorizontalPosition.Center },
            new() { HorizontalAnchor = HorizontalPosition.Right });

        merged.HorizontalAnchor.Should().Be(HorizontalPosition.Right);
    }

    [Fact]
    public void MergeOptions_MergesTopLevelKey()
    {
        var merged = OverlayDialogBuilder.MergeOptions(
            new(),
            new() { TopLevelKey = 99 });

        merged.TopLevelKey.Should().Be(99);
    }

    [Fact]
    public void MergeOptions_KeepsBaseSeverityWhenOverrideIsCustom()
    {
        var merged = OverlayDialogBuilder.MergeOptions(
            new() { Severity = MessageSeverity.Warning },
            new() { Severity = MessageSeverity.Custom });

        merged.Severity.Should().Be(MessageSeverity.Warning);
    }
}
