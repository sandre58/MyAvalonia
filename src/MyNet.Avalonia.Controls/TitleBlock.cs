// -----------------------------------------------------------------------
// <copyright file="TitleBlock.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Stacks a title and optional subtitle with consistent typography.
/// </summary>
[PseudoClasses(":title-empty", ":subtitle-empty")]
public class TitleBlock : TemplatedControl
{
    static TitleBlock()
    {
        TitleProperty.Changed.AddClassHandler<TitleBlock>((c, _) =>
        {
            c.UpdateTitleState();
            c.ApplyTextAlignmentToPresenters();
        });
        SubtitleProperty.Changed.AddClassHandler<TitleBlock>((c, _) =>
        {
            c.UpdateSubtitleState();
            c.ApplyTextAlignmentToPresenters();
        });
        TextAlignmentProperty.Changed.AddClassHandler<TitleBlock>((c, _) => c.ApplyTextAlignmentToPresenters());
    }

    public TitleBlock()
    {
        UpdateTitleState();
        UpdateSubtitleState();
    }

    #region Title

    public static readonly StyledProperty<object?> TitleProperty =
        AvaloniaProperty.Register<TitleBlock, object?>(nameof(Title));

    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> TitleTemplateProperty =
        AvaloniaProperty.Register<TitleBlock, IDataTemplate?>(nameof(TitleTemplate));

    public IDataTemplate? TitleTemplate
    {
        get => GetValue(TitleTemplateProperty);
        set => SetValue(TitleTemplateProperty, value);
    }

    public static readonly StyledProperty<double> TitleFontSizeProperty =
        AvaloniaProperty.Register<TitleBlock, double>(nameof(TitleFontSize), 16);

    public double TitleFontSize
    {
        get => GetValue(TitleFontSizeProperty);
        set => SetValue(TitleFontSizeProperty, value);
    }

    public static readonly StyledProperty<FontWeight> TitleFontWeightProperty =
        AvaloniaProperty.Register<TitleBlock, FontWeight>(nameof(TitleFontWeight), FontWeight.SemiBold);

    public FontWeight TitleFontWeight
    {
        get => GetValue(TitleFontWeightProperty);
        set => SetValue(TitleFontWeightProperty, value);
    }

    public static readonly StyledProperty<IBrush?> TitleForegroundProperty =
        AvaloniaProperty.Register<TitleBlock, IBrush?>(nameof(TitleForeground));

    public IBrush? TitleForeground
    {
        get => GetValue(TitleForegroundProperty);
        set => SetValue(TitleForegroundProperty, value);
    }

    #endregion

    #region Subtitle

    public static readonly StyledProperty<object?> SubtitleProperty =
        AvaloniaProperty.Register<TitleBlock, object?>(nameof(Subtitle));

    public object? Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> SubtitleTemplateProperty =
        AvaloniaProperty.Register<TitleBlock, IDataTemplate?>(nameof(SubtitleTemplate));

    public IDataTemplate? SubtitleTemplate
    {
        get => GetValue(SubtitleTemplateProperty);
        set => SetValue(SubtitleTemplateProperty, value);
    }

    public static readonly StyledProperty<double> SubtitleFontSizeProperty =
        AvaloniaProperty.Register<TitleBlock, double>(nameof(SubtitleFontSize), 12);

    public double SubtitleFontSize
    {
        get => GetValue(SubtitleFontSizeProperty);
        set => SetValue(SubtitleFontSizeProperty, value);
    }

    public static readonly StyledProperty<IBrush?> SubtitleForegroundProperty =
        AvaloniaProperty.Register<TitleBlock, IBrush?>(nameof(SubtitleForeground));

    public IBrush? SubtitleForeground
    {
        get => GetValue(SubtitleForegroundProperty);
        set => SetValue(SubtitleForegroundProperty, value);
    }

    #endregion

    #region Text Alignment

    /// <summary>
    /// Gets or sets the horizontal alignment of title and subtitle text within the block.
    /// </summary>
    public static readonly StyledProperty<TextAlignment> TextAlignmentProperty =
        AvaloniaProperty.Register<TitleBlock, TextAlignment>(nameof(TextAlignment));

    public TextAlignment TextAlignment
    {
        get => GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    #endregion

    #region Horizontal Text Alignment

    /// <summary>
    /// Obsolete layout helper — title presenters always stretch; use <see cref="TextAlignment"/> instead.
    /// </summary>
    public static readonly StyledProperty<HorizontalAlignment> HorizontalTextAlignmentProperty =
        AvaloniaProperty.Register<TitleBlock, HorizontalAlignment>(nameof(HorizontalTextAlignment));

    public HorizontalAlignment HorizontalTextAlignment
    {
        get => GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    #endregion

    #region Layout

    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<TitleBlock, double>(nameof(Spacing), 4);

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    #endregion

    private ContentPresenter? _titlePresenter;
    private ContentPresenter? _subtitlePresenter;

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _titlePresenter = e.NameScope.Find<ContentPresenter>("PART_Title");
        _subtitlePresenter = e.NameScope.Find<ContentPresenter>("PART_Subtitle");
        ApplyTextAlignmentToPresenters();
    }

    private void ApplyTextAlignmentToPresenters()
    {
        ApplyTextAlignment(_titlePresenter);
        ApplyTextAlignment(_subtitlePresenter);
    }

    private void ApplyTextAlignment(ContentPresenter? presenter)
    {
        if (presenter is null)
        {
            return;
        }

        var alignment = TextAlignment;
        presenter.SetValue(TextBlock.TextAlignmentProperty, alignment);

        if (presenter.Child is TextBlock textBlock)
        {
            textBlock.TextAlignment = alignment;
            textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
        }
    }

    private void UpdateTitleState() => PseudoClasses.Set(":title-empty", IsEmptyLike(Title));

    private void UpdateSubtitleState() => PseudoClasses.Set(":subtitle-empty", IsEmptyLike(Subtitle));

    private static bool IsEmptyLike(object? value) => value is null || value switch
    {
        string str => string.IsNullOrEmpty(str),
        double dbl => double.IsNaN(dbl),
        Array arr => arr.Length == 0,
        DateTime date => date == DateTime.MinValue,
        _ => false
    };
}
