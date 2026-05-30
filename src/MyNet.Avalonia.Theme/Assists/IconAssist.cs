// -----------------------------------------------------------------------
// <copyright file="IconAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Helpers;

namespace MyNet.Avalonia.Theme.Assists;

public static class IconAssist
{
    #region Icon

    /// <summary>
    /// Provides Icon Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<object?> IconProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("Icon", typeof(IconAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IconProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IconProperty"/>.</param>
    public static void SetIcon(StyledElement element, object? value) => element.SetValue(IconProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IconProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object? GetIcon(StyledElement element) => element.GetValue(IconProperty);

    #endregion

    #region IconTemplate

    /// <summary>
    /// Provides IconTemplate Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<IDataTemplate> IconTemplateProperty = AvaloniaProperty.RegisterAttached<StyledElement, IDataTemplate>("IconTemplate", typeof(IconAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IconTemplateProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IconTemplateProperty"/>.</param>
    public static void SetIconTemplate(StyledElement element, IDataTemplate value) => element.SetValue(IconTemplateProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IconTemplateProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IDataTemplate GetIconTemplate(StyledElement element) => element.GetValue(IconTemplateProperty);

    #endregion

    #region Opacity

    /// <summary>
    /// Provides Opacity Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> OpacityProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Opacity", typeof(IconAssist), 0.7);

    /// <summary>
    /// Accessor for Attached  <see cref="OpacityProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="OpacityProperty"/>.</param>
    public static void SetOpacity(StyledElement element, double value) => element.SetValue(OpacityProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OpacityProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetOpacity(StyledElement element) => element.GetValue(OpacityProperty);

    #endregion

    #region Spacing

    /// <summary>
    /// Provides Spacing Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> SpacingProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Spacing", typeof(IconAssist), 5.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="SpacingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SpacingProperty"/>.</param>
    public static void SetSpacing(StyledElement element, double value) => element.SetValue(SpacingProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SpacingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetSpacing(StyledElement element) => element.GetValue(SpacingProperty);

    #endregion

    #region Alignment

    /// <summary>
    /// Provides Alignment Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<Position> AlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, Position>("Alignment", typeof(IconAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="AlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AlignmentProperty"/>.</param>
    public static void SetAlignment(StyledElement element, Position value) => element.SetValue(AlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Position GetAlignment(StyledElement element) => element.GetValue(AlignmentProperty);

    #endregion

    #region Role

    /// <summary>
    /// Provides Role Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<IconRole> RoleProperty = AvaloniaPropertyHelper.RegisterEnumProperty("Role", typeof(IconAssist), IconRole.None, CssPrefix.Icon, inherits: true);

    /// <summary>
    /// Accessor for Attached  <see cref="RoleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RoleProperty"/>.</param>
    public static void SetRole(StyledElement element, IconRole value) => element.SetValue(RoleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RoleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IconRole GetRole(StyledElement element) => element.GetValue(RoleProperty);

    #endregion

    #region Size

    /// <summary>
    /// Provides Size Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> SizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Size", typeof(IconAssist), 18.0d, inherits: true);

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

    #region LabelSize

    /// <summary>
    /// Provides LabelSize Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> LabelSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("LabelSize", typeof(IconAssist), 15.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="LabelSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="LabelSizeProperty"/>.</param>
    public static void SetLabelSize(StyledElement element, double value) => element.SetValue(LabelSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="LabelSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetLabelSize(StyledElement element) => element.GetValue(LabelSizeProperty);

    #endregion

    #region PrimarySize

    /// <summary>
    /// Provides PrimarySize Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> PrimarySizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("PrimarySize", typeof(IconAssist), 18.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="PrimarySizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PrimarySizeProperty"/>.</param>
    public static void SetPrimarySize(StyledElement element, double value) => element.SetValue(PrimarySizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PrimarySizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetPrimarySize(StyledElement element) => element.GetValue(PrimarySizeProperty);

    #endregion
}

public enum IconRole
{
    None,

    Primary,

    Label
}
