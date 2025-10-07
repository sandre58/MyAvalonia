// -----------------------------------------------------------------------
// <copyright file="ClearByNotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace MyNet.Avalonia.UI.Toasting.Lifetime.Clear;

public class ClearByNotification(MyNet.UI.Notifications.INotification notification) : IClearStrategy
{
    public IEnumerable<Toast> GetToastsToRemove(IEnumerable<Toast> toasts)
    {
        var notificationsToRemove = toasts
            .Where(x => x.Notification.Equals(notification))
            .ToList();

        return notificationsToRemove;
    }
}
