// -----------------------------------------------------------------------
// <copyright file="WindowMessageBox.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Input;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class WindowMessageBox : WindowDialog
{
    public const string PartYesButton = WindowMessageBoxContent.PartYesButton;
    public const string PartNoButton = WindowMessageBoxContent.PartNoButton;
    public const string PartOkButton = WindowMessageBoxContent.PartOkButton;
    public const string PartCancelButton = WindowMessageBoxContent.PartCancelButton;

    #region Severity

    public static readonly StyledProperty<MessageSeverity> SeverityProperty = AvaloniaProperty.Register<WindowMessageBox, MessageSeverity>(nameof(Severity));

    public MessageSeverity Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    #endregion

    #region Buttons

    public static readonly StyledProperty<MessageBoxResultOption> ButtonsProperty = AvaloniaProperty.Register<WindowMessageBox, MessageBoxResultOption>(nameof(Buttons));

    public MessageBoxResultOption Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    #endregion

    #region Message

    public static readonly StyledProperty<object?> MessageProperty = AvaloniaProperty.Register<WindowMessageBox, object?>(nameof(Message));

    public object? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    #endregion

    static WindowMessageBox() => MessageProperty.Changed.AddClassHandler<WindowMessageBox>((window, e) => window.SetCurrentValue(ContentProperty, e.NewValue));

    public WindowMessageBox()
    {
    }

    public WindowMessageBox(MessageBoxResultOption buttons) => Buttons = buttons;

    protected override Type StyleKeyOverride => typeof(WindowMessageBox);

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        if (e.Key is not Key.Escape) return;

        CloseWithResult(DialogButtonHelper.GetDefaultCloseResult(Buttons));
    }
}
