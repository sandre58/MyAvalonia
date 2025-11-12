// -----------------------------------------------------------------------
// <copyright file="TickBar.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Primitives;

/// <summary>
/// An element that is used for drawing <see cref="Slider"/>'s Ticks.
/// </summary>
public class TickBar : Control
{
    static TickBar() => AffectsRender<TickBar>(FillProperty,
                               ForegroundProperty,
                               FontSizeProperty,
                               FontFamilyProperty,
                               IsDirectionReversedProperty,
                               ReservedSpaceProperty,
                               MaximumProperty,
                               MinimumProperty,
                               OrientationProperty,
                               PlacementProperty,
                               TickFrequencyProperty,
                               TicksProperty,
                               TickModeProperty);

    public static readonly StyledProperty<IBrush?> ForegroundProperty = AvaloniaProperty.Register<TickBar, IBrush?>(nameof(Foreground));

    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    #region FontSize

    /// <summary>
    /// Provides FontSize Property.
    /// </summary>
    public static readonly StyledProperty<double> FontSizeProperty = AvaloniaProperty.Register<TickBar, double>(nameof(FontSize), 12.0);

    /// <summary>
    /// Gets or sets the FontSize property.
    /// </summary>
    public double FontSize
    {
        get => GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    #endregion

    #region FontFamily

    /// <summary>
    /// Provides FontFamily Property.
    /// </summary>
    public static readonly StyledProperty<FontFamily> FontFamilyProperty = AvaloniaProperty.Register<TickBar, FontFamily>(nameof(FontFamily));

    /// <summary>
    /// Gets or sets the FontFamily property.
    /// </summary>
    public FontFamily FontFamily
    {
        get => GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    #endregion

    #region TickMode

    /// <summary>
    /// Provides TickMode Property.
    /// </summary>
    public static readonly StyledProperty<TickMode> TickModeProperty = AvaloniaProperty.Register<TickBar, TickMode>(nameof(TickMode));

    /// <summary>
    /// Gets or sets the TickMode property.
    /// </summary>
    public TickMode TickMode
    {
        get => GetValue(TickModeProperty);
        set => SetValue(TickModeProperty, value);
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="Fill"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> FillProperty =
        AvaloniaProperty.Register<TickBar, IBrush?>(nameof(Fill));

    /// <summary>
    /// Gets or sets brush used to fill the TickBar's Ticks.
    /// </summary>
    public IBrush? Fill
    {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Minimum"/> property.
    /// </summary>
    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<TickBar, double>(nameof(Minimum));

    /// <summary>
    /// Gets or sets logical position where the Minimum Tick will be drawn.
    /// </summary>
    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Maximum"/> property.
    /// </summary>
    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<TickBar, double>(nameof(Maximum));

    /// <summary>
    /// Gets or sets logical position where the Maximum Tick will be drawn.
    /// </summary>
    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="TickFrequency"/> property.
    /// </summary>
    public static readonly StyledProperty<double> TickFrequencyProperty =
        AvaloniaProperty.Register<TickBar, double>(nameof(TickFrequency));

    /// <summary>
    /// Gets or sets tickFrequency property defines how the tick will be drawn.
    /// </summary>
    public double TickFrequency
    {
        get => GetValue(TickFrequencyProperty);
        set => SetValue(TickFrequencyProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Orientation"/> property.
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<TickBar, Orientation>(nameof(Orientation));

    /// <summary>
    /// Gets or sets tickBar parent's orientation.
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Ticks"/> property.
    /// </summary>
    public static readonly StyledProperty<AvaloniaList<double>?> TicksProperty =
        AvaloniaProperty.Register<TickBar, AvaloniaList<double>?>(nameof(Ticks));

    /// <summary>
    /// Gets or sets the Ticks property contains collection of value of type Double which
    /// are the logical positions use to draw the ticks.
    /// The property value is a <see cref="AvaloniaList{T}" />.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Could be binded")]
    public AvaloniaList<double>? Ticks
    {
        get => GetValue(TicksProperty);
        set => SetValue(TicksProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="IsDirectionReversed"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsDirectionReversedProperty =
        AvaloniaProperty.Register<TickBar, bool>(nameof(IsDirectionReversed));

    /// <summary>
    /// Gets or sets a value indicating whether the IsDirectionReversed property defines the direction of value incrementation.
    /// By default, if Tick's orientation is Horizontal, ticks will be drawn from left to right.
    /// (And, bottom to top for Vertical orientation).
    /// If IsDirectionReversed is 'true' the direction of the drawing will be in opposite direction.
    /// </summary>
    public bool IsDirectionReversed
    {
        get => GetValue(IsDirectionReversedProperty);
        set => SetValue(IsDirectionReversedProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Placement"/> property.
    /// </summary>
    public static readonly StyledProperty<TickBarPlacement> PlacementProperty =
        AvaloniaProperty.Register<TickBar, TickBarPlacement>(nameof(Placement));

    /// <summary>
    /// Gets or sets placement property specified how the Tick will be placed.
    /// This property affects the way ticks are drawn.
    /// This property has type of <see cref="TickBarPlacement" />.
    /// </summary>
    public TickBarPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="ReservedSpace"/> property.
    /// </summary>
    public static readonly StyledProperty<Rect> ReservedSpaceProperty =
        AvaloniaProperty.Register<TickBar, Rect>(nameof(ReservedSpace));

    /// <summary>
    /// Gets or sets tickBar will use ReservedSpaceProperty for left and right spacing (for horizontal orientation) or
    /// top and bottom spacing (for vertical orientation).
    /// The space on both sides of TickBar is half of specified ReservedSpace.
    /// This property has type of <see cref="Rect" />.
    /// </summary>
    public Rect ReservedSpace
    {
        get => GetValue(ReservedSpaceProperty);
        set => SetValue(ReservedSpaceProperty, value);
    }

    /// <summary>
    /// <para>
    /// Draw ticks.
    /// Ticks can be draw in 8 different ways depends on Placement property and IsDirectionReversed property.
    /// </para>
    /// <para>
    /// This function also draw selection-tick(s) if IsSelectionRangeEnabled is 'true' and
    /// SelectionStart and SelectionEnd are valid.
    /// </para>
    /// <para>
    /// The primary ticks (for Minimum and Maximum value) height will be 100% of TickBar's render size (use Width or Height
    /// depends on Placement property).
    /// </para>
    /// <para>The secondary ticks (all other ticks, including selection-tics) height will be 75% of TickBar's render size.</para>
    /// <para>Brush that use to fill ticks is specified by Fill property.</para>
    /// </summary>
    public override void Render(DrawingContext context)
    {
        var size = new Size(Bounds.Width, Bounds.Height);
        var range = Maximum - Minimum;
        var coefficientTextHeight = 0;
        var coefficientTextWidth = 0;
        Point startPoint;
        Point endPoint;
        var rSpace = Orientation == Orientation.Horizontal ? ReservedSpace.Width : ReservedSpace.Height;

        // Take Thumb size in to account
        var halfReservedSpace = rSpace * 0.5;

        double tickLen;
        double logicalToPhysical;
        int spaceWithText;
        switch (Placement)
        {
            case TickBarPlacement.Top:
                if (rSpace.GreaterThanOrClose(size.Width))
                    return;

                spaceWithText = -3;
                coefficientTextHeight = -1;
                size = new Size(size.Width - rSpace, size.Height);
                tickLen = -size.Height;
                startPoint = new Point(halfReservedSpace, size.Height);
                endPoint = new Point(halfReservedSpace + size.Width, size.Height);
                logicalToPhysical = size.Width / range;
                break;

            case TickBarPlacement.Bottom:
                if (rSpace.GreaterThanOrClose(size.Width))
                    return;

                spaceWithText = 3;
                size = new Size(size.Width - rSpace, size.Height);
                tickLen = size.Height;
                startPoint = new Point(halfReservedSpace, 0d);
                endPoint = new Point(halfReservedSpace + size.Width, 0d);
                logicalToPhysical = size.Width / range;
                break;

            case TickBarPlacement.Left:
                if (rSpace.GreaterThanOrClose(size.Height))
                    return;

                spaceWithText = -3;
                coefficientTextWidth = -1;
                size = new Size(size.Width, size.Height - rSpace);

                tickLen = -size.Width;
                startPoint = new Point(size.Width, size.Height + halfReservedSpace);
                endPoint = new Point(size.Width, halfReservedSpace);
                logicalToPhysical = size.Height / range * -1;
                break;

            case TickBarPlacement.Right:
                if (rSpace.GreaterThanOrClose(size.Height))
                    return;

                spaceWithText = 3;
                size = new Size(size.Width, size.Height - rSpace);
                tickLen = size.Width;
                startPoint = new Point(0d, size.Height + halfReservedSpace);
                endPoint = new Point(0d, halfReservedSpace);
                logicalToPhysical = size.Height / range * -1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(context));
        }

        // Invert direction of the ticks
        if (IsDirectionReversed)
        {
            logicalToPhysical *= -1;

            // swap startPoint & endPoint
            (endPoint, startPoint) = (startPoint, endPoint);
        }

        if (TickMode == TickMode.Values)
        {
            tickLen = 0;
            spaceWithText = 0;
        }

        var pen = new ImmutablePen(Fill?.ToImmutable());

        // Is it Vertical?
        if (Placement is TickBarPlacement.Left or TickBarPlacement.Right)
        {
            // Reduce tick interval if it is more than would be visible on the screen
            var interval = TickFrequency;
            if (interval > 0.0)
            {
                var minInterval = (Maximum - Minimum) / size.Height;
                if (interval < minInterval)
                    interval = minInterval;
            }

            // Draw Floor & Ceiling tick
            if (TickMode != TickMode.Values)
            {
                context.DrawLine(pen, startPoint, new Point(startPoint.X + tickLen, startPoint.Y));
                context.DrawLine(pen, new Point(startPoint.X, endPoint.Y), new Point(startPoint.X + tickLen, endPoint.Y));
            }

            if (TickMode != TickMode.Tick)
            {
                var formattedText = new FormattedText($"{Minimum}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily), FontSize, Foreground);
                context.DrawText(formattedText, new Point(startPoint.X + tickLen + (coefficientTextWidth * formattedText.Width) + spaceWithText, startPoint.Y - (formattedText.Height / 2)));
                formattedText = new FormattedText($"{Maximum}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily), FontSize, Foreground);
                context.DrawText(formattedText, new Point(startPoint.X + tickLen + (coefficientTextWidth * formattedText.Width) + spaceWithText, endPoint.Y - (formattedText.Height / 2)));
            }

            // This property is rarely set so let's try to avoid the GetValue
            // caching of the mutable default value
            var ticks = Ticks ?? null;

            // Draw ticks using specified Ticks collection
            if (ticks?.Count > 0)
            {
                foreach (var t in ticks)
                {
                    if (t.LessThanOrClose(Minimum) || t.GreaterThanOrClose(Maximum))
                        continue;

                    var adjustedTick = t - Minimum;

                    var y = (adjustedTick * logicalToPhysical) + startPoint.Y;

                    if (TickMode != TickMode.Values)
                    {
                        context.DrawLine(pen,
                            new Point(startPoint.X, y),
                            new Point(startPoint.X + tickLen, y));
                    }

                    if (TickMode == TickMode.Tick)
                        continue;
                    var formattedText = new FormattedText($"{t}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily), FontSize, Foreground);
                    context.DrawText(formattedText, new Point(startPoint.X + tickLen + (coefficientTextWidth * formattedText.Width) + spaceWithText, y - (formattedText.Height / 2)));
                }
            }

            // Draw ticks using specified TickFrequency
            else if (interval > 0.0)
            {
                for (var i = interval; i < range; i += interval)
                {
                    var y = (i * logicalToPhysical) + startPoint.Y;

                    if (TickMode != TickMode.Values)
                    {
                        context.DrawLine(pen,
                        new Point(startPoint.X, y),
                        new Point(startPoint.X + tickLen, y));
                    }

                    if (TickMode == TickMode.Tick)
                        continue;
                    var formattedText = new FormattedText($"{Minimum + i}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily), FontSize, Foreground);
                    context.DrawText(formattedText, new Point(startPoint.X + tickLen + (coefficientTextWidth * formattedText.Width) + spaceWithText, y - (formattedText.Height / 2)));
                }
            }
        }

        // Placement == Top || Placement == Bottom
        else
        {
            // Reduce tick interval if it is more than would be visible on the screen
            var interval = TickFrequency;
            if (interval > 0.0)
            {
                var minInterval = (Maximum - Minimum) / size.Width;
                if (interval < minInterval)
                    interval = minInterval;
            }

            // Draw Floor & Ceiling tick
            if (TickMode != TickMode.Values)
            {
                context.DrawLine(pen, startPoint, new Point(startPoint.X, startPoint.Y + tickLen));
                context.DrawLine(pen, new Point(endPoint.X, startPoint.Y), new Point(endPoint.X, startPoint.Y + tickLen));
            }

            if (TickMode != TickMode.Tick)
            {
                var formattedText = new FormattedText($"{Minimum}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily), FontSize, Foreground);
                context.DrawText(formattedText, new Point(startPoint.X - (formattedText.Width / 2), startPoint.Y + tickLen + (coefficientTextHeight * formattedText.Height) + spaceWithText));
                formattedText = new FormattedText($"{Maximum}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily), FontSize, Foreground);
                context.DrawText(formattedText, new Point(endPoint.X - (formattedText.Width / 2), startPoint.Y + tickLen + (coefficientTextHeight * formattedText.Height) + spaceWithText));
            }

            // This property is rarely set so let's try to avoid the GetValue
            // caching of the mutable default value
            var ticks = Ticks ?? null;

            // Draw ticks using specified Ticks collection
            if (ticks?.Count > 0)
            {
                foreach (var t in ticks)
                {
                    if (t.LessThanOrClose(Minimum) || t.GreaterThanOrClose(Maximum))
                        continue;

                    var adjustedTick = t - Minimum;

                    var x = (adjustedTick * logicalToPhysical) + startPoint.X;
                    if (TickMode != TickMode.Values)
                    {
                        context.DrawLine(pen,
                            new Point(x, startPoint.Y),
                            new Point(x, startPoint.Y + tickLen));
                    }

                    if (TickMode == TickMode.Tick)
                        continue;
                    var formattedText = new FormattedText($"{t}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily), FontSize, Foreground);
                    context.DrawText(formattedText, new Point(x - (formattedText.Width / 2), startPoint.Y + tickLen + (coefficientTextHeight * formattedText.Height) + spaceWithText));
                }
            }

            // Draw ticks using specified TickFrequency
            else if (interval > 0.0)
            {
                for (var i = interval; i < range; i += interval)
                {
                    var x = (i * logicalToPhysical) + startPoint.X;
                    if (TickMode != TickMode.Values)
                    {
                        context.DrawLine(pen,
                        new Point(x, startPoint.Y),
                        new Point(x, startPoint.Y + tickLen));
                    }

                    if (TickMode == TickMode.Tick)
                        continue;
                    var formattedText = new FormattedText($"{Minimum + i}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily), FontSize, Foreground);
                    context.DrawText(formattedText, new Point(x - (formattedText.Width / 2), startPoint.Y + tickLen + (coefficientTextHeight * formattedText.Height) + spaceWithText));
                }
            }
        }
    }
}
