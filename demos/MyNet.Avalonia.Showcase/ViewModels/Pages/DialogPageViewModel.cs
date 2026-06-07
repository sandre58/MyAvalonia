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
using DialogOptions = MyNet.Avalonia.Extended.Dialogs.DialogOptions;

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
                        x => x.DisplayName("Modal").Of<ToggleSwitchEditor>())
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
                        x => x.DisplayName("LightDismiss").Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _fullScreen = (bool?)y ?? false,
                        false,
                        x => x.DisplayName(nameof(SettingsResources.FullScreen)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _buttons = (MessageBoxResultOption?)y ?? MessageBoxResultOption.OkCancel,
                        MessageBoxResultOption.OkCancel,
                        x => x.DisplayName("Buttons")
                            .Of<ListBoxEditor>(editor => editor
                                .AddChoice(MessageBoxResultOption.Ok, b => b.DisplayName(() => MessageBoxResultOption.Ok.Humanize()))
                                .AddChoice(MessageBoxResultOption.OkCancel, b => b.DisplayName(() => MessageBoxResultOption.OkCancel.Humanize()))
                                .AddChoice(MessageBoxResultOption.YesNo, b => b.DisplayName(() => MessageBoxResultOption.YesNo.Humanize()))
                                .AddChoice(MessageBoxResultOption.YesNoCancel, b => b.DisplayName(() => MessageBoxResultOption.YesNoCancel.Humanize()))))
                    .AddValueAction(
                        (_, y) => _horizontalAnchor = (HorizontalPosition?)y ?? HorizontalPosition.Center,
                        HorizontalPosition.Center,
                        x => x.DisplayName("HorizontalAnchor")
                            .Of<ListBoxEditor>(editor => editor
                                .AddChoice(HorizontalPosition.Left, b => b.DisplayName(() => HorizontalPosition.Left.Humanize()).WithIcon(MaterialIconKind.FormatHorizontalAlignLeft))
                                .AddChoice(HorizontalPosition.Center, b => b.DisplayName(() => HorizontalPosition.Center.Humanize()).WithIcon(MaterialIconKind.FormatHorizontalAlignCenter))
                                .AddChoice(HorizontalPosition.Right, b => b.DisplayName(() => HorizontalPosition.Right.Humanize()).WithIcon(MaterialIconKind.FormatHorizontalAlignRight))))
                    .AddValueAction(
                        (_, y) => _verticalAnchor = (VerticalPosition?)y ?? VerticalPosition.Center,
                        VerticalPosition.Center,
                        x => x.DisplayName("VerticalAnchor")
                            .Of<ListBoxEditor>(editor => editor
                                .AddChoice(VerticalPosition.Top, b => b.DisplayName(() => VerticalPosition.Top.Humanize()).WithIcon(MaterialIconKind.FormatVerticalAlignTop))
                                .AddChoice(VerticalPosition.Center, b => b.DisplayName(() => VerticalPosition.Center.Humanize()).WithIcon(MaterialIconKind.FormatVerticalAlignCenter))
                                .AddChoice(VerticalPosition.Bottom, b => b.DisplayName(() => VerticalPosition.Bottom.Humanize()).WithIcon(MaterialIconKind.FormatVerticalAlignBottom)))),

                new ControlThemeBuilder("Window")
                    .AddRoles(ThemeRole.Information, ThemeRole.Success, ThemeRole.Warning, ThemeRole.Error)
                    .AddValueAction(
                        (_, y) => _isModal = (bool?)y ?? true,
                        true,
                        x => x.DisplayName("Modal").Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _canResize = (bool?)y ?? true,
                        true,
                        x => x.DisplayName(nameof(SettingsResources.CanResize)).Of<ToggleSwitchEditor>())
                    .AddValueAction(
                        (_, y) => _buttons = (MessageBoxResultOption?)y ?? MessageBoxResultOption.OkCancel,
                        MessageBoxResultOption.OkCancel,
                        x => x.DisplayName("Buttons")
                            .Of<ListBoxEditor>(editor => editor
                                .AddChoice(MessageBoxResultOption.Ok, b => b.DisplayName(() => MessageBoxResultOption.Ok.Humanize()))
                                .AddChoice(MessageBoxResultOption.OkCancel, b => b.DisplayName(() => MessageBoxResultOption.OkCancel.Humanize()))
                                .AddChoice(MessageBoxResultOption.YesNo, b => b.DisplayName(() => MessageBoxResultOption.YesNo.Humanize()))
                                .AddChoice(MessageBoxResultOption.YesNoCancel, b => b.DisplayName(() => MessageBoxResultOption.YesNoCancel.Humanize()))))
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
        ShowStackedOverlayCommand = commands.Create(async () => await ShowStackedOverlayAsync().ConfigureAwait(false));
        CloseTopDialogCommand = commands.Create(async () => await CloseTopDialogAsync().ConfigureAwait(false));

        Playground.PropertyChanged += OnPlaygroundPropertyChanged;
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.DockWindow;

    /// <summary>Gets a value indicating whether the overlay theme is selected in the playground.</summary>
    public bool IsOverlayPresentation =>
        Playground.SelectedTheme?.Definition.Key?.Contains("Overlay", StringComparison.OrdinalIgnoreCase) == true;

    public ICommand ShowContentDialogCommand { get; }

    public ICommand ShowMessageBoxCommand { get; }

    public ICommand ShowDialogBoxCommand { get; }

    public ICommand ShowStackedOverlayCommand { get; }

    public ICommand CloseTopDialogCommand { get; }

    protected override void DisposeManagedResources()
    {
        Playground.PropertyChanged -= OnPlaygroundPropertyChanged;
        base.DisposeManagedResources();
    }

    private void OnPlaygroundPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PlaygroundViewModel.SelectedTheme))
            OnPropertyChanged(nameof(IsOverlayPresentation), null, IsOverlayPresentation);
    }

    private async Task ShowContentDialogAsync()
    {
        var vm = new LoginDialogViewModel(_commands) { CanResize = _canResize };
        TrackDialog(vm);

        var result = IsOverlayPresentation
            ? await _contentDialogService
                .ShowAsync(vm, DialogOptions.ForOverlay(vm, _isModal, CreateOverlayOptions(), OverlayDialogHostManager.MainHostId))
                .ConfigureAwait(false)
            : await _contentDialogService
                .ShowAsync(vm, DialogOptions.ForWindow(vm, _isModal))
                .ConfigureAwait(false);

        ClearDialog(vm);
        ShowLoginResult(result, vm);
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
                .ShowAsync(messageBox, DialogOptions.ForOverlay(messageBox, _isModal, CreateOverlayOptions(), OverlayDialogHostManager.MainHostId))
                .ConfigureAwait(false)
            : await _contentDialogService
                .ShowAsync(messageBox, DialogOptions.ForWindow(messageBox, _isModal))
                .ConfigureAwait(false);

        ClearDialog(messageBox);
        var mapped = result.IsSuccess ? result.Value : MessageBoxResult.Cancel;
        _notificationPublisher.Publish(new MessageNotification($"Result: {mapped}", severity: NotificationSeverity.Information));
    }

    private async Task ShowDialogBoxAsync(MessageSeverity severity)
    {
        var vm = new ConfirmDialogBoxViewModel(_commands, GetSampleMessage(severity), severity.Humanize());
        TrackDialog(vm);

        var result = IsOverlayPresentation
            ? await _contentDialogService
                .ShowAsync<bool>(vm, DialogOptions.ForOverlay(vm, _isModal, CreateOverlayOptions(), OverlayDialogHostManager.MainHostId))
                .ConfigureAwait(false)
            : await _contentDialogService
                .ShowAsync<bool>(vm, DialogOptions.ForWindow(vm, _isModal))
                .ConfigureAwait(false);

        ClearDialog(vm);
        var message = result.IsSuccess
            ? $"Result: {result.Value}"
            : result.IsCancelled ? "Cancelled" : "Dismissed";
        _notificationPublisher.Publish(new MessageNotification(message, severity: NotificationSeverity.Information));
    }

    private async Task ShowStackedOverlayAsync()
    {
        if (!IsOverlayPresentation)
        {
            _notificationPublisher.Publish(new MessageNotification(
                DialogsPageResources.StackedOverlayRequiresOverlayTheme,
                severity: NotificationSeverity.Warning));
            return;
        }

        var first = new ConfirmDialogBoxViewModel(_commands, DialogsPageResources.StackedOverlayFirstMessage, DialogsPageResources.StackedOverlayFirstTitle);
        TrackDialog(first);
        var firstTask = _contentDialogService.ShowAsync<bool>(
            first,
            DialogOptions.ForOverlay(first, _isModal, CreateOverlayOptions(), OverlayDialogHostManager.MainHostId));

        await Task.Delay(150).ConfigureAwait(false);

        var second = new ConfirmDialogBoxViewModel(_commands, DialogsPageResources.StackedOverlaySecondMessage, DialogsPageResources.StackedOverlaySecondTitle);
        TrackDialog(second);
        var secondResult = await _contentDialogService
            .ShowAsync<bool>(second, DialogOptions.ForOverlay(second, _isModal, CreateOverlayOptions(), OverlayDialogHostManager.MainHostId))
            .ConfigureAwait(false);

        var firstResult = await firstTask.ConfigureAwait(false);
        ClearDialog(second);
        ClearDialog(first);

        _notificationPublisher.Publish(new MessageNotification(
            string.Format(DialogsPageResources.StackedOverlayResultFormat, secondResult.IsSuccess, firstResult.IsSuccess),
            severity: NotificationSeverity.Information));
    }

    private async Task CloseTopDialogAsync()
    {
        if (!IsOverlayPresentation)
        {
            _notificationPublisher.Publish(new MessageNotification(
                DialogsPageResources.CloseTopRequiresOverlayTheme,
                severity: NotificationSeverity.Warning));
            return;
        }

        if (_lastDialog is null)
        {
            _notificationPublisher.Publish(new MessageNotification(
                DialogsPageResources.NoOpenDialog,
                severity: NotificationSeverity.Warning));
            return;
        }

        await _contentDialogService.CloseAsync(_lastDialog).ConfigureAwait(false);
        _notificationPublisher.Publish(new MessageNotification(
            DialogsPageResources.DialogClosedProgrammatically,
            severity: NotificationSeverity.Success));
    }

    private OverlayDialogOptions CreateOverlayOptions() => new()
    {
        IsCloseButtonVisible = _showCloseButton,
        CanLightDismiss = _canLightDismiss,
        CanDragMove = _canDragMove,
        FullScreen = _fullScreen,
        HorizontalAnchor = _horizontalAnchor,
        VerticalAnchor = _verticalAnchor,
        TopLevelKey = OverlayDialogHostManager.GetTopLevelKey(_hostOptions.TopLevelProvider())
    };

    private void TrackDialog(IDialog dialog) => _lastDialog = dialog;

    private void ClearDialog(IDialog dialog)
    {
        if (ReferenceEquals(_lastDialog, dialog))
            _lastDialog = null;
    }

    private static MessageSeverity ToSeverity(ThemeRole role) => role switch
    {
        ThemeRole.Success => MessageSeverity.Success,
        ThemeRole.Warning => MessageSeverity.Warning,
        ThemeRole.Error => MessageSeverity.Error,
        _ => MessageSeverity.Information
    };

    private static string GetSampleMessage(MessageSeverity severity) => severity switch
    {
        MessageSeverity.Information => "This is an informational message.",
        MessageSeverity.Success => "Operation completed successfully!",
        MessageSeverity.Warning => "Are you sure you want to continue? This action may have consequences.",
        MessageSeverity.Error => "An unexpected error has occurred. Please try again.",
        _ => "This is a dialog message."
    };

    private void ShowLoginResult(DialogResult<bool> result, LoginDialogViewModel viewModel)
    {
        if (result.IsDismissed)
            _notificationPublisher.Publish(new MessageNotification("No result.", severity: NotificationSeverity.Warning));
        else if (result.IsSuccess)
            _notificationPublisher.Publish(new MessageNotification("Dialog has been validated.", severity: NotificationSeverity.Success));
        else
            _notificationPublisher.Publish(new MessageNotification("Dialog has been cancelled.", severity: NotificationSeverity.Error));

        _notificationPublisher.Publish(new MessageNotification(
            $"Login: {viewModel.Form.Login} ; Password: {viewModel.Form.Password}",
            severity: NotificationSeverity.Information));
    }
}
