// -----------------------------------------------------------------------
// <copyright file="IValueSelector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls.Primitives;

public interface IValueSelector<T> : IValueSelector
{
    T? SelectedValue { get; set; }
}

public interface IValueSelector
{
    event EventHandler<SelectionChangedEventArgs>? SelectedValueChanged;

    bool IsEmpty();

    void Clear();
}
