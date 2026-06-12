// -----------------------------------------------------------------------
// <copyright file="RegionControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace MyNet.Avalonia.Controls.Primitives;

/// <summary>
/// Base for content controls that expose semantic layout regions.
/// </summary>
public abstract class RegionControl : ContentControl
{
    #region Leading

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

    #endregion

    #region Header

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
}
