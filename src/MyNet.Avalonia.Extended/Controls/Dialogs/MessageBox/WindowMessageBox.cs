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
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartNoButton, typeof(Button))]
[TemplatePart(PartOkButton, typeof(Button))]
[TemplatePart(PartCancelButton, typeof(Button))]
[TemplatePart(PartYesButton, typeof(Button))]
public class WindowMessageBox : WindowDialog
{
    public const string PartYesButton = "PART_YesButton";
    public const string PartNoButton = "PART_NoButton";
    public const string PartOkButton = "PART_OKButton";
    public const string PartCancelButton = "PART_CancelButton";

    #region Severity

    /// <summary>
    /// Provides Severity Property.
    /// </summary>
    public static readonly StyledProperty<MessageSeverity> SeverityProperty =
        AvaloniaProperty.Register<WindowMessageBox, MessageSeverity>(nameof(Severity));

    /// <summary>
    /// Gets or sets the Severity property.
    /// </summary>
    public MessageSeverity Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    #endregion

    #region Buttons

    /// <summary>
    /// Provides Buttons Property.
    /// </summary>
    public static readonly StyledProperty<MessageBoxResultOption> ButtonsProperty =
        AvaloniaProperty.Register<WindowMessageBox, MessageBoxResultOption>(nameof(Buttons));

    /// <summary>
    /// Gets or sets the Buttons property.
    /// </summary>
    public MessageBoxResultOption Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    #endregion

    private Button? _cancelButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _yesButton;

    public WindowMessageBox()
    {
    }

    public WindowMessageBox(MessageBoxResultOption buttons) => Buttons = buttons;

    protected override Type StyleKeyOverride => typeof(WindowMessageBox);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        _yesButton = e.NameScope.Find<Button>(PartYesButton);
        _noButton = e.NameScope.Find<Button>(PartNoButton);
        _okButton = e.NameScope.Find<Button>(PartOkButton);
        _cancelButton = e.NameScope.Find<Button>(PartCancelButton);
        Button.ClickEvent.AddHandler(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        SetButtonVisibility();
    }

    private void SetButtonVisibility() => DialogButtonHelper.SetButtonVisibility(Buttons, _okButton, _cancelButton, _yesButton, _noButton);

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

        Close(DialogButtonHelper.GetDefaultCloseResult(Buttons));
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
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
}
