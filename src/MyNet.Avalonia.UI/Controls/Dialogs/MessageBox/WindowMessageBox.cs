// -----------------------------------------------------------------------
// <copyright file="WindowMessageBox.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Extensions;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartCloseButton, typeof(Button))]
[TemplatePart(PartNoButton, typeof(Button))]
[TemplatePart(PartOKButton, typeof(Button))]
[TemplatePart(PartCancelButton, typeof(Button))]
[TemplatePart(PartYesButton, typeof(Button))]
public class WindowMessageBox(MessageBoxResultOption buttons) : Window
{
    public const string PartCloseButton = "PART_CloseButton";
    public const string PartYesButton = "PART_YesButton";
    public const string PartNoButton = "PART_NoButton";
    public const string PartOKButton = "PART_OKButton";
    public const string PartCancelButton = "PART_CancelButton";

    public static readonly StyledProperty<MessageBoxIcon> MessageIconProperty =
        AvaloniaProperty.Register<WindowMessageBox, MessageBoxIcon>(
            nameof(MessageIcon));

    private Button? _closeButton;

    private Button? _cancelButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _yesButton;

    public WindowMessageBox()
        : this(MessageBoxResultOption.Ok)
    {
    }

    protected override Type StyleKeyOverride => typeof(WindowMessageBox);

    public MessageBoxIcon MessageIcon
    {
        get => GetValue(MessageIconProperty);
        set => SetValue(MessageIconProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        Button.ClickEvent.RemoveHandler(OnCloseButtonClick, _closeButton);
        _yesButton = e.NameScope.Find<Button>(PartYesButton);
        _noButton = e.NameScope.Find<Button>(PartNoButton);
        _okButton = e.NameScope.Find<Button>(PartOKButton);
        _cancelButton = e.NameScope.Find<Button>(PartCancelButton);
        _closeButton = e.NameScope.Find<Button>(PartCloseButton);
        Button.ClickEvent.AddHandler(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        Button.ClickEvent.AddHandler(OnCloseButtonClick, _closeButton);
        SetButtonVisibility();
    }

    private void SetButtonVisibility()
    {
        var closeButtonVisible = buttons != MessageBoxResultOption.YesNo;
        IsVisibleProperty.SetValue(closeButtonVisible, _closeButton);
        switch (buttons)
        {
            case MessageBoxResultOption.Ok:
                IsVisibleProperty.SetValue(true, _okButton);
                IsVisibleProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                break;
            case MessageBoxResultOption.OkCancel:
                IsVisibleProperty.SetValue(true, _okButton, _cancelButton);
                IsVisibleProperty.SetValue(false, _yesButton, _noButton);
                break;
            case MessageBoxResultOption.YesNo:
                IsVisibleProperty.SetValue(false, _okButton, _cancelButton);
                IsVisibleProperty.SetValue(true, _yesButton, _noButton);
                break;
            case MessageBoxResultOption.YesNoCancel:
                IsVisibleProperty.SetValue(false, _okButton);
                IsVisibleProperty.SetValue(true, _cancelButton, _yesButton, _noButton);
                break;
            case MessageBoxResultOption.None:
            default:
                break;
        }
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        if (buttons == MessageBoxResultOption.Ok) Close(MessageBoxResult.Ok);

        Close(MessageBoxResult.Cancel);
    }

    private void OnDefaultButtonClick(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _okButton))
            Close(MessageBoxResult.Ok);
        else if (Equals(sender, _cancelButton))
            Close(MessageBoxResult.Cancel);
        else if (Equals(sender, _yesButton))
            Close(MessageBoxResult.Yes);
        else if (Equals(sender, _noButton)) Close(MessageBoxResult.No);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        if (e.Key is not Key.Escape) return;

        switch (buttons)
        {
            case MessageBoxResultOption.Ok:
                Close(MessageBoxResult.Ok);
                break;
            case MessageBoxResultOption.OkCancel:
            case MessageBoxResultOption.YesNoCancel:
            case MessageBoxResultOption.None:
                Close(MessageBoxResult.Cancel);
                break;
            case MessageBoxResultOption.YesNo:
                Close(MessageBoxResult.No);
                break;
            default:
                break;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e) => BeginMoveDrag(e);

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var defaultButton = buttons switch
        {
            MessageBoxResultOption.Ok => _okButton,
            MessageBoxResultOption.OkCancel => _cancelButton,
            MessageBoxResultOption.YesNo => _yesButton,
            MessageBoxResultOption.YesNoCancel => _cancelButton,
            MessageBoxResultOption.None => null,
            _ => null
        };
        Button.IsDefaultProperty.SetValue(true, defaultButton);
        _ = defaultButton?.Focus();
    }
}
