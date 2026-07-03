// -----------------------------------------------------------------------
// <copyright file="Rating.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Styling;
using Material.Icons;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Internals.Rating;
using MyNet.Avalonia.Controls.Localization;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A visual rating control that displays and optionally edits a numeric score using customizable symbols.
/// </summary>
[TemplatePart(PartItemsPanel, typeof(Panel))]
[PseudoClasses(PseudoClassName.Horizontal, PseudoClassName.Vertical, PseudoClassName.ReadOnly, PseudoClassName.Empty)]
public partial class Rating : TemplatedControl
{
    public const string PartItemsPanel = "PART_ItemsPanel";

    private readonly List<RatingItem> _items = [];
    private Panel? _itemsPanel;
    private double? _previewValue;

    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Rating, double>(
            nameof(Value),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay,
            coerce: CoerceValue);

    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<Rating, double>(nameof(Minimum));

    public static readonly StyledProperty<int> MaxRatingProperty =
        AvaloniaProperty.Register<Rating, int>(nameof(MaxRating), 5, coerce: CoerceMaxRating);

    public static readonly StyledProperty<RatingPrecision> PrecisionProperty =
        AvaloniaProperty.Register<Rating, RatingPrecision>(nameof(Precision), RatingPrecision.Integer);

    public static readonly StyledProperty<bool> IsClearableProperty =
        AvaloniaProperty.Register<Rating, bool>(nameof(IsClearable), true);

    public static readonly StyledProperty<bool> ClearOnReselectProperty =
        AvaloniaProperty.Register<Rating, bool>(nameof(ClearOnReselect));

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<Rating, bool>(nameof(IsReadOnly));

    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<Rating, Orientation>(nameof(Orientation), Orientation.Horizontal);

    public static readonly StyledProperty<double> ItemSizeProperty =
        AvaloniaProperty.Register<Rating, double>(nameof(ItemSize), 24);

    public static readonly StyledProperty<ControlTheme?> ItemThemeProperty =
        AvaloniaProperty.Register<Rating, ControlTheme?>(nameof(ItemTheme));

    public static readonly StyledProperty<object?> EmptySymbolProperty =
        AvaloniaProperty.Register<Rating, object?>(nameof(EmptySymbol), MaterialIconKind.Star);

    public static readonly StyledProperty<object?> FilledSymbolProperty =
        AvaloniaProperty.Register<Rating, object?>(nameof(FilledSymbol));

    public static readonly DirectProperty<Rating, object?> EffectiveFilledSymbolProperty =
        AvaloniaProperty.RegisterDirect<Rating, object?>(nameof(EffectiveFilledSymbol), o => o.GetEffectiveFilledSymbol());

    public static readonly RoutedEvent<ValueChangedEventArgs<double>> ValueChangedEvent =
        RoutedEvent.Register<Rating, ValueChangedEventArgs<double>>(nameof(ValueChanged), RoutingStrategies.Bubble);

    static Rating()
    {
        FocusableProperty.OverrideDefaultValue<Rating>(true);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<Rating>(AutomationControlType.Slider);

        ValueProperty.Changed.AddClassHandler<Rating>((rating, e) => rating.OnValueChanged(e));
        MinimumProperty.Changed.AddClassHandler<Rating>((rating, _) => rating.OnRangeChanged());
        MaxRatingProperty.Changed.AddClassHandler<Rating>((rating, _) => rating.OnMaxRatingChanged());
        IsClearableProperty.Changed.AddClassHandler<Rating>((rating, _) => rating.OnRangeChanged());
        IsReadOnlyProperty.Changed.AddClassHandler<Rating>((rating, _) => rating.UpdateInteractionState());
        OrientationProperty.Changed.AddClassHandler<Rating>((rating, _) => rating.UpdateOrientationState());
        ItemThemeProperty.Changed.AddClassHandler<Rating>((rating, _) => rating.UpdateItemThemes());
        ItemSizeProperty.Changed.AddClassHandler<Rating>((rating, _) => rating.UpdateItemSizes());
    }

    public Rating() => UpdateOrientationState();

    /// <summary>
    /// Gets or sets the current rating value.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "GetValue is in base class")]
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum allowed rating value.
    /// </summary>
    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum number of visual rating items.
    /// </summary>
    public int MaxRating
    {
        get => GetValue(MaxRatingProperty);
        set => SetValue(MaxRatingProperty, value);
    }

    /// <summary>
    /// Gets or sets how input values are quantized during editing.
    /// </summary>
    public RatingPrecision Precision
    {
        get => GetValue(PrecisionProperty);
        set => SetValue(PrecisionProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the rating can be cleared to <see cref="Minimum"/>.
    /// </summary>
    public bool IsClearable
    {
        get => GetValue(IsClearableProperty);
        set => SetValue(IsClearableProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether clicking the current value clears the rating.
    /// </summary>
    public bool ClearOnReselect
    {
        get => GetValue(ClearOnReselectProperty);
        set => SetValue(ClearOnReselectProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the rating is read-only.
    /// </summary>
    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    /// Gets or sets the orientation of the rating items.
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the width and height of each rating item.
    /// </summary>
    public double ItemSize
    {
        get => GetValue(ItemSizeProperty);
        set => SetValue(ItemSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the theme applied to each <see cref="RatingItem"/>.
    /// </summary>
    public ControlTheme? ItemTheme
    {
        get => GetValue(ItemThemeProperty);
        set => SetValue(ItemThemeProperty, value);
    }

    /// <summary>
    /// Gets or sets the symbol displayed for empty rating slots.
    /// Accepts a <see cref="MaterialIconKind"/>, a control, or a data template.
    /// </summary>
    public object? EmptySymbol
    {
        get => GetValue(EmptySymbolProperty);
        set => SetValue(EmptySymbolProperty, value);
    }

    /// <summary>
    /// Gets or sets the symbol displayed for filled rating slots.
    /// When <see langword="null"/>, <see cref="EmptySymbol"/> is reused and states are differentiated by color only.
    /// </summary>
    public object? FilledSymbol
    {
        get => GetValue(FilledSymbolProperty);
        set => SetValue(FilledSymbolProperty, value);
    }

    /// <summary>
    /// Gets the symbol used for the filled layer (<see cref="FilledSymbol"/> or <see cref="EmptySymbol"/>).
    /// </summary>
    public object? EffectiveFilledSymbol => GetEffectiveFilledSymbol();

    /// <summary>
    /// Raised when <see cref="Value"/> changes.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs<double>>? ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    internal IReadOnlyList<RatingItem> Items => _items;

    private static double CoerceValue(AvaloniaObject sender, double value) => sender is not Rating rating ? value : value.SafeClamp(rating.Minimum, rating.MaxRating);

    private static int CoerceMaxRating(AvaloniaObject sender, int value) =>
        Math.Max(1, value);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PointerExitedEvent.RemoveHandler(OnPointerExited, this);
        _itemsPanel = e.NameScope.Find<Panel>(PartItemsPanel);
        PointerExitedEvent.AddHandler(OnPointerExited, this);

        RebuildItems();
        UpdateItemStates();
        UpdateInteractionState();
        UpdateAutomationName();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsEnabledProperty)
            UpdateInteractionState();
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        ClearPreview();
        base.OnLostFocus(e);
    }

    private void OnValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        UpdateEmptyState();
        UpdateItemStates();
        UpdateAutomationName();

        var oldValue = e.GetOldValue<double>();
        var newValue = e.GetNewValue<double>();
        if (!oldValue.Equals(newValue))
            RaiseEvent(new ValueChangedEventArgs<double>(ValueChangedEvent, oldValue, newValue));
    }

    private void OnRangeChanged()
    {
        var effectiveMinimum = GetEffectiveMinimum();
        if (Value < effectiveMinimum)
            SetCurrentValue(ValueProperty, effectiveMinimum);
        else if (Value > MaxRating)
            SetCurrentValue(ValueProperty, MaxRating);

        UpdateItemStates();
        UpdateAutomationName();
    }

    private void OnMaxRatingChanged()
    {
        RebuildItems();
        OnRangeChanged();
    }

    private void RebuildItems()
    {
        if (_itemsPanel is null)
            return;

        _itemsPanel.Children.Clear();
        _items.Clear();

        for (var i = 1; i <= MaxRating; i++)
        {
            var item = new RatingItem
            {
                Index = i,
                Owner = this,
                Theme = ItemTheme,
                IconSize = ItemSize,
                IsEnabled = IsEnabled,
                IsHitTestVisible = CanEdit()
            };
            item.UpdateAutomationName();
            item.UpdateOrientationPseudoClasses(Orientation == Orientation.Horizontal, Orientation == Orientation.Vertical);
            _itemsPanel.Children.Add(item);
            _items.Add(item);
        }
    }

    private void UpdateItemStates()
    {
        foreach (var item in _items)
            item.ApplyVisualState(RatingItemStateCalculator.Calculate(item.Index, Value, _previewValue));
    }

    private void UpdateItemThemes()
    {
        foreach (var item in _items)
            item.Theme = ItemTheme;
    }

    private void UpdateItemSizes()
    {
        foreach (var item in _items)
            item.IconSize = ItemSize;
    }

    private void UpdateOrientationState()
    {
        var isHorizontal = Orientation == Orientation.Horizontal;
        var isVertical = Orientation == Orientation.Vertical;
        PseudoClasses.Set(PseudoClassName.Horizontal, isHorizontal);
        PseudoClasses.Set(PseudoClassName.Vertical, isVertical);

        if (_itemsPanel is StackPanel stackPanel)
            stackPanel.Orientation = Orientation;

        foreach (var item in _items)
            item.UpdateOrientationPseudoClasses(isHorizontal, isVertical);
    }

    private void UpdateInteractionState()
    {
        var readOnly = IsReadOnly;
        var canEdit = CanEdit();
        PseudoClasses.Set(PseudoClassName.ReadOnly, readOnly);
        Focusable = !readOnly;
        IsHitTestVisible = canEdit;

        foreach (var item in _items)
        {
            item.IsEnabled = IsEnabled;
            item.IsHitTestVisible = canEdit;
        }

        ClearPreview();
    }

    private object? GetEffectiveFilledSymbol() => FilledSymbol ?? EmptySymbol;

    private void UpdateEmptyState() =>
        PseudoClasses.Set(PseudoClassName.Empty, Value <= Minimum);

    private void UpdateAutomationName()
    {
        AutomationProperties.SetName(this, string.Format(
            System.Globalization.CultureInfo.CurrentCulture,
            RatingResources.AutomationName,
            Value,
            MaxRating));

        AutomationProperties.SetControlTypeOverride(
            this,
            IsReadOnly ? AutomationControlType.Text : AutomationControlType.Slider);
    }

    private double GetEffectiveMinimum() =>
        RatingValueHelper.GetEffectiveMinimum(IsClearable, Minimum);

    private double GetEffectiveMaximum() => MaxRating;

    private bool CanEdit() => !IsReadOnly && IsEnabled;

    internal void HandleItemPointerMoved(RatingItem item, PointerEventArgs e)
    {
        if (!CanEdit())
            return;

        var fraction = GetPointerFraction(item, e);
        var candidate = RatingValueHelper.ValueFromItemPosition(
            item.Index,
            fraction,
            Precision,
            GetEffectiveMinimum(),
            GetEffectiveMaximum());

        SetPreviewValue(candidate);
    }

    internal void HandleItemPointerPressed(RatingItem item, PointerPressedEventArgs e)
    {
        if (!CanEdit())
            return;

        var fraction = GetPointerFraction(item, e);
        var candidate = RatingValueHelper.ValueFromItemPosition(
            item.Index,
            fraction,
            Precision,
            GetEffectiveMinimum(),
            GetEffectiveMaximum());

        if (ClearOnReselect && Math.Abs(candidate - Value) < double.Epsilon)
            candidate = GetEffectiveMinimum();

        CommitValue(candidate);
        ClearPreview();
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        if (!IsPointerOver)
            ClearPreview();
    }

    private void ClearPreview()
    {
        if (!_previewValue.HasValue)
            return;

        _previewValue = null;
        UpdateItemStates();
    }

    private double GetActiveEditValue() => _previewValue ?? Value;

    private void SetPreviewValue(double candidate)
    {
        candidate = candidate.SafeClamp(GetEffectiveMinimum(), GetEffectiveMaximum());

        if (_previewValue is { } current && Math.Abs(current - candidate) < double.Epsilon)
            return;

        _previewValue = candidate;
        UpdateItemStates();
    }

    private void CommitPreview()
    {
        if (!_previewValue.HasValue)
            return;

        CommitValue(_previewValue.Value);
        ClearPreview();
    }

    private double GetPointerFraction(RatingItem item, PointerEventArgs e)
    {
        if (item.TryGetPointerFraction(e, Orientation == Orientation.Horizontal, out var fraction))
            return fraction;

        var position = e.GetPosition(item);
        var padding = item.Padding;
        return RatingValueHelper.GetPointerFractionInContent(
            Orientation == Orientation.Horizontal,
            ItemSize,
            item.Bounds.Width,
            item.Bounds.Height,
            padding.Left,
            padding.Top,
            padding.Right,
            padding.Bottom,
            position.X,
            position.Y);
    }

    private void CommitValue(double newValue)
    {
        newValue = newValue.SafeClamp(GetEffectiveMinimum(), GetEffectiveMaximum());
        SetCurrentValue(ValueProperty, newValue);
    }
}
