// -----------------------------------------------------------------------
// <copyright file="ITextPicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls.Primitives;

public interface ITextPicker : IValueSelector, IPopupControl
{
    string? Text { get; set; }

    event EventHandler<TextChangedEventArgs>? TextChanged;
}
