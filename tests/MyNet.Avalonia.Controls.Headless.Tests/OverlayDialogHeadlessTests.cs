// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using FluentAssertions;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class OverlayDialogHeadlessTests
{
    [AvaloniaFact]
    public void GetTopLevelKey_ReturnsStableValueForSameWindow()
    {
        var window = new Window();
        window.Show();

        var first = OverlayDialogHostManager.GetTopLevelKey(window);
        var second = OverlayDialogHostManager.GetTopLevelKey(window);

        first.Should().NotBeNull();
        first.Should().Be(second);
    }

    [AvaloniaFact]
    public void GetTopLevelKey_ReturnsDistinctValuesForDifferentWindows()
    {
        var windowA = new Window();
        var windowB = new Window();
        windowA.Show();
        windowB.Show();

        var keyA = OverlayDialogHostManager.GetTopLevelKey(windowA);
        var keyB = OverlayDialogHostManager.GetTopLevelKey(windowB);

        keyA.Should().NotBeNull();
        keyB.Should().NotBeNull();
        keyA.Should().NotBe(keyB);
    }

    [AvaloniaFact]
    public void ApplyTemplate_ShowsTitle()
    {
        var dialog = new OverlayDialog
        {
            Title = "Settings",
            Content = new TextBlock { Text = "Body" }
        };

        HeadlessControlHost.Show(dialog, new(320, 200));

        dialog.GetVisualDescendants().OfType<TextBlock>()
            .Select(t => t.Text)
            .Should().Contain("Settings");
    }

    [AvaloniaFact]
    public void IsCloseButtonVisible_HidesCloseButton()
    {
        var dialog = new OverlayDialog
        {
            Title = "Hidden close",
            IsCloseButtonVisible = false,
            Content = new TextBlock { Text = "Body" }
        };

        HeadlessControlHost.Show(dialog, new(320, 200));

        var closeButton = HeadlessControlHost.FindByName<Button>(dialog, OverlayDialog.PartCloseButton);
        closeButton.Should().NotBeNull();
        closeButton.IsVisible.Should().BeFalse();
    }

    [AvaloniaFact]
    public void ModalDialog_Close_RemovesFromHost()
    {
        var host = new OverlayDialogHost
        {
            Width = 400,
            Height = 300,
            HostId = "headless"
        };
        var surface = new Grid { Children = { host } };
        HeadlessControlHost.Show(surface, new(400, 300));

        var dialog = new OverlayDialog
        {
            Title = "Modal",
            Content = new TextBlock { Text = "Content" }
        };

        host.AddModalDialog(dialog);
        host.Children.Should().Contain(dialog);

        dialog.Close();

        host.Children.Should().NotContain(dialog);
    }

    [AvaloniaFact]
    public void GetHost_ReturnsRegisteredHostByIdAndTopLevelKey()
    {
        var host = new OverlayDialogHost
        {
            Width = 400,
            Height = 300,
            HostId = "registered"
        };
        var window = HeadlessControlHost.Show(host, new(400, 300));
        var topLevelKey = OverlayDialogHostManager.GetTopLevelKey(window);

        var resolved = OverlayDialogHostManager.GetHost("registered", topLevelKey);

        resolved.Should().BeSameAs(host);
    }

    [AvaloniaFact]
    public void LightDismiss_Close_RemovesNonModalDialog()
    {
        var host = new OverlayDialogHost
        {
            Width = 400,
            Height = 300
        };
        var surface = new Grid { Children = { host } };
        HeadlessControlHost.Show(surface, new(400, 300));

        var dialog = new OverlayDialog
        {
            CanLightDismiss = true,
            Content = new TextBlock { Text = "Dismiss me" }
        };

        host.AddDialog(dialog);
        host.Children.Should().Contain(dialog);

        dialog.Close();

        host.Children.Should().NotContain(dialog);
    }

    [AvaloniaFact]
    public void Recall_ReturnsContentAssignableToRequestedType()
    {
        var host = new OverlayDialogHost { Width = 400, Height = 300 };
        HeadlessControlHost.Show(new Grid { Children = { host } }, new(400, 300));

        var dialog = new OverlayDialog { Content = new RecallDerivedContent { Value = 42 } };
        host.AddModalDialog(dialog);

        host.Recall<RecallContentBase>()!.Value.Should().Be(42);
    }

    [AvaloniaFact]
    public void BringToFront_RaisesTopDialogAboveOthers()
    {
        var host = new OverlayDialogHost { Width = 400, Height = 300 };
        HeadlessControlHost.Show(new Grid { Children = { host } }, new(400, 300));

        var bottom = new OverlayDialog { Title = "Bottom", Content = new TextBlock() };
        var top = new OverlayDialog { Title = "Top", Content = new TextBlock() };
        host.AddModalDialog(bottom);
        host.AddModalDialog(top);

        bottom.BringToFront();

        bottom.ZIndex.Should().BeGreaterThan(top.ZIndex);
    }

    private class RecallContentBase
    {
        public int Value { get; init; }
    }

    private sealed class RecallDerivedContent : RecallContentBase;
}
