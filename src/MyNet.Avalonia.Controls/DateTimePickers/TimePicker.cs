// -----------------------------------------------------------------------
// <copyright file="TimePicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartPresenter, typeof(TimePickerPresenter))]
[TemplatePart(PartTextBox, typeof(Button))]
public class TimePicker : TimePickerBase
{
    public const string PartTextBox = "PART_TextBox";
    public const string PartPresenter = "PART_Presenter";
    public const string PartButton = "PART_Button";
    public const string PartPopup = "PART_Popup";

    public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePicker, TimeSpan?>(
            nameof(SelectedTime), defaultBindingMode: BindingMode.TwoWay);

    private Button? _button;

    private bool _isFocused;
    private Popup? _popup;
    private TimePickerPresenter? _presenter;

    private bool _suppressTextPresenterEvent;
    private TextBox? _textBox;

    static TimePicker()
    {
        FocusableProperty.OverrideDefaultValue<TimePicker>(true);
        _ = SelectedTimeProperty.Changed.AddClassHandler<TimePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args));
        _ = DisplayFormatProperty.Changed.AddClassHandler<TimePicker, string?>((picker, _) =>
            picker.OnDisplayFormatChanged());
    }

    public TimeSpan? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public void Clear()
    {
        _ = Focus(NavigationMethod.Pointer);
        _presenter?.SyncTime(null);
    }

    private void OnDisplayFormatChanged()
    {
        if (_textBox is null) return;
        SyncTimeToText(SelectedTime);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _textBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnPresenterTimeChanged, _presenter);

        _textBox = e.NameScope.Find<TextBox>(PartTextBox);
        _popup = e.NameScope.Find<Popup>(PartPopup);
        _presenter = e.NameScope.Find<TimePickerPresenter>(PartPresenter);
        _button = e.NameScope.Find<Button>(PartButton);

        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _textBox);
        Button.ClickEvent.AddHandler(OnButtonClick, _button);
        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnPresenterTimeChanged, _presenter);

        _presenter?.SyncTime(SelectedTime);
        SyncTimeToText(SelectedTime);
        SyncTimeToText(SelectedTime);
    }

    private void OnPresenterTimeChanged(object? sender, TimeChangedEventArgs e)
    {
        if (!IsInitialized) return;
        if (_suppressTextPresenterEvent) return;
        SetCurrentValue(SelectedTimeProperty, e.NewTime);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (IsFocused)
        {
            SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
        }
    }

    private void OnTextBoxGetFocus(object? sender, GotFocusEventArgs e) => SetCurrentValue(IsDropDownOpenProperty, true);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Down)
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Tab)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
            return;
        }

        if (e.Key == Key.Enter)
        {
            CommitInput(true);
            SetCurrentValue(IsDropDownOpenProperty, false);
            e.Handled = true;
            return;
        }

        base.OnKeyDown(e);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(_textBox?.Text))
        {
            _presenter?.SyncTime(null);
        }
        else if (string.IsNullOrEmpty(DisplayFormat))
        {
            if (TimeSpan.TryParse(_textBox?.Text, out var defaultTime))
            {
                _presenter?.SyncTime(defaultTime);
            }
        }
        else
        {
            CommitInput(false);
        }
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<TimeSpan?> args)
    {
        if (_textBox is null) return;
        _suppressTextPresenterEvent = true;
        _presenter?.SyncTime(args.NewValue.Value);
        SyncTimeToText(args.NewValue.Value);
        _suppressTextPresenterEvent = false;
    }

    private void SyncTimeToText(TimeSpan? time)
    {
        if (_textBox is null) return;
        if (time is null)
        {
            _textBox.Text = null;
            return;
        }

        var date = new DateTime(1, 1, 1, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
        var text = date.ToString(DisplayFormat, CultureInfo.CurrentCulture);
        _textBox.Text = text;
    }

    public void Confirm()
    {
        _presenter?.Confirm();
        SetCurrentValue(IsDropDownOpenProperty, false);
        _ = Focus();
    }

    public void Dismiss()
    {
        SetCurrentValue(IsDropDownOpenProperty, false);
        _ = Focus();
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedTimeProperty) DataValidationErrors.SetError(this, error);
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        FocusChanged(IsKeyboardFocusWithin);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        FocusChanged(IsKeyboardFocusWithin);
        var top = TopLevel.GetTopLevel(this);
        var element = top?.FocusManager?.GetFocusedElement();
        if (element is Visual v && _popup?.IsInsidePopup(v) == true) return;
        if (element == _textBox) return;
        CommitInput(true);
        SetCurrentValue(IsDropDownOpenProperty, false);
    }

    private void FocusChanged(bool hasFocus)
    {
        var wasFocused = _isFocused;
        _isFocused = hasFocus;

        if (hasFocus)
        {
            if (!wasFocused && _textBox != null)
            {
                _ = _textBox.Focus();
                _textBox.SelectAll();
            }
        }
    }

    private void CommitInput(bool clearWhenInvalid)
    {
        if (DateTime.TryParseExact(_textBox?.Text, DisplayFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var time))
        {
            SetCurrentValue(SelectedTimeProperty, time.TimeOfDay);
            _presenter?.SyncTime(time.TimeOfDay);
        }
        else
        {
            if (clearWhenInvalid)
            {
                SetCurrentValue(SelectedTimeProperty, null);
            }

            _presenter?.SyncTime(null);
        }
    }
}
