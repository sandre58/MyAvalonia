// -----------------------------------------------------------------------
// <copyright file="ThemesCatalogView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Showcase.ViewModels.Playground;

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

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is ThemesCatalogViewModel catalog && IsVisible)
            catalog.EnsureLoaded();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsVisibleProperty
            && change.GetNewValue<bool>()
            && DataContext is ThemesCatalogViewModel catalog)
        {
            catalog.EnsureLoaded();
        }
    }
}
