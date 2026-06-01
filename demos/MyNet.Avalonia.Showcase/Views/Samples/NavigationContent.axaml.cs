// -----------------------------------------------------------------------
// <copyright file="NavigationContent.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.Views.Samples;

public partial class NavigationContent : UserControl
{
    public NavigationContent() => InitializeComponent();

    #region Header

    public static readonly StyledProperty<string?> HeaderProperty = AvaloniaProperty.Register<NavigationContent, string?>(nameof(Header));

    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    #endregion

    #region Body

    public static readonly StyledProperty<string?> BodyProperty = AvaloniaProperty.Register<NavigationContent, string?>(nameof(Body));

    public string? Body
    {
        get => GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }

    #endregion
}
