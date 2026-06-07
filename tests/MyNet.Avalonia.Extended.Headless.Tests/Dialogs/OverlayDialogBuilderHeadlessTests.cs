// -----------------------------------------------------------------------
// <copyright file="OverlayDialogBuilderHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Headless.XUnit;
using FluentAssertions;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs.Internal;

namespace MyNet.Avalonia.Extended.Headless.Tests.Dialogs;

public class OverlayDialogBuilderHeadlessTests
{
    [AvaloniaFact]
    public void PrepareOverlayDialog_AppliesCanDragMoveToShell()
    {
        var shell = new ContentOverlayDialog();
        var content = new ContentDialog { Header = "Sample" };

        OverlayDialogBuilder.PrepareOverlayDialog(
            shell,
            new() { CanDragMove = false },
            new() { Title = "Sample" });

        shell.Content = content;
        OverlayDialog.GetCanDragMove(shell).Should().BeFalse();
    }
}
