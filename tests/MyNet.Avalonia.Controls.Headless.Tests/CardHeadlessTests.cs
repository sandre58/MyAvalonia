// -----------------------------------------------------------------------
// <copyright file="CardHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Styling;
using Avalonia.VisualTree;
using FluentAssertions;
using Material.Icons;
using MyNet.Avalonia.Controls;
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

        card.GetVisualDescendants().OfType<TextBlock>().Select(x => x.Text).Should().Contain(["Title", "Subtitle"]);
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
    public void VerticalLayout_HidesLeadingBackground()
    {
        var card = new Card
        {
            Layout = CardLayout.Vertical,
            Leading = new MaterialIcon { Kind = MaterialIconKind.Palette },
            Title = "Theme",
            Subtitle = "Open the theme page."
        };

        HeadlessControlHost.Show(card, new(320, 120));

        var leadingBackground = card.GetVisualDescendants().OfType<Border>().FirstOrDefault(b => b.Name == "PART_LeadingBackground");
        leadingBackground.Should().NotBeNull();
        leadingBackground!.IsVisible.Should().BeFalse();
        card.GetVisualDescendants().OfType<TextBlock>().Select(x => x.Text).Should().Contain(["Theme", "Open the theme page."]);
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
        header!.IsVisible.Should().BeFalse();
    }

    [AvaloniaFact]
    public void Footer_IsHiddenWhenNull()
    {
        var card = new Card { Title = "Test" };

        HeadlessControlHost.Show(card, new(240, 80));

        var footer = card.GetVisualDescendants().OfType<Border>().FirstOrDefault(b => b.Name == "PART_Footer");
        footer.Should().NotBeNull();
        footer!.IsVisible.Should().BeFalse();
    }

    private sealed class HeadlessCommand : System.Windows.Input.ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) { }
    }
}
