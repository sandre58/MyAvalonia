// -----------------------------------------------------------------------
// <copyright file="CompositeAvaloniaToastContentFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Extended.Toasting;

/// <summary>
/// Resolves toast content through registered <see cref="IAvaloniaToastContentContributor"/> instances.
/// </summary>
public sealed class CompositeAvaloniaToastContentFactory(IEnumerable<IAvaloniaToastContentContributor> contributors) : IAvaloniaToastContentFactory
{
    private readonly IAvaloniaToastContentContributor[] _contributors = contributors.OrderBy(static x => x.Order).ToArray()
        ?? throw new ArgumentNullException(nameof(contributors));

    /// <inheritdoc />
    public object CreateContent(INotification notification, double? width = null)
    {
        ArgumentNullException.ThrowIfNull(notification);

        foreach (var contributor in _contributors)
        {
            if (contributor.TryCreateContent(notification, width, out var content))
                return content;
        }

        throw new InvalidOperationException($"No toast content contributor could render '{notification.GetType().FullName}'.");
    }
}
