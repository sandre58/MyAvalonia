// -----------------------------------------------------------------------
// <copyright file="ThemesCatalogView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace MyNet.Avalonia.Showcase.Views.Playground;

internal sealed partial class ThemesCatalogView : UserControl
{
    public ThemesCatalogView() => InitializeComponent();

    public static readonly StyledProperty<IDataTemplate?> ControlTemplateProperty = AvaloniaProperty.Register<ThemesCatalogView, IDataTemplate?>(nameof(ControlTemplate));

    public IDataTemplate? ControlTemplate
    {
        get => GetValue(ControlTemplateProperty);
        set => SetValue(ControlTemplateProperty, value);
    }
}
