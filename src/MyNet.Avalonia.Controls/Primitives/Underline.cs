// -----------------------------------------------------------------------
// <copyright file="Underline.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls.Primitives;

public class Underline : TemplatedControl
{
    #region ActiveBrush

    /// <summary>
    /// Defines the <see cref="ActiveBrush"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> ActiveBrushProperty = AvaloniaProperty.Register<Underline, IBrush?>(nameof(ActiveBrush));

    public IBrush? ActiveBrush
    {
        get => GetValue(ActiveBrushProperty);
        set => SetValue(ActiveBrushProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="IsActive"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<Underline, bool>(nameof(IsActive));

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    #endregion ActiveBrush

    #region ActiveHeight

    /// <summary>
    /// Provides ActiveHeight Property.
    /// </summary>
    public static readonly StyledProperty<double> ActiveHeightProperty = AvaloniaProperty.Register<Underline, double>(nameof(ActiveHeight), 2.0);

    /// <summary>
    /// Gets or sets the ActiveHeight property.
    /// </summary>
    public double ActiveHeight
    {
        get => GetValue(ActiveHeightProperty);
        set => SetValue(ActiveHeightProperty, value);
    }

    #endregion

    #region InactiveHeight

    /// <summary>
    /// Provides InactiveHeight Property.
    /// </summary>
    public static readonly StyledProperty<double> InactiveHeightProperty = AvaloniaProperty.Register<Underline, double>(nameof(InactiveHeight), 1.0);

    /// <summary>
    /// Gets or sets the InactiveHeight property.
    /// </summary>
    public double InactiveHeight
    {
        get => GetValue(InactiveHeightProperty);
        set => SetValue(InactiveHeightProperty, value);
    }

    #endregion
}
