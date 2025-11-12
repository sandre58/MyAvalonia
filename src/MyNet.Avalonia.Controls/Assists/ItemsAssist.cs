// -----------------------------------------------------------------------
// <copyright file="ItemsAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Observable.Translatables;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Assists;

public static class ItemsAssist
{
    static ItemsAssist()
    {
        OverrideContentTemplateProperty.Changed.Subscribe(OverrideContentTemplatePropertyChangedCallback);
        EnumSourceTypeProperty.Changed.Subscribe(EnumSourceTypePropertyChangedCallback);
    }

    #region Background

    /// <summary>
    /// Provides Background Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> BackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("Background", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BackgroundProperty"/>.</param>
    public static void SetBackground(StyledElement element, IBrush? value) => element.SetValue(BackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetBackground(StyledElement element) => element.GetValue(BackgroundProperty);

    #endregion

    #region Foreground

    /// <summary>
    /// Provides Foreground Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("Foreground", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ForegroundProperty"/>.</param>
    public static void SetForeground(StyledElement element, IBrush? value) => element.SetValue(ForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetForeground(StyledElement element) => element.GetValue(ForegroundProperty);

    #endregion

    #region BorderBrush

    /// <summary>
    /// Provides BorderBrush Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> BorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("BorderBrush", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BorderBrushProperty"/>.</param>
    public static void SetBorderBrush(StyledElement element, IBrush? value) => element.SetValue(BorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetBorderBrush(StyledElement element) => element.GetValue(BorderBrushProperty);

    #endregion

    #region BorderThickness

    /// <summary>
    /// Provides BorderThickness Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> BorderThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("BorderThickness", typeof(ItemsAssist), new Thickness(0));

    /// <summary>
    /// Accessor for Attached  <see cref="BorderThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BorderThicknessProperty"/>.</param>
    public static void SetBorderThickness(StyledElement element, Thickness value) => element.SetValue(BorderThicknessProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BorderThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetBorderThickness(StyledElement element) => element.GetValue(BorderThicknessProperty);

    #endregion

    #region HoverBackground

    /// <summary>
    /// Provides HoverBackground Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverBackground", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HoverBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HoverBackgroundProperty"/>.</param>
    public static void SetHoverBackground(StyledElement element, IBrush? value) => element.SetValue(HoverBackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HoverBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetHoverBackground(StyledElement element) => element.GetValue(HoverBackgroundProperty);

    #endregion

    #region HoverBorderBrush

    /// <summary>
    /// Provides HoverBorderBrush Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverBorderBrush", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HoverBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HoverBorderBrushProperty"/>.</param>
    public static void SetHoverBorderBrush(StyledElement element, IBrush? value) => element.SetValue(HoverBorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HoverBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetHoverBorderBrush(StyledElement element) => element.GetValue(HoverBorderBrushProperty);

    #endregion

    #region HoverForeground

    /// <summary>
    /// Provides HoverForeground Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverForeground", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HoverForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HoverForegroundProperty"/>.</param>
    public static void SetHoverForeground(StyledElement element, IBrush? value) => element.SetValue(HoverForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HoverForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetHoverForeground(StyledElement element) => element.GetValue(HoverForegroundProperty);

    #endregion

    #region ActiveBackground

    /// <summary>
    /// Provides ActiveBackground Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveBackground", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveBackgroundProperty"/>.</param>
    public static void SetActiveBackground(StyledElement element, IBrush? value) => element.SetValue(ActiveBackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetActiveBackground(StyledElement element) => element.GetValue(ActiveBackgroundProperty);

    #endregion

    #region ActiveBorderBrush

    /// <summary>
    /// Provides ActiveBorderBrush Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveBorderBrush", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveBorderBrushProperty"/>.</param>
    public static void SetActiveBorderBrush(StyledElement element, IBrush? value) => element.SetValue(ActiveBorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetActiveBorderBrush(StyledElement element) => element.GetValue(ActiveBorderBrushProperty);

    #endregion

    #region ActiveForeground

    /// <summary>
    /// Provides ActiveForeground Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveForeground", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveForegroundProperty"/>.</param>
    public static void SetActiveForeground(StyledElement element, IBrush? value) => element.SetValue(ActiveForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetActiveForeground(StyledElement element) => element.GetValue(ActiveForegroundProperty);

    #endregion

    #region RippleColor

    /// <summary>
    /// Provides RippleColor Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> RippleColorProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("RippleColor", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RippleColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RippleColorProperty"/>.</param>
    public static void SetRippleColor(StyledElement element, IBrush? value) => element.SetValue(RippleColorProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RippleColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetRippleColor(StyledElement element) => element.GetValue(RippleColorProperty);

    #endregion

    #region CornerRadius

    /// <summary>
    /// Provides CornerRadius Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<CornerRadius> CornerRadiusProperty = AvaloniaProperty.RegisterAttached<StyledElement, CornerRadius>("CornerRadius", typeof(ItemsAssist), new CornerRadius(0));

    /// <summary>
    /// Accessor for Attached  <see cref="CornerRadiusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="CornerRadiusProperty"/>.</param>
    public static void SetCornerRadius(StyledElement element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="CornerRadiusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static CornerRadius GetCornerRadius(StyledElement element) => element.GetValue(CornerRadiusProperty);

    #endregion

    #region Padding

    /// <summary>
    /// Provides Padding Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> PaddingProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Padding", typeof(ItemsAssist), new Thickness(0));

    /// <summary>
    /// Accessor for Attached  <see cref="PaddingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PaddingProperty"/>.</param>
    public static void SetPadding(StyledElement element, Thickness value) => element.SetValue(PaddingProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PaddingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetPadding(StyledElement element) => element.GetValue(PaddingProperty);

    #endregion

    #region Margin

    /// <summary>
    /// Provides Margin Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> MarginProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Margin", typeof(ItemsAssist), new Thickness(0));

    /// <summary>
    /// Accessor for Attached  <see cref="MarginProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="MarginProperty"/>.</param>
    public static void SetMargin(StyledElement element, Thickness value) => element.SetValue(MarginProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="MarginProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetMargin(StyledElement element) => element.GetValue(MarginProperty);

    #endregion

    #region HorizontalAlignment

    /// <summary>
    /// Provides HorizontalAlignment Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<HorizontalAlignment> HorizontalAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, HorizontalAlignment>("HorizontalAlignment", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HorizontalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HorizontalAlignmentProperty"/>.</param>
    public static void SetHorizontalAlignment(StyledElement element, HorizontalAlignment value) => element.SetValue(HorizontalAlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HorizontalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static HorizontalAlignment GetHorizontalAlignment(StyledElement element) => element.GetValue(HorizontalAlignmentProperty);

    #endregion

    #region VerticalAlignment

    /// <summary>
    /// Provides VerticalContentAlignment Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<VerticalAlignment> VerticalAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, VerticalAlignment>("VerticalAlignment", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="VerticalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="VerticalAlignmentProperty"/>.</param>
    public static void SetVerticalAlignment(StyledElement element, VerticalAlignment value) => element.SetValue(VerticalAlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="VerticalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static VerticalAlignment GetVerticalAlignment(StyledElement element) => element.GetValue(VerticalAlignmentProperty);

    #endregion

    #region HorizontalAlignment

    /// <summary>
    /// Provides HorizontalContentAlignment Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<HorizontalAlignment> HorizontalContentAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, HorizontalAlignment>("HorizontalAlignment", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HorizontalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HorizontalAlignmentProperty"/>.</param>
    public static void SetHorizontalContentAlignment(StyledElement element, HorizontalAlignment value) => element.SetValue(HorizontalContentAlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HorizontalContentAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static HorizontalAlignment GetHorizontalContentAlignment(StyledElement element) => element.GetValue(HorizontalContentAlignmentProperty);

    #endregion

    #region VerticalContentAlignment

    /// <summary>
    /// Provides VerticalAlignment Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<VerticalAlignment> VerticalContentAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, VerticalAlignment>("VerticalContentAlignment", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="VerticalContentAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="VerticalAlignmentProperty"/>.</param>
    public static void SetVerticalContentAlignment(StyledElement element, VerticalAlignment value) => element.SetValue(VerticalContentAlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="VerticalContentAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static VerticalAlignment GetVerticalContentAlignment(StyledElement element) => element.GetValue(VerticalContentAlignmentProperty);

    #endregion

    #region OverrideContentTemplate

    /// <summary>
    /// Provides VerticalAlignment Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> OverrideContentTemplateProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("OverrideContentTemplate", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="OverrideContentTemplateProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="VerticalAlignmentProperty"/>.</param>
    public static void SetOverrideContentTemplate(StyledElement element, bool value) => element.SetValue(OverrideContentTemplateProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OverrideContentTemplateProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetOverrideContentTemplate(StyledElement element) => element.GetValue(OverrideContentTemplateProperty);

    private static void OverrideContentTemplatePropertyChangedCallback(AvaloniaPropertyChangedEventArgs obj)
    {
        if (obj.Sender is not ContentControl sender || ((bool?)obj.NewValue).IsFalse()) return;

        if (sender.Parent is not ItemsControl itemsControl) return;

        itemsControl.PropertyChanged += (_, e) =>
        {
            if (e.Property == ItemsControl.ItemTemplateProperty)
            {
                itemTemplateChanged();
            }
        };
        itemTemplateChanged();

        void itemTemplateChanged()
        {
            if (sender.ContentTemplate is null && itemsControl.ItemTemplate is not null)
            {
                sender.SetCurrentValue(ContentControl.ContentTemplateProperty, itemsControl.ItemTemplate);
            }
        }
    }

    #endregion

    #region ShadowDepth

    public static readonly AvaloniaProperty<ShadowDepth> ShadowDepthProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ShadowDepth>("ShadowDepth", typeof(ItemsAssist));

    public static void SetShadowDepth(AvaloniaObject element, ShadowDepth value) => element.SetValue(ShadowDepthProperty, value);

    public static ShadowDepth GetShadowDepth(AvaloniaObject element) => element.GetValue<ShadowDepth>(ShadowDepthProperty);

    #endregion ShadowDepth

    #region Size

    /// <summary>
    /// Provides Size Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> SizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Size", typeof(ItemsAssist), 55.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="SizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SizeProperty"/>.</param>
    public static void SetSize(StyledElement element, double value) => element.SetValue(SizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetSize(StyledElement element) => element.GetValue(SizeProperty);

    #endregion

    #region Placement

    /// <summary>
    /// Provides Placement Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<Position> PlacementProperty = AvaloniaProperty.RegisterAttached<StyledElement, Position>("Placement", typeof(ItemsAssist), Position.Bottom);

    /// <summary>
    /// Accessor for Attached  <see cref="PlacementProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlacementProperty"/>.</param>
    public static void SetPlacement(StyledElement element, Position value) => element.SetValue(PlacementProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlacementProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Position GetPlacement(StyledElement element) => element.GetValue(PlacementProperty);

    #endregion

    #region EnumSourceType

    /// <summary>
    /// Provides EnumSourceType Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<Type> EnumSourceTypeProperty = AvaloniaProperty.RegisterAttached<StyledElement, Type>("EnumSourceType", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="EnumSourceTypeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="EnumSourceTypeProperty"/>.</param>
    public static void SetEnumSourceType(StyledElement element, Type value) => element.SetValue(EnumSourceTypeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="EnumSourceTypeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Type GetEnumSourceType(StyledElement element) => element.GetValue(EnumSourceTypeProperty);

    private static void EnumSourceTypePropertyChangedCallback(AvaloniaPropertyChangedEventArgs obj)
    {
        if (obj.Sender is not SelectingItemsControl sender) return;
        if (obj.NewValue is not Type type) return;

        var excludedValues = GetExcludedValues(sender) ?? [];

        IEnumerable? values = null;
        if (type.IsEnum)
        {
            values = Enum.GetValues(type).Cast<Enum>().Where(x => excludedValues?.Contains(x) != true).Select(x => new EnumTranslatable(x));
        }
        else if (type.IsAssignableTo(typeof(IEnumeration)))
        {
            values = EnumClass.GetAll(type).Cast<IEnumeration>().Where(x => excludedValues?.Contains(x) != true).Select(x => new EnumClassTranslatable(x));
        }
        else
        {
            return;
        }

        sender.ItemsSource = values;
        sender.SelectedValueBinding = new Binding(nameof(EnumTranslatable<>.Value));

        if (GetUseDisplayMember(sender))
        {
            sender.ItemTemplate = null;
            sender.DisplayMemberBinding = new Binding(nameof(EnumTranslatable<>.Display));
        }
    }

    #endregion

    #region ExcludedValues

    /// <summary>
    /// Provides ExcludedValues Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<ICollection<object>> ExcludedValuesProperty = AvaloniaProperty.RegisterAttached<StyledElement, ICollection<object>>("ExcludedValues", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ExcludedValuesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ExcludedValuesProperty"/>.</param>
    public static void SetExcludedValues(StyledElement element, ICollection<object> value) => element.SetValue(ExcludedValuesProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ExcludedValuesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ICollection<object> GetExcludedValues(StyledElement element) => element.GetValue(ExcludedValuesProperty);

    #endregion

    #region UseDisplayMember

    /// <summary>
    /// Provides UseDisplayMember Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> UseDisplayMemberProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UseDisplayMember", typeof(ItemsAssist), true);

    /// <summary>
    /// Accessor for Attached  <see cref="UseDisplayMemberProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UseDisplayMemberProperty"/>.</param>
    public static void SetUseDisplayMember(StyledElement element, bool value) => element.SetValue(UseDisplayMemberProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UseDisplayMemberProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUseDisplayMember(StyledElement element) => element.GetValue(UseDisplayMemberProperty);

    #endregion
}
