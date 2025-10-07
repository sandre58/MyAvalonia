// -----------------------------------------------------------------------
// <copyright file="IControlProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.Proxy;

public interface IControlProxy : IDisposable
{
    bool IsEmpty();

    bool IsFocused();

    bool IsActive();

    event EventHandler? IsEmptyChanged;

    event EventHandler? IsFocusedChanged;

    event EventHandler? IsActiveChanged;
}
