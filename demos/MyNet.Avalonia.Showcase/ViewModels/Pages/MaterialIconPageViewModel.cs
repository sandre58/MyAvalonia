// -----------------------------------------------------------------------
// <copyright file="MaterialIconPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class MaterialIconPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(MaterialIcon), commands, [
    new ControlThemeBuilder()
        .AddStandardSizes()
        .AddVariants("kind-glyph")
        .AddEnumProperty<MaterialIconAnimation, ComboBoxEditor>(
            MaterialIcon.AnimationProperty,
            MaterialIconAnimation.None,
            x => x.DisplayName(SettingsResources.Animation))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.AnimationOutline;
}
