// -----------------------------------------------------------------------
// <copyright file="DialogPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Dialogs;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Humanizer.Facade;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Notifications;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

/// <summary>
/// Dialog playground: overlay and window presentation with shared severity roles and advanced overlay scenarios.
/// </summary>
internal sealed class DialogPageViewModel : ShowcaseViewModel
{
    private readonly INotificationPublisher _notificationPublisher;
    private readonly IContentDialogService _contentDialogService;
    private readonly IMessageBoxFactory _messageBoxFactory;
    private readonly DialogHostOptions _hostOptions;
    private readonly ICommandFactory _commands;

    private static bool _isModal = true;
    private static bool _showCloseButton = true;
    private static bool _canDragMove = true;
    private static bool _canLightDismiss;
    private static bool _fullScreen;
    private static bool _canResize = true;
    private static MessageBoxResultOption _buttons = MessageBoxResultOption.OkCancel;
    private static HorizontalPosition _horizontalAnchor = HorizontalPosition.Center;
    private static VerticalPosition _verticalAnchor = VerticalPosition.Center;

    private IDialog? _lastDialog;

    public DialogPageViewModel(
        INotificationPublisher notificationPublisher,
        IContentDialogService contentDialogService,
        IMessageBoxFactory messageBoxFactory,
        ICommandFactory commands,
        DialogHostOptions hostOptions)
        : base(nameof(Dialogs),
            commands,
            [
                new ControlThemeBuilder("Overlay")
                    .AddRoles(ThemeRole.Information, ThemeRole.Success, ThemeRole.Warning, ThemeRole.Error)
                    .AddValueAction(
                        (_, y) => _isModal = (bool?)y ?? true,
                        true,
                        x => x.DisplayName(nameof(SettingsResources.Modal)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _showCloseButton = (bool?)y ?? true,
                        true,
                        x => x.DisplayName(nameof(SettingsResources.ShowCloseButton)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _canDragMove = (bool?)y ?? true,
                        true,
                        x => x.DisplayName(nameof(SettingsResources.CanDragMove)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _canLightDismiss = (bool?)y ?? false,
                        false,
                        x => x.DisplayName(nameof(SettingsResources.LightDismiss)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _fullScreen = (bool?)y ?? false,
                        false,
                        x => x.DisplayName(nameof(SettingsResources.FullScreen)).Of<ToggleSwitchEditor>())
                    .AddEnumValue<MessageBoxResultOption, ComboBoxEditor>(
                        (_, y) => _buttons = y ?? MessageBoxResultOption.OkCancel,
                        MessageBoxResultOption.OkCancel,
                        x => x.DisplayName(nameof(SettingsResources.Buttons)))
                    .AddValueAction(
                        (_, y) => _horizontalAnchor = (HorizontalPosition?)y ?? HorizontalPosition.Center,
                        HorizontalPosition.Center,
                        x => x.DisplayName(nameof(SettingsResources.HorizontalAnchor))
                            .Of<ListBoxEditor>(editor => editor
                                .AddChoice(HorizontalPosition.Left, b => b.DisplayName(() => HorizontalPosition.Left.Humanize()).WithIcon(MaterialIconKind.FormatHorizontalAlignLeft))
                                .AddChoice(HorizontalPosition.Center, b => b.DisplayName(() => HorizontalPosition.Center.Humanize()).WithIcon(MaterialIconKind.FormatHorizontalAlignCenter))
                                .AddChoice(HorizontalPosition.Right, b => b.DisplayName(() => HorizontalPosition.Right.Humanize()).WithIcon(MaterialIconKind.FormatHorizontalAlignRight))))
                    .AddValueAction(
                        (_, y) => _verticalAnchor = (VerticalPosition?)y ?? VerticalPosition.Center,
                        VerticalPosition.Center,
                        x => x.DisplayName(nameof(SettingsResources.VerticalAnchor))
                            .Of<ListBoxEditor>(editor => editor
                                .AddChoice(VerticalPosition.Top, b => b.DisplayName(() => VerticalPosition.Top.Humanize()).WithIcon(MaterialIconKind.FormatVerticalAlignTop))
                                .AddChoice(VerticalPosition.Center, b => b.DisplayName(() => VerticalPosition.Center.Humanize()).WithIcon(MaterialIconKind.FormatVerticalAlignCenter))
                                .AddChoice(VerticalPosition.Bottom, b => b.DisplayName(() => VerticalPosition.Bottom.Humanize()).WithIcon(MaterialIconKind.FormatVerticalAlignBottom)))),

                new ControlThemeBuilder("Window")
                    .AddRoles(ThemeRole.Information, ThemeRole.Success, ThemeRole.Warning, ThemeRole.Error)
                    .AddValueAction(
                        (_, y) => _isModal = (bool?)y ?? true,
                        true,
                        x => x.DisplayName(nameof(SettingsResources.Modal)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _canResize = (bool?)y ?? true,
                        true,
                        x => x.DisplayName(nameof(SettingsResources.CanResize)).Of<ToggleSwitchEditor>())
                    .AddEnumValue<MessageBoxResultOption, ComboBoxEditor>(
                        (_, y) => _buttons = y ?? MessageBoxResultOption.OkCancel,
                        MessageBoxResultOption.OkCancel,
                        x => x.DisplayName(nameof(SettingsResources.Buttons)))
            ])
    {
        _notificationPublisher = notificationPublisher;
        _contentDialogService = contentDialogService;
        _messageBoxFactory = messageBoxFactory;
        _hostOptions = hostOptions;
        _commands = commands;

        ShowContentDialogCommand = commands.Create(async () => await ShowContentDialogAsync().ConfigureAwait(false));
        ShowMessageBoxCommand = commands.CreateRequired<ThemeRole>(async role => await ShowMessageBoxAsync(ToSeverity(role)).ConfigureAwait(false));
        ShowDialogBoxCommand = commands.CreateRequired<ThemeRole>(async role => await ShowDialogBoxAsync(ToSeverity(role)).ConfigureAwait(false));

        Playground.PropertyChanged += OnPlaygroundPropertyChanged;
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.DockWindow;

    /// <summary>Gets a value indicating whether the overlay theme is selected in the playground.</summary>
    public bool IsOverlayPresentation => Playground.SelectedTheme?.Definition.Key?.Contains("Overlay", StringComparison.OrdinalIgnoreCase) == true;

    public ICommand ShowContentDialogCommand { get; }

    public ICommand ShowMessageBoxCommand { get; }

    public ICommand ShowDialogBoxCommand { get; }

    protected override void DisposeManagedResources()
    {
        Playground.PropertyChanged -= OnPlaygroundPropertyChanged;
        base.DisposeManagedResources();
    }

    private void OnPlaygroundPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PlaygroundViewModel.SelectedTheme))
            NotifyPropertyChanged(nameof(IsOverlayPresentation));
    }

    private async Task ShowContentDialogAsync()
    {
        var vm = new LoginDialogViewModel(_commands, _notificationPublisher) { CanResize = _canResize };
        TrackDialog(vm);

        var result = IsOverlayPresentation
            ? await _contentDialogService
                .ShowAsync(vm, DialogOptionsFactory.ForOverlay(vm, _isModal, CreateOverlayOptions(), OverlayDialogHostManager.MainHostId))
                .ConfigureAwait(false)
            : await _contentDialogService
                .ShowAsync(vm, DialogOptionsFactory.ForWindow(vm, _isModal, windowOptions: CreateWindowOptions(false)))
                .ConfigureAwait(false);

        ClearDialog(vm);
        ShowLoginResult(result);
    }

    private async Task ShowMessageBoxAsync(MessageSeverity severity)
    {
        var messageBox = _messageBoxFactory.Create(MessageBoxOptions.Create(
            GetSampleMessage(severity),
            severity.Humanize(),
            severity,
            _buttons));
        TrackDialog(messageBox);

        var result = IsOverlayPresentation
            ? await _contentDialogService
                .ShowAsync(messageBox, DialogOptionsFactory.ForOverlay(messageBox, _isModal, CreateOverlayOptions(), OverlayDialogHostManager.MainHostId))
                .ConfigureAwait(false)
            : await _contentDialogService
                .ShowAsync(messageBox, DialogOptionsFactory.ForWindow(messageBox, _isModal, windowOptions: CreateWindowOptions(true)))
                .ConfigureAwait(false);

        ClearDialog(messageBox);
        var mapped = result.IsSuccess ? result.Value : MessageBoxResult.Cancel;
        var resultSeverity = result switch
        {
            { IsSuccess: true, Value: MessageBoxResult.Ok or MessageBoxResult.Yes } => NotificationSeverity.Success,
            { IsSuccess: true, Value: MessageBoxResult.Cancel or MessageBoxResult.No } => NotificationSeverity.Warning,
            { IsSuccess: true, Value: MessageBoxResult.None } => NotificationSeverity.Information,
            _ => NotificationSeverity.Error
        };
        _notificationPublisher.Publish(new MessageNotification($"Result: {mapped}", severity: resultSeverity));
    }

    private async Task ShowDialogBoxAsync(MessageSeverity severity)
    {
        var vm = new ConfirmDialogBoxViewModel(_commands, GetSampleMessage(severity), severity.Humanize());
        TrackDialog(vm);

        var result = IsOverlayPresentation
            ? await _contentDialogService
                .ShowAsync(vm, DialogOptionsFactory.ForOverlay(vm, _isModal, CreateOverlayOptions(), OverlayDialogHostManager.MainHostId))
                .ConfigureAwait(false)
            : await _contentDialogService
                .ShowAsync(vm, DialogOptionsFactory.ForWindow(vm, _isModal, windowOptions: CreateWindowOptions(false)))
                .ConfigureAwait(false);

        ClearDialog(vm);
        var message = result.IsSuccess
            ? $"Result: {result.Value}"
            : result.IsCancelled
                ? "Cancelled"
                : "Dismissed";
        var resultSeverity = result switch
        {
            { IsSuccess: true, Value: true } => NotificationSeverity.Success,
            { IsSuccess: true, Value: false } => NotificationSeverity.Warning,
            _ => NotificationSeverity.Error
        };
        _notificationPublisher.Publish(new MessageNotification(message, severity: resultSeverity));
    }

    private OverlayDialogOptions CreateOverlayOptions()
        => new()
        {
            IsCloseButtonVisible = _showCloseButton,
            CanLightDismiss = _canLightDismiss,
            CanDragMove = _canDragMove,
            FullScreen = _fullScreen,
            HorizontalAnchor = _horizontalAnchor,
            VerticalAnchor = _verticalAnchor,
            TopLevelKey = OverlayDialogHostManager.GetTopLevelKey(_hostOptions.TopLevelProvider())
        };

    private static WindowDialogOptions CreateWindowOptions(bool isMessageBox)
        => new()
        {
            CanResize = _canResize && !isMessageBox,
            CanDragMove = true,
            ShowInTaskbar = !isMessageBox
        };

    private void TrackDialog(IDialog dialog) => _lastDialog = dialog;

    private void ClearDialog(IDialog dialog)
    {
        if (ReferenceEquals(_lastDialog, dialog))
            _lastDialog = null;
    }

    private static MessageSeverity ToSeverity(ThemeRole role)
        => role switch
        {
            ThemeRole.Success => MessageSeverity.Success,
            ThemeRole.Warning => MessageSeverity.Warning,
            ThemeRole.Error => MessageSeverity.Error,
            _ => MessageSeverity.Information
        };

    private static string GetSampleMessage(MessageSeverity severity)
        => severity switch
        {
            MessageSeverity.Information => DialogsPageResources.InformationMessage,
            MessageSeverity.Success => DialogsPageResources.SuccessMessage,
            MessageSeverity.Warning => DialogsPageResources.WarningMessage,
            MessageSeverity.Error => DialogsPageResources.ErrorMessage,
            _ => string.Empty
        };

    private void ShowLoginResult(DialogResult<LoginResult> result)
    {
        var message = $"Login: {result.Value?.Login} ; Password: {result.Value?.Password}";
        if (result.IsDismissed)
            _notificationPublisher.Publish(new MessageNotification(message, "No result.", severity: NotificationSeverity.Warning));
        else if (result.IsSuccess)
            _notificationPublisher.Publish(new MessageNotification(message, "Dialog has been validated.", severity: NotificationSeverity.Success));
        else
            _notificationPublisher.Publish(new MessageNotification(message, "Dialog has been cancelled.", severity: NotificationSeverity.Error));
    }
}
