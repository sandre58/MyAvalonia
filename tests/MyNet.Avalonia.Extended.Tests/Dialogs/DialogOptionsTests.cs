// -----------------------------------------------------------------------
// <copyright file="DialogOptionsTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs;
using Xunit;
using AvaloniaDialogOptions = MyNet.Avalonia.Extended.Dialogs.DialogOptions;

namespace MyNet.Avalonia.Extended.Tests.Dialogs;

public class DialogOptionsTests
{
    [Fact]
    public void Resolve_ReturnsDefaultRequestWhenOwnerMissing()
    {
        var resolved = AvaloniaDialogOptions.Resolve(null);
        resolved.Mode.Should().Be(DialogPresentationMode.Overlay);
        resolved.OverlayOptions.Should().BeNull();
    }

    [Fact]
    public void ForOverlay_CarriesHostRequestAndTopLevelKey()
    {
        var dialog = new TestDialogStub();
        var overlayOptions = new OverlayDialogOptions { TopLevelKey = 42, CanLightDismiss = true };

        var options = AvaloniaDialogOptions.ForOverlay(dialog, isModal: false, overlayOptions, "main");

        options.IsModal.Should().BeFalse();
        options.CloseOnOverlayClick.Should().BeTrue();
        var request = AvaloniaDialogOptions.Resolve(options);
        request.Mode.Should().Be(DialogPresentationMode.Overlay);
        request.OverlayHostId.Should().Be("main");
        request.OverlayOptions!.TopLevelKey.Should().Be(42);
    }

    [Fact]
    public void ForWindow_CarriesWindowMode()
    {
        var dialog = new TestDialogStub();
        var options = AvaloniaDialogOptions.ForWindow(dialog, isModal: true);

        AvaloniaDialogOptions.Resolve(options).Mode.Should().Be(DialogPresentationMode.Window);
    }
}
