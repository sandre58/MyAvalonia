// -----------------------------------------------------------------------
// <copyright file="SettingsBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Behaviors;

internal static class SettingsBehavior
{
    static SettingsBehavior()
    {
        IconLayoutProperty.Changed.Subscribe(OnIconLayoutChanged);
        IndicatorHorizontalAlignmentClassProperty.Changed.Subscribe(OnIndicatorHorizontalAlignmentClassChanged);
        AlignmentClassProperty.Changed.Subscribe(OnAlignmentClassChanged);
        HeaderAlignmentClassProperty.Changed.Subscribe(OnHeaderAlignmentClassChanged);
        CarouselTypeProperty.Changed.Subscribe(OnCarouselTypeChanged);
        CarouselPositionProperty.Changed.Subscribe(OnCarouselPositionChanged);
        CornerPositionProperty.Changed.Subscribe(OnCornerPositionChanged);
        ExpandDirectionProperty.Changed.Subscribe(OnExpandDirectionChanged);
        ProgressBarValuePositionProperty.Changed.Subscribe(OnProgressBarValuePositionChanged);
    }

    #region IconLayout

    /// <summary>
    /// Provides IconLayout Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> IconLayoutProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("IconLayout", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="IconLayoutProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IconLayoutProperty"/>.</param>
    public static void SetIconLayout(StyledElement element, int value) => element.SetValue(IconLayoutProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IconLayoutProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetIconLayout(StyledElement element) => element.GetValue(IconLayoutProperty);

    private static void OnIconLayoutChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not StyledElement ctrl)
            return;

        var index = args.GetNewValue<int>();
        var list = new List<string> { "Left", "Right", "Top", "Bottom" };
        ctrl.Classes.RemoveAll(list.Select(y => $"Icon{y}"));

        if (index > 0)
        {
            IconAssist.SetIcon(ctrl, RandomGenerator.Enum<IconData>().ToIcon());
            ctrl.AddClasses($"Icon{list[index - 1]}");
        }
        else
        {
            IconAssist.SetIcon(ctrl, null);
        }
    }

    #endregion

    #region IndicatorHorizontalAlignmentClass

    /// <summary>
    /// Provides IndicatorHorizontalAlignmentClass Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> IndicatorHorizontalAlignmentClassProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("IndicatorHorizontalAlignmentClass", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorHorizontalAlignmentClassProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IndicatorHorizontalAlignmentClassProperty"/>.</param>
    public static void SetIndicatorHorizontalAlignmentClass(StyledElement element, int value) => element.SetValue(IndicatorHorizontalAlignmentClassProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorHorizontalAlignmentClassProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetIndicatorHorizontalAlignmentClass(StyledElement element) => element.GetValue(IndicatorHorizontalAlignmentClassProperty);

    private static void OnIndicatorHorizontalAlignmentClassChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not StyledElement ctrl)
            return;

        var index = args.GetNewValue<int>();
        SetClasses(ctrl, ["IndicatorLeft", "IndicatorCenter", "IndicatorRight"], index);
    }

    private static void SetClasses(StyledElement ctrl, string[] classes, int index)
    {
        ctrl.Classes.RemoveAll(classes);
        ctrl.AddClasses(classes.GetByIndex(index).OrEmpty());
    }

    #endregion

    #region AlignmentClass

    /// <summary>
    /// Provides AlignmentClass Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> AlignmentClassProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("AlignmentClass", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="AlignmentClassProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AlignmentClassProperty"/>.</param>
    public static void SetAlignmentClass(StyledElement element, int value) => element.SetValue(AlignmentClassProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AlignmentClassProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetAlignmentClass(StyledElement element) => element.GetValue(AlignmentClassProperty);

    private static void OnAlignmentClassChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not StyledElement ctrl)
            return;

        var index = args.GetNewValue<int>();
        SetClasses(ctrl, ["Left", "Right", "Top", "Bottom"], index);
    }

    #endregion

    #region CarouselType

    /// <summary>
    /// Provides CarouselType Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> CarouselTypeProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("CarouselType", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="CarouselTypeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="CarouselTypeProperty"/>.</param>
    public static void SetCarouselType(StyledElement element, int value) => element.SetValue(CarouselTypeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="CarouselTypeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetCarouselType(StyledElement element) => element.GetValue(CarouselTypeProperty);

    private static void OnCarouselTypeChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not StyledElement ctrl)
            return;

        var index = args.GetNewValue<int>();
        SetClasses(ctrl, ["Dots", "Columnar", "Line"], index);
    }

    #endregion

    #region CarouselPosition

    /// <summary>
    /// Provides CarouselPosition Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> CarouselPositionProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("CarouselPosition", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="CarouselPositionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="CarouselPositionProperty"/>.</param>
    public static void SetCarouselPosition(StyledElement element, int value) => element.SetValue(CarouselPositionProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="CarouselPositionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetCarouselPosition(StyledElement element) => element.GetValue(CarouselPositionProperty);

    private static void OnCarouselPositionChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not StyledElement ctrl)
            return;

        var index = args.GetNewValue<int>();
        SetClasses(ctrl, ["Left", "Center", "Right"], index);
    }

    #endregion

    #region CornerPosition

    /// <summary>
    /// Provides CornerPosition Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> CornerPositionProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("CornerPosition", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="CornerPositionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="CornerPositionProperty"/>.</param>
    public static void SetCornerPosition(StyledElement element, int value) => element.SetValue(CornerPositionProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="CornerPositionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetCornerPosition(StyledElement element) => element.GetValue(CornerPositionProperty);

    private static void OnCornerPositionChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not MyNet.Avalonia.Controls.Badge ctrl)
            return;

        var index = args.GetNewValue<int>();
        ctrl.CornerPosition = (CornerPosition)index;
    }

    #endregion

    #region HeaderAlignmentClass

    /// <summary>
    /// Provides HeaderAlignmentClass Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> HeaderAlignmentClassProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("HeaderAlignmentClass", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="HeaderAlignmentClassProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HeaderAlignmentClassProperty"/>.</param>
    public static void SetHeaderAlignmentClass(StyledElement element, int value) => element.SetValue(HeaderAlignmentClassProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HeaderAlignmentClassProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetHeaderAlignmentClass(StyledElement element) => element.GetValue(HeaderAlignmentClassProperty);

    private static void OnHeaderAlignmentClassChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not StyledElement ctrl)
            return;

        var index = args.GetNewValue<int>();
        SetClasses(ctrl, ["HeaderLeft", "HeaderRight", "HeaderTop", "HeaderBottom", "HeaderCenter"], index);
    }

    #endregion

    #region ExpandDirection

    /// <summary>
    /// Provides ExpandDirection Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> ExpandDirectionProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("ExpandDirection", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="ExpandDirectionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ExpandDirectionProperty"/>.</param>
    public static void SetExpandDirection(StyledElement element, int value) => element.SetValue(ExpandDirectionProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ExpandDirectionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetExpandDirection(StyledElement element) => element.GetValue(ExpandDirectionProperty);

    private static void OnExpandDirectionChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not Expander ctrl)
            return;

        var index = args.GetNewValue<int>();
        switch (index)
        {
            case 0:
                ctrl.ExpandDirection = ExpandDirection.Down;
                ctrl.VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Top;
                ctrl.HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Left;
                ctrl.Width = 300;
                ctrl.Height = double.NaN;
                break;

            case 1:
                ctrl.ExpandDirection = ExpandDirection.Up;
                ctrl.VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Bottom;
                ctrl.HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Left;
                ctrl.Width = 300;
                ctrl.Height = double.NaN;
                break;

            case 2:
                ctrl.ExpandDirection = ExpandDirection.Left;
                ctrl.VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Top;
                ctrl.HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Right;
                ctrl.Width = double.NaN;
                ctrl.Height = 300;
                break;

            case 3:
                ctrl.ExpandDirection = ExpandDirection.Right;
                ctrl.VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Top;
                ctrl.HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Left;
                ctrl.Width = double.NaN;
                ctrl.Height = 300;
                break;
        }
    }

    #endregion

    #region ProgressBarValuePosition

    /// <summary>
    /// Provides ProgressBarValuePosition Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<int> ProgressBarValuePositionProperty = AvaloniaProperty.RegisterAttached<StyledElement, int>("ProgressBarValuePosition", typeof(SettingsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="ProgressBarValuePositionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ProgressBarValuePositionProperty"/>.</param>
    public static void SetProgressBarValuePosition(StyledElement element, int value) => element.SetValue(ProgressBarValuePositionProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ProgressBarValuePositionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static int GetProgressBarValuePosition(StyledElement element) => element.GetValue(ProgressBarValuePositionProperty);

    private static void OnProgressBarValuePositionChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not ProgressBar ctrl)
            return;

        var index = args.GetNewValue<int>();
        ctrl.Classes.Remove("Left");
        ctrl.Classes.Remove("Right");

        switch (index)
        {
            case 0:
                ctrl.ShowProgressText = false;
                break;

            case 1:
                ctrl.ShowProgressText = true;
                ctrl.Classes.Add("Left");
                break;

            case 2:
                ctrl.ShowProgressText = true;
                break;

            case 3:
                ctrl.ShowProgressText = true;
                ctrl.Classes.Add("Right");
                break;
        }
    }

    #endregion
}
