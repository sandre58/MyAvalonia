// -----------------------------------------------------------------------
// <copyright file="ClockSelector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartCarousel, typeof(Carousel))]
public class ClockSelector : TimeSelectorBase
{
    public const string PartCarousel = "PART_Carousel";

    private Carousel? _carousel;

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _carousel = e.NameScope.Find<Carousel>(PartCarousel);
    }

    #region HandBrush

    /// <summary>
    /// Provides HandBrush Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> HandBrushProperty = ClockTimeComponent.HandBrushProperty.AddOwner<ClockSelector>();

    /// <summary>
    /// Gets or sets the HandBrush property.
    /// </summary>
    public IBrush? HandBrush
    {
        get => GetValue(HandBrushProperty);
        set => SetValue(HandBrushProperty, value);
    }

    #endregion

    #region CenterBackground

    /// <summary>
    /// Provides CenterBackground Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> CenterBackgroundProperty = ClockTimeComponent.CenterBackgroundProperty.AddOwner<ClockSelector>();

    /// <summary>
    /// Gets or sets the CenterBackground property.
    /// </summary>
    public IBrush? CenterBackground
    {
        get => GetValue(CenterBackgroundProperty);
        set => SetValue(CenterBackgroundProperty, value);
    }

    #endregion

    #region CenterBorderBrush

    /// <summary>
    /// Provides CenterBorderBrush Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> CenterBorderBrushProperty = ClockTimeComponent.CenterBorderBrushProperty.AddOwner<ClockSelector>();

    /// <summary>
    /// Gets or sets the CenterBorderBrush property.
    /// </summary>
    public IBrush? CenterBorderBrush
    {
        get => GetValue(CenterBorderBrushProperty);
        set => SetValue(CenterBorderBrushProperty, value);
    }

    #endregion

    #region CenterBorderThickness

    /// <summary>
    /// Provides CenterBorderThickness Property.
    /// </summary>
    public static readonly StyledProperty<double> CenterBorderThicknessProperty = ClockTimeComponent.CenterBorderThicknessProperty.AddOwner<ClockSelector>();

    /// <summary>
    /// Gets or sets the CenterBorderThickness property.
    /// </summary>
    public double CenterBorderThickness
    {
        get => GetValue(CenterBorderThicknessProperty);
        set => SetValue(CenterBorderThicknessProperty, value);
    }

    #endregion

    #region HandWidth

    public static readonly StyledProperty<double> HandWidthProperty = ClockTimeComponent.HandWidthProperty.AddOwner<ClockSelector>();

    public double HandWidth
    {
        get => GetValue(HandWidthProperty);
        set => SetValue(HandWidthProperty, value);
    }

    #endregion

    #region AutoChangeMode

    /// <summary>
    /// Provides AutoChangeMode Property.
    /// </summary>
    public static readonly StyledProperty<bool> AutoChangeModeProperty = AvaloniaProperty.Register<ClockSelector, bool>(nameof(AutoChangeMode), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the AutoChangeMode property.
    /// </summary>
    public bool AutoChangeMode
    {
        get => GetValue(AutoChangeModeProperty);
        set => SetValue(AutoChangeModeProperty, value);
    }

    #endregion

    #region Mouse handlers

    private void ComponentIsDragged(object? sender, EventArgs e) => AutoChangeMode.IfTrue(MoveToNextComponent);

    #endregion

    protected override void AddComponentHandlers(IComponentTimeSelector component)
    {
        base.AddComponentHandlers(component);

        if (component is ClockTimeComponent clockTimeComponent)
            clockTimeComponent.IsDragged += ComponentIsDragged;
    }

    protected override void RemoveComponentHandlers(IComponentTimeSelector component)
    {
        base.RemoveComponentHandlers(component);

        if (component is ClockTimeComponent clockTimeComponent)
            clockTimeComponent.IsDragged -= ComponentIsDragged;
    }

    protected override void ShowComponent(IComponentTimeSelector component) => _carousel?.SelectedItem = component;
}
