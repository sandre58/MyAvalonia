// -----------------------------------------------------------------------
// <copyright file="Badge.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls;

[TemplatePart(PartBadgeContainer, typeof(Panel))]
public class Badge : HeaderedContentControl
{
    public const string PartBadgeContainer = "PART_BadgeContainer";

    private Panel? _badgeContainer;

    public static readonly StyledProperty<CornerPosition> CornerPositionProperty = AvaloniaProperty.Register<Badge, CornerPosition>(
        nameof(CornerPosition));

    public CornerPosition CornerPosition
    {
        get => GetValue(CornerPositionProperty);
        set => SetValue(CornerPositionProperty, value);
    }

    #region IsRounded

    /// <summary>
    /// Provides IsRounded Property.
    /// </summary>
    public static readonly StyledProperty<bool> IsRoundedProperty = AvaloniaProperty.Register<Badge, bool>(nameof(IsRounded));

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the IsRounded property.
    /// </summary>
    public bool IsRounded
    {
        get => GetValue(IsRoundedProperty);
        set => SetValue(IsRoundedProperty, value);
    }

    #endregion

    #region BadgeWidth

    /// <summary>
    /// Provides BadgeWidth Property.
    /// </summary>
    public static readonly StyledProperty<double> BadgeWidthProperty = AvaloniaProperty.Register<Badge, double>(nameof(BadgeWidth), double.NaN);

    /// <summary>
    /// Gets or sets the BadgeWidth property.
    /// </summary>
    public double BadgeWidth
    {
        get => GetValue(BadgeWidthProperty);
        set => SetValue(BadgeWidthProperty, value);
    }

    #endregion

    #region BadgeHeight

    /// <summary>
    /// Provides BadgeHeight Property.
    /// </summary>
    public static readonly StyledProperty<double> BadgeHeightProperty = AvaloniaProperty.Register<Badge, double>(nameof(BadgeHeight), double.NaN);

    /// <summary>
    /// Gets or sets the BadgeHeight property.
    /// </summary>
    public double BadgeHeight
    {
        get => GetValue(BadgeHeightProperty);
        set => SetValue(BadgeHeightProperty, value);
    }

    #endregion

    #region MaskBackground

    /// <summary>
    /// Provides MaskBackground Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> MaskBackgroundProperty = AvaloniaProperty.Register<Badge, IBrush?>(nameof(MaskBackground));

    /// <summary>
    /// Gets or sets the MaskBackground property.
    /// </summary>
    public IBrush? MaskBackground
    {
        get => GetValue(MaskBackgroundProperty);
        set => SetValue(MaskBackgroundProperty, value);
    }

    #endregion

    #region BadgeForeground

    /// <summary>
    /// Provides BadgeForeground Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> BadgeForegroundProperty = AvaloniaProperty.Register<Badge, IBrush?>(nameof(BadgeForeground));

    /// <summary>
    /// Gets or sets the BadgeForeground property.
    /// </summary>
    public IBrush? BadgeForeground
    {
        get => GetValue(BadgeForegroundProperty);
        set => SetValue(BadgeForegroundProperty, value);
    }

    #endregion

    #region BadgeFontFamily

    /// <summary>
    /// Provides BadgeFontFamily Property.
    /// </summary>
    public static readonly StyledProperty<FontFamily> BadgeFontFamilyProperty = AvaloniaProperty.Register<Badge, FontFamily>(nameof(BadgeFontFamily));

    /// <summary>
    /// Gets or sets the BadgeFontFamily property.
    /// </summary>
    public FontFamily BadgeFontFamily
    {
        get => GetValue(BadgeFontFamilyProperty);
        set => SetValue(BadgeFontFamilyProperty, value);
    }

    #endregion

    #region BadgeFontSize

    /// <summary>
    /// Provides BadgeFontSize Property.
    /// </summary>
    public static readonly StyledProperty<double> BadgeFontSizeProperty = AvaloniaProperty.Register<Badge, double>(nameof(BadgeFontSize));

    /// <summary>
    /// Gets or sets the BadgeFontSize property.
    /// </summary>
    public double BadgeFontSize
    {
        get => GetValue(BadgeFontSizeProperty);
        set => SetValue(BadgeFontSizeProperty, value);
    }

    #endregion

    #region BadgeFontWeight

    /// <summary>
    /// Provides BadgeFontWeight Property.
    /// </summary>
    public static readonly StyledProperty<FontWeight> BadgeFontWeightProperty = AvaloniaProperty.Register<Badge, FontWeight>(nameof(BadgeFontWeight));

    /// <summary>
    /// Gets or sets the BadgeFontWeight property.
    /// </summary>
    public FontWeight BadgeFontWeight
    {
        get => GetValue(BadgeFontWeightProperty);
        set => SetValue(BadgeFontWeightProperty, value);
    }

    #endregion

    #region OffsetX

    /// <summary>
    /// Provides OffsetX Property.
    /// </summary>
    public static readonly StyledProperty<double> OffsetXProperty = AvaloniaProperty.Register<Badge, double>(nameof(OffsetX));

    /// <summary>
    /// Gets or sets the OffsetX property.
    /// </summary>
    public double OffsetX
    {
        get => GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    #endregion

    #region OffsetY

    /// <summary>
    /// Provides OffsetY Property.
    /// </summary>
    public static readonly StyledProperty<double> OffsetYProperty = AvaloniaProperty.Register<Badge, double>(nameof(OffsetY));

    /// <summary>
    /// Gets or sets the OffsetY property.
    /// </summary>
    public double OffsetY
    {
        get => GetValue(OffsetYProperty);
        set => SetValue(OffsetYProperty, value);
    }

    #endregion

    static Badge()
    {
        _ = HeaderProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        _ = CornerPositionProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        _ = OffsetXProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        _ = OffsetYProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        _ = BadgeWidthProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        _ = BadgeHeightProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        _ = IsRoundedProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _badgeContainer?.RemoveHandler(SizeChangedEvent, OnBadgeSizeChanged);
        base.OnApplyTemplate(e);
        _badgeContainer = e.NameScope.Find<Panel>(PartBadgeContainer);
        _badgeContainer?.AddHandler(SizeChangedEvent, OnBadgeSizeChanged);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        UpdateBadgePosition();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        UpdateBadgePosition();
        return base.ArrangeOverride(finalSize);
    }

    private void OnBadgeSizeChanged(object? sender, SizeChangedEventArgs e) => UpdateBadgePosition();

    private void UpdateBadgePosition()
    {
        if (_badgeContainer is null) return;

        var vertical = CornerPosition is CornerPosition.TopLeft or CornerPosition.TopRight ? -1 : 1;
        var horizontal = CornerPosition is CornerPosition.BottomLeft or CornerPosition.TopLeft ? -1 : 1;

        _badgeContainer.HorizontalAlignment = CornerPosition is CornerPosition.TopLeft or CornerPosition.BottomLeft
            ? global::Avalonia.Layout.HorizontalAlignment.Left
            : global::Avalonia.Layout.HorizontalAlignment.Right;
        _badgeContainer.VerticalAlignment = CornerPosition is CornerPosition.BottomLeft or CornerPosition.BottomRight
            ? global::Avalonia.Layout.VerticalAlignment.Bottom
            : global::Avalonia.Layout.VerticalAlignment.Top;
        _badgeContainer.RenderTransform = new TransformGroup()
        {
            Children =
            [
                new TranslateTransform((horizontal * _badgeContainer.Bounds.Width / 2) + OffsetX, (vertical * _badgeContainer.Bounds.Height / 2) + OffsetY)
            ]
        };
    }
}
