// -----------------------------------------------------------------------
// <copyright file="NotificationsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Demo.Resources;
using MyNet.Avalonia.Demo.Views.Samples;
using MyNet.Avalonia.Templates;
using MyNet.Avalonia.UI.Toasting;
using MyNet.Observable;
using MyNet.UI.Notifications;
using MyNet.UI.Toasting.Settings;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class NotificationsPage : AutoBuildPage, IDisposable
{
    private sealed class CustomNotification : ObservableObject, INotification
    {
        public NotificationSeverity Severity => NotificationSeverity.None;

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsSimilar(object? obj) => true;
    }

    private ToasterService? _toasterService;

    static NotificationsPage() => RegisteredDataTemplate.Register<CustomNotification>(_ => new LargeContent1(), nameof(INotification));

    public NotificationsPage()
    {
        InitializeComponent();
        ResetToasterService();
    }

    protected override Control CreateControl(ControlData data)
    {
        var item = new Button
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = data.Color.Or("Default")
        };

        item.Click += (sender, _) =>
        {
            if (sender is not Button { Content: string content })
                return;
            var settings = new ToastSettings
            {
                ClosingStrategy = (ToastClosingStrategy)ClosingStrategy.SelectedValue!,
                FreezeOnMouseEnter = FreezeOnMouseEnter.IsChecked.IsTrue()
            };
            var severity = data.Color switch
            {
                "Positive" => NotificationSeverity.Success,
                "Warning" => NotificationSeverity.Warning,
                "Negative" => NotificationSeverity.Error,
                _ => NotificationSeverity.Information
            };

            var onClick = new Action<INotification>(x => _toasterService?.Show(new MessageNotification(DemoResources.NotificationClickMessage.FormatWith(x), severity: NotificationSeverity.Information), ToastSettings.Default));
            var onClose = new Action(() => _toasterService?.Show(new MessageNotification(DemoResources.NotificationClosedMessage, severity: NotificationSeverity.Success), ToastSettings.Default));
            _toasterService?.Show(new MessageNotification(SentenceGenerator.Paragraph(RandomGenerator.Int(4, 7), RandomGenerator.Int(1, 3)), content, severity), settings, onClick: OnClick.IsChecked.IsTrue() ? onClick : null, onClose: OnClose.IsChecked.IsTrue() ? onClose : null);
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.Hidden)
            .AddStyles("Solid")
            .AddColors(Color.Positive, Color.Negative, Color.Warning, Color.Information)
            .AddCustomControls(() =>
            {
                var item = new Button
                {
                    HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
                    Content = "Custom"
                };

                item.Click += (sender, _) =>
                {
                    if (sender is not Button { Content: string })
                        return;
                    var settings = new ToastSettings
                    {
                        ClosingStrategy = (ToastClosingStrategy)ClosingStrategy.SelectedValue!,
                        FreezeOnMouseEnter = FreezeOnMouseEnter.IsChecked.IsTrue()
                    };

                    var onClick = new Action<INotification>(x => _toasterService?.Show(new MessageNotification(DemoResources.NotificationClickMessage.FormatWith(x), severity: NotificationSeverity.Information), ToastSettings.Default));
                    var onClose = new Action(() => _toasterService?.Show(new MessageNotification(DemoResources.NotificationClosedMessage, severity: NotificationSeverity.Success), ToastSettings.Default));
                    _toasterService?.Show(new CustomNotification(), settings, onClick: OnClick.IsChecked.IsTrue() ? onClick : null, onClose: OnClose.IsChecked.IsTrue() ? onClose : null);
                };

                return [item];
            })
        ];

    private void ResetToasterService()
    {
        _toasterService?.Dispose();
        _toasterService = new ToasterService(() => (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow, new ToasterSettings
        {
            Duration = TimeSpan.FromSeconds((long)Duration.Value!),
            Position = (ToasterPosition)Placement.SelectedValue!,
            MaxItems = (int)MaxItems.Value!,
            OffsetX = OffsetX.Value,
            OffsetY = OffsetY.Value,
            Width = ToastWidth.Value
        });
    }

    private void Placement_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_toasterService is not null)
            ResetToasterService();
    }

    private void Slider_ValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (_toasterService is not null)
            ResetToasterService();
    }

    private void NumericUpDown_ValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (_toasterService is not null)
            ResetToasterService();
    }

    private void Button_Click(object? sender, RoutedEventArgs e) => _toasterService?.Clear();

    public void Dispose() => _toasterService?.Dispose();
}
