// -----------------------------------------------------------------------
// <copyright file="FormItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.Enums;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Provides attached properties for configuring form items.
/// </summary>
public static class FormItem
{
    /// <summary>
    /// Defines the Label attached property.
    /// </summary>
    public static readonly AttachedProperty<object?> LabelProperty = AvaloniaProperty.RegisterAttached<Control, object?>("Label", typeof(FormItem));

    /// <summary>
    /// Defines the LabelTemplate attached property.
    /// </summary>
    public static readonly AttachedProperty<IDataTemplate?> LabelTemplateProperty = AvaloniaProperty.RegisterAttached<Control, IDataTemplate?>("LabelTemplate", typeof(FormItem));

    /// <summary>
    /// Defines the NoLabel attached property.
    /// </summary>
    public static readonly AttachedProperty<bool> NoLabelProperty = AvaloniaProperty.RegisterAttached<Control, bool>("NoLabel", typeof(FormItem), defaultValue: false);

    /// <summary>
    /// Defines the LabelPosition attached property.
    /// </summary>
    public static readonly AttachedProperty<Position?> LabelPositionProperty = AvaloniaProperty.RegisterAttached<Control, Position?>("LabelPosition", typeof(FormItem));

    /// <summary>
    /// Defines the LabelWidth attached property.
    /// </summary>
    public static readonly AttachedProperty<GridLength?> LabelWidthProperty = AvaloniaProperty.RegisterAttached<Control, GridLength?>("LabelWidth", typeof(FormItem));

    /// <summary>
    /// Defines the LabelMargin attached property.
    /// </summary>
    public static readonly AttachedProperty<Thickness?> LabelMarginProperty = AvaloniaProperty.RegisterAttached<Control, Thickness?>("LabelMargin", typeof(FormItem));

    /// <summary>
    /// Defines the LabelWidth attached property.
    /// </summary>
    public static readonly AttachedProperty<HorizontalAlignment?> LabelAlignmentProperty = AvaloniaProperty.RegisterAttached<Control, HorizontalAlignment?>("LabelAlignment", typeof(FormItem));

    /// <summary>
    /// Defines the IsRequired attached property.
    /// </summary>
    public static readonly AttachedProperty<bool> IsRequiredProperty = AvaloniaProperty.RegisterAttached<Control, bool>("IsRequired", typeof(FormItem), defaultValue: false);

    /// <summary>
    /// Defines the RequiredIndicator attached property.
    /// </summary>
    public static readonly AttachedProperty<string?> RequiredIndicatorProperty = AvaloniaProperty.RegisterAttached<Control, string?>("RequiredIndicator", typeof(FormItem));

    /// <summary>
    /// Defines the HelpText attached property.
    /// </summary>
    public static readonly AttachedProperty<string?> HelpTextProperty = AvaloniaProperty.RegisterAttached<Control, string?>("HelpText", typeof(FormItem));

    /// <summary>
    /// Defines the ColumnSpan attached property.
    /// </summary>
    public static readonly AttachedProperty<int> ColumnSpanProperty = AvaloniaProperty.RegisterAttached<Control, int>("ColumnSpan", typeof(FormItem), defaultValue: 1);

    /// <summary>
    /// Defines the RowSpan attached property.
    /// </summary>
    public static readonly AttachedProperty<int> RowSpanProperty = AvaloniaProperty.RegisterAttached<Control, int>("RowSpan", typeof(FormItem), defaultValue: 1);

    /// <summary>
    /// Defines the TextWrapping attached property.
    /// </summary>
    public static readonly AttachedProperty<bool> TextWrappingProperty = AvaloniaProperty.RegisterAttached<Control, bool>("TextWrapping", typeof(FormItem), defaultValue: false);

    // Getters and Setters
    public static object? GetLabel(Control control) => control.GetValue(LabelProperty);

    public static void SetLabel(Control control, object? value) => control.SetValue(LabelProperty, value);

    public static IDataTemplate? GetLabelTemplate(Control control) => control.GetValue(LabelTemplateProperty);

    public static void SetLabelTemplate(Control control, IDataTemplate? value) => control.SetValue(LabelTemplateProperty, value);

    public static bool GetNoLabel(Control control) => control.GetValue(NoLabelProperty);

    public static void SetNoLabel(Control control, bool value) => control.SetValue(NoLabelProperty, value);

    public static Position? GetLabelPosition(Control control) => control.GetValue(LabelPositionProperty);

    public static void SetLabelPosition(Control control, Position? value) => control.SetValue(LabelPositionProperty, value);

    public static GridLength? GetLabelWidth(Control control) => control.GetValue(LabelWidthProperty);

    public static void SetLabelWidth(Control control, GridLength? value) => control.SetValue(LabelWidthProperty, value);

    public static Thickness? GetLabelMargin(Control control) => control.GetValue(LabelMarginProperty);

    public static void SetLabelMargin(Control control, Thickness? value) => control.SetValue(LabelMarginProperty, value);

    public static HorizontalAlignment? GetLabelAlignment(Control control) => control.GetValue(LabelAlignmentProperty);

    public static void SetLabelAlignment(Control control, HorizontalAlignment? value) => control.SetValue(LabelAlignmentProperty, value);

    public static bool GetIsRequired(Control control) => control.GetValue(IsRequiredProperty);

    public static void SetIsRequired(Control control, bool value) => control.SetValue(IsRequiredProperty, value);

    public static string? GetRequiredIndicator(Control control) => control.GetValue(RequiredIndicatorProperty);

    public static void SetRequiredIndicator(Control control, string? value) => control.SetValue(RequiredIndicatorProperty, value);

    public static string? GetHelpText(Control control) => control.GetValue(HelpTextProperty);

    public static void SetHelpText(Control control, string? value) => control.SetValue(HelpTextProperty, value);

    public static int GetColumnSpan(Control control) => control.GetValue(ColumnSpanProperty);

    public static void SetColumnSpan(Control control, int value) => control.SetValue(ColumnSpanProperty, value);

    public static int GetRowSpan(Control control) => control.GetValue(RowSpanProperty);

    public static void SetRowSpan(Control control, int value) => control.SetValue(RowSpanProperty, value);

    public static bool GetTextWrapping(Control control) => control.GetValue(TextWrappingProperty);

    public static void SetTextWrapping(Control control, bool value) => control.SetValue(TextWrappingProperty, value);
}
