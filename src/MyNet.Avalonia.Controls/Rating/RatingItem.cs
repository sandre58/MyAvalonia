// -----------------------------------------------------------------------
// <copyright file="RatingItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Internals.Rating;
using MyNet.Avalonia.Controls.Localization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A single visual slot in a <see cref="Rating"/> control.
/// </summary>
[TemplatePart(PartSymbolHost, typeof(Panel))]
[PseudoClasses(
    PseudoClassName.Empty,
    PseudoClassName.Partial,
    PseudoClassName.Full,
    PseudoClassName.Preview,
    PseudoClassName.PreviewExtend,
    PseudoClassName.PreviewHold,
    PseudoClassName.PreviewRetract,
    PseudoClassName.PreviewSplit,
    PseudoClassName.Horizontal,
    PseudoClassName.Vertical)]
public class RatingItem : TemplatedControl
{
    public const string PartSymbolHost = "PART_SymbolHost";

    private Panel? _symbolHost;

    static RatingItem()
    {
        FocusableProperty.OverrideDefaultValue<RatingItem>(false);
        IsHitTestVisibleProperty.OverrideDefaultValue<RatingItem>(true);
    }

    #region Index

    public static readonly DirectProperty<RatingItem, int> IndexProperty =
        AvaloniaProperty.RegisterDirect<RatingItem, int>(nameof(Index), o => o.Index);

    public int Index
    {
        get;
        internal set => SetAndRaise(IndexProperty, ref field, value);
    }

    #endregion

    #region FillRatio

    public static readonly StyledProperty<double> FillRatioProperty =
        AvaloniaProperty.Register<RatingItem, double>(nameof(FillRatio));

    public double FillRatio
    {
        get => GetValue(FillRatioProperty);
        internal set => SetValue(FillRatioProperty, value);
    }

    #endregion

    #region PreviewFillRatio

    public static readonly StyledProperty<double> PreviewFillRatioProperty =
        AvaloniaProperty.Register<RatingItem, double>(nameof(PreviewFillRatio));

    public double PreviewFillRatio
    {
        get => GetValue(PreviewFillRatioProperty);
        internal set => SetValue(PreviewFillRatioProperty, value);
    }

    #endregion

    #region IsPreview

    public static readonly StyledProperty<bool> IsPreviewProperty =
        AvaloniaProperty.Register<RatingItem, bool>(nameof(IsPreview));

    public bool IsPreview
    {
        get => GetValue(IsPreviewProperty);
        internal set => SetValue(IsPreviewProperty, value);
    }

    #endregion

    #region IsPreviewExtend

    public static readonly StyledProperty<bool> IsPreviewExtendProperty =
        AvaloniaProperty.Register<RatingItem, bool>(nameof(IsPreviewExtend));

    public bool IsPreviewExtend
    {
        get => GetValue(IsPreviewExtendProperty);
        internal set => SetValue(IsPreviewExtendProperty, value);
    }

    #endregion

    #region IsPreviewHold

    public static readonly StyledProperty<bool> IsPreviewHoldProperty =
        AvaloniaProperty.Register<RatingItem, bool>(nameof(IsPreviewHold));

    public bool IsPreviewHold
    {
        get => GetValue(IsPreviewHoldProperty);
        internal set => SetValue(IsPreviewHoldProperty, value);
    }

    #endregion

    #region IsPreviewRetract

    public static readonly StyledProperty<bool> IsPreviewRetractProperty =
        AvaloniaProperty.Register<RatingItem, bool>(nameof(IsPreviewRetract));

    public bool IsPreviewRetract
    {
        get => GetValue(IsPreviewRetractProperty);
        internal set => SetValue(IsPreviewRetractProperty, value);
    }

    #endregion

    #region RetractFillRatio

    public static readonly StyledProperty<double> RetractFillRatioProperty =
        AvaloniaProperty.Register<RatingItem, double>(nameof(RetractFillRatio));

    public double RetractFillRatio
    {
        get => GetValue(RetractFillRatioProperty);
        internal set => SetValue(RetractFillRatioProperty, value);
    }

    #endregion

    #region FilledClipRatio

    public static readonly StyledProperty<double> FilledClipRatioProperty =
        AvaloniaProperty.Register<RatingItem, double>(nameof(FilledClipRatio));

    public double FilledClipRatio
    {
        get => GetValue(FilledClipRatioProperty);
        internal set => SetValue(FilledClipRatioProperty, value);
    }

    #endregion

    #region FilledClipOffsetRatio

    public static readonly StyledProperty<double> FilledClipOffsetRatioProperty =
        AvaloniaProperty.Register<RatingItem, double>(nameof(FilledClipOffsetRatio));

    public double FilledClipOffsetRatio
    {
        get => GetValue(FilledClipOffsetRatioProperty);
        internal set => SetValue(FilledClipOffsetRatioProperty, value);
    }

    #endregion

    #region FilledSymbolOffsetRatio

    public static readonly StyledProperty<double> FilledSymbolOffsetRatioProperty =
        AvaloniaProperty.Register<RatingItem, double>(nameof(FilledSymbolOffsetRatio));

    public double FilledSymbolOffsetRatio
    {
        get => GetValue(FilledSymbolOffsetRatioProperty);
        internal set => SetValue(FilledSymbolOffsetRatioProperty, value);
    }

    #endregion

    #region IsPreviewSplit

    public static readonly StyledProperty<bool> IsPreviewSplitProperty =
        AvaloniaProperty.Register<RatingItem, bool>(nameof(IsPreviewSplit));

    public bool IsPreviewSplit
    {
        get => GetValue(IsPreviewSplitProperty);
        internal set => SetValue(IsPreviewSplitProperty, value);
    }

    #endregion

    #region IconSize

    public static readonly StyledProperty<double> IconSizeProperty =
        AvaloniaProperty.Register<RatingItem, double>(nameof(IconSize), 18.0d);

    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    #endregion

    internal Rating? Owner { get; set; }

    internal bool TryGetPointerFraction(PointerEventArgs e, bool isHorizontal, out double fraction)
    {
        if (_symbolHost is null)
        {
            fraction = 1;
            return false;
        }

        var position = e.GetPosition(_symbolHost);
        fraction = RatingValueHelper.GetPointerFraction(
            isHorizontal,
            _symbolHost.Bounds.Width,
            _symbolHost.Bounds.Height,
            position.X,
            position.Y);
        return true;
    }

    internal void ApplyVisualState(in RatingItemVisualState state)
    {
        var filledClipRatio = state.IsPreviewSplit ? state.RetractFillRatio : state.FillRatio;
        var filledClipOffsetRatio = state.IsPreviewSplit ? state.PreviewFillRatio : 0;
        var filledSymbolOffsetRatio = state.IsPreviewSplit ? -filledClipOffsetRatio : 0;

        if (FillRatio.Equals(state.FillRatio)
            && PreviewFillRatio.Equals(state.PreviewFillRatio)
            && RetractFillRatio.Equals(state.RetractFillRatio)
            && FilledClipRatio.Equals(filledClipRatio)
            && FilledClipOffsetRatio.Equals(filledClipOffsetRatio)
            && FilledSymbolOffsetRatio.Equals(filledSymbolOffsetRatio)
            && IsPreviewExtend == state.IsPreviewExtend
            && IsPreviewHold == state.IsPreviewHold
            && IsPreviewRetract == state.IsPreviewRetract
            && IsPreviewSplit == state.IsPreviewSplit)
            return;

        FillRatio = state.FillRatio;
        PreviewFillRatio = state.PreviewFillRatio;
        RetractFillRatio = state.RetractFillRatio;
        IsPreviewExtend = state.IsPreviewExtend;
        IsPreviewHold = state.IsPreviewHold;
        IsPreviewRetract = state.IsPreviewRetract;
        IsPreviewSplit = state.IsPreviewSplit;
        IsPreview = state.IsPreview;
        FilledClipRatio = filledClipRatio;
        FilledClipOffsetRatio = filledClipOffsetRatio;
        FilledSymbolOffsetRatio = filledSymbolOffsetRatio;
        UpdatePseudoClasses();
    }

    internal void UpdateOrientationPseudoClasses(bool isHorizontal, bool isVertical)
    {
        PseudoClasses.Set(PseudoClassName.Horizontal, isHorizontal);
        PseudoClasses.Set(PseudoClassName.Vertical, isVertical);
    }

    internal void UpdatePseudoClasses()
    {
        var ratio = FillRatio;
        PseudoClasses.Set(PseudoClassName.Empty, ratio <= 0);
        PseudoClasses.Set(PseudoClassName.Partial, ratio is > 0 and < 1);
        PseudoClasses.Set(PseudoClassName.Full, ratio >= 1);
        PseudoClasses.Set(PseudoClassName.Preview, IsPreview);
        PseudoClasses.Set(PseudoClassName.PreviewExtend, IsPreviewExtend);
        PseudoClasses.Set(PseudoClassName.PreviewHold, IsPreviewHold);
        PseudoClasses.Set(PseudoClassName.PreviewRetract, IsPreviewRetract);
        PseudoClasses.Set(PseudoClassName.PreviewSplit, IsPreviewSplit);
    }

    internal void UpdateAutomationName()
        => AutomationProperties.SetName(this, string.Format(
            System.Globalization.CultureInfo.CurrentCulture,
            RatingResources.ItemAutomationName,
            Index));

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _symbolHost = e.NameScope.Find<Panel>(PartSymbolHost);
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);

        if (Owner is null || e.Handled)
            return;

        Owner.HandleItemPointerMoved(this, e);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (Owner is null || e.Handled)
            return;

        Owner.HandleItemPointerPressed(this, e);
        e.Handled = true;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (Owner is null || e.Handled)
            return;

        Owner.HandleItemPointerMoved(this, e);
    }
}

/// <summary>
/// Computed visual state for a single <see cref="RatingItem"/> slot.
/// </summary>
/// <param name="FillRatio">Committed fill ratio (0..1), always derived from <see cref="Rating.Value"/>.</param>
/// <param name="PreviewFillRatio">Preview fill ratio (0..1), derived from the hover preview value.</param>
/// <param name="RetractFillRatio">Retracted committed fill ratio (0..1) on split boundary items.</param>
/// <param name="IsPreviewExtend">Preview extends beyond the committed value on this item.</param>
/// <param name="IsPreviewHold">Preview holds the committed fill on this item while retracting elsewhere.</param>
/// <param name="IsPreviewRetract">Committed fill remains visible but attenuated beyond the preview value.</param>
/// <param name="IsPreviewSplit">Preview and retract share the same item at a fractional boundary.</param>
internal readonly record struct RatingItemVisualState(
    double FillRatio,
    double PreviewFillRatio,
    double RetractFillRatio,
    bool IsPreviewExtend,
    bool IsPreviewHold,
    bool IsPreviewRetract,
    bool IsPreviewSplit)
{
    /// <summary>Gets a value indicating whether any preview pseudo-class is active.</summary>
    public bool IsPreview => IsPreviewExtend || IsPreviewHold || IsPreviewRetract || IsPreviewSplit;
}
