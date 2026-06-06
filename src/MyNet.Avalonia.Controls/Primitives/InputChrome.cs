// -----------------------------------------------------------------------
// <copyright file="InputChrome.cs" company="Stéphane ANDRE">
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
public class InputChrome : ContentControl
{
    static InputChrome() => ProxyProperty.Changed.AddClassHandler<InputChrome, IControlProxy>((o, e) => o.OnProxyChanged(e));

    #region PlaceholderText

    /// <summary>
    /// Defines the <see cref="PlaceholderText"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> PlaceholderTextProperty = AvaloniaProperty.Register<InputChrome, string?>(nameof(PlaceholderText));

    /// <summary>
    /// Gets or sets the placeholder or descriptive text that is displayed even if the text.
    /// property is not yet set.
    /// </summary>
    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    #endregion

    #region UseFloatingPlaceholder

    /// <summary>
    /// Defines the <see cref="UseFloatingPlaceholder"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> UseFloatingPlaceholderProperty = AvaloniaProperty.Register<InputChrome, bool>(nameof(UseFloatingPlaceholder));

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="PlaceholderText"/> will still be shown above the
    /// text even after a text value is set.
    /// </summary>
    public bool UseFloatingPlaceholder
    {
        get => GetValue(UseFloatingPlaceholderProperty);
        set => SetValue(UseFloatingPlaceholderProperty, value);
    }

    #endregion

    #region FloatingScale

    /// <summary>
    /// Provides FloatingScale Property.
    /// </summary>
    public static readonly StyledProperty<double> FloatingScaleProperty = AvaloniaProperty.Register<InputChrome, double>(nameof(FloatingScale), 0.75d);

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
    public static readonly StyledProperty<double> FloatingOffsetProperty = AvaloniaProperty.Register<InputChrome, double>(nameof(FloatingOffset), 12.0d);

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
    public static readonly StyledProperty<double> CurrentFloatingScaleProperty = AvaloniaProperty.Register<InputChrome, double>(nameof(CurrentFloatingScale), 1.0d);

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
    public static readonly StyledProperty<double> CurrentFloatingOffsetProperty = AvaloniaProperty.Register<InputChrome, double>(nameof(CurrentFloatingOffset));

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
    public static readonly StyledProperty<IBrush?> ActiveForegroundProperty = AvaloniaProperty.Register<InputChrome, IBrush?>(nameof(ActiveForeground));

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
    public static readonly StyledProperty<IBrush> InactiveForegroundProperty = AvaloniaProperty.Register<InputChrome, IBrush>(nameof(InactiveForeground));

    /// <summary>
    /// Gets or sets the InactiveForeground property.
    /// </summary>
    public IBrush InactiveForeground
    {
        get => GetValue(InactiveForegroundProperty);
        set => SetValue(InactiveForegroundProperty, value);
    }

    #endregion

    #region PlaceholderFontSize

    /// <summary>
    /// Provides PlaceholderFontSize Property.
    /// </summary>
    public static readonly StyledProperty<double> PlaceholderFontSizeProperty = AvaloniaProperty.Register<InputChrome, double>(nameof(PlaceholderFontSize));

    /// <summary>
    /// Gets or sets the PlaceholderFontSize property.
    /// </summary>
    public double PlaceholderFontSize
    {
        get => GetValue(PlaceholderFontSizeProperty);
        set => SetValue(PlaceholderFontSizeProperty, value);
    }

    #endregion

    #region InnerLeftContent

    /// <summary>
    /// Defines the <see cref="InnerLeftContent"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<InputChrome, object?>(nameof(InnerLeftContent));

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
        AvaloniaProperty.Register<InputChrome, object?>(nameof(InnerRightContent));

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
    public static readonly StyledProperty<IBrush> InnerForegroundProperty = AvaloniaProperty.Register<InputChrome, IBrush>(nameof(InnerForeground));

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
    public static readonly StyledProperty<double> InnerFontSizeProperty = AvaloniaProperty.Register<InputChrome, double>(nameof(InnerFontSize));

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
    public static readonly StyledProperty<IControlProxy> ProxyProperty = AvaloniaProperty.Register<InputChrome, IControlProxy>(nameof(Proxy));

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
        if (args.Sender is not InputChrome inputChrome) return;

        if (args.OldValue.Value is { } oldHintProxy)
        {
            oldHintProxy.IsEmptyChanged -= inputChrome.IsEmptyChangedCallback;
            oldHintProxy.IsFocusedChanged -= inputChrome.IsFocusedChangedCallback;
            oldHintProxy.IsActiveChanged -= inputChrome.IsActiveChangedCallback;
        }

        if (args.NewValue.Value is not { } newHintProxy)
            return;

        newHintProxy.IsEmptyChanged -= inputChrome.IsEmptyChangedCallback;
        newHintProxy.IsFocusedChanged -= inputChrome.IsFocusedChangedCallback;
        newHintProxy.IsActiveChanged -= inputChrome.IsActiveChangedCallback;

        newHintProxy.IsEmptyChanged += inputChrome.IsEmptyChangedCallback;
        newHintProxy.IsFocusedChanged += inputChrome.IsFocusedChangedCallback;
        newHintProxy.IsActiveChanged += inputChrome.IsActiveChangedCallback;

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
        PseudoClasses.Set(PseudoClassName.Active, isFocused && ((UseFloatingPlaceholder && isFloating) || !UseFloatingPlaceholder));
    }

    private void RefreshIsFloating()
    {
        var isFloating = Proxy.IsActive();
        PseudoClasses.Set(PseudoClassName.Floating, UseFloatingPlaceholder && isFloating);
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
