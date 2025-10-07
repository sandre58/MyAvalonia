// -----------------------------------------------------------------------
// <copyright file="ResultEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class ResultEventArgs : RoutedEventArgs
{
    public object? Result { get; set; }

    public ResultEventArgs(object? result) => Result = result;

    public ResultEventArgs(RoutedEvent routedEvent, object? result)
        : base(routedEvent) => Result = result;
}
