// -----------------------------------------------------------------------
// <copyright file="WindowDialog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using MyNet.UI;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class WindowDialog : Window
{
    static WindowDialog() => DataContextProperty.Changed.AddClassHandler<WindowDialog, object?>((window, e) => window.OnDataContextChanged(e));

    protected override Type StyleKeyOverride { get; } = typeof(WindowDialog);

    private void OnDataContextChanged(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IClosable oldContext)
            oldContext.CloseRequested -= OnContextRequestClose;

        if (args.NewValue.Value is IClosable newContext)
            newContext.CloseRequested += OnContextRequestClose;
    }

    private void OnContextRequestClose(object? sender, CloseRequestedEventArgs args) => Close(args.Force ? true : null);
}
