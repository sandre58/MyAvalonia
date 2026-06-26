// -----------------------------------------------------------------------
// <copyright file="AvaloniaToastHostHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using FluentAssertions;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.UI.Notifications.Models;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Models;
using MyNet.UI.Toasting.Settings;

namespace MyNet.Avalonia.Extended.Headless.Tests.Toasting;

public class AvaloniaToastHostHeadlessTests
{
    [AvaloniaFact]
    public void FirstToast_IsDisplayedAfterWindowAndManagerAreReady()
    {
        Window? window = null;

        var toastManager = new TestToastManager();
        var host = new AvaloniaToastHost(
            () => window,
            toastManager,
            new PassthroughToastContentFactory(),
            new(),
            TimeSpan.FromSeconds(5));

        try
        {
            toastManager.Add(CreateToast("First toast"));

            window = HeadlessControlHost.Show(new Panel(), new(640, 480));
            Dispatcher.UIThread.RunJobs(DispatcherPriority.Background);

            window.GetVisualDescendants()
                .OfType<TextBlock>()
                .Should()
                .ContainSingle(x => x.Text == "First toast");
        }
        finally
        {
            host.Dispose();
        }
    }

    private static Toast CreateToast(string message)
        => new(
            new MessageNotification(message, severity: NotificationSeverity.Information),
            ToastSettings.Default);

    private sealed class TestToastManager : IToastManager
    {
        private readonly ObservableCollection<IToast> _toasts = [];

        public TestToastManager() => Toasts = new(_toasts);

        public ReadOnlyObservableCollection<IToast> Toasts { get; }

        public void Add(IToast toast) => _toasts.Add(toast);

        public void Clear() => _toasts.Clear();

        public void Remove(IToast toast) => _toasts.Remove(toast);

        public void Dispose()
        {
        }
    }

    private sealed class PassthroughToastContentFactory : IAvaloniaToastContentFactory
    {
        public object CreateContent(INotification notification, double? width)
            => new TextBlock { Text = notification is MessageNotification message ? message.Message : notification.ToString() };
    }
}
