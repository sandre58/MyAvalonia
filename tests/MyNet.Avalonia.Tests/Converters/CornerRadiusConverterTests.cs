// -----------------------------------------------------------------------
// <copyright file="CornerRadiusConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class CornerRadiusConverterTests
{
    [Fact]
    public void FromDoubleAll_AppliesUniformRadius()
    {
        var result = CornerRadiusFromDoubleConverter.All.Convert(6.0, typeof(CornerRadius), null, CultureInfo.InvariantCulture);

        result.Should().Be(new CornerRadius(6));
    }

    [Fact]
    public void AdjustLeft_ClearsRightCorners()
    {
        var input = new CornerRadius(4, 8, 12, 16);

        var result = CornerRadiusAdjustConverter.Left.Convert(input, typeof(CornerRadius), null, CultureInfo.InvariantCulture);

        ((CornerRadius)result!).Should().Be(new CornerRadius(4, 0, 0, 16));
    }
}
