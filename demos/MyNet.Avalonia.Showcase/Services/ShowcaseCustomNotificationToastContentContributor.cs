// -----------------------------------------------------------------------
// <copyright file="ShowcaseCustomNotificationToastContentContributor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.Avalonia.Showcase.Notifications;
using MyNet.Avalonia.Showcase.Views.Samples;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Showcase.Services;

/// <summary>
/// Renders the showcase custom notification sample.
/// </summary>
internal sealed class ShowcaseCustomNotificationToastContentContributor : IAvaloniaToastContentContributor
{
    /// <inheritdoc />
    public int Order => 0;

    /// <inheritdoc />
    public bool TryCreateContent(INotification notification, double? width, [NotNullWhen(true)] out object? content)
    {
        if (notification is not ShowcaseCustomNotification)
        {
            content = null;
            return false;
        }

        content = new LargeContent1();
        return true;
    }
}
