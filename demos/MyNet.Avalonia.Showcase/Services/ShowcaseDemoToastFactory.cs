// -----------------------------------------------------------------------
// <copyright file="ShowcaseDemoToastFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.UI.Commands;
using MyNet.UI.Notifications.Models;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Models;
using MyNet.UI.Toasting.Settings;

namespace MyNet.Avalonia.Showcase.Services;

/// <summary>
/// Toast factory used by the showcase notifications playground.
/// </summary>
internal sealed class ShowcaseDemoToastFactory(ICommandFactory commandFactory) : IToastFactory
{
    /// <summary>
    /// Gets or sets toast settings applied to newly created toasts.
    /// </summary>
    public static ToastSettings CurrentSettings { get; set; } = ToastSettings.Default;

    /// <inheritdoc />
    public IToast Create(INotification notification)
    {
        var settings = CurrentSettings;

        var closeCommand = notification is IClosableNotification { IsClosable: true } closable
            ? commandFactory.Create((Action)closable.RequestClose)
            : null;

        var clickCommand = notification is ActionNotification actionNotification && actionNotification.Action is not null
            ? commandFactory.Create(() => actionNotification.Action(actionNotification))
            : null;

        return new Toast(notification, settings, clickCommand, closeCommand);
    }
}
