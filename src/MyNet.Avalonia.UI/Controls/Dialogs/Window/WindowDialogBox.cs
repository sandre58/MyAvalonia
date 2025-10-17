// -----------------------------------------------------------------------
// <copyright file="WindowDialogBox.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Extensions;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartYesButton, typeof(Button))]
[TemplatePart(PartNoButton, typeof(Button))]
[TemplatePart(PartOKButton, typeof(Button))]
[TemplatePart(PartCancelButton, typeof(Button))]
public class WindowDialogBox : WindowDialog
{
    public const string PartYesButton = "PART_YesButton";
    public const string PartNoButton = "PART_NoButton";
    public const string PartOKButton = "PART_OKButton";
    public const string PartCancelButton = "PART_CancelButton";

    public static readonly StyledProperty<MessageBoxResultOption> ButtonsProperty = AvaloniaProperty.Register<WindowDialogBox, MessageBoxResultOption>(nameof(Buttons));

    public static readonly StyledProperty<MessageSeverity> SeverityProperty = AvaloniaProperty.Register<WindowDialogBox, MessageSeverity>(nameof(Severity));

    private Button? _cancelButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _yesButton;

    protected override Type StyleKeyOverride { get; } = typeof(WindowDialogBox);

    public MessageBoxResultOption Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public MessageSeverity Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnDefaultClose, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PartOKButton);
        _cancelButton = e.NameScope.Find<Button>(PartCancelButton);
        _yesButton = e.NameScope.Find<Button>(PartYesButton);
        _noButton = e.NameScope.Find<Button>(PartNoButton);
        Button.ClickEvent.AddHandler(OnDefaultClose, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }

    private void OnDefaultClose(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _yesButton))
            Close(MessageBoxResult.Yes);
        else if (Equals(sender, _noButton))
            Close(MessageBoxResult.No);
        else if (Equals(sender, _okButton))
            Close(MessageBoxResult.Ok);
        else if (Equals(sender, _cancelButton))
            Close(MessageBoxResult.Cancel);
    }

    private void SetButtonVisibility()
    {
        // Close button should be hidden instead if invisible to retain layout.
        IsVisibleProperty.SetValue(true, CloseButton);
        var closeButtonVisible = IsCloseButtonVisible && (DataContext is IDialogViewModel || Buttons != MessageBoxResultOption.YesNo);
        IsHitTestVisibleProperty.SetValue(closeButtonVisible, CloseButton);
        if (!closeButtonVisible)
        {
            OpacityProperty.SetValue(0, CloseButton);
        }

        switch (Buttons)
        {
            case MessageBoxResultOption.None:
                IsVisibleProperty.SetValue(false, _okButton, _cancelButton, _yesButton, _noButton);
                break;
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
            default:
                break;
        }
    }

    protected override void OnCloseButtonClicked(object? sender, RoutedEventArgs args)
    {
        if (DataContext is IDialogViewModel context)
        {
            context.Close();
        }
        else
        {
            var result = Buttons switch
            {
                MessageBoxResultOption.None => MessageBoxResult.None,
                MessageBoxResultOption.Ok => MessageBoxResult.Ok,
                MessageBoxResultOption.OkCancel => MessageBoxResult.Cancel,
                MessageBoxResultOption.YesNo => MessageBoxResult.No,
                MessageBoxResultOption.YesNoCancel => MessageBoxResult.Cancel,
                _ => MessageBoxResult.None
            };
            Close(result);
        }
    }
}
