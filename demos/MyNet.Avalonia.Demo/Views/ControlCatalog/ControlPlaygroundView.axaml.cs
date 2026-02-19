// -----------------------------------------------------------------------
// <copyright file="ControlPlaygroundView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Views.ControlCatalog;

[DoNotNotify]
internal sealed partial class ControlPlaygroundView : UserControl
{
    public ControlPlaygroundView() => InitializeComponent();

    public static readonly StyledProperty<IDataTemplate?> ControlTemplateProperty = AvaloniaProperty.Register<ControlPlaygroundView, IDataTemplate?>(nameof(ControlTemplate));

    public IDataTemplate? ControlTemplate
    {
        get => GetValue(ControlTemplateProperty);
        set => SetValue(ControlTemplateProperty, value);
    }

    public static readonly StyledProperty<Form?> SettingsProperty = AvaloniaProperty.Register<ControlPlaygroundView, Form?>(nameof(Settings));

    public Form? Settings
    {
        get => GetValue(SettingsProperty);
        set => SetValue(SettingsProperty, value);
    }

    public static readonly StyledProperty<bool> ShowPreviewCodeProperty = AvaloniaProperty.Register<ControlPlaygroundView, bool>(nameof(ShowPreviewCode), true);

    public bool ShowPreviewCode
    {
        get => GetValue(ShowPreviewCodeProperty);
        set => SetValue(ShowPreviewCodeProperty, value);
    }

    public static readonly StyledProperty<bool> ShowAppearanceProperty = AvaloniaProperty.Register<ControlPlaygroundView, bool>(nameof(ShowAppearance), true);

    public bool ShowAppearance
    {
        get => GetValue(ShowAppearanceProperty);
        set => SetValue(ShowAppearanceProperty, value);
    }

    public static readonly StyledProperty<bool> ShowIconProperty = AvaloniaProperty.Register<ControlPlaygroundView, bool>(nameof(ShowIcon), true);

    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    public static readonly StyledProperty<bool> ShowContentProperty = AvaloniaProperty.Register<ControlPlaygroundView, bool>(nameof(ShowContent), true);

    public bool ShowContent
    {
        get => GetValue(ShowContentProperty);
        set => SetValue(ShowContentProperty, value);
    }

    public static readonly StyledProperty<bool> ShowBackgroundsSelectionProperty = AvaloniaProperty.Register<ControlPlaygroundView, bool>(nameof(ShowBackgroundsSelection), true);

    public bool ShowBackgroundsSelection
    {
        get => GetValue(ShowBackgroundsSelectionProperty);
        set => SetValue(ShowBackgroundsSelectionProperty, value);
    }
}
