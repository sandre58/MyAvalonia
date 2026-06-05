// -----------------------------------------------------------------------
// <copyright file="ShowcaseView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.VisualTree;
using MyNet.Avalonia.Showcase.ViewModels.Playground;

namespace MyNet.Avalonia.Showcase.Views.Playground;

internal sealed partial class ShowcaseView : UserControl
{
    public ShowcaseView()
    {
        InitializeComponent();
        TabbedPages.SelectionChanged += OnTabbedPagesSelectionChanged;
    }

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

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        ActivateSelectedTabContent();
    }

    private void OnTabbedPagesSelectionChanged(object? sender, PageSelectionChangedEventArgs e)
        => ActivateSelectedTabContent();

    private void ActivateSelectedTabContent()
    {
        if (DataContext is not ShowcaseViewModel viewModel)
            return;

        var selectedPage = TabbedPages.SelectedPage;
        if (selectedPage is null)
            return;

        if (selectedPage.GetVisualDescendants().OfType<PlaygroundView>().Any())
            _ = viewModel.Playground;

        if (selectedPage.GetVisualDescendants().OfType<ThemesCatalogView>().Any())
            viewModel.Catalog.EnsureLoaded();
    }
}
