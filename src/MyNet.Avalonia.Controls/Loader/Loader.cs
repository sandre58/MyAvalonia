// -----------------------------------------------------------------------
// <copyright file="Loader.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Indeterminate loading animation. Decoupled from <c>IBusyService</c>; use inside buttons, cards, or
/// <see cref="BusyIndicator"/> overlays.
/// </summary>
[PseudoClasses(
    "circular",
    "ring",
    "dots",
    "bars",
    "pulse",
    PseudoClassName.Circular,
    PseudoClassName.Ring,
    PseudoClassName.Dots,
    PseudoClassName.Bars,
    PseudoClassName.Pulse,
    PseudoClassName.Active,
    PseudoClassName.Inactive)]
public class Loader : TemplatedControl
{
    /// <summary>
    /// Defines the <see cref="Animation"/> property.
    /// </summary>
    public static readonly StyledProperty<LoaderAnimation> AnimationProperty = AvaloniaProperty.Register<Loader, LoaderAnimation>(nameof(Animation));

    /// <summary>
    /// Defines the <see cref="IsActive"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<Loader, bool>(nameof(IsActive), true);

    /// <summary>
    /// Defines the <see cref="StrokeThickness"/> property.
    /// </summary>
    public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<Loader, double>(nameof(StrokeThickness), 2.5d);

    static Loader()
    {
        WidthProperty.OverrideDefaultValue<Loader>(32);
        HeightProperty.OverrideDefaultValue<Loader>(32);
        FocusableProperty.OverrideDefaultValue<Loader>(false);

        AnimationProperty.Changed.AddClassHandler<Loader>((control, _) => control.UpdateAnimationState());
        IsActiveProperty.Changed.AddClassHandler<Loader>((control, _) => control.UpdateActiveState());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Loader"/> class.
    /// </summary>
    public Loader()
    {
        UpdateAnimationState();
        UpdateActiveState();
    }

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

    private void UpdateAnimationState()
    {
        var animation = Animation;
        PseudoClasses.Set(PseudoClassName.Circular, animation == LoaderAnimation.Circular);
        PseudoClasses.Set(PseudoClassName.Ring, animation == LoaderAnimation.Ring);
        PseudoClasses.Set(PseudoClassName.Dots, animation == LoaderAnimation.Dots);
        PseudoClasses.Set(PseudoClassName.Bars, animation == LoaderAnimation.Bars);
        PseudoClasses.Set(PseudoClassName.Pulse, animation == LoaderAnimation.Pulse);
    }

    private void UpdateActiveState()
    {
        var isActive = IsActive;
        PseudoClasses.Set(PseudoClassName.Active, isActive);
        PseudoClasses.Set(PseudoClassName.Inactive, !isActive);
    }
}
