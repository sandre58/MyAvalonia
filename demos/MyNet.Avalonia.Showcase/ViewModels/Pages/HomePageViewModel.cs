// -----------------------------------------------------------------------
// <copyright file="HomePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Showcase.ViewModels.Home;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class HomePageViewModel : PageViewModel
{
    private readonly INavigationClient _navigationClient;

    public HomePageViewModel(INavigationClient navigationClient, ICommandFactory commandFactory)
    {
        _navigationClient = navigationClient;
        OpenLinkCommand = commandFactory.Create<HomeLinkViewModel>(link => NavigateAsync(link.NavigationKey));
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Home;

    public IReadOnlyList<HomeLinkViewModel> FeaturedLinks { get; } =
    [
        new(MaterialIconKind.Palette, nameof(MenuResources.Theme), "FeaturedThemeDescription", "Theme"),
        new(MaterialIconKind.TagFaces, nameof(MenuResources.Icons), "FeaturedIconsDescription", "Icons"),
        new(MaterialIconKind.FormatLineStyle, "FeaturedFormTitle", "FeaturedFormDescription", "Form"),
        new(MaterialIconKind.MessageAlertOutline, nameof(MenuResources.DialogsAndFeeback), "FeaturedDialogsDescription", "Dialogs")
    ];

    public IReadOnlyList<HomeLinkViewModel> CategoryLinks { get; } =
    [
        new(MaterialIconKind.FormatText, nameof(MenuResources.Texts), "CategoryTextsDescription", "Texts"),
        new(MaterialIconKind.GestureTapButton, nameof(MenuResources.Buttons), "CategoryButtonsDescription", "Buttons"),
        new(MaterialIconKind.FormTextbox, nameof(MenuResources.Inputs), "CategoryInputsDescription", "Inputs"),
        new(MaterialIconKind.ViewCarousel, nameof(MenuResources.Containers), "CategoryContainersDescription", "Containers"),
        new(MaterialIconKind.Table, nameof(MenuResources.DataAndLists), "CategoryDataDescription", "DataAndLists"),
        new(MaterialIconKind.BookOpenPageVariantOutline, nameof(MenuResources.Navigation), "CategoryNavigationDescription", "Navigation"),
        new(MaterialIconKind.MessageAlertOutline, nameof(MenuResources.DialogsAndFeeback), "CategoryDialogsDescription", "DialogsAndFeedback"),
        new(MaterialIconKind.Shape, nameof(MenuResources.ShapesAndVisuals), "CategoryShapesDescription", "ShapesAndVisuals")
    ];

    public ICommand OpenLinkCommand { get; }

    private Task NavigateAsync(string key) => key switch
    {
        "Theme" => _navigationClient.NavigateToAsync<ThemePageViewModel>(),
        "Icons" => _navigationClient.NavigateToAsync<IconsPageViewModel>(),
        "Form" => _navigationClient.NavigateToAsync<FormPageViewModel>(),
        "Dialogs" => _navigationClient.NavigateToAsync<DialogPageViewModel>(),
        "Texts" => _navigationClient.NavigateToAsync<LabelPageViewModel>(),
        "Buttons" => _navigationClient.NavigateToAsync<ButtonPageViewModel>(),
        "Inputs" => _navigationClient.NavigateToAsync<FieldsPageViewModel>(),
        "Containers" => _navigationClient.NavigateToAsync<FormPageViewModel>(),
        "DataAndLists" => _navigationClient.NavigateToAsync<DataGridPageViewModel>(),
        "Navigation" => _navigationClient.NavigateToAsync<NavigationMenuPageViewModel>(),
        "DialogsAndFeedback" => _navigationClient.NavigateToAsync<NotificationPageViewModel>(),
        "ShapesAndVisuals" => _navigationClient.NavigateToAsync<BorderPageViewModel>(),
        _ => Task.CompletedTask
    };
}
