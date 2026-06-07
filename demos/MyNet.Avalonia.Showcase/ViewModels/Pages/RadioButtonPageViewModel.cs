// -----------------------------------------------------------------------
// <copyright file="RadioButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class RadioButtonPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(RadioButton), commands, [
    new ControlThemeBuilder()
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
        .AddShapes(CssClass.ShapeCircle, CssClass.ShapeAlternate)
        .AddDefaultSizes()
        .AddDefaultRoles()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.RadioboxMarked;
}
