// -----------------------------------------------------------------------
// <copyright file="FallbackToastContentContributor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Extended.Toasting;

/// <summary>
/// Fallback toast content for notifications without a dedicated template.
/// </summary>
public sealed class FallbackToastContentContributor : IAvaloniaToastContentContributor
{
    /// <inheritdoc />
    public int Order => int.MaxValue;

    /// <inheritdoc />
    public bool TryCreateContent(INotification notification, double? width, [NotNullWhen(true)] out object? content)
    {
        var control = new ContentControl { Content = notification };

        if (width is > 0)
            control.Width = width.Value;

        content = control;
        return true;
    }
}
