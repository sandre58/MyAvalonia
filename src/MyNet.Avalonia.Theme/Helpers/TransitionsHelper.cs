// -----------------------------------------------------------------------
// <copyright file="TransitionsHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.Helpers;

public static class TransitionsHelper
{
    /// <summary>
    /// Creates an <see cref="IPageTransition"/> instance based on the specified parameters. The method uses a switch expression to determine the type of transition to create, and applies default values for any parameters that are not provided.
    /// </summary>
    /// <param name="type">The type of page transition to create.</param>
    /// <param name="duration">The duration of the transition.</param>
    /// <param name="orientation">The orientation for slide transitions.</param>
    /// <param name="fillMode">The fill mode for the transition.</param>
    /// <param name="easing">The easing function for the transition.</param>
    /// <returns>An <see cref="IPageTransition"/> instance configured with the specified values.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an unknown page transition type is specified.</exception>
    public static IPageTransition CreatePageTransition(PageTransitionType type, TimeSpan? duration = null, PageSlide.SlideAxis? orientation = null, FillMode? fillMode = null, Easing? easing = null)
    {
        var finalDuration = duration ?? TimeSpan.FromMilliseconds(300);
        var finalOrientation = orientation ?? PageSlide.SlideAxis.Horizontal;
        var finalFillMode = fillMode ?? FillMode.Forward;
        var finalEasing = easing ?? new LinearEasing();
        return type switch
        {
            PageTransitionType.None => null!,
            PageTransitionType.Slide => new PageSlide(finalDuration, finalOrientation) { FillMode = finalFillMode, SlideInEasing = finalEasing, SlideOutEasing = finalEasing },
            PageTransitionType.Crossfade => new CrossFade(finalDuration) { FillMode = finalFillMode, FadeInEasing = finalEasing, FadeOutEasing = finalEasing },
            PageTransitionType.Composite => new CompositePageTransition
            {
                PageTransitions =
                [
                    new PageSlide(finalDuration, finalOrientation) { FillMode = finalFillMode, SlideInEasing = finalEasing, SlideOutEasing = finalEasing },
                    new CrossFade(finalDuration) { FillMode = finalFillMode, FadeOutEasing = finalEasing, FadeInEasing = finalEasing }
                ]
            },
            _ => throw new InvalidOperationException($"Unknown page transition type: {type}")
        };
    }
}
