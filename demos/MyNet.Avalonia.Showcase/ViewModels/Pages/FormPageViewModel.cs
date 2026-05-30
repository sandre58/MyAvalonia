// -----------------------------------------------------------------------
// <copyright file="FormPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Samples;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class FormPageViewModel() : ShowcaseViewModel(nameof(Form),
[
    new()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FormatLineStyle;

    public FormViewModel Form { get; } = new();
}
