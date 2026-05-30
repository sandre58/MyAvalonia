// -----------------------------------------------------------------------
// <copyright file="PageTransitionExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Helpers;
using static Avalonia.Animation.PageSlide;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating page transitions with configurable parameters.
/// </summary>
public class PageTransitionExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageTransitionExtension"/> class.
    /// </summary>
    public PageTransitionExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PageTransitionExtension"/> class with the specified transition type.
    /// </summary>
    /// <param name="type">The type of page transition to create.</param>
    public PageTransitionExtension(PageTransitionType type) => Type = type;

    /// <summary>
    /// Gets or sets the type of page transition.
    /// </summary>
    [ConstructorArgument("type")]
    public PageTransitionType Type { get; set; } = PageTransitionType.Slide;

    /// <summary>
    /// Gets or sets the duration of the transition in milliseconds.
    /// </summary>
    public double Duration { get; set; } = 300;

    /// <summary>
    /// Gets or sets the orientation for slide transitions (Horizontal or Vertical).
    /// </summary>
    public SlideAxis Orientation { get; set; } = SlideAxis.Horizontal;

    /// <summary>
    /// Gets or sets the fill mode for the transition.
    /// </summary>
    public FillMode FillMode { get; set; } = FillMode.Forward;

    /// <summary>
    /// Gets or sets the easing function for the transition.
    /// </summary>
    public Easing Easing { get; set; } = new LinearEasing();

    /// <summary>
    /// Provides the value for the markup extension, returning an <see cref="IPageTransition"/> configured with the specified parameters.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>An <see cref="IPageTransition"/> instance configured with the specified values.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an unknown page transition type is specified.</exception>
    public override object ProvideValue(IServiceProvider serviceProvider) => TransitionsHelper.CreatePageTransition(Type, TimeSpan.FromMilliseconds(Duration), Orientation,  FillMode, Easing);
}
