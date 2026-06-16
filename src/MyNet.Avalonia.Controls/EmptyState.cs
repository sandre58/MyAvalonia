// -----------------------------------------------------------------------
// <copyright file="EmptyState.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Centered placeholder for lists, panels, or pages with no data: illustration, title, description, and optional actions.
/// </summary>
/// <remarks>
/// <para>Use <see cref="Leading"/> for an illustration or icon, <see cref="Title"/> / <see cref="Subtitle"/> for the
/// heading (rendered via <see cref="TitleBlock"/>), <see cref="ContentControl.Content"/> for supplemental body content,
/// and <see cref="RegionControl.Actions"/> for primary/secondary buttons.</para>
/// </remarks>
[PseudoClasses(":title-empty", ":subtitle-empty")]
public class EmptyState : RegionControl
{
    static EmptyState()
    {
        TitleProperty.Changed.AddClassHandler<EmptyState>((c, _) => c.UpdateTitleState());
        SubtitleProperty.Changed.AddClassHandler<EmptyState>((c, _) => c.UpdateSubtitleState());
    }

    public EmptyState()
    {
        UpdateTitleState();
        UpdateSubtitleState();
    }

    #region Title / Subtitle

    public static readonly StyledProperty<object?> TitleProperty =
        AvaloniaProperty.Register<EmptyState, object?>(nameof(Title));

    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> TitleTemplateProperty =
        AvaloniaProperty.Register<EmptyState, IDataTemplate?>(nameof(TitleTemplate));

    public IDataTemplate? TitleTemplate
    {
        get => GetValue(TitleTemplateProperty);
        set => SetValue(TitleTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> SubtitleProperty =
        AvaloniaProperty.Register<EmptyState, object?>(nameof(Subtitle));

    public object? Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> SubtitleTemplateProperty =
        AvaloniaProperty.Register<EmptyState, IDataTemplate?>(nameof(SubtitleTemplate));

    public IDataTemplate? SubtitleTemplate
    {
        get => GetValue(SubtitleTemplateProperty);
        set => SetValue(SubtitleTemplateProperty, value);
    }

    #endregion

    private void UpdateTitleState() => PseudoClasses.Set(":title-empty", IsEmptyLike(Title));

    private void UpdateSubtitleState() => PseudoClasses.Set(":subtitle-empty", IsEmptyLike(Subtitle));
}
