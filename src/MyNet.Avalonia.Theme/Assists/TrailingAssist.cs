// -----------------------------------------------------------------------
// <copyright file="TrailingAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Layout;

namespace MyNet.Avalonia.Theme.Assists;

public static class TrailingAssist
{
    #region Padding

    public static readonly AttachedProperty<Thickness> PaddingProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Padding", typeof(TrailingAssist));

    public static void SetPadding(StyledElement obj, Thickness value) => obj.SetValue(PaddingProperty, value);

    public static Thickness GetPadding(StyledElement obj) => obj.GetValue(PaddingProperty);

    #endregion Padding

    #region Margin

    public static readonly AttachedProperty<Thickness> MarginProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Margin", typeof(TrailingAssist));

    public static void SetMargin(StyledElement obj, Thickness value) => obj.SetValue(MarginProperty, value);

    public static Thickness GetMargin(StyledElement obj) => obj.GetValue(MarginProperty);

    #endregion Margin

    #region HorizontalAlignment

    public static readonly AttachedProperty<HorizontalAlignment> HorizontalAlignmentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, HorizontalAlignment>("HorizontalAlignment", typeof(TrailingAssist), HorizontalAlignment.Center);

    public static void SetHorizontalAlignment(StyledElement obj, HorizontalAlignment value) => obj.SetValue(HorizontalAlignmentProperty, value);

    public static HorizontalAlignment GetHorizontalAlignment(StyledElement obj) => obj.GetValue(HorizontalAlignmentProperty);

    #endregion HorizontalAlignment

    #region VerticalAlignment

    public static readonly AttachedProperty<VerticalAlignment> VerticalAlignmentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, VerticalAlignment>("VerticalAlignment", typeof(TrailingAssist), VerticalAlignment.Center);

    public static void SetVerticalAlignment(StyledElement obj, VerticalAlignment value) => obj.SetValue(VerticalAlignmentProperty, value);

    public static VerticalAlignment GetVerticalAlignment(StyledElement obj) => obj.GetValue(VerticalAlignmentProperty);

    #endregion VerticalAlignment

    #region IsVisible

    public static readonly AttachedProperty<bool> IsVisibleProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>("IsVisible", typeof(TrailingAssist));

    public static void SetIsVisible(StyledElement obj, bool value) => obj.SetValue(IsVisibleProperty, value);

    public static bool GetIsVisible(StyledElement obj) => obj.GetValue(IsVisibleProperty);

    #endregion IsVisible
}
