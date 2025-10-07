// -----------------------------------------------------------------------
// <copyright file="ClearBySimilarNotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using MyNet.UI.Notifications;

namespace MyNet.Avalonia.UI.Toasting.Lifetime.Clear;

public class ClearBySimilarNotification(INotification notification) : IClearStrategy
{
    public IEnumerable<Toast> GetToastsToRemove(IEnumerable<Toast> toasts)
    {
        var notificationsToRemove = toasts
            .Where(x => x.Notification.IsSimilar(notification))
            .ToList();

        return notificationsToRemove;
    }
}
