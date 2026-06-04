// -----------------------------------------------------------------------
// <copyright file="NotificationPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows.Input;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.Avalonia.Extended.Toasting.Settings;
using MyNet.Avalonia.Showcase.Notifications;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Fakers.Static;
using MyNet.Generator.Facade;
using MyNet.Humanizer.Facade;
using MyNet.Primitives;
using MyNet.UI;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;
using MyNet.UI.Notifications.Models;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class NotificationPageViewModel : ShowcaseViewModel
{
    private readonly INotificationPublisher _notificationPublisher;

    private static bool _enableOnClick;
    private static bool _enableOnClose;

    public NotificationPageViewModel(
        INotificationPublisher notificationPublisher,
        IToastManager toastManager,
        AvaloniaToastHost toastHost,
        AvaloniaToastHostOptions hostOptions,
        ToastManagerOptions managerOptions,
        ICommandFactory commands)
        : base("Notifications",
            commands,
            [
                new ControlThemeBuilder()
                    .AddRoles(ThemeRole.Success, ThemeRole.Error, ThemeRole.Warning, ThemeRole.Information, ThemeRole.Inverse)
                    .AddAction<Button>(_ => toastManager.Clear(), x => x.DisplayName(nameof(UiResources.Clear))
                                                                                                      .WithIcon(MaterialIconKind.CloseCircle)
                                                                                                      .Of<ButtonEditor>(editor => editor.WithRole(ThemeRole.Error)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            hostOptions.Position = (AvaloniaToastPosition?)y ?? AvaloniaToastPosition.BottomRight;
                            toastHost.RefreshLayout();
                        },
                        AvaloniaToastPosition.BottomRight,
                        x => x.DisplayName(nameof(SettingsResources.PopupPlacement))
                            .Of<ListBoxEditor>(editor => editor.AddChoice(AvaloniaToastPosition.BottomRight, builder => builder.DisplayName(() => AvaloniaToastPosition.BottomRight.Humanize()).WithIcon(MaterialIconKind.PanBottomRight))
                                .AddChoice(AvaloniaToastPosition.BottomCenter, builder => builder.DisplayName(() => AvaloniaToastPosition.BottomCenter.Humanize()).WithIcon(MaterialIconKind.PanDown))
                                .AddChoice(AvaloniaToastPosition.BottomLeft, builder => builder.DisplayName(() => AvaloniaToastPosition.BottomLeft.Humanize()).WithIcon(MaterialIconKind.PanBottomLeft))
                                .AddChoice(AvaloniaToastPosition.TopLeft, builder => builder.DisplayName(() => AvaloniaToastPosition.TopLeft.Humanize()).WithIcon(MaterialIconKind.PanTopLeft))
                                .AddChoice(AvaloniaToastPosition.TopCenter, builder => builder.DisplayName(() => AvaloniaToastPosition.TopCenter.Humanize()).WithIcon(MaterialIconKind.PanUp))
                                .AddChoice(AvaloniaToastPosition.TopRight, builder => builder.DisplayName(() => AvaloniaToastPosition.TopRight.Humanize()).WithIcon(MaterialIconKind.PanTopRight))))
                    .AddValueAction(
                        (_, y) => managerOptions.DefaultDuration = TimeSpan.FromSeconds((double)(y ?? 3.5)),
                        3.5d,
                        x => x.DisplayName(nameof(SettingsResources.DisplayDuration)).Of<SliderEditor>(editor => editor.WithRange(0, 60).WithIncrement(0.1M)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            var maxItems = Convert.ToInt32((decimal)(y ?? 30));
                            hostOptions.MaxItems = maxItems;
                            managerOptions.MaxVisibleToasts = maxItems;
                            toastHost.RefreshLayout();
                        },
                        30.0M,
                        x => x.DisplayName(nameof(SettingsResources.MaxItems)).Of<NumericUpDownEditor>(editor => editor.WithRange(0.0M, 30.0M)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            hostOptions.Width = (double)(y ?? 300);
                            toastHost.RefreshLayout();
                        },
                        300,
                        x => x.DisplayName(nameof(SettingsResources.ToastWidth)).Of<IntSliderEditor>(editor => editor.WithRange(0, 800)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            hostOptions.OffsetX = (double)(y ?? 10);
                            toastHost.RefreshLayout();
                        },
                        10,
                        x => x.DisplayName(nameof(SettingsResources.OffsetX)).Of<IntSliderEditor>(editor => editor.WithRange(0, 300)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            hostOptions.OffsetY = (double)(y ?? 10);
                            toastHost.RefreshLayout();
                        },
                        10,
                        x => x.DisplayName(nameof(SettingsResources.OffsetY)).Of<IntSliderEditor>(editor => editor.WithRange(0, 300)))
                    .AddValueAction(
                        (_, _) =>
                        {
                        },
                        ToastClosingStrategy.Both,
                        x => x.DisplayName(nameof(SettingsResources.ClosingStrategy))
                            .Of<ListBoxEditor>(editor => editor.AddChoice(ToastClosingStrategy.None, builder => builder.DisplayName(() => ToastClosingStrategy.None.Humanize()).WithIcon(MaterialIconKind.CircleOffOutline))
                                .AddChoice(ToastClosingStrategy.AutoClose, builder => builder.DisplayName(() => ToastClosingStrategy.AutoClose.Humanize()).WithIcon(MaterialIconKind.ProgressClose))
                                .AddChoice(ToastClosingStrategy.CloseButton, builder => builder.DisplayName(() => ToastClosingStrategy.CloseButton.Humanize()).WithIcon(MaterialIconKind.CloseBox))
                                .AddChoice(ToastClosingStrategy.Both, builder => builder.DisplayName(() => ToastClosingStrategy.CloseButton.Humanize()).WithIcon(MaterialIconKind.CloseBoxMultiple))))
                    .AddValueAction(
                        (_, _) =>
                        {
                        },
                        false,
                        x => x.DisplayName(nameof(SettingsResources.FreezeOnMouseEnter)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _enableOnClick = (bool?)y ?? false,
                        false,
                        x => x.DisplayName(nameof(SettingsResources.ActivateOnClick)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _enableOnClose = (bool?)y ?? false,
                        false,
                        x => x.DisplayName(nameof(SettingsResources.ActivateOnClose)).Of<ToggleSwitchEditor>())
            ])
    {
        _notificationPublisher = notificationPublisher;

        ShowNotificationCommand = commands.CreateRequired<ThemeRole>(ShowNotification);
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.MessageAlert;

    public ICommand ShowNotificationCommand { get; }

    private void ShowNotification(ThemeRole role) => PublishNotification(CreateNotificationFromRole(role));

    private void PublishNotification(INotification notification)
    {
        if (notification is MessageNotification message)
        {
            if (_enableOnClick)
            {
                _notificationPublisher.Publish(new ActionNotification(
                    message.Message,
                    message.Title,
                    message.Severity,
                    action: x => _notificationPublisher.Publish(new MessageNotification(
                        NotificationPageResources.NotificationClickMessage.FormatWith(CultureInfo.CurrentCulture, x),
                        severity: NotificationSeverity.Information))));
                return;
            }

            if (_enableOnClose)
            {
                var closable = new ClosableNotification(message.Message, message.Title, message.Severity);
                closable.CloseRequested += OnDemoCloseRequested;
                _notificationPublisher.Publish(closable);
                return;
            }
        }

        _notificationPublisher.Publish(notification);
    }

    private void OnDemoCloseRequested(object? sender, CloseRequestedEventArgs e)
    {
        if (sender is ClosableNotification closable)
            closable.CloseRequested -= OnDemoCloseRequested;

        _notificationPublisher.Publish(new MessageNotification(
            NotificationPageResources.NotificationClosedMessage,
            severity: NotificationSeverity.Success));
    }

    private static INotification CreateNotificationFromRole(ThemeRole role)
    {
        if (role == ThemeRole.Inverse)
            return new ShowcaseCustomNotification();

        var severity = role switch
        {
            ThemeRole.Success => NotificationSeverity.Success,
            ThemeRole.Warning => NotificationSeverity.Warning,
            ThemeRole.Error => NotificationSeverity.Error,
            _ => NotificationSeverity.Information
        };

        return new MessageNotification(
            Faker.Texts.Paragraph(RandomGenerator.Current.Int(4, 7), RandomGenerator.Current.Int(1, 3)),
            role.ToString(),
            severity);
    }
}
