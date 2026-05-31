// -----------------------------------------------------------------------
// <copyright file="DropDownControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls;

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[PseudoClasses(PseudoClassName.FlyoutOpen)]
public class DropDownControl : TemplatedControl, IPopupControl
{
    public const string PartButton = "PART_Button";
    public const string PartPopup = "PART_Popup";

    private readonly CompositeDisposable _subscriptionsOnOpen = [];

    private Button? _button;
    private Popup? _popup;

    public event EventHandler? DropDownClosed;

    public event EventHandler? DropDownOpened;

    static DropDownControl()
    {
        FocusableProperty.OverrideDefaultValue<DropDownControl>(true);
        IsDropDownOpenProperty.AffectsPseudoClass<DropDownControl>(PseudoClassName.FlyoutOpen);
        KeyboardNavigation.TabNavigationProperty.OverrideDefaultValue<DropDownControl>(KeyboardNavigationMode.Once);
    }

    private static bool CanFocus(Control control) => control is { Focusable: true, IsEffectivelyEnabled: true, IsVisible: true };

    #region IsDropDownOpen

    public static readonly StyledProperty<bool> IsDropDownOpenProperty = AvaloniaProperty.Register<DropDownControl, bool>(nameof(IsDropDownOpen), defaultBindingMode: BindingMode.TwoWay);

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);

        if (_popup != null)
        {
            _popup.Opened -= OnPopupOpened;
            _popup.Closed -= OnPopupClosed;
        }

        _button = e.NameScope.Find<Button>(PartButton);
        _popup = e.NameScope.Find<Popup>(PartPopup);

        if (_popup != null)
        {
            _popup.Opened += OnPopupOpened;
            _popup.Closed += OnPopupClosed;
        }

        Button.ClickEvent.AddHandler(OnButtonClick, RoutingStrategies.Bubble, true, _button);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        // IsDropDownOpen
        if (change.Property == IsDropDownOpenProperty)
        {
            UpdatePseudoClasses();
        }

        base.OnPropertyChanged(change);
    }

    protected void UpdatePseudoClasses() => PseudoClasses.Set(PseudoClassName.FlyoutOpen, IsDropDownOpen);

    #region Focus
    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);

        ClosePopup();
    }

    #endregion

    #region Keyboard handlers

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Handled) return;

        switch (e.Key)
        {
            case Key.Enter:
                OpenPopup();

                break;

            case Key.Escape:
                ClosePopup();

                break;

            case Key.Down:
            case Key.Up:
                if (e.KeyModifiers.HasFlag(KeyModifiers.Alt))
                    OpenPopup();

                break;
        }
    }

    #endregion

    #region Mouse handlers

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        _ = Focus(NavigationMethod.Pointer);
        TogglePopup();
    }

    #endregion

    #region Popup handlers

    private void OnPopupClosed(object? sender, EventArgs e)
    {
        _subscriptionsOnOpen.Clear();

        if (CanFocus(this))
        {
            Focus();
        }

        DropDownClosed?.Invoke(this, EventArgs.Empty);
    }

    private void OnPopupOpened(object? sender, EventArgs e)
    {
        TryFocusPopupContent();

        _subscriptionsOnOpen.Clear();

        this.GetObservable(IsVisibleProperty).Subscribe(IsVisibleChanged).DisposeWith(_subscriptionsOnOpen);

        foreach (var parent in this.GetVisualAncestors().OfType<Control>())
        {
            parent.GetObservable(IsVisibleProperty).Subscribe(IsVisibleChanged).DisposeWith(_subscriptionsOnOpen);
        }

        DropDownOpened?.Invoke(this, EventArgs.Empty);
    }

    private void IsVisibleChanged(bool isVisible)
    {
        if (!isVisible)
            ClosePopup();
    }

    #endregion

    public void TogglePopup()
    {
        if (IsDropDownOpen)
        {
            Focus();
            ClosePopup();
        }
        else
        {
            OpenPopup();
        }
    }

    public void OpenPopup() => IsDropDownOpen.IfFalse(() => SetCurrentValue(IsDropDownOpenProperty, true));

    public void ClosePopup() => IsDropDownOpen.IfTrue(() => SetCurrentValue(IsDropDownOpenProperty, false));

    private void TryFocusPopupContent()
    {
        if (IsDropDownOpen)
        {
            var focusable = _popup?.Child?.GetVisualDescendants().OfType<Control>().FirstOrDefault(CanFocus);
            focusable?.Focus();
        }
    }
}
