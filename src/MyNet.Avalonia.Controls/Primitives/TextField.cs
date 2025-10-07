// -----------------------------------------------------------------------
// <copyright file="TextField.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Proxy;

namespace MyNet.Avalonia.Controls.Primitives;

[PseudoClasses(PseudoClassName.Active, PseudoClassName.Empty, PseudoClassName.Floating)]
public class TextField : ContentControl
{
    static TextField() => ProxyProperty.Changed.AddClassHandler<TextField, IControlProxy>((o, e) => o.OnProxyChanged(e));

    #region Watermark

    /// <summary>
    /// Defines the <see cref="Watermark"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> WatermarkProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Watermark));

    /// <summary>
    /// Gets or sets the placeholder or descriptive text that is displayed even if the text.
    /// property is not yet set.
    /// </summary>
    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    #endregion

    #region UseFloatingWatermark

    /// <summary>
    /// Defines the <see cref="UseFloatingWatermark"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> UseFloatingWatermarkProperty =
        AvaloniaProperty.Register<TextField, bool>(nameof(UseFloatingWatermark));

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Watermark"/> will still be shown above the
    /// text even after a text value is set.
    /// </summary>
    public bool UseFloatingWatermark
    {
        get => GetValue(UseFloatingWatermarkProperty);
        set => SetValue(UseFloatingWatermarkProperty, value);
    }

    #endregion

    #region FloatingScale

    /// <summary>
    /// Provides FloatingScale Property.
    /// </summary>
    public static readonly StyledProperty<double> FloatingScaleProperty = AvaloniaProperty.Register<TextField, double>(nameof(FloatingScale), 0.75d);

    /// <summary>
    /// Gets or sets the FloatingScale property.
    /// </summary>
    public double FloatingScale
    {
        get => GetValue(FloatingScaleProperty);
        set => SetValue(FloatingScaleProperty, value);
    }

    #endregion

    #region FloatingOffset

    /// <summary>
    /// Provides FloatingOffset Property.
    /// </summary>
    public static readonly StyledProperty<double> FloatingOffsetProperty = AvaloniaProperty.Register<TextField, double>(nameof(FloatingOffset), 12.0d);

    /// <summary>
    /// Gets or sets the FloatingOffset property.
    /// </summary>
    public double FloatingOffset
    {
        get => GetValue(FloatingOffsetProperty);
        set => SetValue(FloatingOffsetProperty, value);
    }

    #endregion

    #region CurrentFloatingScale

    /// <summary>
    /// Provides CurrentFloatingScale Property.
    /// </summary>
    public static readonly StyledProperty<double> CurrentFloatingScaleProperty = AvaloniaProperty.Register<TextField, double>(nameof(CurrentFloatingScale), 1.0d);

    /// <summary>
    /// Gets or sets the CurrentFloatingScale property.
    /// </summary>
    public double CurrentFloatingScale
    {
        get => GetValue(CurrentFloatingScaleProperty);
        set => SetValue(CurrentFloatingScaleProperty, value);
    }

    #endregion

    #region CurrentFloatingOffset

    /// <summary>
    /// Provides CurrentFloatingOffset Property.
    /// </summary>
    public static readonly StyledProperty<double> CurrentFloatingOffsetProperty = AvaloniaProperty.Register<TextField, double>(nameof(CurrentFloatingOffset));

    /// <summary>
    /// Gets or sets the CurrentFloatingOffset property.
    /// </summary>
    public double CurrentFloatingOffset
    {
        get => GetValue(CurrentFloatingOffsetProperty);
        set => SetValue(CurrentFloatingOffsetProperty, value);
    }

    #endregion

    #region ActiveForeground

    /// <summary>
    /// Defines the <see cref="ActiveForeground"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> ActiveForegroundProperty = AvaloniaProperty.Register<TextField, IBrush?>(nameof(ActiveForeground));

    public IBrush? ActiveForeground
    {
        get => GetValue(ActiveForegroundProperty);
        set => SetValue(ActiveForegroundProperty, value);
    }

    #endregion ActiveForeground

    #region InactiveForeground

    /// <summary>
    /// Provides InactiveForeground Property.
    /// </summary>
    public static readonly StyledProperty<IBrush> InactiveForegroundProperty = AvaloniaProperty.Register<TextField, IBrush>(nameof(InactiveForeground));

    /// <summary>
    /// Gets or sets the InactiveForeground property.
    /// </summary>
    public IBrush InactiveForeground
    {
        get => GetValue(InactiveForegroundProperty);
        set => SetValue(InactiveForegroundProperty, value);
    }

    #endregion

    #region WatermarkFontSize

    /// <summary>
    /// Provides WatermarkFontSize Property.
    /// </summary>
    public static readonly StyledProperty<double> WatermarkFontSizeProperty = AvaloniaProperty.Register<TextField, double>(nameof(WatermarkFontSize));

    /// <summary>
    /// Gets or sets the WatermarkFontSize property.
    /// </summary>
    public double WatermarkFontSize
    {
        get => GetValue(WatermarkFontSizeProperty);
        set => SetValue(WatermarkFontSizeProperty, value);
    }

    #endregion

    #region InnerLeftContent

    /// <summary>
    /// Defines the <see cref="InnerLeftContent"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<TextField, object?>(nameof(InnerLeftContent));

    /// <summary>
    /// Gets or sets custom content that is positioned on the left side of the text layout box.
    /// </summary>
    public object? InnerLeftContent
    {
        get => GetValue(InnerLeftContentProperty);
        set => SetValue(InnerLeftContentProperty, value);
    }

    #endregion

    #region InnerRightContent

    /// <summary>
    /// Defines the <see cref="InnerRightContent"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<TextField, object?>(nameof(InnerRightContent));

    /// <summary>
    /// Gets or sets custom content that is positioned on the right side of the text layout box.
    /// </summary>
    public object? InnerRightContent
    {
        get => GetValue(InnerRightContentProperty);
        set => SetValue(InnerRightContentProperty, value);
    }

    #endregion

    #region InnerForeground

    /// <summary>
    /// Provides InnerForeground Property.
    /// </summary>
    public static readonly StyledProperty<IBrush> InnerForegroundProperty = AvaloniaProperty.Register<TextField, IBrush>(nameof(InnerForeground));

    /// <summary>
    /// Gets or sets the InnerForeground property.
    /// </summary>
    public IBrush InnerForeground
    {
        get => GetValue(InnerForegroundProperty);
        set => SetValue(InnerForegroundProperty, value);
    }

    #endregion

    #region InnerFontSize

    /// <summary>
    /// Provides InnerFontSize Property.
    /// </summary>
    public static readonly StyledProperty<double> InnerFontSizeProperty = AvaloniaProperty.Register<TextField, double>(nameof(InnerFontSize));

    /// <summary>
    /// Gets or sets the InnerFontSize property.
    /// </summary>
    public double InnerFontSize
    {
        get => GetValue(InnerFontSizeProperty);
        set => SetValue(InnerFontSizeProperty, value);
    }

    #endregion

    #region Proxy

    /// <summary>
    /// Provides Proxy Property.
    /// </summary>
    public static readonly StyledProperty<IControlProxy> ProxyProperty = AvaloniaProperty.Register<TextField, IControlProxy>(nameof(Proxy));

    /// <summary>
    /// Gets or sets the Proxy property.
    /// </summary>
    public IControlProxy Proxy
    {
        get => GetValue(ProxyProperty);
        set => SetValue(ProxyProperty, value);
    }

    private void OnProxyChanged(AvaloniaPropertyChangedEventArgs<IControlProxy> args)
    {
        if (args.Sender is not TextField textField) return;

        if (args.OldValue.Value is { } oldHintProxy)
        {
            oldHintProxy.IsEmptyChanged -= textField.IsEmptyChangedCallback;
            oldHintProxy.IsFocusedChanged -= textField.IsFocusedChangedCallback;
            oldHintProxy.IsActiveChanged -= textField.IsActiveChangedCallback;
            oldHintProxy.Dispose();
        }

        if (args.NewValue.Value is not { } newHintProxy)
            return;
        newHintProxy.IsEmptyChanged += textField.IsEmptyChangedCallback;
        newHintProxy.IsFocusedChanged += textField.IsFocusedChangedCallback;
        newHintProxy.IsActiveChanged += textField.IsActiveChangedCallback;

        RefreshIsActive();
        RefreshIsFloating();
        RefreshIsEmpty();
    }

    private void IsEmptyChangedCallback(object? sender, System.EventArgs e) => RefreshIsEmpty();

    private void IsFocusedChangedCallback(object? sender, System.EventArgs e) => RefreshIsActive();

    private void IsActiveChangedCallback(object? sender, System.EventArgs e)
    {
        RefreshIsActive();
        RefreshIsFloating();
    }

    private void RefreshIsActive()
    {
        var isFloating = Proxy.IsActive();
        var isFocused = Proxy.IsFocused();
        PseudoClasses.Set(PseudoClassName.Active, isFocused && ((UseFloatingWatermark && isFloating) || !UseFloatingWatermark));
    }

    private void RefreshIsFloating()
    {
        var isFloating = Proxy.IsActive();
        PseudoClasses.Set(PseudoClassName.Floating, UseFloatingWatermark && isFloating);
    }

    private void RefreshIsEmpty()
    {
        var isEmpty = Proxy.IsEmpty();
        PseudoClasses.Set(PseudoClassName.Empty, isEmpty);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        RefreshIsActive();
        RefreshIsFloating();
        RefreshIsEmpty();
    }
}
