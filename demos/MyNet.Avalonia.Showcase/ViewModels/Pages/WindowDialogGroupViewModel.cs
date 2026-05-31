// -----------------------------------------------------------------------
// <copyright file="WindowDialogGroupViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Avalonia.Extended.Dialogs;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Dialogs;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;
using MyNet.Avalonia.Showcase.Views.Dialogs;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Humanizer;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Notifications;
using MyNet.UI.Notifications.Models;
using MyNet.Utilities;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

/// <summary>
/// Represents a group view model for Window-based dialogs (Window Dialog and Window MessageBox),
/// providing options and commands to open each dialog type.
/// </summary>
internal sealed class WindowDialogGroupViewModel : ObservableObject
{
    private static readonly WindowDialogService WindowDialogService = new();
    private static readonly WindowMessageBoxService WindowMessageBoxService = new();
    private readonly INotificationPublisher _notificationPublisher;

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
    /// <param name="notificationPublisher">Publishes toast notifications after dialog actions.</param>
    public WindowDialogGroupViewModel(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
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

        var themes = new[] { new ControlThemeViewModelFactory(builder).Create("WindowDialog") }.ToObservableCollection();
        Playground = new PlaygroundViewModel("WindowDialog", themes);

        ShowWindowDialogCommand = CommandsManager.Create(async () => await ShowWindowDialogAsync().ConfigureAwait(false));
        ShowWindowMessageBoxCommand = CommandsManager.CreateNotNull<ThemeRole>(async x => await ShowWindowMessageBoxAsync(ToSeverity(x)).ConfigureAwait(false));
    }

    /// <summary>
    /// Performs cleanup operations when the view model is disposed.
    /// </summary>
    protected override void Cleanup()
    {
        base.Cleanup();
        Playground.Dispose();
    }

    private async Task ShowWindowDialogAsync()
    {
        var vm = new LoginDialogViewModel();
        var view = new LoginDialogView { DataContext = vm };

        if (_isModal)
        {
            var result = await WindowDialogService.ShowModalAsync(view, vm).ConfigureAwait(false);
            ShowToasterResult(result, vm);
        }
        else
        {
            await WindowDialogService.ShowAsync(view, vm).ConfigureAwait(false);
        }
    }

    private async Task ShowWindowMessageBoxAsync(MessageSeverity severity)
    {
        var result = await WindowMessageBoxService.ShowAsync(
            GetSampleMessage(severity),
            severity.Humanize(),
            _buttons,
            severity).ConfigureAwait(false);
        _notificationPublisher.Publish(new MessageNotification($"Result: {result}", severity: NotificationSeverity.Information));
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

    private void ShowToasterResult(bool? result, LoginDialogViewModel viewModel)
    {
        if (!result.HasValue)
            _notificationPublisher.Publish(new MessageNotification("No result.", severity: NotificationSeverity.Warning));
        else if (result.Value)
            _notificationPublisher.Publish(new MessageNotification("Dialog has been validated.", severity: NotificationSeverity.Success));
        else
            _notificationPublisher.Publish(new MessageNotification("Dialog has been cancelled.", severity: NotificationSeverity.Error));

        _notificationPublisher.Publish(new MessageNotification(
            $"Login: {viewModel.Form.Login} ; Password: {viewModel.Form.Password}",
            severity: NotificationSeverity.Information));
    }
}

