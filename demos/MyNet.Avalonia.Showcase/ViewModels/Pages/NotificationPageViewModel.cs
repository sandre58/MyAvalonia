// -----------------------------------------------------------------------
// <copyright file="NotificationPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Material.Icons;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.Views.Samples;
using MyNet.Avalonia.Templates;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Humanizer;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;
using MyNet.UI.Resources;
using MyNet.UI.Toasting.Settings;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class NotificationPageViewModel : ShowcaseViewModel
{
    private static ToasterService? _toasterService;
    private static readonly ToasterSettings ToasterSettings = new();

    private static ToastClosingStrategy _closingStrategy = ToastClosingStrategy.Both;
    private static bool _freezeOnMouseEnter;
    private static bool _enableOnClick;
    private static bool _enableOnClose;

    static NotificationPageViewModel() => RegisteredDataTemplate.Register<CustomNotification>(_ => new LargeContent1(), nameof(INotification));

    public NotificationPageViewModel()
        : base("Notifications",
            [
                new ControlThemeBuilder()
                    .AddRoles(ThemeRole.Success, ThemeRole.Error, ThemeRole.Warning, ThemeRole.Information, ThemeRole.Inverse)
                    .AddAction<Button>(_ => _toasterService?.Clear(), x => x.DisplayName(nameof(UiResources.Clear))
                                                                                                      .WithIcon(MaterialIconKind.CloseCircle)
                                                                                                      .Of<ButtonEditor>(editor => editor.WithRole(ThemeRole.Error)))
                    .AddValueAction(
                        (_, y) =>
                            {
                                ToasterSettings.Position = (ToasterPosition?)y ?? default;
                                ResetToasterService();
                            },
                        ToasterPosition.BottomRight,
                        x => x.DisplayName(nameof(SettingsResources.PopupPlacement))
                                                        .Of<ListBoxEditor>(editor => editor.AddChoice(ToasterPosition.BottomRight, builder => builder.DisplayName(() => ToasterPosition.BottomRight.Humanize()).WithIcon(MaterialIconKind.PanBottomRight))
                                                                                           .AddChoice(ToasterPosition.BottomCenter, builder => builder.DisplayName(() => ToasterPosition.BottomCenter.Humanize()).WithIcon(MaterialIconKind.PanDown))
                                                                                           .AddChoice(ToasterPosition.BottomLeft, builder => builder.DisplayName(() => ToasterPosition.BottomLeft.Humanize()).WithIcon(MaterialIconKind.PanBottomLeft))
                                                                                           .AddChoice(ToasterPosition.TopLeft, builder => builder.DisplayName(() => ToasterPosition.TopLeft.Humanize()).WithIcon(MaterialIconKind.PanTopLeft))
                                                                                           .AddChoice(ToasterPosition.TopCenter, builder => builder.DisplayName(() => ToasterPosition.TopCenter.Humanize()).WithIcon(MaterialIconKind.PanUp))
                                                                                           .AddChoice(ToasterPosition.TopRight, builder => builder.DisplayName(() => ToasterPosition.TopRight.Humanize()).WithIcon(MaterialIconKind.PanTopRight))))
                    .AddValueAction(
                        (_, y) =>
                            {
                                ToasterSettings.Duration = TimeSpan.FromSeconds((double)(y ?? 3.5));
                                ResetToasterService();
                            },
                        3.5d,
                        x => x.DisplayName(nameof(SettingsResources.DisplayDuration)).Of<SliderEditor>(editor => editor.WithRange(0, 60).WithIncrement(0.1M)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            ToasterSettings.MaxItems = Convert.ToInt32((decimal)(y ?? 30));
                            ResetToasterService();
                        },
                        30.0M,
                        x => x.DisplayName(nameof(SettingsResources.MaxItems)).Of<NumericUpDownEditor>(editor => editor.WithRange(0.0M, 30.0M)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            ToasterSettings.Width = (double)(y ?? 300);
                            ResetToasterService();
                        },
                        300,
                        x => x.DisplayName(nameof(SettingsResources.ToastWidth)).Of<IntSliderEditor>(editor => editor.WithRange(0, 800)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            ToasterSettings.OffsetX = (double)(y ?? 10);
                            ResetToasterService();
                        },
                        10,
                        x => x.DisplayName(nameof(SettingsResources.OffsetX)).Of<IntSliderEditor>(editor => editor.WithRange(0, 300)))
                    .AddValueAction(
                        (_, y) =>
                        {
                            ToasterSettings.OffsetY = (double)(y ?? 10);
                            ResetToasterService();
                        },
                        10,
                        x => x.DisplayName(nameof(SettingsResources.OffsetY)).Of<IntSliderEditor>(editor => editor.WithRange(0, 300)))
                    .AddValueAction(
                        (_, y) => _closingStrategy = (ToastClosingStrategy?)y ?? ToastClosingStrategy.Both,
                        ToastClosingStrategy.Both,
                        x => x.DisplayName(nameof(SettingsResources.ClosingStrategy))
                            .Of<ListBoxEditor>(editor => editor.AddChoice(ToastClosingStrategy.None, builder => builder.DisplayName(() => ToastClosingStrategy.None.Humanize()).WithIcon(MaterialIconKind.CircleOffOutline))
                                                               .AddChoice(ToastClosingStrategy.AutoClose, builder => builder.DisplayName(() => ToastClosingStrategy.AutoClose.Humanize()).WithIcon(MaterialIconKind.ProgressClose))
                                                               .AddChoice(ToastClosingStrategy.CloseButton, builder => builder.DisplayName(() => ToastClosingStrategy.CloseButton.Humanize()).WithIcon(MaterialIconKind.CloseBox))
                                                               .AddChoice(ToastClosingStrategy.Both, builder => builder.DisplayName(() => ToastClosingStrategy.CloseButton.Humanize()).WithIcon(MaterialIconKind.CloseBoxMultiple))))
                    .AddValueAction(
                        (_, y) => _freezeOnMouseEnter = (bool?)y ?? false,
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
        ResetToasterService();
        ShowNotificationCommand = CommandsManager.CreateNotNull<ThemeRole>(ShowNotification);
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.MessageAlert;

    public ICommand ShowNotificationCommand { get; }

    public static void ShowNotification(ThemeRole role) => ShowNotification(CreateNotificationFromRole(role));

    public static void ShowNotification(INotification notification)
    {
        var settings = new ToastSettings
        {
            ClosingStrategy = _closingStrategy,
            FreezeOnMouseEnter = _freezeOnMouseEnter
        };

        var onClick = new Action<INotification>(x => _toasterService?.Show(new MessageNotification(NotificationPageResources.NotificationClickMessage.FormatWith(x), severity: NotificationSeverity.Information), ToastSettings.Default));
        var onClose = new Action(() => _toasterService?.Show(new MessageNotification(NotificationPageResources.NotificationClosedMessage, severity: NotificationSeverity.Success), ToastSettings.Default));
        _toasterService?.Show(notification, settings, onClick: _enableOnClick ? onClick : null, onClose: _enableOnClose ? onClose : null);
    }

    private static INotification CreateNotificationFromRole(ThemeRole role)
    {
        if (role == ThemeRole.Inverse) return new CustomNotification();

        var severity = role switch
        {
            ThemeRole.Success => NotificationSeverity.Success,
            ThemeRole.Warning => NotificationSeverity.Warning,
            ThemeRole.Error => NotificationSeverity.Error,
            _ => NotificationSeverity.Information
        };

        return new MessageNotification(SentenceGenerator.Paragraph(RandomGenerator.Int(4, 7), RandomGenerator.Int(1, 3)), role.ToString(), severity);
    }

    private static void ResetToasterService()
    {
        _toasterService?.Dispose();
        _toasterService = new(() => (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow, ToasterSettings);
    }

    protected override void OnPropertyIsModified(string propertyName, object before, object after)
    {
        base.OnPropertyIsModified(propertyName, before, after);

        ResetToasterService();
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        _toasterService?.Dispose();
    }

    private sealed class CustomNotification : ObservableObject, INotification
    {
        public NotificationSeverity Severity => NotificationSeverity.None;

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsSimilar(object? obj) => true;
    }
}
