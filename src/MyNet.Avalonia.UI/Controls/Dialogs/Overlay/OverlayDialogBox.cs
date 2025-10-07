// -----------------------------------------------------------------------
// <copyright file="OverlayDialogBox.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.UI.Controls.Primitives;
using MyNet.UI.Dialogs.CustomDialogs;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartOKButton, typeof(Button))]
[TemplatePart(PartCancelButton, typeof(Button))]
[TemplatePart(PartYesButton, typeof(Button))]
[TemplatePart(PartNoButton, typeof(Button))]
public class OverlayDialogBox : OverlayDialogBase
{
    public const string PartOKButton = "PART_OKButton";
    public const string PartCancelButton = "PART_CancelButton";
    public const string PartYesButton = "PART_YesButton";
    public const string PartNoButton = "PART_NoButton";

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<OverlayDialogBox, string?>(
            nameof(Title));

    public static readonly StyledProperty<MessageBoxResultOption> ButtonsProperty = AvaloniaProperty.Register<OverlayDialogBox, MessageBoxResultOption>(nameof(Buttons));

    public static readonly StyledProperty<MessageSeverity> SeverityProperty = AvaloniaProperty.Register<OverlayDialogBox, MessageSeverity>(nameof(Severity));

    private Button? _cancelButton;
    private Button? _noButton;

    private Button? _okButton;
    private Button? _yesButton;

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

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
        Button.ClickEvent.RemoveHandler(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PartOKButton);
        _cancelButton = e.NameScope.Find<Button>(PartCancelButton);
        _yesButton = e.NameScope.Find<Button>(PartYesButton);
        _noButton = e.NameScope.Find<Button>(PartNoButton);
        Button.ClickEvent.AddHandler(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }

    private void SetButtonVisibility()
    {
        var closeButtonVisible = IsCloseButtonVisible ?? (DataContext is IDialogViewModel || Buttons != MessageBoxResultOption.YesNo);
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

    private void DefaultButtonsClose(object? sender, RoutedEventArgs args)
    {
        if (sender is Button button)
        {
            if (button == _okButton)
                OnElementClosing(this, MessageBoxResult.Ok);
            else if (button == _cancelButton)
                OnElementClosing(this, MessageBoxResult.Cancel);
            else if (button == _yesButton)
                OnElementClosing(this, MessageBoxResult.Yes);
            else if (button == _noButton) OnElementClosing(this, MessageBoxResult.No);
        }
    }

    public override void Close()
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
            OnElementClosing(this, result);
        }
    }
}
