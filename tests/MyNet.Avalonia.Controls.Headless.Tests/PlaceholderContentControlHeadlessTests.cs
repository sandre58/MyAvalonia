// -----------------------------------------------------------------------
// <copyright file="PlaceholderContentControlHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using FluentAssertions;
using Material.Icons;
using MyNet.Avalonia.Theme.Controls.Assists;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class PlaceholderContentControlHeadlessTests
{
    [AvaloniaFact]
    public void ApplyTemplate_UsesSinglePresenter()
    {
        var control = CreateControl(null, "Pick one");
        HeadlessControlHost.Show(control, new(240, 120));

        control.GetVisualDescendants().OfType<ContentPresenter>().Should().ContainSingle();
    }

    [AvaloniaFact]
    public void EmptyContent_ShowsPlaceholderState()
    {
        var control = CreateControl(null, "Pick one");
        HeadlessControlHost.Show(control, new(240, 120));

        control.IsPlaceholderVisible.Should().BeTrue();
        control.Classes.Should().Contain(":placeholder");
    }

    [AvaloniaFact]
    public void NonEmptyContent_ShowsContentState()
    {
        var control = CreateControl("Selected", "Pick one");
        HeadlessControlHost.Show(control, new(240, 120));

        control.IsPlaceholderVisible.Should().BeFalse();
        control.Classes.Should().NotContain(":placeholder");
    }

    [AvaloniaFact]
    public void PlaceholderActiveTrue_ForcesPlaceholderWithNonEmptyContent()
    {
        var control = CreateControl(new ScrollViewer(), "No results");
        control.PlaceholderActive = true;
        HeadlessControlHost.Show(control, new(240, 120));

        control.IsPlaceholderVisible.Should().BeTrue();
        control.Classes.Should().Contain(":placeholder");
    }

    [AvaloniaFact]
    public void PlaceholderActiveFalse_ForcesContentWithEmptyContent()
    {
        var control = CreateControl(null, "Pick one");
        control.PlaceholderActive = false;
        HeadlessControlHost.Show(control, new(240, 120));

        control.IsPlaceholderVisible.Should().BeFalse();
        control.Classes.Should().NotContain(":placeholder");
    }

    [AvaloniaFact]
    public void VariantWatermark_WithIconKind_ShowsMaterialIcon()
    {
        var control = CreateControl(null, "Pick one");
        control.Classes.Add("variant-watermark");
        control.Classes.Add("size-sm");
        IconAssist.SetIcon(control, MaterialIconKind.FileSearchOutline);
        HeadlessControlHost.Show(control, new(240, 120));

        control.IsPlaceholderVisible.Should().BeTrue();
        control.GetVisualDescendants().OfType<MaterialIcon>().Should().ContainSingle();
    }

    [AvaloniaFact]
    public void VariantWatermark_IconVisibleAfterPlaceholderToggle()
    {
        var control = CreateControl(new ScrollViewer(), "No results");
        control.Classes.Add("variant-watermark");
        control.Classes.Add("size-sm");
        IconAssist.SetIcon(control, MaterialIconKind.FileSearchOutline);
        HeadlessControlHost.Show(control, new(240, 120));

        control.PlaceholderActive = true;
        control.GetVisualDescendants().OfType<MaterialIcon>().Should().ContainSingle();

        control.PlaceholderActive = false;
        control.GetVisualDescendants().OfType<MaterialIcon>().Should().BeEmpty();

        control.PlaceholderActive = true;
        control.GetVisualDescendants().OfType<MaterialIcon>().Should().ContainSingle();
    }

    [AvaloniaFact]
    public void PlaceholderMinHeight_AppliesOnlyInPlaceholderState()
    {
        var control = CreateControl(new ScrollViewer(), "No results");
        control.PlaceholderMinHeight = 96;
        HeadlessControlHost.Show(control, new(240, 120));

        var presenter = HeadlessControlHost.FindByName<ContentPresenter>(control, PlaceholderContentControl.PartPresenter);
        presenter.Should().NotBeNull();
        presenter!.MinHeight.Should().Be(0);

        control.PlaceholderActive = true;
        presenter.MinHeight.Should().Be(96);

        control.PlaceholderActive = false;
        presenter.MinHeight.Should().Be(0);
    }

    private static PlaceholderContentControl CreateControl(object? content, object placeholderText) =>
        new()
        {
            Content = content,
            PlaceholderText = placeholderText,
            Width = 240,
            Height = 120,
        };
}
