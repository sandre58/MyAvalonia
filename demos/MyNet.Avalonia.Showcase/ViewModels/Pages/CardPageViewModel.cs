// -----------------------------------------------------------------------
// <copyright file="CardPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Options;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class CardPageViewModel(ICommandFactory commands, INotificationPublisher notificationPublisher) : ShowcaseViewModel(nameof(Card), commands, [
    new ControlThemeBuilder()
        .WithContent(Card.TitleProperty, ContentProviderType.Text)
        .WithIcon(Card.LeadingProperty, true)
        .AddAllVariants()
        .AddVariants(CssClass.ShadowSurface, CssClass.ShadowControl)
        .AddVariant(new("is-hover"))
        .AddAllRoles()
        .AddDefaultSizes()
        .AddEnumProperty<CardLayout, ListBoxEditor>(Card.CardLayoutProperty, configure: x => x.DisplayName(nameof(SettingsResources.Layout)))
        .AddProperty(Card.ShowTrailingProperty, configure: x => x.DisplayName(nameof(CardPageResources.ShowTrailing)))
        .AddProperty(Card.SubtitleProperty, configure: x => x.DisplayName(nameof(CardPageResources.Subtitle)).Of<TextBoxEditor>(y => y.WithValue("Subtitle with a very long text must be wrapped").WithRandomizeText(RandomizeText.Sentence)))
        .AddProperty(Card.HeaderProperty, configure: x => x.DisplayName(nameof(CardPageResources.Header)).Of<TextBoxEditor>())
        .AddProperty(Card.FooterProperty, configure: x => x.DisplayName(nameof(CardPageResources.Footer)).Of<TextBoxEditor>()),

    new ControlThemeBuilder("Interactive")
        .WithContent(Card.TitleProperty, ContentProviderType.Text)
        .WithIcon(Card.LeadingProperty, true)
        .AddAllVariants()
        .AddAllRoles()
        .AddDefaultSizes()
        .AddEnumProperty<CardLayout, ListBoxEditor>(Card.CardLayoutProperty, configure: x => x.DisplayName(nameof(SettingsResources.Layout)))
        .AddProperty(Card.ShowTrailingProperty, true, configure: x => x.DisplayName(nameof(CardPageResources.ShowTrailing)))
        .AddProperty(Card.SubtitleProperty, configure: x => x.DisplayName(nameof(CardPageResources.Subtitle)).Of<TextBoxEditor>(y => y.WithValue("Subtitle with a very long text must be wrapped").WithRandomizeText(RandomizeText.Sentence)))
        .AddProperty(Card.HeaderProperty, configure: x => x.DisplayName(nameof(CardPageResources.Header)).Of<TextBoxEditor>())
        .AddProperty(Card.FooterProperty, configure: x => x.DisplayName(nameof(CardPageResources.Footer)).Of<TextBoxEditor>()),
])
{
    public ICommand SampleCommand { get; } = commands.Create(() => notificationPublisher.Publish(new MessageNotification(CardPageResources.ClickTitle, CardPageResources.ClickMessage, NotificationSeverity.Success)));

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.CardOutline;
}
