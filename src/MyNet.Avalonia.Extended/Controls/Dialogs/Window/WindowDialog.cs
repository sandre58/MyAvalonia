// -----------------------------------------------------------------------
// <copyright file="WindowDialog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using MyNet.UI.Dialogs.ContentDialogs;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class WindowDialog : Window
{
    static WindowDialog() => DataContextProperty.Changed.AddClassHandler<WindowDialog, object?>((window, e) => window.OnDataContextChanged(e));

    protected override Type StyleKeyOverride { get; } = typeof(WindowDialog);

    private void OnDataContextChanged(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogViewModel oldContext) oldContext.CloseRequest -= OnContextRequestClose;

        if (args.NewValue.Value is IDialogViewModel newContext) newContext.CloseRequest += OnContextRequestClose;
    }

    private void OnContextRequestClose(object? sender, object? args) => Close(args);
}
