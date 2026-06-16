// -----------------------------------------------------------------------
// <copyright file="Card.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
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
/// Supports compact and tile layouts and full MyNet theming.
/// Use the interactive control theme for command-driven cards.
/// </summary>
/// <remarks>
/// <para><see cref="RegionControl.Header"/> on <see cref="Card"/> is an optional <strong>top chrome band</strong>,
/// not the tile title. Use <see cref="Title"/> and <see cref="Subtitle"/> for the primary heading in the body grid
/// (rendered via <see cref="TitleBlock"/> in the default theme).</para>
/// </remarks>
[PseudoClasses(PseudoClassName.Compact, PseudoClassName.Tile, ":title-empty", ":subtitle-empty")]
[TemplatePart(PartRoot, typeof(Border))]
[TemplatePart(PartActionButton, typeof(Button))]
public class Card : RegionControl
{
    public const string PartRoot = "PART_Root";
    public const string PartActionButton = "PART_ActionButton";

    static Card()
    {
        CardLayoutProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateLayoutState());
        TitleProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateTitleState());
        SubtitleProperty.Changed.AddClassHandler<Card>((c, _) => c.UpdateSubtitleState());
    }

    public Card()
    {
        UpdateLayoutState();
        UpdateTitleState();
        UpdateSubtitleState();
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

    #endregion

    private void UpdateLayoutState()
    {
        var layout = Layout;
        PseudoClasses.Set(PseudoClassName.Compact, layout == CardLayout.Compact);
        PseudoClasses.Set(PseudoClassName.Tile, layout == CardLayout.Tile);
    }

    private void UpdateTitleState() => PseudoClasses.Set(":title-empty", IsEmptyLike(Title));

    private void UpdateSubtitleState() => PseudoClasses.Set(":subtitle-empty", IsEmptyLike(Subtitle));

    private static bool IsEmptyLike(object? value) => value is null || value switch
    {
        string str => string.IsNullOrEmpty(str),
        double dbl => double.IsNaN(dbl),
        Array arr => arr.Length == 0,
        DateTime date => date == DateTime.MinValue,
        _ => false,
    };
}
