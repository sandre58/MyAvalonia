// -----------------------------------------------------------------------
// <copyright file="BorderClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Represents a utility class that provides methods for registering and managing border-related properties for various UI controls.
/// </summary>
public static class BorderClassRegistry
{
    /// <summary>
    /// Registers predefined border thickness values for supported control types, enabling dynamic styling based on
    /// thickness indices.
    /// </summary>
    /// <remarks>This method associates border thickness values ranging from 0 to 7 with various controls,
    /// including TemplatedControl, Border, Ellipse, and Rectangle. The registered thickness can be applied dynamically
    /// using the corresponding CSS class, allowing for flexible UI customization.</remarks>
    public static void RegisterBorderThickness()
    {
        for (var i = 0; i < 8; i++)
        {
            ClassRegistry.Register<Control>(CssClass.Border(i), x => setBorderThickness(x, new(i)));
        }

        static IDisposable setBorderThickness(Control control, Thickness thickness) => control switch
        {
            TemplatedControl templatedControl => templatedControl.SetProperty(TemplatedControl.BorderThicknessProperty, thickness),
            Border border => border.SetProperty(Border.BorderThicknessProperty, thickness),
            Ellipse ellipse => ellipse.SetProperty(Shape.StrokeThicknessProperty, thickness.Left),
            Rectangle rectangle => rectangle.SetProperty(Shape.StrokeThicknessProperty, thickness.Left),
            _ => Disposable.Empty
        };
    }

    /// <summary>
    /// Registers the corner radius property for supported control types, enabling consistent application of rounded
    /// corners across UI elements.
    /// </summary>
    /// <remarks>This method configures the corner radius setting for controls such as TemplatedControl,
    /// Border, and Rectangle. Only controls that support corner radius adjustments will be affected. Use this method to
    /// ensure that custom or themed controls respond to corner radius styling in a unified manner.</remarks>
    public static void RegisterCornerRadius()
    {
        ClassRegistry.RegisterMany<CornerSize, Control>(CssPrefix.CornerRadius, (x, y) => setCornerRadius(x, ThemeResources.Corners.Get(y).Value));

        static IDisposable setCornerRadius(Control control, CornerRadius cornerRadius) => control switch
        {
            TemplatedControl templatedControl => templatedControl.SetProperty(TemplatedControl.CornerRadiusProperty, cornerRadius),
            Border border => border.SetProperty(Border.CornerRadiusProperty, cornerRadius),
            Rectangle rectangle => new CompositeDisposable
                    {
                        rectangle.SetProperty(Rectangle.RadiusXProperty, cornerRadius.TopLeft),
                        rectangle.SetProperty(Rectangle.RadiusYProperty, cornerRadius.TopRight)
                    },
            _ => Disposable.Empty
        };
    }
}
