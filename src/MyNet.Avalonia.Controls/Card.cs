// -----------------------------------------------------------------------
// <copyright file="Card.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// A flexible surface control that displays related information across multiple configurable regions.
/// Supports horizontal and vertical layouts and full MyNet theming.
/// Use the interactive control theme for command-driven cards.
/// </summary>
[PseudoClasses(PseudoClassName.Horizontal, PseudoClassName.Vertical)]
[TemplatePart(PartRoot, typeof(Border))]
[TemplatePart(PartActionButton, typeof(Button))]
public class Card : ContentControl
{
    public const string PartRoot = "PART_Root";
    public const string PartActionButton = "PART_ActionButton";

    static Card() => CardLayoutProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateLayoutState());

    public Card() => UpdateLayoutState();

    #region Header / Footer

    public static readonly StyledProperty<object?> HeaderProperty =
        HeaderedContentControl.HeaderProperty.AddOwner<Card>();

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
        HeaderedContentControl.HeaderTemplateProperty.AddOwner<Card>();

    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> FooterProperty =
        AvaloniaProperty.Register<Card, object?>(nameof(Footer));

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> FooterTemplateProperty =
        AvaloniaProperty.Register<Card, IDataTemplate?>(nameof(FooterTemplate));

    public IDataTemplate? FooterTemplate
    {
        get => GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    #endregion

    #region Header band

    public static readonly StyledProperty<Thickness> HeaderPaddingProperty =
        AvaloniaProperty.Register<Card, Thickness>(nameof(HeaderPadding));

    public Thickness HeaderPadding
    {
        get => GetValue(HeaderPaddingProperty);
        set => SetValue(HeaderPaddingProperty, value);
    }

    public static readonly StyledProperty<IBrush?> HeaderBackgroundProperty =
        AvaloniaProperty.Register<Card, IBrush?>(nameof(HeaderBackground));

    public IBrush? HeaderBackground
    {
        get => GetValue(HeaderBackgroundProperty);
        set => SetValue(HeaderBackgroundProperty, value);
    }

    public static readonly StyledProperty<Thickness> HeaderMarginProperty =
        AvaloniaProperty.Register<Card, Thickness>(nameof(HeaderMargin));

    public Thickness HeaderMargin
    {
        get => GetValue(HeaderMarginProperty);
        set => SetValue(HeaderMarginProperty, value);
    }

    public static readonly StyledProperty<double> HeaderFontSizeProperty =
        AvaloniaProperty.Register<Card, double>(nameof(HeaderFontSize), 16);

    public double HeaderFontSize
    {
        get => GetValue(HeaderFontSizeProperty);
        set => SetValue(HeaderFontSizeProperty, value);
    }

    public static readonly StyledProperty<FontWeight> HeaderFontWeightProperty =
        AvaloniaProperty.Register<Card, FontWeight>(nameof(HeaderFontWeight), FontWeight.SemiBold);

    public FontWeight HeaderFontWeight
    {
        get => GetValue(HeaderFontWeightProperty);
        set => SetValue(HeaderFontWeightProperty, value);
    }

    public static readonly StyledProperty<IBrush?> HeaderForegroundProperty =
        AvaloniaProperty.Register<Card, IBrush?>(nameof(HeaderForeground));

    public IBrush? HeaderForeground
    {
        get => GetValue(HeaderForegroundProperty);
        set => SetValue(HeaderForegroundProperty, value);
    }

    #endregion

    #region Footer band

    public static readonly StyledProperty<Thickness> FooterPaddingProperty =
        AvaloniaProperty.Register<Card, Thickness>(nameof(FooterPadding));

    public Thickness FooterPadding
    {
        get => GetValue(FooterPaddingProperty);
        set => SetValue(FooterPaddingProperty, value);
    }

    public static readonly StyledProperty<IBrush?> FooterBackgroundProperty =
        AvaloniaProperty.Register<Card, IBrush?>(nameof(FooterBackground));

    public IBrush? FooterBackground
    {
        get => GetValue(FooterBackgroundProperty);
        set => SetValue(FooterBackgroundProperty, value);
    }

    public static readonly StyledProperty<Thickness> FooterMarginProperty =
        AvaloniaProperty.Register<Card, Thickness>(nameof(FooterMargin));

    public Thickness FooterMargin
    {
        get => GetValue(FooterMarginProperty);
        set => SetValue(FooterMarginProperty, value);
    }

    public static readonly StyledProperty<double> FooterFontSizeProperty =
        AvaloniaProperty.Register<Card, double>(nameof(FooterFontSize), 16);

    public double FooterFontSize
    {
        get => GetValue(FooterFontSizeProperty);
        set => SetValue(FooterFontSizeProperty, value);
    }

    public static readonly StyledProperty<FontWeight> FooterFontWeightProperty =
        AvaloniaProperty.Register<Card, FontWeight>(nameof(FooterFontWeight), FontWeight.SemiBold);

    public FontWeight FooterFontWeight
    {
        get => GetValue(FooterFontWeightProperty);
        set => SetValue(FooterFontWeightProperty, value);
    }

    public static readonly StyledProperty<IBrush?> FooterForegroundProperty =
        AvaloniaProperty.Register<Card, IBrush?>(nameof(FooterForeground));

    public IBrush? FooterForeground
    {
        get => GetValue(FooterForegroundProperty);
        set => SetValue(FooterForegroundProperty, value);
    }

    #endregion

    #region Leading

    public static readonly StyledProperty<object?> LeadingProperty =
        AvaloniaProperty.Register<Card, object?>(nameof(Leading));

    public object? Leading
    {
        get => GetValue(LeadingProperty);
        set => SetValue(LeadingProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> LeadingTemplateProperty =
        AvaloniaProperty.Register<Card, IDataTemplate?>(nameof(LeadingTemplate));

    public IDataTemplate? LeadingTemplate
    {
        get => GetValue(LeadingTemplateProperty);
        set => SetValue(LeadingTemplateProperty, value);
    }

    public static readonly StyledProperty<IBrush?> LeadingBackgroundProperty =
        AvaloniaProperty.Register<Card, IBrush?>(nameof(LeadingBackground));

    public IBrush? LeadingBackground
    {
        get => GetValue(LeadingBackgroundProperty);
        set => SetValue(LeadingBackgroundProperty, value);
    }

    #endregion

    #region Title / Subtitle

    public static readonly StyledProperty<object?> TitleProperty =
        AvaloniaProperty.Register<Card, object?>(nameof(Title));

    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> TitleTemplateProperty =
        AvaloniaProperty.Register<Card, IDataTemplate?>(nameof(TitleTemplate));

    public IDataTemplate? TitleTemplate
    {
        get => GetValue(TitleTemplateProperty);
        set => SetValue(TitleTemplateProperty, value);
    }

    public static readonly StyledProperty<double> TitleFontSizeProperty =
        AvaloniaProperty.Register<Card, double>(nameof(TitleFontSize), 16);

    public double TitleFontSize
    {
        get => GetValue(TitleFontSizeProperty);
        set => SetValue(TitleFontSizeProperty, value);
    }

    public static readonly StyledProperty<FontWeight> TitleFontWeightProperty =
        AvaloniaProperty.Register<Card, FontWeight>(nameof(TitleFontWeight), FontWeight.SemiBold);

    public FontWeight TitleFontWeight
    {
        get => GetValue(TitleFontWeightProperty);
        set => SetValue(TitleFontWeightProperty, value);
    }

    public static readonly StyledProperty<IBrush?> TitleForegroundProperty =
        AvaloniaProperty.Register<Card, IBrush?>(nameof(TitleForeground));

    public IBrush? TitleForeground
    {
        get => GetValue(TitleForegroundProperty);
        set => SetValue(TitleForegroundProperty, value);
    }

    public static readonly StyledProperty<object?> SubtitleProperty =
        AvaloniaProperty.Register<Card, object?>(nameof(Subtitle));

    public object? Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> SubtitleTemplateProperty =
        AvaloniaProperty.Register<Card, IDataTemplate?>(nameof(SubtitleTemplate));

    public IDataTemplate? SubtitleTemplate
    {
        get => GetValue(SubtitleTemplateProperty);
        set => SetValue(SubtitleTemplateProperty, value);
    }

    public static readonly StyledProperty<double> SubtitleFontSizeProperty =
        AvaloniaProperty.Register<Card, double>(nameof(SubtitleFontSize), 12);

    public double SubtitleFontSize
    {
        get => GetValue(SubtitleFontSizeProperty);
        set => SetValue(SubtitleFontSizeProperty, value);
    }

    public static readonly StyledProperty<IBrush?> SubtitleForegroundProperty =
        AvaloniaProperty.Register<Card, IBrush?>(nameof(SubtitleForeground));

    public IBrush? SubtitleForeground
    {
        get => GetValue(SubtitleForegroundProperty);
        set => SetValue(SubtitleForegroundProperty, value);
    }

    public static readonly StyledProperty<double> SubtitleOpacityProperty =
        AvaloniaProperty.Register<Card, double>(nameof(SubtitleOpacity), 1);

    public double SubtitleOpacity
    {
        get => GetValue(SubtitleOpacityProperty);
        set => SetValue(SubtitleOpacityProperty, value);
    }

    #endregion

    #region Trailing

    public static readonly StyledProperty<object?> TrailingProperty =
        AvaloniaProperty.Register<Card, object?>(nameof(Trailing));

    public object? Trailing
    {
        get => GetValue(TrailingProperty);
        set => SetValue(TrailingProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> TrailingTemplateProperty =
        AvaloniaProperty.Register<Card, IDataTemplate?>(nameof(TrailingTemplate));

    public IDataTemplate? TrailingTemplate
    {
        get => GetValue(TrailingTemplateProperty);
        set => SetValue(TrailingTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> ShowTrailingProperty =
        AvaloniaProperty.Register<Card, bool>(nameof(ShowTrailing));

    public bool ShowTrailing
    {
        get => GetValue(ShowTrailingProperty);
        set => SetValue(ShowTrailingProperty, value);
    }

    #endregion

    #region Command

    public static readonly StyledProperty<ICommand?> CommandProperty =
        Button.CommandProperty.AddOwner<Card>();

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object?> CommandParameterProperty =
        Button.CommandParameterProperty.AddOwner<Card>();

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    #endregion

    #region Layout

    public static readonly StyledProperty<CardLayout> CardLayoutProperty =
        AvaloniaProperty.Register<Card, CardLayout>(nameof(Layout));

    public CardLayout Layout
    {
        get => GetValue(CardLayoutProperty);
        set => SetValue(CardLayoutProperty, value);
    }

    #endregion

    private void UpdateLayoutState()
    {
        var layout = Layout;
        PseudoClasses.Set(PseudoClassName.Horizontal, layout == CardLayout.Horizontal);
        PseudoClasses.Set(PseudoClassName.Vertical, layout == CardLayout.Vertical);
    }
}
