// -----------------------------------------------------------------------
// <copyright file="LeadingAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Assists;

public static class LeadingAssist
{
    #region Padding

    public static readonly AttachedProperty<Thickness> PaddingProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Padding", typeof(LeadingAssist));

    public static void SetPadding(StyledElement obj, Thickness value) => obj.SetValue(PaddingProperty, value);

    public static Thickness GetPadding(StyledElement obj) => obj.GetValue(PaddingProperty);

    #endregion Padding

    #region Margin

    public static readonly AttachedProperty<Thickness> MarginProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Margin", typeof(LeadingAssist));

    public static void SetMargin(StyledElement obj, Thickness value) => obj.SetValue(MarginProperty, value);

    public static Thickness GetMargin(StyledElement obj) => obj.GetValue(MarginProperty);

    #endregion Margin

    #region Background

    public static readonly AttachedProperty<IBrush> BackgroundProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Background", typeof(LeadingAssist));

    public static void SetBackground(StyledElement obj, IBrush value) => obj.SetValue(BackgroundProperty, value);

    public static IBrush GetBackground(StyledElement obj) => obj.GetValue(BackgroundProperty);

    #endregion Background

    #region CornerRadius

    public static readonly AttachedProperty<CornerRadius> CornerRadiusProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, CornerRadius>("CornerRadius", typeof(LeadingAssist));

    public static void SetCornerRadius(StyledElement obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);

    public static CornerRadius GetCornerRadius(StyledElement obj) => obj.GetValue(CornerRadiusProperty);

    #endregion CornerRadius

    #region HorizontalAlignment

    public static readonly AttachedProperty<HorizontalAlignment> HorizontalAlignmentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, HorizontalAlignment>("HorizontalAlignment", typeof(LeadingAssist),  HorizontalAlignment.Left);

    public static void SetHorizontalAlignment(StyledElement obj, HorizontalAlignment value) => obj.SetValue(HorizontalAlignmentProperty, value);

    public static HorizontalAlignment GetHorizontalAlignment(StyledElement obj) => obj.GetValue(HorizontalAlignmentProperty);

    #endregion HorizontalAlignment

    #region VerticalAlignment

    public static readonly AttachedProperty<VerticalAlignment> VerticalAlignmentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, VerticalAlignment>("VerticalAlignment", typeof(LeadingAssist), VerticalAlignment.Top);

    public static void SetVerticalAlignment(StyledElement obj, VerticalAlignment value) => obj.SetValue(VerticalAlignmentProperty, value);

    public static VerticalAlignment GetVerticalAlignment(StyledElement obj) => obj.GetValue(VerticalAlignmentProperty);

    #endregion VerticalAlignment
}
