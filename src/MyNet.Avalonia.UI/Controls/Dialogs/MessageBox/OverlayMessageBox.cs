// -----------------------------------------------------------------------
// <copyright file="OverlayMessageBox.cs" company="Stéphane ANDRE">
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
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
///     The messageBox used to display in OverlayDialogHost.
/// </summary>
[TemplatePart(PartNoButton, typeof(Button))]
[TemplatePart(PartOKButton, typeof(Button))]
[TemplatePart(PartCancelButton, typeof(Button))]
[TemplatePart(PartYesButton, typeof(Button))]
public class OverlayMessageBox : OverlayDialogBase
{
    public const string PartYesButton = "PART_YesButton";
    public const string PartNoButton = "PART_NoButton";
    public const string PartOKButton = "PART_OKButton";
    public const string PartCancelButton = "PART_CancelButton";

    #region Severity

    /// <summary>
    /// Provides Severity Property.
    /// </summary>
    public static readonly StyledProperty<MessageSeverity> SeverityProperty = AvaloniaProperty.Register<OverlayMessageBox, MessageSeverity>(nameof(Severity));

    /// <summary>
    /// Gets or sets the Severity property.
    /// </summary>
    public MessageSeverity Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    #endregion

    public static readonly StyledProperty<MessageBoxResultOption> ButtonsProperty = AvaloniaProperty.Register<OverlayMessageBox, MessageBoxResultOption>(nameof(Buttons));

    public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<OverlayMessageBox, string?>(nameof(Title));

    private Button? _cancelButton;
    private Button? _noButton;
    private Button? _okButton;

    private Button? _yesButton;

    static OverlayMessageBox() => ButtonsProperty.Changed.AddClassHandler<OverlayMessageBox>((o, _) => o.SetButtonVisibility());

    public MessageBoxResultOption Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var defaultButton = Buttons switch
        {
            MessageBoxResultOption.Ok => _okButton,
            MessageBoxResultOption.OkCancel => _cancelButton,
            MessageBoxResultOption.YesNo => _yesButton,
            MessageBoxResultOption.YesNoCancel => _cancelButton,
            MessageBoxResultOption.None => null,
            _ => null
        };
        _ = defaultButton?.Focus();
    }

    private void DefaultButtonsClose(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        var result = button switch
        {
            _ when button == _okButton => MessageBoxResult.Ok,
            _ when button == _cancelButton => MessageBoxResult.Cancel,
            _ when button == _yesButton => MessageBoxResult.Yes,
            _ when button == _noButton => MessageBoxResult.No,
            _ => MessageBoxResult.None
        };
        OnElementClosing(this, result);
    }

    private void SetButtonVisibility()
    {
        var closeButtonVisible = Buttons != MessageBoxResultOption.YesNo;
        IsVisibleProperty.SetValue(closeButtonVisible, CloseButton);
        switch (Buttons)
        {
            case MessageBoxResultOption.Ok:
                IsVisibleProperty.SetValue(true, _okButton);
                IsVisibleProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                Button.IsDefaultProperty.SetValue(true, _okButton);
                Button.IsDefaultProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                break;
            case MessageBoxResultOption.OkCancel:
                IsVisibleProperty.SetValue(true, _okButton, _cancelButton);
                IsVisibleProperty.SetValue(false, _yesButton, _noButton);
                Button.IsDefaultProperty.SetValue(true, _okButton);
                Button.IsDefaultProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
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
                break;
            default:
                break;
        }
    }

    public override void Close()
    {
        var result = Buttons switch
        {
            MessageBoxResultOption.Ok => MessageBoxResult.Ok,
            MessageBoxResultOption.OkCancel => MessageBoxResult.Cancel,
            MessageBoxResultOption.YesNo => MessageBoxResult.No,
            MessageBoxResultOption.YesNoCancel => MessageBoxResult.Cancel,
            MessageBoxResultOption.None => MessageBoxResult.None,
            _ => MessageBoxResult.None
        };
        OnElementClosing(this, result);
    }
}
