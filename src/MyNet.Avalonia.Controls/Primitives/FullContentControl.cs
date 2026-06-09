// -----------------------------------------------------------------------
// <copyright file="FullContentControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls.Primitives;

public class FullContentControl : HeaderedContentControl
{
    #region Footer

    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<Card, object?>(nameof(Footer));

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> FooterTemplateProperty = AvaloniaProperty.Register<Card, IDataTemplate?>(nameof(FooterTemplate));

    public IDataTemplate? FooterTemplate
    {
        get => GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    #endregion

    #region Header band

    public static readonly StyledProperty<Thickness> HeaderPaddingProperty = AvaloniaProperty.Register<Card, Thickness>(nameof(HeaderPadding));

    public Thickness HeaderPadding
    {
        get => GetValue(HeaderPaddingProperty);
        set => SetValue(HeaderPaddingProperty, value);
    }

    public static readonly StyledProperty<IBrush?> HeaderBackgroundProperty = AvaloniaProperty.Register<Card, IBrush?>(nameof(HeaderBackground));

    public IBrush? HeaderBackground
    {
        get => GetValue(HeaderBackgroundProperty);
        set => SetValue(HeaderBackgroundProperty, value);
    }

    public static readonly StyledProperty<Thickness> HeaderMarginProperty = AvaloniaProperty.Register<Card, Thickness>(nameof(HeaderMargin));

    public Thickness HeaderMargin
    {
        get => GetValue(HeaderMarginProperty);
        set => SetValue(HeaderMarginProperty, value);
    }

    public static readonly StyledProperty<double> HeaderFontSizeProperty = AvaloniaProperty.Register<Card, double>(nameof(HeaderFontSize), 16);

    public double HeaderFontSize
    {
        get => GetValue(HeaderFontSizeProperty);
        set => SetValue(HeaderFontSizeProperty, value);
    }

    public static readonly StyledProperty<FontWeight> HeaderFontWeightProperty = AvaloniaProperty.Register<Card, FontWeight>(nameof(HeaderFontWeight), FontWeight.SemiBold);

    public FontWeight HeaderFontWeight
    {
        get => GetValue(HeaderFontWeightProperty);
        set => SetValue(HeaderFontWeightProperty, value);
    }

    public static readonly StyledProperty<IBrush?> HeaderForegroundProperty = AvaloniaProperty.Register<Card, IBrush?>(nameof(HeaderForeground));

    public IBrush? HeaderForeground
    {
        get => GetValue(HeaderForegroundProperty);
        set => SetValue(HeaderForegroundProperty, value);
    }

    #endregion

    #region Footer band

    public static readonly StyledProperty<Thickness> FooterPaddingProperty = AvaloniaProperty.Register<Card, Thickness>(nameof(FooterPadding));

    public Thickness FooterPadding
    {
        get => GetValue(FooterPaddingProperty);
        set => SetValue(FooterPaddingProperty, value);
    }

    public static readonly StyledProperty<IBrush?> FooterBackgroundProperty = AvaloniaProperty.Register<Card, IBrush?>(nameof(FooterBackground));

    public IBrush? FooterBackground
    {
        get => GetValue(FooterBackgroundProperty);
        set => SetValue(FooterBackgroundProperty, value);
    }

    public static readonly StyledProperty<Thickness> FooterMarginProperty = AvaloniaProperty.Register<Card, Thickness>(nameof(FooterMargin));

    public Thickness FooterMargin
    {
        get => GetValue(FooterMarginProperty);
        set => SetValue(FooterMarginProperty, value);
    }

    public static readonly StyledProperty<double> FooterFontSizeProperty = AvaloniaProperty.Register<Card, double>(nameof(FooterFontSize), 16);

    public double FooterFontSize
    {
        get => GetValue(FooterFontSizeProperty);
        set => SetValue(FooterFontSizeProperty, value);
    }

    public static readonly StyledProperty<FontWeight> FooterFontWeightProperty = AvaloniaProperty.Register<Card, FontWeight>(nameof(FooterFontWeight), FontWeight.SemiBold);

    public FontWeight FooterFontWeight
    {
        get => GetValue(FooterFontWeightProperty);
        set => SetValue(FooterFontWeightProperty, value);
    }

    public static readonly StyledProperty<IBrush?> FooterForegroundProperty = AvaloniaProperty.Register<Card, IBrush?>(nameof(FooterForeground));

    public IBrush? FooterForeground
    {
        get => GetValue(FooterForegroundProperty);
        set => SetValue(FooterForegroundProperty, value);
    }

    #endregion
}
