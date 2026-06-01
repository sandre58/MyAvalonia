// -----------------------------------------------------------------------
// <copyright file="PaginationHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Headless.XUnit;
using FluentAssertions;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class PaginationHeadlessTests
{
    [AvaloniaFact]
    public void ApplyTemplate_ComputesPageCountFromTotalCountAndPageSize()
    {
        var pagination = new Pagination
        {
            TotalCount = 95,
            PageSize = 10
        };

        HeadlessControlHost.Show(pagination, new(640, 48));

        pagination.PageCount.Should().Be(10);
    }

    [AvaloniaFact]
    public void NextButton_IncrementsCurrentPage()
    {
        var pagination = new Pagination
        {
            TotalCount = 100,
            PageSize = 10,
            CurrentPage = 1
        };

        HeadlessControlHost.Show(pagination, new(640, 48));

        var nextButton = HeadlessControlHost.FindByName<PaginationButton>(pagination, Pagination.PartNextButton);
        nextButton.Should().NotBeNull();

        HeadlessControlHost.Click(nextButton);

        pagination.CurrentPage.Should().Be(2);
    }

    [AvaloniaFact]
    public void PreviousButton_DecrementsCurrentPage()
    {
        var pagination = new Pagination
        {
            TotalCount = 100,
            PageSize = 10
        };

        HeadlessControlHost.Show(pagination, new(640, 48));
        pagination.CurrentPage = 3;

        var previousButton = HeadlessControlHost.FindByName<PaginationButton>(pagination, Pagination.PartPreviousButton);
        previousButton.Should().NotBeNull();

        HeadlessControlHost.Click(previousButton);

        pagination.CurrentPage.Should().Be(2);
    }

    [AvaloniaFact]
    public void TotalCountChange_RecalculatesPageCountAndClampsCurrentPage()
    {
        var pagination = new Pagination
        {
            TotalCount = 100,
            PageSize = 10
        };

        HeadlessControlHost.Show(pagination, new(640, 48));
        pagination.CurrentPage = 10;
        pagination.TotalCount = 25;

        pagination.PageCount.Should().Be(3);
        pagination.CurrentPage.Should().Be(3);
    }
}
