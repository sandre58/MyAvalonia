// -----------------------------------------------------------------------
// <copyright file="OverlayDialogGroupViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Dialogs;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Notifications;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

/// <summary>
/// Represents a group view model for Overlay-based dialogs (Overlay Dialog, Overlay MessageBox and Overlay DialogBox),
/// providing options and commands to open each dialog type.
/// </summary>
internal sealed class OverlayDialogGroupViewModel : ObservableObject
{
    private readonly INotificationPublisher _notificationPublisher;
    private readonly IContentDialogService _contentDialogService;
    private readonly IMessageBoxFactory _messageBoxFactory;
    private readonly DialogHostOptions _hostOptions;

    private bool _isModal = true;
    private bool _showCloseButton = true;
    private bool _canDragMove = true;
    private bool _canLightDismiss;
    private bool _fullScreen;
    private MessageBoxResultOption _buttons = MessageBoxResultOption.OkCancel;
    private HorizontalPosition _horizontalAnchor = HorizontalPosition.Center;
    private VerticalPosition _verticalAnchor = VerticalPosition.Center;

    /// <summary>
    /// Gets the playground view model used to render the settings panel for this group.
    /// </summary>
    public PlaygroundViewModel Playground { get; }

    /// <summary>
    /// Gets the command to open an Overlay Dialog.
    /// </summary>
    public ICommand ShowOverlayDialogCommand { get; }

    /// <summary>
    /// Gets the command to open an Overlay MessageBox with the specified severity role.
    /// </summary>
    public ICommand ShowOverlayMessageBoxCommand { get; }

    /// <summary>
    /// Gets the command to open an Overlay DialogBox with the specified severity role.
    /// </summary>
    public ICommand ShowOverlayDialogBoxCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OverlayDialogGroupViewModel"/> class.
    /// </summary>
    public OverlayDialogGroupViewModel(
        INotificationPublisher notificationPublisher,
        IContentDialogService contentDialogService,
        IMessageBoxFactory messageBoxFactory,
        ICommandFactory commands,
        DialogHostOptions hostOptions)
    {
        _notificationPublisher = notificationPublisher;
        _contentDialogService = contentDialogService;
        _messageBoxFactory = messageBoxFactory;
        _hostOptions = hostOptions;

        var builder = new ControlThemeBuilder()
            .AddRoles(ThemeRole.Information, ThemeRole.Success, ThemeRole.Warning, ThemeRole.Error)
            .AddValueAction(
                (_, y) => _isModal = (bool?)y ?? true,
                true,
                x => x.DisplayName("Modal").Of<ToggleSwitchEditor>())
            .AddValueAction(
                (_, y) => _showCloseButton = (bool?)y ?? true,
                true,
                x => x.DisplayName("ShowCloseButton").Of<ToggleSwitchEditor>())
            .AddValueAction(
                (_, y) => _canDragMove = (bool?)y ?? true,
                true,
                x => x.DisplayName("CanDragMove").Of<ToggleSwitchEditor>())
            .AddValueAction(
                (_, y) => _canLightDismiss = (bool?)y ?? false,
                false,
                x => x.DisplayName("LightDismiss").Of<ToggleSwitchEditor>())
            .AddValueAction(
                (_, y) => _fullScreen = (bool?)y ?? false,
                false,
                x => x.DisplayName("FullScreen").Of<ToggleSwitchEditor>())
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
                           .AddChoice(VerticalPosition.Bottom, b => b.DisplayName(() => VerticalPosition.Bottom.Humanize()).WithIcon(MaterialIconKind.FormatVerticalAlignBottom))));

        var themes = new[] { new ControlThemeViewModelFactory(builder, commands).Create("OverlayDialog") }.ToObservableCollection();
        Playground = new PlaygroundViewModel("OverlayDialog", themes);

        ShowOverlayDialogCommand = commands.Create(async () => await ShowOverlayDialogAsync().ConfigureAwait(false));
        ShowOverlayMessageBoxCommand = commands.CreateRequired<ThemeRole>(async x => await ShowOverlayMessageBoxAsync(ToSeverity(x)).ConfigureAwait(false));
        ShowOverlayDialogBoxCommand = commands.CreateRequired<ThemeRole>(async x => await ShowOverlayDialogBoxAsync(ToSeverity(x)).ConfigureAwait(false));
    }

    /// <summary>
    /// Performs cleanup operations when the view model is disposed.
    /// </summary>
    protected override void DisposeManagedResources()
    {
        Playground.Dispose();
        base.DisposeManagedResources();
    }

    private async Task ShowOverlayDialogAsync()
    {
        var vm = new LoginDialogViewModel();
        var options = CreateOverlayOptions();

        var result = await _contentDialogService
            .ShowAsync(vm, DialogOptions.ForOverlay(vm, _isModal, options, OverlayDialogHostManager.MainHostId))
            .ConfigureAwait(false);
        ShowToasterResult(result, vm);
    }

    private async Task ShowOverlayMessageBoxAsync(MessageSeverity severity)
    {
        var result = await ShowOverlayMessageBoxCoreAsync(severity).ConfigureAwait(false);
        _notificationPublisher.Publish(new MessageNotification($"Result: {result}", severity: NotificationSeverity.Information));
    }

    private async Task ShowOverlayDialogBoxAsync(MessageSeverity severity)
    {
        var result = await ShowOverlayMessageBoxCoreAsync(severity).ConfigureAwait(false);
        _notificationPublisher.Publish(new MessageNotification($"Result: {result}", severity: NotificationSeverity.Information));
    }

    private async Task<MessageBoxResult> ShowOverlayMessageBoxCoreAsync(MessageSeverity severity)
    {
        var overlayOptions = CreateOverlayOptions();
        var messageBox = _messageBoxFactory.Create(MessageBoxOptionsHelper.Create(
            GetSampleMessage(severity),
            severity.Humanize(),
            severity,
            _buttons));

        var result = await _contentDialogService
            .ShowAsync<MessageBoxResult>(
                messageBox,
                DialogOptions.ForOverlay(
                    messageBox,
                    isModal: _isModal,
                    overlayOptions,
                    OverlayDialogHostManager.MainHostId))
            .ConfigureAwait(false);

        return result.IsSuccess ? result.Value : MessageBoxResult.Cancel;
    }

    private OverlayDialogOptions CreateOverlayOptions() => new()
    {
        IsCloseButtonVisible = _showCloseButton,
        CanLightDismiss = _canLightDismiss,
        FullScreen = _fullScreen,
        HorizontalAnchor = _horizontalAnchor,
        VerticalAnchor = _verticalAnchor,
        TopLevelKey = OverlayDialogHostManager.GetTopLevelKey(_hostOptions.TopLevelProvider())
    };

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

    private void ShowToasterResult(DialogResult<bool> result, LoginDialogViewModel viewModel)
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
