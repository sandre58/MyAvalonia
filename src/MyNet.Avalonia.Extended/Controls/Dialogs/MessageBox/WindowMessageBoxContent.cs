// -----------------------------------------------------------------------
// <copyright file="WindowMessageBoxContent.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartOkButton, typeof(Button))]
[TemplatePart(PartCancelButton, typeof(Button))]
[TemplatePart(PartYesButton, typeof(Button))]
[TemplatePart(PartNoButton, typeof(Button))]
public class WindowMessageBoxContent : TemplatedControl
{
    public const string PartYesButton = "PART_YesButton";
    public const string PartNoButton = "PART_NoButton";
    public const string PartOkButton = "PART_OKButton";
    public const string PartCancelButton = "PART_CancelButton";

    public static readonly StyledProperty<object?> MessageProperty =
        AvaloniaProperty.Register<WindowMessageBoxContent, object?>(nameof(Message));

    public static readonly StyledProperty<MessageSeverity> SeverityProperty =
        AvaloniaProperty.Register<WindowMessageBoxContent, MessageSeverity>(nameof(Severity));

    public static readonly StyledProperty<MessageBoxResultOption> ButtonsProperty =
        AvaloniaProperty.Register<WindowMessageBoxContent, MessageBoxResultOption>(nameof(Buttons));

    private Button? _cancelButton;
    private WindowMessageBox? _host;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _yesButton;

    static WindowMessageBoxContent() => ButtonsProperty.Changed.AddClassHandler<WindowMessageBoxContent>((content, _) => content.SetButtonVisibility());

    public object? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public MessageSeverity Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    public MessageBoxResultOption Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    protected override Type StyleKeyOverride => typeof(WindowMessageBoxContent);

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _host = this.FindAncestorOfType<WindowMessageBox>();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _host = null;
        base.OnDetachedFromVisualTree(e);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PartOkButton);
        _cancelButton = e.NameScope.Find<Button>(PartCancelButton);
        _yesButton = e.NameScope.Find<Button>(PartYesButton);
        _noButton = e.NameScope.Find<Button>(PartNoButton);
        Button.ClickEvent.AddHandler(OnButtonClick, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        FocusDefaultButton();
    }

    private void FocusDefaultButton()
    {
        var defaultButton = Buttons switch
        {
            MessageBoxResultOption.Ok => _okButton,
            MessageBoxResultOption.OkCancel => _cancelButton,
            MessageBoxResultOption.YesNo => _yesButton,
            MessageBoxResultOption.YesNoCancel => _cancelButton,
            _ => null
        };
        Button.IsDefaultProperty.SetValue(true, defaultButton);
        _ = defaultButton?.Focus();
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        var window = _host ?? this.FindAncestorOfType<WindowMessageBox>();
        if (window is null)
            return;

        if (Equals(sender, _okButton))
            window.CloseWithResult(MessageBoxResult.Ok);
        else if (Equals(sender, _cancelButton))
            window.CloseWithResult(MessageBoxResult.Cancel);
        else if (Equals(sender, _yesButton))
            window.CloseWithResult(MessageBoxResult.Yes);
        else if (Equals(sender, _noButton))
            window.CloseWithResult(MessageBoxResult.No);
    }

    private void SetButtonVisibility()
        => DialogButtonHelper.SetButtonVisibility(Buttons, _okButton, _cancelButton, _yesButton, _noButton);
}
