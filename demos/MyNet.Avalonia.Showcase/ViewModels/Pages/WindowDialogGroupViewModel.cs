// -----------------------------------------------------------------------
// <copyright file="WindowDialogGroupViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Windows.Input;
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
/// Represents a group view model for Window-based dialogs (Window Dialog and Window MessageBox),
/// providing options and commands to open each dialog type.
/// </summary>
internal sealed class WindowDialogGroupViewModel : ObservableObject
{
    private readonly INotificationPublisher _notificationPublisher;
    private readonly IContentDialogService _contentDialogService;
    private readonly IMessageBoxFactory _messageBoxFactory;
    private bool _isModal = true;
    private MessageBoxResultOption _buttons = MessageBoxResultOption.OkCancel;

    /// <summary>
    /// Gets the playground view model used to render the settings panel for this group.
    /// </summary>
    public PlaygroundViewModel Playground { get; }

    /// <summary>
    /// Gets the command to open a Window Dialog.
    /// </summary>
    public ICommand ShowWindowDialogCommand { get; }

    /// <summary>
    /// Gets the command to open a Window MessageBox with the specified severity role.
    /// </summary>
    public ICommand ShowWindowMessageBoxCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowDialogGroupViewModel"/> class.
    /// </summary>
    public WindowDialogGroupViewModel(
        INotificationPublisher notificationPublisher,
        IContentDialogService contentDialogService,
        IMessageBoxFactory messageBoxFactory,
        ICommandFactory commands)
    {
        _notificationPublisher = notificationPublisher;
        _contentDialogService = contentDialogService;
        _messageBoxFactory = messageBoxFactory;

        var builder = new ControlThemeBuilder()
            .AddRoles(ThemeRole.Information, ThemeRole.Success, ThemeRole.Warning, ThemeRole.Error)
            .AddValueAction(
                (_, y) => _isModal = (bool?)y ?? true,
                true,
                x => x.DisplayName("Modal").Of<ToggleSwitchEditor>())
            .AddValueAction(
                (_, y) => _buttons = (MessageBoxResultOption?)y ?? MessageBoxResultOption.OkCancel,
                MessageBoxResultOption.OkCancel,
                x => x.DisplayName("Buttons")
                       .Of<ListBoxEditor>(editor => editor
                           .AddChoice(MessageBoxResultOption.Ok, b => b.DisplayName(() => MessageBoxResultOption.Ok.Humanize()))
                           .AddChoice(MessageBoxResultOption.OkCancel, b => b.DisplayName(() => MessageBoxResultOption.OkCancel.Humanize()))
                           .AddChoice(MessageBoxResultOption.YesNo, b => b.DisplayName(() => MessageBoxResultOption.YesNo.Humanize()))
                           .AddChoice(MessageBoxResultOption.YesNoCancel, b => b.DisplayName(() => MessageBoxResultOption.YesNoCancel.Humanize()))));

        var themes = new[] { new ControlThemeViewModelFactory(builder, commands).Create("WindowDialog") }.ToObservableCollection();
        Playground = new("WindowDialog", themes);

        ShowWindowDialogCommand = commands.Create(async () => await ShowWindowDialogAsync().ConfigureAwait(false));
        ShowWindowMessageBoxCommand = commands.CreateRequired<ThemeRole>(async x => await ShowWindowMessageBoxAsync(ToSeverity(x)).ConfigureAwait(false));
    }

    /// <summary>
    /// Performs cleanup operations when the view model is disposed.
    /// </summary>
    protected override void DisposeManagedResources()
    {
        Playground.Dispose();
        base.DisposeManagedResources();
    }

    private async Task ShowWindowDialogAsync()
    {
        var vm = new LoginDialogViewModel();

        var result = await _contentDialogService
            .ShowAsync(vm, DialogOptions.ForWindow(vm, _isModal))
            .ConfigureAwait(false);
        ShowToasterResult(result, vm);
    }

    private async Task ShowWindowMessageBoxAsync(MessageSeverity severity)
    {
        var messageBox = _messageBoxFactory.Create(MessageBoxOptionsHelper.Create(
            GetSampleMessage(severity),
            severity.Humanize(),
            severity,
            _buttons));

        var result = await _contentDialogService
            .ShowAsync(messageBox, DialogOptions.ForWindow(messageBox, _isModal))
            .ConfigureAwait(false);

        var mapped = result.IsSuccess ? result.Value : MessageBoxResult.Cancel;
        _notificationPublisher.Publish(new MessageNotification($"Result: {mapped}", severity: NotificationSeverity.Information));
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
