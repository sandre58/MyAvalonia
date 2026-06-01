// -----------------------------------------------------------------------
// <copyright file="OverlayDialogLayerTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Dialogs;

public class OverlayDialogLayerTests
{
    [Fact]
    public void ChangeLayer_RaisesLayerChangedWithExpectedType()
    {
        OverlayDialogLayerChangeType? received = null;
        var dialog = new OverlayDialog();
        dialog.LayerChanged += (_, args) => received = args.ChangeType;

        dialog.BringToFront();

        received.Should().Be(OverlayDialogLayerChangeType.BringToFront);
    }

    [Theory]
    [InlineData(nameof(OverlayDialog.BringForward), OverlayDialogLayerChangeType.BringForward)]
    [InlineData(nameof(OverlayDialog.SendBackward), OverlayDialogLayerChangeType.SendBackward)]
    [InlineData(nameof(OverlayDialog.BringToFront), OverlayDialogLayerChangeType.BringToFront)]
    [InlineData(nameof(OverlayDialog.SendToBack), OverlayDialogLayerChangeType.SendToBack)]
    public void LayerMethods_RaiseMatchingChangeType(string methodName, OverlayDialogLayerChangeType expected)
    {
        OverlayDialogLayerChangeType? received = null;
        var dialog = new OverlayDialog();
        dialog.LayerChanged += (_, args) => received = args.ChangeType;

        var method = typeof(OverlayDialog).GetMethod(methodName)!;
        method.Invoke(dialog, null);

        received.Should().Be(expected);
    }
}
