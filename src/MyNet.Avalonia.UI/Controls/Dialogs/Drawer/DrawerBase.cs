// -----------------------------------------------------------------------
// <copyright file="DrawerBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extensions;
using MyNet.UI.Dialogs.CustomDialogs;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartCloseButton, typeof(Button))]
public abstract class DrawerBase : OverlayFeedbackElement
{
    public const string PartCloseButton = "PART_CloseButton";

    protected internal Button? CloseButton { get; set; }

    public static readonly StyledProperty<Position> PositionProperty =
        AvaloniaProperty.Register<DrawerBase, Position>(
            nameof(Position), defaultValue: Position.Right);

    public static readonly StyledProperty<bool> CanResizeProperty = AvaloniaProperty.Register<DrawerBase, bool>(
        nameof(CanResize));

    public bool CanResize
    {
        get => GetValue(CanResizeProperty);
        set => SetValue(CanResizeProperty, value);
    }

    public Position Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    public static readonly StyledProperty<bool> IsOpenProperty = AvaloniaProperty.Register<DrawerBase, bool>(
        nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    internal bool? IsCloseButtonVisible { get; set; }

    protected internal bool CanLightDismiss { get; set; }

    static DrawerBase() => DataContextProperty.Changed.AddClassHandler<DrawerBase, object?>((o, e) => o.OnDataContextChange(e));

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnCloseButtonClick, CloseButton);
        CloseButton = e.NameScope.Find<Button>(PartCloseButton);
        Button.ClickEvent.AddHandler(OnCloseButtonClick, CloseButton);
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogViewModel oldContext)
        {
            oldContext.CloseRequest -= OnContextRequestClose;
        }

        if (args.NewValue.Value is IDialogViewModel newContext)
        {
            newContext.CloseRequest += OnContextRequestClose;
        }
    }

    private void OnContextRequestClose(object? sender, object? e) => RaiseEvent(new ResultEventArgs(e) { RoutedEvent = ClosedEvent, Source = this });

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e) => Close();

    public override void Close()
    {
        if (DataContext is IDialogViewModel context)
        {
            context.Close();
        }
        else
        {
            RaiseEvent(new ResultEventArgs(ClosedEvent, null));
        }
    }
}
