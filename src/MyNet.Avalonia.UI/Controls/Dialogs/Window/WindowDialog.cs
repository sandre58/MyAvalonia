// -----------------------------------------------------------------------
// <copyright file="WindowDialog.cs" company="Stéphane ANDRE">
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
using MyNet.UI.Dialogs.ContentDialogs;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartCloseButton, typeof(Button))]
[TemplatePart(PartTitleArea, typeof(Panel))]
public class WindowDialog : ExtendedWindow
{
    public const string PartCloseButton = "PART_CloseButton";
    public const string PartTitleArea = "PART_TitleArea";

    protected internal Button? CloseButton { get; private set; }

    private Panel? _titleArea;

    static WindowDialog() => DataContextProperty.Changed.AddClassHandler<WindowDialog, object?>((window, e) => window.OnDataContextChange(e));

    public bool CanDragMove { get; set; } = true;

    protected override Type StyleKeyOverride { get; } = typeof(WindowDialog);

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogViewModel oldContext) oldContext.CloseRequest -= OnContextRequestClose;

        if (args.NewValue.Value is IDialogViewModel newContext) newContext.CloseRequest += OnContextRequestClose;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnCloseButtonClicked, CloseButton);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        CloseButton = e.NameScope.Find<Button>(PartCloseButton);
        IsVisibleProperty.SetValue(IsCloseButtonVisible, CloseButton);
        Button.ClickEvent.AddHandler(OnCloseButtonClicked, CloseButton);
        _titleArea = e.NameScope.Find<Panel>(PartTitleArea);
        IsHitTestVisibleProperty.SetValue(CanDragMove, _titleArea);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
    }

    private void OnContextRequestClose(object? sender, object? args) => Close(args);

    protected virtual void OnCloseButtonClicked(object? sender, RoutedEventArgs args)
    {
        if (DataContext is IDialogViewModel context)
            context.Close();
        else
            Close(null);
    }

    private void OnTitlePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (CanDragMove)
            BeginMoveDrag(e);
    }
}
