// -----------------------------------------------------------------------
// <copyright file="Loader.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Indeterminate loading animation. Decoupled from <c>IBusyService</c>; use inside buttons, cards, or
/// <see cref="BusyIndicator"/> overlays.
/// </summary>
[PseudoClasses(PseudoClassName.Active, PseudoClassName.Inactive)]
[TemplatePart("PART_Animation", typeof(ContentPresenter))]
public class Loader : TemplatedControl
{
    private const string AnimationsActiveClass = "animations-active";

    private ContentPresenter? _animationPresenter;

    /// <summary>
    /// Defines the <see cref="Animation"/> property.
    /// </summary>
    public static readonly StyledProperty<LoaderAnimation> AnimationProperty =
        AvaloniaProperty.Register<Loader, LoaderAnimation>(nameof(Animation));

    /// <summary>
    /// Defines the <see cref="IsActive"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<Loader, bool>(nameof(IsActive), true);

    /// <summary>
    /// Defines the <see cref="StrokeThickness"/> property.
    /// </summary>
    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<Loader, double>(nameof(StrokeThickness), 2.5d);

    /// <summary>
    /// Defines the <see cref="DotSize"/> property.
    /// </summary>
    public static readonly StyledProperty<double> DotSizeProperty =
        AvaloniaProperty.Register<Loader, double>(nameof(DotSize), 6.0d);

    /// <summary>
    /// Defines the <see cref="BarWidth"/> property.
    /// </summary>
    public static readonly StyledProperty<double> BarWidthProperty =
        AvaloniaProperty.Register<Loader, double>(nameof(BarWidth), 3.0d);

    static Loader()
    {
        WidthProperty.OverrideDefaultValue<Loader>(32);
        HeightProperty.OverrideDefaultValue<Loader>(32);
        FocusableProperty.OverrideDefaultValue<Loader>(false);

        IsActiveProperty.Changed.AddClassHandler<Loader>((control, _) => control.UpdateActiveState());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Loader"/> class.
    /// </summary>
    public Loader() => UpdateActiveState();

    /// <summary>
    /// Gets or sets the visual animation style.
    /// </summary>
    public LoaderAnimation Animation
    {
        get => GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the animation is running.
    /// </summary>
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    /// <summary>
    /// Gets or sets the stroke thickness for arc and ring animations.
    /// </summary>
    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the dot diameter for the <see cref="LoaderAnimation.Dots"/> animation.
    /// </summary>
    public double DotSize
    {
        get => GetValue(DotSizeProperty);
        set => SetValue(DotSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the bar width for the <see cref="LoaderAnimation.Bars"/> animation.
    /// </summary>
    public double BarWidth
    {
        get => GetValue(BarWidthProperty);
        set => SetValue(BarWidthProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _animationPresenter = e.NameScope.Find<ContentPresenter>("PART_Animation");
        UpdateActiveState();
    }

    private void UpdateActiveState()
    {
        var isActive = IsActive;
        PseudoClasses.Set(PseudoClassName.Active, isActive);
        PseudoClasses.Set(PseudoClassName.Inactive, !isActive);
        _animationPresenter?.Classes.Set(AnimationsActiveClass, isActive);
    }
}
