// -----------------------------------------------------------------------
// <copyright file="OverlayMessageBoxHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Headless.XUnit;
using FluentAssertions;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs.Internal;

namespace MyNet.Avalonia.Extended.Headless.Tests.Dialogs;

public class OverlayMessageBoxHeadlessTests
{
    [AvaloniaFact]
    public void PrepareOverlayDialog_AppliesAnchorsAndLightDismiss()
    {
        var overlay = new OverlayMessageBox
        {
            Content = "Confirm",
            Title = "Headless"
        };

        OverlayDialogBuilder.PrepareOverlayDialog(
            overlay,
            new()
            {
                HorizontalAnchor = HorizontalPosition.Left,
                VerticalAnchor = VerticalPosition.Top,
                CanLightDismiss = true
            },
            new() { Title = "Headless" });

        HeadlessControlHost.Show(overlay, new(360, 220));

        overlay.HorizontalAnchor.Should().Be(HorizontalPosition.Left);
        overlay.VerticalAnchor.Should().Be(VerticalPosition.Top);
        overlay.CanLightDismiss.Should().BeTrue();
    }
}
