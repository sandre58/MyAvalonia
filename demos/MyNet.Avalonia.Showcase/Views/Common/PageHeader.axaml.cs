// -----------------------------------------------------------------------
// <copyright file="PageHeader.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Primitives;
using Material.Icons;

namespace MyNet.Avalonia.Showcase.Views.Common;

public class PageHeader : HeaderedContentControl
{
    public static readonly StyledProperty<MaterialIconKind?> IconProperty = AvaloniaProperty.Register<PageHeader, MaterialIconKind?>(nameof(Icon));

    public MaterialIconKind? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
