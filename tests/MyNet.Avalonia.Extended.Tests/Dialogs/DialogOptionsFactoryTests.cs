// -----------------------------------------------------------------------
// <copyright file="DialogOptionsFactoryTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs;
using Xunit;

namespace MyNet.Avalonia.Extended.Tests.Dialogs;

public class DialogOptionsFactoryTests
{
    [Fact]
    public void Resolve_ReturnsDefaultRequestWhenOwnerMissing()
    {
        var resolved = DialogOptionsFactory.Resolve(null);
        resolved.Mode.Should().Be(DialogPresentationMode.Overlay);
        resolved.OverlayOptions.Should().BeNull();
    }

    [Fact]
    public void ForOverlay_CarriesHostRequestAndTopLevelKey()
    {
        var dialog = new TestDialogStub();
        var overlayOptions = new OverlayDialogOptions { TopLevelKey = 42, CanLightDismiss = true };

        var options = DialogOptionsFactory.ForOverlay(dialog, isModal: false, overlayOptions, "main");

        options.IsModal.Should().BeFalse();
        options.CloseOnOverlayClick.Should().BeTrue();
        var request = DialogOptionsFactory.Resolve(options);
        request.Mode.Should().Be(DialogPresentationMode.Overlay);
        request.OverlayHostId.Should().Be("main");
        request.OverlayOptions!.TopLevelKey.Should().Be(42);
    }

    [Fact]
    public void ForWindow_CarriesWindowMode()
    {
        var dialog = new TestDialogStub();
        var options = DialogOptionsFactory.ForWindow(dialog, isModal: true);

        DialogOptionsFactory.Resolve(options).Mode.Should().Be(DialogPresentationMode.Window);
    }
}
