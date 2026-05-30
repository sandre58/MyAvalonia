// -----------------------------------------------------------------------
// <copyright file="CarouselPagePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Linq;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Helpers;
using MyNet.UI.Resources;
using MyNet.Utilities;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class CarouselPagePageViewModel() : ShowcaseViewModel(nameof(CarouselPage),
[
    new ControlThemeBuilder()
        .AddAction<CarouselPage>(x => (x.SelectedIndex < x.Pages.Count()).IfTrue(() => x.SelectedIndex++),
            x => x.DisplayName(UiResources.NextPage).WithIcon(MaterialIconKind.PageNext))
        .AddAction<CarouselPage>(x => (x.SelectedIndex > 0).IfTrue(() => x.SelectedIndex--),
            x => x.DisplayName(UiResources.PreviousPage).WithIcon(MaterialIconKind.PagePrevious))
        .AddEnumValue<PageTransitionType, ComboBoxEditor>((x, y) => (x as CarouselPage)?.SetValue(CarouselPage.PageTransitionProperty, TransitionsHelper.CreatePageTransition(y.GetValueOrDefault())),
            PageTransitionType.None,
            x => x.DisplayName(SettingsResources.Transition))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.BookOpenPageVariantOutline;
}
