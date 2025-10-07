// -----------------------------------------------------------------------
// <copyright file="ClearFirst.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace MyNet.Avalonia.UI.Toasting.Lifetime.Clear;

public class ClearFirst : IClearStrategy
{
    public IEnumerable<Toast> GetToastsToRemove(IEnumerable<Toast> toasts)
    {
        var list = toasts.ToList();
        if (list.Count == 0)
        {
            return [];
        }

        var lastMessage = list[0];

        return [lastMessage];
    }
}
