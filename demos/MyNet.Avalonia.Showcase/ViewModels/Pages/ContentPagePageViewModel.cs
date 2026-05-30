// -----------------------------------------------------------------------
// <copyright file="ContentPagePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Helpers;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Helpers;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ContentPagePageViewModel() : ShowcaseViewModel(nameof(ContentPage),
[
    new ControlThemeBuilder()
        .AddAction<NavigationPage>(async x => await x.PushAsync(PageHelper.MakeNavigationPage($"Page {x.Pages?.Count() + 1}", $"ContentPage #{x.Pages?.Count() + 1}.\nNavigate back using the back button.")).ConfigureAwait(false),
            x => x.DisplayName(SettingsResources.PushPage).WithIcon(MaterialIconKind.PageNext))
        .AddAction<NavigationPage>(async x => await x.PopAsync().ConfigureAwait(false),
            x => x.DisplayName(SettingsResources.PopPage).WithIcon(MaterialIconKind.PagePrevious))
        .AddProperty(NavigationPage.IsBackButtonVisibleProperty, true, x => x.DisplayName(SettingsResources.IsBackButtonVisible))
        .AddEnumValue<PageTransitionType, ComboBoxEditor>((x, y) => (x as NavigationPage)?.SetValue(NavigationPage.PageTransitionProperty, TransitionsHelper.CreatePageTransition(y.GetValueOrDefault())),
            PageTransitionType.Slide,
            x => x.DisplayName(SettingsResources.Transition))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FileDocumentOutline;
}
