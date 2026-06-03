// -----------------------------------------------------------------------
// <copyright file="WindowAssistTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Theme.Controls.Assists;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Assists;

public class WindowAssistTests
{
    [Fact]
    public void CaptionButtonsWidth_ReflectsVisibleButtons() => WindowCaptionLayout.CalculateCaptionButtonsWidth(true, true, true, false)
        .Should().Be(3 * WindowLayoutMetrics.CaptionButtonWidth);

    [Fact]
    public void CaptionButtonsWidth_UpdatesWhenCloseButtonHidden() => WindowCaptionLayout.CalculateCaptionButtonsWidth(true, true, false, false)
        .Should().Be(2 * WindowLayoutMetrics.CaptionButtonWidth);

    [Fact]
    public void TitleBarContentInset_IsZeroByDefault() => WindowCaptionLayout.CalculateContentInset(false, false, 30)
        .Should().Be(new Thickness(0));

    [Fact]
    public void TitleBarContentInset_ReservesTopWhenSafeAreaEnabled() => WindowCaptionLayout.CalculateContentInset(true, true, 40)
        .Should().Be(new Thickness(0, 40, 0, 0));

    [Fact]
    public void TitleBarContentInset_IgnoresSafeAreaWhenContentNotExtended() => WindowCaptionLayout.CalculateContentInset(false, true, 30)
        .Should().Be(new Thickness(0));

    [Fact]
    public void TitleBarHeight_DefaultMatchesLayoutToken()
    {
        var element = new Border();
        WindowAssist.GetTitleBarHeight(element).Should().Be(WindowLayoutMetrics.TitleBarHeight);
    }
}
