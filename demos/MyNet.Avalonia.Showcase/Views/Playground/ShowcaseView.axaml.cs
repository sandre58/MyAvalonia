// -----------------------------------------------------------------------
// <copyright file="ShowcaseView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PropertyChanged;

namespace MyNet.Avalonia.Showcase.Views.Playground;

[DoNotNotify]
internal sealed partial class ShowcaseView : UserControl
{
    public ShowcaseView() => InitializeComponent();

    public static readonly StyledProperty<IDataTemplate?> PlaygroundControlTemplateProperty = AvaloniaProperty.Register<ShowcaseView, IDataTemplate?>(nameof(PlaygroundControlTemplate));

    public IDataTemplate? PlaygroundControlTemplate
    {
        get => GetValue(PlaygroundControlTemplateProperty);
        set => SetValue(PlaygroundControlTemplateProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> ThemeControlTemplateProperty = AvaloniaProperty.Register<ShowcaseView, IDataTemplate?>(nameof(ThemeControlTemplate));

    public IDataTemplate? ThemeControlTemplate
    {
        get => GetValue(ThemeControlTemplateProperty);
        set => SetValue(ThemeControlTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> CustomContentProperty = AvaloniaProperty.Register<ShowcaseView, object?>(nameof(CustomContent));

    public object? CustomContent
    {
        get => GetValue(CustomContentProperty);
        set => SetValue(CustomContentProperty, value);
    }

    public static readonly StyledProperty<bool> ShowPlaygroundProperty = AvaloniaProperty.Register<ShowcaseView, bool>(nameof(ShowPlayground), true);

    public bool ShowPlayground
    {
        get => GetValue(ShowPlaygroundProperty);
        set => SetValue(ShowPlaygroundProperty, value);
    }

    public static readonly StyledProperty<bool> ShowThemesProperty = AvaloniaProperty.Register<ShowcaseView, bool>(nameof(ShowThemes), true);

    public bool ShowThemes
    {
        get => GetValue(ShowThemesProperty);
        set => SetValue(ShowThemesProperty, value);
    }

    public static readonly StyledProperty<bool> ShowPreviewCodeProperty = AvaloniaProperty.Register<ShowcaseView, bool>(nameof(ShowPreviewCode), true);

    public bool ShowPreviewCode
    {
        get => GetValue(ShowPreviewCodeProperty);
        set => SetValue(ShowPreviewCodeProperty, value);
    }

    public static readonly StyledProperty<bool> ShowAppearanceProperty = AvaloniaProperty.Register<ShowcaseView, bool>(nameof(ShowAppearance), true);

    public bool ShowAppearance
    {
        get => GetValue(ShowAppearanceProperty);
        set => SetValue(ShowAppearanceProperty, value);
    }

    public static readonly StyledProperty<bool> ShowIconProperty = AvaloniaProperty.Register<ShowcaseView, bool>(nameof(ShowIcon), true);

    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    public static readonly StyledProperty<bool> ShowContentProperty = AvaloniaProperty.Register<ShowcaseView, bool>(nameof(ShowContent), true);

    public bool ShowContent
    {
        get => GetValue(ShowContentProperty);
        set => SetValue(ShowContentProperty, value);
    }

    public static readonly StyledProperty<bool> ShowBackgroundsSelectionProperty = AvaloniaProperty.Register<ShowcaseView, bool>(nameof(ShowBackgroundsSelection), true);

    public bool ShowBackgroundsSelection
    {
        get => GetValue(ShowBackgroundsSelectionProperty);
        set => SetValue(ShowBackgroundsSelectionProperty, value);
    }
}
