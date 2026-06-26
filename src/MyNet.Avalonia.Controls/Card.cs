// -----------------------------------------------------------------------
// <copyright file="Card.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// A flexible surface control that displays related information across multiple configurable regions.
/// Supports multiple body layouts, leading presentations, and full MyNet theming.
/// Use the interactive control theme for command-driven cards.
/// </summary>
/// <remarks>
/// <para><see cref="RegionControl.Header"/> on <see cref="Card"/> is an optional <strong>top chrome band</strong>,
/// not the tile title. Use <see cref="Title"/> and <see cref="Subtitle"/> for the primary heading in the body grid
/// (rendered via <see cref="TitleBlock"/> in the default theme).</para>
/// <para>When only <see cref="ContentControl.Content"/> is set, the card automatically collapses to a content-only body.</para>
/// </remarks>
[PseudoClasses(
    PseudoClassName.Horizontal,
    PseudoClassName.Vertical,
    PseudoClassName.LayoutStat,
    PseudoClassName.LayoutMediaTop,
    PseudoClassName.LayoutMediaLeft,
    PseudoClassName.ContentOnly,
    PseudoClassName.LeadingBadge,
    PseudoClassName.LeadingPlain,
    PseudoClassName.LeadingHero,
    PseudoClassName.LeadingNone,
    ":title-empty",
    ":subtitle-empty")]
[TemplatePart(PartRoot, typeof(Border))]
[TemplatePart(PartActionButton, typeof(Button))]
public class Card : RegionControl
{
    public const string PartRoot = "PART_Root";
    public const string PartActionButton = "PART_ActionButton";

    static Card()
    {
        CardLayoutProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateLayoutState());
        LeadingPresentationProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateLeadingPresentationState());
        TitleProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateTitleState());
        SubtitleProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateSubtitleState());
        ContentProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateContentOnlyState());
        LeadingProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateContentOnlyState());
    }

    public Card()
    {
        UpdateLayoutState();
        UpdateLeadingPresentationState();
        UpdateTitleState();
        UpdateSubtitleState();
        UpdateContentOnlyState();
    }

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

    public static readonly StyledProperty<LeadingPresentation> LeadingPresentationProperty =
        AvaloniaProperty.Register<Card, LeadingPresentation>(nameof(LeadingPresentation));

    public LeadingPresentation LeadingPresentation
    {
        get => GetValue(LeadingPresentationProperty);
        set => SetValue(LeadingPresentationProperty, value);
    }

    #endregion

    private void UpdateLayoutState()
    {
        var layout = Layout;
        PseudoClasses.Set(PseudoClassName.Horizontal, layout == CardLayout.Horizontal);
        PseudoClasses.Set(PseudoClassName.Vertical, layout == CardLayout.Vertical);
        PseudoClasses.Set(PseudoClassName.LayoutStat, layout == CardLayout.Stat);
        PseudoClasses.Set(PseudoClassName.LayoutMediaTop, layout == CardLayout.MediaTop);
        PseudoClasses.Set(PseudoClassName.LayoutMediaLeft, layout == CardLayout.MediaLeft);
    }

    private void UpdateLeadingPresentationState()
    {
        var presentation = LeadingPresentation;
        PseudoClasses.Set(PseudoClassName.LeadingBadge, presentation == LeadingPresentation.Badge);
        PseudoClasses.Set(PseudoClassName.LeadingPlain, presentation == LeadingPresentation.Plain);
        PseudoClasses.Set(PseudoClassName.LeadingHero, presentation == LeadingPresentation.Hero);
        PseudoClasses.Set(PseudoClassName.LeadingNone, presentation == LeadingPresentation.None);
        IsLeadingSlotVisible = presentation != LeadingPresentation.None;
    }

    private void UpdateContentOnlyState()
    {
        var contentOnly = !IsEmptyLike(Content)
                          && IsEmptyLike(Title)
                          && IsEmptyLike(Subtitle)
                          && IsEmptyLike(Leading);
        PseudoClasses.Set(PseudoClassName.ContentOnly, contentOnly);
    }

    private void UpdateTitleState()
    {
        PseudoClasses.Set(":title-empty", IsEmptyLike(Title));
        UpdateContentOnlyState();
    }

    private void UpdateSubtitleState()
    {
        PseudoClasses.Set(":subtitle-empty", IsEmptyLike(Subtitle));
        UpdateContentOnlyState();
    }
}
