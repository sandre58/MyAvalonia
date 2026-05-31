// -----------------------------------------------------------------------
// <copyright file="IntToDecimalConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class IntToDecimalConverterTests
{
    [Fact]
    public void Convert_IntToDecimal()
    {
        IntToDecimalConverter.Default.Convert(7, typeof(decimal), null, CultureInfo.InvariantCulture).Should().Be(7m);
    }

    [Fact]
    public void ConvertBack_DecimalToInt()
    {
        IntToDecimalConverter.Default.ConvertBack(9m, typeof(int), null, CultureInfo.InvariantCulture).Should().Be(9);
    }

    [Fact]
    public void Convert_WrongType_ReturnsNull()
    {
        IntToDecimalConverter.Default.Convert("7", typeof(decimal), null, CultureInfo.InvariantCulture).Should().BeNull();
    }
}
