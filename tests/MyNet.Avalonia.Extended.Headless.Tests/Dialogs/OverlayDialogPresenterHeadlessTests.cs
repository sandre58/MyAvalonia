// -----------------------------------------------------------------------
// <copyright file="OverlayDialogPresenterHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using FluentAssertions;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs;
using MyNet.Avalonia.Extended.Dialogs.Internal;
using MyNet.Avalonia.Extended.Dialogs.Presentation;
using MyNet.Avalonia.Extended.Schedulers;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Threading;

namespace MyNet.Avalonia.Extended.Headless.Tests.Dialogs;

public class OverlayDialogPresenterHeadlessTests
{
    [AvaloniaFact]
    public async Task PresentAsync_ModalContentDialog_ReturnsOkWhenClosedWithTrueAsync()
    {
        var host = new OverlayDialogHost
        {
            Width = 400,
            Height = 300,
            HostId = "content-test"
        };
        var owner = HeadlessControlHost.Show(host, new(400, 300));
        var topLevelKey = OverlayDialogHostManager.GetTopLevelKey(owner);

        var dialog = new TestDialogStub();
        var registry = new DialogSessionRegistry();
        var hostOptions = new DialogHostOptions(() => owner);
        var presenter = new OverlayDialogPresenter(
            hostOptions,
            new PassthroughViewFactory(new ContentDialog { Header = "Edit", ShowCloseButton = true }),
            registry,
            new AvaloniaUiThreadDispatcher());
        var uiOptions = DialogOptionsFactory.ForOverlay(
            dialog,
            isModal: true,
            new() { TopLevelKey = topLevelKey },
            "content-test");

        var presentTask = presenter.PresentAsync(dialog, uiOptions, CancellationToken.None);

        var shell = host.GetVisualDescendants().OfType<OverlayContentDialog>().Single();
        var content = host.GetVisualDescendants().OfType<ContentDialog>().Single();
        HeaderAssist.GetIsVisible(content).Should().BeFalse();
        await Dispatcher.UIThread.InvokeAsync(() => shell.CloseWithResult(true));

        var result = await presentTask.ConfigureAwait(true);
        result.IsSuccess.Should().BeTrue();
    }
}
