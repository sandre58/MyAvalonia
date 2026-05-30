// -----------------------------------------------------------------------
// <copyright file="PlaygroundView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PropertyChanged;

namespace MyNet.Avalonia.Showcase.Views.Playground;

[DoNotNotify]
internal sealed partial class PlaygroundView : UserControl
{
    public PlaygroundView() => InitializeComponent();

    public static readonly StyledProperty<IDataTemplate?> ControlTemplateProperty = AvaloniaProperty.Register<PlaygroundView, IDataTemplate?>(nameof(ControlTemplate));

    public IDataTemplate? ControlTemplate
    {
        get => GetValue(ControlTemplateProperty);
        set => SetValue(ControlTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> ShowPreviewCodeProperty = AvaloniaProperty.Register<PlaygroundView, bool>(nameof(ShowPreviewCode), true);

    public bool ShowPreviewCode
    {
        get => GetValue(ShowPreviewCodeProperty);
        set => SetValue(ShowPreviewCodeProperty, value);
    }

    public static readonly StyledProperty<bool> ShowAppearanceProperty = AvaloniaProperty.Register<PlaygroundView, bool>(nameof(ShowAppearance), true);

    public bool ShowAppearance
    {
        get => GetValue(ShowAppearanceProperty);
        set => SetValue(ShowAppearanceProperty, value);
    }

    public static readonly StyledProperty<bool> ShowIconProperty = AvaloniaProperty.Register<PlaygroundView, bool>(nameof(ShowIcon), true);

    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    public static readonly StyledProperty<bool> ShowContentProperty = AvaloniaProperty.Register<PlaygroundView, bool>(nameof(ShowContent), true);

    public bool ShowContent
    {
        get => GetValue(ShowContentProperty);
        set => SetValue(ShowContentProperty, value);
    }

    public static readonly StyledProperty<bool> ShowBackgroundsSelectionProperty = AvaloniaProperty.Register<PlaygroundView, bool>(nameof(ShowBackgroundsSelection), true);

    public bool ShowBackgroundsSelection
    {
        get => GetValue(ShowBackgroundsSelectionProperty);
        set => SetValue(ShowBackgroundsSelectionProperty, value);
    }
}
