// -----------------------------------------------------------------------
// <copyright file="MessageBoxContent.cs" company="Stéphane ANDRE">
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
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Shared message box body: horizontal layout with semantic leading badge, title, message, and action buttons.
/// </summary>
[TemplatePart(PartOkButton, typeof(Button))]
[TemplatePart(PartCancelButton, typeof(Button))]
[TemplatePart(PartYesButton, typeof(Button))]
[TemplatePart(PartNoButton, typeof(Button))]
public class MessageBoxContent : DialogPanel
{
    public const string PartYesButton = "PART_YesButton";
    public const string PartNoButton = "PART_NoButton";
    public const string PartOkButton = "PART_OKButton";
    public const string PartCancelButton = "PART_CancelButton";

    public static readonly StyledProperty<object?> MessageProperty =
        AvaloniaProperty.Register<MessageBoxContent, object?>(nameof(Message));

    public static readonly StyledProperty<object?> TitleProperty =
        AvaloniaProperty.Register<MessageBoxContent, object?>(nameof(Title));

    public static readonly StyledProperty<MessageSeverity> SeverityProperty =
        AvaloniaProperty.Register<MessageBoxContent, MessageSeverity>(nameof(Severity));

    public static readonly StyledProperty<MessageBoxResultOption> ButtonsProperty =
        AvaloniaProperty.Register<MessageBoxContent, MessageBoxResultOption>(nameof(Buttons));

    private Button? _cancelButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _yesButton;

    static MessageBoxContent()
    {
        FocusableProperty.OverrideDefaultValue<MessageBoxContent>(true);
        ButtonsProperty.Changed.AddClassHandler<MessageBoxContent>((content, _) => content.SetButtonVisibility());
        TitleProperty.Changed.AddClassHandler<MessageBoxContent>((content, e) => content.SetCurrentValue(HeaderProperty, e.NewValue));
        MessageProperty.Changed.AddClassHandler<MessageBoxContent>((content, e) => content.SetCurrentValue(ContentProperty, e.NewValue));
    }

    public object? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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

    protected override Type StyleKeyOverride => typeof(MessageBoxContent);

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
        ConfigureDefaultButtons();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        ConfigureDefaultButtons();
        _ = DialogButtonHelper.GetAffirmativeButton(Buttons, _okButton, _yesButton)?.Focus();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        DialogKeyboardHelper.TryHandleMessageBoxKey(e, Buttons, CloseWithResult);
    }

    private void ConfigureDefaultButtons()
    {
        foreach (var button in new[] { _okButton, _cancelButton, _yesButton, _noButton })
            Button.IsDefaultProperty.SetValue(false, button);

        var affirmative = DialogButtonHelper.GetAffirmativeButton(Buttons, _okButton, _yesButton);
        Button.IsDefaultProperty.SetValue(true, affirmative);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _okButton))
            CloseWithResult(MessageBoxResult.Ok);
        else if (Equals(sender, _cancelButton))
            CloseWithResult(MessageBoxResult.Cancel);
        else if (Equals(sender, _yesButton))
            CloseWithResult(MessageBoxResult.Yes);
        else if (Equals(sender, _noButton))
            CloseWithResult(MessageBoxResult.No);
    }

    private void CloseWithResult(MessageBoxResult result)
    {
        if (this.FindAncestorOfType<WindowMessageBox>() is { } window)
        {
            window.CloseWithResult(result);
            return;
        }

        if (this.FindAncestorOfType<OverlayMessageBox>() is { } overlay)
            overlay.CloseWithResult(result);
    }

    private void SetButtonVisibility()
        => DialogButtonHelper.SetButtonVisibility(Buttons, _okButton, _cancelButton, _yesButton, _noButton);
}
