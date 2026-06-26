// -----------------------------------------------------------------------
// <copyright file="OverlayMessageBox.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Overlay message box shell. Content is rendered via <see cref="MessageBoxContent"/> in <c>ContentTemplate</c>.
/// </summary>
public class OverlayMessageBox : OverlayDialog
{
    public const string PartYesButton = MessageBoxContent.PartYesButton;
    public const string PartNoButton = MessageBoxContent.PartNoButton;
    public const string PartOkButton = MessageBoxContent.PartOkButton;
    public const string PartCancelButton = MessageBoxContent.PartCancelButton;

    public static readonly StyledProperty<MessageSeverity> SeverityProperty =
        AvaloniaProperty.Register<OverlayMessageBox, MessageSeverity>(nameof(Severity));

    public static readonly StyledProperty<MessageBoxResultOption> ButtonsProperty =
        AvaloniaProperty.Register<OverlayMessageBox, MessageBoxResultOption>(nameof(Buttons));

    public static readonly StyledProperty<object?> MessageProperty =
        AvaloniaProperty.Register<OverlayMessageBox, object?>(nameof(Message));

    static OverlayMessageBox()
    {
        MessageProperty.Changed.AddClassHandler<OverlayMessageBox>((messageBox, e) => messageBox.SetCurrentValue(ContentProperty, e.NewValue));
        ButtonsProperty.Changed.AddClassHandler<OverlayMessageBox>((messageBox, _) => messageBox.UpdateCloseButtonVisibility());
        IsCloseButtonVisibleProperty.Changed.AddClassHandler<OverlayMessageBox>((messageBox, _) => messageBox.UpdateCloseButtonVisibility());
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

    public object? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    internal void CloseWithResult(MessageBoxResult result) => OnElementClosing(this, result);

    public override void Close() => CloseWithResult(DialogButtonHelper.GetDefaultCloseResult(Buttons));

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdateCloseButtonVisibility();
    }

    private void UpdateCloseButtonVisibility()
    {
        var closeButtonVisible = IsCloseButtonVisible && Buttons != MessageBoxResultOption.YesNo;
        CloseButton?.IsVisible = closeButtonVisible;
    }
}
