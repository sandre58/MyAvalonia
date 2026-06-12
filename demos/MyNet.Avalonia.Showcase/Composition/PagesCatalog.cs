// -----------------------------------------------------------------------
// <copyright file="PagesCatalog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Material.Icons;
using MyNet.Avalonia.Showcase.Pages;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ViewModels.Menu;
using MyNet.Avalonia.Showcase.ViewModels.Pages;

namespace MyNet.Avalonia.Showcase.Composition;

/// <summary>
/// Explicit registry of showcase demo pages and menu groups.
/// </summary>
internal static class PagesCatalog
{
    /// <summary>Returns all page providers for the showcase navigation menu.</summary>
    public static IReadOnlyList<IPagesProvider> GetProviders()
        =>
        [
            new PageAssociation(typeof(HomePageViewModel), typeof(HomePage), MaterialIconKind.Home),
            new PageAssociation(typeof(ThemePageViewModel), typeof(ThemePage), MaterialIconKind.PaletteSwatchVariant),
            new PageAssociation(typeof(IconsPageViewModel), typeof(IconsPage), MaterialIconKind.TagFaces),

            new PagesGroup(nameof(MenuResources.Typography), MaterialIconKind.FormatText, [
                new(typeof(LabelPageViewModel), typeof(LabelPage), MaterialIconKind.TagText),
                new(typeof(TextBlockPageViewModel), typeof(TextBlockPage), MaterialIconKind.FormatText),
                new(typeof(SelectableTextBlockPageViewModel), typeof(SelectableTextBlockPage), MaterialIconKind.CursorText)
            ]),

            new PagesGroup(nameof(MenuResources.ButtonsAndActions), MaterialIconKind.GestureTapButton, [
                new(typeof(ButtonPageViewModel), typeof(ButtonPage), MaterialIconKind.ButtonPointer),
                new(typeof(ButtonSpinnerPageViewModel), typeof(ButtonSpinnerPage), MaterialIconKind.ButtonCursor),
                new(typeof(HyperLinkButtonPageViewModel), typeof(HyperLinkButtonPage), MaterialIconKind.LinkVariant),
                new(typeof(DropDownButtonPageViewModel), typeof(DropDownButtonPage), MaterialIconKind.FormSelect),
                new(typeof(SplitButtonPageViewModel), typeof(SplitButtonPage), MaterialIconKind.FormDropdown),
                new(typeof(ToggleSplitButtonPageViewModel), typeof(ToggleSplitButtonPage), MaterialIconKind.FormDropdown)
            ]),

            new PagesGroup(nameof(MenuResources.Selection), MaterialIconKind.CheckboxMarkedOutline, [
                new(typeof(CheckBoxPageViewModel), typeof(CheckBoxPage), MaterialIconKind.CheckboxMultipleMarked),
                new(typeof(RadioButtonPageViewModel), typeof(RadioButtonPage), MaterialIconKind.RadioboxMarked),
                new(typeof(ToggleButtonPageViewModel), typeof(ToggleButtonPage), MaterialIconKind.ToggleSwitchVariant),
                new(typeof(ToggleSwitchPageViewModel), typeof(ToggleSwitchPage), MaterialIconKind.ToggleSwitch)
            ]),

            new PagesGroup(nameof(MenuResources.Inputs), MaterialIconKind.FormTextbox, [
                new(typeof(FieldsPageViewModel), typeof(FieldsPage), MaterialIconKind.FocusFieldHorizontal),
                new(typeof(MultiComboBoxPageViewModel), typeof(MultiComboBoxPage), MaterialIconKind.FormSelect),
                new(typeof(TagBoxPageViewModel), typeof(TagBoxPage), MaterialIconKind.TagMultiple),
                new(typeof(SliderPageViewModel), typeof(SliderPage), MaterialIconKind.TuneVariant),
                new(typeof(ColorViewPageViewModel), typeof(ColorViewPage), MaterialIconKind.Palette),
                new(typeof(CalendarPageViewModel), typeof(CalendarPage), MaterialIconKind.Calendar),
                new(typeof(ClockPageViewModel), typeof(ClockPage), MaterialIconKind.Clock),
                new(typeof(ClockSelectorPageViewModel), typeof(ClockSelectorPage), MaterialIconKind.ClockEdit),
                new(typeof(TimeViewPageViewModel), typeof(TimeViewPage), MaterialIconKind.TimerOutline)
            ]),

            new PagesGroup(nameof(MenuResources.Layout), MaterialIconKind.ViewDashboardOutline, [
                new(typeof(ExpanderPageViewModel), typeof(ExpanderPage), MaterialIconKind.ArrowExpand),
                new(typeof(SplitViewPageViewModel), typeof(SplitViewPage), MaterialIconKind.ViewSplitVertical),
                new(typeof(TabControlPageViewModel), typeof(TabControlPage), MaterialIconKind.Tab),
                new(typeof(GridSplitterPageViewModel), typeof(GridSplitterPage), MaterialIconKind.ArrowSplitVertical),
                new(typeof(HeaderedContentControlPageViewModel), typeof(HeaderedContentControlPage), MaterialIconKind.CardBulleted),
                new(typeof(FormPageViewModel), typeof(FormPage), MaterialIconKind.FormatLineStyle),
                new(typeof(BannerPageViewModel), typeof(BannerPage), MaterialIconKind.InformationBox),
                new(typeof(CardPageViewModel), typeof(CardPage), MaterialIconKind.CardOutline),
                new(typeof(BadgePageViewModel), typeof(BadgePage), MaterialIconKind.CheckboxBlankBadge),
                new(typeof(AvatarPageViewModel), typeof(AvatarPage), MaterialIconKind.AccountBox),
                new(typeof(CarouselPageViewModel), typeof(CarouselPage), MaterialIconKind.ViewCarousel)
            ]),

            new PagesGroup(nameof(MenuResources.DataAndLists), MaterialIconKind.Table, [
                new(typeof(DataGridPageViewModel), typeof(DataGridPage), MaterialIconKind.Table),
                new(typeof(ListBoxPageViewModel), typeof(ListBoxPage), MaterialIconKind.ListBox),
                new(typeof(TreeViewPageViewModel), typeof(TreeViewPage), MaterialIconKind.FileTree)
            ]),

            new PagesGroup(nameof(MenuResources.ShellNavigation), MaterialIconKind.BookOpenPageVariantOutline, [
                new(typeof(NavigationMenuPageViewModel), typeof(NavigationMenuPage), MaterialIconKind.Navigation),
                new(typeof(MenuPageViewModel), typeof(MenuPage), MaterialIconKind.Menu),
                new(typeof(DrawerPagePageViewModel), typeof(DrawerPagePage), MaterialIconKind.ViewSplitVertical),
                new(typeof(PaginationPageViewModel), typeof(PaginationPage), MaterialIconKind.PageLayoutBody),
                new(typeof(ContentPagePageViewModel), typeof(ContentPagePage), MaterialIconKind.FileDocumentOutline),
                new(typeof(TabbedPagePageViewModel), typeof(TabbedPagePage), MaterialIconKind.Tab),
                new(typeof(CarouselPagePageViewModel), typeof(CarouselPagePage), MaterialIconKind.BookOpenPageVariantOutline)
            ]),

            new PagesGroup(nameof(MenuResources.Feedback), MaterialIconKind.MessageAlertOutline, [
                new(typeof(DialogPageViewModel), typeof(DialogPage), MaterialIconKind.DockWindow),
                new(typeof(NotificationPageViewModel), typeof(NotificationPage), MaterialIconKind.MessageAlert),
                new(typeof(ProgressBarPageViewModel), typeof(ProgressBarPage), MaterialIconKind.ProgressCheck)
            ]),

            new PagesGroup(nameof(MenuResources.ShapesAndDrawing), MaterialIconKind.Shape, [
                new(typeof(PathIconPageViewModel), typeof(PathIconPage), MaterialIconKind.VectorSquare),
                new(typeof(ExtendedIconPageViewModel), typeof(ExtendedIconPage), MaterialIconKind.Shape),
                new(typeof(MaterialIconPageViewModel), typeof(MaterialIconPage), MaterialIconKind.AnimationOutline),
                new(typeof(AdornedContentControlPageViewModel), typeof(AdornedContentControlPage), MaterialIconKind.TextBoxOutline),
                new(typeof(RipplePageViewModel), typeof(RipplePage), MaterialIconKind.RadiusOutline),
                new(typeof(BorderPageViewModel), typeof(BorderPage), MaterialIconKind.CardOutline),
                new(typeof(EllipsePageViewModel), typeof(EllipsePage), MaterialIconKind.Circle)
            ])
        ];

    /// <summary>Number of top-level page entries before catalog groups (Home, Theme, Icons).</summary>
    public const int RootPageCount = 3;

    /// <summary>Gets the total number of registered showcase demo pages.</summary>
    public static int DemoPageCount => GetProviders().SelectMany(p => p.GetPageAssociations()).Count();

    /// <summary>Gets the number of component catalog menu groups.</summary>
    public static int CategoryGroupCount => GetProviders().Count(p => p is PagesGroup);

    /// <summary>Builds the full navigation menu, including the components catalog section header.</summary>
    public static IReadOnlyList<IMenuItemViewModel> CreateMenuItems(IServiceProvider services)
    {
        var items = new List<IMenuItemViewModel>();
        foreach (var provider in GetProviders())
        {
            items.Add(CreateMenuItem(provider, services));
            if (items.Count == RootPageCount)
                items.Add(MenuSectionViewModel.ComponentsCatalog);
        }

        return items;
    }

    public static IMenuItemViewModel CreateMenuItem(IPagesProvider pagesProvider, IServiceProvider services)
    {
        switch (pagesProvider)
        {
            case PageAssociation pageAssociation:
                return new LazyPageMenuItem(pageAssociation.ViewModelType, pageAssociation.Icon, services);

            case PagesGroup pagesGroup:
                var group = new PagesGroupViewModel(pagesGroup.ResourceKey, pagesGroup.Icon);
                group.AddPages(pagesGroup.Associations.Select(x => (x.ViewModelType, x.Icon)), services);
                return group;

            default:
                throw new ArgumentOutOfRangeException(nameof(pagesProvider), pagesProvider, null);
        }
    }

    internal interface IPagesProvider
    {
        IEnumerable<PageAssociation> GetPageAssociations();
    }

    internal sealed record PagesGroup(string? ResourceKey, MaterialIconKind Icon, IList<PageAssociation> Associations)
        : IPagesProvider
    {
        public IEnumerable<PageAssociation> GetPageAssociations() => Associations;
    }

    internal sealed record PageAssociation(Type ViewModelType, Type ViewType, MaterialIconKind Icon) : IPagesProvider
    {
        public IEnumerable<PageAssociation> GetPageAssociations() => [this];
    }
}
