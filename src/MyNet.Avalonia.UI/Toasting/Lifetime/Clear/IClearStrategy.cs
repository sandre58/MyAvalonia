// -----------------------------------------------------------------------
// <copyright file="IClearStrategy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace MyNet.Avalonia.UI.Toasting.Lifetime.Clear;

public interface IClearStrategy
{
    IEnumerable<Toast> GetToastsToRemove(IEnumerable<Toast> toasts);
}
