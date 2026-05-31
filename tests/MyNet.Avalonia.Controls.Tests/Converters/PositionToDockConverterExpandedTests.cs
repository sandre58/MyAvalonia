// -----------------------------------------------------------------------
// <copyright file="PositionToDockConverterExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Controls.Converters;
using MyNet.Avalonia.Controls.Enums;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Converters;

public class PositionToDockConverterExpandedTests
{
    [Fact]
    public void ConvertBack_UnknownValueDefaultsToLeft()
    {
        var result = PositionToDockConverter.Default.ConvertBack(999, typeof(Position), null, CultureInfo.InvariantCulture);

        result.Should().Be(Position.Left);
    }

    [Fact]
    public void ConvertBack_NullDefaultsToLeft()
    {
        var result = PositionToDockConverter.Default.ConvertBack(null, typeof(Position), null, CultureInfo.InvariantCulture);

        result.Should().Be(Position.Left);
    }

    [Fact]
    public void Default_IsSingleton()
    {
        PositionToDockConverter.Default.Should().BeSameAs(PositionToDockConverter.Default);
    }
}
