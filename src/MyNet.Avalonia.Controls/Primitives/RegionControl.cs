// -----------------------------------------------------------------------
// <copyright file="RegionControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls;

namespace MyNet.Avalonia.Controls.Primitives;

/// <summary>
/// Base for content controls that expose semantic layout regions.
/// </summary>
/// <remarks>
/// <para>Region semantics by derived control:</para>
/// <list type="bullet">
/// <item><description><see cref="Card"/>: <see cref="Header"/> = optional top chrome band;
/// use <see cref="Card.Title"/> / <see cref="Card.Subtitle"/> for the tile heading in the body.</description></item>
/// <item><description><see cref="Banner"/>, <see cref="DialogPanel"/>, <see cref="ContentDialog"/>:
/// <see cref="Header"/> = primary heading text.</description></item>
/// <item><description><c>MessageBoxContent</c> (Extended): chromeless message box body inside window/overlay shells;
/// <see cref="Header"/> = title (via <c>Title</c>), <see cref="ContentControl.Content"/> = message,
/// <see cref="Leading"/> = severity icon badge.</description></item>
/// <item><description><c>MessageNotificationControl</c> (Extended): chromeless toast body inside <c>NotificationCard</c>;
/// <see cref="Header"/> = title, <see cref="ContentControl.Content"/> = message, <see cref="Leading"/> = severity icon.</description></item>
/// <item><description><see cref="EmptyState"/>: <see cref="EmptyState.Title"/> / <see cref="EmptyState.Subtitle"/> convenience for
/// the centered heading; <see cref="Leading"/> = illustration; <see cref="Actions"/> = call-to-action buttons.</description></item>
/// </list>
/// <para>Style regions with attached assists (<c>HeaderAssist</c>, <c>LeadingAssist</c>, …).
/// Toggle the header band with <c>HeaderAssist.IsVisible</c> (for example when an overlay shell shows the title).</para>
/// </remarks>
public abstract class RegionControl : ContentControl
{
    #region Leading

    /// <summary>
    /// Gets or sets the leading region (icon, avatar, indicator).
    /// </summary>
    public static readonly StyledProperty<object?> LeadingProperty =
        AvaloniaProperty.Register<RegionControl, object?>(nameof(Leading));

    public object? Leading
    {
        get => GetValue(LeadingProperty);
        set => SetValue(LeadingProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> LeadingTemplateProperty =
        AvaloniaProperty.Register<RegionControl, IDataTemplate?>(nameof(LeadingTemplate));

    public IDataTemplate? LeadingTemplate
    {
        get => GetValue(LeadingTemplateProperty);
        set => SetValue(LeadingTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the leading slot is shown when <see cref="Leading"/> is set.
    /// </summary>
    public static readonly StyledProperty<bool> IsLeadingSlotVisibleProperty =
        AvaloniaProperty.Register<RegionControl, bool>(nameof(IsLeadingSlotVisible), true);

    public bool IsLeadingSlotVisible
    {
        get => GetValue(IsLeadingSlotVisibleProperty);
        set => SetValue(IsLeadingSlotVisibleProperty, value);
    }

    #endregion

    #region Header

    /// <summary>
    /// Gets or sets the header region. Meaning depends on the derived control — see <see cref="RegionControl"/> remarks.
    /// </summary>
    public static readonly StyledProperty<object?> HeaderProperty =
        AvaloniaProperty.Register<RegionControl, object?>(nameof(Header));

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
        AvaloniaProperty.Register<RegionControl, IDataTemplate?>(nameof(HeaderTemplate));

    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    #endregion

    #region Trailing

    /// <summary>
    /// Gets or sets the trailing region (chevron, menu, secondary action).
    /// </summary>
    public static readonly StyledProperty<object?> TrailingProperty =
        AvaloniaProperty.Register<RegionControl, object?>(nameof(Trailing));

    public object? Trailing
    {
        get => GetValue(TrailingProperty);
        set => SetValue(TrailingProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> TrailingTemplateProperty =
        AvaloniaProperty.Register<RegionControl, IDataTemplate?>(nameof(TrailingTemplate));

    public IDataTemplate? TrailingTemplate
    {
        get => GetValue(TrailingTemplateProperty);
        set => SetValue(TrailingTemplateProperty, value);
    }

    #endregion

    #region Actions

    /// <summary>
    /// Gets or sets the actions region (button row, toolbar).
    /// </summary>
    public static readonly StyledProperty<object?> ActionsProperty =
        AvaloniaProperty.Register<RegionControl, object?>(nameof(Actions));

    public object? Actions
    {
        get => GetValue(ActionsProperty);
        set => SetValue(ActionsProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> ActionsTemplateProperty =
        AvaloniaProperty.Register<RegionControl, IDataTemplate?>(nameof(ActionsTemplate));

    public IDataTemplate? ActionsTemplate
    {
        get => GetValue(ActionsTemplateProperty);
        set => SetValue(ActionsTemplateProperty, value);
    }

    #endregion

    /// <summary>
    /// Returns whether a region value should be treated as empty for layout pseudo-classes.
    /// </summary>
    protected static bool IsEmptyLike(object? value) => value is null || value switch
    {
        string str => string.IsNullOrEmpty(str),
        double dbl => double.IsNaN(dbl),
        Array arr => arr.Length == 0,
        DateTime date => date == DateTime.MinValue,
        _ => false,
    };

    /// <summary>
    /// Updates the <see cref="PseudoClassName.HeaderEmpty"/> pseudo-class from <see cref="Header"/>.
    /// </summary>
    protected void UpdateHeaderEmptyPseudoClass()
        => PseudoClasses.Set(PseudoClassName.HeaderEmpty, IsEmptyLike(Header));
}
