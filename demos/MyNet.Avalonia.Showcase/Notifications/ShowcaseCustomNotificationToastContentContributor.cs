// -----------------------------------------------------------------------
// <copyright file="ShowcaseCustomNotificationToastContentContributor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.Avalonia.Showcase.Views.Samples;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Showcase.Notifications;

/// <summary>
/// Renders <see cref="ShowcaseCustomNotification"/> with inverse theme styling.
/// </summary>
internal sealed class ShowcaseCustomNotificationToastContentContributor : IAvaloniaToastContentContributor
{
    /// <inheritdoc />
    public int Order => 50;

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
