// -----------------------------------------------------------------------
// <copyright file="ShadowAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.Assists;

public static class ShadowAssist
{
    #region ShadowDepth

    public static readonly AvaloniaProperty<ShadowDepth> ShadowDepthProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ShadowDepth>("ShadowDepth", typeof(ShadowAssist));

    public static void SetShadowDepth(AvaloniaObject element, ShadowDepth value) => element.SetValue(ShadowDepthProperty, value);

    public static ShadowDepth GetShadowDepth(AvaloniaObject element) => element.GetValue<ShadowDepth>(ShadowDepthProperty);

    private static void ShadowDepthChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        switch (args.Sender)
        {
            case Border border:
                border.BoxShadow = (args.NewValue as ShadowDepth? ?? ShadowDepth.Depth0).ToBoxShadows();
                break;
            case Ellipse ellipse:
                ellipse.Effect = (args.NewValue as ShadowDepth? ?? ShadowDepth.Depth0).ToBoxShadows().ToDropShadowEffect();
                break;
            default:
                break;
        }
    }

    #endregion ShadowDepth

    #region Darken

    public static readonly AvaloniaProperty<bool> DarkenProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>("Darken", typeof(ShadowAssist));

    public static void SetDarken(AvaloniaObject element, bool value) => element.SetValue(DarkenProperty, value);

    public static bool GetDarken(AvaloniaObject element) => element.GetValue<bool>(DarkenProperty);

    static ShadowAssist()
    {
        _ = ShadowDepthProperty.Changed.Subscribe(ShadowDepthChangedCallback);
        _ = DarkenProperty.Changed.Subscribe(DarkenPropertyChangedCallback);
    }

    private static void DarkenPropertyChangedCallback(AvaloniaPropertyChangedEventArgs obj)
    {
        switch (obj.Sender)
        {
            case Border border:
                {
                    var targetBoxShadows = (bool?)obj.NewValue == true
                        ? GetShadowDepth(border).ToBoxShadows(Colors.Black)
                        : GetShadowDepth(border).ToBoxShadows();

                    _ = border.SetValue(Border.BoxShadowProperty, targetBoxShadows);
                    break;
                }

            case Ellipse ellipse:
                {
                    var targetBoxShadows = (bool?)obj.NewValue == true
                        ? GetShadowDepth(ellipse).ToBoxShadows(Colors.Black)
                        : GetShadowDepth(ellipse).ToBoxShadows();

                    _ = ellipse.SetValue(Visual.EffectProperty, targetBoxShadows.ToDropShadowEffect());
                    break;
                }

            default:
                break;
        }
    }
    #endregion Darken
}

public static class ShadowProvider
{
    public static Color ShadowColor { get; set; } = Color.FromArgb(76, 0, 0, 0);

    public static BoxShadows ToBoxShadows(this ShadowDepth shadowDepth, Color? overrideColor = null)
        => shadowDepth switch
        {
            ShadowDepth.Depth0 => new BoxShadows(default),
            ShadowDepth.Depth1 => new BoxShadows(new BoxShadow
            { Blur = 5, OffsetX = 1, OffsetY = 1, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.Depth2 => new BoxShadows(new BoxShadow
            { Blur = 8, OffsetX = 1.5, OffsetY = 1.5, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.Depth3 => new BoxShadows(new BoxShadow
            { Blur = 14, OffsetX = 4.5, OffsetY = 4.5, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.Depth4 => new BoxShadows(new BoxShadow
            { Blur = 25, OffsetX = 8, OffsetY = 8, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.Depth5 => new BoxShadows(new BoxShadow
            { Blur = 35, OffsetX = 13, OffsetY = 13, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.CenterDepth1 => new BoxShadows(new BoxShadow
            { Blur = 5, OffsetY = 1, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.CenterDepth2 => new BoxShadows(new BoxShadow
            { Blur = 8, OffsetY = 1.5, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.CenterDepth3 => new BoxShadows(new BoxShadow
            { Blur = 14, OffsetY = 4.5, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.CenterDepth4 => new BoxShadows(new BoxShadow
            { Blur = 25, OffsetY = 8, Color = overrideColor ?? ShadowColor }),
            ShadowDepth.CenterDepth5 => new BoxShadows(new BoxShadow
            { Blur = 35, OffsetY = 13, Color = overrideColor ?? ShadowColor }),
            _ => throw new ArgumentOutOfRangeException(nameof(shadowDepth))
        };

    public static DropShadowEffect? ToDropShadowEffect(this BoxShadows boxShadows) => boxShadows.Count > 0 ? new()
    {
        BlurRadius = boxShadows[0].Blur,
        Color = boxShadows[0].Color,
        OffsetX = boxShadows[0].OffsetX,
        OffsetY = boxShadows[0].OffsetY
    }
    : null;
}
