// -----------------------------------------------------------------------
// <copyright file="ClearAll.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace MyNet.Avalonia.UI.Toasting.Lifetime.Clear;

public class ClearAll : IClearStrategy
{
    public IEnumerable<Toast> GetToastsToRemove(IEnumerable<Toast> toasts) => toasts;
}
