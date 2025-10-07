// -----------------------------------------------------------------------
// <copyright file="EyeDropperHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace MyNet.Avalonia.Controls.Helpers;

public static class EyeDropperHelper
{
    public static Color GetPixelColor(PixelPoint pixelPoint)
    {
        var bitmap = new RenderTargetBitmap(new PixelSize(1, 1), new Vector(96, 96));
        using (var ctx = bitmap.CreateDrawingContext())
        {
            ctx.DrawRectangle(Brushes.Transparent, null, new Rect(pixelPoint.X, pixelPoint.Y, 1, 1));
        }

        var buffer = new byte[4];
        unsafe
        {
            fixed (byte* p = buffer)
            {
                bitmap.CopyPixels(new PixelRect(0, 0, 1, 1), (nint)p, 4, 4);
            }
        }

        var color = Color.FromArgb(
            buffer[3],
            buffer[2],
            buffer[1],
            buffer[0]);

        return color;
    }

    public static RenderTargetBitmap CaptureRegion(Rect region)
    {
        var bitmap = new RenderTargetBitmap(new PixelSize((int)region.Width, (int)region.Height), new Vector(96, 96));
        using var ctx = bitmap.CreateDrawingContext();
        ctx.DrawRectangle(Brushes.Transparent, null, region);

        return bitmap;
    }
}
