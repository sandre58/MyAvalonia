// -----------------------------------------------------------------------
// <copyright file="RippleHandler.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Rendering.Composition;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal sealed class RippleHandler(IImmutableBrush brush,
    Easing easing,
    TimeSpan duration,
    double opacity,
    Point center,
    double radius,
    bool transitions) : CompositionCustomVisualHandler
{
    public static readonly object FirstStepMessage = new();
    public static readonly object SecondStepMessage = new();

    private TimeSpan _animationElapsed;
    private TimeSpan? _lastServerTime;
    private TimeSpan? _secondStepStart;

    public override void OnRender(ImmediateDrawingContext drawingContext)
    {
        if (_lastServerTime.HasValue) _animationElapsed += CompositionNow - _lastServerTime.Value;
        _lastServerTime = CompositionNow;

        var currentRadius = radius;
        var currentOpacity = opacity;

        if (transitions)
        {
            var expandingStep = easing.Ease((double)_animationElapsed.Ticks / duration.Ticks);
            currentRadius = radius * expandingStep;

            if (_secondStepStart is { } secondStepStart)
            {
                var opacityStep = easing.Ease((double)(_animationElapsed - secondStepStart).Ticks /
                                               (duration - secondStepStart).Ticks);
                currentOpacity = opacity - (opacity * opacityStep);
            }
        }

        using (drawingContext.PushOpacity(currentOpacity, default))
        {
            drawingContext.DrawEllipse(brush, null, center, currentRadius, currentRadius);
        }
    }

    public override void OnMessage(object message)
    {
        if (message == FirstStepMessage)
        {
            _lastServerTime = null;
            _secondStepStart = null;
            RegisterForNextAnimationFrameUpdate();
        }
        else if (message == SecondStepMessage)
        {
            _secondStepStart = _animationElapsed;
        }
    }

    public override void OnAnimationFrameUpdate()
    {
        if (_animationElapsed >= duration) return;
        Invalidate();
        RegisterForNextAnimationFrameUpdate();
    }
}
