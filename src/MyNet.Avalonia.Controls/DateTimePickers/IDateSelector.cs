// -----------------------------------------------------------------------
// <copyright file="IDateSelector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.DateTimePickers;

public interface IDateSelector
{
    bool Match(DateTime? date);
}
