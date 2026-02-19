// -----------------------------------------------------------------------
// <copyright file="Page.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Pages;

[DoNotNotify]
internal abstract class Page : UserControl
{
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataContextProperty && change.NewValue is null && change.OldValue is not null)
        {
            Content = null;
        }
    }
}
