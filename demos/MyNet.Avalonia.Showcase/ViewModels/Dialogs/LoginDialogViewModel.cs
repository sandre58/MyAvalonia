// -----------------------------------------------------------------------
// <copyright file="LoginDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Avalonia.Showcase.ViewModels.Samples;
using MyNet.Globalization.Facade;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;
using MyNet.UI.ViewModels.Dialog;

namespace MyNet.Avalonia.Showcase.ViewModels.Dialogs;

internal sealed class LoginDialogViewModel : DialogViewModel<LoginResult>
{
    private readonly INotificationPublisher _notificationPublisher;

    public LoginDialogViewModel(ICommandFactory commandFactory, INotificationPublisher notificationPublisher)
        : base(commandFactory)
    {
        _notificationPublisher = notificationPublisher;
        SubmitCommand = commandFactory.Create(() => Submit());
    }

    #region Commands & status

    public ICommand SubmitCommand { get; }

    public string? StatusMessage
    {
        get;
        private set => SetProperty(ref field, value);
    }

    public bool IsSubmitSuccessful
    {
        get;
        private set => SetProperty(ref field, value);
    }

    #endregion

    public FormViewModel Form { get; } = new();

    public bool CanResize { get; set; } = true;

    public bool Submit()
    {
        StatusMessage = null;
        IsSubmitSuccessful = false;

        if (!Form.Validate())
        {
            StatusMessage = "ValidationFailed".Translate();
            return false;
        }

        IsSubmitSuccessful = true;
        StatusMessage = "SubmitSuccess".Translate();

        _notificationPublisher.PublishSuccess(StatusMessage);

        Close(new(Form.Login, Form.Password));
        return true;
    }

    protected override Task OnResetAsync(CancellationToken cancellationToken = default)
    {
        Form.Reset();
        IsSubmitSuccessful = false;
        StatusMessage = null;
        return Task.CompletedTask;
    }
}

internal record LoginResult(string Login, string Password);
