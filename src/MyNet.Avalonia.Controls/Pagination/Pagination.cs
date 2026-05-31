// -----------------------------------------------------------------------
// <copyright file="Pagination.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using MyNet.Avalonia.Commands;
using MyNet.Avalonia.Controls.Internals;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Pagination is a control that displays a series of buttons that can be used to navigate to pages.
/// CurrentPage starts from 1.
/// Pagination only stores an approximate index internally.
/// </summary>
[TemplatePart(PartPreviousButton, typeof(PaginationButton))]
[TemplatePart(PartNextButton, typeof(PaginationButton))]
[TemplatePart(PartButtonPanel, typeof(StackPanel))]
[TemplatePart(PartQuickJumpInput, typeof(NumericUpDown))]
public class Pagination : TemplatedControl
{
    public const string PartPreviousButton = "PART_PreviousButton";
    public const string PartNextButton = "PART_NextButton";
    public const string PartButtonPanel = "PART_ButtonPanel";
    public const string PartQuickJumpInput = "PART_QuickJumpInput";

    public static readonly StyledProperty<int?> CurrentPageProperty = AvaloniaProperty.Register<Pagination, int?>(
        nameof(CurrentPage), coerce: CoerceCurrentPage);

    public static readonly RoutedEvent<ValueChangedEventArgs<int>> CurrentPageChangedEvent =
        RoutedEvent.Register<Pagination, ValueChangedEventArgs<int>>(nameof(CurrentPageChanged),
            RoutingStrategies.Bubble);

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<Pagination, ICommand?>(
        nameof(Command));

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<Pagination, object?>(nameof(CommandParameter));

    public static readonly StyledProperty<ICommand?> SetPageSizeCommandProperty = AvaloniaProperty.Register<Pagination, ICommand?>(
        nameof(SetPageSizeCommand));

    public static readonly StyledProperty<int> TotalCountProperty = AvaloniaProperty.Register<Pagination, int>(
        nameof(TotalCount));

    public static readonly StyledProperty<int> PageSizeProperty = AvaloniaProperty.Register<Pagination, int>(
        nameof(PageSize), 10);

    public static readonly DirectProperty<Pagination, int> PageCountProperty =
        AvaloniaProperty.RegisterDirect<Pagination, int>(
            nameof(PageCount), o => o.PageCount, (o, e) => o.PageCount = e);

    public static readonly StyledProperty<AvaloniaList<int>> PageSizeOptionsProperty =
        AvaloniaProperty.Register<Pagination, AvaloniaList<int>>(
            nameof(PageSizeOptions));

    public static readonly StyledProperty<ControlTheme> PageButtonThemeProperty =
        AvaloniaProperty.Register<Pagination, ControlTheme>(
            nameof(PageButtonTheme));

    public static readonly StyledProperty<bool> ShowPageSizeSelectorProperty =
        AvaloniaProperty.Register<Pagination, bool>(
            nameof(ShowPageSizeSelector));

    public static readonly StyledProperty<bool> ShowQuickJumpProperty = AvaloniaProperty.Register<Pagination, bool>(
        nameof(ShowQuickJump));

    private readonly PaginationButton[] _buttons = new PaginationButton[7];
    private StackPanel? _buttonPanel;
    private PaginationButton? _nextButton;
    private PaginationButton? _previousButton;
    private NumericUpDown? _quickJumpInput;

    static Pagination()
    {
        _ = PageSizeProperty.Changed.AddClassHandler<Pagination, int>((pagination, args) =>
            pagination.OnPageSizeChanged(args));
        _ = CurrentPageProperty.Changed.AddClassHandler<Pagination, int?>((pagination, args) =>
            pagination.UpdateButtonsByCurrentPage(args.NewValue.Value));
        _ = CurrentPageProperty.Changed.AddClassHandler<Pagination, int?>((pagination, args) =>
            pagination.OnCurrentPageChanged(args));
        _ = TotalCountProperty.Changed.AddClassHandler<Pagination, int>((pagination, _) =>
            pagination.UpdateButtonsByCurrentPage(pagination.CurrentPage));
    }

    public Pagination() => SetPageSizeCommand = ActionCommand.Create<int>(pageSize => PageSize = pageSize, pageSize => pageSize > 0);

    public int? CurrentPage
    {
        get => GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public ICommand? SetPageSizeCommand
    {
        get => GetValue(SetPageSizeCommandProperty);
        set => SetValue(SetPageSizeCommandProperty, value);
    }

    public int TotalCount
    {
        get => GetValue(TotalCountProperty);
        set => SetValue(TotalCountProperty, value);
    }

    public int PageSize
    {
        get => GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    public int PageCount
    {
        get;
        private set => SetAndRaise(PageCountProperty, ref field, value);
    }

    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "It's for binding")]
    public AvaloniaList<int> PageSizeOptions
    {
        get => GetValue(PageSizeOptionsProperty);
        set => SetValue(PageSizeOptionsProperty, value);
    }

    public ControlTheme PageButtonTheme
    {
        get => GetValue(PageButtonThemeProperty);
        set => SetValue(PageButtonThemeProperty, value);
    }

    public bool ShowPageSizeSelector
    {
        get => GetValue(ShowPageSizeSelectorProperty);
        set => SetValue(ShowPageSizeSelectorProperty, value);
    }

    public bool ShowQuickJump
    {
        get => GetValue(ShowQuickJumpProperty);
        set => SetValue(ShowQuickJumpProperty, value);
    }

    public static readonly StyledProperty<bool> DisplayCurrentPageInQuickJumperProperty = AvaloniaProperty.Register<Pagination, bool>(
        nameof(DisplayCurrentPageInQuickJumper));

    public bool DisplayCurrentPageInQuickJumper
    {
        get => GetValue(DisplayCurrentPageInQuickJumperProperty);
        set => SetValue(DisplayCurrentPageInQuickJumperProperty, value);
    }

    private static int? CoerceCurrentPage(AvaloniaObject arg1, int? arg2) =>
        arg1 is Pagination pagination
            ? PaginationHelper.CoerceCurrentPage(arg2, pagination.PageCount)
            : arg2;

    private void OnCurrentPageChanged(AvaloniaPropertyChangedEventArgs<int?> args)
    {
        var oldValue = args.GetOldValue<int?>();
        var newValue = args.GetNewValue<int?>();
        var e = new ValueChangedEventArgs<int>(CurrentPageChangedEvent, oldValue, newValue);
        if (DisplayCurrentPageInQuickJumper)
            _quickJumpInput?.SetCurrentValue(NumericUpDown.ValueProperty, newValue);
        RaiseEvent(e);
    }

    /// <summary>
    ///     Raised when the <see cref="CurrentPage" /> changes.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs<int>>? CurrentPageChanged
    {
        add => AddHandler(CurrentPageChangedEvent, value);
        remove => RemoveHandler(CurrentPageChangedEvent, value);
    }

    private void OnPageSizeChanged(AvaloniaPropertyChangedEventArgs<int> args)
    {
        PageCount = PaginationHelper.CalculatePageCount(TotalCount, args.NewValue.Value);
        if (CurrentPage > PageCount) CurrentPage = null;
        UpdateButtonsByCurrentPage(CurrentPage);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Button.ClickEvent.RemoveHandler(OnButtonClick, _previousButton, _nextButton);
        _previousButton = e.NameScope.Find<PaginationButton>(PartPreviousButton);
        _nextButton = e.NameScope.Find<PaginationButton>(PartNextButton);
        _buttonPanel = e.NameScope.Find<StackPanel>(PartButtonPanel);
        Button.ClickEvent.AddHandler(OnButtonClick, _previousButton, _nextButton);

        KeyDownEvent.RemoveHandler(OnQuickJumpInputKeyDown, _quickJumpInput);
        LostFocusEvent.RemoveHandler(OnQuickJumpInputLostFocus, _quickJumpInput);
        _quickJumpInput = e.NameScope.Find<NumericUpDown>(PartQuickJumpInput);
        KeyDownEvent.AddHandler(OnQuickJumpInputKeyDown, _quickJumpInput);
        LostFocusEvent.AddHandler(OnQuickJumpInputLostFocus, _quickJumpInput);

        InitializePanelButtons();
        UpdateButtonsByCurrentPage(0);
    }

    private void OnQuickJumpInputKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key is Key.Enter) SyncQuickJumperValue();
    }

    private void OnQuickJumpInputLostFocus(object? sender, RoutedEventArgs e) => SyncQuickJumperValue();

    private void SyncQuickJumperValue()
    {
        var value = _quickJumpInput?.Value;
        if (value is null) return;
        value = PaginationHelper.ClampQuickJump(value.Value, PageCount);
        SetCurrentValue(CurrentPageProperty, (int)value);
        if (!DisplayCurrentPageInQuickJumper)
            _quickJumpInput?.SetCurrentValue(NumericUpDown.ValueProperty, null);
        InvokeCommand();
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        var diff = Equals(sender, _previousButton) ? -1 : 1;
        AddCurrentPage(diff);
        InvokeCommand();
    }

    private void InitializePanelButtons()
    {
        if (_buttonPanel is null) return;
        _buttonPanel.Children.Clear();
        for (var i = 1; i <= 7; i++)
        {
            var button = new PaginationButton
            {
                Page = i,
                IsVisible = true,
                Theme = PageButtonTheme
            };
            _buttonPanel.Children.Add(button);
            _buttons[i - 1] = button;
            Button.ClickEvent.AddHandler(OnPageButtonClick, button);
        }
    }

    private void OnPageButtonClick(object? sender, RoutedEventArgs args)
    {
        if (sender is PaginationButton pageButton)
        {
            if (pageButton.IsFastForward)
                AddCurrentPage(-5);
            else if (pageButton.IsFastBackward)
                AddCurrentPage(5);
            else
                CurrentPage = pageButton.Page;
        }

        InvokeCommand();
    }

    private void AddCurrentPage(int pageChange)
    {
        var newValue = PaginationHelper.AddPageOffset(CurrentPage ?? 0, pageChange, PageCount);
        SetCurrentValue(CurrentPageProperty, newValue);
    }

    private void UpdateButtonsByCurrentPage(int? page)
    {
        if (PageSize == 0) return;

        var pageCount = PaginationHelper.CalculatePageCount(TotalCount, PageSize);
        if (_buttonPanel is null)
        {
            SetCurrentValue(PageCountProperty, pageCount);
            SetCurrentValue(CurrentPageProperty, page);
            return;
        }

        ApplyButtonStates(PaginationLayoutHelper.BuildButtonStates(page, pageCount));

        SetCurrentValue(PageCountProperty, pageCount);
        SetCurrentValue(CurrentPageProperty, page);

        var (previousEnabled, nextEnabled) = PaginationHelper.GetNavigationState(CurrentPage, pageCount);
        _previousButton?.IsEnabled = previousEnabled;
        _nextButton?.IsEnabled = nextEnabled;
    }

    private void ApplyButtonStates(PaginationButtonState[] states)
    {
        for (var i = 0; i < PaginationLayoutHelper.ButtonSlotCount; i++)
        {
            var state = states[i];
            var button = _buttons[i];

            if (!state.IsVisible)
            {
                button.IsVisible = false;
                continue;
            }

            button.IsVisible = true;
            button.SetStatus(state.Page, state.IsSelected, state.IsLeftEllipsis, state.IsRightEllipsis);
        }
    }

    private void InvokeCommand()
    {
        if (Command?.CanExecute(CommandParameter) == true) Command.Execute(CommandParameter);
    }
}
