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
using MyNet.Avalonia.Extended.Controls.Primitives;
using MyNet.Avalonia.Extensions;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartOkButton, typeof(Button))]
[TemplatePart(PartCancelButton, typeof(Button))]
[TemplatePart(PartYesButton, typeof(Button))]
[TemplatePart(PartNoButton, typeof(Button))]
public class OverlayDialogBox : OverlayDialogBase
{
    public const string PartOkButton = "PART_OKButton";
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
        _okButton = e.NameScope.Find<Button>(PartOkButton);
        _cancelButton = e.NameScope.Find<Button>(PartCancelButton);
        _yesButton = e.NameScope.Find<Button>(PartYesButton);
        _noButton = e.NameScope.Find<Button>(PartNoButton);
        Button.ClickEvent.AddHandler(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }

    private void SetButtonVisibility()
    {
        var closeButtonVisible = IsCloseButtonVisible ?? (DataContext is IDialogViewModel || Buttons != MessageBoxResultOption.YesNo);
        CloseButton?.SetValue(IsHitTestVisibleProperty, closeButtonVisible);
        if (!closeButtonVisible && CloseButton != null)
        {
            CloseButton.SetValue(OpacityProperty, 0);
        }

        switch (Buttons)
        {
            case MessageBoxResultOption.None:
                _okButton?.SetValue(IsVisibleProperty, false);
                _cancelButton?.SetValue(IsVisibleProperty, false);
                _yesButton?.SetValue(IsVisibleProperty, false);
                _noButton?.SetValue(IsVisibleProperty, false);
                break;
            case MessageBoxResultOption.Ok:
                _okButton?.SetValue(IsVisibleProperty, true);
                _cancelButton?.SetValue(IsVisibleProperty, false);
                _yesButton?.SetValue(IsVisibleProperty, false);
                _noButton?.SetValue(IsVisibleProperty, false);
                break;
            case MessageBoxResultOption.OkCancel:
                _okButton?.SetValue(IsVisibleProperty, true);
                _cancelButton?.SetValue(IsVisibleProperty, true);
                _yesButton?.SetValue(IsVisibleProperty, false);
                _noButton?.SetValue(IsVisibleProperty, false);
                break;
            case MessageBoxResultOption.YesNo:
                _okButton?.SetValue(IsVisibleProperty, false);
                _cancelButton?.SetValue(IsVisibleProperty, false);
                _yesButton?.SetValue(IsVisibleProperty, true);
                _noButton?.SetValue(IsVisibleProperty, true);
                break;
            case MessageBoxResultOption.YesNoCancel:
                _okButton?.SetValue(IsVisibleProperty, false);
                _cancelButton?.SetValue(IsVisibleProperty, true);
                _yesButton?.SetValue(IsVisibleProperty, true);
                _noButton?.SetValue(IsVisibleProperty, true);
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
