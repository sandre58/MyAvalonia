// -----------------------------------------------------------------------
// <copyright file="ActionsAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Layout;

namespace MyNet.Avalonia.Theme.Assists;

public static class ActionsAssist
{
    #region Padding

    public static readonly AttachedProperty<Thickness> PaddingProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Padding", typeof(ActionsAssist));

    public static void SetPadding(StyledElement obj, Thickness value) => obj.SetValue(PaddingProperty, value);

    public static Thickness GetPadding(StyledElement obj) => obj.GetValue(PaddingProperty);

    #endregion Padding

    #region Margin

    public static readonly AttachedProperty<Thickness> MarginProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Margin", typeof(ActionsAssist));

    public static void SetMargin(StyledElement obj, Thickness value) => obj.SetValue(MarginProperty, value);

    public static Thickness GetMargin(StyledElement obj) => obj.GetValue(MarginProperty);

    #endregion Margin

    #region HorizontalAlignment

    public static readonly AttachedProperty<HorizontalAlignment> HorizontalAlignmentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, HorizontalAlignment>("HorizontalAlignment", typeof(ActionsAssist),  HorizontalAlignment.Right);

    public static void SetHorizontalAlignment(StyledElement obj, HorizontalAlignment value) => obj.SetValue(HorizontalAlignmentProperty, value);

    public static HorizontalAlignment GetHorizontalAlignment(StyledElement obj) => obj.GetValue(HorizontalAlignmentProperty);

    #endregion HorizontalAlignment
}
