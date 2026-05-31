// -----------------------------------------------------------------------
// <copyright file="PositionToDockConverterTests.cs" company="Stéphane ANDRE">
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

public class PositionToDockConverterTests
{
    [Theory]
    [InlineData(Position.Left, Dock.Left)]
    [InlineData(Position.Right, Dock.Right)]
    [InlineData(Position.Top, Dock.Top)]
    [InlineData(Position.Bottom, Dock.Bottom)]
    public void Convert_MapsPositionToDock(Position position, Dock expected)
    {
        var result = PositionToDockConverter.Default.Convert(position, typeof(Dock), null, CultureInfo.InvariantCulture);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(Dock.Left, Position.Left)]
    [InlineData(Dock.Right, Position.Right)]
    [InlineData(Dock.Top, Position.Top)]
    [InlineData(Dock.Bottom, Position.Bottom)]
    public void ConvertBack_MapsDockToPosition(Dock dock, Position expected)
    {
        var result = PositionToDockConverter.Default.ConvertBack(dock, typeof(Position), null, CultureInfo.InvariantCulture);

        result.Should().Be(expected);
    }

    [Fact]
    public void Convert_UnknownValueDefaultsToLeft()
    {
        var result = PositionToDockConverter.Default.Convert(null, typeof(Dock), null, CultureInfo.InvariantCulture);

        result.Should().Be(Dock.Left);
    }
}
