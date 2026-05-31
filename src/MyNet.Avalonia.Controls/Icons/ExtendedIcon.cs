// -----------------------------------------------------------------------
// <copyright file="ExtendedIcon.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class ExtendedIcon : PathIcon, IImage
{
    #region Constructor

    static ExtendedIcon()
    {
        AffectsRender<ExtendedIcon>(
            DataProperty,
            BorderBrushProperty,
            BackgroundProperty,
            ActiveForegroundProperty,
            ActiveBorderBrushProperty);
        IsActiveProperty.AffectsPseudoClass<ExtendedIcon>(PseudoClassName.Active);
    }

    public ExtendedIcon() => Drawing.Brush = Foreground;

    #endregion

    #region Properties

    public static readonly StyledProperty<double> IconSizeProperty = AvaloniaProperty.Register<ExtendedIcon, double>(nameof(IconSize), defaultValue: double.NaN);

    /// <summary>
    /// Gets or sets the uniform size of the icon.
    /// </summary>
    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<ExtendedIcon, bool>(nameof(IsActive), defaultBindingMode: BindingMode.TwoWay);

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ActiveForegroundProperty = AvaloniaProperty.Register<ExtendedIcon, IBrush?>(nameof(ActiveForeground));

    public IBrush? ActiveForeground
    {
        get => GetValue(ActiveForegroundProperty);
        set => SetValue(ActiveForegroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ActiveBorderBrushProperty = AvaloniaProperty.Register<ExtendedIcon, IBrush?>(nameof(ActiveBorderBrush));

    public IBrush? ActiveBorderBrush
    {
        get => GetValue(ActiveBorderBrushProperty);
        set => SetValue(ActiveBorderBrushProperty, value);
    }

    public static readonly DirectProperty<ExtendedIcon, GeometryDrawing> DrawingProperty = AvaloniaProperty.RegisterDirect<ExtendedIcon, GeometryDrawing>(nameof(Drawing), o => o.Drawing);

    /// <summary>
    /// Gets the <see cref="GeometryDrawing"/> of the icon.
    /// </summary>
    public GeometryDrawing Drawing { get; } = new();

    // Default size for Material Icons
    private static readonly Rect DefaultIconBounds = new(0, 0, 24, 24);

    #endregion

    #region Overrides

    /// <inheritdoc />
    protected override void OnLoaded(RoutedEventArgs e)
    {
        if (Drawing.Geometry is null)
            UpdateGeometry();
        base.OnLoaded(e);
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataProperty)
        {
            UpdateGeometry();
        }
        else if (change.Property == ForegroundProperty)
        {
            Drawing.Brush = Foreground;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PseudoClasses.Set(PseudoClassName.Active, IsActive);
    }

    #endregion

    #region Methods

    protected virtual Geometry? ProvideGeometry() => Data;

    protected void UpdateGeometry() => Drawing.Geometry = ProvideGeometry();

    #endregion

    #region IImage Implementation

    /// <inheritdoc/>
    public Size Size => DefaultIconBounds.Size;

    /// <inheritdoc/>
    public void Draw(DrawingContext context, Rect sourceRect, Rect destRect)
    {
        if (Drawing.Geometry is null)
            UpdateGeometry();

        var bounds = DefaultIconBounds;
        var scale = Matrix.CreateScale(
            destRect.Width / sourceRect.Width,
            destRect.Height / sourceRect.Height);
        var translate = Matrix.CreateTranslation(
            -sourceRect.X + destRect.X - bounds.X,
            -sourceRect.Y + destRect.Y - bounds.Y);

        using (context.PushClip(destRect))
        using (context.PushTransform(translate * scale))
        {
            Drawing.Draw(context);
        }
    }

    #endregion
}
