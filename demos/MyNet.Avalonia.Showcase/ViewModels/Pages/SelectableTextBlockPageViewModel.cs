// -----------------------------------------------------------------------
// <copyright file="SelectableTextBlockPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class SelectableTextBlockPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(SelectableTextBlock), commands, [
    new ControlThemeBuilder()
        .AddVariants("opacity-high", "opacity-medium", "opacity-low", "text-helper", "text-watermark", "variant-underline", "variant-strikethrough", "is-disablable")
        .AddDefaultRoles()
        .AddSizes("font-xs", "font-sm", "font-md", "font-lg", "font-xl", "font-h6", "font-h5", "font-h4", "font-h3", "font-h2", "font-h1")
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.CursorText;
}
