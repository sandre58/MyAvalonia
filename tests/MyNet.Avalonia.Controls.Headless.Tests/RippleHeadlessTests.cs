// -----------------------------------------------------------------------
// <copyright file="RippleHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Rendering.Composition;
using Avalonia.VisualTree;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class RippleHeadlessTests
{
    [AvaloniaFact]
    public void Enter_OnSingleRippleHost_StartsRipple()
    {
        var ripple = CreateRipple();
        var host = CreateFocusableHost(ripple);

        HeadlessControlHost.Show(host, new(120, 48));
        host.Focus();

        HeadlessControlHost.KeyDown(host, Key.Enter);

        GetRippleVisualChildCount(ripple).Should().Be(1);
    }

    [AvaloniaFact]
    public void Enter_OnCompositeWithMultipleRipples_StartsNoRipple()
    {
        var ripples = Enumerable.Range(0, 3).Select(_ => CreateRipple()).ToArray();
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Children = { ripples[0], ripples[1], ripples[2] }
        };
        var host = CreateFocusableHost(panel);

        HeadlessControlHost.Show(host, new(240, 48));
        host.Focus();

        HeadlessControlHost.KeyDown(host, Key.Enter);

        ripples.Should().OnlyContain(r => GetRippleVisualChildCount(r) == 0);
    }

    [AvaloniaFact]
    public void Enter_OnRating_StartsNoRipple()
    {
        var rating = new Rating { MaxRating = 5, Value = 2 };

        HeadlessControlHost.Show(rating, new(240, 48));
        rating.Focus();

        HeadlessControlHost.KeyDown(rating, Key.Enter);

        rating.GetVisualDescendants()
            .OfType<Ripple>()
            .Should()
            .OnlyContain(r => GetRippleVisualChildCount(r) == 0);
    }

    private static Ripple CreateRipple() =>
        new()
        {
            Width = 40,
            Height = 40,
            RippleFill = Brushes.Red,
            Content = new Border { Width = 40, Height = 40 }
        };

    private static Border CreateFocusableHost(Control child) =>
        new()
        {
            Focusable = true,
            Width = 240,
            Height = 48,
            Child = child
        };

    private static int GetRippleVisualChildCount(Ripple ripple)
    {
        var visual = ElementComposition.GetElementChildVisual(ripple);
        return visual is CompositionContainerVisual container ? container.Children.Count : 0;
    }
}
