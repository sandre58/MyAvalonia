// -----------------------------------------------------------------------
// <copyright file="CardHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using FluentAssertions;
using Material.Icons;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class CardHeadlessTests
{
    [AvaloniaFact]
    public void HorizontalLayout_RendersTitleAndSubtitle()
    {
        var card = new Card
        {
            Layout = CardLayout.Horizontal,
            Leading = new MaterialIcon { Kind = MaterialIconKind.Home },
            Title = "Title",
            Subtitle = "Subtitle",
            Padding = new(16)
        };

        HeadlessControlHost.Show(card, new(320, 120));

        var titleBlock = card.GetVisualDescendants().OfType<TitleBlock>().FirstOrDefault();
        titleBlock.Should().NotBeNull();
        titleBlock.Title.Should().Be("Title");
        titleBlock.Subtitle.Should().Be("Subtitle");
    }

    [AvaloniaFact]
    public void VerticalLayout_SetsVerticalPseudoClass()
    {
        var card = new Card { Layout = CardLayout.Vertical };

        HeadlessControlHost.Show(card, new(240, 120));

        card.Classes.Should().Contain(":vertical");
        card.Classes.Should().NotContain(":horizontal");
    }

    [AvaloniaFact]
    public void VerticalLayout_CentersTitlePresenters()
    {
        var card = new Card
        {
            Layout = CardLayout.Vertical,
            Title = "Title",
            Subtitle = "Subtitle with enough text to wrap on narrow cards.",
            Padding = new(16)
        };

        HeadlessControlHost.Show(card, new(200, 160));

        var titlePresenters = card.GetVisualDescendants()
            .OfType<ContentPresenter>()
            .Where(p => p.Name is "PART_Title" or "PART_Subtitle")
            .ToList();

        titlePresenters.Should().HaveCount(2);
        titlePresenters.Should().OnlyContain(p => p.GetValue(TextBlock.TextAlignmentProperty) == TextAlignment.Center);
        titlePresenters.Should().OnlyContain(p => p.HorizontalAlignment == HorizontalAlignment.Stretch);

        var textBlocks = titlePresenters
            .Select(p => p.Child)
            .OfType<TextBlock>()
            .ToList();

        textBlocks.Should().HaveCount(2);
        textBlocks.Should().OnlyContain(tb => tb.TextAlignment == TextAlignment.Center);
        textBlocks.Should().OnlyContain(tb => tb.HorizontalAlignment == HorizontalAlignment.Stretch);
    }

    [AvaloniaFact]
    public void LeadingNone_HidesLeadingHost()
    {
        var card = new Card
        {
            LeadingPresentation = LeadingPresentation.None,
            Leading = new MaterialIcon { Kind = MaterialIconKind.EyeOffOutline },
            Title = "Hidden leading"
        };

        HeadlessControlHost.Show(card, new(320, 120));

        var leadingHost = card.GetVisualDescendants().OfType<Panel>().FirstOrDefault(b => b.Name == "PART_LeadingHost");
        leadingHost.Should().NotBeNull();
        leadingHost.IsVisible.Should().BeFalse();
    }

    [AvaloniaFact]
    public void PlainLeading_HidesLeadingBackground()
    {
        var card = new Card
        {
            LeadingPresentation = LeadingPresentation.Plain,
            Leading = new MaterialIcon { Kind = MaterialIconKind.Palette },
            Title = "Theme",
            Subtitle = "Open the theme page."
        };

        HeadlessControlHost.Show(card, new(320, 120));

        var leadingBackground = card.GetVisualDescendants().OfType<Border>().FirstOrDefault(b => b.Name == "PART_LeadingBackground");
        leadingBackground.Should().NotBeNull();
        leadingBackground.IsVisible.Should().BeFalse();
    }

    [AvaloniaFact]
    public void ContentOnly_SetsPseudoClassWhenOnlyContentIsSet()
    {
        var card = new Card { Content = "Widget body" };

        HeadlessControlHost.Show(card, new(240, 120));

        card.Classes.Should().Contain(":content-only");
    }

    [AvaloniaFact]
    public void ContentOnly_ClearsWhenTitleIsSet()
    {
        var card = new Card { Content = "Widget body", Title = "Title" };

        HeadlessControlHost.Show(card, new(240, 120));

        card.Classes.Should().NotContain(":content-only");
    }

    [AvaloniaFact]
    public void InteractiveTheme_RendersActionButton()
    {
        Application.Current!.TryGetResource("MyNet.Theme.Card.Interactive", null, out var theme);
        var card = new Card
        {
            Title = "Navigate",
            Command = new HeadlessCommand(),
            Theme = theme as ControlTheme
        };

        HeadlessControlHost.Show(card, new(240, 80));

        card.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_ActionButton").Should().NotBeNull();
    }

    [AvaloniaFact]
    public void Header_IsHiddenWhenNull()
    {
        var card = new Card { Title = "Test" };

        HeadlessControlHost.Show(card, new(240, 80));

        var header = card.GetVisualDescendants().OfType<Border>().FirstOrDefault(b => b.Name == "PART_Header");
        header.Should().NotBeNull();
        header.IsVisible.Should().BeFalse();
    }

    [AvaloniaFact]
    public void Actions_IsHiddenWhenNull()
    {
        var card = new Card { Title = "Test" };

        HeadlessControlHost.Show(card, new(240, 80));

        var actions = card.GetVisualDescendants().OfType<Border>().FirstOrDefault(b => b.Name == "PART_Actions");
        actions.Should().NotBeNull();
        actions.IsVisible.Should().BeFalse();
    }

    private sealed class HeadlessCommand : System.Windows.Input.ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) { }
    }
}
