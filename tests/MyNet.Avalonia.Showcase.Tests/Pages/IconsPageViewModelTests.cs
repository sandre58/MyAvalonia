// -----------------------------------------------------------------------
// <copyright file="IconsPageViewModelTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using MyNet.Avalonia.Controls.Icons;
using MyNet.Avalonia.Showcase.Tests.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Pages;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.Pages;

public class IconsPageViewModelTests
{
    [Fact]
    public void Items_ShowsSinglePage()
    {
        var viewModel = new IconsPageViewModel(new TestCommandFactory());

        viewModel.Icons.Paging!.TotalItems.Should().Be(MaterialIconCatalog.Groups.Count);
        viewModel.Icons.Paging.PageSize.Should().Be(100);
        viewModel.Icons.Items.Count.Should().BeLessThanOrEqualTo(100);
        viewModel.Icons.Items.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Search_ReducesPagedItems()
    {
        var viewModel = new IconsPageViewModel(new TestCommandFactory());
        viewModel.SearchText = "Account";

        viewModel.Icons.Paging!.TotalItems.Should().BeLessThan(MaterialIconCatalog.Groups.Count);
        viewModel.Icons.Items.Count.Should().BeLessThanOrEqualTo(viewModel.Icons.Paging.PageSize);
        viewModel.Icons.Items.Should().OnlyContain(icon =>
            icon.DisplayName.Contains("Account", StringComparison.OrdinalIgnoreCase)
            || icon.Name.Contains("Account", StringComparison.OrdinalIgnoreCase)
            || icon.Aliases.Any(alias => alias.Contains("Account", StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public void MoveToPage_UpdatesItems()
    {
        var viewModel = new IconsPageViewModel(new TestCommandFactory());
        var firstPage = viewModel.Icons.Items.Select(x => x.Name).ToList();

        viewModel.Icons.Paging!.MoveToPage(2);

        viewModel.Icons.Paging.CurrentPage.Should().Be(2);
        viewModel.Icons.Items.Select(x => x.Name).Should().NotEqual(firstPage);
    }
}
